using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{
    public class IndexNumber
    {

        private enum SYSTEM_FILES
        {
            MFT = 0,
            MFTMirr = 1,
            LogFile = 2,
            Volume = 3,
            AttrDef = 4,
            Root = 5,
            Bitmap = 6,
            Boot = 7,
            BadClus = 8,
            Secure = 9,
            UpCase = 10,
            Extend = 11,
            Quota = 24,
            ObjId = 25,
            Reparse = 26,
            RmMetadata = 27,
            Repair = 28,
            TxfLog = 29,
            Txf = 30,
            Tops = 31,
            TxfLogblf = 32,
            TxfLogContainer00000000000000000001 = 33,
            TxfLogContainer00000000000000000002 = 34
        }

        public static int Get(FileStream streamToRead, byte[] MFT, string filePath)
        {
            if (Enum.IsDefined(typeof(SYSTEM_FILES), filePath))
            {
                #region SystemFilesIndex

                switch (filePath)
                {
                    case "MFT":
                        //case "$MFT":
                        return (int)SYSTEM_FILES.MFT;
                    case "MFTMirr":
                        //case "$MFTMirr":
                        return (int)SYSTEM_FILES.MFTMirr;
                    case "LogFile":
                        //case "$LogFile":
                        return (int)SYSTEM_FILES.LogFile;

                    case "Volume":
                        //case "$Volume":
                        return (int)SYSTEM_FILES.Volume;
                    case "AttrDef":
                        //case "$AttrDef":
                        return (int)SYSTEM_FILES.AttrDef;
                    case "C:\\":
                    case "Root":
                        return (int)SYSTEM_FILES.Root;
                    case "Bitmap":
                        //case "$Bitmap":
                        return (int)SYSTEM_FILES.Bitmap;
                    case "Boot":
                        //case "$Boot":
                        return (int)SYSTEM_FILES.Boot;
                    case "BadClus":
                        //case "$BadClus":
                        return (int)SYSTEM_FILES.BadClus;
                    case "Secure":
                        //case "$Secure":
                        return (int)SYSTEM_FILES.Secure;
                    case "UpCase":
                        //case "$UpCase":
                        return (int)SYSTEM_FILES.UpCase;
                    case "Extend":
                        //case "$Extend":
                        return (int)SYSTEM_FILES.Extend;
                    case "Quota":
                        //case "$Quota":
                        return (int)SYSTEM_FILES.Quota;
                    case "ObjId":
                        //case "$ObjId":
                        return (int)SYSTEM_FILES.ObjId;
                    case "Reparse":
                        //case "$Reparse":
                        return (int)SYSTEM_FILES.Reparse;
                    case "RmMetadata":
                        //case "$RmMetadata":
                        return (int)SYSTEM_FILES.RmMetadata;
                    case "Repair":
                        //case "$Repair":
                        return (int)SYSTEM_FILES.Repair;
                    case "TxfLog":
                        //case "$TxfLog":
                        return (int)SYSTEM_FILES.TxfLog;
                    case "Txf":
                        //case "$Txf":
                        return (int)SYSTEM_FILES.Txf;
                    case "Tops":
                        //case "$Tops":
                        return (int)SYSTEM_FILES.Tops;
                    case "TxfLog.blf":
                        //case "$TxfLog.blf":
                        return (int)SYSTEM_FILES.TxfLogblf;
                    case "TxfLogContainer00000000000000000001":
                        //case "$TxfLogContainer00000000000000000001":
                        return (int)SYSTEM_FILES.TxfLogContainer00000000000000000001;
                    case "TxfLogContainer00000000000000000002":
                        //case "$TxfLogContainer00000000000000000002":
                        return (int)SYSTEM_FILES.TxfLogContainer00000000000000000002;
                }

                #endregion SystemFilesIndex
            }
            else
            {

                IntPtr hFile = NativeMethods.getHandle(filePath);

                // Check to see if file handle is valid
                if (hFile.ToInt32() == -1)
                {

                    string[] directoryArray = filePath.Split('\\');
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
            return 0;
        }

        public static int Get(string volume, string filePath)
        {
            if (Enum.IsDefined(typeof(SYSTEM_FILES), filePath))
            {
                #region SystemFilesIndex

                switch (filePath)
                {
                    case "MFT":
                        //case "$MFT":
                        return (int)SYSTEM_FILES.MFT;
                    case "MFTMirr":
                        //case "$MFTMirr":
                        return (int)SYSTEM_FILES.MFTMirr;
                    case "LogFile":
                        //case "$LogFile":
                        return (int)SYSTEM_FILES.LogFile;

                    case "Volume":
                        //case "$Volume":
                        return (int)SYSTEM_FILES.Volume;
                    case "AttrDef":
                        //case "$AttrDef":
                        return (int)SYSTEM_FILES.AttrDef;
                    case "Root":
                        //case "$Root":
                        return (int)SYSTEM_FILES.Root;
                    case "Bitmap":
                        //case "$Bitmap":
                        return (int)SYSTEM_FILES.Bitmap;
                    case "Boot":
                        //case "$Boot":
                        return (int)SYSTEM_FILES.Boot;
                    case "BadClus":
                        //case "$BadClus":
                        return (int)SYSTEM_FILES.BadClus;
                    case "Secure":
                        //case "$Secure":
                        return (int)SYSTEM_FILES.Secure;
                    case "UpCase":
                        //case "$UpCase":
                        return (int)SYSTEM_FILES.UpCase;
                    case "Extend":
                        //case "$Extend":
                        return (int)SYSTEM_FILES.Extend;
                    case "Quota":
                        //case "$Quota":
                        return (int)SYSTEM_FILES.Quota;
                    case "ObjId":
                        //case "$ObjId":
                        return (int)SYSTEM_FILES.ObjId;
                    case "Reparse":
                        //case "$Reparse":
                        return (int)SYSTEM_FILES.Reparse;
                    case "RmMetadata":
                        //case "$RmMetadata":
                        return (int)SYSTEM_FILES.RmMetadata;
                    case "Repair":
                        //case "$Repair":
                        return (int)SYSTEM_FILES.Repair;
                    case "TxfLog":
                        //case "$TxfLog":
                        return (int)SYSTEM_FILES.TxfLog;
                    case "Txf":
                        //case "$Txf":
                        return (int)SYSTEM_FILES.Txf;
                    case "Tops":
                        //case "$Tops":
                        return (int)SYSTEM_FILES.Tops;
                    case "TxfLog.blf":
                        //case "$TxfLog.blf":
                        return (int)SYSTEM_FILES.TxfLogblf;
                    case "TxfLogContainer00000000000000000001":
                        //case "$TxfLogContainer00000000000000000001":
                        return (int)SYSTEM_FILES.TxfLogContainer00000000000000000001;
                    case "TxfLogContainer00000000000000000002":
                        //case "$TxfLogContainer00000000000000000002":
                        return (int)SYSTEM_FILES.TxfLogContainer00000000000000000002;
                }

                #endregion SystemFilesIndex
            }
            else
            {
                IntPtr hFile = NativeMethods.getHandle(filePath);

                // Check to see if file handle is valid
                if (hFile.ToInt32() == -1)
                {

                    string[] directoryArray = filePath.Split('\\');
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

            return 0;

        }

    }

}
