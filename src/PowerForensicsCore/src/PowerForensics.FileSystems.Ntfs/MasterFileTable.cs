using System;
using System.Collections.Generic;
using System.IO;
using PowerForensics.FileSystems;

namespace PowerForensics.FileSystems.Ntfs
{
    class MftIndex
    {
        internal const int MFT_INDEX = 0;
        internal const int MFTMIRR_INDEX = 1;
        internal const int LOGFILE_INDEX = 2;
        internal const int VOLUME_INDEX = 3;
        internal const int ATTRDEF_INDEX = 4;
        internal const int ROOT_INDEX = 5;
        internal const int BITMAP_INDEX = 6;
        internal const int BOOT_INDEX = 7;
        internal const int BADCLUS_INDEX = 8;
        internal const int SECURE_INDEX = 9;
        internal const int UPCASE_INDEX = 10;
        internal const int EXTEND_INDEX = 11;
    }

    /// <summary>
    /// 
    /// </summary>
    public class MasterFileTable
    {
        #region Static Methods

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