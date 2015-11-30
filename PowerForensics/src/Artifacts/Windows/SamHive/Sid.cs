using System;
using System.Security.Principal;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class Sid
    {
        public static SecurityIdentifier Get()
        {
            ValueKey vk = ValueKey.Get(@"C:\Windows\system32\config\SAM", @"SAM\Domains\Account", "V");
            return new SecurityIdentifier(vk.GetData(), (int)vk.DataLength - 0x18);
        }

        public static SecurityIdentifier Get(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("SAM"))
            {
                ValueKey vk = ValueKey.Get(hivePath, @"SAM\Domains\Account", "V");
                return new SecurityIdentifier(vk.GetData(), (int)vk.DataLength - 0x18);
            }
            else
            {
                throw new Exception("Invalid SAM hive provided to -HivePath parameter.");
            }
        }
    }
}
