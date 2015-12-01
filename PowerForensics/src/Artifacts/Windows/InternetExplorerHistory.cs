using System;
using System.Collections.Generic;
using System.Text;
using PowerForensics.Ntfs;

namespace PowerForensics.Artifacts
{
    public class InternetExplorerHistoryHeader
    {
        #region Properties

        public readonly string Signature;
        public readonly uint Size;
        public readonly uint FirstPageOffset;
        public readonly uint BlockCount;
        public readonly uint AllocatedBlockCount;
        public readonly ulong CacheLimit;
        public readonly ulong CacheSize;
        public readonly ulong CacheExempt;
        public readonly uint SubdirectoryCount;
        public readonly byte[] Subdirectories;
        public readonly byte[] HeaderData;
        public readonly byte[] AllocationBitmap;

        #endregion Properties

        #region Constructor

        internal InternetExplorerHistoryHeader(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x1C).TrimEnd('\0');
            Size = BitConverter.ToUInt32(bytes, 0x1C);
            FirstPageOffset = BitConverter.ToUInt32(bytes, 0x20);
            BlockCount = BitConverter.ToUInt32(bytes, 0x24);
            AllocatedBlockCount = BitConverter.ToUInt32(bytes, 0x28);
            CacheLimit = BitConverter.ToUInt64(bytes, 0x30);
            CacheSize = BitConverter.ToUInt64(bytes, 0x38);
            CacheExempt = BitConverter.ToUInt64(bytes, 0x40);
            SubdirectoryCount = BitConverter.ToUInt32(bytes, 0x48);
            Subdirectories = Util.GetSubArray(bytes, 0x4C, 0x180);
            HeaderData = Util.GetSubArray(bytes, 0x1CC, 0x80);
            AllocationBitmap = Util.GetSubArray(bytes, 0x250, 0x3DB0);
        }

        #endregion Constructor
    }

    public class InternetExplorerHistory
    {
        #region Properties
        
        
        
        #endregion Properties

        #region Constructors
        
        

        #endregion Constructors

        #region StaticMethods

        public static InternetExplorerHistory[] Get(string volume)
        {
            // Determine Windows Version
            WindowsVersion version = WindowsVersion.Get(volume);

            string HistoryPath = null;

            /*switch (version.CurrentVersion)
            {
                case "5.1":
                    //% systemdir %\Documents and Settings\% username %\Local Settings\History\history.ie5
                    break;
                case "5.2":
                    //% systemdir %\Documents and Settings\% username %\Local Settings\History\history.ie5
                    break;
                case "6.0":
                    //% systemdir % \Users\% username %\AppData\Local\Microsoft\Windows\Temporary Internet Files\ 
                    //% systemdir % \Users\% username %\AppData\Local\Microsoft\Windows\Temporary Internet Files\Low\
                    break;
                case "6.1":
                    //% systemdir % \Users\% username %\AppData\Local\Microsoft\Windows\Temporary Internet Files\ 
                    //% systemdir % \Users\% username %\AppData\Local\Microsoft\Windows\Temporary Internet Files\Low\
                    break;
                case "6.2":
                    //C:\Users\tester\AppData\Local\Microsoft\Windows\WebCache\WebcacheV01.dat
                    break;
                case "6.3":
                    //C: \Users\tester\AppData\Local\Microsoft\Windows\WebCache\WebcacheV01.dat
                    break;
            }*/
            // Iterate through Index.Dat Files


            // Return InternetExplorerHistory Objects
            return null;
        }

        public static InternetExplorerHistory[] GetByPath(string path)
        {
            return Get(FileRecord.Get(path, true).GetContent());
        }

        private static InternetExplorerHistory[] Get(byte[] bytes)
        {
            InternetExplorerHistoryHeader header = new InternetExplorerHistoryHeader(bytes);

            List<InternetExplorerHistory> historyList = new List<InternetExplorerHistory>();

            /*for ()
            {

            }*/

            return historyList.ToArray();
        }

        #endregion StaticMethods
    }
}
