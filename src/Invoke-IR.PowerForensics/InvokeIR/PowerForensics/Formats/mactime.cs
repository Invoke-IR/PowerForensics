using System;
using System.Linq;
using System.Collections.Generic;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Formats
{

    public enum ACTIVITY_TYPE
    {
        m = 0x8,
        a = 0x4,
        c = 0x2,
        b = 0x1
    }

    public class mactime
    {

        public uint Index;
        public DateTime DateTime;
        public ushort ActivityType;
        public string FileName;

        internal mactime(uint index, DateTime dateTime, ushort activityType, string fileName)
        {
            Index = index;
            DateTime = dateTime;
            ActivityType = activityType;
            FileName = fileName;
        }

        public static List<mactime> Get(MFTRecord record)
        {
            // Modified Time
            Dictionary<DateTime, ACTIVITY_TYPE> dictionary = new Dictionary<DateTime, ACTIVITY_TYPE>();
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

            List<mactime> macs = new List<mactime>();

            //for (int i = 0; i < dictionary[record.RecordNumber].Count; i++)
            //{
            //   mactime mac = new mactime(record.RecordNumber, dictionary[record.RecordNumber].Keys[0],  )
            //}

            foreach (var time in dictionary)
            {
                macs.Add(new mactime(record.RecordNumber, time.Key, (ushort)time.Value, record.Name));
            }

            return macs;

        }

    
    }
}
