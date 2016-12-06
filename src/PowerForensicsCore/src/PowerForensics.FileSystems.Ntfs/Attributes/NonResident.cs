using System;
using System.IO;
using System.Collections.Generic;
using PowerForensics.FileSystems;

namespace PowerForensics.FileSystems.Ntfs
{   
    /// <summary>
    /// 
    /// </summary>
    public class NonResident : FileRecordAttribute
    {
        #region Properties

        private string Volume;

        internal CommonHeader commonHeader;             // Common Header Object

        internal ulong StartVCN;                        // Starting VCN

        internal ulong LastVCN;                         // Last VCN

        internal ushort DataRunOffset;                  // Offset to the Data Runs

        internal ushort CompUnitSize;                   // Compression unit size

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong AllocatedSize;            // Allocated size of the attribute

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RealSize;                 // Real size of the attribute

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong InitializedSize;          // Initialized data size of the stream 

        /// <summary>
        /// 
        /// </summary>
        public readonly DataRun[] DataRun;

        #endregion Properties

        #region Constructors

        internal NonResident(NonResidentHeader header, byte[] bytes, int offset, string attrName, string volume)
        {
            Volume = volume;

            // Attr Object
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // NonResident Attribute
            commonHeader = header.commonHeader;
            StartVCN = header.StartVCN;
            LastVCN = header.LastVCN;
            DataRunOffset = header.DataRunOffset;
            CompUnitSize = header.CompUnitSize;
            AllocatedSize = header.AllocatedSize;
            RealSize = header.RealSize;
            InitializedSize = header.InitializedSize;
            DataRun = Ntfs.DataRun.GetInstances(bytes, offset, volume);
        }

        #endregion Constructors

        #region InstanceMethods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] fileBytes = new byte[this.RealSize];

            int offset = 0;

            Helper.getVolumeName(ref this.Volume);

            using (FileStream streamToRead = Helper.getFileStream(this.Volume))
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
                        long startOffset = VBR.BytesPerCluster * dr.StartCluster;
                        long count = VBR.BytesPerCluster * dr.ClusterLength;
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, startOffset, count);

