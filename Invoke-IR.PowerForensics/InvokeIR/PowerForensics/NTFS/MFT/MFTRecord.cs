using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        struct FILE_RECORD_HEADER
        {
            internal uint Magic;			// "FILE"
            internal ushort OffsetOfUS;		// Offset of Update Sequence
            internal ushort SizeOfUS;		    // Size in words of Update Sequence Number & Array
            internal ulong LSN;			    // $LogFile Sequence Number
            internal ushort SeqNo;			// Sequence number
            internal ushort Hardlinks;		// Hard link count
            internal ushort OffsetOfAttr;	    // Offset of the first Attribute
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

        public uint RecordNumber;
        public ushort SequenceNumber;
        public ulong LogFileSequenceNumber;
        public ushort Links;
        public string Flags;
        public Attr[] Attribute;

        internal MFTRecord(uint recordNumber, ushort sequenceNumber, ulong logFileSequenceNumber, ushort links, string flags, Attr[] attribute)
        {
            RecordNumber = recordNumber;
            SequenceNumber = sequenceNumber;
            LogFileSequenceNumber = logFileSequenceNumber;
            Links = links;
            Flags = flags;
            Attribute = attribute;
        }

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

        //Good
        private static bool checkMFTRecord(uint magic)
            {
                return magic == 1162627398;
            }

        //Good
        private static byte[] getMFTRecordBytes(byte[] mftBytes, int inode)
            {

                int recordOffset = inode * 1024;
                byte[] mftRecordBytes = new byte[1024];
                Array.Copy(mftBytes, recordOffset, mftRecordBytes, 0, 1024);

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
                    int offset = offsetToATTR;
                    Attr attr = AttributeFactory.Get(recordBytes, offset, out offsetToATTR);
                    AttributeList.Add(attr);
                }

                Attr[] AttributeArray = AttributeList.ToArray();

                // Return FileRecord object
                return new MFTRecord(RecordHeader.RecordNo, RecordHeader.SeqNo, RecordHeader.LSN, RecordHeader.Hardlinks, flagAttr.ToString(), AttributeArray);

            }

            else
            {

                return null;
            
            }
        
        }

        //Good
        public static MFTRecord Get(string volume, int inode)
        {
            byte[] mftBytes = MasterFileTable.GetBytes(volume);

            return MFTRecord.Get(getMFTRecordBytes(mftBytes, inode));
        }

        //Good
        public static MFTRecord Get(byte[] mftBytes, int inode)
        {

            return MFTRecord.Get(getMFTRecordBytes(mftBytes, inode));
        
        }

        //Good
        public static MFTRecord[] GetInstances(string volume)
        {
            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            int recordCount = mftBytes.Length / 1024;
            MFTRecord[] recordArray = new MFTRecord[recordCount];
            for (int i = 0; i < mftBytes.Length; i += 1024)
            {
                int index = i / 1024;
                recordArray[index] = MFTRecord.Get(getMFTRecordBytes(mftBytes, index));
            }

            return recordArray;
        
        }

        //Good
        public static MFTRecord[] GetInstances(byte[] mftBytes)
        {
            int recordCount = mftBytes.Length / 1024; 
            MFTRecord[] recordArray = new MFTRecord[recordCount];
            for (int i = 0; i < mftBytes.Length; i += 1024)
            {
                int index = i / 1024;
                recordArray[index] = MFTRecord.Get(getMFTRecordBytes(mftBytes, index));
            }

            return recordArray;
        
        }

    }

}
