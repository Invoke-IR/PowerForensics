using System;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    #region ShimcacheClass

    public class Shimcache
    {
        #region Constants

        internal const uint WINXP_MAGIC = 0xDEADBEEF;
        internal const uint NT5_2_MAGIC = 0xBADC0FFE;
        internal const uint NT6_1_MAGIC = 0xBADC0FEE;
        //internal const uint WIN8_MAGIC = ;
        //internal const uint WIN81_MAGIC = ;

        #endregion Constants

        #region Properties

        internal readonly ushort Length;
        internal readonly ushort MaximumLength;
        internal readonly uint PathOffset;
        public readonly string Path;
        public readonly DateTime ModificationTime;
        internal readonly uint FileFlags;
        internal readonly uint Flags;
        internal readonly uint BlobSize;
        internal readonly uint BlobOffset;

        #endregion Properties

        #region Constructors

        internal Shimcache(byte[] bytes)
        {
            
        }

        #endregion Constructors

        #region StaticMethods

        public static byte[] Get()
        {
            return Shimcache.Get(@"C:\Windows\system32\config\SYSTEM");
        }

        public static byte[] Get(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("SYSTEM"))
            {
                ValueKey vk = ValueKey.Get(hivePath, @"ControlSet001\Control\Session Manager\AppCompatCache", "AppCompatCache");
                byte[] bytes = vk.GetData();

                switch (BitConverter.ToUInt32(bytes, 0x00))
                {
                    // Windows 5.2 and 6.0 (Server 2003, Vista, & Server 2008)
                    case WINXP_MAGIC:
                        Console.WriteLine("XP");
                        break;
                    case NT5_2_MAGIC:
                        Console.WriteLine("5.2");
                        break;
                    case NT6_1_MAGIC:
                        Console.WriteLine("6.1");
                        break;
                    default:
                        //Console.WriteLine("Default");
                        break;
                }

                return bytes;
            }
            else
            {
                throw new Exception("Invalid SYSTEM hive provided to -HivePath parameter.");
            }
        }

        #endregion StaticMethods
    }

    #endregion ShimcacheClass
}
