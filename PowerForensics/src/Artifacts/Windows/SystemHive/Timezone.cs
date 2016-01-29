using System;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    #region TimezoneClass

    public class Timezone
    {
        #region Properties

        public readonly string RegistryTimezone;

        #endregion Properties

        #region Constructors

        internal Timezone(string registry)
        {
            RegistryTimezone = registry;
        }

        #endregion Constructors

        #region StaticMethods

        public static Timezone Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            string volLetter = Helper.GetVolumeLetter(volume);
            return Timezone.GetByPath(volLetter + @"\Windows\system32\config\SYSTEM");
        }

        public static Timezone GetByPath(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "SYSTEM"))
            {
                ValueKey vk = ValueKey.Get(hivePath, @"ControlSet001\Control\TimeZoneInformation", "TimeZoneKeyName");
                return new Timezone((string)vk.GetData());
            }
            else
            {
                throw new Exception("Invalid SYSTEM hive provided to -HivePath parameter.");
            }
        }

        #endregion StaticMethods
    }

    #endregion TimezoneClass
}
