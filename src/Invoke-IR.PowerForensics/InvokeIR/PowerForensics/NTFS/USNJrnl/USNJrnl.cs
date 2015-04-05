using System;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{
    #region USNJrnlClass

    class USNJrnl
    {
        private enum USN_REASON : uint
        {
            DATA_OVERWRITE = 0x00000001,
            DATA_EXTENDED = 0x00000002,
            DATA_TRUNCATION = 0x00000004,
            NAMED_DATA_OVERWRITE = 0x00000010,
            NAMED_DATA_EXTEND = 0x00000020,
            NAMED_DATA_TRUNCATION = 0x00000040,
            FILE_CREATE = 0x00000100,
            FILE_DELETE = 0x00000200,
            EA_CHANGE = 0x00000400,
            SECURITY_CHANGE = 0x00000800,
            RENAME_OLD_NAME = 0x00001000,
            RENAME_NEW_NAME = 0x00002000,
            INDEXABLE_CHANGE = 0x00004000,
            BASIC_INFO_CHANGE = 0x00008000,
            HARD_LINK_CHANGE = 0x00010000,
            COMPRESSION_CHANGE = 0x00020000,
            ENCRYPTION_CHANGE = 0x00040000,
            OBJECT_ID_CHANGE = 0x00080000,
            REPARSE_POINT_CHANGE = 0x00100000,
            STREAM_CHANGE = 0x00200000,
            CLOSE = 0x80000000
        }

        private enum USN_SOURCE : uint
        {
            DATA_MANAGEMENT = 0x00000001,
            AUXILIARY_DATA = 0x00000002,
            REPLICATION_MANAGEMENT = 0x00000004
        }

        struct USN_RECORD
        {
            internal uint RecordLength;
            internal ushort MajorVersion;
            internal ushort MinorVersion;
            internal ulong FileReferenceNumber;
            internal ulong ParentFileReferenceNumber;
            internal ulong Usn;
            internal long TimeStamp;
            internal uint Reason;
            internal uint SourceInfo;
            internal uint SecurityId;
            internal uint FileAttributes;
            internal ushort FileNameLength;
            internal ushort FileNameOffset;
            internal byte[] FileName;

            internal USN_RECORD(byte[] bytes)
            {
                RecordLength = BitConverter.ToUInt32(bytes, 0);
                MajorVersion = BitConverter.ToUInt16(bytes, 4);
                MinorVersion = BitConverter.ToUInt16(bytes, 6);
                FileReferenceNumber = BitConverter.ToUInt64(bytes, 8);
                ParentFileReferenceNumber = BitConverter.ToUInt64(bytes, 16);
                Usn = BitConverter.ToUInt64(bytes, 24);
                TimeStamp = BitConverter.ToInt64(bytes, 32);
                Reason = BitConverter.ToUInt32(bytes, 40);
                SourceInfo = BitConverter.ToUInt32(bytes, 44);
                SecurityId = BitConverter.ToUInt32(bytes, 48);
                FileAttributes = BitConverter.ToUInt32(bytes, 52);
                FileNameLength = BitConverter.ToUInt16(bytes, 56);
                FileNameOffset = BitConverter.ToUInt16(bytes, 58);
                FileName = bytes.Skip(60).Take(FileNameLength).ToArray();
            }
        }

        #region Properties

        public ulong FileReferenceNumber;
        public ulong ParentFileReferenceNumber;
        public ulong Usn;
        public DateTime TimeStamp;
        public string Reason;
        public string SourceInfo;
        public uint SecurityId;
        public uint FileAttributes;
        public string FileName;

        #endregion Properties

        #region Constructors

        internal USNJrnl(byte[] bytes)
        {
            USN_RECORD structUSN = new USN_RECORD(bytes);

            #region UnmaskReason

            string reason = null;

            if ((structUSN.Reason & (uint)USN_REASON.BASIC_INFO_CHANGE) == (uint)USN_REASON.BASIC_INFO_CHANGE)
            {
                reason = "Basic Info Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.CLOSE) == (uint)USN_REASON.CLOSE)
            {
                reason = "Close";
            }
            if ((structUSN.Reason & (uint)USN_REASON.COMPRESSION_CHANGE) == (uint)USN_REASON.COMPRESSION_CHANGE)
            {
                reason = "Compression Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.DATA_EXTENDED) == (uint)USN_REASON.DATA_EXTENDED)
            {
                reason = "Data Extended";
            }
            if ((structUSN.Reason & (uint)USN_REASON.DATA_OVERWRITE) == (uint)USN_REASON.DATA_OVERWRITE)
            {
                reason = "Data Overwrite";
            }
            if ((structUSN.Reason & (uint)USN_REASON.DATA_TRUNCATION) == (uint)USN_REASON.DATA_TRUNCATION)
            {
                reason = "Data Truncation";
            }
            if ((structUSN.Reason & (uint)USN_REASON.EA_CHANGE) == (uint)USN_REASON.EA_CHANGE)
            {
                reason = "EA Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.ENCRYPTION_CHANGE) == (uint)USN_REASON.ENCRYPTION_CHANGE)
            {
                reason = "Encryption Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.FILE_CREATE) == (uint)USN_REASON.FILE_CREATE)
            {
                reason = "File Create";
            }
            if ((structUSN.Reason & (uint)USN_REASON.FILE_DELETE) == (uint)USN_REASON.FILE_DELETE)
            {
                reason = "File Delete";
            }
            if ((structUSN.Reason & (uint)USN_REASON.HARD_LINK_CHANGE) == (uint)USN_REASON.HARD_LINK_CHANGE)
            {
                reason = "Hard Link Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.INDEXABLE_CHANGE) == (uint)USN_REASON.INDEXABLE_CHANGE)
            {
                reason = "Indexable Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.NAMED_DATA_EXTEND) == (uint)USN_REASON.NAMED_DATA_EXTEND)
            {
                reason = "Named Data Extend";
            }
            if ((structUSN.Reason & (uint)USN_REASON.NAMED_DATA_OVERWRITE) == (uint)USN_REASON.NAMED_DATA_OVERWRITE)
            {
                reason = "Named Data Overwrite";
            }
            if ((structUSN.Reason & (uint)USN_REASON.NAMED_DATA_TRUNCATION) == (uint)USN_REASON.NAMED_DATA_TRUNCATION)
            {
                reason = "Named Data Truncation";
            }
            if ((structUSN.Reason & (uint)USN_REASON.OBJECT_ID_CHANGE) == (uint)USN_REASON.OBJECT_ID_CHANGE)
            {
                reason = "Object ID Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.RENAME_NEW_NAME) == (uint)USN_REASON.RENAME_NEW_NAME)
            {
                reason = "Rename: New Name";
            }
            if ((structUSN.Reason & (uint)USN_REASON.RENAME_OLD_NAME) == (uint)USN_REASON.RENAME_OLD_NAME)
            {
                reason = "Rename: Old Name";
            }
            if ((structUSN.Reason & (uint)USN_REASON.REPARSE_POINT_CHANGE) == (uint)USN_REASON.REPARSE_POINT_CHANGE)
            {
                reason = "Reparse Point Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.SECURITY_CHANGE) == (uint)USN_REASON.SECURITY_CHANGE)
            {
                reason = "Security Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON.STREAM_CHANGE) == (uint)USN_REASON.STREAM_CHANGE)
            {
                reason = "Stream Change";
            }

            #endregion UnmaskReason

            #region UnmaskSourceInfo

            string sourceInfo = null;

            if ((structUSN.SourceInfo & (uint)USN_SOURCE.AUXILIARY_DATA) == (uint)USN_SOURCE.AUXILIARY_DATA)
            {
                sourceInfo = "Auxiliary Data";
            }
            if ((structUSN.SourceInfo & (uint)USN_SOURCE.DATA_MANAGEMENT) == (uint)USN_SOURCE.DATA_MANAGEMENT)
            {
                sourceInfo = "Data Management";
            }
            if ((structUSN.SourceInfo & (uint)USN_SOURCE.REPLICATION_MANAGEMENT) == (uint)USN_SOURCE.REPLICATION_MANAGEMENT)
            {
                sourceInfo = "Replication Management";
            }

            #endregion UnmaskSourceInfo

            FileReferenceNumber = structUSN.FileReferenceNumber;
            ParentFileReferenceNumber = structUSN.ParentFileReferenceNumber;
            Usn = structUSN.Usn;
            TimeStamp = DateTime.FromFileTime(structUSN.TimeStamp);
            Reason = reason;
            SourceInfo = sourceInfo;
            SecurityId = structUSN.SecurityId;
            FileAttributes = structUSN.FileAttributes;
            FileName = Encoding.Unicode.GetString(structUSN.FileName);
        }

        #endregion Constructors

    }

    #endregion USNJrnlClass
}
