using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.Windows.Artifacts
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/cc248285.aspx
    /// </summary>
    public class ScheduledJob
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum PRODUCT_VERSION
        {
            /// <summary>
            /// 
            /// </summary>
            WindowsNT4 = 0x0400,

            /// <summary>
            /// 
            /// </summary>
            Windows2000 = 0x0500,

            /// <summary>
            /// 
            /// </summary>
            WindowsXP = 0x0501,

            /// <summary>
            /// 
            /// </summary>
            WindowsVista = 0x0600,

            /// <summary>
            /// 
            /// </summary>
            Windows7 = 0x0601,

            /// <summary>
            /// 
            /// </summary>
            Windows8 = 0x0602,

            /// <summary>
            /// 
            /// </summary>
            Windows8_1 = 0x0603
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum PRIORITY_CLASS
        {
            /// <summary>
            /// 
            /// </summary>
            NORMAL = 0x4000000,

            /// <summary>
            /// 
            /// </summary>
            IDLE = 0x2000000,

            /// <summary>
            /// 
            /// </summary>
            HIGH = 0x1000000,

            /// <summary>
            /// 
            /// </summary>
            REALTIME = 0x800000
        }

        /// <summary>
        /// 
        /// </summary>
        public enum STATUS
        {
            /// <summary>
            /// 
            /// </summary>
            SCHED_S_TASK_READY = 0x00041300,

            /// <summary>
            /// 
            /// </summary>
            SCHED_S_TASK_RUNNING = 0x00041301,

            /// <summary>
            /// 
            /// </summary>
            SCHED_S_TASK_NOT_SCHEDULED = 0x00041305
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum TASK_FLAG : uint
        {
            /// <summary>
            /// 
            /// </summary>
            INTERACTIVE = 0x80000000,

            /// <summary>
            /// 
            /// </summary>
            DELETE_WHEN_DONE = 0x40000000,

            /// <summary>
            /// 
            /// </summary>
            DISABLED = 0x20000000,

            /// <summary>
            /// 
            /// </summary>
            START_ONLY_IF_IDLE = 0x8000000,

            /// <summary>
            /// 
            /// </summary>
            KILL_ON_IDLE_END = 0x4000000,

            /// <summary>
            /// 
            /// </summary>
            DONT_START_IF_ON_BATTERIES = 0x2000000,

            /// <summary>
            /// 
            /// </summary>
            KILL_IF_GOING_ON_BATTERIES = 0x1000000,

            /// <summary>
            /// 
            /// </summary>
            RUN_ONLY_IF_DOCKED = 0x800000,

            /// <summary>
            /// 
            /// </summary>
            HIDDEN = 0x400000,

            /// <summary>
            /// 
            /// </summary>
            RUN_IF_CONNECTED_TO_INTERNET = 0x200000,

            /// <summary>
            /// 
            /// </summary>
            RESTART_ON_IDLE_RESUME = 0x100000,

            /// <summary>
            /// 
            /// </summary>
            SYSTEM_REQUIRED = 0x80000,

            /// <summary>
            /// 
            /// </summary>
            RUN_ONLY_IF_LOGGED_ON = 0x40000,

            /// <summary>
            /// 
            /// </summary>
            APPLICATION_NAME = 0x80,
        }

        #endregion Enums

        #region Properties

        // FIXDLEN_DATA

        /// <summary>
        /// 
        /// </summary>
        public readonly PRODUCT_VERSION ProductVersion;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort FileVersion;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid Uuid;

        private readonly ushort ApplicationNameOffset;

        private readonly ushort TriggerOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ErrorRetryCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ErrorRetryInterval;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort IdleDeadline;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort IdleWait;
        
        //public readonly string Priority;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint MaximumRuntime;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ExitCode;

        /// <summary>
        /// 
        /// </summary>
        public readonly STATUS Status;

        /// <summary>
        /// 
        /// </summary>
        public readonly TASK_FLAG Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime RunTime;

        // Variable-Length Data Section
        /// <summary>
        /// 
        /// </summary>
        public readonly ushort RunningInstanceCount;

        private readonly ushort ApplicationNameLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string ApplicationName;

        private readonly ushort ParameterLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Parameters;

        private readonly ushort WorkingDirectoryLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string WorkingDirectory;

        private readonly ushort AuthorLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Author;

        private readonly ushort CommentLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Comment;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
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

        #endregion Static Methods

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[PROGRAM EXECUTION] {0} executed at {1} via Scheduled Job", this.ApplicationName, this.StartTime);
        }

        #endregion Override Methods
    }
}
