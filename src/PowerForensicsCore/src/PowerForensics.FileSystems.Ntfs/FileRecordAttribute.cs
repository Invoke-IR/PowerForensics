using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    enum ATTR_FILENAME_FLAG
    {
        /// <summary>
        /// 
        /// </summary>
        READONLY = 0x00000001,

        /// <summary>
        /// 
        /// </summary>
        HIDDEN = 0x00000002,

        /// <summary>
        /// 
        /// </summary>
        SYSTEM = 0x00000004,

        /// <summary>
        /// 
        /// </summary>
        ARCHIVE = 0x00000020,

        /// <summary>
        /// 
        /// </summary>
        DEVICE = 0x00000040,

        /// <summary>
        /// 
        /// </summary>
        NORMAL = 0x00000080,

        /// <summary>
        /// 
        /// </summary>
        TEMP = 0x00000100,

        /// <summary>
        /// 
        /// </summary>
        SPARSE = 0x00000200,

        /// <summary>
        /// 
        /// </summary>
        REPARSE = 0x00000400,

        /// <summary>
        /// 
        /// </summary>
        COMPRESSED = 0x00000800,

        /// <summary>
        /// 
        /// </summary>
        OFFLINE = 0x00001000,

        /// <summary>
        /// 
        /// </summary>
        NCI = 0x00002000,

        /// <summary>
        /// 
        /// </summary>
        ENCRYPTED = 0x00004000,

        /// <summary>
        /// 
        /// </summary>
        DIRECTORY = 0x10000000,

        /// <summary>
        /// 
        /// </summary>
        INDEXVIEW = 0x20000000
    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum INDEX_ROOT_FLAGS
    {

        /// <summary>
        /// 
        /// </summary>
        INDEX_ROOT_ONLY = 0x00,

        /// <summary>
        /// 
        /// </summary>
        INDEX_ALLOCATION = 0x01
    }

    /// <summary>
    /// 
    /// </summary>
    public class AttributeList : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly AttrRef[] AttributeReference;

        #endregion Properties

        #region Constructors

        internal AttributeList(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (FileRecordAttribute.ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            #region AttributeReference

            int i = offset;
            List<AttrRef> refList = new List<AttrRef>();

            while (i < offset + header.AttrSize)
            {
                AttrRef attrRef = new AttrRef(bytes, i);
                refList.Add(attrRef);
                i += attrRef.RecordLength;
            }
            AttributeReference = refList.ToArray();

            #endregion AttributeReference
        }

        internal AttributeList(NonResident nonRes)
        {
            Name = (FileRecordAttribute.ATTR_TYPE)nonRes.Name;
            NameString = nonRes.NameString;
            NonResident = nonRes.NonResident;
            AttributeId = nonRes.AttributeId;
            AttributeSize = nonRes.AttributeSize;

            #region AttributeReference

            List<AttrRef> refList = new List<AttrRef>();

            byte[] bytes = nonRes.GetBytes();

            int i = 0;

            while (i < bytes.Length)
            {
                AttrRef attrRef = new AttrRef(bytes, i);
                refList.Add(attrRef);
                i += attrRef.RecordLength;
            }
            AttributeReference = refList.ToArray();

            #endregion AttributeReference
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class AttrRef
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;

        internal readonly ushort RecordLength;

        internal readonly byte AttributeNameLength;

        internal readonly byte AttributeNameOffset;

        internal readonly ulong LowestVCN;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort SequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly string NameString;

        #endregion Properties

        #region Constructors

        internal AttrRef(byte[] bytes, int offset)
        {
            Name = Enum.GetName(typeof(FileRecordAttribute.ATTR_TYPE), BitConverter.ToInt32(bytes, 0x00 + offset));
            RecordLength = BitConverter.ToUInt16(bytes, 0x04 + offset);
            AttributeNameLength = bytes[0x06 + offset];
            AttributeNameOffset = bytes[0x07 + offset];
            LowestVCN = BitConverter.ToUInt64(bytes, 0x08 + offset);
            if (LowestVCN == 0)
            {
                RecordNumber = BitConverter.ToUInt64(bytes, 0x10 + offset) & 0x0000FFFFFFFFFFFF;
                SequenceNumber = BitConverter.ToUInt16(bytes, 0x16 + offset);
                NameString = Encoding.Unicode.GetString(bytes, AttributeNameOffset + offset, AttributeNameLength * 2);
            }
        }

        #endregion Constructors
    }

    class CommonHeader
    {
        #region Constants

        internal const byte RESIDENT = 0x00;
        internal const byte NONRESIDENT = 0x01;

        #endregion Constants

        #region Properties

        internal uint ATTRType;			// Attribute Type
        internal int TotalSize;		    // Length (including this header)
        internal bool NonResident;	    // 0 - resident, 1 - non resident
        internal int NameLength;		// name length in words
        internal ushort NameOffset;		// offset to the name
        internal ushort Flags;			// Flags
        internal ushort Id;				// Attribute Id

        #endregion Properties

        #region Constructors

        internal CommonHeader(byte[] bytes)
        {
            ATTRType = BitConverter.ToUInt32(bytes, 0x00);
            TotalSize = BitConverter.ToInt32(bytes, 0x04);
            NonResident = (bytes[0x08] == NONRESIDENT);
            NameLength = bytes[0x09] * 2;
            NameOffset = BitConverter.ToUInt16(bytes, 0x0A);
            Flags = BitConverter.ToUInt16(bytes, 0x0C);
            Id = BitConverter.ToUInt16(bytes, 0x0E);
        }

        internal CommonHeader(byte[] bytes, int offset)
        {
            ATTRType = BitConverter.ToUInt32(bytes, 0x00 + offset);
            TotalSize = BitConverter.ToInt32(bytes, 0x04 + offset);
            NonResident = (bytes[0x08 + offset] == NONRESIDENT);
            NameLength = bytes[0x09 + offset] * 2;
            NameOffset = BitConverter.ToUInt16(bytes, 0x0A + offset);
            Flags = BitConverter.ToUInt16(bytes, 0x0C + offset);
            Id = BitConverter.ToUInt16(bytes, 0x0E + offset);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class Data : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] RawData;

        #endregion Properties

        #region Constructors

        internal Data(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;
            RawData = Helper.GetSubArray(bytes, (0x00 + offset), (int)header.AttrSize);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataRun
    {
        #region Properties

        private readonly string Volume;

        /// <summary>
        /// 
        /// </summary>
        public readonly long StartCluster;

        /// <summary>
        /// 
        /// </summary>
        public readonly long ClusterLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Sparse;

        #endregion Properties

        #region Constructors

        private DataRun()
        {

        }

        private DataRun(byte[] bytes, int offset, int lengthByteCount, int offsetByteCount, DataRun previousDR, string volume)
        {
            Volume = volume;

            if (offsetByteCount == 0)
            {
                Sparse = true;
            }

            if (!((lengthByteCount > 8) || (offsetByteCount > 8)))
            {
                byte[] DataRunLengthBytes = new byte[0x08];
                Array.Copy(bytes, offset + 1, DataRunLengthBytes, 0x00, lengthByteCount);
                ClusterLength = BitConverter.ToInt64(DataRunLengthBytes, 0x00);

                byte[] offsetBytes = new byte[0x08];
                int arrayOffset = 0x08 - offsetByteCount;
                Array.Copy(bytes, offset + 0x01 + lengthByteCount, offsetBytes, arrayOffset, offsetByteCount);
                long relativeOffset = BitConverter.ToInt64(offsetBytes, 0x00) >> arrayOffset * 0x08;
                StartCluster = previousDR.StartCluster + relativeOffset;
            }
        }

        #endregion Constructors

        #region Static Methods

        internal static DataRun[] GetInstances(byte[] bytes, string volume)
        {
            List<DataRun> datarunList = new List<DataRun>();
            int i = 0;
            DataRun dr = new DataRun();

            while (i < bytes.Length - 1)
            {
                int DataRunLengthByteCount = bytes[i] & 0x0F;
                int DataRunOffsetByteCount = ((bytes[i] & 0xF0) >> 4);

                if (DataRunLengthByteCount == 0)
                {
                    break;
                }
                else if ((i + DataRunLengthByteCount + DataRunOffsetByteCount + 1) > bytes.Length)
                {
                    break;
                }

                dr = Get(bytes, i, DataRunLengthByteCount, DataRunOffsetByteCount, dr, volume);
                datarunList.Add(dr);
                i += (1 + DataRunLengthByteCount + DataRunOffsetByteCount);
            }

            return datarunList.ToArray();
        }

        internal static DataRun[] GetInstances(byte[] bytes, int offset, string volume)
        {
            List<DataRun> datarunList = new List<DataRun>();

            int i = offset;
            DataRun dr = new DataRun();

            while (i < bytes.Length - 1)
            {
                int DataRunLengthByteCount = bytes[i] & 0x0F;
                int DataRunOffsetByteCount = ((bytes[i] & 0xF0) >> 4);

                if (DataRunLengthByteCount == 0)
                {
                    break;
                }
                else if ((i + DataRunLengthByteCount + DataRunOffsetByteCount + 1) > bytes.Length)
                {
                    break;
                }

                dr = Get(bytes, i, DataRunLengthByteCount, DataRunOffsetByteCount, dr, volume);
                datarunList.Add(dr);
                i += (1 + DataRunLengthByteCount + DataRunOffsetByteCount);
            }

            return datarunList.ToArray();
        }

        private static DataRun Get(byte[] bytes, int offset, int lengthByteCount, int offsetByteCount, DataRun previousDR, string volume)
        {
            return new DataRun(bytes, offset, lengthByteCount, offsetByteCount, previousDR, volume);
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            VolumeBootRecord vbr = VolumeBootRecord.Get(this.Volume);
            return Helper.readDrive(this.Volume, this.StartCluster * vbr.BytesPerCluster, this.ClusterLength * vbr.BytesPerCluster);
        }

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class EA : FileRecordAttribute
    {
        #region Properties

        #endregion Properties

        #region Constructors

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class EAInformation : FileRecordAttribute
    {
        #region Properties

        #endregion Properties

        #region Constructors

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileName : FileRecordAttribute
    {
        #region Constants

        private const byte ATTR_FILENAME_NAMESPACE_POSIX = 0x00;
        private const byte ATTR_FILENAME_NAMESPACE_WIN32 = 0x01;
        private const byte ATTR_FILENAME_NAMESPACE_DOS = 0x02;

        #endregion Constants

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Filename;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ParentSequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong ParentRecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly int Namespace;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong AllocatedSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RealSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ER;

        private readonly byte NameLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifiedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ChangedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BornTime;

        #endregion Properties

        #region Constructors

        internal FileName(byte[] bytes)
        {
            try
            {
                // FILE_NAME Attribute
                ParentRecordNumber = (BitConverter.ToUInt64(bytes, 0x00) & 0x0000FFFFFFFFFFFF);
                ParentSequenceNumber = (BitConverter.ToUInt16(bytes, 0x06));
                BornTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x08));
                ChangedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x10));
                ModifiedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x18));
                AccessedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x20));
                AllocatedSize = BitConverter.ToUInt64(bytes, 0x28);
                RealSize = BitConverter.ToUInt64(bytes, 0x30);
                Flags = BitConverter.ToUInt32(bytes, 0x38);
                ER = BitConverter.ToUInt32(bytes, 0x3C);
                NameLength = bytes[0x40];
                Namespace = Convert.ToInt32(bytes[0x41]);
                Filename = Encoding.Unicode.GetString(bytes, 0x42, NameLength * 2).TrimEnd('\0');
            }
            catch
            {

            }
        }

        internal FileName(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // FILE_NAME Attribute
            ParentRecordNumber = (BitConverter.ToUInt64(bytes, 0x00 + offset) & 0x0000FFFFFFFFFFFF);
            ParentSequenceNumber = (BitConverter.ToUInt16(bytes, 0x06 + offset));
            BornTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x08 + offset));
            ChangedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x10 + offset));
            ModifiedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x18 + offset));
            AccessedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x20 + offset));
            AllocatedSize = BitConverter.ToUInt64(bytes, 0x28 + offset);
            RealSize = BitConverter.ToUInt64(bytes, 0x30 + offset);
            Flags = BitConverter.ToUInt32(bytes, 0x38 + offset);
            ER = BitConverter.ToUInt32(bytes, 0x3C + offset);
            NameLength = bytes[0x40 + offset];
            Namespace = Convert.ToInt32(bytes[0x41 + offset]);

            // Get FileName
            Filename = Encoding.Unicode.GetString(bytes, 0x42 + offset, NameLength * 2).TrimEnd('\0');
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileRecordAttribute
    {
        #region Constants

        private const int COMMONHEADERSIZE = 0x10;
        private const int RESIDENTHEADERSIZE = 0x08;
        private const int NONRESIDENTHEADERSIZE = 0x30;

        #endregion Constants

        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum ATTR_TYPE
        {

            /// <summary>
            /// 
            /// </summary>
            STANDARD_INFORMATION = 0x10,

            /// <summary>
            /// 
            /// </summary>
            ATTRIBUTE_LIST = 0x20,

            /// <summary>
            /// 
            /// </summary>
            FILE_NAME = 0x30,

            /// <summary>
            /// 
            /// </summary>
            OBJECT_ID = 0x40,

            /// <summary>
            /// 
            /// </summary>
            SECURITY_DESCRIPTOR = 0x50,

            /// <summary>
            /// 
            /// </summary>
            VOLUME_NAME = 0x60,

            /// <summary>
            /// 
            /// </summary>
            VOLUME_INFORMATION = 0x70,

            /// <summary>
            /// 
            /// </summary>
            DATA = 0x80,

            /// <summary>
            /// 
            /// </summary>
            INDEX_ROOT = 0x90,

            /// <summary>
            /// 
            /// </summary>
            INDEX_ALLOCATION = 0xA0,

            /// <summary>
            /// 
            /// </summary>
            BITMAP = 0xB0,

            /// <summary>
            /// 
            /// </summary>
            REPARSE_POINT = 0xC0,

            /// <summary>
            /// 
            /// </summary>
            EA_INFORMATION = 0xD0,

            /// <summary>
            /// 
            /// </summary>
            EA = 0xE0,

            /// <summary>
            /// 
            /// </summary>
            LOGGED_UTILITY_STREAM = 0x100,


            /// <summary>
            /// 
            /// </summary>
            ATTR_FLAG_COMPRESSED = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            ATTR_FLAG_ENCRYPTED = 0x4000,

            /// <summary>
            /// 
            /// </summary>
            ATTR_FLAG_SPARSE = 0x8000
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ATTR_TYPE Name;

        /// <summary>
        /// 
        /// </summary>
        public string NameString;

        internal bool NonResident;

        /// <summary>
        /// 
        /// </summary>
        public ushort AttributeId;

        internal int AttributeSize;

        #endregion Properties

        #region Static Methods

        internal static FileRecordAttribute[] GetInstances(byte[] bytes, int offset, int bytesPerFileRecord, string volume)
        {
            List<FileRecordAttribute> AttributeList = new List<FileRecordAttribute>();

            int i = offset;

            //while (i < offset + bytesPerFileRecord)
            while (i < offset + (bytesPerFileRecord - (offset % bytesPerFileRecord)))
            {
                // Get attribute size
                int attrSize = BitConverter.ToInt32(bytes, i + 0x04);

                if((attrSize == 0) || (attrSize + i > offset + bytesPerFileRecord))
                {
                    break;
                }

                FileRecordAttribute attr = Get(bytes, i, volume);

                i += attrSize;

                if (attr != null)
                {
                    AttributeList.Add(attr);
                }
            } 

            return AttributeList.ToArray();
        }

        internal static FileRecordAttribute Get(byte[] bytes, int offset, string volume)
        {
            #region CommonHeader

            // Instantiate a Common Header Object
            CommonHeader commonHeader = new CommonHeader(bytes, offset);

            // Decode Name byte[] into Unicode String
            string attributeName = Encoding.Unicode.GetString(bytes, commonHeader.NameOffset + offset, commonHeader.NameLength);

            #endregion CommonHeader

            #region NonResidentAttribute

            // If Attribute is NonResident
            if (commonHeader.NonResident)
            {
                #region NonResidentHeader

                // Instantiate a Resident Header Object
                NonResidentHeader nonresidentHeader = new NonResidentHeader(bytes, commonHeader, COMMONHEADERSIZE + offset);

                #endregion NonResidentHeader

                #region DataRun

                int headerSize = 0x00;

                if (commonHeader.NameOffset != 0x00)
                {
                    headerSize = commonHeader.NameOffset + commonHeader.NameLength + (commonHeader.NameLength % 8);
                }
                else
                {
                    headerSize = COMMONHEADERSIZE + NONRESIDENTHEADERSIZE;
                }

                int attributeoffset = headerSize + offset;

                return new NonResident(nonresidentHeader, bytes, attributeoffset, attributeName, volume);

                #endregion DataRun
            }

            #endregion NonResidentAttribute

            #region ResidentAttribute
            // Else Attribute is Resident
            else
            {
                #region ResidentHeader

                // Instantiate a Resident Header Object
                ResidentHeader residentHeader = new ResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE + offset, RESIDENTHEADERSIZE), commonHeader);

                #endregion ResidentHeader
                
                int headerSize = COMMONHEADERSIZE + RESIDENTHEADERSIZE + commonHeader.NameLength;
                int attributeoffset = headerSize + offset;

                #region ATTRSwitch

                switch (residentHeader.commonHeader.ATTRType)
                {
                    case (Int32)FileRecordAttribute.ATTR_TYPE.STANDARD_INFORMATION:
                        return new StandardInformation(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.ATTRIBUTE_LIST:
                        return new AttributeList(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.FILE_NAME:
                        return new FileName(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.OBJECT_ID:
                        return new ObjectId(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.VOLUME_NAME:
                        return new VolumeName(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.VOLUME_INFORMATION:
                        return new VolumeInformation(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.DATA:
                        return new Data(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.INDEX_ROOT:
                        return new IndexRoot(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.EA:
                        return null;

                    case (Int32)FileRecordAttribute.ATTR_TYPE.EA_INFORMATION:
                        return null;

                    default:
                        return null;
                }

                #endregion ATTRSwitch
            }
            #endregion ResidentAttribute
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class IndexAllocation : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly IndexEntry[] Entries;

        #endregion Properties

        #region Constructors

        internal IndexAllocation(NonResident header, string volume)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = header.NameString;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // Get IndexAllocation Bytes
            byte[] bytes = header.GetBytes();

            // Instantiate empty IndexEntry List
            List<IndexEntry> indexEntryList = new List<IndexEntry>();

            // Iterate through IndexBlocks (4096 bytes in size)
            for (int offset = 0; offset < bytes.Length; offset += 4096)
            {
                // Detemine size of Update Sequence
                ushort usOffset = BitConverter.ToUInt16(bytes, offset + 0x04);
                ushort usSize = BitConverter.ToUInt16(bytes, offset + 0x06);
                int indexBlockSize = usOffset + (usSize * 2);

                if (indexBlockSize == 0)
                {
                    break;
                }

                IndexBlock.ApplyFixup(ref bytes, offset);

                // Instantiate IndexBlock Object (Header)
                IndexBlock indexBlock = new IndexBlock(Helper.GetSubArray(bytes, offset, indexBlockSize));

                if (indexBlock.Signature == "INDX")
                {
                    // Create byte array for IndexEntry object
                    // 0x18 represents the offset of the EntryOffset value, so it must be added on
                    byte[] indexEntryBytes = Helper.GetSubArray(bytes, offset + (int)indexBlock.EntryOffset + 0x18, (int)indexBlock.TotalEntrySize);

                    int entryOffset = 0;

                    do
                    {
                        // Instantiate an IndexEntry Object
                        IndexEntry indexEntry = new IndexEntry(Helper.GetSubArray(indexEntryBytes, entryOffset, BitConverter.ToUInt16(indexEntryBytes, entryOffset + 0x08)));
                        entryOffset += indexEntry.Size;

                        // Check if entry is the last in the Entry array
                        if (indexEntry.Flags == 0x02 || indexEntry.Flags == 0x03)
                        {
                            break;
                        }

                        // Add IndexEntry Object to list
                        indexEntryList.Add(indexEntry);

                    } while (entryOffset < indexEntryBytes.Length);
                }
            }
            Entries = indexEntryList.ToArray();
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class IndexAllocationTest : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly IndexEntry[] Entries;

        #endregion Properties

        #region Constructors

        internal IndexAllocationTest(NonResident header, string volume)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = header.NameString;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            // Instantiate empty IndexEntry List
            List<IndexEntry> indexEntryList = new List<IndexEntry>();

            foreach (DataRun dr in header.DataRun)
            {
                // Get IndexAllocation Bytes
                byte[] bytes = dr.GetBytes();
                // Detemine size of Update Sequence
                ushort usOffset = BitConverter.ToUInt16(bytes, +0x04);
                ushort usSize = BitConverter.ToUInt16(bytes, +0x06);
                int indexBlockSize = usOffset + (usSize * 2);

                if (indexBlockSize == 0)
                {
                    break;
                }

                //IndexBlock.ApplyFixup(ref bytes, offset);

                // Instantiate IndexBlock Object (Header)
                IndexBlock indexBlock = new IndexBlock(Helper.GetSubArray(bytes, 0x00, indexBlockSize));

                // Create byte array for IndexEntry object
                // 0x18 represents the offset of the EntryOffset value, so it must be added on
                byte[] indexEntryBytes = Helper.GetSubArray(bytes, (int)indexBlock.EntryOffset + 0x18, (int)indexBlock.TotalEntrySize);

                int entryOffset = 0;

                do
                {
                    // Instantiate an IndexEntry Object
                    IndexEntry indexEntry = new IndexEntry(Helper.GetSubArray(indexEntryBytes, entryOffset, BitConverter.ToUInt16(indexEntryBytes, entryOffset + 0x08)));
                    entryOffset += indexEntry.Size;

                    // Check if entry is the last in the Entry array
                    if (indexEntry.Flags == 0x02 || indexEntry.Flags == 0x03)
                    {
                        break;
                    }

                    // Add IndexEntry Object to list
                    indexEntryList.Add(indexEntry);

                } while (entryOffset < indexEntryBytes.Length);
            }
            Entries = indexEntryList.ToArray();
        }

        #endregion Constructors
    }

    internal class IndexBlock
    {
        #region Properties

        // Index Block Header
        internal readonly string Signature;         // "INDX"
        internal ushort OffsetOfUS;                 // Offset of Update Sequence
        internal ushort SizeOfUS;                   // Size in words of Update Sequence Number & Array
        internal readonly ushort UpdateSequenceNumber;
        internal readonly byte[] UpdateSequenceArray;
        internal readonly ulong LSN;                // $LogFile Sequence Number
        internal readonly ulong VCN;                // VCN of this index block in the index allocation

        // Index Header
        internal readonly uint EntryOffset;         // Offset of the index entries, relative to this address(0x18)
        internal readonly uint TotalEntrySize;      // Total size of the index entries
        internal readonly uint AllocEntrySize;      // Allocated size of index entries
        internal readonly byte NotLeaf;             // 1 if not leaf node (has children)

        #endregion Properties

        #region Constructors

        internal IndexBlock(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x04);
            OffsetOfUS = BitConverter.ToUInt16(bytes, 0x04);
            SizeOfUS = BitConverter.ToUInt16(bytes, 0x06);
            UpdateSequenceNumber = BitConverter.ToUInt16(bytes, OffsetOfUS);
            UpdateSequenceArray = Helper.GetSubArray(bytes, (OffsetOfUS + 2), (2 * SizeOfUS) - 2);
            LSN = BitConverter.ToUInt64(bytes, 0x08);
            VCN = BitConverter.ToUInt64(bytes, 0x10);

            // Index Header
            EntryOffset = BitConverter.ToUInt32(bytes, 0x18);
            TotalEntrySize = BitConverter.ToUInt32(bytes, 0x1C);
            AllocEntrySize = BitConverter.ToUInt32(bytes, 0x20);
            NotLeaf = bytes[0x24];
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        internal static void ApplyFixup(ref byte[] bytes, int offset)
        {
            // Take UpdateSequence into account
            ushort usoffset = BitConverter.ToUInt16(bytes, 4);
            ushort ussize = BitConverter.ToUInt16(bytes, 6);

            if (ussize != 0)
            {
                ushort UpdateSequenceNumber = BitConverter.ToUInt16(bytes, usoffset + offset);
                byte[] UpdateSequenceArray = Helper.GetSubArray(bytes, (usoffset + 2 + offset), (2 * ussize));

                bytes[0x1FE + offset] = UpdateSequenceArray[0];
                bytes[0x1FF + offset] = UpdateSequenceArray[1];
                bytes[0x3FE + offset] = UpdateSequenceArray[2];
                bytes[0x3FF + offset] = UpdateSequenceArray[3];
                bytes[0x5FE + offset] = UpdateSequenceArray[4];
                bytes[0x5FF + offset] = UpdateSequenceArray[5];
                bytes[0x7FE + offset] = UpdateSequenceArray[6];
                bytes[0x7FF + offset] = UpdateSequenceArray[7];
                bytes[0x9FE + offset] = UpdateSequenceArray[8];
                bytes[0x9FF + offset] = UpdateSequenceArray[9];
                bytes[0xBFE + offset] = UpdateSequenceArray[10];
                bytes[0xBFF + offset] = UpdateSequenceArray[11];
                bytes[0xDFE + offset] = UpdateSequenceArray[12];
                bytes[0xDFF + offset] = UpdateSequenceArray[13];
                bytes[0xFFE + offset] = UpdateSequenceArray[14];
                bytes[0xFFF + offset] = UpdateSequenceArray[15];
            }
        }

        #endregion StaticMethods
    }

    /// <summary>
    /// 
    /// </summary>
    public class IndexRoot : FileRecordAttribute
    {
        // ATTR_HEADER_RESIDENT
        // IndexRoot
        // IndexHeader
        // IndexEntry[]

        #region Properties

        // Index Root
        /// <summary>
        /// 
        /// </summary>
        public readonly ATTR_TYPE AttributeType;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CollationSortingRule;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint IndexSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte ClustersPerIndexRecord;

        // IndexHeader
        private readonly int StartOffset;

        private readonly int TotalSize;

        private readonly int AllocatedSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly INDEX_ROOT_FLAGS Flags;

        // IndexEntry[]
        private readonly byte[] EntryBytes;

        /// <summary>
        /// 
        /// </summary>
        public readonly IndexEntry[] Entries;

        #endregion Properties

        #region Constructors

        internal IndexRoot(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            // Get ResidentHeader (includes Common Header)
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // IndexRoot
            AttributeType = (ATTR_TYPE)BitConverter.ToUInt32(bytes, 0x00 + offset);
            CollationSortingRule = BitConverter.ToUInt32(bytes, 0x04 + offset);
            IndexSize = BitConverter.ToUInt32(bytes, 0x08 + offset);
            ClustersPerIndexRecord = bytes[0x0C + offset];

            // IndexHeader
            StartOffset = (BitConverter.ToInt32(bytes, 0x10 + offset) + 0x10 + offset);  // Add 0x10 bytes to start offset to account for its offset
            TotalSize = BitConverter.ToInt32(bytes, 0x14 + offset);
            AllocatedSize = BitConverter.ToInt32(bytes, 0x18 + offset);
            Flags = ((INDEX_ROOT_FLAGS)BitConverter.ToUInt32(bytes, 0x1C + offset));

            // IndexEntry[]
            EntryBytes = Helper.GetSubArray(bytes, StartOffset, TotalSize);

            // Iterate through IndexEntry object
            int indexEntryOffset = 0;

            if (AttributeType == ATTR_TYPE.FILE_NAME)
            {
                // Instantiate empty IndexEntry List
                List<IndexEntry> entryList = new List<IndexEntry>();

                while (indexEntryOffset < (EntryBytes.Length - 0x10))
                {
                    // There has to be a better way
                    if (BitConverter.ToUInt16(EntryBytes, 0x0A + indexEntryOffset) == 0)
                    {
                        break;
                    }

                    // Instantiate an IndexEntry Object
                    IndexEntry indexEntry = new IndexEntry(EntryBytes, indexEntryOffset);

                    // Add IndexEntry Object to FileName List
                    entryList.Add(indexEntry);

                    // Increment indexEntryOffset
                    indexEntryOffset += indexEntry.Size;
                }

                Entries = entryList.ToArray();
            }
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class NonResident : FileRecordAttribute
    {
        #region Properties

        private string Volume;

        internal CommonHeader commonHeader;             // Common Header Object

        internal ulong StartVCN;                        // Starting VCN

        internal ulong LastVCN;                         // Last VCN

        internal ushort DataRunOffset;                  // Offset to the Data Runs

        internal ushort CompUnitSize;                   // Compression unit size

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong AllocatedSize;            // Allocated size of the attribute

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RealSize;                 // Real size of the attribute

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong InitializedSize;          // Initialized data size of the stream 

        /// <summary>
        /// 
        /// </summary>
        public readonly DataRun[] DataRun;

        #endregion Properties

        #region Constructors

        internal NonResident(NonResidentHeader header, byte[] bytes, int offset, string attrName, string volume)
        {
            Volume = volume;

            // Attr Object
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // NonResident Attribute
            commonHeader = header.commonHeader;
            StartVCN = header.StartVCN;
            LastVCN = header.LastVCN;
            DataRunOffset = header.DataRunOffset;
            CompUnitSize = header.CompUnitSize;
            AllocatedSize = header.AllocatedSize;
            RealSize = header.RealSize;
            InitializedSize = header.InitializedSize;
            DataRun = Ntfs.DataRun.GetInstances(bytes, offset, volume);
        }

        #endregion Constructors

        #region InstanceMethods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] fileBytes = new byte[this.RealSize];

            int offset = 0;

            Helper.getVolumeName(ref this.Volume);

            using (FileStream streamToRead = Helper.getFileStream(this.Volume))
            {
                VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

                foreach (DataRun dr in this.DataRun)
                {
                    if (dr.Sparse)
                    {
                        // Figure out how to add Sparse Bytes
                    }
                    else
                    {
                        long startOffset = VBR.BytesPerCluster * dr.StartCluster;
                        long count = VBR.BytesPerCluster * dr.ClusterLength;
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, startOffset, count);

                        if ((offset + count) <= fileBytes.Length)
                        {
                            // Save dataRunBytes to fileBytes
                            Array.Copy(dataRunBytes, 0x00, fileBytes, offset, dataRunBytes.Length);

                            // Increment Offset Value
                            offset += dataRunBytes.Length;
                        }
                        else
                        {
                            Array.Copy(dataRunBytes, 0x00, fileBytes, offset, (fileBytes.Length - offset));
                            break;
                        }
                    }
                }
                return fileBytes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytesTest()
        {
            Helper.getVolumeName(ref this.Volume);

            List<byte> byteList = new List<byte>();

            using (FileStream streamToRead = Helper.getFileStream(this.Volume))
            {
                VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

                foreach (DataRun dr in this.DataRun)
                {
                    if (dr.Sparse)
                    {
                        // Figure out how to add Sparse Bytes
                    }
                    else
                    {
                        long startOffset = VBR.BytesPerCluster * dr.StartCluster;
                        Console.WriteLine(this.Volume);
                        long count = VBR.BytesPerCluster * dr.ClusterLength;
                        byteList.AddRange(Helper.readDrive(streamToRead, startOffset, count));
                    }
                }

                return Helper.GetSubArray(byteList.ToArray(), 0, (long)this.RealSize);
            }
        }

        internal byte[] GetBytes(VolumeBootRecord VBR)
        {
            byte[] fileBytes = new byte[this.RealSize];

            int offset = 0;

            Helper.getVolumeName(ref this.Volume);

            using (FileStream streamToRead = Helper.getFileStream(this.Volume))
            {
                foreach (DataRun dr in this.DataRun)
                {
                    if (dr.Sparse)
                    {
                        // Figure out how to add Sparse Bytes
                    }
                    else
                    {
                        long startOffset = VBR.BytesPerCluster * dr.StartCluster;
                        long count = VBR.BytesPerCluster * dr.ClusterLength;
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, startOffset, count);

                        if ((offset + count) <= fileBytes.Length)
                        {
                            // Save dataRunBytes to fileBytes
                            Array.Copy(dataRunBytes, 0, fileBytes, offset, dataRunBytes.Length);

                            // Increment Offset Value
                            offset += dataRunBytes.Length;
                        }
                        else
                        {
                            Array.Copy(dataRunBytes, 0, fileBytes, offset, (fileBytes.Length - offset));
                            break;
                        }
                    }
                }
                return fileBytes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetSlack()
        {
            Helper.getVolumeName(ref this.Volume);

            using (FileStream streamToRead = Helper.getFileStream(this.Volume))
            {
                if (this.DataRun.Length != 0)
                {
                    VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);
                    ulong slackSize = this.AllocatedSize - this.RealSize;
                    if ((slackSize > 0) && (slackSize <= (ulong)VBR.BytesPerCluster))
                    {
                        DataRun dr = this.DataRun[this.DataRun.Length - 1];
                        long lastCluster = dr.StartCluster + dr.ClusterLength - 1;
                        byte[] dataRunBytes = Helper.readDrive(streamToRead, VBR.BytesPerCluster * lastCluster, VBR.BytesPerCluster);
                        byte[] slackBytes = new byte[slackSize];
                        Array.Copy(dataRunBytes, VBR.BytesPerCluster - ((int)this.AllocatedSize - (int)this.RealSize), slackBytes, 0x00, slackBytes.Length);
                        return slackBytes;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion Instance Methods
    }

    class NonResidentHeader
    {
        #region Properties

        internal CommonHeader commonHeader;
        internal ulong StartVCN;		        // Starting VCN
        internal ulong LastVCN;		            // Last VCN
        internal ushort DataRunOffset;	        // Offset to the Data Runs
        internal ushort CompUnitSize;	        // Compression unit size
        internal readonly ulong AllocatedSize;    // Allocated size of the attribute
        internal readonly ulong RealSize;         // Real size of the attribute
        internal readonly ulong InitializedSize;  // Initialized data size of the stream 

        #endregion Properties

        #region Constructors

        internal NonResidentHeader(byte[] bytes, CommonHeader common)
        {
            commonHeader = common;
            StartVCN = BitConverter.ToUInt64(bytes, 0x00);
            LastVCN = BitConverter.ToUInt64(bytes, 0x08);
            DataRunOffset = BitConverter.ToUInt16(bytes, 0x10);
            CompUnitSize = BitConverter.ToUInt16(bytes, 0x12);
            AllocatedSize = BitConverter.ToUInt64(bytes, 0x18);
            RealSize = BitConverter.ToUInt64(bytes, 0x20);
            InitializedSize = BitConverter.ToUInt64(bytes, 0x28);
        }

        internal NonResidentHeader(byte[] bytes, CommonHeader common, int offset)
        {
            commonHeader = common;
            StartVCN = BitConverter.ToUInt64(bytes, 0x00 + offset);
            LastVCN = BitConverter.ToUInt64(bytes, 0x08 + offset);
            DataRunOffset = BitConverter.ToUInt16(bytes, 0x10 + offset);
            CompUnitSize = BitConverter.ToUInt16(bytes, 0x12 + offset);
            AllocatedSize = BitConverter.ToUInt64(bytes, 0x18 + offset);
            RealSize = BitConverter.ToUInt64(bytes, 0x20 + offset);
            InitializedSize = BitConverter.ToUInt64(bytes, 0x28 + offset);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class ObjectId : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid ObjectIdGuid;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid BirthVolumeId;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid BirthObjectId;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid BirthDomainId;

        #endregion Properties

        #region Constructors

        internal ObjectId(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            ObjectIdGuid = new Guid(Helper.GetSubArray(bytes, 0x00 + offset, 0x10));

            if (!(bytes.Length < 0x20))
            {
                BirthVolumeId = new Guid(Helper.GetSubArray(bytes, 0x10 + offset, 0x10));

                if (!(bytes.Length < 0x30))
                {
                    BirthObjectId = new Guid(Helper.GetSubArray(bytes, 0x20 + offset, 0x10));

                    if (bytes.Length == 0x40)
                    {
                        BirthDomainId = new Guid(Helper.GetSubArray(bytes, 0x30 + offset, 0x10));
                    }
                }
            }
        }

        #endregion Constructors
    }

    class ResidentHeader
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        internal CommonHeader commonHeader;

        /// <summary>
        /// 
        /// </summary>
        internal uint AttrSize;

        /// <summary>
        /// 
        /// </summary>
        internal ushort AttrOffset;

        /// <summary>
        /// 
        /// </summary>
        internal byte IndexedFlag;

        #endregion Properties

        #region Constructors

        internal ResidentHeader(byte[] bytes, CommonHeader common)
        {
            commonHeader = common;
            AttrSize = BitConverter.ToUInt32(bytes, 0);
            AttrOffset = BitConverter.ToUInt16(bytes, 4);
            IndexedFlag = bytes[6];
        }

        internal ResidentHeader(byte[] bytes, CommonHeader common, int offset)
        {
            commonHeader = common;
            AttrSize = BitConverter.ToUInt32(bytes, 0x00 + offset);
            AttrOffset = BitConverter.ToUInt16(bytes, 0x04 + offset);
            IndexedFlag = bytes[0x06 + offset];
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class StandardInformation : FileRecordAttribute
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum ATTR_STDINFO_PERMISSION : uint
        {
            /// <summary>
            /// 
            /// </summary>
            READONLY = 0x00000001,

            /// <summary>
            /// 
            /// </summary>
            HIDDEN = 0x00000002,

            /// <summary>
            /// 
            /// </summary>
            SYSTEM = 0x00000004,

            /// <summary>
            /// 
            /// </summary>
            ARCHIVE = 0x00000020,

            /// <summary>
            /// 
            /// </summary>
            DEVICE = 0x00000040,

            /// <summary>
            /// 
            /// </summary>
            NORMAL = 0x00000080,

            /// <summary>
            /// 
            /// </summary>
            TEMP = 0x00000100,

            /// <summary>
            /// 
            /// </summary>
            SPARSE = 0x00000200,

            /// <summary>
            /// 
            /// </summary>
            REPARSE = 0x00000400,

            /// <summary>
            /// 
            /// </summary>
            COMPRESSED = 0x00000800,

            /// <summary>
            /// 
            /// </summary>
            OFFLINE = 0x00001000,

            /// <summary>
            /// 
            /// </summary>
            NCI = 0x00002000,

            /// <summary>
            /// 
            /// </summary>
            ENCRYPTED = 0x00004000
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BornTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifiedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ChangedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly ATTR_STDINFO_PERMISSION Permission;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint MaxVersionNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint VersionNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClassId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint OwnerId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SecurityId;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong QuotaCharged;

        /// <summary>
        /// 
        /// </summary>
        public readonly long UpdateSequenceNumber;

        #endregion Properties

        #region Constructors

        internal StandardInformation(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            BornTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x00 + offset));
            ModifiedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x08 + offset));
            ChangedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x10 + offset));
            AccessedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x18 + offset));
            Permission = ((ATTR_STDINFO_PERMISSION)BitConverter.ToUInt32(bytes, 0x20 + offset));
            MaxVersionNumber = BitConverter.ToUInt32(bytes, 0x24 + offset);
            VersionNumber = BitConverter.ToUInt32(bytes, 0x28 + offset);
            ClassId = BitConverter.ToUInt32(bytes, 0x2C + offset);

            if (header.AttrSize == 0x48)
            {
                OwnerId = BitConverter.ToUInt32(bytes, 0x30 + offset);
                SecurityId = BitConverter.ToUInt32(bytes, 0x34 + offset);
                QuotaCharged = BitConverter.ToUInt64(bytes, 0x38 + offset);
                UpdateSequenceNumber = BitConverter.ToInt64(bytes, 0x40 + offset);
            }
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class VolumeInformation : FileRecordAttribute
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum ATTR_VOLINFO
        {
            /// <summary>
            /// Dirty
            /// </summary>
            FLAG_DIRTY = 0x0001,

            /// <summary>
            /// Resize logfile
            /// </summary>
            FLAG_RLF = 0x0002,

            /// <summary>
            /// Upgrade on mount
            /// </summary>
            FLAG_UOM = 0x0004,

            /// <summary>
            /// Mounted on NT4
            /// </summary>
            FLAG_MONT = 0x0008,

            /// <summary>
            /// Delete USN underway
            /// </summary>
            FLAG_DUSN = 0x0010,

            /// <summary>
            /// Repair object Ids
            /// </summary>
            FLAG_ROI = 0x0020,

            /// <summary>
            /// Modified by chkdsk
            /// </summary>
            FLAG_MBC = 0x8000
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly Version Version;

        /// <summary>
        /// 
        /// </summary>
        public readonly ATTR_VOLINFO Flags;

        #endregion Properties

        #region Constructors

        internal VolumeInformation(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            Version = new Version(bytes[0x08 + offset], bytes[0x09 + offset]);
            Flags = (ATTR_VOLINFO)BitConverter.ToInt16(bytes, 0x0A + offset);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static VolumeInformation Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return Get(FileRecord.Get(volume, MftIndex.VOLUME_INDEX, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static VolumeInformation GetByPath(string path)
        {
            return Get(FileRecord.Get(path, true));
        }

        private static VolumeInformation Get(FileRecord record)
        {
            foreach (FileRecordAttribute attr in record.Attribute)
            {
                VolumeInformation volInfo = attr as VolumeInformation;
                if (volInfo != null)
                {
                    return volInfo;
                }
            }
            throw new Exception("No VOLUME_INFORMATION attribute found.");
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class VolumeName : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string VolumeNameString;

        #endregion Properties

        #region Constructors

        internal VolumeName(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            VolumeNameString = Encoding.Unicode.GetString(bytes, 0x00 + offset, (int)header.AttrSize);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static VolumeName Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return Get(FileRecord.Get(volume, MftIndex.VOLUME_INDEX, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static VolumeName GetByPath(string path)
        {
            return Get(FileRecord.Get(path, true));
        }

        private static VolumeName Get(FileRecord fileRecord)
        {
            foreach (FileRecordAttribute attr in fileRecord.Attribute)
            {
                VolumeName volName = attr as VolumeName;
                if (volName != null)
                {
                    return volName;
                }
            }
            throw new Exception("No VOLUME_NAME attribute found.");
        }

        #endregion Static Methods
    }
}