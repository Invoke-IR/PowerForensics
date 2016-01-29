using System;
using System.Text;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class LastVisitedMRU
    {
        #region Properties

        public readonly string User;
        public readonly string ImagePath;

        #endregion Properties

        #region Constructors

        private LastVisitedMRU(string user, string data)
        {
            User = user;
            ImagePath = data;
        }

        #endregion Constructors

        #region StaticMethods

        public static LastVisitedMRU[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\LastVisitedPidlMRU";

                byte[] bytes = Registry.RegistryHelper.GetHiveBytes(hivePath);

                NamedKey nk = null;

                try
                {
                    nk = NamedKey.Get(bytes, hivePath, Key);
                }
                catch
                {
                    try
                    {
                        Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\LastVisitedMRU";
                        nk = NamedKey.Get(bytes, hivePath, Key);
                    }
                    catch
                    {
                        return null;
                    }
                }

                ValueKey MRUList = ValueKey.Get(bytes, hivePath, Key, "MRUListEx");

                LastVisitedMRU[] dataStrings = new LastVisitedMRU[nk.NumberOfValues - 1];

                byte[] MRUListBytes = (byte[])MRUList.GetData(bytes);

                for (int i = 0; i < MRUListBytes.Length - 4; i += 4)
                {
                    uint MRUValue = BitConverter.ToUInt32(MRUListBytes, i);
                    dataStrings[i / 4] = new LastVisitedMRU(RegistryHelper.GetUserHiveOwner(hivePath), (string)ValueKey.Get(bytes, hivePath, Key, MRUValue.ToString()).GetData(bytes));
                }

                return dataStrings;
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }

        #endregion StaticMethods
    }
}
