using System;
using System.IO;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{   
    #region NonResidentClass

    public class NonResident : FileRecordAttribute
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
            DataRun = Ntfs.DataRun.GetInstances(bytes);
        }

        #endregion Constructors

        #region InstanceMethods

        public byte[] GetBytes(string volume)
        {
            byte[] fileBytes = new byte[this.RealSize];

            int offset = 0;

            Helper.getVolumeName(ref volume);
            
            using (FileStream streamToRead = Helper.getFileStream(volume))
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
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, startOffset, count);

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
            Helper.getVolumeName(ref volume);

            using(FileStream streamToRead = Helper.getFileStream(volume))
            {
                if (this.DataRun.Length != 0)
                {
                    VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);
                    ulong slackSize = this.AllocatedSize - this.RealSize;
                    if ((slackSize > 0) && (slackSize <= VBR.BytesPerCluster))
                    {
                        DataRun dr = this.DataRun[this.DataRun.Length - 1];
                        ulong lastCluster = (ulong)dr.StartCluster + (ulong)dr.ClusterLength - 1;
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, VBR.BytesPerCluster * lastCluster, VBR.BytesPerCluster);
                        byte[] slackBytes = new byte[slackSize];
                        Array.Copy(dataRunBytes, (int)VBR.BytesPerCluster - ((int)this.AllocatedSize - (int)this.RealSize), slackBytes, 0x00, slackBytes.Length);
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

            Helper.getVolumeName(ref volume);

            using (FileStream streamToRead = Helper.getFileStream(volume))
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
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, startOffset, count);

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
