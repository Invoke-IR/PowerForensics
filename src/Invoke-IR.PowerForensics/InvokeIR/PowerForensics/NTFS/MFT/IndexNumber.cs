using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS.MFT
{
    public class IndexNumber
    {
        
        public static int Get(FileStream streamToRead, byte[] MFT, string fileName)
        {
            IntPtr hFile = NativeMethods.getHandle(fileName);

            // Check to see if file handle is valid
            if (hFile.ToInt32() == -1)
            {

                string[] directoryArray = fileName.Split('\\');
                string directory = null;

                for (int i = 0; i < (directoryArray.Length - 1); i++)
                {
                    directory += directoryArray[i];
                    directory += "\\";
                }

                int dirIndex = IndexNumber.Get(streamToRead, MFT, directory);

                List<IndexEntry> indxArray = IndexEntry.Get(streamToRead, MFT, dirIndex);

                foreach (IndexEntry indxEntry in indxArray)
                {

                    if (indxEntry.Name == directoryArray[(directoryArray.Length - 1)])
                    {
                        return (int)indxEntry.FileIndex;
                    }

                }

            }

            NativeMethods.BY_HANDLE_FILE_INFORMATION fileInfo = new NativeMethods.BY_HANDLE_FILE_INFORMATION();
            bool Success = NativeMethods.GetFileInformationByHandle(
                hFile: hFile,
                lpFileInformation: out fileInfo);

            // Combine two 32 bit unsigned integers into one 64 bit unsigned integer
            ulong FileIndex = fileInfo.nFileIndexHigh;
            FileIndex = FileIndex << 32;
            FileIndex = FileIndex + fileInfo.nFileIndexLow;

            // Unmask relevent bytes for MFT Index Number
            ulong Index = FileIndex & 0x0000FFFFFFFFFFFF;

            return (int)Index;

        }

        public static int Get(string volume, string fileName)
        {

            IntPtr hFile = NativeMethods.getHandle(fileName);

            // Check to see if file handle is valid
            if (hFile.ToInt32() == -1)
            {

                string[] directoryArray = fileName.Split('\\');
                string directory = null;

                for (int i = 0; i < (directoryArray.Length - 1); i++)
                {
                    directory += directoryArray[i];
                    directory += "\\";
                }

                int dirIndex = IndexNumber.Get(volume, directory);

                List<IndexEntry> indxArray = IndexEntry.Get(volume, dirIndex);

                foreach (IndexEntry indxEntry in indxArray)
                {

                    if (indxEntry.Name == directoryArray[(directoryArray.Length - 1)])
                    {
                        return (int)indxEntry.FileIndex;
                    }

                }

            }

            NativeMethods.BY_HANDLE_FILE_INFORMATION fileInfo = new NativeMethods.BY_HANDLE_FILE_INFORMATION();
            bool Success = NativeMethods.GetFileInformationByHandle(
                hFile: hFile,
                lpFileInformation: out fileInfo);

            // Combine two 32 bit unsigned integers into one 64 bit unsigned integer
            ulong FileIndex = fileInfo.nFileIndexHigh;
            FileIndex = FileIndex << 32;
            FileIndex = FileIndex + fileInfo.nFileIndexLow;

            // Unmask relevent bytes for MFT Index Number
            ulong Index = FileIndex & 0x0000FFFFFFFFFFFF;

            return (int)Index;

        }

    }

}
