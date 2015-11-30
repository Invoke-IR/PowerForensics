using System;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    #region TimezoneClass

    public class Timezone
    {
        #region Properties

        public readonly string RegistryTimezone;
        public readonly string dotNetStandardTimezone;
        public readonly string dotNetDaylightTimezone;
        public readonly bool IsDaylightSavingTime;

        #endregion Properties

        #region Constructors

        internal Timezone(string registry, string standard, string daylight, bool dst)
        {
            RegistryTimezone = registry;
            dotNetStandardTimezone = standard;
            dotNetDaylightTimezone = daylight;
            IsDaylightSavingTime = dst;
        }

        #endregion Constructors

        #region StaticMethods

        public static Timezone Get()
        {
            return Timezone.Get(@"C:\Windows\system32\config\SYSTEM");
        }

        public static Timezone Get(string hivePath)
        {
            if (RegistryHeader.Get(hivePath).HivePath.Contains("SYSTEM"))
            {
                ValueKey vk = ValueKey.Get(hivePath, @"ControlSet001\Control\TimeZoneInformation", "TimeZoneKeyName");
                TimeZone tz = TimeZone.CurrentTimeZone;

                return new Timezone(System.Text.Encoding.Unicode.GetString(vk.GetData()), tz.StandardName, tz.DaylightName, tz.IsDaylightSavingTime(DateTime.Now));
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
