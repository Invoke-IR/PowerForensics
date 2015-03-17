using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InvokeIR.PowerForensics.NTFS
{


    public class NTFSVolumeData
    {

        struct NTFS_VOLUME_DATA_BUFFER
        {
            internal ulong VolumeSerialNumber;
            internal long NumberSectors;
            internal long TotalClusters;
            internal long FreeClusters;
            internal long TotalReserved;
            internal int BytesPerSector;
            internal int BytesPerCluster;
            internal int BytesPerFileRecordSegment;
            internal int ClustersPerFileRecordSegment;
            internal long MftValidDataLength;
            internal long MftStartLcn;
            internal long Mft2StartLcn;
            internal long MftZoneStart;
            internal long MftZoneEnd;

            internal NTFS_VOLUME_DATA_BUFFER(byte[] bytes)
            {
                VolumeSerialNumber = BitConverter.ToUInt64(bytes, 0);
                NumberSectors = BitConverter.ToInt64(bytes, 8);
                TotalClusters = BitConverter.ToInt64(bytes, 16);
                FreeClusters = BitConverter.ToInt64(bytes, 24);
                TotalReserved = BitConverter.ToInt64(bytes, 32);
                BytesPerSector = BitConverter.ToInt32(bytes, 40);
                BytesPerCluster = BitConverter.ToInt32(bytes, 44);
                BytesPerFileRecordSegment = BitConverter.ToInt32(bytes, 48);
                ClustersPerFileRecordSegment = BitConverter.ToInt32(bytes, 52);
                MftValidDataLength = BitConverter.ToInt64(bytes, 56);
                MftStartLcn = BitConverter.ToInt64(bytes, 64);
                Mft2StartLcn = BitConverter.ToInt64(bytes, 72);
                MftZoneStart = BitConverter.ToInt64(bytes, 80);
                MftZoneEnd = BitConverter.ToInt64(bytes, 88);
            }
        }

        public long VolumeSize_MB;
        public long TotalSectors;
        public long TotalClusters;
        public long FreeClusters;
        public long FreeSpace_MB;
        public int BytesPerSector;
        public int BytesPerCluster;
        public int BytesPerMFTRecord;
        public int ClustersPerMFTRecord;
        public long MFTSize_MB;
        public long MFTStartCluster;
        public long MFTZoneClusterStart;
        public long MFTZoneClusterEnd;
        public long MFTZoneSize;
        public long MFTMirrorStart;

        internal NTFSVolumeData(long totalSectors, long totalClusters, long freeClusters, int bytesPerSector, int bytesPerCluster, int bytesPerMFTRecord, int clustersPerMFTRecord, long mftValidDataLength, long mftStartCluster, long mftZoneClusterStart, long mftZoneClusterEnd, long mftMirrorStart)
        {
            VolumeSize_MB = (totalClusters * bytesPerCluster) / 0x100000;
            TotalSectors = totalSectors;
            TotalClusters = totalClusters;
            FreeClusters = freeClusters;
            FreeSpace_MB = ((totalClusters - freeClusters) * bytesPerCluster) / 0x100000;
            BytesPerSector = bytesPerSector;
            BytesPerCluster = bytesPerCluster;
            BytesPerMFTRecord = bytesPerMFTRecord;
            ClustersPerMFTRecord = clustersPerMFTRecord;
            MFTSize_MB = (mftValidDataLength) / 0x100000;
            MFTStartCluster = mftStartCluster;
            MFTZoneClusterStart = mftZoneClusterStart;
            MFTZoneClusterEnd = mftZoneClusterEnd;
            MFTZoneSize = (mftZoneClusterEnd - mftZoneClusterStart) * bytesPerCluster;
            MFTMirrorStart = mftMirrorStart;
        }

        public static NTFSVolumeData Get(IntPtr hDrive)
        {

            // Create a byte array the size of the NTFS_VOLUME_DATA_BUFFER struct
            byte[] ntfsVolData = new byte[96];
            // Instatiate an integer to accept the amount of bytes read
            int buf = new int();

            // Return the NTFS_VOLUME_DATA_BUFFER struct
            var status = Win32.DeviceIoControl(
                hDevice: hDrive,
                dwIoControlCode: (uint)0x00090064,
                InBuffer: null,
                nInBufferSize: 0,
                OutBuffer: ntfsVolData,
                nOutBufferSize: ntfsVolData.Length,
                lpBytesReturned: ref buf,
                lpOverlapped: IntPtr.Zero);

            NTFS_VOLUME_DATA_BUFFER ntfsVD = new NTFS_VOLUME_DATA_BUFFER(ntfsVolData);

            // Return the NTFSVolumeData Object
            return new NTFSVolumeData(
                ntfsVD.NumberSectors, 
                ntfsVD.TotalClusters, 
                ntfsVD.FreeClusters, 
                ntfsVD.BytesPerSector, 
                ntfsVD.BytesPerCluster, 
                ntfsVD.BytesPerFileRecordSegment, 
                ntfsVD.ClustersPerFileRecordSegment, 
                ntfsVD.MftValidDataLength, 
                ntfsVD.MftStartLcn, 
                ntfsVD.MftZoneStart, 
                ntfsVD.MftZoneEnd, 
                ntfsVD.Mft2StartLcn);
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct NTFS_BPB
    {
        // jump instruction
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        internal byte[] Jmp;

        // signature
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
        internal byte[] Signature;

        // BPB and extended BPB
        internal ushort BytesPerSector;
        internal byte SectorsPerCluster;
        internal ushort ReservedSectors;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        internal byte[] Zeros1;
        internal ushort NotUsed1;
        internal byte MediaDescriptor;
        internal ushort Zeros2;
        internal ushort SectorsPerTrack;
        internal ushort NumberOfHeads;
        internal uint HiddenSectors;
        internal uint NotUsed2;
        internal uint NotUsed3;
        internal ulong TotalSectors;
        internal ulong LCN_MFT;
        internal ulong LCN_MFTMirr;
        internal uint ClustersPerFileRecord;
        internal uint ClustersPerIndexBlock;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
        internal byte[] VolumeSN;

        // boot code
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 430)]
        internal byte[] Code;

        //0xAA55
        internal byte _AA;
        internal byte _55;

        internal NTFS_BPB(byte[] bytes)
        {
            Jmp = bytes.Skip(0).Take(3).ToArray();
            Signature = bytes.Skip(3).Take(8).ToArray();
            BytesPerSector = BitConverter.ToUInt16(bytes, 11);
            SectorsPerCluster = bytes[13];
            ReservedSectors = BitConverter.ToUInt16(bytes, 14);
            Zeros1 = bytes.Skip(16).Take(3).ToArray();
            NotUsed1 = BitConverter.ToUInt16(bytes, 19);
            MediaDescriptor = bytes[21];
            Zeros2 = BitConverter.ToUInt16(bytes, 22);
            SectorsPerTrack = BitConverter.ToUInt16(bytes, 24);
            NumberOfHeads = BitConverter.ToUInt16(bytes, 26);
            HiddenSectors = BitConverter.ToUInt32(bytes, 28);
            NotUsed2 = BitConverter.ToUInt32(bytes, 32);
            NotUsed3 = BitConverter.ToUInt32(bytes, 36);
            TotalSectors = BitConverter.ToUInt64(bytes, 40);
            LCN_MFT = BitConverter.ToUInt64(bytes, 48);
            LCN_MFTMirr = BitConverter.ToUInt64(bytes, 56);
            ClustersPerFileRecord = BitConverter.ToUInt32(bytes, 64);
            ClustersPerIndexBlock = BitConverter.ToUInt32(bytes, 68);
            VolumeSN = bytes.Skip(72).Take(8).ToArray();
            Code = bytes.Skip(80).Take(430).ToArray();
            _AA = bytes[510];
            _55 = bytes[511];
        }
    }

    internal class IndexEntry
    {

        enum INDEX_ENTRY_FLAG
        {
            SUBNODE = 0x01,     // Index entry points to a sub-node
            LAST = 0x02         // Last index entry in the node, no Stream
        }

        internal struct INDEX_ENTRY
        {
            internal ulong FileReference;    // Low 6B: MFT record index, High 2B: MFT record sequence number
            internal ushort Size;            // Length of the index entry
            internal ushort StreamSize;      // Length of the stream
            internal byte Flags;             // Flags
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] Padding;         // Padding
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1)]
            internal byte[] Stream;          // Stream
            // VCN of the sub node in Index Allocation, Offset = Size - 8

            internal INDEX_ENTRY(byte[] bytes)
            {
                FileReference = BitConverter.ToUInt64(bytes, 0);
                Size = BitConverter.ToUInt16(bytes, 8);
                StreamSize = BitConverter.ToUInt16(bytes, 10);
                Flags = bytes[12];
                Padding = bytes.Skip(13).Take(3).ToArray();
                Stream = bytes.Skip(16).Take(StreamSize).ToArray();
            }
        }

        struct INDEX_BLOCK
        {
            // Index Block Header
            internal uint Magic;                // "INDX"
            internal ushort OffsetOfUS;           // Offset of Update Sequence
            internal ushort SizeOfUS;             // Size in words of Update Sequence Number & Array
            internal ulong LSN;                  // $LogFile Sequence Number
            internal ulong VCN;                  // VCN of this index block in the index allocation
            // Index Header
            internal uint EntryOffset;          // Offset of the index entries, relative to this address(0x18)
            internal uint TotalEntrySize;       // Total size of the index entries
            internal uint AllocEntrySize;       // Allocated size of index entries
            internal byte NotLeaf;                // 1 if not leaf node (has children)
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] Padding;              // Padding

            internal INDEX_BLOCK(byte[] bytes)
            {
                Magic = BitConverter.ToUInt32(bytes, 0);
                OffsetOfUS = BitConverter.ToUInt16(bytes, 4);
                SizeOfUS = BitConverter.ToUInt16(bytes, 6);
                LSN = BitConverter.ToUInt64(bytes, 8);
                VCN = BitConverter.ToUInt64(bytes, 16);
                EntryOffset = BitConverter.ToUInt32(bytes, 24);
                TotalEntrySize = BitConverter.ToUInt32(bytes, 28);
                AllocEntrySize = BitConverter.ToUInt32(bytes, 32);
                NotLeaf = bytes[36];
                Padding = bytes.Skip(37).Take(3).ToArray();
            }
        }

        internal ulong FileIndex;
        internal string Flags;
        internal string Name;

        internal IndexEntry(INDEX_ENTRY indxEntry, string flag, string name)
        {
            FileIndex = (indxEntry.FileReference & 0x0000FFFFFFFFFFFF);
            Flags = flag;
            Name = name;
        }

        internal static List<IndexEntry> Get(IntPtr hVolume, FileStream streamToRead, uint inode)
        {

            FileRecord fileRecord = FileRecord.Get(hVolume, streamToRead, inode);

            NonResident INDX = null;

            foreach (Attr attr in fileRecord.Attribute)
            {
                if (attr.Name == "INDEX_ALLOCATION")
                {
                    if (attr.NonResident)
                    {
                        INDX = (NonResident)attr;
                    }
                }
            }

            List<byte> nonResBytes = NonResident.GetContent(streamToRead, INDX);

            List<IndexEntry> indxEntryList = new List<IndexEntry>();

            for (int offset = 0; offset < nonResBytes.Count; offset += 4096)
            {

                byte[] indxBytes = nonResBytes.Skip(offset).Take(4096).ToArray();

                INDEX_BLOCK indxBlock = new INDEX_BLOCK(indxBytes.Take(40).ToArray());

                byte[] IndexAllocEntryBytes = indxBytes.Skip(64).ToArray();

                int offsetIndx = 0;
                int offsetIndxPrev = 1;

                while ((offsetIndx < IndexAllocEntryBytes.Length) && (offsetIndx != offsetIndxPrev))
                {

                    INDEX_ENTRY indxEntryStruct = new INDEX_ENTRY(IndexAllocEntryBytes.Skip(offsetIndx).ToArray());

                    offsetIndxPrev = offsetIndx;
                    offsetIndx += indxEntryStruct.Size;
                    if (indxEntryStruct.Stream.Length > 66)
                    {

                        FileName.ATTR_FILE_NAME fileNameStruct = new FileName.ATTR_FILE_NAME(indxEntryStruct.Stream);

                        #region indxFlags

                        StringBuilder indxFlags = new StringBuilder();
                        if (indxEntryStruct.Flags != 0)
                        {
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.SUBNODE) == (int)INDEX_ENTRY_FLAG.SUBNODE)
                            {
                                indxFlags.Append("Subnode, ");
                            }
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.LAST) == (int)INDEX_ENTRY_FLAG.LAST)
                            {
                                indxFlags.Append("Last Entry, ");
                            }
                            indxFlags.Length -= 2;
                        }

                        #endregion indxFlags

                        string Name = System.Text.Encoding.Unicode.GetString(fileNameStruct.Name);
                        IndexEntry indxEntry = new IndexEntry(indxEntryStruct, indxFlags.ToString(), Name);
                        indxEntryList.Add(indxEntry);

                    }

                }

            }

            return indxEntryList;
        }

    }

    public class IndexNumber
    {
        public static uint Get(IntPtr hVolume, FileStream streamToRead, string fileName)
        {

            IntPtr hFile = Win32.getHandle(fileName);

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

                uint dirIndex = IndexNumber.Get(hVolume, streamToRead, directory);

                List<IndexEntry> indxArray = IndexEntry.Get(hVolume, streamToRead, dirIndex);

                foreach (IndexEntry indxEntry in indxArray)
                {
                    if (indxEntry.Name == directoryArray[(directoryArray.Length - 1)])
                    {
                        return (uint)indxEntry.FileIndex;
                    }
                }

            }

            Win32.BY_HANDLE_FILE_INFORMATION fileInfo = new Win32.BY_HANDLE_FILE_INFORMATION();
            bool Success = Win32.GetFileInformationByHandle(
                hFile: hFile,
                lpFileInformation: out fileInfo);

            // Combine two 32 bit unsigned integers into one 64 bit unsigned integer
            ulong FileIndex = fileInfo.nFileIndexHigh;
            FileIndex = FileIndex << 32;
            FileIndex = FileIndex + fileInfo.nFileIndexLow;
            // Unmask relevent bytes for MFT Index Number
            ulong Index = FileIndex & 0x0000FFFFFFFFFFFF;

            return (uint)Index;
        }

    }

    public class FileRecord
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

        internal FileRecord(uint recordNumber, ushort sequenceNumber, ulong logFileSequenceNumber, ushort links, string flags, Attr[] attribute)
        {
            RecordNumber = recordNumber;
            SequenceNumber = sequenceNumber;
            LogFileSequenceNumber = logFileSequenceNumber;
            Links = links;
            Flags = flags;
            Attribute = attribute;
        }

        public static List<byte> getFile(IntPtr hVolume, FileStream streamToRead, string fileName)
        {

            uint inode = IndexNumber.Get(hVolume, streamToRead, fileName);

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            FileRecord MFTRecord = FileRecord.Get(hVolume, streamToRead, inode);

            if (!(MFTRecord.Flags.Contains("Directory")))
            {

                foreach (Attr attr in MFTRecord.Attribute)
                {

                    if (attr.Name == "DATA")
                    {

                        if (attr.NonResident == true)
                        {

                            NonResident nonResAttr = (NonResident)attr;

                            return NonResident.GetContent(streamToRead, nonResAttr);

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

        private static bool checkMFTRecord(uint magic)
        {
            return magic == 1162627398;
        }

        private static byte[] getMFTRecordBytes(IntPtr hVolume, FileStream streamToRead, uint inode)
        {

            // Gather Data about Volume
            NTFSVolumeData volData = NTFSVolumeData.Get(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MFTStartCluster);

            // Determine offset to specified MFT Record
            long offsetMFTRecord = (inode * volData.BytesPerMFTRecord) + mftOffset;

            // Read bytes belonging to specified MFT Record and store in byte array
            return Win32.readDrive(streamToRead, offsetMFTRecord, volData.BytesPerMFTRecord);

        }

        public static FileRecord Get(IntPtr hVolume, FileStream streamToRead, uint inode)
        {

            byte[] MFTRecordBytes = getMFTRecordBytes(hVolume, streamToRead, inode);

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            FILE_RECORD_HEADER RecordHeader = new FILE_RECORD_HEADER(MFTRecordBytes);

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
                    AttributeReturn attrReturn = AttributeReturn.Get(MFTRecordBytes, offsetToATTR);
                    offsetToATTR = attrReturn.StartByte;
                    AttributeList.Add(attrReturn.Attribute);
                }

                Attr[] AttributeArray = AttributeList.ToArray();

                // Return FileRecord object
                return new FileRecord(RecordHeader.RecordNo, RecordHeader.SeqNo, RecordHeader.LSN, RecordHeader.Hardlinks, flagAttr.ToString(), AttributeArray);

            }

            else
            {
                Console.WriteLine("Fail");
                return null;
            }
        }

    }


    #region Attributes

    enum ATTR_TYPE
    {
        STANDARD_INFORMATION = 0x10,
        ATTRIBUTE_LIST = 0x20,
        FILE_NAME = 0x30,
        OBJECT_ID = 0x40,
        SECURITY_DESCRIPTOR = 0x50,
        VOLUME_NAME = 0x60,
        VOLUME_INFORMATION = 0x70,
        DATA = 0x80,
        INDEX_ROOT = 0x90,
        INDEX_ALLOCATION = 0xA0,
        BITMAP = 0xB0,
        REPARSE_POINT = 0xC0,
        EA_INFORMATION = 0xD0,
        EA = 0xE0,
        LOGGED_UTILITY_STREAM = 0x100,

        ATTR_FLAG_COMPRESSED = 0x0001,
        ATTR_FLAG_ENCRYPTED = 0x4000,
        ATTR_FLAG_SPARSE = 0x8000
    }

    class AttrHeader
    {

        private const byte RESIDENT = 0x00;
        private const byte NONRESIDENT = 0x01;

        internal struct ATTR_HEADER_COMMON
        {
            internal uint ATTRType;			// Attribute Type
            internal uint TotalSize;		    // Length (including this header)
            internal bool NonResident;	    // 0 - resident, 1 - non resident
            internal byte NameLength;		// name length in words
            internal ushort NameOffset;		// offset to the name
            internal ushort Flags;			// Flags
            internal ushort Id;				// Attribute Id

            internal ATTR_HEADER_COMMON(byte[] bytes)
            {
                ATTRType = BitConverter.ToUInt32(bytes, 0);
                TotalSize = BitConverter.ToUInt32(bytes, 4);
                NonResident = (bytes[8] == NONRESIDENT);
                NameLength = bytes[9];
                NameOffset = BitConverter.ToUInt16(bytes, 10);
                Flags = BitConverter.ToUInt16(bytes, 12);
                Id = BitConverter.ToUInt16(bytes, 14);
            }
        }

        internal struct ATTR_HEADER_RESIDENT
        {
            internal ATTR_HEADER_COMMON commonHeader;	// Common data structure
            internal uint AttrSize;		                // Length of the attribute body
            internal ushort AttrOffset;		            // Offset to the Attribute
            internal byte IndexedFlag;	                // Indexed flag
            internal byte Padding;		                // Padding

            internal ATTR_HEADER_RESIDENT(byte[] bytes)
            {
                commonHeader = new ATTR_HEADER_COMMON(bytes.Take(16).ToArray());
                AttrSize = BitConverter.ToUInt32(bytes, 16);
                AttrOffset = BitConverter.ToUInt16(bytes, 20);
                IndexedFlag = bytes[22];
                Padding = bytes[23];
            }
        }
    }

    public abstract class Attr
    {

        public string Name;
        public string NameString;
        public bool NonResident;

    }

    public class StandardInformation : Attr
    {

        enum ATTR_STDINFO_PERMISSION
        {
            READONLY = 0x00000001,
            HIDDEN = 0x00000002,
            SYSTEM = 0x00000004,
            ARCHIVE = 0x00000020,
            DEVICE = 0x00000040,
            NORMAL = 0x00000080,
            TEMP = 0x00000100,
            SPARSE = 0x00000200,
            REPARSE = 0x00000400,
            COMPRESSED = 0x00000800,
            OFFLINE = 0x00001000,
            NCI = 0x00002000,
            ENCRYPTED = 0x00004000
        }

        struct ATTR_STANDARD_INFORMATION
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal DateTime CreateTime;	// File creation time
            internal DateTime AlterTime;	// File altered time
            internal DateTime MFTTime;	// MFT changed time
            internal DateTime ReadTime;	// File read time
            internal uint Permission;	// Dos file permission
            internal uint MaxVersionNo;	// Maximum number of file versions
            internal uint VersionNo;	// File version number
            internal uint ClassId;		// Class Id
            internal uint OwnerId;		// Owner Id
            internal uint SecurityId;	// Security Id
            internal ulong QuotaCharged;	// Quota charged
            internal ulong USN;			// USN Journel

            internal ATTR_STANDARD_INFORMATION(byte[] bytes, int length)
            {
                if (length == 120)
                {
                    header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                    CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 0));
                    AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 8));
                    MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 16));
                    ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 24));
                    Permission = BitConverter.ToUInt32(bytes, 32);
                    MaxVersionNo = BitConverter.ToUInt32(bytes, 36);
                    VersionNo = BitConverter.ToUInt32(bytes, 40);
                    ClassId = BitConverter.ToUInt32(bytes, 44);
                    OwnerId = BitConverter.ToUInt32(bytes, 48);
                    SecurityId = BitConverter.ToUInt32(bytes, 52);
                    QuotaCharged = BitConverter.ToUInt64(bytes, 56);
                    USN = BitConverter.ToUInt64(bytes, 64);
                }
                else
                {
                    header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                    CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 0));
                    AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 8));
                    MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 16));
                    ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 24));
                    Permission = BitConverter.ToUInt32(bytes, 32);
                    MaxVersionNo = 0;
                    VersionNo = 0;
                    ClassId = 0;
                    OwnerId = 0;
                    SecurityId = 0;
                    QuotaCharged = 0;
                    USN = 0;
                }
            }
        }

        public string Flags;
        public uint OwnerId;
        public uint SecurityId;
        public DateTime CreateTime;
        public DateTime FileModifiedTime;
        public DateTime MFTModifiedTime;
        public DateTime AccessTime;

        internal StandardInformation(uint ATTRType, string name, bool nonResident, string flags, uint ownerId, uint securityId, DateTime createTime, DateTime alterTime, DateTime mftTime, DateTime readTime)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            Flags = flags;
            OwnerId = ownerId;
            SecurityId = securityId;
            CreateTime = createTime;
            FileModifiedTime = alterTime;
            MFTModifiedTime = mftTime;
            AccessTime = readTime;
        }

        internal static StandardInformation Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_STANDARD_INFORMATION stdInfo = new ATTR_STANDARD_INFORMATION(AttrBytes, AttrBytes.Length);

            #region stdInfoFlags

            StringBuilder permissionFlags = new StringBuilder();
            if (stdInfo.Permission != 0)
            {
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.READONLY) == (uint)ATTR_STDINFO_PERMISSION.READONLY)
                {
                    permissionFlags.Append("READONLY, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.HIDDEN) == (uint)ATTR_STDINFO_PERMISSION.HIDDEN)
                {
                    permissionFlags.Append("HIDDEN, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.SYSTEM) == (uint)ATTR_STDINFO_PERMISSION.SYSTEM)
                {
                    permissionFlags.Append("SYSTEM, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.ARCHIVE) == (uint)ATTR_STDINFO_PERMISSION.ARCHIVE)
                {
                    permissionFlags.Append("ARCHIVE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.DEVICE) == (uint)ATTR_STDINFO_PERMISSION.DEVICE)
                {
                    permissionFlags.Append("DEVICE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.NORMAL) == (uint)ATTR_STDINFO_PERMISSION.NORMAL)
                {
                    permissionFlags.Append("NORMAL, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.TEMP) == (uint)ATTR_STDINFO_PERMISSION.TEMP)
                {
                    permissionFlags.Append("TEMP, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.SPARSE) == (uint)ATTR_STDINFO_PERMISSION.SPARSE)
                {
                    permissionFlags.Append("SPARSE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.REPARSE) == (uint)ATTR_STDINFO_PERMISSION.REPARSE)
                {
                    permissionFlags.Append("REPARSE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.COMPRESSED) == (uint)ATTR_STDINFO_PERMISSION.COMPRESSED)
                {
                    permissionFlags.Append("COMPRESSED, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.OFFLINE) == (uint)ATTR_STDINFO_PERMISSION.OFFLINE)
                {
                    permissionFlags.Append("OFFLINE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.NCI) == (uint)ATTR_STDINFO_PERMISSION.NCI)
                {
                    permissionFlags.Append("NCI, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.ENCRYPTED) == (uint)ATTR_STDINFO_PERMISSION.ENCRYPTED)
                {
                    permissionFlags.Append("ENCRYPTED, ");
                }
                if (permissionFlags.Length > 2)
                {
                    permissionFlags.Length -= 2;

                }
            }
            #endregion stdInfoFlags

            return new StandardInformation(
                stdInfo.header.commonHeader.ATTRType,
                AttrName,
                stdInfo.header.commonHeader.NonResident,
                permissionFlags.ToString(),
                stdInfo.OwnerId,
                stdInfo.SecurityId,
                stdInfo.CreateTime,
                stdInfo.AlterTime,
                stdInfo.MFTTime,
                stdInfo.ReadTime);

        }

    }

    public class FileName : Attr
    {
        enum ATTR_FILENAME_FLAG
        {
            READONLY = 0x00000001,
            HIDDEN = 0x00000002,
            SYSTEM = 0x00000004,
            ARCHIVE = 0x00000020,
            DEVICE = 0x00000040,
            NORMAL = 0x00000080,
            TEMP = 0x00000100,
            SPARSE = 0x00000200,
            REPARSE = 0x00000400,
            COMPRESSED = 0x00000800,
            OFFLINE = 0x00001000,
            NCI = 0x00002000,
            ENCRYPTED = 0x00004000,
            DIRECTORY = 0x10000000,
            INDEXVIEW = 0x20000000
        }

        private const byte ATTR_FILENAME_NAMESPACE_POSIX = 0x00;
        private const byte ATTR_FILENAME_NAMESPACE_WIN32 = 0x01;
        private const byte ATTR_FILENAME_NAMESPACE_DOS = 0x02;

        internal struct ATTR_FILE_NAME
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal ulong ParentRef;		    // File reference to the parent directory
            internal DateTime CreateTime;		// File creation time
            internal DateTime AlterTime;		// File altered time
            internal DateTime MFTTime;		    // MFT changed time
            internal DateTime ReadTime;		    // File read time
            internal ulong AllocSize;		    // Allocated size of the file
            internal ulong RealSize;		    // Real size of the file
            internal uint Flags;			    // Flags
            internal uint ER;				    // Used by EAs and Reparse
            internal byte NameLength;		    // Filename length in characters
            internal byte NameSpace;		    // Filename space
            internal byte[] Name;		        // Filename

            internal ATTR_FILE_NAME(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                ParentRef = BitConverter.ToUInt64(bytes, 24);
                CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 32));
                AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 40));
                MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 48));
                ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 56));
                AllocSize = BitConverter.ToUInt64(bytes, 64);
                RealSize = BitConverter.ToUInt64(bytes, 72);
                Flags = BitConverter.ToUInt32(bytes, 80);
                ER = BitConverter.ToUInt32(bytes, 84);
                NameLength = bytes[88];
                NameSpace = bytes[89];
                Name = bytes.Skip(90).ToArray();
            }
        }

        public string Filename;
        public ulong ParentIndex;
        public DateTime CreateTime;
        public DateTime FileModifiedTime;
        public DateTime MFTModifiedTime;
        public DateTime AccessTime;

        internal FileName(uint AttrType, string attrName, bool nonResident, byte[] name, ulong parentIndex, DateTime createTime, DateTime alterTime, DateTime mftTime, DateTime readTime)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), AttrType);
            NameString = attrName;
            NonResident = nonResident;
            Filename = Encoding.Unicode.GetString(name);
            ParentIndex = parentIndex;
            CreateTime = createTime;
            FileModifiedTime = alterTime;
            MFTModifiedTime = mftTime;
            AccessTime = readTime;
        }

        internal static FileName Get(byte[] AttrBytes, string attrName)
        {
        

            ATTR_FILE_NAME fileName = new ATTR_FILE_NAME(AttrBytes);

            return new FileName(
                fileName.header.commonHeader.ATTRType, 
                attrName, 
                fileName.header.commonHeader.NonResident,
                fileName.Name, 
                (fileName.ParentRef & 0x000000000000FFFF),
                fileName.CreateTime,
                fileName.AlterTime,
                fileName.MFTTime,
                fileName.ReadTime);
  
        }

    }

    public class Data : Attr
    {
        struct ATTR_DATA
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal byte[] RawBytes;
            
            internal ATTR_DATA(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                RawBytes = bytes.Skip(24).ToArray();
            }

        }

        public byte[] RawData;

        internal Data(uint ATTRType, string name, bool nonResident, byte[] bytes)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            RawData = bytes;
        }

        internal static Data Get(byte[] AttrBytes, string attrName)
        {

            ATTR_DATA data = new ATTR_DATA(AttrBytes);

            return new Data(
                data.header.commonHeader.ATTRType,
                attrName,
                data.header.commonHeader.NonResident,
                data.RawBytes);
        }

    }

    public class ObjectId : Attr
    {

        struct ATTR_OBJECT_ID
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal byte[] ObjectId;
            internal byte[] BirthVolumeId;
            internal byte[] BirthObjectId;
            internal byte[] BirthDomainId;

            internal ATTR_OBJECT_ID(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                ObjectId = bytes.Skip(24).Take(16).ToArray();
                BirthVolumeId = null;
                BirthObjectId = null;
                BirthDomainId = null;
            }

        }

        public byte[] ObjectIdBytes;

        internal ObjectId(uint ATTRType, string name, bool nonResident, byte[] objectId)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            ObjectIdBytes = objectId;
        }

        internal static ObjectId Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_OBJECT_ID objectId = new ATTR_OBJECT_ID(AttrBytes);

            return new ObjectId(
                objectId.header.commonHeader.ATTRType, 
                AttrName,
                objectId.header.commonHeader.NonResident,
                objectId.ObjectId);

        }

    }

    public class VolumeName : Attr
    {

        struct ATTR_VOLNAME
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal string VolumeNameString;

            internal ATTR_VOLNAME(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                VolumeNameString = Encoding.Unicode.GetString(bytes.Skip(24).ToArray());
            }

        }
        
        public string VolumeNameString;

        internal VolumeName(uint ATTRType, string name, bool nonResident, string volumeName)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            VolumeNameString = volumeName;
        }
      
        internal static VolumeName Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_VOLNAME volName = new ATTR_VOLNAME(AttrBytes);
            return new VolumeName(
                volName.header.commonHeader.ATTRType,
                AttrName, 
                volName.header.commonHeader.NonResident,
                volName.VolumeNameString);

        }

    }

    public class VolumeInformation : Attr
    {

        enum ATTR_VOLINFO
        {
            FLAG_DIRTY = 0x0001,	// Dirty
            FLAG_RLF = 0x0002,	    // Resize logfile
            FLAG_UOM = 0x0004,	    // Upgrade on mount
            FLAG_MONT = 0x0008,	    // Mounted on NT4
            FLAG_DUSN = 0x0010,	    // Delete USN underway
            FLAG_ROI = 0x0020,	    // Repair object Ids
            FLAG_MBC = 0x8000	    // Modified by chkdsk
        }

        struct ATTR_VOLUME_INFORMATION
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal byte[] Reserved1;	// Always 0 ?
            internal byte MajorVersion;	// Major version
            internal byte MinorVersion;	// Minor version
            public byte[] Flags;		// Flags

            internal ATTR_VOLUME_INFORMATION(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                Reserved1 = bytes.Skip(24).Take(8).ToArray();
                MajorVersion = bytes[32];
                MinorVersion = bytes[33];
                Flags = bytes.Skip(34).Take(2).ToArray();
            }
        }
        
        public uint Major;
        public uint Minor;
        public string Flags;

        internal VolumeInformation(uint ATTRType, string name, bool nonResident, byte majorVersion, byte minorVersion, string flags)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            Major = majorVersion;
            Minor = minorVersion;
            Flags = flags;
        }

        internal static VolumeInformation Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_VOLUME_INFORMATION volInfo = new ATTR_VOLUME_INFORMATION(AttrBytes);
            Int16 flags = BitConverter.ToInt16(volInfo.Flags, 0);

            #region volInfoFlags

            StringBuilder volumeFlags = new StringBuilder();
            if (flags != 0)
            {
                if ((flags & (int)ATTR_VOLINFO.FLAG_DIRTY) == (int)ATTR_VOLINFO.FLAG_DIRTY)
                {
                    volumeFlags.Append("Dirty, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_RLF) == (int)ATTR_VOLINFO.FLAG_RLF)
                {
                    volumeFlags.Append("Resize Logfile, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_UOM) == (int)ATTR_VOLINFO.FLAG_UOM)
                {
                    volumeFlags.Append("Upgrade on Mount, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_MONT) == (int)ATTR_VOLINFO.FLAG_MONT)
                {
                    volumeFlags.Append("Mounted on NT4, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_DUSN) == (int)ATTR_VOLINFO.FLAG_DUSN)
                {
                    volumeFlags.Append("Delete USN Underway, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_ROI) == (int)ATTR_VOLINFO.FLAG_ROI)
                {
                    volumeFlags.Append("Repair Object Ids, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_MBC) == (int)ATTR_VOLINFO.FLAG_MBC)
                {
                    volumeFlags.Append("Modified By ChkDisk, ");
                }
                volumeFlags.Length -= 2;
            }

            #endregion volInfoFlags

            return new VolumeInformation(
                volInfo.header.commonHeader.ATTRType, 
                AttrName, 
                volInfo.header.commonHeader.NonResident,
                volInfo.MajorVersion,
                volInfo.MinorVersion,
                volumeFlags.ToString());

        }

    }

    public class NonResident : Attr
    {

        struct ATTR_HEADER_NON_RESIDENT
        {
            internal AttrHeader.ATTR_HEADER_COMMON commonHeader;	// Common data structure
            internal ulong StartVCN;		                        // Starting VCN
            internal ulong LastVCN;		                            // Last VCN
            internal ushort DataRunOffset;	                        // Offset to the Data Runs
            internal ushort CompUnitSize;	                        // Compression unit size
            internal uint Padding;		                            // Padding
            internal ulong AllocSize;		                        // Allocated size of the attribute
            internal ulong RealSize;		                        // Real size of the attribute
            internal ulong IniSize;		                            // Initialized data size of the stream 

            internal ATTR_HEADER_NON_RESIDENT(byte[] bytes)
            {
                commonHeader = new AttrHeader.ATTR_HEADER_COMMON(bytes.Take(16).ToArray());
                StartVCN = BitConverter.ToUInt64(bytes, 16);
                LastVCN = BitConverter.ToUInt64(bytes, 24);
                DataRunOffset = BitConverter.ToUInt16(bytes, 32);
                CompUnitSize = BitConverter.ToUInt16(bytes, 34);
                Padding = BitConverter.ToUInt32(bytes, 36);
                AllocSize = BitConverter.ToUInt64(bytes, 40);
                RealSize = BitConverter.ToUInt64(bytes, 48);
                IniSize = BitConverter.ToUInt64(bytes, 56);
            }
        }

        public ulong AllocatedSize;
        public ulong RealSize;
        public ulong InitializedSize;
        public ulong[] StartCluster;
        public ulong[] EndCluster;

        internal NonResident(uint AttrType, string name, bool nonResident, ulong allocatedSize, ulong realSize, ulong iniSize, ulong[] startCluster, ulong[] endCluster)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), AttrType);
            NameString = name;
            NonResident = nonResident;
            AllocatedSize = allocatedSize;
            RealSize = realSize;
            InitializedSize = iniSize;
            StartCluster = startCluster;
            EndCluster = endCluster;
        }

        internal static List<byte> GetContent(FileStream streamToRead, NonResident nonResAttr)
        {

            List<byte> DataBytes = new List<byte>();

            for (int i = 0; i < nonResAttr.StartCluster.Length; i++)
            {
                long offset = (long)nonResAttr.StartCluster[i] * 4096;
                long length = ((long)nonResAttr.EndCluster[i] - (long)nonResAttr.StartCluster[i]) * 4096;
                DataBytes.AddRange(Win32.readDrive(streamToRead, offset, length));
            }

            DataBytes.Take((int)nonResAttr.RealSize);
            return DataBytes;

        }

        internal static NonResident Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_HEADER_NON_RESIDENT nonResAttrHeader = new ATTR_HEADER_NON_RESIDENT(AttrBytes);

            int offset = 0;
            int DataRunStart = nonResAttrHeader.DataRunOffset;
            int DataRunSize = (int)nonResAttrHeader.commonHeader.TotalSize - nonResAttrHeader.DataRunOffset;
            byte[] DataRunBytes = AttrBytes.Skip(DataRunStart).Take(DataRunSize).ToArray();

            int DataRunLengthByteCount = DataRunBytes[offset] & 0x0F;
            int DataRunOffsetByteCount = ((DataRunBytes[offset] & 0xF0) >> 4);

            int i = 0;
            ulong startCluster = 0;
            List<ulong> startClusterList = new List<ulong>();
            List<ulong> endClusterList = new List<ulong>();

            while ((offset < DataRunSize - 1) && (DataRunLengthByteCount != 0))
            {

                byte[] DataRunLengthBytes = DataRunBytes.Skip(offset + 1).Take(DataRunLengthByteCount).ToArray();
                Array.Resize(ref DataRunLengthBytes, 8);
                byte[] DataRunOffsetBytes = DataRunBytes.Skip((offset + 1 + DataRunLengthByteCount)).Take(DataRunOffsetByteCount).ToArray();
                Array.Resize(ref DataRunOffsetBytes, 8);

                ulong DataRunLength = BitConverter.ToUInt64(DataRunLengthBytes, 0);
                ulong DataRunOffset = BitConverter.ToUInt64(DataRunOffsetBytes, 0);

                startCluster += DataRunOffset;
                startClusterList.Add(startCluster);
                endClusterList.Add(startCluster + DataRunLength);

                offset = offset + 1 + DataRunLengthByteCount + DataRunOffsetByteCount;

                DataRunLengthByteCount = DataRunBytes[offset] & 0x0F;
                DataRunOffsetByteCount = ((DataRunBytes[offset] & 0xF0) >> 4);

                i++;

            }

            return new NonResident(
                nonResAttrHeader.commonHeader.ATTRType,
                AttrName,
                nonResAttrHeader.commonHeader.NonResident,
                nonResAttrHeader.AllocSize,
                nonResAttrHeader.RealSize,
                nonResAttrHeader.IniSize,
                startClusterList.ToArray(), 
                endClusterList.ToArray());

        }

    }

    #endregion Attributes

    class AttributeReturn
    {

        internal Attr Attribute;
        internal int StartByte;

        internal AttributeReturn(Attr attribute, int startBytes)
        {
            Attribute = attribute;
            StartByte = startBytes;
        }

        internal static AttributeReturn Get(byte[] Bytes, int offsetToATTR)
        {
            AttrHeader.ATTR_HEADER_COMMON commonAttributeHeader = new AttrHeader.ATTR_HEADER_COMMON(Bytes.Skip(offsetToATTR).Take(16).ToArray());

            byte[] AttrBytes = Bytes.Skip(offsetToATTR).Take((int)commonAttributeHeader.TotalSize).ToArray();
            byte[] NameBytes = AttrBytes.Skip(commonAttributeHeader.NameOffset).Take(commonAttributeHeader.NameLength * 2).ToArray();
            string AttrName = Encoding.Unicode.GetString(NameBytes);

            int offset = offsetToATTR + (int)commonAttributeHeader.TotalSize;
            AttributeReturn attrReturn = null;

            if (commonAttributeHeader.NonResident)
            {
                NonResident NonResAttr = NonResident.Get(AttrBytes, AttrName);
                attrReturn = new AttributeReturn(NonResAttr, offset);
            }
            else
            {
                AttrHeader.ATTR_HEADER_RESIDENT AttrHeaderResident = new AttrHeader.ATTR_HEADER_RESIDENT(AttrBytes.Take(24).ToArray());

                #region ATTRSwitch

                switch (commonAttributeHeader.ATTRType)
                {

                    case (Int32)ATTR_TYPE.STANDARD_INFORMATION:
                        StandardInformation StdInfoAttr = StandardInformation.Get(AttrBytes, AttrName);
                        attrReturn = new AttributeReturn(StdInfoAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.ATTRIBUTE_LIST:
                        break;

                    case (Int32)ATTR_TYPE.FILE_NAME:
                        FileName FileNameAttr = FileName.Get(AttrBytes, AttrName);
                        attrReturn = new AttributeReturn(FileNameAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.OBJECT_ID:
                        ObjectId ObjectIdAttr = ObjectId.Get(AttrBytes, AttrName);
                        attrReturn = new AttributeReturn(ObjectIdAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.SECURITY_DESCRIPTOR:
                        break;

                    case (Int32)ATTR_TYPE.VOLUME_NAME:
                        VolumeName VolumeNameAttr = VolumeName.Get(AttrBytes, AttrName);
                        attrReturn = new AttributeReturn(VolumeNameAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.VOLUME_INFORMATION:
                        VolumeInformation VolInfoAttr = VolumeInformation.Get(AttrBytes, AttrName);
                        attrReturn = new AttributeReturn(VolInfoAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.DATA:
                        Data DataAttr = Data.Get(AttrBytes, AttrName);
                        attrReturn = new AttributeReturn(DataAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.INDEX_ROOT:
                        //IndexRoot indxRootAttr = IndexRoot.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        //attrReturn = new AttributeReturn(indxRootAttr, offset);
                        break;

                    default:
                        break;

                }

                #endregion ATTRSwitch

            }

            if (attrReturn == null)
            {
                //Attr attr = new Attr(commonAttributeHeader, AttrName);
                //attrReturn = new AttributeReturn(attr, offset);
                attrReturn = new AttributeReturn(null, offset);
            }

            return attrReturn;
        }

    }


}
