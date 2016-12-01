using System;
using System.Collections.Generic;
using System.IO;
using PowerForensics.Generic;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class MasterFileTable
    {
        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamToRead"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        internal static FileRecord GetRecord(FileStream streamToRead, string volume)
        {
            // Instantiate VolumeBootRecord object
            NtfsVolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead) as NtfsVolumeBootRecord;

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (VBR.BytesPerCluster * VBR.MftStartIndex);

            // Read bytes belonging to specified MFT Record
            byte[] recordBytes = Helper.readDrive(streamToRead, mftOffset, VBR.BytesPerFileRecord);

            // Instantiate a FileRecord object for the $MFT file
            return FileRecord.Get(recordBytes, volume, (int)VBR.BytesPerFileRecord, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string volume)
        {
            Helper.getVolumeName(ref volume);

            using (FileStream streamToRead = Helper.getFileStream(volume))
            {
                return GetBytes(streamToRead, volume);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamToRead"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        internal static byte[] GetBytes(FileStream streamToRead, string volume)
        {
            FileRecord mftRecord = GetRecord(streamToRead, volume);
            return mftRecord.GetContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static byte[] GetSlack(string volume)
        {
            Helper.getVolumeName(ref volume);
            byte[] bytes = GetBytes(volume);
            return GetSlack(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetSlackByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            byte[] bytes = record.GetContent();
            return GetSlack(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static byte[] GetSlack(byte[] bytes)
        {
            List<byte> slackBytes = new List<byte>();

            for(int i = 0; i < bytes.Length; i += 1024)
            {
                uint realsize = BitConverter.ToUInt32(bytes, i + 0x18);
                uint allocatedsize = BitConverter.ToUInt32(bytes, i + 0x1C);
                uint slacksize = allocatedsize - realsize;
                slackBytes.AddRange(Helper.GetSubArray(bytes, i + (int)realsize, (int)slacksize));
            }

            return slackBytes.ToArray();
        }

        #endregion Static Methods
    }
}