using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts.MicrosoftOffice
{
    public class FileMRU
    {
        #region Properties

        public readonly string User;
        public readonly string Path;
        public readonly DateTime LastAccessedTime;

        #endregion Properties

        #region Constructors

        private FileMRU(string user, string data)
        {
            User = user;
            Path = data.Split('*')[1];
            LastAccessedTime = DateTime.FromFileTimeUtc(Convert.ToInt64(data.Split('T')[1].Split(']')[0], 16));
        }

        #endregion Constructors

        #region StaticMethods

        public static FileMRU[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string user = RegistryHelper.GetUserHiveOwner(hivePath);
                List<FileMRU> fileList = new List<FileMRU>();

                byte[] bytes = RegistryHelper.GetHiveBytes(hivePath);
                NamedKey OfficeKey = RegistryHelper.GetOfficeKey(bytes, hivePath);

                foreach (NamedKey nk in OfficeKey.GetSubKeys(bytes))
                {
                    if (nk.Name == "Word" || nk.Name == "Excel" || nk.Name == "PowerPoint")
                    {
                        foreach (NamedKey k in nk.GetSubKeys(bytes))
                        {
                            if (k.Name == "File MRU")
                            {
                                foreach (ValueKey vk in k.GetValues(bytes))
                                {
                                    if (vk.Name != "Max Display")
                                    {
                                        fileList.Add(new FileMRU(user, (string)vk.GetData(bytes)));
                                    }
                                }
                            }
                        }
                    }
                }
                return fileList.ToArray();
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
}

        public static FileMRU[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<FileMRU> list = new List<FileMRU>();

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
