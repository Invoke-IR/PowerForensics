using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace PowerForensics
{
    internal static class NativeMethods
    {
        #region Constants

        internal const Int32 FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        internal const Int32 FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;
        internal const Int32 FILE_FLAG_NO_BUFFERING = 0x20000000;
        internal const Int32 FILE_FLAG_OPEN_NO_RECALL = 0x00100000;
        internal const Int32 FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
        internal const Int32 FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const Int32 FILE_FLAG_POSIX_SEMANTICS = 0x00100000;
        internal const Int32 FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        internal const Int32 FILE_FLAG_SESSION_AWARE = 0x00800000;

        internal const Int32 BYTES_PER_SECTOR = 0x200;

        #endregion Constants

        #region Enums

        [Flags]
        internal enum OpenFlags
        {
            O_RDONLY = 0x000,
            O_WRONLY = 0x001,
            O_RDWR = 0x002,
            O_CLOEXEC = 0x010,
            O_CREAT = 0x020,
            O_EXCL = 0x040,
            O_TRUNC = 0x080,
            O_SYNC = 0x100
        }

        #endregion Enums

        #region PInvoke

        [DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool ConvertSidToStringSid
            (
                [MarshalAs(UnmanagedType.LPArray)] byte[] pSID,
                out IntPtr ptrSid
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile
            (
                string fileName,
                [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
                [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                int flags,
                IntPtr template
            );

        [DllImport("System.Native", EntryPoint = "SystemNative_Open", SetLastError = true)]
        internal static extern SafeFileHandle Open
            (
                string filename,
                OpenFlags flags,
                int mode
            );

        #endregion PInvoke
    }
}

