using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Ntfs;

namespace PowerForensics.Artifacts
{
    #region ScheduledJobClass

    // https://msdn.microsoft.com/en-us/library/cc248285.aspx
    public class ScheduledJob
    {
        #region Enums

        public enum PRODUCT_VERSION
        {
            WindowsNT4 = 0x0400,
            Windows2000 = 0x0500,
            WindowsXP = 0x0501,
            WindowsVista = 0x0600,
            Windows7 = 0x0601,
            Windows8 = 0x0602,
            Windows8_1 = 0x0603
        }

        [FlagsAttribute]
        public enum PRIORITY_CLASS
        {
            NORMAL = 0x4000000,
            IDLE = 0x2000000,
            HIGH = 0x1000000,
            REALTIME = 0x800000
        }

        public enum STATUS
        {
            SCHED_S_TASK_READY = 0x00041300,
            SCHED_S_TASK_RUNNING = 0x00041301,
            SCHED_S_TASK_NOT_SCHEDULED = 0x00041305
        }

        [FlagsAttribute]
        public enum TASK_FLAG : uint
        {
            INTERACTIVE = 0x80000000,
            DELETE_WHEN_DONE = 0x40000000,
            DISABLED = 0x20000000,
            START_ONLY_IF_IDLE = 0x8000000,
            KILL_ON_IDLE_END = 0x4000000,
            DONT_START_IF_ON_BATTERIES = 0x2000000,
            KILL_IF_GOING_ON_BATTERIES = 0x1000000,
            RUN_ONLY_IF_DOCKED = 0x800000,
            HIDDEN = 0x400000,
            RUN_IF_CONNECTED_TO_INTERNET = 0x200000,
            RESTART_ON_IDLE_RESUME = 0x100000,
            SYSTEM_REQUIRED = 0x80000,
            RUN_ONLY_IF_LOGGED_ON = 0x40000,
            APPLICATION_NAME = 0x80,
        }

        #endregion Enums

        #region Properties

        // FIXDLEN_DATA
        public readonly PRODUCT_VERSION ProductVersion;
        public readonly ushort FileVersion;
        public readonly Guid Uuid;
        private readonly ushort ApplicationNameOffset;
        private readonly ushort TriggerOffset;
        public readonly ushort ErrorRetryCount;
        public readonly ushort ErrorRetryInterval;
        public readonly ushort IdleDeadline;
        public readonly ushort IdleWait;
        //public readonly string Priority;
        public readonly uint MaximumRuntime;
        public readonly uint ExitCode;
        public readonly STATUS Status;
        public readonly TASK_FLAG Flags;
        public readonly DateTime RunTime;

        // Variable-Length Data Section
        public readonly ushort RunningInstanceCount;
        private readonly ushort ApplicationNameLength;
        public readonly string ApplicationName;
        private readonly ushort ParameterLength;
        public readonly string Parameters;
        private readonly ushort WorkingDirectoryLength;
        public readonly string WorkingDirectory;
        private readonly ushort AuthorLength;
        public readonly string Author;
        private readonly ushort CommentLength;
        public readonly string Comment;
        public readonly DateTime StartTime;

        #endregion Properties

        #region Constructors

