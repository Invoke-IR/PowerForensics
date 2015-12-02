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
            string Key = @"Software\Microsoft\Internet Explorer\TypedUrls";

            byte[] bytes = Registry.Helper.GetHiveBytes(hivePath);
            
            NamedKey nk = NamedKey.Get(bytes, hivePath, Key);

            string[] urls = new string[nk.NumberOfValues];

            foreach (ValueKey vk in nk.GetValues(bytes))
            {
                for (int i = 0; i < urls.Length; i++)
                {
                    urls[i] = Encoding.Unicode.GetString(vk.GetData(bytes));
                }
            }
            return urls;
        }

        #endregion StaticMethods
    }
}
