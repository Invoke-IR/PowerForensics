using System;
using System.Collections.Generic;
using PowerForensics.HFSPlus.BTree;

namespace PowerForensics.HFSPlus
{
    public class CatalogFile
    {
        #region Enums

        public enum TEXT_ENCODING
        {
            MacRoman = 0x00,
            MacJapanese = 0x01,
            MacChineseTrad = 0x02,
            MacKorean = 0x03,
            MacArabic = 0x04,
            MacHebrew = 0x05,
            MacGreek = 0x06,
            MacCyrillic = 0x07,
            MacDevanagari = 0x09,
            MacGurmukhi = 0x0A,
            MacGujarati = 0x0B,
            MacOriya = 0x0C,
            MacBengali = 0x0D,
            MacTamil = 0x0E,
            MacTelugu = 0x0F,
            MacKannada = 0x10,
            MacMalayalam = 0x11,
            MacSinhales = 0x12,
            MacBurmese = 0x13,
            MacKhmer = 0x14,
            MacThai = 0x15,
            MacLaotian = 0x16,
            MacGeorgian = 0x17,
            MacArmenian = 0x18,
            MacChineseSimp = 0x19,
            MacTibetan = 0x1A,
            MacMongolian = 0x1B,
            MacEthiopic = 0x1C,
            MacCentralEurRoman = 0x1D,
            MacVietnamese = 0x1E,
            MacExtArabic = 0x1F,
            MacSymbol = 0x21,
            MacDingbats = 0x22,
            MacTurkish = 0x23,
            MacCroatian = 0x24,
            MacIcelandic = 0x25,
            MacRomanian = 0x26,
            MacUkrainian = 0x30,
            MacFarsi = 0x31
        }

        #endregion Enums

        #region StaticMethods

