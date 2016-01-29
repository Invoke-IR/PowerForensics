using System;
using System.Collections.Generic;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts.MicrosoftOffice
{
    #region OutlookCatalogClass
    public class OutlookCatalog
    {
        #region Properties

        public readonly string User;
        public readonly string Path;

        #endregion Properties

        #region Constructors

        private OutlookCatalog(string user, ValueKey vk)
        {
            User = user;
            Path = vk.Name;
        }

        #endregion Construtors

        #region StaticMethods

        public static OutlookCatalog[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                byte[] hiveBytes = RegistryHelper.GetHiveBytes(hivePath);

                string user = RegistryHelper.GetUserHiveOwner(hivePath);

                string OfficeVersion = RegistryHelper.GetOfficeVersion(hiveBytes, hivePath);

                List<OutlookCatalog> list = new List<OutlookCatalog>();

                NamedKey CatalogKey = null;

                if (OfficeVersion == "12.0")
                {
                    CatalogKey = NamedKey.Get(hiveBytes, hivePath, @"Software\Microsoft\Office\" + OfficeVersion + @"\Outlook\Catalog");
                }
                else
                {
                    CatalogKey = NamedKey.Get(hiveBytes, hivePath, @"Software\Microsoft\Office\" + OfficeVersion + @"\Outlook\Search\Catalog");
                }

                if (CatalogKey.NumberOfValues > 0)
                {
                    foreach (ValueKey vk in CatalogKey.GetValues())
                    {
                        list.Add(new OutlookCatalog(user, vk));
                    }
                }

                return list.ToArray();
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }

        public static OutlookCatalog[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<OutlookCatalog> list = new List<OutlookCatalog>();

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
    #endregion OutlookCatalogClass
}
