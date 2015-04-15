using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{
    #region MFTRecordClass

    public class MFTRecord
    {

        #region Enums

        enum FILE_RECORD_FLAG
        {
            INUSE = 0x01,	// File record is in use
            DIR = 0x02	    // File record is a directory
        }

        #endregion Enums

        #region Structs

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

        #endregion Structs

        #region Properties

        public readonly string FullPath;
        public readonly string Name;
        public readonly ulong ParentIndex;
        public readonly uint RecordNumber;
        public readonly ulong Size;
        public readonly DateTime ModifiedTime;
        public readonly DateTime AccessedTime;
        public readonly DateTime ChangedTime;
        public readonly DateTime BornTime;
        public readonly string Permission;
        public readonly ushort SequenceNumber;
        public readonly ulong LogFileSequenceNumber;
        public readonly ushort Links;
        public readonly bool Deleted;
        public readonly bool Directory;
        public readonly Attr[] Attribute;

        #endregion Properties

        #region Constructors

        internal MFTRecord(byte[] recordBytes)
        {
            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            FILE_RECORD_HEADER RecordHeader = new FILE_RECORD_HEADER(recordBytes);

            // Check MFT Signature (FILE) to ensure bytes actually represent an MFT Record
            if (checkMFTRecord(RecordHeader.Magic))
            {
                RecordNumber = RecordHeader.RecordNo;
                Size = RecordHeader.RealSize;
                SequenceNumber = RecordHeader.SeqNo;
                LogFileSequenceNumber = RecordHeader.LSN;
                Links = RecordHeader.Hardlinks;

                // Unmask Header Flags
                #region HeaderFlags

                if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.INUSE) == (ushort)FILE_RECORD_FLAG.INUSE)
                {
                    Deleted = false;
                }
                else
                {
                    Deleted = true;
                }
                if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.DIR) == (ushort)FILE_RECORD_FLAG.DIR)
                {
                    Directory = true;
                }
                else
                {
                    Directory = false;
                }

                #endregion HeaderFlags

                List<Attr> AttributeList = new List<Attr>();
                int offsetToATTR = RecordHeader.OffsetOfAttr;

                while (offsetToATTR < (RecordHeader.RealSize - 8))
                {
                    int offset = offsetToATTR;
                    Attr attr = AttributeFactory.Get(recordBytes, offset, out offsetToATTR);
                    if (attr != null)
                    {
                        if (attr.Name == "STANDARD_INFORMATION")
                        {
                            StandardInformation stdInfo = attr as StandardInformation;
                            ModifiedTime = stdInfo.ModifiedTime;
                            AccessedTime = stdInfo.AccessedTime;
                            ChangedTime = stdInfo.ChangedTime;
                            BornTime = stdInfo.BornTime;
                            Permission = stdInfo.Permission;
                        }
                        else if (attr.Name == "FILE_NAME")
                        {
                            FileName fN = attr as FileName;
                            if (!(fN.Filename.Contains("~")))
                            {
                                Name = fN.Filename;
                                ParentIndex = fN.ParentIndex;
                            }

                        }
                        AttributeList.Add(attr);
                    }
                }

                Attribute = AttributeList.ToArray();
            }
        }

        internal MFTRecord(byte[] mftBytes, int index, string volLetter, string fileName)
        {

            byte[] recordBytes = getMFTRecordBytes(mftBytes, index);

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            FILE_RECORD_HEADER RecordHeader = new FILE_RECORD_HEADER(recordBytes);

            // Check MFT Signature (FILE) to ensure bytes actually represent an MFT Record
            if (checkMFTRecord(RecordHeader.Magic))
            {
                RecordNumber = RecordHeader.RecordNo;
                Size = RecordHeader.RealSize;
                SequenceNumber = RecordHeader.SeqNo;
                LogFileSequenceNumber = RecordHeader.LSN;
                Links = RecordHeader.Hardlinks;

                // Unmask Header Flags
                #region HeaderFlags

                if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.INUSE) == (ushort)FILE_RECORD_FLAG.INUSE)
                {
                    Deleted = false;
                }
                else
                {
                    Deleted = true;
                }
                if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.DIR) == (ushort)FILE_RECORD_FLAG.DIR)
                {
                    Directory = true;
                }
                else
                {
                    Directory = false;
                }

                #endregion HeaderFlags

                List<Attr> AttributeList = new List<Attr>();
                int offsetToATTR = RecordHeader.OffsetOfAttr;

                while (offsetToATTR < (RecordHeader.RealSize - 8))
                {
                    int offset = offsetToATTR;
                    Attr attr = AttributeFactory.Get(recordBytes, offset, out offsetToATTR);
                    if (attr != null)
                    {
                        if (attr.Name == "STANDARD_INFORMATION")
                        {
                            StandardInformation stdInfo = attr as StandardInformation;
                            ModifiedTime = stdInfo.ModifiedTime;
                            AccessedTime = stdInfo.AccessedTime;
                            ChangedTime = stdInfo.ChangedTime;
                            BornTime = stdInfo.BornTime;
                            Permission = stdInfo.Permission;
                        }
                        else if (attr.Name == "FILE_NAME")
                        {
                            FileName fN = attr as FileName;
                            if(!(fN.Filename.Contains("~")))
                            {
                                Name = fN.Filename;
                                ParentIndex = fN.ParentIndex;
                            }

                        }
                        AttributeList.Add(attr);
                    }
                }

                Attribute = AttributeList.ToArray();

                if (RecordNumber == ParentIndex)
                {
                    FullPath = volLetter;
                }
                else
                {
                    if (fileName != null)
                    {
                        FullPath = fileName;
                    }
                    else
                    {
                        MFTRecord parent = new MFTRecord(mftBytes, (int)ParentIndex, volLetter, fileName);
                        FullPath = parent.FullPath + Name;
                    }
                    if (Directory)
                    {
                        FullPath += '\\';
                    }
                }
            }
        }

        internal MFTRecord(byte[] mftBytes, int index, ref MFTRecord[] recordArray, string volLetter)
        {
            
            // Get byte array representing current record
            byte[] recordBytes = getMFTRecordBytes(mftBytes, index);

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            FILE_RECORD_HEADER RecordHeader = new FILE_RECORD_HEADER(recordBytes);

            // Check MFT Signature (FILE) to ensure bytes actually represent an MFT Record
            if (checkMFTRecord(RecordHeader.Magic))
            {
                RecordNumber = RecordHeader.RecordNo;
                Size = RecordHeader.RealSize;
                SequenceNumber = RecordHeader.SeqNo;
                LogFileSequenceNumber = RecordHeader.LSN;
                Links = RecordHeader.Hardlinks;

                // Unmask Header Flags
                #region HeaderFlags

                if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.INUSE) == (ushort)FILE_RECORD_FLAG.INUSE)
                {
                    Deleted = false;
                }
                else
                {
                    Deleted = true;
                }
                if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.DIR) == (ushort)FILE_RECORD_FLAG.DIR)
                {
                    Directory = true;
                }
                else
                {
                    Directory = false;
                }

                #endregion HeaderFlags

                List<Attr> AttributeList = new List<Attr>();
                int offsetToATTR = RecordHeader.OffsetOfAttr;

                while (offsetToATTR < (RecordHeader.RealSize - 8))
                {
                    //sw.Start();
                    int offset = offsetToATTR;
                    Attr attr = AttributeFactory.Get(recordBytes, offset, out offsetToATTR);
                    if (attr != null)
                    {
                        if (attr.Name == "STANDARD_INFORMATION")
                        {
                            StandardInformation stdInfo = attr as StandardInformation;
                            ModifiedTime = stdInfo.ModifiedTime;
                            AccessedTime = stdInfo.AccessedTime;
                            ChangedTime = stdInfo.ChangedTime;
                            BornTime = stdInfo.BornTime;
                            Permission = stdInfo.Permission;
                        }
                        else if (attr.Name == "FILE_NAME")
                        {
                            FileName fN = attr as FileName;
                            if(!(fN.Filename.Contains("~")))
                            {
                                Name = fN.Filename;
                                ParentIndex = fN.ParentIndex;
                            }
                        }
                        AttributeList.Add(attr);
                    }
                }
                // Check if MFT Record is for the root directory (should be Record Index 5)
                // If index and ParentIndex are not the same then get FullPath
                if((ulong)index != ParentIndex)
                {
                    // Check if ParentIndex Record has already been constructed and added to array
                    if (recordArray[ParentIndex] == null)
                    {
                        recordArray[ParentIndex] = new MFTRecord(mftBytes, (int)ParentIndex, ref recordArray, volLetter);   
                    }
                    // FullPath equals the ParentIndex FullPath + the current Index Name
                    // Make more efficient with String Builder
                    FullPath = recordArray[ParentIndex].FullPath + Name;
                    if(Directory)
                    {
                        FullPath += "\\";
                    }
                }
                else
                {
                    FullPath = volLetter;
                }
                Attribute = AttributeList.ToArray();
            }
            else
            {

            }
        }

        #endregion Constructors

        #region PrivateMethods

        // Check that bytes actually represent and MFT Record
        private static bool checkMFTRecord(uint magic)
        {
            return magic == 1162627398;
        }

        #endregion PrivateMethods

        // I think these belong elsewhere
        #region getFile

        internal static List<byte> getFile(string volume, FileStream streamToRead, byte[] MFT, string fileName)
        {

            string volLetter = volume.TrimStart('\\').TrimStart('.').TrimStart('\\') + '\\';

            int inode = IndexNumber.Get(streamToRead, MFT, fileName);

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            MFTRecord MFTRecord = MFTRecord.Get(MFT, inode, volLetter, fileName);

            if (!(MFTRecord.Directory))
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

        public static byte[] getFile(string volume, string fileName)
        {

            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            int inode = IndexNumber.Get(volume, fileName);

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            MFTRecord MFTRecord = MFTRecord.Get(mftBytes, inode, null, fileName);

            if (!(MFTRecord.Directory))
            {
                foreach (Attr attr in MFTRecord.Attribute)
                {
                    if (attr.Name == "DATA")
                    {
                        if (attr.NonResident == true)
                        {
                            NonResident nonResAttr = (NonResident)attr;
                            return NonResident.GetContent(volume, nonResAttr).ToArray();
                        }
                        else
                        {
                            Data dataAttr = (Data)attr;
                            return dataAttr.RawData;
                        }
                    }
                }
            }
            return null;
        }

        internal static byte[] getFile(string volume, int index)
        {

            byte[] mftBytes = MasterFileTable.GetBytes(volume);

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            MFTRecord MFTRecord = MFTRecord.Get(mftBytes, index, null, null);

            if (!(MFTRecord.Directory))
            {
                foreach (Attr attr in MFTRecord.Attribute)
                {
                    if (attr.Name == "DATA")
                    {
                        if (attr.NonResident == true)
                        {
                            NonResident nonResAttr = (NonResident)attr;
                            return NonResident.GetContent(volume, nonResAttr).ToArray();
                        }
                        else
                        {
                            Data dataAttr = (Data)attr;
                            return dataAttr.RawData;
                        }
                    }
                }
            }
            return null;
        }

        internal static byte[] getFile(FileStream streamToRead, MFTRecord mftRecord)
        {
            if (!(mftRecord.Directory))
            {
                foreach (Attr attr in mftRecord.Attribute)
                {
                    if (attr.Name == "DATA")
                    {
                        if (attr.NonResident == true)
                        {
                            NonResident nonResAttr = attr as NonResident;
                            return NonResident.GetContent(streamToRead, nonResAttr);
                        }
                        else
                        {
                            Data dataAttr = attr as Data;
                            return dataAttr.RawData;
                        }
                    }
                }
            }
            return null;
        }

        #endregion getFile

        #region getMFTRecordBytesMethods

        internal static byte[] getMFTRecordBytes(string volume, int index)
        {
            
            // Get handle for volume
            IntPtr hVolume = NativeMethods.getHandle(volume);
            
            // Get filestream based on hVolume
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);
            
            // 
            VolumeData volData = new VolumeData(hVolume);

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

        #endregion getMFTRecordBytesMethods

        // Add FullPath to individual Records
        #region GetMethods

        // Get an MFT record based on a byte array of the MFT and MFT Record Index
        public static MFTRecord Get(byte[] mftBytes, int index, string volLetter, string fileName)
        {
            return new MFTRecord(mftBytes, index, volLetter, fileName);
        }

        #endregion GetMethods

        #region GetInstancesMethods

        // Get all MFT Records from the MFT byte array
        internal static MFTRecord[] GetInstances(byte[] mftBytes, string volLetter)
        {
            // Determine number of MFT Records (each record is 1024 bytes)
            // Create an array large enough to hold each MFT Record
            int recordCount = mftBytes.Length / 1024;
            MFTRecord[] recordArray = new MFTRecord[recordCount];

            // Iterate through each index number and add MFTRecord to MFTRecord[]
            for (int i = 0; i < mftBytes.Length; i += 1024)
            {
                int index = i / 1024;
                if (recordArray[index] == null)
                {
                    recordArray[index] = new MFTRecord(mftBytes, index, ref recordArray, volLetter);
                }
            }

            // Return MFTRecord[]
            return recordArray;
        }

        // Get all MFT Records for the specified volume (Ex. \\.\C:)
        public static MFTRecord[] GetInstances(string volume, string volLetter)
        {
            // Get MFT as byte array
            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            // Call private GetInstances
            return GetInstances(mftBytes, volLetter);
        }

        #endregion GetInstancesMethods

    }

    #endregion MFTRecordClass
}
