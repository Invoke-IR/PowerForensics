using System;
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

        #endregion Properties

        #region Constructors

        private DataRun()
        {

        }

        private DataRun(byte[] bytes, int offset, int lengthByteCount, int offsetByteCount, DataRun previousDR)
        {
            if (offsetByteCount == 0)
            {
                Sparse = true;
            }

            byte[] DataRunLengthBytes = new byte[8];
            Array.Copy(bytes, offset + 1, DataRunLengthBytes, 0, lengthByteCount);
            ClusterLength = BitConverter.ToInt64(DataRunLengthBytes, 0);

            byte[] offsetBytes = new byte[0x08];
            int arrayOffset = 0x08 - offsetByteCount;
            Array.Copy(bytes, offset + 0x01 + lengthByteCount, offsetBytes, arrayOffset, offsetByteCount);
            long relativeOffset = BitConverter.ToInt64(offsetBytes, 0x00) >> arrayOffset * 0x08;
            StartCluster = previousDR.StartCluster + relativeOffset;
        }

        #endregion Constructors

        #region InstanceMethods

        public static DataRun[] GetInstances(byte[] bytes)
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

                dr = Get(bytes, i, DataRunLengthByteCount, DataRunOffsetByteCount, dr);
                datarunList.Add(dr);
                i += (1 + DataRunLengthByteCount + DataRunOffsetByteCount);
            }

            return datarunList.ToArray();
        }

        public static DataRun Get(byte[] bytes, int offset, int lengthByteCount, int offsetByteCount, DataRun previousDR)
        {
            return new DataRun(bytes, offset, lengthByteCount, offsetByteCount, previousDR);
        }

        public byte[] GetBytes(string volume)
        {
            VolumeBootRecord vbr = VolumeBootRecord.Get(volume);
            return Helper.readDrive(volume, (ulong)this.StartCluster * vbr.BytesPerCluster, (ulong)this.ClusterLength * vbr.BytesPerCluster);
        }

        #endregion InstanceMethods
    }

    #endregion DataRunClass
}
