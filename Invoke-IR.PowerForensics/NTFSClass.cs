using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;

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

            // Return the NTFSVolumeData Object
            return new NTFSVolumeData(new NTFS_VOLUME_DATA_BUFFER(ntfsVolData));
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

            BY_HANDLE_FILE_INFORMATION fileInfo = new BY_HANDLE_FILE_INFORMATION();
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
       
        public static List<byte> getFile(IntPtr hVolume, FileStream streamToRead, string fileName)
        {

            uint inode = IndexNumber.Get(hVolume, streamToRead, fileName);

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            FileRecord MFTRecord = FileRecord.Get(hVolume, streamToRead, inode);

            if (!(MFTRecord.Flags.Contains("Directory")))
            {

                foreach (Attribute attr in MFTRecord.Attribute)
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

        private static bool checkMFTRecord(FILE_RECORD_HEADER mftRecordHeader)
        {
            return mftRecordHeader.Magic == 1162627398;
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

        internal static FileRecord Get(IntPtr hVolume, FileStream streamToRead, uint inode)
        {

            byte[] MFTRecordBytes = getMFTRecordBytes(hVolume, streamToRead, inode);

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            FILE_RECORD_HEADER RecordHeader = new FILE_RECORD_HEADER(MFTRecordBytes);

            // Check MFT Signature (FILE) to ensure bytes actually represent an MFT Record
            if (checkMFTRecord(RecordHeader))
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

                List<Attribute> AttributeList = new List<Attribute>();
                int offsetToATTR = RecordHeader.OffsetOfAttr;

                while (offsetToATTR < (RecordHeader.RealSize - 8))
                {
                    AttributeReturn attrReturn = AttributeReturn.Get(MFTRecordBytes, offsetToATTR);
                    offsetToATTR = attrReturn.StartByte;
                    AttributeList.Add(attrReturn.Attribute);
                }

                Attribute[] AttributeArray = AttributeList.ToArray();

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

        internal static StandardInformation Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, ATTR_HEADER_RESIDENT AttrHeaderResident, string AttrName)
        {

            ATTR_STANDARD_INFORMATION stdInfo = new ATTR_STANDARD_INFORMATION(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray(), (int)AttrHeaderResident.AttrSize);
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
            return new StandardInformation(commonAttributeHeader, AttrName, permissionFlags.ToString(), stdInfo);
        
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

        internal static FileName Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, ATTR_HEADER_RESIDENT AttrHeaderResident, string AttrName)
        {
            
            ATTR_FILE_NAME fileName = new ATTR_FILE_NAME(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
            ulong ParentIndex = fileName.ParentRef & 0x000000000000FFFF;
            return new FileName(commonAttributeHeader, AttrName, fileName, ParentIndex);
  
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

        internal static Data Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, ATTR_HEADER_RESIDENT AttrHeaderResident, string AttrName)
        {
            
            return new Data(commonAttributeHeader, AttrName, AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());

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

        internal static ObjectId Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, ATTR_HEADER_RESIDENT AttrHeaderResident, string AttrName)
        {

            ATTR_OBJECT_ID objectId = new ATTR_OBJECT_ID(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
            return new ObjectId(commonAttributeHeader, AttrName, objectId);
            
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

        internal static VolumeName Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, ATTR_HEADER_RESIDENT AttrHeaderResident, string AttrName)
        {

            string vName = Encoding.Unicode.GetString(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
            return new VolumeName(commonAttributeHeader, AttrName, vName);

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

        internal static VolumeInformation Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, ATTR_HEADER_RESIDENT AttrHeaderResident, string AttrName)
        {

            ATTR_VOLUME_INFORMATION volInfo = new ATTR_VOLUME_INFORMATION(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
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

            return new VolumeInformation(commonAttributeHeader, AttrName, volInfo, volumeFlags.ToString());

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

        internal static IndexRoot Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, ATTR_HEADER_RESIDENT AttrHeaderResident, string AttrName)
        {
            ATTR_INDEX_ROOT indxRoot = new ATTR_INDEX_ROOT(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
            return new IndexRoot(commonAttributeHeader, AttrName, indxRoot, "test");
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

        internal static List<byte> GetContent(FileStream streamToRead, NonResident nonResAttr)
        {
            List<byte> DataBytes = new List<byte>();

            for (int i = 0; i < nonResAttr.StartCluster.Length; i++)
            {
                long offset = (long)nonResAttr.StartCluster[i] * 4096;
                long length = ((long)nonResAttr.EndCluster[i] - (long)nonResAttr.StartCluster[i]) * 4096;
                //long offset = (long)nonResAttr.StartCluster[i];
                //long length = ((long)nonResAttr.EndCluster[i] - (long)nonResAttr.StartCluster[i]);
                DataBytes.AddRange(Win32.readDrive(streamToRead, offset, length));
            }

            DataBytes.Take((int)nonResAttr.RealSize);
            return DataBytes;

        }

        internal static NonResident Get(byte[] AttrBytes, ATTR_HEADER_COMMON commonAttributeHeader, string AttrName)
        {

            ATTR_HEADER_NON_RESIDENT nonResAttrHeader = new ATTR_HEADER_NON_RESIDENT(commonAttributeHeader, AttrBytes.Skip(16).Take(48).ToArray());

            int offset = 0;
            int DataRunStart = nonResAttrHeader.DataRunOffset;
            int DataRunSize = (int)commonAttributeHeader.TotalSize - nonResAttrHeader.DataRunOffset;
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

            return new NonResident(commonAttributeHeader, AttrName, nonResAttrHeader, startClusterList.ToArray(), endClusterList.ToArray());

        }

    }

    #endregion Attributes

    class AttributeReturn
    {
        internal Attribute Attribute;
        internal int StartByte;

        internal AttributeReturn(Attribute attribute, int startBytes)
        {
            Attribute = attribute;
            StartByte = startBytes;
        }

        internal static AttributeReturn Get(byte[] Bytes, int offsetToATTR)
        {
            ATTR_HEADER_COMMON commonAttributeHeader = new ATTR_HEADER_COMMON(Bytes.Skip(offsetToATTR).Take(16).ToArray());

            byte[] AttrBytes = Bytes.Skip(offsetToATTR).Take((int)commonAttributeHeader.TotalSize).ToArray();
            byte[] NameBytes = AttrBytes.Skip(commonAttributeHeader.NameOffset).Take(commonAttributeHeader.NameLength * 2).ToArray();
            string AttrName = Encoding.Unicode.GetString(NameBytes);

            int offset = offsetToATTR + (int)commonAttributeHeader.TotalSize;
            AttributeReturn attrReturn = null;

            if (commonAttributeHeader.NonResident)
            {
                NonResident NonResAttr = InvokeIR.PowerForensics.NonResident.Get(AttrBytes, commonAttributeHeader, AttrName);
                attrReturn = new AttributeReturn(NonResAttr, offset);
            }
            else
            {
                ATTR_HEADER_RESIDENT AttrHeaderResident = new ATTR_HEADER_RESIDENT(commonAttributeHeader, AttrBytes.Skip(16).Take(8).ToArray());

                #region ATTRSwitch

                switch (commonAttributeHeader.ATTRType)
                {

                    case (Int32)ATTR_TYPE.STANDARD_INFORMATION:
                        StandardInformation StdInfoAttr = StandardInformation.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        attrReturn = new AttributeReturn(StdInfoAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.ATTRIBUTE_LIST:
                        break;

                    case (Int32)ATTR_TYPE.FILE_NAME:
                        FileName FileNameAttr = FileName.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        attrReturn = new AttributeReturn(FileNameAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.OBJECT_ID:
                        ObjectId ObjectIdAttr = ObjectId.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        attrReturn = new AttributeReturn(ObjectIdAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.SECURITY_DESCRIPTOR:
                        break;

                    case (Int32)ATTR_TYPE.VOLUME_NAME:
                        VolumeName VolumeNameAttr = VolumeName.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        attrReturn = new AttributeReturn(VolumeNameAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.VOLUME_INFORMATION:
                        VolumeInformation VolInfoAttr = VolumeInformation.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        attrReturn = new AttributeReturn(VolInfoAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.DATA:
                        Data DataAttr = Data.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        attrReturn = new AttributeReturn(DataAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.INDEX_ROOT:
                        IndexRoot indxRootAttr = IndexRoot.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                        attrReturn = new AttributeReturn(indxRootAttr, offset);
                        break;

                    default:
                        break;

                }

                #endregion ATTRSwitch

            }

            if (attrReturn == null)
            {
                Attribute attr = new Attribute(commonAttributeHeader, AttrName);
                attrReturn = new AttributeReturn(attr, offset);
            }

            return attrReturn;
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

        internal static List<IndexEntry> Get(IntPtr hVolume, FileStream streamToRead, uint inode)
        {

            FileRecord fileRecord = FileRecord.Get(hVolume, streamToRead, inode);

            NonResident INDX = null;

            foreach (Attribute attr in fileRecord.Attribute)
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

                        ATTR_FILE_NAME fileNameStruct = new ATTR_FILE_NAME(indxEntryStruct.Stream);

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

    #endregion classes

}
