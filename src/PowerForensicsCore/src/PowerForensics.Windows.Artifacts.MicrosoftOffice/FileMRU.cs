using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts.MicrosoftOffice
{
    /// <summary>
    /// 
    /// </summary>
    public class FileMRU
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string User;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        public static FileMRU[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string user = RegistryHelper.GetUserHiveOwner(hivePath);
                List<FileMRU> fileList = new List<FileMRU>();

                byte[] bytes = RegistryHelper.GetHiveBytes(hivePath);
                string key = @"Software\Microsoft\Office";

                NamedKey OfficeKey = null;

                try
                {
                    OfficeKey = NamedKey.Get(bytes, hivePath, key);
                }
                catch
                {
                    throw new Exception(String.Format("Microsoft Office is not installed on this system or has not been opened by this User"));
                }

                foreach (NamedKey ov in OfficeKey.GetSubKeys(bytes))
                {
                    if (ov.Name.Contains(@".0"))
                    {
                        if (ov.Name != "8.0")
                        {
                            foreach (NamedKey nk in ov.GetSubKeys(bytes))
                            {
                                if (nk.Name == "Word" || nk.Name == "Excel" || nk.Name == "PowerPoint")
                                {
                                    foreach (NamedKey k in nk.GetSubKeys(bytes))
                                    {
                                        if (k.Name == "File MRU")
                                        {
                                            foreach (ValueKey vk in k.GetValues(bytes))
                                            {
                                                if(null == vk)
                                                {
                                                    continue;
                                                }else
                                                {
                                                    if (vk.Name.StartsWith("Item"))
                                                    {
                                                        fileList.Add(new FileMRU(user, (string)vk.GetData(bytes)));
                                                    }
                                                }
                                            }
                                        }
                                        else if(k.Name == "User MRU")
                                        {
                                           foreach (NamedKey sk in k.GetSubKeys(bytes))
                                           {
                                               foreach (NamedKey ssk in sk.GetSubKeys(bytes))
                                               {
                                                   if (ssk.Name == "File MRU")
                                                   {
                                                        foreach (ValueKey vk in ssk.GetValues(bytes))
                                                        {
                                                            if (vk.Name.StartsWith("Item"))
                                                            {
                                                                fileList.Add(new FileMRU(user, (string)vk.GetData(bytes)));
                                                            }
                                                        }
                                                   } 
                                               }
                                            }
                                        }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
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

        #endregion Static Methods
    }
}
