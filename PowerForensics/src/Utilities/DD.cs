using System;
using System.IO;
using System.Collections.Generic;

namespace PowerForensics.Utilities
{
    #region DDClass

    public class DD
    {
        public static void Get(string inFile, string outFile, ulong offset, uint blockSize, uint count)
        {   
            // Get FileStream for reading from the hVolume handle
            using (FileStream streamToRead = Helper.getFileStream(inFile))
            {
                // Open file for reading
                using (FileStream streamToWrite = new FileStream(outFile, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    for (int i = 0; i < count; i++)
                    {
                        // Read the block size amount of bytes from the Volume
                        byte[] buffer = Helper.readDrive(streamToRead, offset, blockSize);

                        // Writes a block of bytes to this stream using data from a byte array.
                        streamToWrite.Write(buffer, 0, buffer.Length);

                        // Increment offset to read from
                        offset += blockSize;
                    }
                }
            }
        }

        public static byte[] Get(string inFile, ulong offset, uint blockSize, uint count)
        {
            List<byte> byteList = new List<byte>();

            using (FileStream streamToRead = Helper.getFileStream(inFile))
            {
                for(int i = 0; i < count; i++)
                {
                    byteList.AddRange(Helper.readDrive(streamToRead, offset, blockSize));
                    offset += blockSize;
                }
            }
            
            return byteList.ToArray();
        }
    }  

    #endregion DDClass
}
