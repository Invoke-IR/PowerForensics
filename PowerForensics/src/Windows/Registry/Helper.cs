using System;
using PowerForensics.Ntfs;

namespace PowerForensics.Registry
{
    #region HelperClass

    public class Helper
    {
        #region StaticMethods

        public static byte[] GetHiveBytes(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return record.GetContent();
        }

        public static NamedKey GetRootKey(string path)
        {
            byte[] bytes = GetHiveBytes(path);

            RegistryHeader header = new RegistryHeader(Util.GetSubArray(bytes, 0x00, 0x200));
            int offset = (int)header.RootKeyOffset + RegistryHeader.HBINOFFSET;
            int size = Math.Abs(BitConverter.ToInt32(bytes, offset));

            return new NamedKey(Util.GetSubArray(bytes, (uint)offset, (uint)size), path, "");
        }

        internal static NamedKey GetRootKey(byte[] bytes, string path)
        {
            #region RegistryHeader

            RegistryHeader header = new RegistryHeader(Util.GetSubArray(bytes, 0x00, 0x200));

            #endregion RegistryHeader

            int offset = (int)header.RootKeyOffset + RegistryHeader.HBINOFFSET;
            int size = Math.Abs(BitConverter.ToInt32(bytes, offset));

            return new NamedKey(Util.GetSubArray(bytes, (uint)offset, (uint)size), path, "");
        }
        
        #endregion StaticMethods
    }

    #endregion HelperClass
}
