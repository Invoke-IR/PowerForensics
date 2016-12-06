using System;
using System.Text;

namespace PowerForensics.FileSystems.Fat
{
    /// <summary>
    /// 
    /// </summary>
    public class LongDirectoryEntry
    {
        #region Properties

        internal readonly byte LDIR_Ord;

        private readonly string LDIR_Name1;

        internal readonly DirectoryEntry.FILE_ATTR LDIR_Attr;

        internal readonly byte LDIR_Type;

        internal readonly byte LDIR_Chksum;

        private readonly string LDIR_Name2;

        internal readonly ushort LDIR_FstClusLO;

        private readonly string LDIR_Name3;

        internal readonly string LDIR_NamePart;

        #endregion Properties

        #region Constructors

        private LongDirectoryEntry(byte[] bytes, int index)
        {
            LDIR_Ord = bytes[0 + index];
            LDIR_Name1 = Encoding.Unicode.GetString(bytes, 1 + index, 10);
            LDIR_Attr = (DirectoryEntry.FILE_ATTR)bytes[11 + index];
            LDIR_Type = bytes[12 + index];
            LDIR_Chksum = bytes[13 + index];
            LDIR_Name2 = Encoding.Unicode.GetString(bytes, 14 + index, 12);
            LDIR_FstClusLO = BitConverter.ToUInt16(bytes, 26 + index);
            LDIR_Name3 = Encoding.Unicode.GetString(bytes, 28 + index, 4);
            LDIR_NamePart = String.Format("{0}{1}{2}", LDIR_Name1, LDIR_Name2, LDIR_Name3);
        }

        #endregion Constructors

        #region Static Methods
        
        internal static LongDirectoryEntry Get(byte[] bytes, int index)
        {
            return new LongDirectoryEntry(bytes, index);
        }

        #endregion Static Methods
    }
}
