using System;
using System.Collections.Generic;
using PowerForensics.FileSystems.HFSPlus.BTree;

namespace PowerForensics.FileSystems.HFSPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class CatalogFile
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum TEXT_ENCODING
        {
            /// <summary>
            /// 
            /// </summary>
            MacRoman = 0x00,

            /// <summary>
            /// 
            /// </summary>
            MacJapanese = 0x01,

            /// <summary>
            /// 
            /// </summary>
            MacChineseTrad = 0x02,

            /// <summary>
            /// 
            /// </summary>
            MacKorean = 0x03,

            /// <summary>
            /// 
            /// </summary>
            MacArabic = 0x04,

            /// <summary>
            /// 
            /// </summary>
            MacHebrew = 0x05,

            /// <summary>
            /// 
            /// </summary>
            MacGreek = 0x06,

            /// <summary>
            /// 
            /// </summary>
            MacCyrillic = 0x07,

            /// <summary>
            /// 
            /// </summary>
            MacDevanagari = 0x09,

            /// <summary>
            /// 
            /// </summary>
            MacGurmukhi = 0x0A,

            /// <summary>
            /// 
            /// </summary>
            MacGujarati = 0x0B,

            /// <summary>
            /// 
            /// </summary>
            MacOriya = 0x0C,

            /// <summary>
            /// 
            /// </summary>
            MacBengali = 0x0D,

            /// <summary>
            /// 
            /// </summary>
            MacTamil = 0x0E,

            /// <summary>
            /// 
            /// </summary>
            MacTelugu = 0x0F,

            /// <summary>
            /// 
            /// </summary>
            MacKannada = 0x10,

            /// <summary>
            /// 
            /// </summary>
            MacMalayalam = 0x11,

            /// <summary>
            /// 
            /// </summary>
            MacSinhales = 0x12,

            /// <summary>
            /// 
            /// </summary>
            MacBurmese = 0x13,

            /// <summary>
            /// 
            /// </summary>
            MacKhmer = 0x14,

            /// <summary>
            /// 
            /// </summary>
            MacThai = 0x15,

            /// <summary>
            /// 
            /// </summary>
            MacLaotian = 0x16,

            /// <summary>
            /// 
            /// </summary>
            MacGeorgian = 0x17,

            /// <summary>
            /// 
            /// </summary>
            MacArmenian = 0x18,

            /// <summary>
            /// 
            /// </summary>
            MacChineseSimp = 0x19,

            /// <summary>
            /// 
            /// </summary>
            MacTibetan = 0x1A,

            /// <summary>
            /// 
            /// </summary>
            MacMongolian = 0x1B,

            /// <summary>
            /// 
            /// </summary>
            MacEthiopic = 0x1C,

            /// <summary>
            /// 
            /// </summary>
            MacCentralEurRoman = 0x1D,

            /// <summary>
            /// 
            /// </summary>
            MacVietnamese = 0x1E,

            /// <summary>
            /// 
            /// </summary>
            MacExtArabic = 0x1F,

            /// <summary>
            /// 
            /// </summary>
            MacSymbol = 0x21,

            /// <summary>
            /// 
            /// </summary>
            MacDingbats = 0x22,

            /// <summary>
            /// 
            /// </summary>
            MacTurkish = 0x23,

            /// <summary>
            /// 
            /// </summary>
            MacCroatian = 0x24,

            /// <summary>
            /// 
            /// </summary>
            MacIcelandic = 0x25,

            /// <summary>
            /// 
            /// </summary>
            MacRomanian = 0x26,

            /// <summary>
            /// 
            /// </summary>
            MacUkrainian = 0x30,

            /// <summary>
            /// 
            /// </summary>
            MacFarsi = 0x31
        }

        #endregion Enums

        #region Static Methods

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

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class CatalogFolderRecord : DataRecord
    {
        #region Properties

        private readonly ushort Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Valence;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CatalogNodeId;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime CreateDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ContentModDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AttributeModDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BackupDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly BSDInfo Permissions;

        /// <summary>
        /// 
        /// </summary>
        public readonly FolderInfo UserInfo;

        /// <summary>
        /// 
        /// </summary>
        public readonly ExtendedFolderInfo FinderInfo;

        /// <summary>
        /// 
        /// </summary>
        public readonly CatalogFile.TEXT_ENCODING TextEncoding;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static CatalogFolderRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new CatalogFolderRecord(bytes, offset, volumeName, fileName);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class CatalogFileRecord : DataRecord
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FILE_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            kHFSFileLockedBit = 0x0000,

            /// <summary>
            /// 
            /// </summary>
            kHFSFileLockedMask = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            kHFSThreadExistsBit = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            kHFSThreadExistsMask = 0x0002
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly FILE_FLAGS Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CatalogNodeId;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime CreateDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ContentModDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AttributeModDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BackupDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly BSDInfo Permissions;

        /// <summary>
        /// 
        /// </summary>
        public readonly FileInfo UserInfo;

        /// <summary>
        /// 
        /// </summary>
        public readonly ExtendedFileInfo FinderInfo;

        /// <summary>
        /// 
        /// </summary>
        public readonly CatalogFile.TEXT_ENCODING TextEncoding;

        /// <summary>
        /// 
        /// </summary>
        public readonly ForkData DataFork;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static CatalogFileRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new CatalogFileRecord(bytes, offset, volumeName, fileName);
        }

        #endregion Static Methods

        #region Instance Methods

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

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class CatalogThread : DataRecord
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ParentId;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static CatalogThread Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new CatalogThread(bytes, offset, volumeName, fileName);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class BSDInfo
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum ADMIN_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            SF_ARCHIVED = 0,

            /// <summary>
            /// 
            /// </summary>
            SF_IMMUTABLE = 1,

            /// <summary>
            /// 
            /// </summary>
            SF_APPEND = 2
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum OWNER_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            UF_NODUMP = 0,

            /// <summary>
            /// 
            /// </summary>
            UF_IMMUTABLE = 1,

            /// <summary>
            /// 
            /// </summary>
            UF_APPEND = 2,

            /// <summary>
            /// 
            /// </summary>
            UF_OPAQUE = 3
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FILE_MODE
        {
            /// <summary>
            /// 
            /// </summary>
            S_ISTXT = 0x200,       // sticky bit 512

            /// <summary>
            /// 
            /// </summary>
            S_ISGID = 0x400,       // set group id on execution 1024

            /// <summary>
            /// 
            /// </summary>
            S_ISUID = 0x800,       // set user id on execution 2048

            /// <summary>
            /// 
            /// </summary>
            S_IXUSR = 0x40,        // X for owner 64

            /// <summary>
            /// 
            /// </summary>
            S_IWUSR = 0x80,        // W for owner 128

            /// <summary>
            /// 
            /// </summary>
            S_IRUSR = 0x100,       // R for owner 256

            /// <summary>
            /// 
            /// </summary>
            S_IRWXU = 0x1C0,       // RWX mask for owner 448
            
            /// <summary>
            /// 
            /// </summary>
            S_IXGRP = 0x008,       // X for group 8

            /// <summary>
            /// 
            /// </summary>
            S_IWGRP = 0x010,       // W for group 16

            /// <summary>
            /// 
            /// </summary>
            S_IRGRP = 0x020,       // R for group 32

            /// <summary>
            /// 
            /// </summary>
            S_IRWXG = 0x038,       // RWX mask for group 56

            /// <summary>
            /// 
            /// </summary>
            S_IXOTH = 0x001,       // X for other 1

            /// <summary>
            /// 
            /// </summary>
            S_IWOTH = 0x002,       // W for other 2

            /// <summary>
            /// 
            /// </summary>
            S_IROTH = 0x004,       // R for other 4

            /// <summary>
            /// 
            /// </summary>
            S_IRWXO = 0x007,       // RWX mask for other 7

            /// <summary>
            /// 
            /// </summary>
            S_IFIFO = 0x1000,      // named pipe (fifo) 4096

            /// <summary>
            /// 
            /// </summary>
            S_IFCHR = 0x2000,      // character special 8192

            /// <summary>
            /// 
            /// </summary>
            S_IFDIR = 0x4000,      // directory 16384

            /// <summary>
            /// 
            /// </summary>
            S_IFBLK = 0x6000,      // block special 24576

            /// <summary>
            /// 
            /// </summary>
            S_IFREG = 0x8000,      // regular 32768

            /// <summary>
            /// 
            /// </summary>
            S_IFLNK = 0xA0000,     // symbolic link 40960

            /// <summary>
            /// 
            /// </summary>
            S_IFSOCK = 0xC000,     // socket 49152

            /// <summary>
            /// 
            /// </summary>
            S_IFWHT = 0xE000,      // whiteout 57344

            /// <summary>
            /// 
            /// </summary>
            S_IFMT = 0xF000,       // type of file mask 61440
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly uint OwnerID;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint GroupID;

        /// <summary>
        /// 
        /// </summary>
        public readonly ADMIN_FLAGS AdminFlags;

        /// <summary>
        /// 
        /// </summary>
        public readonly OWNER_FLAGS OwnerFlags;

        /// <summary>
        /// 
        /// </summary>
        public readonly FILE_MODE FileMode;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static BSDInfo Get(byte[] bytes, int offset)
        {
            return new BSDInfo(bytes, offset);
        }

        #endregion Static Methods

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion Override Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class Point
    {
        #region Properties


        /// <summary>
        /// 
        /// </summary>
        public readonly short Vertical;

        /// <summary>
        /// 
        /// </summary>
        public readonly short Horizontal;

        #endregion Properties

        #region Constructors

        private Point(byte[] bytes, int offset)
        {
            Vertical = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset));
            Horizontal = Helper.SwapEndianness(BitConverter.ToInt16(bytes, offset + 0x02));
        }

        #endregion Constructors

        #region Static Methods

        internal static Point Get(byte[] bytes, int offset)
        {
            return new Point(bytes, offset);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class Rect
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly short Top;

        /// <summary>
        /// 
        /// </summary>
        public readonly short Left;

        /// <summary>
        /// 
        /// </summary>
        public readonly short Bottom;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static Rect Get(byte[] bytes, int offset)
        {
            return new Rect(bytes, offset);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileInfo
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FileType;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FileCreator;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort FinderFlags;

        /// <summary>
        /// 
        /// </summary>
        public readonly Point Location;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static FileInfo Get(byte[] bytes, int offset)
        {
            return new FileInfo(bytes, offset);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExtendedFileInfo
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ExtendedFinderFlags;

        /// <summary>
        /// 
        /// </summary>
        public readonly int PutAwayFolderID;

        #endregion Properties

        #region Constructors

        private ExtendedFileInfo(byte[] bytes, int offset)
        {
            ExtendedFinderFlags = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x08));
            PutAwayFolderID = Helper.SwapEndianness(BitConverter.ToInt32(bytes, offset + 0x0C));
        }

        #endregion Constructors

        #region Static Methods

        internal static ExtendedFileInfo Get(byte[] bytes, int offset)
        {
            return new ExtendedFileInfo(bytes, offset);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class FolderInfo
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly Rect WindowBounds;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort FinderFlags;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static FolderInfo Get(byte[] bytes, int offset)
        {
            return new FolderInfo(bytes, offset);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExtendedFolderInfo
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly Point ScrollPosition;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ExtendedFinderFlags;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        internal static ExtendedFolderInfo Get(byte[] bytes, int offset)
        {
            return new ExtendedFolderInfo(bytes, offset);
        }

        #endregion Static Methods
    }
}