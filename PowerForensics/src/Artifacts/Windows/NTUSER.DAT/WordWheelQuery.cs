using System;
using System.Text;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class WordWheelQuery
    {
        public static string[] GetInstances(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.ToUpper().Contains("NTUSER.DAT"))
            {
                string Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\WordWheelQuery";

                byte[] bytes = Registry.Helper.GetHiveBytes(hivePath);

                NamedKey nk = null;

                try
                {
                    nk = NamedKey.Get(bytes, hivePath, Key);
                }
                catch
                {
                    return null;
                }

                ValueKey MRUList = ValueKey.Get(bytes, hivePath, Key, "MRUListEx");

                string[] dataStrings = new string[nk.NumberOfValues - 1];

                byte[] MRUListBytes = MRUList.GetData(bytes);

                for (int i = 0; i < MRUListBytes.Length - 4; i += 4)
                {
                    uint MRUValue = BitConverter.ToUInt32(MRUListBytes, i);
                    dataStrings[i / 4] = Encoding.Unicode.GetString(ValueKey.Get(bytes, hivePath, Key, MRUValue.ToString()).GetData(bytes));
                }

                return dataStrings;
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }
    }
}
