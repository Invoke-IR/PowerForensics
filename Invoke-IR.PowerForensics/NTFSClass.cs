using System;
using System.Text;

namespace InvokeIR.PowerForensics
{

    #region classes

    public class NTFSVolumeData
    {
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

        internal NTFSVolumeData(NTFS_VOLUME_DATA_BUFFER buf)
        {
            VolumeSize_MB = (buf.TotalClusters * buf.BytesPerCluster) / 1024 / 1024;
            TotalSectors = buf.NumberSectors;
            TotalClusters = buf.TotalClusters;
            FreeClusters = buf.FreeClusters;
            FreeSpace_MB = ((buf.TotalClusters - buf.FreeClusters) * buf.BytesPerCluster) / 1024 / 1024;
            BytesPerSector = buf.BytesPerSector;
            BytesPerCluster = buf.BytesPerCluster;
            BytesPerMFTRecord = buf.BytesPerFileRecordSegment;
            ClustersPerMFTRecord = buf.ClustersPerFileRecordSegment;
            MFTSize_MB = (buf.MftValidDataLength) / 1024 / 1024;
            MFTStartCluster = buf.MftStartLcn;
            MFTZoneClusterStart = buf.MftZoneStart;
            MFTZoneClusterEnd = buf.MftZoneEnd;
            MFTZoneSize = (buf.MftZoneEnd - buf.MftZoneStart) * buf.BytesPerCluster;
            MFTMirrorStart = buf.Mft2StartLcn;
        }
    }

    public class FileRecord
    {
        public uint RecordNumber;
        public ushort SequenceNumber;
        public ulong LogFileSequenceNumber;
        public ushort Links;
        public string Flags;
        public Attribute[] Attribute;

        internal FileRecord(uint recordNumber, ushort sequenceNumber, ulong logFileSequenceNumber, ushort links, string flags, Attribute[] attribute)
        {
            RecordNumber = recordNumber;
            SequenceNumber = sequenceNumber;
            LogFileSequenceNumber = logFileSequenceNumber;
            Links = links;
            Flags = flags;
            Attribute = attribute;
        }
    }

    public class Attribute
    {
        public string Name;
        public uint AttributeId;
        public string NameString;
        public bool NonResident;

        internal Attribute()
        {

        }

        internal Attribute(ATTR_HEADER_COMMON commonHeader, string name)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
        }
    }

    public class StandardInformation : Attribute
    {
        public string Flags;
        public uint OwnerId;
        public uint SecurityId;
        public DateTime CreateTime;
        public DateTime FileModifiedTime;
        public DateTime MFTModifiedTime;
        public DateTime AccessTime;

        internal StandardInformation(ATTR_HEADER_COMMON commonHeader, string name, string flags, ATTR_STANDARD_INFORMATION stdInfo)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            Flags = flags;
            OwnerId = stdInfo.OwnerId;
            SecurityId = stdInfo.SecurityId;
            CreateTime = stdInfo.CreateTime;
            FileModifiedTime = stdInfo.AlterTime;
            MFTModifiedTime = stdInfo.MFTTime;
            AccessTime = stdInfo.ReadTime;
        }
    }

    public class FileName : Attribute
    {
        public string Filename;
        public ulong ParentIndex;
        public DateTime CreateTime;
        public DateTime FileModifiedTime;
        public DateTime MFTModifiedTime;
        public DateTime AccessTime;

        internal FileName(ATTR_HEADER_COMMON commonHeader, string name, ATTR_FILE_NAME fileName, ulong parentIndex)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            Filename = Encoding.Unicode.GetString(fileName.Name);
            ParentIndex = parentIndex;
            CreateTime = fileName.CreateTime;
            FileModifiedTime = fileName.AlterTime;
            MFTModifiedTime = fileName.MFTTime;
            AccessTime = fileName.ReadTime;
        }
    }

    public class Data : Attribute
    {
        public byte[] RawData;

        internal Data(ATTR_HEADER_COMMON commonHeader, string name, byte[] bytes)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            RawData = bytes;
        }
    }

    public class ObjectId : Attribute
    {
        public byte[] ObjectIdBytes;

        internal ObjectId(ATTR_HEADER_COMMON commonHeader, string name, ATTR_OBJECT_ID objectId)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            ObjectIdBytes = objectId.ObjectId;
        }
    }

    public class VolumeName : Attribute
    {
        public string VolumeNameString;

        internal VolumeName(ATTR_HEADER_COMMON commonHeader, string name, string volumeName)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            VolumeNameString = volumeName;
        }
    }

    public class VolumeInformation : Attribute
    {
        public int Major;
        public int Minor;
        public string Flags;

        internal VolumeInformation(ATTR_HEADER_COMMON commonHeader, string name, ATTR_VOLUME_INFORMATION volInfo, string flags)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            Major = (int)volInfo.MajorVersion;
            Minor = (int)volInfo.MinorVersion;
            Flags = flags;
        }
    }

    public class IndexRoot : Attribute
    {
        public uint SizeOfIndexBlock;
        public uint EntryOffset;
        public uint TotalEntrySize;
        public uint AllocEntrySize;
        public string Flags;

        internal IndexRoot(ATTR_HEADER_COMMON commonHeader, string name, ATTR_INDEX_ROOT indexRoot, string flags)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            SizeOfIndexBlock = indexRoot.IBSize;
            EntryOffset = indexRoot.EntryOffset;
            TotalEntrySize = indexRoot.TotalEntrySize;
            AllocEntrySize = indexRoot.AllocEntrySize;
            Flags = flags;
        }
    }

    public class IndexBlock : Attribute
    {

    }

    public class NonResident : Attribute
    {
        public ulong AllocatedSize;
        public ulong RealSize;
        public ulong InitializedSize;
        public ulong[] StartCluster;
        public ulong[] EndCluster;

        internal NonResident(ATTR_HEADER_COMMON commonHeader, string name, ATTR_HEADER_NON_RESIDENT nonResHeader, ulong[] startCluster, ulong[] endCluster)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), commonHeader.ATTRType);
            AttributeId = commonHeader.ATTRType;
            NameString = name;
            NonResident = commonHeader.NonResident;
            AllocatedSize = nonResHeader.AllocSize;
            RealSize = nonResHeader.RealSize;
            InitializedSize = nonResHeader.IniSize;
            StartCluster = startCluster;
            EndCluster = endCluster;
        }
    }

    internal class AttributeReturn
    {
        internal Attribute Attribute;
        internal int StartByte;

        internal AttributeReturn(Attribute attribute, int startBytes)
        {
            Attribute = attribute;
            StartByte = startBytes;
        }
    }

    public class IndexEntry
    {
        public ulong FileIndex;
        public string Flags;
        public string Name;

        internal IndexEntry(INDEX_ENTRY indxEntry, string flag, string name)
        {
            FileIndex = (indxEntry.FileReference & 0x0000FFFFFFFFFFFF);
            Flags = flag;
            Name = name;
        }
        
    }

    #endregion classes

}
