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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            // Get a handle to the specified volume
            IntPtr hVolume = NativeMethods.getHandle(volume);

            sw.Stop();
            Console.WriteLine("time: {0}", sw.ElapsedMilliseconds);
            sw.Start();

            // Instatiate null byte array
            byte[] mftBytes = null;

            // Create FileStream to read from the Volume file handle
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {

                sw.Stop();
                Console.WriteLine("getFileStream: {0}", sw.ElapsedMilliseconds);
                sw.Start();
                
                // Instantiate VolumeData object
                VolumeData volData = new VolumeData(hVolume);

                sw.Stop();
                Console.WriteLine("Instantiate VolumeData object: {0}", sw.ElapsedMilliseconds);
                sw.Start();

                // Calculate byte offset to the Master File Table (MFT)
                ulong mftOffset = ((ulong)volData.BytesPerCluster * volData.MFTStartCluster);

                // Read bytes belonging to specified MFT Record and store in byte array
                MFTRecord mftRecord = new MFTRecord(NativeMethods.readDrive(streamToRead, mftOffset, (ulong)volData.BytesPerMFTRecord));

                sw.Stop();
                Console.WriteLine("readDrive: {0}", sw.ElapsedMilliseconds);
                sw.Start();

                mftBytes = MFTRecord.getFile(streamToRead, mftRecord);

                sw.Stop();
                Console.WriteLine("getFile: {0}", sw.ElapsedMilliseconds);
                sw.Start();
            }

            NativeMethods.CloseHandle(hVolume);

            // Return byte array representing the Master File Table
            return mftBytes;
        }
    }

    #endregion MasterFileTableClass
}
