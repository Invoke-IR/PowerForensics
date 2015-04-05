using System;
using System.IO;
using System.Diagnostics;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{

    public class MasterFileTable
    {

        public static byte[] GetBytes(IntPtr hVolume, FileStream streamToRead)
        {
            VolumeData volData = new VolumeData(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            ulong mftOffset = ((ulong)volData.BytesPerCluster * volData.MFTStartCluster);

            // Read bytes belonging to specified MFT Record and store in byte array
            byte[] mftBytes = NativeMethods.readDrive(streamToRead, mftOffset, volData.MFTSize);

            return mftBytes;
        }


        public static byte[] GetBytes(string volume)
        {
            IntPtr hVolume = NativeMethods.getHandle(volume);
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);
            VolumeData volData = new VolumeData(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            ulong mftOffset = ((ulong)volData.BytesPerCluster * volData.MFTStartCluster);

            // Read bytes belonging to specified MFT Record and store in byte array
            byte[] mftBytes = NativeMethods.readDrive(streamToRead, mftOffset, volData.MFTSize);

            return mftBytes;
        }

    }

}
