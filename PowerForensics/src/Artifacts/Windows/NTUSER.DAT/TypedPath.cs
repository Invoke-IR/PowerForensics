using System;
using System.Text;
using PowerForensics.Registry;

// TODO: Add TypedURLtimes for Win8

namespace PowerForensics.Artifacts
{
    public class TypedPaths
    {
        #region StaticMethods

        public static string[] GetInstances(string hivePath)
        {
            string Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\TypedPaths";

            byte[] bytes = Registry.Helper.GetHiveBytes(hivePath);

            NamedKey nk = NamedKey.Get(bytes, hivePath, Key);

            string[] paths = new string[nk.NumberOfValues];

            int i = 0;

            foreach (ValueKey vk in nk.GetValues(bytes))
            {
                paths[i] = Encoding.Unicode.GetString(vk.GetData(bytes));
                i++;
            }
            return paths;
        }

        #endregion StaticMethods
    }
}
