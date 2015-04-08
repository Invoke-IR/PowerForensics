using System;
using System.Linq;
using System.Text;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{
    #region USNJrnlClass

    public class USNJrnl
    {

        #region Enums

        private enum USN_JOURNAL_RETURN_CODE
        {
            INVALID_HANDLE_VALUE = -1,
            USN_JOURNAL_SUCCESS = 0x00000000,
            ERROR_INVALID_FUNCTION = 0x00000001,
            ERROR_FILE_NOT_FOUND = 0x00000002,
            ERROR_PATH_NOT_FOUND = 0x00000003,
            ERROR_TOO_MANY_OPEN_FILES = 0x00000004,
            ERROR_ACCESS_DENIED = 0x00000005,
            ERROR_INVALID_HANDLE = 0x00000006,
            ERROR_INVALID_DATA = 0x0000000D,
            ERROR_HANDLE_EOF = 0x00000026,
            ERROR_NOT_SUPPORTED = 0x00000032,
            ERROR_INVALID_PARAMETER = 0x00000057,
            ERROR_JOURNAL_DELETE_IN_PROGRESS = 0x0000049A,
            USN_JOURNAL_NOT_ACTIVE = 0x0000049B,
            ERROR_JOURNAL_ENTRY_DELETED = 0x0000049D,
            ERROR_INVALID_USER_BUFFER = 0x000006F8,
            USN_JOURNAL_INVALID = 0x00004269,
            VOLUME_NOT_NTFS = 0x0000426B,
            INVALID_FILE_REFERENCE_NUMBER = 0x0000426C,
            USN_JOURNAL_ERROR = 0x0000426D
        }

        //
        //  Flags for the additional source information above.
        //
        //      USN_SOURCE_DATA_MANAGEMENT - Service is not modifying the external view
        //          of any part of the file.  Typical case is HSM moving data to
        //          and from external storage.
        //
        //      USN_SOURCE_AUXILIARY_DATA - Service is not modifying the external view
        //          of the file with regard to the application that created this file.
        //          Can be used to add private data streams to a file.
        //
        //      USN_SOURCE_REPLICATION_MANAGEMENT - Service is modifying a file to match
        //          the contents of the same file which exists in another member of the
        //          replica set.
        //
        private enum USN_SOURCE : uint
        {
            DATA_MANAGEMENT = 0x00000001,
            AUXILIARY_DATA = 0x00000002,
            REPLICATION_MANAGEMENT = 0x00000004
        }

        #endregion Enums

        #region Constants

        private const int FR_OFFSET = 8;
        private const int PFR_OFFSET = 16;
        private const int USN_OFFSET = 24;
        private const int REASON_OFFSET = 40;
        public const int FA_OFFSET = 52;
        private const int FNL_OFFSET = 56;
        private const int FN_OFFSET = 58;

        public const UInt32 USN_REASON_DATA_OVERWRITE = 0x00000001;
        public const UInt32 USN_REASON_DATA_EXTEND = 0x00000002;
        public const UInt32 USN_REASON_DATA_TRUNCATION = 0x00000004;
        public const UInt32 USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010;
        public const UInt32 USN_REASON_NAMED_DATA_EXTEND = 0x00000020;
        public const UInt32 USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040;
        public const UInt32 USN_REASON_FILE_CREATE = 0x00000100;
        public const UInt32 USN_REASON_FILE_DELETE = 0x00000200;
        public const UInt32 USN_REASON_EA_CHANGE = 0x00000400;
        public const UInt32 USN_REASON_SECURITY_CHANGE = 0x00000800;
        public const UInt32 USN_REASON_RENAME_OLD_NAME = 0x00001000;
        public const UInt32 USN_REASON_RENAME_NEW_NAME = 0x00002000;
        public const UInt32 USN_REASON_INDEXABLE_CHANGE = 0x00004000;
        public const UInt32 USN_REASON_BASIC_INFO_CHANGE = 0x00008000;
        public const UInt32 USN_REASON_HARD_LINK_CHANGE = 0x00010000;
        public const UInt32 USN_REASON_COMPRESSION_CHANGE = 0x00020000;
        public const UInt32 USN_REASON_ENCRYPTION_CHANGE = 0x00040000;
        public const UInt32 USN_REASON_OBJECT_ID_CHANGE = 0x00080000;
        public const UInt32 USN_REASON_REPARSE_POINT_CHANGE = 0x00100000;
        public const UInt32 USN_REASON_STREAM_CHANGE = 0x00200000;
        public const UInt32 USN_REASON_CLOSE = 0x80000000;

        #endregion Constants

        struct USN_JOURNAL_DATA
        {
            internal ulong UsnJournalID;
            internal long FirstUsn;
            internal long NextUsn;
            internal long LowestValidUsn;
            internal long MaxUsn;
            internal ulong MaximumSize;
            internal ulong AllocationDelta;

            internal USN_JOURNAL_DATA(byte[] bytes)
            {
                UsnJournalID = BitConverter.ToUInt64(bytes, 0);
                FirstUsn = BitConverter.ToInt64(bytes, 8);
                NextUsn = BitConverter.ToInt64(bytes, 16);
                LowestValidUsn = BitConverter.ToInt64(bytes, 24);
                MaxUsn = BitConverter.ToInt64(bytes, 32);
                MaximumSize = BitConverter.ToUInt64(bytes, 40);
                AllocationDelta = BitConverter.ToUInt64(bytes, 48);
            }
        }

        struct READ_USN_JOURNAL_DATA
        {
            internal long StartUsn;
            internal uint ReasonMask;
            internal uint ReturnOnlyOnClose;
            internal ulong Timeout;
            internal ulong bytesToWaitFor;
            internal ulong UsnJournalId;

            internal READ_USN_JOURNAL_DATA(byte[] bytes)
            {
                StartUsn = BitConverter.ToInt64(bytes, 0);
                ReasonMask = BitConverter.ToUInt32(bytes, 8);
                ReturnOnlyOnClose = BitConverter.ToUInt32(bytes, 12);
                Timeout = BitConverter.ToUInt64(bytes, 16);
                bytesToWaitFor = BitConverter.ToUInt64(bytes, 24);
                UsnJournalId = BitConverter.ToUInt64(bytes, 32);
            }
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

        public readonly ulong FileReferenceNumber;
        public readonly ulong ParentFileReferenceNumber;
        public readonly ulong Usn;
        public readonly DateTime TimeStamp;
        public readonly string Reason;
        public readonly string SourceInfo;
        public readonly uint SecurityId;
        public readonly uint FileAttributes;
        public readonly string FileName;

        #endregion Properties

        #region Constructors

        internal USNJrnl(byte[] bytes)
        {
            USN_RECORD structUSN = new USN_RECORD(bytes);

            #region UnmaskReason

            string reason = null;

            if ((structUSN.Reason & (uint)USN_REASON_BASIC_INFO_CHANGE) == (uint)USN_REASON_BASIC_INFO_CHANGE)
            {
                reason = "Basic Info Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_CLOSE) == (uint)USN_REASON_CLOSE)
            {
                reason = "Close";
            }
            if ((structUSN.Reason & (uint)USN_REASON_COMPRESSION_CHANGE) == (uint)USN_REASON_COMPRESSION_CHANGE)
            {
                reason = "Compression Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_DATA_EXTEND) == (uint)USN_REASON_DATA_EXTEND)
            {
                reason = "Data Extended";
            }
            if ((structUSN.Reason & (uint)USN_REASON_DATA_OVERWRITE) == (uint)USN_REASON_DATA_OVERWRITE)
            {
                reason = "Data Overwrite";
            }
            if ((structUSN.Reason & (uint)USN_REASON_DATA_TRUNCATION) == (uint)USN_REASON_DATA_TRUNCATION)
            {
                reason = "Data Truncation";
            }
            if ((structUSN.Reason & (uint)USN_REASON_EA_CHANGE) == (uint)USN_REASON_EA_CHANGE)
            {
                reason = "EA Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_ENCRYPTION_CHANGE) == (uint)USN_REASON_ENCRYPTION_CHANGE)
            {
                reason = "Encryption Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_FILE_CREATE) == (uint)USN_REASON_FILE_CREATE)
            {
                reason = "File Create";
            }
            if ((structUSN.Reason & (uint)USN_REASON_FILE_DELETE) == (uint)USN_REASON_FILE_DELETE)
            {
                reason = "File Delete";
            }
            if ((structUSN.Reason & (uint)USN_REASON_HARD_LINK_CHANGE) == (uint)USN_REASON_HARD_LINK_CHANGE)
            {
                reason = "Hard Link Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_INDEXABLE_CHANGE) == (uint)USN_REASON_INDEXABLE_CHANGE)
            {
                reason = "Indexable Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_NAMED_DATA_EXTEND) == (uint)USN_REASON_NAMED_DATA_EXTEND)
            {
                reason = "Named Data Extend";
            }
            if ((structUSN.Reason & (uint)USN_REASON_NAMED_DATA_OVERWRITE) == (uint)USN_REASON_NAMED_DATA_OVERWRITE)
            {
                reason = "Named Data Overwrite";
            }
            if ((structUSN.Reason & (uint)USN_REASON_NAMED_DATA_TRUNCATION) == (uint)USN_REASON_NAMED_DATA_TRUNCATION)
            {
                reason = "Named Data Truncation";
            }
            if ((structUSN.Reason & (uint)USN_REASON_OBJECT_ID_CHANGE) == (uint)USN_REASON_OBJECT_ID_CHANGE)
            {
                reason = "Object ID Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_RENAME_NEW_NAME) == (uint)USN_REASON_RENAME_NEW_NAME)
            {
                reason = "Rename: New Name";
            }
            if ((structUSN.Reason & (uint)USN_REASON_RENAME_OLD_NAME) == (uint)USN_REASON_RENAME_OLD_NAME)
            {
                reason = "Rename: Old Name";
            }
            if ((structUSN.Reason & (uint)USN_REASON_REPARSE_POINT_CHANGE) == (uint)USN_REASON_REPARSE_POINT_CHANGE)
            {
                reason = "Reparse Point Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_SECURITY_CHANGE) == (uint)USN_REASON_SECURITY_CHANGE)
            {
                reason = "Security Change";
            }
            if ((structUSN.Reason & (uint)USN_REASON_STREAM_CHANGE) == (uint)USN_REASON_STREAM_CHANGE)
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

        private static byte[] GetFSCTL(IntPtr hVolume, uint size, uint dwIoControlCode)
        {
            // Create a byte array the size of the  struct
            byte[] structbytes = new byte[size];

            // Instatiate an integer to accept the amount of bytes read
            int buf = new int();

            NativeMethods.DeviceIoControl(
                hDevice: hVolume,
                dwIoControlCode: dwIoControlCode,
                InBuffer: null,
                nInBufferSize: 0,
                OutBuffer: structbytes,
                nOutBufferSize: structbytes.Length,
                lpBytesReturned: ref buf,
                lpOverlapped: IntPtr.Zero);

            return structbytes;
        }

        private static USN_JOURNAL_DATA QueryUSNJournal(IntPtr hVolume)
        {   
            return new USN_JOURNAL_DATA(GetFSCTL(hVolume, 56, WinIoCtl.FSCTL_QUERY_USN_JOURNAL));
        }

        private static READ_USN_JOURNAL_DATA ReadUSNJournalData(IntPtr hVolume)
        {
            return new READ_USN_JOURNAL_DATA(GetFSCTL(hVolume, 40, WinIoCtl.FSCTL_READ_USN_JOURNAL));
        }

    }

    #endregion USNJrnlClass
}
