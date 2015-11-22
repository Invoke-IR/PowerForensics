using System;
using System.IO;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    #region DataRunClass

    public class DataRun
    {
        #region Properties

        public readonly long StartCluster;
        public readonly long ClusterLength;
        public readonly bool Sparse;
        public readonly bool Compressed;

        #endregion Properties

        #region Constructors

        internal DataRun(byte[] bytes, int lengthByteCount, int offsetByteCount, ref long previousStartCluster)
        {
            if (offsetByteCount == 0)
            {
                Sparse = true;
            }

            byte[] DataRunLengthBytes = new byte[8];
            Array.Copy(bytes, 0, DataRunLengthBytes, 0, lengthByteCount);
            long DataRunLength = BitConverter.ToInt64(DataRunLengthBytes, 0);

            byte[] DataRunOffsetBytes = new byte[8];
            Array.Copy(bytes, lengthByteCount, DataRunOffsetBytes, 0, offsetByteCount);

            long DataRunOffset = BitConverter.ToInt64(DataRunOffsetBytes, 0);

            if ((DataRunOffset & (1 << ((offsetByteCount * 8) - 1))) != 0)
            {
                DataRunOffset = (DataRunOffset | (~0) << (offsetByteCount * 8));
            }

            previousStartCluster += DataRunOffset;
            StartCluster = previousStartCluster;
            ClusterLength = DataRunLength;
        }

        #endregion Constructors
    }

    #endregion DataRunClass

    #region NonResidentClass

    public class NonResident : Attr
    {
        #region Properties

        internal CommonHeader commonHeader;	            // Common Header Object
        internal ulong StartVCN;		                // Starting VCN
        internal ulong LastVCN;		                    // Last VCN
        internal ushort DataRunOffset;	                // Offset to the Data Runs
        internal ushort CompUnitSize;	                // Compression unit size
        public readonly ulong AllocatedSize;            // Allocated size of the attribute
        public readonly ulong RealSize;                 // Real size of the attribute
        public readonly ulong InitializedSize;          // Initialized data size of the stream 
        public readonly DataRun[] DataRun;

        #endregion Properties

        #region Constructors

        internal NonResident(NonResidentHeader header, byte[] bytes, string attrName)
        {
            // Attr Object
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            // NonResident Attribute
            commonHeader = header.commonHeader;
            StartVCN = header.StartVCN;
            LastVCN = header.LastVCN;
            DataRunOffset = header.DataRunOffset;
            CompUnitSize = header.CompUnitSize;
            AllocatedSize = header.AllocatedSize;
            RealSize = header.RealSize;
            InitializedSize = header.InitializedSize;
            
            int DataRunLengthByteCount = bytes[0] & 0x0F;
            int DataRunOffsetByteCount = ((bytes[0] & 0xF0) >> 4);

            if (DataRunLengthByteCount != 0)
            {
                // DataRun
                int offset = 0;
                long startCluster = 0;

                List<DataRun> dataRunList = new List<DataRun>();
                do
                {
                    if ((offset + DataRunLengthByteCount + DataRunOffsetByteCount + 1) > bytes.Length)
                    {
                        break;
                    }

                    // Instantiate a DataRun object
                    DataRun dataRun = new DataRun(Util.GetSubArray(bytes, (uint)(offset + 1), (uint)DataRunLengthByteCount + (uint)DataRunOffsetByteCount), DataRunLengthByteCount, DataRunOffsetByteCount, ref startCluster);

                    // Add DataRun Object to dataRunList
                    dataRunList.Add(dataRun);

                    // Increment offset
                    offset = offset + 1 + DataRunLengthByteCount + DataRunOffsetByteCount;

                    if (offset <= (bytes.Length - 1))
                    {
                        DataRunLengthByteCount = bytes[offset] & 0x0F;
                        DataRunOffsetByteCount = ((bytes[offset] & 0xF0) >> 4);    
                    }
                    else
                    {
                        break;
                    }
                    
                } while (((offset + DataRunLengthByteCount + DataRunOffsetByteCount + 1) < bytes.Length) && (DataRunLengthByteCount != 0));
                
                DataRun = dataRunList.ToArray();
            }
        }

        #endregion Constructors

        #region InstanceMethods

        public byte[] GetBytes(string volume)
        {
            byte[] fileBytes = new byte[this.RealSize];

            int offset = 0;

            Util.getVolumeName(ref volume);
            IntPtr hVolume = Util.getHandle(volume);

            using (FileStream streamToRead = Util.getFileStream(hVolume))
            {
                VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

                foreach (DataRun dr in this.DataRun)
                {
                    if (dr.Sparse)
                    {
                        // Figure out how to add Sparse Bytes
                    }
                    else
                    {
                        ulong startOffset = (ulong)VBR.BytesPerCluster * (ulong)dr.StartCluster;
                        ulong count = (ulong)VBR.BytesPerCluster * (ulong)dr.ClusterLength;
                        byte[] dataRunBytes = Util.readDrive(streamToRead, startOffset, count);

                        if (((ulong)offset + count) <= (ulong)fileBytes.Length)
                        {
                            // Save dataRunBytes to fileBytes
                            Array.Copy(dataRunBytes, 0, fileBytes, offset, dataRunBytes.Length);

                            // Increment Offset Value
                            offset += dataRunBytes.Length;
                        }
                        else
                        {
                            Array.Copy(dataRunBytes, 0, fileBytes, offset, (fileBytes.Length - offset));
                            break;
                        }
                    }
                }
                return fileBytes;
            }
        }

        public byte[] GetSlack(string volume)
        {
            Util.getVolumeName(ref volume);
            IntPtr hVolume = Util.getHandle(volume);

            using(FileStream streamToRead = Util.getFileStream(hVolume))
            {
                if (this.DataRun != null)
                {
                    VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);
                    ulong slackSize = this.AllocatedSize - this.RealSize;
                    if (slackSize <= VBR.BytesPerCluster)
                    {
                        DataRun dr = this.DataRun[this.DataRun.Length - 1];
                        ulong lastCluster = (ulong)dr.StartCluster + (ulong)dr.ClusterLength - 1;
                        byte[] dataRunBytes = Util.readDrive(streamToRead, VBR.BytesPerCluster * lastCluster, VBR.BytesPerCluster);
                        byte[] slackBytes = new byte[slackSize];
                        Array.Copy(dataRunBytes, (long)VBR.BytesPerCluster - ((long)this.AllocatedSize - (long)this.RealSize), slackBytes, 0x00, slackBytes.Length);
                        return slackBytes;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        internal byte[] GetBytes(string volume, VolumeBootRecord VBR)
        {
            byte[] fileBytes = new byte[this.RealSize];

            int offset = 0;

            Util.getVolumeName(ref volume);
            IntPtr hVolume = Util.getHandle(volume);

            using (FileStream streamToRead = Util.getFileStream(hVolume))
            {
                foreach (DataRun dr in this.DataRun)
                {
                    if (dr.Sparse)
                    {
                        // Figure out how to add Sparse Bytes
                    }
                    else
                    {
                        ulong startOffset = (ulong)VBR.BytesPerCluster * (ulong)dr.StartCluster;
                        ulong count = (ulong)VBR.BytesPerCluster * (ulong)dr.ClusterLength;
                        byte[] dataRunBytes = Util.readDrive(streamToRead, startOffset, count);

                        if (((ulong)offset + count) <= (ulong)fileBytes.Length)
                        {
                            // Save dataRunBytes to fileBytes
                            Array.Copy(dataRunBytes, 0, fileBytes, offset, dataRunBytes.Length);

                            // Increment Offset Value
                            offset += dataRunBytes.Length;
                        }
                        else
                        {
                            Array.Copy(dataRunBytes, 0, fileBytes, offset, (fileBytes.Length - offset));
                            break;
                        }
                    }
                }
                return fileBytes;
            }
        }

        #endregion InstanceMethods
    }

    #endregion NonResidentClass
}
