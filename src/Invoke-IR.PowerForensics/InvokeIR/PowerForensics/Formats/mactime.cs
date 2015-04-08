using System;
using System.Linq;
using System.Collections.Generic;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Formats
{

    #region mactimeClass

    public class Mactime
    {

        public enum ACTIVITY_TYPE
        {
            m = 0x8,
            a = 0x4,
            c = 0x2,
            b = 0x1
        }

        #region Properties

        public readonly DateTime DateTime;
        public readonly ulong Size;
        public readonly ushort ActivityType;
        //Permissions
        public readonly uint UserId;
        public readonly uint GroupId;
        public readonly uint Index;
        public readonly string FileName;

        #endregion Properties

        #region Constructors

        internal Mactime(DateTime dateTime, ulong size, ushort activityType, uint index, string fileName, bool deleted)
        {
            if (deleted)
            {
                fileName += " (deleted)";
            }
            
            DateTime = dateTime;
            ActivityType = activityType;
            Size = size;
            UserId = 0;
            GroupId = 0;
            Index = index;
            FileName = fileName;
        }

        #endregion Constructors

        public static Mactime[] Get(MFTRecord record)
        {
            #region DetermineTime

            Dictionary<DateTime, ACTIVITY_TYPE> dictionary = new Dictionary<DateTime, ACTIVITY_TYPE>();
            
            // Modified Time
            dictionary[record.ModifiedTime] = ACTIVITY_TYPE.m;

            // Access Time
            if (dictionary.ContainsKey(record.AccessedTime))
            {
                dictionary[record.AccessedTime] = dictionary[record.AccessedTime] | ACTIVITY_TYPE.a;
            }
            else
            {
                dictionary.Add(record.AccessedTime, ACTIVITY_TYPE.a);
            }

            // MFT Changed Time
            if (dictionary.ContainsKey(record.ChangedTime))
            {
                dictionary[record.ChangedTime] = dictionary[record.ChangedTime] | ACTIVITY_TYPE.c;
            }
            else
            {
                dictionary.Add(record.ChangedTime, ACTIVITY_TYPE.c);
            }

            // Born Time
            if (dictionary.ContainsKey(record.BornTime))
            {
                dictionary[record.BornTime] = dictionary[record.BornTime] | ACTIVITY_TYPE.b;
            }
            else
            {
                dictionary.Add(record.BornTime, ACTIVITY_TYPE.b);
            }

            #endregion DetermineTime

            List<Mactime> macs = new List<Mactime>();

            foreach (var time in dictionary)
            {
                macs.Add(new Mactime(time.Key, record.Size, (ushort)time.Value, record.RecordNumber, record.FullPath, record.Deleted));
            }

            return macs.ToArray();
        }

    }

    #endregion mactimeClass

}
