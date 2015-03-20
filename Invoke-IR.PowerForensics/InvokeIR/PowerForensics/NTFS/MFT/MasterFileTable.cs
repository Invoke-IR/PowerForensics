using System;
using System.IO;
using System.Diagnostics;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS.MFT
{

    public class MasterFileTable
    {

        public static byte[] GetBytes(IntPtr hVolume, FileStream streamToRead)
        {
            NativeMethods.NTFS_VOLUME_DATA_BUFFER volData = NativeMethods.NTFS_VOLUME_DATA_BUFFER.Get(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MftStartLcn);

            // Read bytes belonging to specified MFT Record and store in byte array
            byte[] mftBytes = NativeMethods.readDrive(streamToRead, mftOffset, volData.MftValidDataLength);

            return mftBytes;
        }


        public static byte[] GetBytes(string volume)
        {
            IntPtr hVolume = Win32.getHandle(volume);
            FileStream streamToRead = Win32.getFileStream(hVolume);
            NativeMethods.NTFS_VOLUME_DATA_BUFFER volData = NativeMethods.NTFS_VOLUME_DATA_BUFFER.Get(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MftStartLcn);

            // Read bytes belonging to specified MFT Record and store in byte array
            byte[] mftBytes = Win32.readDrive(streamToRead, mftOffset, volData.MftValidDataLength);

            return mftBytes;
        }

    }

}