        /// <summary>
        /// Returns the contents of the HFS+ Catalog File.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static byte[] GetContent(string volumeName)
        {
            return VolumeHeader.Get(volumeName).GetCatalogFileBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static Node GetHeaderNode(string volumeName)
        {
            return Node.GetHeaderNode(volumeName, "Catalog");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        public static Node GetNode(string volumeName, uint nodeNumber)
        {
            return Node.Get(volumeName, "Catalog", nodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        public static byte[] GetNodeBytes(string volumeName, uint nodeNumber)
        {
            return Node.GetBytes(volumeName, "Catalog", nodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static int GetRecords(string volumeName)
        {
            HeaderRecord headerRecord = GetHeaderNode(volumeName).Records[0] as HeaderRecord;
            Node leafNode = headerRecord.GetFirstLeafNode();

            List<Record> recordList = new List<Record>();
            int i = 0;

            while (leafNode.NodeDescriptor.fLink != 0)
            {
                /*foreach(DataRecord rec in leafNode.Records)
                {
                    switch(rec.RecordType)
                    {
                        case DataRecord.RECORD_TYPE.kHFSPlusFolderRecord:
                            recordList.Add(rec);
                            break;
                        case DataRecord.RECORD_TYPE.kHFSPlusFileRecord:
                            recordList.Add(rec);
                            break;
                        default:
                            break;
                    }
                }*/
                i++;
                leafNode = leafNode.NodeDescriptor.GetfLink();
            }

            return i;
        }

        #endregion StaticMethods
    }

    public class CatalogFolderRecord : DataRecord
    {
        #region Properties

        private readonly ushort Flags;
        public readonly uint Valence;
        public readonly uint CatalogNodeId;
        public readonly DateTime CreateDate;
        public readonly DateTime ContentModDate;
        public readonly DateTime AttributeModDate;
        public readonly DateTime AccessDate;
        public readonly DateTime BackupDate;
        public readonly BSDInfo Permissions;
        public readonly FolderInfo UserInfo;
        public readonly ExtendedFolderInfo FinderInfo;
        public readonly CatalogFile.TEXT_ENCODING TextEncoding;
        public readonly uint FolderCount;

        #endregion Properties

        #region Constructors

        private CatalogFolderRecord(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeName = volumeName;
            FileName = fileName;
            KeyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            ParentCatalogNodeId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x02));
            Name = GetHfsString(bytes, offset + 0x06);

            int dataOffset = offset + KeyLength + 0x02;

            RecordType = (RECORD_TYPE)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, dataOffset));
            Flags = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, dataOffset + 0x02));
            Valence = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x04));
            CatalogNodeId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x08));
            CreateDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x0C)));
            ContentModDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x10)));
            AttributeModDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x14)));
            AccessDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x18)));
            BackupDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x1C)));
            Permissions = BSDInfo.Get(bytes, dataOffset + 0x20);
            UserInfo = FolderInfo.Get(bytes, dataOffset + 0x30);
            FinderInfo = ExtendedFolderInfo.Get(bytes, dataOffset + 0x40);
            TextEncoding = (CatalogFile.TEXT_ENCODING)Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x50));
            FolderCount = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x54));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static CatalogFolderRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new CatalogFolderRecord(bytes, offset, volumeName, fileName);
        }

        #endregion StaticMethods
    }

    public class CatalogFileRecord : DataRecord
    {
        #region Enums

        [Flags]
        public enum FILE_FLAGS
        {
            kHFSFileLockedBit = 0x0000,
            kHFSFileLockedMask = 0x0001,
            kHFSThreadExistsBit = 0x0001,
            kHFSThreadExistsMask = 0x0002
        }

        #endregion Enums

        #region Properties

        public readonly FILE_FLAGS Flags;
        public readonly uint CatalogNodeId;
        public readonly DateTime CreateDate;
        public readonly DateTime ContentModDate;
        public readonly DateTime AttributeModDate;
        public readonly DateTime AccessDate;
        public readonly DateTime BackupDate;
        public readonly BSDInfo Permissions;
        public readonly FileInfo UserInfo;
        public readonly ExtendedFileInfo FinderInfo;
        public readonly CatalogFile.TEXT_ENCODING TextEncoding;
        public readonly ForkData DataFork;
        public readonly ForkData ResourceFork;

        #endregion Properties

        #region Constructors

        private CatalogFileRecord(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeName = volumeName;
            FileName = fileName;
            KeyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            ParentCatalogNodeId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x02));
            Name = GetHfsString(bytes, offset + 0x06);

            int dataOffset = offset + KeyLength + 0x02;

            RecordType = (RECORD_TYPE)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, dataOffset));
            Flags = (FILE_FLAGS)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, dataOffset + 0x02));
            CatalogNodeId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x08));
            CreateDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x0C)));
            ContentModDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x10)));
            AttributeModDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x14)));
            AccessDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x18)));
            BackupDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x1C)));
            Permissions = BSDInfo.Get(bytes, dataOffset + 0x20);
            UserInfo = FileInfo.Get(bytes, dataOffset + 0x30);
            FinderInfo = ExtendedFileInfo.Get(bytes, dataOffset + 0x40);
            TextEncoding = (CatalogFile.TEXT_ENCODING)Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x50));

            VolumeHeader volHeader = VolumeHeader.Get(volumeName);

            DataFork = ForkData.Get(bytes, dataOffset + 0x58, volumeName, volHeader.BlockSize);
            ResourceFork = ForkData.Get(bytes, dataOffset + 0xA8, volumeName, volHeader.BlockSize);
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static CatalogFileRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new CatalogFileRecord(bytes, offset, volumeName, fileName);
        }

        #endregion StaticMethods

        #region InstanceMethods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetDataForkContent()
        {
            return DataFork.GetContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetResourceForkContent()
        {
            return ResourceFork.GetContent();
        }

        #endregion InstanceMethods
    }

    public class CatalogThread : DataRecord
    {
        #region Properties

        public readonly uint ParentId;
        public readonly string NodeName;

        #endregion Properties

        #region Constructors

        private CatalogThread(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeName = volumeName;
            FileName = fileName;
            KeyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            ParentCatalogNodeId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x02));
            Name = GetHfsString(bytes, offset + 0x06);

            int dataOffset = offset + KeyLength + 0x02;

            RecordType = (RECORD_TYPE)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, dataOffset));
            ParentId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, dataOffset + 0x04));
            NodeName = GetHfsString(bytes, dataOffset + 0x08);
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static CatalogThread Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new CatalogThread(bytes, offset, volumeName, fileName);
        }

        #endregion StaticMethods
    }

    public class BSDInfo
    {
        #region Enums

        public enum ADMIN_FLAGS
        {
            SF_ARCHIVED = 0,
            SF_IMMUTABLE = 1,
            SF_APPEND = 2
        }

        [Flags]
        public enum OWNER_FLAGS
        {
            UF_NODUMP = 0,
            UF_IMMUTABLE = 1,
            UF_APPEND = 2,
            UF_OPAQUE = 3
        }

        [Flags]
        public enum FILE_MODE
        {
            S_ISTXT = 0x200,       // sticky bit 512
            S_ISGID = 0x400,       // set group id on execution 1024
            S_ISUID = 0x800,       // set user id on execution 2048

            S_IXUSR = 0x40,        // X for owner 64
            S_IWUSR = 0x80,        // W for owner 128
            S_IRUSR = 0x100,       // R for owner 256
            S_IRWXU = 0x1C0,       // RWX mask for owner 448

            S_IXGRP = 0x008,       // X for group 8
            S_IWGRP = 0x010,       // W for group 16
            S_IRGRP = 0x020,       // R for group 32
            S_IRWXG = 0x038,       // RWX mask for group 56

            S_IXOTH = 0x001,       // X for other 1
            S_IWOTH = 0x002,       // W for other 2
            S_IROTH = 0x004,       // R for other 4
            S_IRWXO = 0x007,       // RWX mask for other 7

            S_IFIFO = 0x1000,      // named pipe (fifo) 4096
            S_IFCHR = 0x2000,      // character special 8192
            S_IFDIR = 0x4000,      // directory 16384
            S_IFBLK = 0x6000,      // block special 24576
            S_IFREG = 0x8000,      // regular 32768
            S_IFLNK = 0xA0000,     // symbolic link 40960
            S_IFSOCK = 0xC000,     // socket 49152
            S_IFWHT = 0xE000,      // whiteout 57344
            S_IFMT = 0xF000,       // type of file mask 61440
        }

        #endregion Enums

        #region Properties

        public readonly uint OwnerID;
        public readonly uint GroupID;
        public readonly ADMIN_FLAGS AdminFlags;
        public readonly OWNER_FLAGS OwnerFlags;
        public readonly FILE_MODE FileMode;
        public readonly uint Special;

        #endregion Properties

        #region Constructors

        private BSDInfo(byte[] bytes, int offset)
        {
            OwnerID = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset));
            GroupID = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x04));
            AdminFlags = (ADMIN_FLAGS)bytes[offset + 0x08];
            OwnerFlags = (OWNER_FLAGS)bytes[offset + 0x09];
            FileMode = (FILE_MODE)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x0A));
            Special = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x0C));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static BSDInfo Get(byte[] bytes, int offset)
        {
            return new BSDInfo(bytes, offset);
        }

        #endregion StaticMethods

        #region OverrideMethods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion OverrideMethods
    }

    public class Point
    {
        #region Properties

        public readonly short Vertical;
        public readonly short Horizontal;

        #endregion Properties

        #region Constructors

        private Point(byte[] bytes, int offset)
        {
            Vertical = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset));
            Horizontal = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset + 0x02));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static Point Get(byte[] bytes, int offset)
        {
            return new Point(bytes, offset);
        }

        #endregion StaticMethods
    }

    public class Rect
    {
        #region Properties

        public readonly short Top;
        public readonly short Left;
        public readonly short Bottom;
        public readonly short Right;

        #endregion Properties

        #region Constructors

        private Rect(byte[] bytes, int offset)
        {
            Top = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset));
            Left = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset + 0x02));
            Bottom = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset + 0x04));
            Right = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset + 0x06));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static Rect Get(byte[] bytes, int offset)
        {
            return new Rect(bytes, offset);
        }

        #endregion StaticMethods
    }

    public class FileInfo
    {
        #region Properties

        public readonly uint FileType;
        public readonly uint FileCreator;
        public readonly ushort FinderFlags;
        public readonly Point Location;
        public readonly ushort ReservedField;

        #endregion Properties

        #region Constructors

        private FileInfo(byte[] bytes, int offset)
        {
            FileType = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset));
            FileCreator = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x04));
            FinderFlags = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x08));
            Location = Point.Get(bytes, offset + 0x0A);
            ReservedField = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x0E));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static FileInfo Get(byte[] bytes, int offset)
        {
            return new FileInfo(bytes, offset);
        }

        #endregion StaticMethods
    }

    public class ExtendedFileInfo
    {
        #region Properties

        public readonly ushort ExtendedFinderFlags;
        public readonly int PutAwayFolderID;

        #endregion Properties

        #region Constructors

        private ExtendedFileInfo(byte[] bytes, int offset)
        {
            ExtendedFinderFlags = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x08));
            PutAwayFolderID = Helper.SwapEndianness(BitConverter.ToInt32(bytes, offset + 0x0C));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static ExtendedFileInfo Get(byte[] bytes, int offset)
        {
            return new ExtendedFileInfo(bytes, offset);
        }

        #endregion StaticMethods
    }

    public class FolderInfo
    {
        #region Properties

        public readonly Rect WindowBounds;
        public readonly ushort FinderFlags;
        public readonly Point Location;

        #endregion Properties

        #region Constructors

        private FolderInfo(byte[] bytes, int offset)
        {
            WindowBounds = Rect.Get(bytes, offset);
            FinderFlags = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x08));
            Location = Point.Get(bytes, offset + 0x0A);
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static FolderInfo Get(byte[] bytes, int offset)
        {
            return new FolderInfo(bytes, offset);
        }

        #endregion StaticMethods
    }

    public class ExtendedFolderInfo
    {
        #region Properties

        public readonly Point ScrollPosition;
        public readonly ushort ExtendedFinderFlags;
        public readonly int PutAwayFolderID;

        #endregion Properties

        #region Constructors

        private ExtendedFolderInfo(byte[] bytes, int offset)
        {
            ScrollPosition = Point.Get(bytes, offset);
            ExtendedFinderFlags = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x08));
            PutAwayFolderID = Helper.SwapEndianness(BitConverter.ToInt32(bytes, offset + 0x0C));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static ExtendedFolderInfo Get(byte[] bytes, int offset)
        {
            return new ExtendedFolderInfo(bytes, offset);
        }

        #endregion StaticMethods
    }
}