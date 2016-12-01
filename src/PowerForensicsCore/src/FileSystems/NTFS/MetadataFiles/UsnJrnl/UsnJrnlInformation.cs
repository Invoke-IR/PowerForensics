using System;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class UsnJrnlInformation
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ulong MaxSize;

        /// <summary>
        /// 
        /// </summary>
        public ulong AllocationDelta;

        /// <summary>
        /// 
        /// </summary>
        public ulong UsnId;

        /// <summary>
        /// 
        /// </summary>
        public ulong LowestUsn;

        #endregion Properties

        #region Constructors

        private UsnJrnlInformation(byte[] maxBytes)
        {
            MaxSize = BitConverter.ToUInt64(maxBytes, 0x00);
            AllocationDelta = BitConverter.ToUInt64(maxBytes, 0x08);
            UsnId = BitConverter.ToUInt64(maxBytes, 0x10);
            LowestUsn = BitConverter.ToUInt64(maxBytes, 0x18);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static UsnJrnlInformation Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UsnJrnlInformation GetByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return new UsnJrnlInformation(record.GetContent(@"$Max"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetBytesByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetBytesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return record.GetContent(@"$Max");
        }

        #endregion Static Methods
    }
}