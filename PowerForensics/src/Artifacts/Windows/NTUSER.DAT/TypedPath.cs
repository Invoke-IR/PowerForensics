using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Registry;

// TODO: Add TypedURLtimes for Win8

namespace PowerForensics.Artifacts
{
    public class TypedPaths
    {
        #region Properties

        public readonly string User;
        public readonly string ImagePath;

        #endregion Properties

        #region Constructors

        internal TypedPaths(string user, string path)
        {
            User = user;
            ImagePath = path;
        }

        #endregion Constructors

        #region StaticMethods

        public static TypedPaths[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\TypedPaths";

                byte[] bytes = Registry.RegistryHelper.GetHiveBytes(hivePath);

                NamedKey nk = NamedKey.Get(bytes, hivePath, Key);

                TypedPaths[] paths = new TypedPaths[nk.NumberOfValues];

                int i = 0;

                foreach (ValueKey vk in nk.GetValues(bytes))
                {
                    paths[i] = new TypedPaths(RegistryHelper.GetUserHiveOwner(hivePath), (string)vk.GetData(bytes));
                    i++;
                }
                return paths;
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }

        public static TypedPaths[] GetInstances(string volume)
        {
            List<TypedPaths> list = new List<TypedPaths>();

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
