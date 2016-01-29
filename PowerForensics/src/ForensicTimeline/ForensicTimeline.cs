using System;
using System.Text;
using System.Management.Automation;
using System.Collections.Generic;
using PowerForensics.Artifacts;
using PowerForensics.Ntfs;
using PowerForensics.EventLog;
using PowerForensics.Registry;

namespace PowerForensics.Formats
{
    #region ForensicTimelineClass

    public class ForensicTimeline
    {
        #region Enums

        [FlagsAttribute]
        public enum ACTIVITY_TYPE
        {
            m = 0x1,
            a = 0x2,
            c = 0x4,
            b = 0x8
        }

        #endregion Enums

        #region Properties

        public readonly DateTime Date;
        public readonly string ActivityType;
        public readonly string Source;
        public readonly string SourceType;
        public readonly string User;
        public readonly string FileName;
        public readonly string Description;

        #endregion Properties

        #region Constructors

        private ForensicTimeline(DateTime date, string activity, string source, string user, string fileName, string description)
        {
            Date = date;
            ActivityType = activity;
            Source = source;
            User = user;
            FileName = fileName;
            Description = description;
        }

        #endregion Constructors

        #region StaticMethods

        /*public static ForensicTimeline Get(PSObject input)
        {
            switch (input.TypeNames[0])
            {
                case "PowerForensics.Artifacts.Amcache":
                    break;
                case "PowerForensics.Artifacts.Prefetch":
                    //return Get(input.BaseObject as Prefetch);
                    break;
                case "PowerForensics.Artifacts.ScheduledJob":
                    return Get(input.BaseObject as ScheduledJob);
                    break;
                case "PowerForensics.Artifacts.UserAssist":
                    return Get(input.BaseObject as UserAssist);
                    break;
                case "PowerForensics.Artifacts.ShellLink":
                    //return Get(input.BaseObject as ShellLink);
                    break;
                case "PowerForensics.Ntfs.FileRecord":
                    try
                    {
                        //return Get(input.BaseObject as FileRecord);
                    }
                    catch
                    {

                    }
                    break;
                case "PowerForensics.Ntfs.UsnJrnl":
                    return Get(input.BaseObject as UsnJrnl);
                    break;
                case "PowerForensics.EventLog.EventRecord":
                    return Get(input.BaseObject as EventRecord);
                    break;
                case "PowerForensics.Registry.NamedKey":
                    return Get(input.BaseObject as NamedKey);
                    break;
                default:
                    Console.WriteLine(input.TypeNames[0]);
                    break;
            }
        }*/

        public static ForensicTimeline Get(Amcache input)
        {
            return null;
        }

