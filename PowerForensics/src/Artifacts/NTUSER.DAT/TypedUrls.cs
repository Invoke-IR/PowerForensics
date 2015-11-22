using System;
using System.Text;
using PowerForensics.Registry;

// TODO: Add TypedURLtimes for Win8

namespace PowerForensics.Artifacts
{
    public class TypedUrls
    {
        #region StaticMethods

        public static string[] GetInstances(string hivePath)
        {
            string Key = @"Software\Microsoft\Internet Explorer";

            byte[] bytes = Registry.Helper.GetHiveBytes(hivePath);
            
            NamedKey[] keys = NamedKey.GetInstances(bytes, hivePath, Key);

            string[] urls = new string[0];

            foreach (NamedKey nk in keys)
            {
                if (nk.Name == "TypedURLs")
                {
                    urls = new string[nk.NumberOfValues];

                    ValueKey[] vkArray = nk.GetValues(bytes);

                    for (int i = 0; i < vkArray.Length; i++)
                    {
                        urls[i] = Encoding.Unicode.GetString(vkArray[i].GetData(bytes));
                    }
                }
            }
            return urls;
        }

        #endregion StaticMethods
    }
}
