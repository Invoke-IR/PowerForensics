using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Registry;

// TODO: Add TypedURL times for Win8

namespace PowerForensics.Artifacts
{
    public class TypedUrls
    {
        #region Properties

        public readonly string User;
        public readonly string UrlString;

        #endregion Properties

        #region Constructors

        private TypedUrls(string user, string path)
        {
            User = user;
            UrlString = path;
        }

        #endregion Constructors

        #region StaticMethods

        public static TypedUrls[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string Key = @"Software\Microsoft\Internet Explorer\TypedUrls";

                byte[] bytes = Registry.RegistryHelper.GetHiveBytes(hivePath);

                NamedKey nk = NamedKey.Get(bytes, hivePath, Key);

                TypedUrls[] urls = new TypedUrls[nk.NumberOfValues];

                foreach (ValueKey vk in nk.GetValues(bytes))
                {
                    for (int i = 0; i < urls.Length; i++)
                    {
                        urls[i] = new TypedUrls(RegistryHelper.GetUserHiveOwner(hivePath), (string)vk.GetData(bytes));
                    }
                }
                return urls;
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }

        public static TypedUrls[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<TypedUrls> list = new List<TypedUrls>();

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