        public static ForensicTimeline[] GetInstances(Amcache[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (Amcache a in input)
            {
                list.Add(Get(a));
            }
            return list.ToArray();
        }

        public static ForensicTimeline[] Get(Prefetch input)
        {
            List<ForensicTimeline> mactimeList = new List<ForensicTimeline>();

            foreach (DateTime time in input.PrefetchAccessTime)
            {
                mactimeList.Add(new ForensicTimeline(time, "MACB", "PREFETCH", "", input.Path, input.ToString()));
            }

            return mactimeList.ToArray();
        }

        public static ForensicTimeline[] GetInstances(Prefetch[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (Prefetch pf in input)
            {
                foreach (ForensicTimeline t in Get(pf))
                {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }

        public static ForensicTimeline Get(ScheduledJob input)
        {
            return new ForensicTimeline(input.StartTime, "MACB", "SCHEDULEDJOB", input.Author, input.ApplicationName, input.ToString());
        }

        public static ForensicTimeline[] GetInstances(ScheduledJob[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (ScheduledJob s in input)
            {
                list.Add(Get(s));
            }
            return list.ToArray();
        }

        public static ForensicTimeline[] Get(ShellLink input)
        {
            List<ForensicTimeline> macs = new List<ForensicTimeline>();

            #region DetermineTime

            Dictionary<DateTime, ACTIVITY_TYPE> dictionary = new Dictionary<DateTime, ACTIVITY_TYPE>();

            // Creation Time
            dictionary[input.CreationTime] = ACTIVITY_TYPE.b;

            // Access Time
            if (dictionary.ContainsKey(input.AccessTime))
            {
                dictionary[input.AccessTime] = dictionary[input.AccessTime] | ACTIVITY_TYPE.a;
            }
            else
            {
                dictionary.Add(input.AccessTime, ACTIVITY_TYPE.a);
            }

            // Modified Time
            if (dictionary.ContainsKey(input.WriteTime))
            {
                dictionary[input.WriteTime] = dictionary[input.WriteTime] | ACTIVITY_TYPE.m;
            }
            else
            {
                dictionary.Add(input.WriteTime, ACTIVITY_TYPE.m);
            }

            #endregion DetermineTime

            foreach (var time in dictionary)
            {
                string activity = ToFriendlyString(time.Value);
                macs.Add(new ForensicTimeline(time.Key, activity, "ShellLink", "", input.LocalBasePath, input.ToString()));
            }

            return macs.ToArray();
        }

        public static ForensicTimeline[] GetInstances(ShellLink[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (ShellLink s in input)
            {
                foreach (ForensicTimeline t in Get(s))
                {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }

        public static ForensicTimeline Get(UserAssist input)
        {
            return new ForensicTimeline(input.LastExecutionTimeUtc, "MACB", "USERASSIST", "", input.ImagePath, input.ToString());
        }

        public static ForensicTimeline[] GetInstances(UserAssist[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (UserAssist u in input)
            {
                list.Add(Get(u));
            }
            return list.ToArray();
        }

        public static ForensicTimeline Get(EventRecord input)
        {
            return new ForensicTimeline(input.WriteTime, "MACB", "EVENTLOG", "", input.LogPath, input.ToString());
        }

        public static ForensicTimeline[] GetInstances(EventRecord[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (EventRecord er in input)
            {
                list.Add(Get(er));
            }
            return list.ToArray();
        }

        public static ForensicTimeline[] Get(FileRecord input)
        {
            List<ForensicTimeline> macs = new List<ForensicTimeline>();
            if (input.SequenceNumber != 0)
            {
                #region DetermineTime

                Dictionary<DateTime, ACTIVITY_TYPE> dictionary = new Dictionary<DateTime, ACTIVITY_TYPE>();

                // Modified Time
                dictionary[input.ModifiedTime] = ACTIVITY_TYPE.m;

                // Access Time
                if (dictionary.ContainsKey(input.AccessedTime))
                {
                    dictionary[input.AccessedTime] = dictionary[input.AccessedTime] | ACTIVITY_TYPE.a;
                }
                else
                {
                    dictionary.Add(input.AccessedTime, ACTIVITY_TYPE.a);
                }

                // MFT Changed Time
                if (dictionary.ContainsKey(input.ChangedTime))
                {
                    dictionary[input.ChangedTime] = dictionary[input.ChangedTime] | ACTIVITY_TYPE.c;
                }
                else
                {
                    dictionary.Add(input.ChangedTime, ACTIVITY_TYPE.c);
                }

                // Born Time
                if (dictionary.ContainsKey(input.BornTime))
                {
                    dictionary[input.BornTime] = dictionary[input.BornTime] | ACTIVITY_TYPE.b;
                }
                else
                {
                    dictionary.Add(input.BornTime, ACTIVITY_TYPE.b);
                }

                #endregion DetermineTime

                foreach (var time in dictionary)
                {
                    string activity = ToFriendlyString(time.Value);
                    macs.Add(new ForensicTimeline(time.Key, activity, "MFT", "", input.FullName, input.ToString()));
                }

                return macs.ToArray();
            }
            else
            {
                macs.Add(new ForensicTimeline(new DateTime(1), "MACB", "MFT", "", "", ""));
                return macs.ToArray();
            }
        }

        public static ForensicTimeline[] GetInstances(FileRecord[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (FileRecord r in input)
            {
                foreach (ForensicTimeline t in Get(r))
                {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }

        public static ForensicTimeline Get(UsnJrnl input)
        {
            return new ForensicTimeline(input.TimeStamp, "MACB", "USNJRNL", "", input.FileName, input.ToString());
        }

        public static ForensicTimeline[] GetInstances(UsnJrnl[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (UsnJrnl u in input)
            {
                list.Add(Get(u));
            } 
            return list.ToArray();
        }

        public static ForensicTimeline Get(NamedKey input)
        {
            return new ForensicTimeline(input.WriteTime, "MACB", "REGISTRY", "", input.FullName, input.ToString());
        }

        public static ForensicTimeline[] GetInstances(NamedKey[] input)
        {
            List<ForensicTimeline> list = new List<ForensicTimeline>();
            foreach (NamedKey nk in input)
            {
                list.Add(Get(nk));
            }
            return list.ToArray();
        }

        public static string ToFriendlyString(ACTIVITY_TYPE type)
        {
            StringBuilder sb = new StringBuilder();

            if ((type & ACTIVITY_TYPE.m) == ACTIVITY_TYPE.m)
            {
                sb.Append('M');
            }
            else
            {
                sb.Append('.');
            }
            if ((type & ACTIVITY_TYPE.a) == ACTIVITY_TYPE.a)
            {
                sb.Append('A');
            }
            else
            {
                sb.Append('.');
            }
            if ((type & ACTIVITY_TYPE.c) == ACTIVITY_TYPE.c)
            {
                sb.Append('C');
            }
            else
            {
                sb.Append('.');
            }
            if ((type & ACTIVITY_TYPE.b) == ACTIVITY_TYPE.b)
            {
                sb.Append('B');
            }
            else
            {
                sb.Append('.');
            }
            return sb.ToString();
        }

        #endregion StaticMethods
    }

    #endregion ForensicTimelineClass
}
