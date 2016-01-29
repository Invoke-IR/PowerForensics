using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Ntfs;

namespace PowerForensics.Artifacts
{
    public class ShellLink
    {
        #region Enums
        
        [FlagsAttribute]
        public enum LINK_FLAGS
        {
            HasLinkTargetIdList = 0x00000001,
            HasLinkInfo = 0x00000002,
            HasName = 0x00000004,
            HasRelativePath = 0x00000008,
            HasWorkingDir = 0x00000010,
            HasArguments = 0x00000020,
            HasIconLocation = 0x00000040,
            IsUnicode = 0x00000080,
            ForceNoLinkInfo = 0x00000100,
            HasExpString = 0x00000200,
            RunInSeparateProcess = 0x00000400,
            Unused1 = 0x00000800,
            HasDarwinId = 0x00001000,
            RunAsUser = 0x00002000,
            HasExpIcon = 0x00004000,
            NoPidlAlias = 0x00008000,
            Unused2 = 0x00010000,
            RunWithShimLayer = 0x00020000,
            ForceNoLinkTrack = 0x00040000,
            EnableTargetMetadata = 0x00080000,
            DisableLinkPathTracking = 0x00100000,
            DisableKnownFolderTracking = 0x00200000,
            DisableKnownFolderAlias = 0x00400000,
            AllowLinkToLink = 0x00800000,
            UnaliasOnSave = 0x01000000,
            PreferEnvironmentPath = 0x02000000,
            KeepLocalIdListForUncTarget = 0x04000000
        }

        [FlagsAttribute]
        public enum FILEATTRIBUTE_FLAGS
        {
            FILE_ATTRIBUTE_READONLY = 0x0001,
            FILE_ATTRIBUTE_HIDDEN = 0x0002,
            FILE_ATTRIBUTE_SYSTEM = 0x0004,
            Reserved1 = 0x0008,
            FILE_ATTRIBUTE_DIRECTORY = 0x0010,
            FILE_ATTRIBUTE_ARCHIVE = 0x0020,
            Reserved2 = 0x0040,
            FILE_ATTRIBUTE_NORMAL = 0x0080,
            FILE_ATTRIBUTE_TEMPORARY = 0x0100,
            FILE_ATTRIBUTE_SPARSE_FILE = 0x0200,
            FILE_ATTRIBUTE_REPARSE_POINT = 0x0400,
            FILE_ATTRIBUTE_COMPRESSED = 0x0800,
            FILE_ATTRIBUTE_OFFLINE = 0x1000,
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x2000,
            FILE_ATTRIBUTE_ENCRYPTED = 0x4000
        }

        public enum SHOWCOMMAND
        {
            SW_SHOWNORMAL = 0x00000001,
            SW_SHOWMAXIMIZED = 0x00000003,
            SW_SHOWMINNOACTIVE = 0x00000007
        }

        [FlagsAttribute]
        public enum HOTKEY_FLAGS
        {
            HOTKEYF_SHIFT = 0x01,
            HOTKEYF_CONTROL = 0x02,
            HOTKEYF_ALT = 0x04,
            K_0 = 0x30,
            K_1 = 0x31,
            K_2 = 0x32,
            K_3 = 0x33,
            K_4 = 0x34,
            K_5 = 0x35,
            K_6 = 0x36,
            K_7 = 0x37,
            K_8 = 0x38,
            K_9 = 0x39,
            K_A = 0x41,
            K_B = 0x42,
            K_C = 0x43,
            K_D = 0x44,
            K_E = 0x45,
            K_F = 0x46,
            K_G = 0x47,
            K_H = 0x48,
            K_I = 0x49,
            K_J = 0x4A,
            K_K = 0x4B,
            K_L = 0x4C,
            K_M = 0x4D,
            K_N = 0x4E,
            K_O = 0x4F,
            K_P = 0x50,
            K_Q = 0x51,
            K_R = 0x52,
            K_S = 0x53,
            K_T = 0x54,
            K_U = 0x55,
            K_V = 0x56,
            K_W = 0x57,
            K_X = 0x58,
            K_Y = 0x59,
            K_Z = 0x5A,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91
        }

