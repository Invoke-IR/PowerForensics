using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class RunMRU
    {
        #region Properties

        public readonly string User;
        public readonly string ImagePath;

        #endregion Properties

        #region Constructors

        private RunMRU(string user, string path)
        {
            User = user;
            ImagePath = path;
        }

        #endregion Constructors

        #region StaticMethods

        public static RunMRU[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string user = RegistryHelper.GetUserHiveOwner(hivePath);
                string Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU";

                byte[] bytes = Registry.RegistryHelper.GetHiveBytes(hivePath);

                NamedKey RunMRUKey = null;
                ValueKey MRUList = null;

                try
                {
                    RunMRUKey = NamedKey.Get(bytes, hivePath, Key);
                }
                catch
                {
                    return null;
                }

                try
                {
                    MRUList = ValueKey.Get(bytes, hivePath, Key, "MRUList");
                }
                catch
                {
                    return null;
                }

                RunMRU[] RunMRUStrings = new RunMRU[RunMRUKey.NumberOfValues - 1];

                byte[] MRUListBytes = (byte[])MRUList.GetData(bytes);

                for(int i = 0; i <= MRUListBytes.Length - 4; i += 4)
                {
                    string MRUValue = Encoding.ASCII.GetString(MRUListBytes).TrimEnd('\0');
                    RunMRUStrings[i / 4] = new RunMRU(user, (string)ValueKey.Get(bytes, hivePath, Key, MRUValue.ToString()).GetData(bytes));
                }

                return RunMRUStrings;
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }

        public static RunMRU[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<RunMRU> list = new List<RunMRU>();

            foreach (string hivePath in RegistryHelper.GetUserHiveInstances(volume))
            {
                try
                {
                    list.AddRange(Get(hivePath));
                }
                catch
                {

                }
            }

            return list.ToArray();
        }

        #endregion StaticMethods
    }
}
