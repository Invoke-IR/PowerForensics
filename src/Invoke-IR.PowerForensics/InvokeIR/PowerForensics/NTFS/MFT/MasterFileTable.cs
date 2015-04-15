using System;
using System.IO;
using System.Diagnostics;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{
    #region MasterFileTableClass

    public class MasterFileTable
    {
        // Get Master File Table Bytes for "GetInstance" Functions
        // Caller is responsible for cleaning up streamToRead and hVolume
        public static byte[] GetBytes(IntPtr hVolume, FileStream streamToRead)
        {
            // Instantiate VolumeData object
            VolumeData volData = new VolumeData(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            ulong mftOffset = ((ulong)volData.BytesPerCluster * volData.MFTStartCluster);

            // Read bytes belonging to specified MFT Record and store in byte array
            MFTRecord mftRecord = new MFTRecord(NativeMethods.readDrive(streamToRead, mftOffset, (ulong)volData.BytesPerMFTRecord));

            // Return byte array representing the Master File Table
            return MFTRecord.getFile(streamToRead, mftRecord);
        }

        // Get Master File Table Bytes for "Get" Methods
        // GetBytes will clean up IntPtr (Volume Handle) and FileStream objects
        public static byte[] GetBytes(string volume)
        {
            // Get a handle to the specified volume
            IntPtr hVolume = NativeMethods.getHandle(volume);

            // Instatiate null byte array
            byte[] mftBytes = null;

            // Create FileStream to read from the Volume file handle
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {
                // Instantiate VolumeData object
                VolumeData volData = new VolumeData(hVolume);

                // Calculate byte offset to the Master File Table (MFT)
                ulong mftOffset = ((ulong)volData.BytesPerCluster * volData.MFTStartCluster);

                // Read bytes belonging to specified MFT Record and store in byte array
                MFTRecord mftRecord = new MFTRecord(NativeMethods.readDrive(streamToRead, mftOffset, (ulong)volData.BytesPerMFTRecord));

                mftBytes = MFTRecord.getFile(streamToRead, mftRecord);
            }

            NativeMethods.CloseHandle(hVolume);

            // Return byte array representing the Master File Table
            return mftBytes;
        }
    }

    #endregion MasterFileTableClass
}
