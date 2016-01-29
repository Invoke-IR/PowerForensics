using System;
using System.Collections.Generic;
using System.Text;

namespace PowerForensics.Fat
{
    public class DirectoryEntry
    {
        #region Enums

        [FlagsAttribute]
        public enum ATTR_FLAGS
        {
            ATTR_READ_ONLY = 0x01,
            ATTR_HIDDEN = 0x02,
            ATTR_SYSTEM = 0x04,
            ATTR_VOLUME_ID = 0x08,
            ATTR_DIRECTORY = 0x10,
            ATTR_ARCHIVE = 0x20,
            ATTR_LONG_NAME = ATTR_READ_ONLY | ATTR_HIDDEN | ATTR_SYSTEM | ATTR_VOLUME_ID
        }

        #endregion Enums

        #region Properties

        public readonly string LongName;
        public readonly string Name;
        public readonly ATTR_FLAGS Attribute;
        public readonly DateTime CreateTime;
        public readonly DateTime AccessTime;
        public readonly ushort FirstClusterHigh;
        public readonly DateTime WriteTime;
        public readonly ushort FirstClusterLow;
        public readonly uint FileSize;

        #endregion Properties

        #region Constructor

        internal DirectoryEntry(byte[] bytes, ref int offset)
        {
            Attribute = (ATTR_FLAGS)bytes[0x0B + offset];

            #region LondName

            string longName = null;

            if (Attribute == ATTR_FLAGS.ATTR_LONG_NAME)
            {
                longName = GetLongName(bytes, ref offset).Split('\0')[0];
                Attribute = (ATTR_FLAGS)bytes[0x0B + offset];
            }

            LongName = longName;

            #endregion LongName

            #region Name

            string name = Encoding.ASCII.GetString(bytes, 0x00 + offset, 0x0B);

            if ((Attribute & ATTR_FLAGS.ATTR_VOLUME_ID) == ATTR_FLAGS.ATTR_VOLUME_ID)
            {
                Name = name;
            }
            else if ((Attribute & ATTR_FLAGS.ATTR_DIRECTORY) == ATTR_FLAGS.ATTR_DIRECTORY)
            {
                Name = name;
            }
            else
            {
                StringBuilder namesb = new StringBuilder();
                namesb.Append(name.Substring(0, 8).TrimEnd(' '));
                namesb.Append(".");
                namesb.Append(name.Substring(8, 3));
                Name = namesb.ToString();
            }

            #endregion Name

            CreateTime = GetFatDate(bytes, 0x10 + offset, 0x0E + offset, bytes[0x0D + offset], false);
            AccessTime = GetFatDate(bytes, 0x12 + offset, 0, 0, true);
            FirstClusterHigh = BitConverter.ToUInt16(bytes, 0x14 + offset);
            WriteTime = GetFatDate(bytes, 0x18 + offset, 0x16 + offset, 0, false);
            FirstClusterLow = BitConverter.ToUInt16(bytes, 0x1A + offset);
            FileSize = BitConverter.ToUInt32(bytes, 0x1C + offset);
            offset += 0x20;           
        }

        #endregion Constructor

        #region StaticMethods

        /*public static DirectoryEntry Get(string volume)
        {
            return Get(GetBytes(volume));
        }*/

        private static DirectoryEntry Get(byte[] bytes, ref int offset)
        {
            return new DirectoryEntry(bytes, ref offset);
        }

        // Not complete
        public static DirectoryEntry[] GetInstances(string volume)
        {
            byte[] bytes = GetBytes(volume);

            int offset = 0;
            List<DirectoryEntry> list = new List<DirectoryEntry>();

            do
            {
                if(bytes[offset] == 0x00 || bytes[offset] == 0xE5)
                {
                    offset += 0x20;
                }
                else
                {
                    list.Add(Get(bytes, ref offset));
                }
            } while (offset < bytes.Length && bytes[offset] != 0);

            return list.ToArray();
        }

        // Not complete
        public static byte[] GetBytes(string volume)
        {
            Fat.VolumeBootRecord vbr = VolumeBootRecord.Get(volume) as Fat.VolumeBootRecord;
            ulong DirectoryEntryOffset = (ulong)(vbr.ReservedSectors * vbr.BytesPerSector) + vbr.TotalFats * (vbr.SectorsPerFat * vbr.BytesPerSector);
            return Helper.readDrive(volume, DirectoryEntryOffset, 0x1000);
        }

        private static DateTime GetFatDate(byte[] bytes, int dateoffset, int timeoffset, ushort mill, bool access)
        {
            /*
            Date Format. A FAT directory entry date stamp is a 16-bit field that is basically a date relative to the
            MS-DOS epoch of 01/01/1980. Here is the format (bit 0 is the LSB of the 16-bit word, bit 15 is the
            MSB of the 16-bit word):
                Bits 0–4: Day of month, valid value range 1-31 inclusive.
                Bits 5–8: Month of year, 1 = January, valid value range 1–12 inclusive.
                Bits 9–15: Count of years from 1980, valid value range 0–127 inclusive (1980–2107).
                
            Time Format. A FAT directory entry time stamp is a 16-bit field that has a granularity of 2 seconds.
            Here is the format (bit 0 is the LSB of the 16-bit word, bit 15 is the MSB of the 16-bit word).
                Bits 0–4: 2-second count, valid value range 0–29 inclusive (0 – 58 seconds).
                Bits 5–10: Minutes, valid value range 0–59 inclusive.
                Bits 11–15: Hours, valid value range 0–23 inclusive.
            
            The valid time range is from Midnight 00:00:00 to 23:59:58. 
            */

            #region date

            ushort date = BitConverter.ToUInt16(bytes, dateoffset);

            int year = ((date >> 9) & 127) + 1980;
            int month = (date >> 5) & 15;
            int day = date & 31;

            #endregion date

            #region time

            int hour = 0;
            int minute = 0;
            int second = 0;

            if (!(access))
            {
                ushort time = BitConverter.ToUInt16(bytes, timeoffset);

                hour = (time >> 11) & 31;
                minute = (time >> 5) & 63;
                second = (time & 31) * 2;
            }

            #endregion time

            if (year == 1980)
            {
                return new DateTime(1980, 1, 1);
            }
            else
            {
                return new DateTime(year, month, day, hour, minute, second, mill);
            }
        }

        private static string GetLongName(byte[] bytes, ref int offset)
        {
            int ord = bytes[0x00 + offset];
            StringBuilder sb = new StringBuilder();
            sb.Append(Encoding.Unicode.GetString(bytes, 0x01 + offset, 0x0A));
            sb.Append(Encoding.Unicode.GetString(bytes, 0x0E + offset, 0x0C));
            sb.Append(Encoding.Unicode.GetString(bytes, 0x1C + offset, 0x04));

            offset += 0x20;

            if (ord == 0x41)
            {
                return sb.ToString();
            }
            else if (ord == 0x01)
            {
                return sb.ToString();
            }
            else
            {
                return GetLongName(bytes, ref offset) + sb.ToString();
            }
        }

        #endregion StaticMethods

        #region InstanceMethods

        public byte[] GetContent()
        {
            return null;
        }

        public byte[] GetChildItem()
        {
            return null;
        }

        #endregion InstanceMethods
    }
}
