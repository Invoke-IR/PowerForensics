using System;
using System.IO;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics
{

    public class DD
    {
        public static void Get(string inFile, string outFile, long offset, int blockSize, int count)
        {

            IntPtr hVolume = NativeMethods.getHandle(inFile);
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);
            
            long sizeToRead = blockSize * count;

            // Read sizeToRead bytes from the Volume
            byte[] buffer = NativeMethods.readDrive(streamToRead, offset, sizeToRead);

            // Open file for reading
            System.IO.FileStream _FileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(buffer, 0, buffer.Length);
            // close file stream
            _FileStream.Close();
        }
    }  
}
