using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Windows.Artifacts.SoftwareHive;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts.ApplicationCompatibilityCache
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/cc248285.aspx
    /// </summary>
    public class Amcache
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort SequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly string ProductName;

        /// <summary>
        /// 
        /// </summary>
        public readonly string CompanyName;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FileSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime CompileTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifiedTimeUtc;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BornTimeUtc;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Path;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifiedTime2Utc;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Hash;

        #endregion Properties

        #region Constructors

        private Amcache(NamedKey nk, byte[] bytes)
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
                switch (vk.Name)
                {
                    case "0":
                        try
                        {
                            ProductName = (string)vk.GetData(bytes);
                        }
                        catch
                        {
                            ProductName = Encoding.ASCII.GetString((byte[])vk.GetData(bytes));
                        }
                        break;
                    case "1":
                        try
                        {
                            CompanyName = (string)vk.GetData(bytes);
                        }
                        catch
                        {
                            CompanyName = Encoding.ASCII.GetString((byte[])vk.GetData(bytes));
                        }
                        break;
                    case "6":
                        FileSize = BitConverter.ToUInt32((byte[])vk.GetData(bytes), 0x00);
                        break;
                    case "c":
                        Description = (string)vk.GetData(bytes);
                        break;
                    case "f":
                        CompileTime = Helper.FromUnixTime(BitConverter.ToUInt32((byte[])vk.GetData(bytes), 0x00));
                        break;
                    case "11":
                        ModifiedTimeUtc = DateTime.FromFileTimeUtc(BitConverter.ToInt64((byte[])vk.GetData(bytes), 0x00));
                        break;
                    case "12":
                        BornTimeUtc = DateTime.FromFileTimeUtc(BitConverter.ToInt64((byte[]) vk.GetData(bytes), 0x00));
                        break;
                    case "15":
                        Path = (string)vk.GetData(bytes);
                        break;
                    case "17":
                        ModifiedTime2Utc = DateTime.FromFileTimeUtc(BitConverter.ToInt64((byte[])vk.GetData(bytes), 0x00));
                        break;
                    case "101":
                        string hash = (string)vk.GetData(bytes);
                        Hash = hash.TrimStart('0');
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static Amcache[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            WindowsVersion version = WindowsVersion.Get(volume);
            if(version.CurrentVersion.CompareTo(new Version("6.2")) >= 0)
            {
                return GetInstancesByPath(Helper.GetVolumeLetter(volume) + @"\Windows\AppCompat\Programs\Amcache.hve");
            }
            else
            {
                throw new Exception("The Amcache hive is only available on Windows 8 and newer Operating Systems.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        public static Amcache[] GetInstancesByPath(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("Amcache.hve"))
            {
                string Key = @"Root\File";

                byte[] bytes = Registry.RegistryHelper.GetHiveBytes(hivePath);

                NamedKey[] FileSubKey = NamedKey.GetInstances(bytes, hivePath, Key);

                List<Amcache> amcacheList = new List<Amcache>();

                foreach (NamedKey key in FileSubKey)
                {
                    if(key.NumberOfSubKeys != 0)
                    {
                        foreach (NamedKey nk in key.GetSubKeys(bytes))
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

        #endregion Static Methods
    }
}
