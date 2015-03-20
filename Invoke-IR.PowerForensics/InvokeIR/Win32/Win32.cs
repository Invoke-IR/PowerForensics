using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace InvokeIR.Win32
{

    public static class NativeMethods
    {

        #region constants

        public const uint NTFS_VOLUME_DATA_BUFFER = 0x00090064;

        #endregion constants

        #region structs

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BY_HANDLE_FILE_INFORMATION
        {
            internal uint dwFileAttributes;
            internal DateTime ftCreationTime;
            internal DateTime ftLastAccessTime;
            internal DateTime ftLastWriteTime;
            internal uint dwVolumeSerialNumber;
            internal uint nFileSizeHigh;
            internal uint nFileSizeLow;
            internal uint nNumberOfLinks;
            internal uint nFileIndexHigh;
            internal uint nFileIndexLow;
        }

        #endregion structs

        #region PInvoke

        //function import
        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
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

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        internal static extern bool CloseHandle
            (
                IntPtr hObject
            );

        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        internal static extern bool GetFileInformationByHandle
            (
                IntPtr hFile,
                out BY_HANDLE_FILE_INFORMATION lpFileInformation
            );

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeviceIoControl
            (
                IntPtr fileHandle, 
                uint ioControlCode, 
                IntPtr inBuffer, 
                uint cbInBuffer, 
                IntPtr outBuffer, 
                uint cbOutBuffer, 
                out uint cbBytesReturned, 
                IntPtr overlapped
            );

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeviceIoControl
            (
                IntPtr hDevice,
                uint dwIoControlCode,
                [MarshalAs(UnmanagedType.AsAny)]
                [In] object InBuffer,
                int nInBufferSize,
                [MarshalAs(UnmanagedType.AsAny)]
                [Out] object OutBuffer,
                int nOutBufferSize,
                ref int lpBytesReturned,
                [In] IntPtr lpOverlapped
            );

        #endregion PInvoke

        public static IntPtr getHandle(string FileName)
        {

            // Get Handle to specified Volume/File/Directory
            IntPtr hDrive = CreateFile(
                fileName: FileName,
                fileAccess: FileAccess.Read,
                fileShare: FileShare.Write | FileShare.Read | FileShare.Delete,
                securityAttributes: IntPtr.Zero,
                creationDisposition: FileMode.Open,
                flags: 0x02000000, //with this also an enum can be used. (as described above as EFileAttributes)
                template: IntPtr.Zero);

            return hDrive;

        }

        public static FileStream getFileStream(IntPtr hVolume)
        {
            // Return a FileStream to read from the specified handle
            return new FileStream(hVolume, FileAccess.Read);
        }

        internal static byte[] readDrive(FileStream streamToRead, long offset, long sizeToRead)
        {

            // Bytes must be read by sector
            if ((sizeToRead < 1)) throw new System.ArgumentException("Size parameter cannot be null or 0 or less than 0!");
            if (((sizeToRead % 512) != 0)) throw new System.ArgumentException("Size parameter must be divisible by 512");
            if (((offset % 512) != 0)) throw new System.ArgumentException("Offset parameter must be divisible by 512");

            // Set offset to begin reading from the drive
            streamToRead.Position = offset;
            // Create a byte array to read into
            byte[] buf = new byte[sizeToRead];
            // Read buf.Length bytes (sizeToRead) from offset 
            streamToRead.Read(buf, 0, buf.Length);

            return buf;
        }

    }

}