        [FlagsAttribute]
        public enum LINKINFO_FLAGS
        {
            VolumeIDAndLocalBasePath = 0x01,
            CommonNetworkRelativeLinkAndPathSuffix = 0x02
        }

        #endregion Enums

        #region Properties

        public readonly string Path;

        // SHELL_LINK_HEADER
        private readonly int HeaderSize;
        private readonly Guid LinkCLSID;
        public readonly LINK_FLAGS LinkFlags;
        public readonly FILEATTRIBUTE_FLAGS FileAttributes;
        public readonly DateTime CreationTime;
        public readonly DateTime AccessTime;
        public readonly DateTime WriteTime;
        public readonly uint FileSize;
        public readonly int IconIndex;
        public readonly SHOWCOMMAND ShowCommand;
        public readonly HOTKEY_FLAGS[] HotKey;

        // LINKTARGET_IDLIST
        private readonly ushort IdListSize;
        public readonly IdList IdList;

        // LINKINFO
        private readonly uint LinkInfoSize;
        private readonly uint LinkInfoHeaderSize;
        private readonly LINKINFO_FLAGS LinkInfoFlags;
        private readonly uint VolumeIdOffset;
        private readonly uint LocalBasePathOffset;
        private readonly uint CommonNetworkRelativeLinkOffset;
        private readonly uint CommonPathSuffixOffset;
        private readonly uint LocalBasePathOffsetUnicode;
        private readonly uint CommonPathSuffixOffsetUnicode;
        public readonly VolumeId VolumeId;
        public readonly string LocalBasePath;
        public readonly CommonNetworkRelativeLink CommonNetworkRelativeLink;
        public readonly string CommonPathSuffix;
        public readonly string LocalBasePathUnicode;
        public readonly string CommonPathSuffixUnicode;

        // STRING_DATA
        private readonly ushort NameSize;
        public readonly string Name;
        private readonly ushort RelativePathSize;
        public readonly string RelativePath;
        private readonly ushort WorkingDirectorySize;
        public readonly string WorkingDirectory;
        private readonly ushort CommandLineArgumentsSize;
        public readonly string CommandLineArguments;
        private readonly ushort IconLocationSize;
        public readonly string IconLocation;

        // EXTRA_DATA
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
                    LinkInfoSize = BitConverter.ToUInt32(bytes, offset);
                    LinkInfoHeaderSize = BitConverter.ToUInt32(bytes, offset + 0x04);
                    LinkInfoFlags = (LINKINFO_FLAGS)BitConverter.ToUInt32(bytes, offset + 0x08);
                    
                    if ((LinkInfoFlags & LINKINFO_FLAGS.VolumeIDAndLocalBasePath) == LINKINFO_FLAGS.VolumeIDAndLocalBasePath)
                    {
                        VolumeIdOffset = BitConverter.ToUInt32(bytes, offset + 0x0C);
                        VolumeId = new VolumeId(bytes, offset + (int)VolumeIdOffset);

                        LocalBasePathOffset = BitConverter.ToUInt32(bytes, offset + 0x10);
                        LocalBasePath = Encoding.Default.GetString(bytes, offset + (int)LocalBasePathOffset, (int)LinkInfoSize - (int)LocalBasePathOffset).Split('\0')[0];
                    }

                    if ((LinkInfoFlags & LINKINFO_FLAGS.CommonNetworkRelativeLinkAndPathSuffix) == LINKINFO_FLAGS.CommonNetworkRelativeLinkAndPathSuffix)
                    {
                        CommonNetworkRelativeLinkOffset = BitConverter.ToUInt32(bytes, offset + 0x14);    
                        CommonNetworkRelativeLink = new CommonNetworkRelativeLink(bytes, offset + (int)CommonNetworkRelativeLinkOffset);

                        CommonPathSuffixOffset = BitConverter.ToUInt32(bytes, offset + 0x18);
                        CommonPathSuffix = Encoding.Default.GetString(bytes, offset + (int)CommonPathSuffixOffset, (int)LinkInfoSize - (int)CommonPathSuffixOffset).Split('\0')[0];
                    }

