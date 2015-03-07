using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace InvokeIR.PowerForensics
{

    #region structs

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BY_HANDLE_FILE_INFORMATION
    {
        internal uint dwFileAttributes;
        internal DateTime ftCreationTime;
        internal DateTime ftLastAccessTime;
        internal DateTime ftLastWriteTime;
        internal uint dwVolumeSerialNumber;
        internal uint nFileSizeHigh;
        internal uint nFileSizeLow;
        internal uint nNumberOfLinks;
        internal uint nFileIndexHigh;
        internal uint nFileIndexLow;
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct NTFS_VOLUME_DATA_BUFFER
    {
        internal ulong VolumeSerialNumber;
        internal Int64 NumberSectors;
        internal Int64 TotalClusters;
        internal Int64 FreeClusters;
        internal Int64 TotalReserved;
        internal Int32 BytesPerSector;
        internal Int32 BytesPerCluster;
        internal Int32 BytesPerFileRecordSegment;
        internal Int32 ClustersPerFileRecordSegment;
        internal Int64 MftValidDataLength;
        internal Int64 MftStartLcn;
        internal Int64 Mft2StartLcn;
        internal Int64 MftZoneStart;
        internal Int64 MftZoneEnd;

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

    enum FILE_RECORD_FLAG
    {
        INUSE = 0x01,	// File record is in use
        DIR = 0x02	    // File record is a directory
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
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

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_HEADER_COMMON
    {
        internal uint ATTRType;			// Attribute Type
        internal uint TotalSize;		// Length (including this header)
        internal bool NonResident;	    // 0 - resident, 1 - non resident
        internal byte NameLength;		    // name length in words
        internal ushort NameOffset;		// offset to the name
        internal ushort Flags;			// Flags
        internal ushort Id;				// Attribute Id

        internal ATTR_HEADER_COMMON(byte[] bytes)
        {
            ATTRType = BitConverter.ToUInt32(bytes, 0);
            TotalSize = BitConverter.ToUInt32(bytes, 4);
            NonResident = (bytes[8] == 1);
            NameLength = bytes[9];
            NameOffset = BitConverter.ToUInt16(bytes, 10);
            Flags = BitConverter.ToUInt16(bytes, 12);
            Id = BitConverter.ToUInt16(bytes, 14);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_HEADER_RESIDENT
    {
        ATTR_HEADER_COMMON Header;	// Common data structure
        internal uint AttrSize;		    // Length of the attribute body
        internal ushort AttrOffset;		// Offset to the Attribute
        internal byte IndexedFlag;	    // Indexed flag
        internal byte Padding;		    // Padding

        internal ATTR_HEADER_RESIDENT(ATTR_HEADER_COMMON header, byte[] bytes)
        {
            Header = header;
            AttrSize = BitConverter.ToUInt32(bytes, 0);
            AttrOffset = BitConverter.ToUInt16(bytes, 4);
            IndexedFlag = bytes[6];
            Padding = bytes[7];
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_HEADER_NON_RESIDENT
    {
        internal ATTR_HEADER_COMMON Header;	// Common data structure
        internal ulong StartVCN;		    // Starting VCN
        internal ulong LastVCN;		        // Last VCN
        internal ushort DataRunOffset;	    // Offset to the Data Runs
        internal ushort CompUnitSize;	    // Compression unit size
        internal uint Padding;		        // Padding
        internal ulong AllocSize;		    // Allocated size of the attribute
        internal ulong RealSize;		    // Real size of the attribute
        internal ulong IniSize;		        // Initialized data size of the stream 

        internal ATTR_HEADER_NON_RESIDENT(ATTR_HEADER_COMMON header, byte[] bytes)
        {
            Header = header;
            StartVCN = BitConverter.ToUInt64(bytes, 0);
            LastVCN = BitConverter.ToUInt64(bytes, 8);
            DataRunOffset = BitConverter.ToUInt16(bytes, 16);
            CompUnitSize = BitConverter.ToUInt16(bytes, 18);
            Padding = BitConverter.ToUInt32(bytes, 20);
            AllocSize = BitConverter.ToUInt64(bytes, 24);
            RealSize = BitConverter.ToUInt64(bytes, 32);
            IniSize = BitConverter.ToUInt64(bytes, 40);
        }
    }

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

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_STANDARD_INFORMATION
    {
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
            if (length == 96)
            {
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
        INDEXVIEW = 0x20000000,

        ATTR_FILENAME_NAMESPACE_POSIX = 0x00,
        ATTR_FILENAME_NAMESPACE_WIN32 = 0x01,
        ATTR_FILENAME_NAMESPACE_DOS = 0x02
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ATTR_FILE_NAME
    {
        public ulong ParentRef;		    // File reference to the parent directory
        public DateTime CreateTime;		// File creation time
        public DateTime AlterTime;		// File altered time
        public DateTime MFTTime;		    // MFT changed time
        public DateTime ReadTime;		    // File read time
        public ulong AllocSize;		    // Allocated size of the file
        public ulong RealSize;		    // Real size of the file
        public uint Flags;			    // Flags
        public uint ER;				    // Used by EAs and Reparse
        public byte NameLength;		    // Filename length in characters
        public byte NameSpace;		    // Filename space
        public byte[] Name;		        // Filename

        public ATTR_FILE_NAME(byte[] bytes)
        {
            ParentRef = BitConverter.ToUInt64(bytes, 0);
            CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 8));
            AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 16));
            MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 24));
            ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 32));
            AllocSize = BitConverter.ToUInt64(bytes, 40);
            RealSize = BitConverter.ToUInt64(bytes, 48);
            Flags = BitConverter.ToUInt32(bytes, 56);
            ER = BitConverter.ToUInt32(bytes, 60);
            NameLength = bytes[64];
            NameSpace = bytes[65];
            Name = bytes.Skip(66).Take(bytes.Length - 66).ToArray();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_ATTRIBUTE_LIST
    {
        internal uint AttrType;		// Attribute type
        internal ushort RecordSize;		// Record length
        internal byte NameLength;		// Name length in characters
        internal byte NameOffset;		// Name offset
        internal ulong StartVCN;		// Start VCN
        internal ulong BaseRef;		    // Base file reference to the attribute
        internal ushort AttrId;			// Attribute Id

        internal ATTR_ATTRIBUTE_LIST(byte[] bytes)
        {
            AttrType = BitConverter.ToUInt32(bytes, 0);
            RecordSize = BitConverter.ToUInt16(bytes, 4);
            NameLength = bytes[6];
            NameOffset = bytes[7];
            StartVCN = BitConverter.ToUInt64(bytes, 8);
            BaseRef = BitConverter.ToUInt64(bytes, 16);
            AttrId = BitConverter.ToUInt16(bytes, 24);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_OBJECT_ID
    {
        internal byte[] ObjectId;
        internal byte[] BirthVolumeId;
        internal byte[] BirthObjectId;
        internal byte[] BirthDomainId;

        internal ATTR_OBJECT_ID(byte[] bytes)
        {
            ObjectId = bytes.Take(16).ToArray();
            BirthVolumeId = null;
            BirthObjectId = null;
            BirthDomainId = null;
        }

    }

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

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_VOLUME_INFORMATION
    {
        internal byte[] Reserved1;	// Always 0 ?
        internal byte MajorVersion;	// Major version
        internal byte MinorVersion;	// Minor version
        public byte[] Flags;		// Flags

        internal ATTR_VOLUME_INFORMATION(byte[] bytes)
        {
            Reserved1 = bytes.Take(8).ToArray();
            MajorVersion = bytes[8];
            MinorVersion = bytes[9];
            Flags = bytes.Skip(10).Take(2).ToArray();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct ATTR_INDEX_ROOT
    {
        // Index Root Header
        internal uint AttrType;        // Attribute type (ATTR_TYPE_FILE_NAME: Directory, 0: Index View)
        internal uint CollRule;        // Collation rule
        internal uint IBSize;          // Size of index block
        internal byte ClustersPerIB;     // Clusters per index block (same as BPB?)
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        internal byte[] Padding1;        // Padding
        // Node Header
        internal uint EntryOffset;     // Offset to the first index entry, relative to this address(0x10)
        internal uint TotalEntrySize;  // Total size of the index entries
        internal uint AllocEntrySize;  // Allocated size of the index entries
        internal byte Flags;             // Flags
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        internal byte[] Padding2;        // Padding

        internal ATTR_INDEX_ROOT(byte[] bytes)
        {
            AttrType = BitConverter.ToUInt32(bytes, 0);
            CollRule = BitConverter.ToUInt32(bytes, 4);
            IBSize = BitConverter.ToUInt32(bytes, 8);
            ClustersPerIB = bytes[12];
            Padding1 = bytes.Skip(13).Take(3).ToArray();
            EntryOffset = BitConverter.ToUInt32(bytes, 16);
            TotalEntrySize = BitConverter.ToUInt32(bytes, 20);
            AllocEntrySize = BitConverter.ToUInt32(bytes, 24);
            Flags = bytes[28];
            Padding2 = bytes.Skip(29).Take(3).ToArray();
        }
    }

    enum INDEX_ENTRY_FLAG
    {
        SUBNODE = 0x01,     // Index entry points to a sub-node
        LAST = 0x02         // Last index entry in the node, no Stream
    }

    [StructLayout(LayoutKind.Sequential)]
    struct INDEX_ENTRY
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

    [StructLayout(LayoutKind.Sequential)]
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

    #endregion structs

}
