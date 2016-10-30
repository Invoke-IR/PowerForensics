using System;
using System.IO;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{   
    #region NonResidentClass

    public class NonResident : FileRecordAttribute
    {
        #region Properties

        private string Volume;
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

        #region GetBytes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="VBR"></param>
        /// <returns></returns>
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

        #endregion GetBytes

        #region GetSlack

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
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

        #endregion GetSlack

        #endregion InstanceMethods
    }

    #endregion NonResidentClass
}