                    if (LinkInfoHeaderSize >= 0x24)
                    {
                        LocalBasePathOffsetUnicode = BitConverter.ToUInt32(bytes, offset + 0x1C);
                        LocalBasePathUnicode = Encoding.Unicode.GetString(bytes, offset + (int)LocalBasePathOffsetUnicode, (int)LinkInfoSize - (int)LocalBasePathOffsetUnicode).Split('\0')[0];

                        CommonPathSuffixOffsetUnicode = BitConverter.ToUInt32(bytes, offset + 0x20);
                        CommonPathSuffixUnicode = Encoding.Unicode.GetString(bytes, offset + (int)CommonPathSuffixOffsetUnicode, (int)LinkInfoSize - (int)CommonPathSuffixOffsetUnicode).Split('\0')[0];
                    }

                    offset += (int)LinkInfoSize;
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

        #region StaticMethods

        public static ShellLink Get(string filePath)
        {
            FileRecord record = FileRecord.Get(filePath, true);
            return new ShellLink(record.GetContent(), record);
        }

        public static ShellLink[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            VolumeBootRecord VBR = VolumeBootRecord.Get(volume);

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

        #endregion StaticMethods

        #region OverrideMethods

        public override string ToString()
        {
            return String.Format("[{0}] {1} {2}", FileSize, LocalBasePath, CommandLineArguments);
        }

        #endregion OverrideMethods
    }

    #region LINKTARGET_IDLIST Classes

    public class IdList
    {
        #region Properties

        public readonly ItemId[] ItemIdList;

        #endregion Properties

        #region Constructors

        internal IdList(byte[] bytes, int offset, ushort IdListSize)
        {
            int endoffset = offset + (int)IdListSize - 0x02;

            List<ItemId> list = new List<ItemId>();

            while (offset < endoffset)
            {
                list.Add(new ItemId(bytes, ref offset));
            }

            ItemIdList = list.ToArray();
        }

        #endregion Constructors
    }

    public class ItemId
    {
        #region Properties

        private readonly ushort ItemIdSize;
        public readonly byte[] Data;

        #endregion Properties

        #region Constructors

        internal ItemId(byte[] bytes, ref int offset)
        {
            ItemIdSize = BitConverter.ToUInt16(bytes, offset);
            Data = Helper.GetSubArray(bytes, offset + 0x02, ItemIdSize - 0x02);

            offset += (int)ItemIdSize;
        }

        #endregion Constructors
    }

    #endregion LINKTARGET_IDLIST Classes

    #region LINK_INFO Classes
    
    public class VolumeId
    {
        #region Enums
        
        public enum DRIVE_TYPE
        {
            DRIVE_UNKNOWN = 0x00,
            DRIVE_NO_ROOT_DIR = 0x01,
            DRIVE_REMOVABLE = 0x02,
            DRIVE_FIXED = 0x03,
            DRIVE_REMOTE = 0x04,
            DRIVE_CDROM = 0x05,
            DRIVE_RAMDISK = 0x06
        }
        
        #endregion Enums

        #region Properties

        private readonly uint VolumeIdSize;
        public readonly DRIVE_TYPE DriveType;
        public readonly uint DriveSerialNumber;
        private readonly uint VolumeLabelOffset;
        private readonly uint VolumeLabelOffsetUnicode;
        public readonly byte[] Data;

        #endregion Properties

        #region Constructors

        internal VolumeId(byte[] bytes, int offset)
        {
            VolumeIdSize = BitConverter.ToUInt32(bytes, offset);
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
            
            Data = Helper.GetSubArray(bytes, offset + suboffset, (int)VolumeIdSize - suboffset);
        }

        #endregion Constructors
    }

    public class CommonNetworkRelativeLink
    {
        #region Enums
        
        [FlagsAttribute]
        private enum COMMON_NETWORK_RELATIVE_LINK_FLAGS
        {
            ValidDevice = 0x01,
            ValidNetType = 0x02
        }

