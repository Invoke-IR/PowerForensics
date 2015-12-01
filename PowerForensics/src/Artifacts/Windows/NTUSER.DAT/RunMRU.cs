using System;
using System.Text;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class RunMRU
    {
        public static string[] GetInstances(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("ntuser.dat"))
            {
                string Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU";

                byte[] bytes = Registry.Helper.GetHiveBytes(hivePath);

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

                string[] RunMRUStrings = new string[RunMRUKey.NumberOfValues - 1];

                byte[] MRUListBytes = MRUList.GetData(bytes);

                for(int i = 0; i <= MRUListBytes.Length - 4; i += 4)
                {
                    string MRUValue = Encoding.ASCII.GetString(MRUListBytes).TrimEnd('\0');
                    RunMRUStrings[i / 4] = Encoding.Unicode.GetString(ValueKey.Get(bytes, hivePath, Key, MRUValue.ToString()).GetData(bytes));
                }

                return RunMRUStrings;
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }
    }
}
