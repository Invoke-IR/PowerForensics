using System;
using System.Text;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts.SoftwareHive
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowsVersion
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string ProductName;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CurrentMajorVersion;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CurrentMinorVersion;

        /// <summary>
        /// 
        /// </summary>
        public readonly Version CurrentVersion;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime InstallTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly string RegisteredOwner;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static WindowsVersion Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return WindowsVersion.GetByPath(Helper.GetVolumeLetter(volume) + @"\Windows\system32\config\SOFTWARE");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
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

        #endregion Static Methods
    }
}