        private ScheduledJob(byte[] bytes)
        {
            #region FIXDLEN_DATA

            ProductVersion = (PRODUCT_VERSION)BitConverter.ToUInt16(bytes, 0x00);
            FileVersion = BitConverter.ToUInt16(bytes, 0x02);
            Uuid = new Guid(BitConverter.ToInt32(bytes, 0x04), BitConverter.ToInt16(bytes, 0x08), BitConverter.ToInt16(bytes, 0x0A), Helper.GetSubArray(bytes, 0x0C, 0x08));
            ApplicationNameOffset = BitConverter.ToUInt16(bytes, 0x14);
            TriggerOffset = BitConverter.ToUInt16(bytes, 0x16);
            ErrorRetryCount = BitConverter.ToUInt16(bytes, 0x18);
            ErrorRetryInterval = BitConverter.ToUInt16(bytes, 0x1A);
            IdleDeadline = BitConverter.ToUInt16(bytes, 0x1C);
            IdleWait = BitConverter.ToUInt16(bytes, 0x1E);
            //Priority = (PRIORITY_CLASS)BitConverter.ToUInt32(bytes, 0x20);
            MaximumRuntime = BitConverter.ToUInt32(bytes, 0x24);
            ExitCode = BitConverter.ToUInt32(bytes, 0x28);
            Status = (STATUS)BitConverter.ToUInt32(bytes, 0x2C);
            Flags = (TASK_FLAG)BitConverter.ToUInt32(bytes, 0x30);
            #region RunTime

            short year = BitConverter.ToInt16(bytes, 0x34);
            short month = BitConverter.ToInt16(bytes, 0x36);
            short day = BitConverter.ToInt16(bytes, 0x3A);
            short hour = BitConverter.ToInt16(bytes, 0x3C);
            short minute = BitConverter.ToInt16(bytes, 0x3E);
            short second = BitConverter.ToInt16(bytes, 0x40);
            short milliseconds = BitConverter.ToInt16(bytes, 0x42);
            if (year != 0)
            {
                RunTime = new DateTime(year, month, day, hour, minute, second, milliseconds, DateTimeKind.Utc);
            }
            else
            {
                RunTime = new DateTime(0);
            }

            #endregion RunTime

            #endregion FIXDLEN_DATA

            #region Variable-Length Data Section

            RunningInstanceCount = BitConverter.ToUInt16(bytes, 0x44);
            ApplicationNameLength = BitConverter.ToUInt16(bytes, ApplicationNameOffset);
            ApplicationName = Encoding.Unicode.GetString(bytes, ApplicationNameOffset + 0x02, ApplicationNameLength * 0x02).Split('\0')[0];
            
            int parameterOffset = ApplicationNameOffset + 0x02 + (ApplicationNameLength * 2);
            ParameterLength = BitConverter.ToUInt16(bytes, parameterOffset);
            Parameters = Encoding.Unicode.GetString(bytes, parameterOffset, ParameterLength * 0x02).Split('\0')[0];

            int workingdirectoryOffset = parameterOffset + 0x02 + (ParameterLength * 2);
            WorkingDirectoryLength = BitConverter.ToUInt16(bytes, workingdirectoryOffset);
            WorkingDirectory = Encoding.Unicode.GetString(bytes, workingdirectoryOffset, WorkingDirectoryLength * 2).Split('\0')[0];

            int authorOffset = workingdirectoryOffset + 0x02 + (WorkingDirectoryLength * 2);
            AuthorLength = BitConverter.ToUInt16(bytes, authorOffset);
            Author = Encoding.Unicode.GetString(bytes, authorOffset, AuthorLength * 2).Split('\0')[0];

            int commentOffset = authorOffset + 0x02 + (AuthorLength * 2);
            CommentLength = BitConverter.ToUInt16(bytes, commentOffset);
            Comment = Encoding.Unicode.GetString(bytes, commentOffset, CommentLength * 2).Split('\0')[0];

            #region StartTime

            year = BitConverter.ToInt16(bytes, TriggerOffset + 0x06);
            month = BitConverter.ToInt16(bytes, TriggerOffset + 0x08);
            day = BitConverter.ToInt16(bytes, TriggerOffset + 0x0A);
            hour = BitConverter.ToInt16(bytes, TriggerOffset + 0x12);
            minute = BitConverter.ToInt16(bytes, TriggerOffset + 0x14);
            StartTime = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);

            #endregion StartTime

            #endregion Variable-Length Data Section
        }

        #endregion Constructors

        #region StaticMethods

        #region GetMethods

        public static ScheduledJob Get(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return new ScheduledJob(record.GetContent());
        }

        internal static ScheduledJob Get(string volume, int recordNumber)
        {
            Helper.getVolumeName(ref volume);
            FileRecord record = FileRecord.Get(volume, recordNumber, true);
            return new ScheduledJob(record.GetContent());
        }

        #endregion GetMethods

        #region GetInstancesMethods

        public static ScheduledJob[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            string path = Helper.GetVolumeLetter(volume) + @"\Windows\Tasks";
            return GetInstances(volume, path);
        }

        private static ScheduledJob[] GetInstances(string volume, string path)
        {
            List<ScheduledJob> jobList = new List<ScheduledJob>();

            foreach (IndexEntry entry in IndexEntry.GetInstances(path))
            {
                if (entry.Filename.Contains(".job"))
                {
                    jobList.Add(ScheduledJob.Get(volume, (int)entry.RecordNumber));
                }
            }

            return jobList.ToArray();
        }

        #endregion GetInstancesMethods

        #endregion StaticMethods

        public override string ToString()
        {
            return String.Format("[PROGRAM EXECUTION] {0} executed at {1} via Scheduled Job", this.ApplicationName, this.StartTime);
        }
    }

    #endregion ScheduledJobClass
}
