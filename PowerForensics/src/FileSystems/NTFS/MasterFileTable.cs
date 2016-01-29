using System;
using System.Collections.Generic;
using System.IO;

namespace PowerForensics.Ntfs
{
    #region MasterFileTableClass

    public class MasterFileTable
    {
        #region StaticMethods

        #region GetRecordMethods

        internal static FileRecord GetRecord(FileStream streamToRead, string volume)
        {
            // Instantiate VolumeBootRecord object
            VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

            // Calculate byte offset to the Master File Table (MFT)
            ulong mftOffset = ((ulong)VBR.BytesPerCluster * VBR.MFTStartIndex);

            // Read bytes belonging to specified MFT Record and store in byte array
            return new FileRecord(Helper.readDrive(streamToRead, mftOffset, (ulong)VBR.BytesPerFileRecord), volume, true);
        }

        #endregion GetRecordMethods

        #region GetBytesMethods

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

            foreach (FileRecordAttribute attr in mftRecord.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    return (attr as NonResident).GetBytes(volume);
                }
            }
            throw new Exception("Error reading MFT bytes.");
        }

        #endregion GetBytesMethods

        #region GetSlackMethods

        public static byte[] GetSlack(string volume)
        {
            Helper.getVolumeName(ref volume);
            byte[] bytes = GetBytes(volume);
            return GetSlack(bytes);
        }

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

        #endregion GetSlackMethods

        #endregion StaticMethods
    }

    #endregion MasterFileTableClass
}
