using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PowerForensics
{
    internal static class NativeMethods
    {
        #region Constants

        internal const Int32 FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
        internal const Int32 FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
        internal const Int32 FILE_ATTRIBUTE_HIDDEN = 0x00000002;
        internal const Int32 FILE_ATTRIBUTE_NORMAL = 0x00000080;
        internal const Int32 FILE_ATTRIBUTE_OFFLINE = 0x00001000;
        internal const Int32 FILE_ATTRIBUTE_READONLY = 0x00000001;
        internal const Int32 FILE_ATTRIBUTE_SYSTEM = 0x00000004;
        internal const Int32 FILE_ATTRIBUTE_TEMPORARY = 0x00000100;

        internal const Int32 FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        internal const Int32 FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;
        internal const Int32 FILE_FLAG_NO_BUFFERING = 0x20000000;
        internal const Int32 FILE_FLAG_OPEN_NO_RECALL = 0x00100000;
        internal const Int32 FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
        internal const Int32 FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const Int32 FILE_FLAG_POSIX_SEMANTICS = 0x00100000;
        internal const Int32 FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        internal const Int32 FILE_FLAG_SESSION_AWARE = 0x00800000;

        internal const Int32 INVALID_HANDLE_VALUE = -1;

        internal const Int32 BYTES_PER_SECTOR = 0x200;

        #endregion Constants

        #region PInvoke

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CreateFile
            (
                string fileName,
                [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
                [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                int flags,
                IntPtr template
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle
            (
                IntPtr hObject
            );

        #endregion PInvoke
    }
}

