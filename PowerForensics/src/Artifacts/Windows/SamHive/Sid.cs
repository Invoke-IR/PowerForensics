using System;
using System.Security.Principal;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class Sid
    {
        public static string Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetByPath(Helper.GetVolumeLetter(volume) + @"\Windows\system32\config\SAM");
        }

        public static string GetByPath(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "SAM"))
            {
                ValueKey vk = ValueKey.Get(hivePath, @"SAM\Domains\Account", "V");
                byte[] bytes = (byte[])vk.GetData();
                return Helper.GetSecurityDescriptor(Helper.GetSubArray(bytes, bytes.Length - 0x18, 0x18));
            }
            else
            {
                throw new Exception("Invalid SAM hive provided to -HivePath parameter.");
            }
        }
    }
}
