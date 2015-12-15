using System;
using System.Security.Principal;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class Sid
    {
        public static SecurityIdentifier Get(string volume)
        {
            return GetByPath(Util.GetVolumeLetter(volume) + @"\Windows\system32\config\SAM");
        }

        public static SecurityIdentifier GetByPath(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "SAM"))
            {
                ValueKey vk = ValueKey.Get(hivePath, @"SAM\Domains\Account", "V");
                return new SecurityIdentifier((byte[])vk.GetData(), (int)vk.DataLength - 0x18);
            }
            else
            {
                throw new Exception("Invalid SAM hive provided to -HivePath parameter.");
            }
        }
    }
}