                        if ((offset + count) <= fileBytes.Length)
                        {
                            // Save dataRunBytes to fileBytes
                            Array.Copy(dataRunBytes, 0x00, fileBytes, offset, dataRunBytes.Length);

                            // Increment Offset Value
                            offset += dataRunBytes.Length;
                        }
                        else
                        {
                            Array.Copy(dataRunBytes, 0x00, fileBytes, offset, (fileBytes.Length - offset));
                            break;
                        }
                    }
                }
                return fileBytes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytesTest()
        {
            Helper.getVolumeName(ref this.Volume);

            List<byte> byteList = new List<byte>();
            
            using (FileStream streamToRead = Helper.getFileStream(this.Volume))
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
                        long startOffset = VBR.BytesPerCluster * dr.StartCluster;
                        Console.WriteLine(this.Volume);
                        long count = VBR.BytesPerCluster * dr.ClusterLength;
                        byteList.AddRange(Helper.readDrive(streamToRead, startOffset, count));
                    }
                }

                return Helper.GetSubArray(byteList.ToArray(), 0, (long)this.RealSize);
            }
        }

        internal byte[] GetBytes(VolumeBootRecord VBR)
        {
            byte[] fileBytes = new byte[this.RealSize];

            int offset = 0;

            Helper.getVolumeName(ref this.Volume);

            using (FileStream streamToRead = Helper.getFileStream(this.Volume))
            {
                foreach (DataRun dr in this.DataRun)
                {
                    if (dr.Sparse)
                    {
                        // Figure out how to add Sparse Bytes
                    }
                    else
                    {
                        long startOffset = VBR.BytesPerCluster * dr.StartCluster;
                        long count = VBR.BytesPerCluster * dr.ClusterLength;
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, startOffset, count);

                        if ((offset + count) <= fileBytes.Length)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetSlack()
        {
            Helper.getVolumeName(ref this.Volume);

            using(FileStream streamToRead = Helper.getFileStream(this.Volume))
            {
                if (this.DataRun.Length != 0)
                {
                    VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);
                    ulong slackSize = this.AllocatedSize - this.RealSize;
                    if ((slackSize > 0) && (slackSize <= (ulong)VBR.BytesPerCluster))
                    {
                        DataRun dr = this.DataRun[this.DataRun.Length - 1];
                        long lastCluster = dr.StartCluster + dr.ClusterLength - 1;
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, VBR.BytesPerCluster * lastCluster, VBR.BytesPerCluster);
                        byte[] slackBytes = new byte[slackSize];
                        Array.Copy(dataRunBytes, VBR.BytesPerCluster - ((int)this.AllocatedSize - (int)this.RealSize), slackBytes, 0x00, slackBytes.Length);
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

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataRun
    {
        #region Properties

        private readonly string Volume;

        /// <summary>
        /// 
        /// </summary>
        public readonly long StartCluster;

        /// <summary>
        /// 
        /// </summary>
        public readonly long ClusterLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Sparse;

        #endregion Properties

        #region Constructors

        private DataRun()
        {

        }

        private DataRun(byte[] bytes, int offset, int lengthByteCount, int offsetByteCount, DataRun previousDR, string volume)
        {
            Volume = volume;

            if (offsetByteCount == 0)
            {
                Sparse = true;
            }

            if (!((lengthByteCount > 8) || (offsetByteCount > 8)))
            {
                byte[] DataRunLengthBytes = new byte[0x08];
                Array.Copy(bytes, offset + 1, DataRunLengthBytes, 0x00, lengthByteCount);
                ClusterLength = BitConverter.ToInt64(DataRunLengthBytes, 0x00);

                byte[] offsetBytes = new byte[0x08];
                int arrayOffset = 0x08 - offsetByteCount;
                Array.Copy(bytes, offset + 0x01 + lengthByteCount, offsetBytes, arrayOffset, offsetByteCount);
                long relativeOffset = BitConverter.ToInt64(offsetBytes, 0x00) >> arrayOffset * 0x08;
                StartCluster = previousDR.StartCluster + relativeOffset;
            }
        }

        #endregion Constructors

        #region Static Methods

        internal static DataRun[] GetInstances(byte[] bytes, string volume)
        {
            List<DataRun> datarunList = new List<DataRun>();
            int i = 0;
            DataRun dr = new DataRun();

            while (i < bytes.Length - 1)
            {
                int DataRunLengthByteCount = bytes[i] & 0x0F;
                int DataRunOffsetByteCount = ((bytes[i] & 0xF0) >> 4);

                if (DataRunLengthByteCount == 0)
                {
                    break;
                }
                else if ((i + DataRunLengthByteCount + DataRunOffsetByteCount + 1) > bytes.Length)
                {
                    break;
                }

                dr = Get(bytes, i, DataRunLengthByteCount, DataRunOffsetByteCount, dr, volume);
                datarunList.Add(dr);
                i += (1 + DataRunLengthByteCount + DataRunOffsetByteCount);
            }

            return datarunList.ToArray();
        }

        internal static DataRun[] GetInstances(byte[] bytes, int offset, string volume)
        {
            List<DataRun> datarunList = new List<DataRun>();

            int i = offset;
            DataRun dr = new DataRun();

            while (i < bytes.Length - 1)
            {
                int DataRunLengthByteCount = bytes[i] & 0x0F;
                int DataRunOffsetByteCount = ((bytes[i] & 0xF0) >> 4);

                if (DataRunLengthByteCount == 0)
                {
                    break;
                }
                else if ((i + DataRunLengthByteCount + DataRunOffsetByteCount + 1) > bytes.Length)
                {
                    break;
                }

                dr = Get(bytes, i, DataRunLengthByteCount, DataRunOffsetByteCount, dr, volume);
                datarunList.Add(dr);
                i += (1 + DataRunLengthByteCount + DataRunOffsetByteCount);
            }

            return datarunList.ToArray();
        }

        private static DataRun Get(byte[] bytes, int offset, int lengthByteCount, int offsetByteCount, DataRun previousDR, string volume)
        {
            return new DataRun(bytes, offset, lengthByteCount, offsetByteCount, previousDR, volume);
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            VolumeBootRecord vbr = VolumeBootRecord.Get(this.Volume);
            return Helper.readDrive(this.Volume, this.StartCluster * vbr.BytesPerCluster, this.ClusterLength * vbr.BytesPerCluster);
        }

        #endregion Instance Methods
    }
}