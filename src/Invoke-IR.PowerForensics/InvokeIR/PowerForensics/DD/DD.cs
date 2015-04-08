using System;
using System.IO;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics
{
    #region DDClass

    public class DD
    {
        public static void Get(string inFile, string outFile, ulong offset, uint blockSize, uint count)
        {
            // Get handle (hVolume) for inFile
            IntPtr hVolume = NativeMethods.getHandle(inFile);
            
            // Get FileStream for reading from the hVolume handle
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {
                // Set sizeToRead to the blockSize * the count
                ulong sizeToRead = blockSize * count;

                // Read sizeToRead bytes from the Volume
                byte[] buffer = NativeMethods.readDrive(streamToRead, offset, sizeToRead);

                // Open file for reading
                System.IO.FileStream streamToWrite = new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                // Writes a block of bytes to this stream using data from a byte array.
                streamToWrite.Write(buffer, 0, buffer.Length);
            }
        }
    }  

    #endregion DDClass
}
