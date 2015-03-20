using System;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace InvokeIR.Win32{
    
    public static class NativeMethods{

        [StructLayout(LayoutKind.Sequential)]
        public struct BY_HANDLE_FILE_INFORMATION
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

        internal class NTFS_VOLUME_DATA_BUFFER
        {

            internal ulong VolumeSerialNumber;
            internal long NumberSectors;
            internal long TotalClusters;
            internal long FreeClusters;
            internal long TotalReserved;
            internal int BytesPerSector;
            internal int BytesPerCluster;
            internal int BytesPerFileRecordSegment;
            internal int ClustersPerFileRecordSegment;
            internal long MftValidDataLength;
            internal long MftStartLcn;
            internal long Mft2StartLcn;
            internal long MftZoneStart;
            internal long MftZoneEnd;

            internal NTFS_VOLUME_DATA_BUFFER(byte[] bytes)
            {
                VolumeSerialNumber = BitConverter.ToUInt64(bytes, 0);
                NumberSectors = BitConverter.ToInt64(bytes, 8);
                TotalClusters = BitConverter.ToInt64(bytes, 16);
                FreeClusters = BitConverter.ToInt64(bytes, 24);
                TotalReserved = BitConverter.ToInt64(bytes, 32);
                BytesPerSector = BitConverter.ToInt32(bytes, 40);
                BytesPerCluster = BitConverter.ToInt32(bytes, 44);
                BytesPerFileRecordSegment = BitConverter.ToInt32(bytes, 48);
                ClustersPerFileRecordSegment = BitConverter.ToInt32(bytes, 52);
                MftValidDataLength = BitConverter.ToInt64(bytes, 56);
                MftStartLcn = BitConverter.ToInt64(bytes, 64);
                Mft2StartLcn = BitConverter.ToInt64(bytes, 72);
                MftZoneStart = BitConverter.ToInt64(bytes, 80);
                MftZoneEnd = BitConverter.ToInt64(bytes, 88);
            }

            internal static NTFS_VOLUME_DATA_BUFFER Get(IntPtr hDrive)
            {

                // Create a byte array the size of the NTFS_VOLUME_DATA_BUFFER struct
                byte[] ntfsVolData = new byte[96];
                // Instatiate an integer to accept the amount of bytes read
                int buf = new int();

                // Return the NTFS_VOLUME_DATA_BUFFER struct
                var status = InvokeIR.PowerForensics.Win32.DeviceIoControl(
                    hDevice: hDrive,
                    dwIoControlCode: 0x00090064,
                    InBuffer: null,
                    nInBufferSize: 0,
                    OutBuffer: ntfsVolData,
                    nOutBufferSize: ntfsVolData.Length,
                    lpBytesReturned: ref buf,
                    lpOverlapped: IntPtr.Zero);

                return new NTFS_VOLUME_DATA_BUFFER(ntfsVolData);

            }

        }

        #region pinvoke

        [DllImport(ExternDll.Kernel32, CharSet=System.Runtime.InteropServices.CharSet.Auto, BestFitMapping=false)]
        internal static extern SafeFileHandle CreateFile
            (
                string lpFileName,
                int dwDesiredAccess,
                int dwShareMode, 
                IntPtr lpSecurityAttributes, 
                int dwCreationDisposition,
                int dwFlagsAndAttributes, 
                SafeFileHandle hTemplateFile
            );

        [DllImport(ExternDll.Kernel32, SetLastError=true)]
        internal static extern bool CloseHandle
            (
                SafeFileHandle handle
            );

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DeviceIoControl
            (
                SafeFileHandle fileHandle, 
                uint ioControlCode, 
                IntPtr inBuffer, uint cbInBuffer, 
                IntPtr outBuffer, 
                uint cbOutBuffer, 
                out uint cbBytesReturned, 
                IntPtr overlapped
            );
        
        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        internal static extern bool GetFileInformationByHandle
            (
                IntPtr hFile, 
                out BY_HANDLE_FILE_INFORMATION lpFileInformation
            );

        #endregion pinvoke

        #region constants
        
        //public const uint NTFS_VOLUME_DATA_BUFFER = 0x00090064;

        public const int FILE_READ_DATA = (0x0001),
        FILE_LIST_DIRECTORY = (0x0001),
        FILE_WRITE_DATA = (0x0002),
        FILE_ADD_FILE = (0x0002),
        FILE_APPEND_DATA = (0x0004),
        FILE_ADD_SUBDIRECTORY = (0x0004),
        FILE_CREATE_PIPE_INSTANCE = (0x0004),
        FILE_READ_EA = (0x0008),
        FILE_WRITE_EA = (0x0010),
        FILE_EXECUTE = (0x0020),
        FILE_TRAVERSE = (0x0020),
        FILE_DELETE_CHILD = (0x0040),
        FILE_READ_ATTRIBUTES = (0x0080),
        FILE_WRITE_ATTRIBUTES = (0x0100),
        FILE_SHARE_READ = 0x00000001,
        FILE_SHARE_WRITE = 0x00000002,
        FILE_SHARE_DELETE = 0x00000004,
        FILE_ATTRIBUTE_READONLY = 0x00000001,
        FILE_ATTRIBUTE_HIDDEN = 0x00000002,
        FILE_ATTRIBUTE_SYSTEM = 0x00000004,
        FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
        FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
        FILE_ATTRIBUTE_NORMAL = 0x00000080,
        FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
        FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
        FILE_ATTRIBUTE_OFFLINE = 0x00001000,
        FILE_NOTIFY_CHANGE_FILE_NAME = 0x00000001,
        FILE_NOTIFY_CHANGE_DIR_NAME = 0x00000002,
        FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004,
        FILE_NOTIFY_CHANGE_SIZE = 0x00000008,
        FILE_NOTIFY_CHANGE_LAST_WRITE = 0x00000010,
        FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020,
        FILE_NOTIFY_CHANGE_CREATION = 0x00000040,
        FILE_NOTIFY_CHANGE_SECURITY = 0x00000100,
        FILE_ACTION_ADDED = 0x00000001,
        FILE_ACTION_REMOVED = 0x00000002,
        FILE_ACTION_MODIFIED = 0x00000003,
        FILE_ACTION_RENAMED_OLD_NAME = 0x00000004,
        FILE_ACTION_RENAMED_NEW_NAME = 0x00000005,
        FILE_CASE_SENSITIVE_SEARCH = 0x00000001,
        FILE_CASE_PRESERVED_NAMES = 0x00000002,
        FILE_UNICODE_ON_DISK = 0x00000004,
        FILE_PERSISTENT_ACLS = 0x00000008,
        FILE_FILE_COMPRESSION = 0x00000010,
        OPEN_EXISTING = 3,
        OPEN_ALWAYS = 4,
        FILE_FLAG_WRITE_THROUGH = unchecked((int)0x80000000),
        FILE_FLAG_OVERLAPPED = 0x40000000,
        FILE_FLAG_NO_BUFFERING = 0x20000000,
        FILE_FLAG_RANDOM_ACCESS = 0x10000000,
        FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000,
        FILE_FLAG_DELETE_ON_CLOSE = 0x04000000,
        FILE_FLAG_BACKUP_SEMANTICS = 0x02000000,
        FILE_FLAG_POSIX_SEMANTICS = 0x01000000,
        FILE_TYPE_UNKNOWN = 0x0000,
        FILE_TYPE_DISK = 0x0001,
        FILE_TYPE_CHAR = 0x0002,
        FILE_TYPE_PIPE = 0x0003,
        FILE_TYPE_REMOTE = unchecked((int)0x8000),
        FILE_VOLUME_IS_COMPRESSED = 0x00008000;

        #endregion constants

        internal static SafeFileHandle GetHandle(string directory)
        { 
        
            // Create handle to file/directory/device
            return CreateFile
                (
                    directory,                              // Directory name
                    FILE_LIST_DIRECTORY,                    // access (read-write) mode
                    FILE_SHARE_READ |
                        FILE_SHARE_DELETE |
                        FILE_SHARE_WRITE,                   // share mode
                    IntPtr.Zero,                            // security descriptor
                    OPEN_EXISTING,                          // how to create
                    FILE_FLAG_BACKUP_SEMANTICS |
                        FILE_FLAG_OVERLAPPED,               // file attributes
                    new SafeFileHandle(IntPtr.Zero, false)  // file with attributes to copy
                );
                               
            
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
