using System;
using System.Text;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    #region WindowsVersion
    public class WindowsVersion
    {
        #region Properties

        public readonly string ProductName;
        public readonly uint CurrentMajorVersion;
        public readonly uint CurrentMinorVersion;
        public readonly Version CurrentVersion;
        public readonly DateTime InstallTime;
        public readonly string RegisteredOwner;
        public readonly string SystemRoot;

        #endregion Properties

        #region Constructors

        internal WindowsVersion(byte[] bytes, NamedKey nk)
        {
            foreach (ValueKey vk in nk.GetValues(bytes))
            {
                switch (vk.Name)
                {
                    case "ProductName":
                        ProductName = Encoding.Unicode.GetString(vk.GetData(bytes));
                        break;
                    case "CurrentMajorVersionNumber":
                        CurrentMajorVersion = BitConverter.ToUInt32(vk.GetData(bytes), 0x00);
                        break;
                    case "CurrentMinorVersionNumber":
                        CurrentMinorVersion = BitConverter.ToUInt32(vk.GetData(bytes), 0x00);
                        break;
                    case "CurrentVersion":
                        CurrentVersion = new Version(Encoding.Unicode.GetString(vk.GetData(bytes)));
                        break;
                    case "InstallTime":
                        InstallTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(vk.GetData(bytes), 0x00));
                        break;
                    case "RegisteredOwner":
                        RegisteredOwner = Encoding.Unicode.GetString(vk.GetData(bytes));
                        break;
                    case "SystemRoot":
                        SystemRoot = Encoding.Unicode.GetString(vk.GetData(bytes));
                        break;
                    default:
                        break;
                }
            }

            //ProductName = ;
            //CurrentVersion = ;
        }

        #endregion Constructors

        #region StaticMethods

        public static WindowsVersion Get(string volume)
        {
            return WindowsVersion.GetByPath(Util.GetVolumeLetter(volume) + @"\Windows\system32\config\SOFTWARE");
        }

        public static WindowsVersion GetByPath(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("SOFTWARE"))
            {
                byte[] bytes = Helper.GetHiveBytes(hivePath);
                NamedKey nk = NamedKey.Get(bytes, hivePath, @"Microsoft\Windows NT\CurrentVersion");
                return new WindowsVersion(bytes, nk);
            }
            else
            {
                throw new Exception("Invalid SOFTWARE hive provided to -HivePath parameter.");
            }
        }

        #endregion StaticMethods
    }
    #endregion WindowsVersion
}
