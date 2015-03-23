using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS.MFT.Attributes;

namespace InvokeIR.PowerForensics.NTFS.MFT
{
    public class MFTRecord
    {

        enum FILE_RECORD_FLAG
        {
            INUSE = 0x01,	// File record is in use
            DIR = 0x02	    // File record is a directory
        }

        internal struct FILE_RECORD_HEADER
        {
            internal uint Magic;			// "FILE"
            internal ushort OffsetOfUS;		// Offset of Update Sequence
            internal ushort SizeOfUS;		// Size in words of Update Sequence Number & Array
            internal ulong LSN;			    // $LogFile Sequence Number
            internal ushort SeqNo;			// Sequence number
            internal ushort Hardlinks;		// Hard link count
            internal ushort OffsetOfAttr;	// Offset of the first Attribute
            internal ushort Flags;			// Flags
            internal uint RealSize;		    // Real size of the FILE record
            internal uint AllocSize;		// Allocated size of the FILE record
            internal ulong RefToBase;		// File reference to the base FILE record
            internal ushort NextAttrId;		// Next Attribute Id
            internal ushort Align;			// Align to 4 byte boundary
            internal uint RecordNo;		    // Number of this MFT Record

            internal FILE_RECORD_HEADER(byte[] bytes)
            {
                Magic = BitConverter.ToUInt32(bytes, 0);
                OffsetOfUS = BitConverter.ToUInt16(bytes, 4);
                SizeOfUS = BitConverter.ToUInt16(bytes, 6);
                LSN = BitConverter.ToUInt64(bytes, 8);
                SeqNo = BitConverter.ToUInt16(bytes, 16);
                Hardlinks = BitConverter.ToUInt16(bytes, 18);
                OffsetOfAttr = BitConverter.ToUInt16(bytes, 20);
                Flags = BitConverter.ToUInt16(bytes, 22);
                RealSize = BitConverter.ToUInt32(bytes, 24);
                AllocSize = BitConverter.ToUInt32(bytes, 28);
                RefToBase = BitConverter.ToUInt64(bytes, 32);
                NextAttrId = BitConverter.ToUInt16(bytes, 40);
                Align = BitConverter.ToUInt16(bytes, 42);
                RecordNo = BitConverter.ToUInt32(bytes, 44);
            }
        }

        #region Properties

        public string Name;
        public uint RecordNumber;
        public ulong Size;
        public DateTime AccessTime;
        public DateTime BornTime;
        public DateTime ChangeTime;
        public DateTime MFTChangeTime;
        public uint Permission;
        public ushort SequenceNumber;
        public ulong LogFileSequenceNumber;
        public ushort Links;
        public string Flags;
        public Attr[] Attribute;

        #endregion Properties

        #region Constructors

        internal MFTRecord(string name, ulong size, uint recordNumber, DateTime atime, DateTime btime, DateTime mtime, DateTime ctime, uint permission, ushort sequenceNumber, ulong logFileSequenceNumber, ushort links, string flags, Attr[] attribute)
        {
            Name = name;
            Size = size;
            RecordNumber = recordNumber;
            AccessTime = atime;
            BornTime = btime;
            ChangeTime = mtime;
            MFTChangeTime = ctime;
            Permission = permission;
            SequenceNumber = sequenceNumber;
            LogFileSequenceNumber = logFileSequenceNumber;
            Links = links;
            Flags = flags;
            Attribute = attribute;
        }

        #endregion Constructors

        public static List<byte> getFile(string volume, FileStream streamToRead, byte[] MFT, string fileName)
        {

            int inode = IndexNumber.Get(streamToRead, MFT, fileName);

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            MFTRecord MFTRecord = MFTRecord.Get(MFT, inode);

            if (!(MFTRecord.Flags.Contains("Directory")))
            {
                foreach (Attr attr in MFTRecord.Attribute)
                {
                    if (attr.Name == "DATA")
                    {
                        if (attr.NonResident == true)
                        {
                            NonResident nonResAttr = (NonResident)attr;
                            return NonResident.GetContent(volume, nonResAttr);
                        }

                        else
                        {
                            Data dataAttr = (Data)attr;
                            return null;
                            //return dataAttr.RawData;
                        }
                    }
                }
            }

            return null;

        }

        public static List<byte> getFile(string volume, string fileName)
        {

            int inode = IndexNumber.Get(volume, fileName);

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            MFTRecord MFTRecord = MFTRecord.Get(volume, inode);

            if (!(MFTRecord.Flags.Contains("Directory")))
            {
                foreach (Attr attr in MFTRecord.Attribute)
                {
                    if (attr.Name == "DATA")
                    {
                        if (attr.NonResident == true)
                        {
                            NonResident nonResAttr = (NonResident)attr;
                            return NonResident.GetContent(volume, nonResAttr);
                        }
                        else
                        {
                            Data dataAttr = (Data)attr;
                            return null;
                            //return dataAttr.RawData;
                        }
                    }
                }
            }
            return null;
        }

        // Check that bytes actually represent and MFT Record
        private static bool checkMFTRecord(uint magic)
            {
                return magic == 1162627398;
            }

        internal static byte[] getMFTRecordBytes(string volume, int index)
        {
            
            // Get handle for volume
            IntPtr hVolume = NativeMethods.getHandle(volume);
            
            // Get filestream based on hVolume
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);
            
            // 
            NTFSVolumeData volData = NTFSVolumeData.Get(hVolume);