        public enum NETWORK_PROVIDER_TYPE
        {
            WNNC_NET_AVID = 0x001A0000,
            WNNC_NET_DOCUSPACE = 0x001B0000,
            WNNC_NET_MANGOSOFT = 0x001C0000,
            WNNC_NET_SERNET = 0x001D0000,
            WNNC_NET_RIVERFRONT1 = 0X001E0000,
            WNNC_NET_RIVERFRONT2 = 0x001F0000,
            WNNC_NET_DECORB = 0x00200000,
            WNNC_NET_PROTSTOR = 0x00210000,
            WNNC_NET_FJ_REDIR = 0x00220000,
            WNNC_NET_DISTINCT = 0x00230000,
            WNNC_NET_TWINS = 0x00240000,
            WNNC_NET_RDR2SAMPLE = 0x00250000,
            WNNC_NET_CSC = 0x00260000,
            WNNC_NET_3IN1 = 0x00270000,
            WNNC_NET_EXTENDNET = 0x00290000,
            WNNC_NET_STAC = 0x002A0000,
            WNNC_NET_FOXBAT = 0x002B0000,
            WNNC_NET_YAHOO = 0x002C0000,
            WNNC_NET_EXIFS = 0x002D0000,
            WNNC_NET_DAV = 0x002E0000,
            WNNC_NET_KNOWARE = 0x002F0000,
            WNNC_NET_OBJECT_DIRE = 0x00300000,
            WNNC_NET_MASFAX = 0x00310000,
            WNNC_NET_HOB_NFS = 0x00320000,
            WNNC_NET_SHIVA = 0x00330000,
            WNNC_NET_IBMAL = 0x00340000,
            WNNC_NET_LOCK = 0x00350000,
            WNNC_NET_TERMSRV = 0x00360000,
            WNNC_NET_SRT = 0x00370000,
            WNNC_NET_QUINCY = 0x00380000,
            WNNC_NET_OPENAFS = 0x00390000,
            WNNC_NET_AVID1 = 0X003A0000,
            WNNC_NET_DFS = 0x003B0000,
            WNNC_NET_KWNP = 0x003C0000,
            WNNC_NET_ZENWORKS = 0x003D0000,
            WNNC_NET_DRIVEONWEB = 0x003E0000,
            WNNC_NET_VMWARE = 0x003F0000,
            WNNC_NET_RSFX = 0x00400000,
            WNNC_NET_MFILES = 0x00410000,
            WNNC_NET_MS_NFS = 0x00420000,
            WNNC_NET_GOOGLE = 0x00430000
        }

        #endregion Enums

        #region Properties

        private readonly uint CommonNetworkRelativeLinkSize;
        private readonly COMMON_NETWORK_RELATIVE_LINK_FLAGS CommonNetworkRelativeLinkFlags;
        private readonly uint NetNameOffset;
        private readonly uint DeviceNameOffset;
        public readonly NETWORK_PROVIDER_TYPE NetworkProviderType;
        private readonly uint NetNameOffsetUnicode;
        private readonly uint DeviceNameOffsetUnicode;
        public readonly string NetName;
        public readonly string DeviceName;
        public readonly string NetNameUnicode;
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
                NetNameOffset = BitConverter.ToUInt32(bytes, offset + 0x08);
                NetName = Encoding.Default.GetString(bytes, offset + (int)NetNameOffset, (int)CommonNetworkRelativeLinkSize - (int)NetNameOffset).Split('\0')[0];
                #endregion NetName

                #region DeviceName
                DeviceNameOffset = BitConverter.ToUInt32(bytes, offset + 0x0C);
                
                if(!((CommonNetworkRelativeLinkFlags & COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidDevice) == COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidDevice))
                {
                    throw new Exception("Invalid DeviceNameOffset value");
                }
                
                DeviceName = Encoding.Default.GetString(bytes, offset + (int)DeviceNameOffset, (int)CommonNetworkRelativeLinkSize - (int)DeviceNameOffset).Split('\0')[0];
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
    
    #endregion LINK_INFO Classes

    // More research needed
    #region EXTRA_DATA Classes
    public class ExtraData
    {
        #region Enums

