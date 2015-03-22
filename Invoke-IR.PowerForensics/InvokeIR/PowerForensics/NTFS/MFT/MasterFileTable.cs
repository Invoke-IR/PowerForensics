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
            NTFSVolumeData volData = NTFSVolumeData.Get(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MFTStartCluster);

            // Read bytes belonging to specified MFT Record and store in byte array
            byte[] mftBytes = NativeMethods.readDrive(streamToRead, mftOffset, volData.MFTSize);

            return mftBytes;
        }


        public static byte[] GetBytes(string volume)
        {
            IntPtr hVolume = NativeMethods.getHandle(volume);
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);
            NTFSVolumeData volData = NTFSVolumeData.Get(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MFTStartCluster);

            // Read bytes belonging to specified MFT Record and store in byte array
            byte[] mftBytes = NativeMethods.readDrive(streamToRead, mftOffset, volData.MFTSize);

            return mftBytes;
        }

    }

}
