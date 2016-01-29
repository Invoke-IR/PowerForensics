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

        private WindowsVersion(byte[] bytes, NamedKey nk)
        {
            foreach (ValueKey vk in nk.GetValues(bytes))
            {
                switch (vk.Name)
                {
                    case "ProductName":
                        ProductName = (string)vk.GetData(bytes);
                        break;
                    case "CurrentMajorVersionNumber":
                        CurrentMajorVersion = BitConverter.ToUInt32((byte[])vk.GetData(bytes), 0x00);
                        break;
                    case "CurrentMinorVersionNumber":
                        CurrentMinorVersion = BitConverter.ToUInt32((byte[])vk.GetData(bytes), 0x00);
                        break;
                    case "CurrentVersion":
                        CurrentVersion = new Version((string)vk.GetData(bytes));
                        break;
                    case "InstallTime":
                        InstallTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64((byte[])vk.GetData(bytes), 0x00));
                        break;
                    case "RegisteredOwner":
                        RegisteredOwner = (string)vk.GetData(bytes);
                        break;
                    case "SystemRoot":
                        SystemRoot = (string)vk.GetData(bytes);
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
            Helper.getVolumeName(ref volume);
            return WindowsVersion.GetByPath(Helper.GetVolumeLetter(volume) + @"\Windows\system32\config\SOFTWARE");
        }

        public static WindowsVersion GetByPath(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "SOFTWARE"))
            {
                byte[] bytes = RegistryHelper.GetHiveBytes(hivePath);
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
