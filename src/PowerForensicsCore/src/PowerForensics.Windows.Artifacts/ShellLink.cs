using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.FileSystems;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.Windows.Artifacts
{
    /// <summary>
    /// 
    /// </summary>
    public class ShellLink
    {
        #region Enums
        
        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum LINK_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            HasLinkTargetIdList = 0x00000001,
            
            /// <summary>
            /// 
            /// </summary>
            HasLinkInfo = 0x00000002,

            /// <summary>
            /// 
            /// </summary>
            HasName = 0x00000004,

            /// <summary>
            /// 
            /// </summary>
            HasRelativePath = 0x00000008,

            /// <summary>
            /// 
            /// </summary>
            HasWorkingDir = 0x00000010,

            /// <summary>
            /// 
            /// </summary>
            HasArguments = 0x00000020,

            /// <summary>
            /// 
            /// </summary>
            HasIconLocation = 0x00000040,

            /// <summary>
            /// 
            /// </summary>
            IsUnicode = 0x00000080,

            /// <summary>
            /// 
            /// </summary>
            ForceNoLinkInfo = 0x00000100,

            /// <summary>
            /// 
            /// </summary>
            HasExpString = 0x00000200,

            /// <summary>
            /// 
            /// </summary>
            RunInSeparateProcess = 0x00000400,

            /// <summary>
            /// 
            /// </summary>
            Unused1 = 0x00000800,

            /// <summary>
            /// 
            /// </summary>
            HasDarwinId = 0x00001000,

            /// <summary>
            /// 
            /// </summary>
            RunAsUser = 0x00002000,

            /// <summary>
            /// 
            /// </summary>
            HasExpIcon = 0x00004000,

            /// <summary>
            /// 
            /// </summary>
            NoPidlAlias = 0x00008000,

            /// <summary>
            /// 
            /// </summary>
            Unused2 = 0x00010000,

            /// <summary>
            /// 
            /// </summary>
            RunWithShimLayer = 0x00020000,

            /// <summary>
            /// 
            /// </summary>
            ForceNoLinkTrack = 0x00040000,

            /// <summary>
            /// 
            /// </summary>
            EnableTargetMetadata = 0x00080000,

            /// <summary>
            /// 
            /// </summary>
            DisableLinkPathTracking = 0x00100000,

            /// <summary>
            /// 
            /// </summary>
            DisableKnownFolderTracking = 0x00200000,

            /// <summary>
            /// 
            /// </summary>
            DisableKnownFolderAlias = 0x00400000,

            /// <summary>
            /// 
            /// </summary>
            AllowLinkToLink = 0x00800000,

            /// <summary>
            /// 
            /// </summary>
            UnaliasOnSave = 0x01000000,

            /// <summary>
            /// 
            /// </summary>
            PreferEnvironmentPath = 0x02000000,

            /// <summary>
            /// 
            /// </summary>
            KeepLocalIdListForUncTarget = 0x04000000
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FILEATTRIBUTE_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_READONLY = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_HIDDEN = 0x0002,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_SYSTEM = 0x0004,

            /// <summary>
            /// 
            /// </summary>
            Reserved1 = 0x0008,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_DIRECTORY = 0x0010,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_ARCHIVE = 0x0020,

            /// <summary>
            /// 
            /// </summary>
            Reserved2 = 0x0040,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_NORMAL = 0x0080,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_TEMPORARY = 0x0100,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_SPARSE_FILE = 0x0200,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_REPARSE_POINT = 0x0400,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_COMPRESSED = 0x0800,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_OFFLINE = 0x1000,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x2000,

            /// <summary>
            /// 
            /// </summary>
            FILE_ATTRIBUTE_ENCRYPTED = 0x4000
        }

        /// <summary>
        /// 
        /// </summary>
        public enum SHOWCOMMAND
        {
            /// <summary>
            /// 
            /// </summary>
            SW_SHOWNORMAL = 0x00000001,

            /// <summary>
            /// 
            /// </summary>
            SW_SHOWMAXIMIZED = 0x00000003,

            /// <summary>
            /// 
            /// </summary>
            SW_SHOWMINNOACTIVE = 0x00000007
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum HOTKEY_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            HOTKEYF_SHIFT = 0x01,

            /// <summary>
            /// 
            /// </summary>
            HOTKEYF_CONTROL = 0x02,

            /// <summary>
            /// 
            /// </summary>
            HOTKEYF_ALT = 0x04,

            /// <summary>
            /// 
            /// </summary>
            K_0 = 0x30,

            /// <summary>
            /// 
            /// </summary>
            K_1 = 0x31,

            /// <summary>
            /// 
            /// </summary>
            K_2 = 0x32,

            /// <summary>
            /// 
            /// </summary>
            K_3 = 0x33,

            /// <summary>
            /// 
            /// </summary>
            K_4 = 0x34,

            /// <summary>
            /// 
            /// </summary>
            K_5 = 0x35,

            /// <summary>
            /// 
            /// </summary>
            K_6 = 0x36,

            /// <summary>
            /// 
            /// </summary>
            K_7 = 0x37,

            /// <summary>
            /// 
            /// </summary>
            K_8 = 0x38,

            /// <summary>
            /// 
            /// </summary>
            K_9 = 0x39,

            /// <summary>
            /// 
            /// </summary>
            K_A = 0x41,

            /// <summary>
            /// 
            /// </summary>
            K_B = 0x42,

            /// <summary>
            /// 
            /// </summary>
            K_C = 0x43,

            /// <summary>
            /// 
            /// </summary>
            K_D = 0x44,

            /// <summary>
            /// 
            /// </summary>
            K_E = 0x45,

            /// <summary>
            /// 
            /// </summary>
            K_F = 0x46,

            /// <summary>
            /// 
            /// </summary>
            K_G = 0x47,

            /// <summary>
            /// 
            /// </summary>
            K_H = 0x48,

            /// <summary>
            /// 
            /// </summary>
            K_I = 0x49,

            /// <summary>
            /// 
            /// </summary>
            K_J = 0x4A,

            /// <summary>
            /// 
            /// </summary>
            K_K = 0x4B,

            /// <summary>
            /// 
            /// </summary>
            K_L = 0x4C,

            /// <summary>
            /// 
            /// </summary>
            K_M = 0x4D,

            /// <summary>
            /// 
            /// </summary>
            K_N = 0x4E,

            /// <summary>
            /// 
            /// </summary>
            K_O = 0x4F,

            /// <summary>
            /// 
            /// </summary>
            K_P = 0x50,

            /// <summary>
            /// 
            /// </summary>
            K_Q = 0x51,

            /// <summary>
            /// 
            /// </summary>
            K_R = 0x52,

            /// <summary>
            /// 
            /// </summary>
            K_S = 0x53,

            /// <summary>
            /// 
            /// </summary>
            K_T = 0x54,

            /// <summary>
            /// 
            /// </summary>
            K_U = 0x55,

            /// <summary>
            /// 
            /// </summary>
            K_V = 0x56,

            /// <summary>
            /// 
            /// </summary>
            K_W = 0x57,

            /// <summary>
            /// 
            /// </summary>
            K_X = 0x58,

            /// <summary>
            /// 
            /// </summary>
            K_Y = 0x59,

            /// <summary>
            /// 
            /// </summary>
            K_Z = 0x5A,

            /// <summary>
            /// 
            /// </summary>
            VK_F1 = 0x70,

            /// <summary>
            /// 
            /// </summary>
            VK_F2 = 0x71,

            /// <summary>
            /// 
            /// </summary>
            VK_F3 = 0x72,

            /// <summary>
            /// 
            /// </summary>
            VK_F4 = 0x73,

            /// <summary>
            /// 
            /// </summary>
            VK_F5 = 0x74,

            /// <summary>
            /// 
            /// </summary>
            VK_F6 = 0x75,

            /// <summary>
            /// 
            /// </summary>
            VK_F7 = 0x76,

            /// <summary>
            /// 
            /// </summary>
            VK_F8 = 0x77,

            /// <summary>
            /// 
            /// </summary>
            VK_F9 = 0x78,

            /// <summary>
            /// 
            /// </summary>
            VK_F10 = 0x79,

            /// <summary>
            /// 
            /// </summary>
            VK_F11 = 0x7A,

            /// <summary>
            /// 
            /// </summary>
            VK_F12 = 0x7B,

            /// <summary>
            /// 
            /// </summary>
            VK_F13 = 0x7C,

            /// <summary>
            /// 
            /// </summary>
            VK_F14 = 0x7D,

            /// <summary>
            /// 
            /// </summary>
            VK_F15 = 0x7E,

            /// <summary>
            /// 
            /// </summary>
            VK_F16 = 0x7F,

            /// <summary>
            /// 
            /// </summary>
            VK_F17 = 0x80,

            /// <summary>
            /// 
            /// </summary>
            VK_F18 = 0x81,

            /// <summary>
            /// 
            /// </summary>
            VK_F19 = 0x82,

            /// <summary>
            /// 
            /// </summary>
            VK_F20 = 0x83,

            /// <summary>
            /// 
            /// </summary>
            VK_F21 = 0x84,

            /// <summary>
            /// 
            /// </summary>
            VK_F22 = 0x85,

            /// <summary>
            /// 
            /// </summary>
            VK_F23 = 0x86,

            /// <summary>
            /// 
            /// </summary>
            VK_F24 = 0x87,

            /// <summary>
            /// 
            /// </summary>
            VK_NUMLOCK = 0x90,

            /// <summary>
            /// 
            /// </summary>
            VK_SCROLL = 0x91
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum LINKINFO_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            VolumeIDAndLocalBasePath = 0x01,

            /// <summary>
            /// 
            /// </summary>
            CommonNetworkRelativeLinkAndPathSuffix = 0x02
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Path;

        // SHELL_LINK_HEADER
        private readonly int HeaderSize;
        
        private readonly Guid LinkCLSID;

        /// <summary>
        /// 
        /// </summary>
        public readonly LINK_FLAGS LinkFlags;

        /// <summary>
        /// 
        /// </summary>
        public readonly FILEATTRIBUTE_FLAGS FileAttributes;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime CreationTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime WriteTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FileSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly int IconIndex;

        /// <summary>
        /// 
        /// </summary>
        public readonly SHOWCOMMAND ShowCommand;

        /// <summary>
        /// 
        /// </summary>
        public readonly HOTKEY_FLAGS[] HotKey;

        // LINKTARGET_IDLIST
        private readonly ushort IdListSize;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly IdList IdList;

        // LINKINFO
        private readonly int LinkInfoSize;
        private readonly uint LinkInfoHeaderSize;
        private readonly LINKINFO_FLAGS LinkInfoFlags;
        private readonly int VolumeIdOffset;
        private readonly int LocalBasePathOffset;
        private readonly int CommonNetworkRelativeLinkOffset;
        private readonly int CommonPathSuffixOffset;
        private readonly int LocalBasePathOffsetUnicode;
        private readonly int CommonPathSuffixOffsetUnicode;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly VolumeId VolumeId;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly string LocalBasePath;

        /// <summary>
        /// 
        /// </summary>
        public readonly CommonNetworkRelativeLink CommonNetworkRelativeLink;

        /// <summary>
        /// 
        /// </summary>
        public readonly string CommonPathSuffix;

        /// <summary>
        /// 
        /// </summary>
        public readonly string LocalBasePathUnicode;

        /// <summary>
        /// 
        /// </summary>
        public readonly string CommonPathSuffixUnicode;

        // STRING_DATA
        private readonly ushort NameSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;
        private readonly ushort RelativePathSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string RelativePath;
        private readonly ushort WorkingDirectorySize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string WorkingDirectory;
        private readonly ushort CommandLineArgumentsSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string CommandLineArguments;
        private readonly ushort IconLocationSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string IconLocation;

        // EXTRA_DATA
        /// <summary>
        /// 
        /// </summary>
        public readonly ExtraData[] ExtraData;

        #endregion Properties

        #region Constructors

        private ShellLink(byte[] bytes, FileRecord record)
        {
            Path = record.FullName;

            HeaderSize = BitConverter.ToInt32(bytes, 0x00);
            
            if (HeaderSize == 0x4C)
            {
                #region SHELL_LINK_HEADER

                LinkCLSID = new Guid(Helper.GetSubArray(bytes, 0x04, 0x10));
                LinkFlags = (LINK_FLAGS)BitConverter.ToUInt32(bytes, 0x14);
                FileAttributes = (FILEATTRIBUTE_FLAGS)BitConverter.ToUInt32(bytes, 0x18);
                CreationTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x1C));
                AccessTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x24));
                WriteTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x2C));
                FileSize = BitConverter.ToUInt32(bytes, 0x34);
                IconIndex = BitConverter.ToInt32(bytes, 0x38);
                ShowCommand = (SHOWCOMMAND)BitConverter.ToUInt32(bytes, 0x3C);
                #region HotKey
                HotKey = new HOTKEY_FLAGS[2];
                HotKey[0] = (HOTKEY_FLAGS)bytes[0x40];
                HotKey[1] = (HOTKEY_FLAGS)bytes[0x41];
                #endregion HotKey

                int offset = 0x4C;

                #endregion SHELL_LINK_HEADER

                // I want to remove one layer of objects
                #region LINKTARGET_IDLIST

                if ((LinkFlags & LINK_FLAGS.HasLinkTargetIdList) == LINK_FLAGS.HasLinkTargetIdList)
                {
                    IdListSize = BitConverter.ToUInt16(bytes, offset);
                    IdList = new IdList(bytes, offset + 0x02, IdListSize);

                    offset += IdListSize + 0x02;
                }

                #endregion LINKTARGET_IDLIST

                #region LINKINFO
                
                if ((LinkFlags & LINK_FLAGS.HasLinkInfo) == LINK_FLAGS.HasLinkInfo)
                {
                    LinkInfoSize = BitConverter.ToInt32(bytes, offset);
                    LinkInfoHeaderSize = BitConverter.ToUInt32(bytes, offset + 0x04);
                    LinkInfoFlags = (LINKINFO_FLAGS)BitConverter.ToUInt32(bytes, offset + 0x08);
                    
                    if ((LinkInfoFlags & LINKINFO_FLAGS.VolumeIDAndLocalBasePath) == LINKINFO_FLAGS.VolumeIDAndLocalBasePath)
                    {
                        VolumeIdOffset = BitConverter.ToInt32(bytes, offset + 0x0C);
                        VolumeId = new VolumeId(bytes, offset + VolumeIdOffset);

                        LocalBasePathOffset = BitConverter.ToInt32(bytes, offset + 0x10);
                        LocalBasePath = Encoding.ASCII.GetString(bytes, offset + LocalBasePathOffset, LinkInfoSize - LocalBasePathOffset).Split('\0')[0];
                    }

                    if ((LinkInfoFlags & LINKINFO_FLAGS.CommonNetworkRelativeLinkAndPathSuffix) == LINKINFO_FLAGS.CommonNetworkRelativeLinkAndPathSuffix)
                    {
                        CommonNetworkRelativeLinkOffset = BitConverter.ToInt32(bytes, offset + 0x14);    
                        CommonNetworkRelativeLink = new CommonNetworkRelativeLink(bytes, offset + CommonNetworkRelativeLinkOffset);

                        CommonPathSuffixOffset = BitConverter.ToInt32(bytes, offset + 0x18);
                        CommonPathSuffix = Encoding.ASCII.GetString(bytes, offset + CommonPathSuffixOffset, LinkInfoSize - CommonPathSuffixOffset).Split('\0')[0];
                    }

                    if (LinkInfoHeaderSize >= 0x24)
                    {
                        LocalBasePathOffsetUnicode = BitConverter.ToInt32(bytes, offset + 0x1C);
                        LocalBasePathUnicode = Encoding.Unicode.GetString(bytes, offset + LocalBasePathOffsetUnicode, LinkInfoSize - LocalBasePathOffsetUnicode).Split('\0')[0];

                        CommonPathSuffixOffsetUnicode = BitConverter.ToInt32(bytes, offset + 0x20);
                        CommonPathSuffixUnicode = Encoding.Unicode.GetString(bytes, offset + CommonPathSuffixOffsetUnicode, LinkInfoSize - CommonPathSuffixOffsetUnicode).Split('\0')[0];
                    }

                    offset += LinkInfoSize;
                }

                #endregion LINKINFO

                #region STRING_DATA

                if ((LinkFlags & LINK_FLAGS.HasName) == LINK_FLAGS.HasName)
                {
                    NameSize = BitConverter.ToUInt16(bytes, offset);
                    Name = Encoding.Unicode.GetString(bytes, offset + 0x02, NameSize * 2);

                    offset += 2 + (NameSize * 2);
                }
                if ((LinkFlags & LINK_FLAGS.HasRelativePath) == LINK_FLAGS.HasRelativePath)
                {
                    RelativePathSize = BitConverter.ToUInt16(bytes, offset);
                    RelativePath = Encoding.Unicode.GetString(bytes, offset + 0x02, RelativePathSize * 2);                    

                    offset += 2 + (RelativePathSize * 2);
                }
                if ((LinkFlags & LINK_FLAGS.HasWorkingDir) == LINK_FLAGS.HasWorkingDir)
                {
                    WorkingDirectorySize = BitConverter.ToUInt16(bytes, offset);
                    WorkingDirectory = Encoding.Unicode.GetString(bytes, offset + 0x02, WorkingDirectorySize * 2);

                    offset += 2 + (WorkingDirectorySize * 2);
                }
                if ((LinkFlags & LINK_FLAGS.HasArguments) == LINK_FLAGS.HasArguments)
                {
                    CommandLineArgumentsSize = BitConverter.ToUInt16(bytes, offset);
                    CommandLineArguments = Encoding.Unicode.GetString(bytes, offset + 0x02, CommandLineArgumentsSize * 2);

                    offset += 2 + (CommandLineArgumentsSize * 2);
                }
                if ((LinkFlags & LINK_FLAGS.HasIconLocation) == LINK_FLAGS.HasIconLocation)
                { 
                    IconLocationSize = BitConverter.ToUInt16(bytes, offset);
                    IconLocation = Encoding.Unicode.GetString(bytes, offset + 0x02, IconLocationSize * 2);

                    offset += 2 + (IconLocationSize * 2);
                }
                
                #endregion STRING_DATA

                #region EXTRA_DATA

                List<ExtraData> edList = new List<ExtraData>();

                int datalength = 0;

                do
                {
                    datalength = BitConverter.ToInt32(bytes, offset);

                    switch (BitConverter.ToUInt32(bytes, offset + 0x04))
                    {
                        case 0xA0000001:
                            edList.Add(new EnvironmentVariableDataBlock(bytes, offset));
                            break;
                        case 0xA0000002:
                            edList.Add(new ConsoleDataBlock(bytes, offset));
                            break;
                        case 0xA0000003:
                            edList.Add(new TrackerDataBlock(bytes, offset));
                            break;
                        case 0xA0000004:
                            edList.Add(new ConsoleFeDataBlock(bytes, offset));
                            break;
                        case 0xA0000005:
                            edList.Add(new SpecialFolderDataBlock(bytes, offset));
                            break;
                        case 0xA0000006:
                            edList.Add(new DarwinDataBlock(bytes, offset));
                            break;
                        case 0xA0000007:
                            edList.Add(new IconEnvironmentDataBlock(bytes, offset));
                            break;
                        case 0xA0000008:
                            edList.Add(new ShimDataBlock(bytes, offset));
                            break;
                        case 0xA0000009:
                            edList.Add(new PropertyStoreDataBlock(bytes, offset));
                            break;
                        case 0xA000000B:
                            edList.Add(new KnownFolderDataBlock(bytes, offset));
                            break;
                        case 0xA000000C:
                            edList.Add(new VistaAndAboveIDListDataBlock(bytes, offset));
                            break;
                    }
                    
                    offset += datalength;

                } while (offset < bytes.Length - 0x04);

                ExtraData = edList.ToArray();

                #endregion EXTRA_DATA
            }
            else
            {
                throw new Exception("Invalid ShellLink Header.");
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ShellLink Get(string filePath)
        {
            FileRecord record = FileRecord.Get(filePath, true);
            return new ShellLink(record.GetContent(), record);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static ShellLink[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            NtfsVolumeBootRecord VBR = VolumeBootRecord.Get(volume) as NtfsVolumeBootRecord;

            List<ShellLink> slList = new List<ShellLink>();

            foreach (FileRecord r in FileRecord.GetInstances(volume))
            {
                try
                {
                    if (r.Name.Contains(".lnk"))
                    {
                        slList.Add(new ShellLink(r.GetContent(VBR), r));
                    }
                }
                catch
                {

                }
            }

            return slList.ToArray();
        }

        #endregion Static Methods

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[{0}] {1} {2}", FileSize, LocalBasePath, CommandLineArguments);
        }

        #endregion Override Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class IdList
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly ItemId[] ItemIdList;

        #endregion Properties

        #region Constructors

        internal IdList(byte[] bytes, int offset, ushort IdListSize)
        {
            int endoffset = offset + IdListSize - 0x02;

            List<ItemId> list = new List<ItemId>();

            while (offset < endoffset)
            {
                list.Add(new ItemId(bytes, ref offset));
            }

            ItemIdList = list.ToArray();
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class ItemId
    {
        #region Properties

        private readonly ushort ItemIdSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] Data;

        #endregion Properties

        #region Constructors

        internal ItemId(byte[] bytes, ref int offset)
        {
            ItemIdSize = BitConverter.ToUInt16(bytes, offset);
            Data = Helper.GetSubArray(bytes, offset + 0x02, ItemIdSize - 0x02);

            offset += ItemIdSize;
        }

        #endregion Constructors
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class VolumeId
    {
        #region Enums
        
        /// <summary>
        /// 
        /// </summary>
        public enum DRIVE_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            DRIVE_UNKNOWN = 0x00,

            /// <summary>
            /// 
            /// </summary>
            DRIVE_NO_ROOT_DIR = 0x01,

            /// <summary>
            /// 
            /// </summary>
            DRIVE_REMOVABLE = 0x02,

            /// <summary>
            /// 
            /// </summary>
            DRIVE_FIXED = 0x03,

            /// <summary>
            /// 
            /// </summary>
            DRIVE_REMOTE = 0x04,

            /// <summary>
            /// 
            /// </summary>
            DRIVE_CDROM = 0x05,

            /// <summary>
            /// 
            /// </summary>
            DRIVE_RAMDISK = 0x06
        }
        
        #endregion Enums

        #region Properties

        private readonly int VolumeIdSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly DRIVE_TYPE DriveType;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint DriveSerialNumber;

        private readonly uint VolumeLabelOffset;
        private readonly uint VolumeLabelOffsetUnicode;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] Data;

        #endregion Properties

        #region Constructors

        internal VolumeId(byte[] bytes, int offset)
        {
            VolumeIdSize = BitConverter.ToInt32(bytes, offset);
            DriveType = (DRIVE_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            DriveSerialNumber = BitConverter.ToUInt32(bytes, offset + 0x08);
            VolumeLabelOffset = BitConverter.ToUInt32(bytes, offset + 0x0C);
            
            int suboffset = 0;

            if (VolumeLabelOffset == 0x14)
            {
                VolumeLabelOffsetUnicode = BitConverter.ToUInt32(bytes, offset + 0x10);
                suboffset = 0x14;
            }
            else
            {
                suboffset = 0x10;
            }
            
            Data = Helper.GetSubArray(bytes, offset + suboffset, VolumeIdSize - suboffset);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommonNetworkRelativeLink
    {
        #region Enums
        
        /// <summary>
        /// 
        /// </summary>
        [Flags]
        private enum COMMON_NETWORK_RELATIVE_LINK_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            ValidDevice = 0x01,

            /// <summary>
            /// 
            /// </summary>
            ValidNetType = 0x02
        }

        /// <summary>
        /// 
        /// </summary>
        public enum NETWORK_PROVIDER_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_AVID = 0x001A0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_DOCUSPACE = 0x001B0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_MANGOSOFT = 0x001C0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_SERNET = 0x001D0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_RIVERFRONT1 = 0X001E0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_RIVERFRONT2 = 0x001F0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_DECORB = 0x00200000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_PROTSTOR = 0x00210000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_FJ_REDIR = 0x00220000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_DISTINCT = 0x00230000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_TWINS = 0x00240000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_RDR2SAMPLE = 0x00250000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_CSC = 0x00260000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_3IN1 = 0x00270000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_EXTENDNET = 0x00290000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_STAC = 0x002A0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_FOXBAT = 0x002B0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_YAHOO = 0x002C0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_EXIFS = 0x002D0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_DAV = 0x002E0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_KNOWARE = 0x002F0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_OBJECT_DIRE = 0x00300000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_MASFAX = 0x00310000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_HOB_NFS = 0x00320000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_SHIVA = 0x00330000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_IBMAL = 0x00340000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_LOCK = 0x00350000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_TERMSRV = 0x00360000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_SRT = 0x00370000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_QUINCY = 0x00380000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_OPENAFS = 0x00390000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_AVID1 = 0X003A0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_DFS = 0x003B0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_KWNP = 0x003C0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_ZENWORKS = 0x003D0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_DRIVEONWEB = 0x003E0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_VMWARE = 0x003F0000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_RSFX = 0x00400000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_MFILES = 0x00410000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_MS_NFS = 0x00420000,

            /// <summary>
            /// 
            /// </summary>
            WNNC_NET_GOOGLE = 0x00430000
        }

        #endregion Enums

        #region Properties

        private readonly uint CommonNetworkRelativeLinkSize;
        private readonly COMMON_NETWORK_RELATIVE_LINK_FLAGS CommonNetworkRelativeLinkFlags;
        private readonly int NetNameOffset;
        private readonly uint DeviceNameOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly NETWORK_PROVIDER_TYPE NetworkProviderType;
        private readonly uint NetNameOffsetUnicode;
        private readonly uint DeviceNameOffsetUnicode;

        /// <summary>
        /// 
        /// </summary>
        public readonly string NetName;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly string DeviceName;

        /// <summary>
        /// 
        /// </summary>
        public readonly string NetNameUnicode;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly string DeviceNameUnicode;

        #endregion Properties

        #region Constructors

        internal CommonNetworkRelativeLink(byte[] bytes, int offset)
        {
            CommonNetworkRelativeLinkSize = BitConverter.ToUInt32(bytes, offset);
            
            if (CommonNetworkRelativeLinkSize >= 0x14)
            {
                CommonNetworkRelativeLinkFlags = (COMMON_NETWORK_RELATIVE_LINK_FLAGS)BitConverter.ToUInt32(bytes, offset + 0x04);

                #region NetName
                NetNameOffset = BitConverter.ToInt32(bytes, offset + 0x08);
                NetName = Encoding.UTF8.GetString(bytes, offset + NetNameOffset, (int)CommonNetworkRelativeLinkSize - (int)NetNameOffset).Split('\0')[0];
                #endregion NetName

                #region DeviceName
                DeviceNameOffset = BitConverter.ToUInt32(bytes, offset + 0x0C);
                
                if(!((CommonNetworkRelativeLinkFlags & COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidDevice) == COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidDevice))
                {
                    throw new Exception("Invalid DeviceNameOffset value");
                }
                
                DeviceName = Encoding.UTF8.GetString(bytes, offset + (int)DeviceNameOffset, (int)CommonNetworkRelativeLinkSize - (int)DeviceNameOffset).Split('\0')[0];
                #endregion DeviceName

                #region NetworkProviderType
                if ((CommonNetworkRelativeLinkFlags & COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidNetType) == COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidNetType)
                {
                    NetworkProviderType = (NETWORK_PROVIDER_TYPE)BitConverter.ToUInt32(bytes, offset + 0x10);
                }
                #endregion NetworkProviderType

                if (NetNameOffset > 0x14)
                {
                    #region NetNameUnicode
                    NetNameOffsetUnicode = BitConverter.ToUInt32(bytes, offset + 0x14);
                    NetNameUnicode = Encoding.Unicode.GetString(bytes, offset + (int)NetNameOffsetUnicode, (int)CommonNetworkRelativeLinkSize - (int)NetNameOffsetUnicode).Split('\0')[0];
                    #endregion NetNameUnicode

                    #region DeviceNameUnicode
                    DeviceNameOffsetUnicode = BitConverter.ToUInt32(bytes, offset + 0x18);
                    DeviceNameUnicode = Encoding.Unicode.GetString(bytes, offset + (int)DeviceNameOffsetUnicode, (int)CommonNetworkRelativeLinkSize - (int)DeviceNameOffsetUnicode).Split('\0')[0];
                    #endregion DeviceNameUnicode
                }

                offset += (int)CommonNetworkRelativeLinkSize;
            }
            else
            {
                throw new Exception("Invalid CommonNetworkRelativeLink Object");
            }
        }

        #endregion Constructors
    }

    // More research needed
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleDataBlock : ExtraData
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum FILL
        {
            /// <summary>
            /// 
            /// </summary>
            FOREGROUND_BLUE = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            FOREGROUND_GREEN = 0x0002,

            /// <summary>
            /// 
            /// </summary>
            FOREGROUND_RED = 0x0004,

            /// <summary>
            /// 
            /// </summary>
            FOREGROUND_INTENSITY = 0x0008,

            /// <summary>
            /// 
            /// </summary>
            BACKGROUND_BLUE = 0x0010,

            /// <summary>
            /// 
            /// </summary>
            BACKGROUND_GREEN = 0x0020,

            /// <summary>
            /// 
            /// </summary>
            BACKGROUND_RED = 0x0040,

            /// <summary>
            /// 
            /// </summary>
            BACKGROUND_INTENSITY = 0x0080
        }

        /// <summary>
        /// 
        /// </summary>
        public enum FONT
        {
            /// <summary>
            /// 
            /// </summary>
            FF_DONTCARE = 0x0000,

            /// <summary>
            /// 
            /// </summary>
            FF_ROMAN = 0x0010,

            /// <summary>
            /// 
            /// </summary>
            FF_SWISS = 0x0020,

            /// <summary>
            /// 
            /// </summary>
            FF_MODERN = 0x0030,

            /// <summary>
            /// 
            /// </summary>
            FF_SCRIPT = 0x0040,

            /// <summary>
            /// 
            /// </summary>
            FF_DECORATIVE = 0x0050,
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly FILL FillAttribute;

        /// <summary>
        /// 
        /// </summary>
        public readonly FILL PopupFillAttribute;

        /// <summary>
        /// 
        /// </summary>
        public readonly short ScreenBufferSizeX;

        /// <summary>
        /// 
        /// </summary>
        public readonly short ScreenBufferSizeY;

        /// <summary>
        /// 
        /// </summary>
        public readonly short WindowSizeX;

        /// <summary>
        /// 
        /// </summary>
        public readonly short WindowSizeY;

        /// <summary>
        /// 
        /// </summary>
        public readonly short WindowOriginX;

        /// <summary>
        /// 
        /// </summary>
        public readonly short WindowOriginY;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FontSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly FONT FontFamily;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FontWeight;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FaceName;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CursorSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool FullScreen;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool QuickEdit;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool InsertMode;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool AutoPosition;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint HistoryBufferSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint NumberOfHistroyBuffers;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool HistoryNoDup;

        // Needs more research
        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] ColorTable;

        #endregion Properties

        #region Constructors

        internal ConsoleDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            FillAttribute = (FILL)BitConverter.ToUInt16(bytes, offset + 0x08);
            PopupFillAttribute = (FILL)BitConverter.ToUInt16(bytes, offset + 0x0A);
            ScreenBufferSizeX = BitConverter.ToInt16(bytes, offset + 0x0C);
            ScreenBufferSizeY = BitConverter.ToInt16(bytes, offset + 0x0E);
            WindowSizeX = BitConverter.ToInt16(bytes, offset + 0x10);
            WindowSizeY = BitConverter.ToInt16(bytes, offset + 0x12);
            WindowOriginX = BitConverter.ToInt16(bytes, offset + 0x14);
            WindowOriginY = BitConverter.ToInt16(bytes, offset + 0x16);
            FontSize = BitConverter.ToUInt32(bytes, offset + 0x20);
            FontFamily = (FONT)BitConverter.ToUInt32(bytes, offset + 0x24);
            FontWeight = BitConverter.ToUInt32(bytes, offset + 0x28);
            FaceName = Encoding.Unicode.GetString(bytes, offset + 0x2C, 0x40);
            CursorSize = BitConverter.ToUInt32(bytes, offset + 0x30);
            FullScreen = BitConverter.ToUInt32(bytes, offset + 0x34) != 0;
            QuickEdit = BitConverter.ToUInt32(bytes, offset + 0x38) != 0;
            InsertMode = BitConverter.ToUInt32(bytes, offset + 0x3C) != 0;
            AutoPosition = BitConverter.ToUInt32(bytes, offset + 0x40) != 0;
            HistoryBufferSize = BitConverter.ToUInt32(bytes, offset + 0x44);
            NumberOfHistroyBuffers = BitConverter.ToUInt32(bytes, offset + 0x48);
            HistoryNoDup = BitConverter.ToUInt32(bytes, offset + 0x4C) != 0;
            // More research needed
            ColorTable = Helper.GetSubArray(bytes, offset + 0x50, 0x40);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConsoleFeDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CodePage;

        #endregion Properties

        #region Constructors

        internal ConsoleFeDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            CodePage = BitConverter.ToUInt32(bytes, offset + 0x08);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class DarwinDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string DarwinDataAnsi;

        /// <summary>
        /// 
        /// </summary>
        public readonly string DarwinDataUnicode;

        #endregion Properties

        #region Constructors

        internal DarwinDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            DarwinDataAnsi = Encoding.ASCII.GetString(bytes, offset + 0x08, 0x104);
            DarwinDataUnicode = Encoding.Unicode.GetString(bytes, offset + 0x10C, 0x208);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class EnvironmentVariableDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string TargetAnsi;

        /// <summary>
        /// 
        /// </summary>
        public readonly string TargetUnicode;

        #endregion Properties

        #region Constructors

        internal EnvironmentVariableDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            TargetAnsi = Encoding.ASCII.GetString(bytes, offset + 0x08, 0x104);
            TargetUnicode = Encoding.Unicode.GetString(bytes, offset + 0x10C, 0x208);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExtraData
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum EXTRA_DATA_TYPE : uint
        {
            /// <summary>
            /// 
            /// </summary>
            EnvironmentVariableDataBlock = 0xA0000001,

            /// <summary>
            /// 
            /// </summary>
            ConsoleDataBlock = 0xA0000002,

            /// <summary>
            /// 
            /// </summary>
            TrackerDataBlock = 0xA0000003,

            /// <summary>
            /// 
            /// </summary>
            ConsoleFeDataBlock = 0xA0000004,

            /// <summary>
            /// 
            /// </summary>
            SpecialFolderDataBlock = 0xA0000005,

            /// <summary>
            /// 
            /// </summary>
            DarwinDataBlock = 0xA0000006,

            /// <summary>
            /// 
            /// </summary>
            IconEnvironmentDataBlock = 0xA0000007,

            /// <summary>
            /// 
            /// </summary>
            ShimDataBlock = 0xA0000008,

            /// <summary>
            /// 
            /// </summary>
            PropertyStoreDataBlock = 0xA0000009,

            /// <summary>
            /// 
            /// </summary>
            KnownFolderDataBlock = 0xA000000B,

            /// <summary>
            /// 
            /// </summary>
            VistaAndAboveIDListDataBlock = 0xA000000C
        }

        #endregion Enums

        #region Properties

        internal uint BlockSize;

        /// <summary>
        /// 
        /// </summary>
        public EXTRA_DATA_TYPE Name;

        #endregion Properties
    }

    /// <summary>
    /// 
    /// </summary>
    public class IconEnvironmentDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string TargetAnsi;

        /// <summary>
        /// 
        /// </summary>
        public readonly string TargetUnicode;

        #endregion Properties

        #region Constructors

        internal IconEnvironmentDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            TargetAnsi = Encoding.ASCII.GetString(bytes, offset + 0x08, 0x104);
            TargetUnicode = Encoding.Unicode.GetString(bytes, offset + 0x10C, 0x208);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class KnownFolderDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid KnownFolderId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Offset;

        #endregion Properties

        #region Constructors

        internal KnownFolderDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            KnownFolderId = new Guid(Helper.GetSubArray(bytes, offset + 0x08, 0x10));
            Offset = BitConverter.ToUInt32(bytes, offset + 0x18);
        }

        #endregion Constructors
    }

    // More research needed
    /// <summary>
    /// 
    /// </summary>
    public class PropertyStoreDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] PropertyStore;

        #endregion Properties

        #region Constructors

        internal PropertyStoreDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            PropertyStore = Helper.GetSubArray(bytes, offset + 0x08, (int)BlockSize - 0x08);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class ShimDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string LayerName;

        #endregion Properties

        #region Constructors

        internal ShimDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            LayerName = Encoding.Unicode.GetString(bytes, offset + 0x08, (int)BlockSize - 0x08);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class SpecialFolderDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SpecialFolderId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Offset;

        #endregion Properties

        #region Constructors

        internal SpecialFolderDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            SpecialFolderId = BitConverter.ToUInt32(bytes, offset + 0x08);
            Offset = BitConverter.ToUInt32(bytes, offset + 0x0C);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class TrackerDataBlock : ExtraData
    {
        #region Properties

        private readonly uint Length;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Version;

        /// <summary>
        /// 
        /// </summary>
        public readonly string MachineId;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid[] Droid;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid[] DroidBirth;

        #endregion Properties

        #region Constructors

        internal TrackerDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            Length = BitConverter.ToUInt32(bytes, offset + 0x08);
            Version = BitConverter.ToUInt32(bytes, offset + 0x0C);
            MachineId = Encoding.UTF8.GetString(bytes, offset + 0x10, (int)BlockSize - 0x10).Split('\0')[0];
            #region Droid
            uint DroidOffset = (uint)offset + 0x10 + (uint)MachineId.Length + 0x01;
            Droid = new Guid[2];
            Droid[0] = new Guid(Helper.GetSubArray(bytes, (int)DroidOffset, 0x10));
            Droid[1] = new Guid(Helper.GetSubArray(bytes, (int)DroidOffset + 0x10, 0x10));
            #endregion Droid
            #region DroidBirth
            DroidBirth = new Guid[2];
            DroidBirth[0] = new Guid(Helper.GetSubArray(bytes, (int)DroidOffset + 0x20, 0x10));
            DroidBirth[1] = new Guid(Helper.GetSubArray(bytes, (int)DroidOffset + 0x30, 0x10));
            #endregion DroidBirth
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class VistaAndAboveIDListDataBlock : ExtraData
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly IdList IdList;

        #endregion Properties

        #region Constructors

        internal VistaAndAboveIDListDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            //IdList = new IdList(bytes, offset + 0x08);
        }

        #endregion Constructors
    }
}