        public enum EXTRA_DATA_TYPE : uint
        {
            EnvironmentVariableDataBlock = 0xA0000001,
            ConsoleDataBlock = 0xA0000002,
            TrackerDataBlock = 0xA0000003,
            ConsoleFeDataBlock = 0xA0000004,
            SpecialFolderDataBlock = 0xA0000005,
            DarwinDataBlock = 0xA0000006,
            IconEnvironmentDataBlock = 0xA0000007,
            ShimDataBlock = 0xA0000008,
            PropertyStoreDataBlock = 0xA0000009,
            KnownFolderDataBlock = 0xA000000B,
            VistaAndAboveIDListDataBlock = 0xA000000C
        }

        #endregion Enums
        
        #region Properties

        internal uint BlockSize;
        public EXTRA_DATA_TYPE Name;

        #endregion Properties
    }

    // More research needed
    public class ConsoleDataBlock : ExtraData
    {
        #region Enums
        
        public enum FILL
        {
            FOREGROUND_BLUE = 0x0001,
            FOREGROUND_GREEN = 0x0002,
            FOREGROUND_RED = 0x0004,
            FOREGROUND_INTENSITY = 0x0008,
            BACKGROUND_BLUE = 0x0010,
            BACKGROUND_GREEN = 0x0020,
            BACKGROUND_RED = 0x0040,
            BACKGROUND_INTENSITY = 0x0080
        }

        public enum FONT
        {
            FF_DONTCARE = 0x0000,
            FF_ROMAN = 0x0010,
            FF_SWISS = 0x0020,
            FF_MODERN = 0x0030,
            FF_SCRIPT = 0x0040,
            FF_DECORATIVE = 0x0050,
        }

        #endregion Enums

        #region Properties
        
        public readonly FILL FillAttribute;
        public readonly FILL PopupFillAttribute;
        public readonly short ScreenBufferSizeX;
        public readonly short ScreenBufferSizeY;
        public readonly short WindowSizeX;
        public readonly short WindowSizeY;
        public readonly short WindowOriginX;
        public readonly short WindowOriginY;
        public readonly uint FontSize;
        public readonly FONT FontFamily;
        public readonly uint FontWeight;
        public readonly string FaceName;
        public readonly uint CursorSize;
        public readonly bool FullScreen;
        public readonly bool QuickEdit;
        public readonly bool InsertMode;
        public readonly bool AutoPosition;
        public readonly uint HistoryBufferSize;
        public readonly uint NumberOfHistroyBuffers;
        public readonly bool HistoryNoDup;
        // Needs more research
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

    public class ConsoleFeDataBlock : ExtraData
    {
        #region Properties

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
        
    public class DarwinDataBlock : ExtraData
    {
        #region Properties

        public readonly string DarwinDataAnsi;
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

    public class EnvironmentVariableDataBlock : ExtraData
    {
        #region Properties

        public readonly string TargetAnsi;
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
    
    public class IconEnvironmentDataBlock : ExtraData
    {
        #region Properties

        public readonly string TargetAnsi;
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
    
    public class KnownFolderDataBlock : ExtraData
    {
        #region Properties

        public readonly Guid KnownFolderId;
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
    public class PropertyStoreDataBlock : ExtraData
    {
        #region Properties

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
        
    public class ShimDataBlock : ExtraData
    {
        #region Properties

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
        
    public class SpecialFolderDataBlock : ExtraData
    {
        #region Properties

        public readonly uint SpecialFolderId;
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
        
    public class TrackerDataBlock : ExtraData
    {
        #region Properties
        
        private readonly uint Length;
        public readonly uint Version;
        public readonly string MachineId;
        public readonly Guid[] Droid;
        public readonly Guid[] DroidBirth;

        #endregion Properties

        #region Constructors

        internal TrackerDataBlock(byte[] bytes, int offset)
        {
            BlockSize = BitConverter.ToUInt32(bytes, offset);
            Name = (EXTRA_DATA_TYPE)BitConverter.ToUInt32(bytes, offset + 0x04);
            Length = BitConverter.ToUInt32(bytes, offset + 0x08);
            Version = BitConverter.ToUInt32(bytes, offset + 0x0C);
            MachineId = Encoding.Default.GetString(bytes, offset + 0x10, (int)BlockSize - 0x10).Split('\0')[0];
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
    
    public class VistaAndAboveIDListDataBlock : ExtraData
    {
        #region Properties

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
    
    #endregion EXTRA_DATA Classes
}
