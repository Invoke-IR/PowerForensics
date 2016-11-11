using System;
using System.Collections.Generic;
using PowerForensics.Generic;

namespace PowerForensics.Ntfs
{
    #region DataRunClass

    public class DataRun
    {
        #region Properties

        private readonly string Volume;
        public readonly long StartCluster;
        public readonly long ClusterLength;
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

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="lengthByteCount"></param>
        /// <param name="offsetByteCount"></param>
        /// <param name="previousDR"></param>
        /// <returns></returns>
        private static DataRun Get(byte[] bytes, int offset, int lengthByteCount, int offsetByteCount, DataRun previousDR, string volume)
        {
            return new DataRun(bytes, offset, lengthByteCount, offsetByteCount, previousDR, volume);
        }

        #endregion StaticMethods

        #region InstanceMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            VolumeBootRecord vbr = VolumeBootRecord.Get(this.Volume);
            return Helper.readDrive(this.Volume, this.StartCluster * vbr.BytesPerCluster, this.ClusterLength * vbr.BytesPerCluster);
        }

        #endregion InstanceMethods
    }

    #endregion DataRunClass
}