            ulong mftStartOffset = volData.MFTStartCluster * (ulong)volData.BytesPerCluster;
            ulong recordOffset = mftStartOffset + ((ulong)index * 1024);

            return NativeMethods.readDrive(streamToRead, recordOffset, 1024);

        }

        // Get byte array representing specific MFT Record (1024 bytes in size)
        internal static byte[] getMFTRecordBytes(byte[] mftBytes, int index)
        {

            // Determine byte offset of MFT Record
            int recordOffset = index * 1024;

            // Create a byte array the size of an MFT Record (1024 bytes)
            byte[] mftRecordBytes = new byte[1024];

            // Create a subarray representing the MFT Record from the MFT byte array
            Array.Copy(mftBytes, recordOffset, mftRecordBytes, 0, 1024);

            // Return the MFT Record byte array
            return mftRecordBytes;

        }

        //Good
        private static MFTRecord Get(byte[] recordBytes)
        {

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            FILE_RECORD_HEADER RecordHeader = new FILE_RECORD_HEADER(recordBytes);

            // Check MFT Signature (FILE) to ensure bytes actually represent an MFT Record
            if (checkMFTRecord(RecordHeader.Magic))
            {

                string fileName = null;
                DateTime atime = new DateTime();
                DateTime btime = new DateTime();
                DateTime mtime = new DateTime();
                DateTime ctime = new DateTime();
                uint permission = 0;

                // Unmask Header Flags
                #region HeaderFlags

                StringBuilder flagAttr = new StringBuilder();
                if (RecordHeader.Flags != 0)
                {
                    if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.INUSE) == (ushort)FILE_RECORD_FLAG.INUSE)
                    {
                        flagAttr.Append("InUse, ");
                    }
                    if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.DIR) == (ushort)FILE_RECORD_FLAG.DIR)
                    {
                        flagAttr.Append("Directory, ");
                    }
                    flagAttr.Length -= 2;
                }

                #endregion HeaderFlags

                List<Attr> AttributeList = new List<Attr>();
                int offsetToATTR = RecordHeader.OffsetOfAttr;

                while (offsetToATTR < (RecordHeader.RealSize - 8))
                {
                    int i = 0;
                    int offset = offsetToATTR;
                    Attr attr = AttributeFactory.Get(recordBytes, offset, out offsetToATTR);
                    if(attr != null)
                    {
                        if(attr.Name == "STANDARD_INFORMATION")
                        {
                            StandardInformation stdInfo = attr as StandardInformation;
                            atime = stdInfo.AccessTime;
                            btime = stdInfo.CreateTime;
                            mtime = stdInfo.FileModifiedTime;
                            ctime = stdInfo.MFTModifiedTime;
                            permission = stdInfo.Permission;
                        }
                        else if((attr.Name == "FILE_NAME") && (i < 1))
                        {
                            FileName fN = attr as FileName;
                            fileName = fN.Filename;
                            i++;
                        }
                        AttributeList.Add(attr);
                    }
                }

                Attr[] AttributeArray = AttributeList.ToArray();

                // Return FileRecord object
                return new MFTRecord(
                    fileName, 
                    RecordHeader.RealSize, 
                    RecordHeader.RecordNo, 
                    atime,
                    btime,
                    mtime,
                    ctime,
                    permission,
                    RecordHeader.SeqNo, 
                    RecordHeader.LSN, 
                    RecordHeader.Hardlinks, 
                    flagAttr.ToString(), 
                    AttributeArray);

            }

            else
            {

                return null;
            
            }
        
        }

        #region GetMethods

        // Get an MFT record based on a byte array of the MFT and MFT Record Index
        public static MFTRecord Get(byte[] mftBytes, int index)
        {
            return MFTRecord.Get(getMFTRecordBytes(mftBytes, index));
        }

        // Get an MFT record based on the volume name (\\.\C:) and MFT Record Index
        public static MFTRecord Get(string volume, int index)
        {
            return MFTRecord.Get(getMFTRecordBytes((MasterFileTable.GetBytes(volume)), index));
        }

        #endregion GetMethods

        #region GetInstancesMethods

        // Get all MFT Records from the MFT byte array
        public static MFTRecord[] GetInstances(byte[] mftBytes)
        {
            // Determine number of MFT Records (each record is 1024 bytes)
            // Create an array large enough to hold each MFT Record
            int recordCount = mftBytes.Length / 1024;
            MFTRecord[] recordArray = new MFTRecord[recordCount];

            // Iterate through each index number and add MFTRecord to MFTRecord[]
            for (int i = 0; i < mftBytes.Length; i += 1024)
            {
                int index = i / 1024;
                recordArray[index] = MFTRecord.Get(getMFTRecordBytes(mftBytes, index));
            }

            // Return MFTRecord[]
            return recordArray;
        }

        // Get all MFT Records for the specified volume (Ex. \\.\C:)
        public static MFTRecord[] GetInstances(string volume)
        {
            // Get MFT as byte array
            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            // Determine number of MFT Records (each record is 1024 bytes)
            // Create an array large enough to hold each MFT Record
            int recordCount = mftBytes.Length / 1024;
            MFTRecord[] recordArray = new MFTRecord[recordCount];

            // Iterate through each index number and add MFTRecord to MFTRecord[]
            for (int i = 0; i < mftBytes.Length; i += 1024)
            {
                int index = i / 1024;
                recordArray[index] = MFTRecord.Get(getMFTRecordBytes(mftBytes, index));
            }

            // Return MFTRecord[]
            return recordArray;
        }

        #endregion GetInstancesMethods

    }

}
