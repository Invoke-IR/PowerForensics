using System;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class WindowsVersion
    {
        #region Properties

        public readonly string ProductName;
        public readonly Version CurrentVersion;

        #endregion Properties

        #region Constructors

        internal WindowsVersion(NamedKey nk)
        {
            //ProductName = ;
            //CurrentVersion = ;
        }

        #endregion Constructors

        #region StaticMethods

        public static WindowsVersion Get()
        {
            return WindowsVersion.Get(@"C:\Windows\system32\config\SOFTWARE");
        }

        public static WindowsVersion Get(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("SOFTWARE"))
            {
                byte[] bytes = Helper.GetHiveBytes(hivePath);
                NamedKey nk = NamedKey.Get(bytes, hivePath, @"Micosoft\Windows NT\CurrentVersion");
                return new WindowsVersion(nk);
            }
            else
            {
                throw new Exception("Invalid SOFTWARE hive provided to -HivePath parameter.");
            }
        }

        #endregion StaticMethods
    }
}
