using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    #region AmcacheClass

    // https://msdn.microsoft.com/en-us/library/cc248285.aspx
    public class Amcache
    {
        #region Properties

        public readonly ushort SequenceNumber;
        public readonly ulong RecordNumber;
        public readonly string ProductName;
        public readonly string CompanyName;
        public readonly uint FileSize;
        public readonly string Description;
        public readonly DateTime CompileTime;
        public readonly DateTime ModifiedTimeUtc;
        public readonly DateTime BornTimeUtc;
        public readonly string Path;
        public readonly DateTime ModifiedTime2Utc;
        public readonly string Hash;

        #endregion Properties

        #region Constructors

        internal Amcache(NamedKey nk, byte[] bytes)
        {
            /*
            Console.WriteLine(nk.Name);
            ulong FileReference = ulong.Parse(nk.Name, System.Globalization.NumberStyles.AllowHexSpecifier);
            byte[] filerefbytes = BitConverter.GetBytes(FileReference);
            SequenceNumber = (BitConverter.ToUInt16(filerefbytes, 0x06));
            RecordNumber = (BitConverter.ToUInt64(filerefbytes, 0x00) & 0x0000FFFFFFFFFFFF);
            */

            foreach (ValueKey vk in nk.GetValues(bytes))
            {
                switch(vk.Name)
                {
                    case "0":
                        ProductName = Encoding.Unicode.GetString(vk.GetData(bytes));
                        break;
                    case "1":
                        CompanyName = Encoding.Unicode.GetString(vk.GetData(bytes));
                        break;
                    case "6":
                        FileSize = BitConverter.ToUInt32(vk.GetData(bytes), 0x00);
                        break;
                    case "c":
                        Description = Encoding.Unicode.GetString(vk.GetData(bytes));
                        break;
                    case "f":
                        CompileTime = Util.FromUnixTime(BitConverter.ToUInt32(vk.GetData(bytes), 0x00));
                        break;
                    case "11":
                        ModifiedTimeUtc = DateTime.FromFileTimeUtc(BitConverter.ToInt64(vk.GetData(bytes), 0x00));
                        break;
                    case "12":
                        BornTimeUtc = DateTime.FromFileTimeUtc(BitConverter.ToInt64(vk.GetData(bytes), 0x00));
                        break;
                    case "15":
                        Path = Encoding.Unicode.GetString(vk.GetData(bytes));
                        break;
                    case "17":
                        ModifiedTime2Utc = DateTime.FromFileTimeUtc(BitConverter.ToInt64(vk.GetData(bytes), 0x00));
                        break;
                    case "101":
                        Hash = Encoding.Unicode.GetString(vk.GetData(bytes)).TrimStart('0');
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion Constructors

        #region StaticMethods

        #region GetInstancesMethods

        public static Amcache[] GetInstances(string volume)
        {
            WindowsVersion version = WindowsVersion.Get(volume);
            if(version.CurrentVersion.CompareTo(new Version("6.2")) >= 0)
            {
                return GetInstancesByPath(Util.GetVolumeLetter(volume) + @"\Windows\AppCompat\Programs\Amcache.hve");
            }
            else
            {
                throw new Exception("The Amcache hive is only available on Windows 8 and newer Operating Systems");
            }
        }

        public static Amcache[] GetInstancesByPath(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("Amcache.hve"))
            {
                string Key = @"Root\File";

                byte[] bytes = Registry.Helper.GetHiveBytes(hivePath);

                NamedKey[] FileSubKey = NamedKey.GetInstances(bytes, hivePath, Key);

                List<Amcache> amcacheList = new List<Amcache>();

                foreach (NamedKey key in FileSubKey)
                {
                    if(key.NumberOfSubKeys != 0)
                    {
                        foreach (NamedKey nk in key.GetSubKeys(bytes, key.FullName))
                        {
                            amcacheList.Add(new Amcache(nk, bytes));
                        }
                    }
                }
                return amcacheList.ToArray();
            }
            else
            {
                throw new Exception("Invalid Amcache.hve hive provided to -HivePath parameter.");
            }

        }

        #endregion GetInstancesMethods

        #endregion StaticMethods
    }

    #endregion AmcacheClass
}
