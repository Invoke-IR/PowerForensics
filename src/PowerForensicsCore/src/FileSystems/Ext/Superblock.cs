using System;
using System.Text;

namespace PowerForensics.Ext
{
    public class Superblock
    {
        #region Enums

        [Flags]
        public enum STATE
        {
            CleanlyUmounted = 0x0001,
            ErrorsDetected = 0x0002,
            RecoveringOrphans = 0x0004
        }

        public enum ERRORS
        {
            Continue = 1,
            RemountReadOnly = 2,
            Panic = 3
        }

        public enum CREATOR_OS
        {
            Linux = 0,
            Hurd = 1,
            Masix = 2,
            FreeBSD = 3,
            Lites = 4
        }

        public enum REVISION_LEVEL
        {
            Originalformat = 0,
            v2formatwithdynamicinodesizes = 1
        }

        [Flags]
        public enum FEATURE_COMPAT
        {
            COMPAT_DIR_PREALLOC = 0x1,
            COMPAT_IMAGIC_INODES = 0x2,
            COMPAT_HAS_JOURNAL = 0x4,
            COMPAT_EXT_ATTR = 0x8,
            COMPAT_RESIZE_INODE = 0x10,
            COMPAT_DIR_INDEX = 0x20,
            COMPAT_LAZY_BG = 0x40,
            COMPAT_EXCLUDE_INODE = 0x80,
            COMPAT_EXCLUDE_BITMAP = 0x100,
            COMPAT_SPARSE_SUPER2 = 0x200
        }

        [Flags]
        public enum FEATURE_INCOMPAT
        {
            INCOMPAT_COMPRESSION = 0x1,
            INCOMPAT_FILETYPE = 0x2,
            INCOMPAT_RECOVER = 0x4,
            INCOMPAT_JOURNAL_DEV = 0x8,
            INCOMPAT_META_BG = 0x10,
            INCOMPAT_EXTENTS = 0x40,
            INCOMPAT_64BIT = 0x80,
            INCOMPAT_MMP = 0x100,
            INCOMPAT_FLEX_BG = 0x200,
            INCOMPAT_EA_INODE = 0x400,
            INCOMPAT_DIRDATA = 0x1000,
            INCOMPAT_CSUM_SEED = 0x2000,
            INCOMPAT_LARGEDIR = 0x4000,
            INCOMPAT_INLINE_DATA = 0x8000,
            INCOMPAT_ENCRYPT = 0x10000
        }

        [Flags]
        public enum FEATURE_RO_COMPAT
        {
            RO_COMPAT_SPARSE_SUPER = 0x1,
            RO_COMPAT_LARGE_FILE = 0x2,
            RO_COMPAT_BTREE_DIR = 0x4,
            RO_COMPAT_HUGE_FILE = 0x8,
            RO_COMPAT_GDT_CSUM = 0x10,
            RO_COMPAT_DIR_NLINK = 0x20,
            RO_COMPAT_EXTRA_ISIZE = 0x40,
            RO_COMPAT_HAS_SNAPSHOT = 0x80,
            RO_COMPAT_QUOTA = 0x100,
            RO_COMPAT_BIGALLOC = 0x200,
            RO_COMPAT_METADATA_CSUM = 0x400,
            RO_COMPAT_REPLICA = 0x800,
            RO_COMPAT_READONLY = 0x1000,
            RO_COMPAT_PROJECT = 0x2000
        }

        public enum DEFAULT_HASH_VERSION
        {
            Legacy = 0x0,
            HalfMD4 = 0x1,
            Tea = 0x2,
            UnsignedLegacy = 0x3,
            UnsignedHalfMD4 = 0x4,
            UnsignedTea = 0x5
        }

        [Flags]
        public enum DEFAULT_MOUNT_OPTIONS
        {
            EXT4_DEFM_DEBUG = 0x0001,
            EXT4_DEFM_BSDGROUPS = 0x0002,
            EXT4_DEFM_XATTR_USER = 0x0004,
            EXT4_DEFM_ACL = 0x0008,
            EXT4_DEFM_UID16 = 0x0010,
            EXT4_DEFM_JMODE_DATA = 0x0020,
            EXT4_DEFM_JMODE_ORDERED = 0x0040,
            EXT4_DEFM_JMODE_WBACK = 0x0060,
            EXT4_DEFM_NOBARRIER = 0x0100,
            EXT4_DEFM_BLOCK_VALIDITY = 0x0200,
            EXT4_DEFM_DISCARD = 0x0400,
            EXT4_DEFM_NODELALLOC = 0x0800
        }

        [Flags]
        public enum FLAGS
        {
            SignedDirectoryHash = 0x0001,
            UnsignedDirectoryHash = 0x0002,
            DevelopmentCode = 0x0004
        }

        public enum CHECKSUM_TYPE
        {
            crc32c = 1
        }

        public enum ENCRYPTION_ALGORITHMS
        {
            ENCRYPTION_MODE_INVALID = 0,
            ENCRYPTION_MODE_AES_256_XTS = 1,
            ENCRYPTION_MODE_AES_256_GCM = 2,
            ENCRYPTION_MODE_AES_256_CBC = 3
        }

        #endregion Enums

        #region Properties

        public readonly uint InodeCount; //Total inode count.
        public readonly ulong BlockCount; //Total block count.
        public readonly ulong RootBlockCount; //This number of blocks can only be allocated by the super - user.
        public readonly ulong FreeBlockCount; //Free block count.
        public readonly uint FreeInodeCount; //Free inode count.
        public readonly uint FirstDataBlock; //First data block.This must be at least 1 for 1k - block filesystems and is typically 0 for all other block sizes.
        public readonly uint BlockSize; //Block size is 2 ^ (10 + s_log_block_size).
        public readonly uint ClusterSize; //Cluster size is (2 ^ s_log_cluster_size) blocks if bigalloc is enabled, zero otherwise.
        public readonly uint BlocksPerGroup; //Blocks per group.
        public readonly uint ClustersPerGroup; //Clusters per group, if bigalloc is enabled.
        public readonly uint InodesPerGroup; //Inodes per group.
        public readonly DateTime MountTime; //Mount time, in seconds since the epoch.
        public readonly DateTime WriteTime; //Write time, in seconds since the epoch.
        public readonly ushort MountCount; //Number of mounts since the last fsck.
        public readonly ushort MaxMountCount; //Number of mounts beyond which a fsck is needed.
        public readonly ushort Magic; //Magic signature, 0xEF53
        public readonly STATE State; //File system state
        public readonly ERRORS Errors; //Behaviour when detecting errors
        public readonly ushort MinorRevisionLevel; //Minor revision level.
        public readonly DateTime LastCheck; //Time of last check, in seconds since the epoch.
        public readonly uint CheckInterval; //Maximum time between checks, in seconds.
        public readonly CREATOR_OS CreatorOs;
        public readonly REVISION_LEVEL RevisionLevel; //Revision level.
        public readonly ushort DefaultUserId; //Default uid for reserved blocks.
        public readonly ushort DefaultGroupId; //Default gid for reserved blocks.
        public readonly uint FirstInode; //First non - reserved inode.
        public readonly ushort InodeSize; //Size of inode structure, in bytes.
        public readonly ushort BlockGroupNumber; //Block group # of this superblock.
        public readonly FEATURE_COMPAT FeatureCompat; //Compatible feature set flags.Kernel can still read / write this fs even if it doesn't understand a flag; fsck should not do that.
        public readonly FEATURE_INCOMPAT FeatureIncompat; //Incompatible feature set.If the kernel or fsck doesn't understand one of these bits, it should stop.
        public readonly FEATURE_RO_COMPAT FeatureRoCompat; //Readonly - compatible feature set. If the kernel doesn't understand one of these bits, it can still mount read-only. Any of:
        public readonly Guid Uuid; //128 - bit UUID for volume.
        public readonly string VolumeName; //Volume label.
        public readonly string LastMountedDirectory; //Directory where filesystem was last mounted.
        public readonly uint AlgorithmUsageBitmap; //For compression(Not used in e2fsprogs / Linux)
        public readonly byte PreallocatedBlocks; //# of blocks to try to preallocate for ... files? (Not used in e2fsprogs/Linux)
        public readonly byte PreallocatedDirectoryBlocks; //# of blocks to preallocate for directories. (Not used in e2fsprogs/Linux)
        public readonly ushort ReservedGdtBlocks; //Number of reserved GDT entries for future filesystem expansion.
        public readonly Guid JournalUuid; //UUID of journal superblock
        public readonly uint JournalInode; //inode number of journal file.
        public readonly uint JournalDevice; //Device number of journal file, if the external journal feature flag is set.
        public readonly uint LastOrphanedList; //Start of list of orphaned inodes to delete.
        public readonly uint HashSeed; //HTREE hash seed.
        public readonly DEFAULT_HASH_VERSION DefaultHashVersion; //Default hash algorithm to use for directory hashes. One of:
        public readonly byte JournalBackupType; //If this value is 0 or EXT3_JNL_BACKUP_BLOCKS(1), then the s_jnl_blocks field contains a duplicate copy of the inode's i_block[] array and i_size.
        public readonly ushort GroupDescriptorSize; //Size of group descriptors, in bytes, if the 64bit incompat feature flag is set.
        public readonly DEFAULT_MOUNT_OPTIONS DefaultMountOptions; //Default mount options.
        public readonly uint FirstMetablockBlockGroup; //First metablock block group, if the meta_bg feature is enabled.
        public readonly DateTime MkfsTime; //When the filesystem was created, in seconds since the epoch.
        public readonly uint JournalBlocks; //Backup copy of the journal inode's i_block[] array in the first 15 elements and i_size_high and i_size in the 16th and 17th elements, respectively.
        public readonly ushort MinimumExtraInodeSize; //All inodes have at least # bytes.
        public readonly ushort DesiredInodeSize; //New inodes should reserve # bytes.
        public readonly FLAGS Flags; //Miscellaneous flags.
        public readonly ushort RaidStride; //RAID stride. This is the number of logical blocks read from or written to the disk before moving to the next disk.This affects the placement of filesystem metadata, which will hopefully make RAID storage faster.
        public readonly ushort MmpInterval; //# seconds to wait in multi-mount prevention (MMP) checking. In theory, MMP is a mechanism to record in the superblock which host and device have mounted the filesystem, in order to prevent multiple mounts. This feature does not seem to be implemented...
        public readonly ulong MmpBlock; //Block # for multi-mount protection data.
        public readonly uint RaidStipeWidth; //RAID stripe width.This is the number of logical blocks read from or written to the disk before coming back to the current disk. This is used by the block allocator to try to reduce the number of read-modify - write operations in a RAID5/ 6.
        public readonly uint FlexibleBlockGroupSize; //Size of a flexible block group is 2 ^ s_log_groups_per_flex.
        public readonly CHECKSUM_TYPE ChecksumType; //Metadata checksum algorithm type. The only valid value is 1(crc32c).
        public readonly ulong KBytesWritten; //Number of KiB written to this filesystem over its lifetime.
        public readonly uint SnapshotInode; //inode number of active snapshot. (Not used in e2fsprogs / Linux.)
        public readonly uint SnapshotId; //Sequential ID of active snapshot. (Not used in e2fsprogs / Linux.)
        public readonly ulong ReservedSnapshotBlockCount; //Number of blocks reserved for active snapshot's future use. (Not used in e2fsprogs/Linux.)
        public readonly uint SnapshotListInode; //inode number of the head of the on - disk snapshot list. (Not used in e2fsprogs / Linux.)
        public readonly uint ErrorCount; //Number of errors seen.
        public readonly DateTime FirstErrorTime; //First time an error happened, in seconds since the epoch.
        public readonly uint FirstErrorInode; //inode involved in first error.
        public readonly ulong FirstErrorBlock; //Number of block involved of first error.
        public readonly string FirstErrorFunction; //Name of function where the error happened.
        public readonly uint FirstErrorLine; //Line number where error happened.
        public readonly DateTime LastErrorTime; //Time of most recent error, in seconds since the epoch.
        public readonly uint LastErrorInode; //inode involved in most recent error.
        public readonly uint LastErrorLine; //Line number where most recent error happened.
        public readonly ulong LastErrorBlock; //Number of block involved in most recent error.
        public readonly string LastErrorFunction; //Name of function where the most recent error happened.
        public readonly string MountOptions; //ASCIIZ string of mount options.
        public readonly uint UserQuotaInode; //Inode number of user quota file.
        public readonly uint GroupQuotaInode; //Inode number of group quota file.
        public readonly uint OverheadBlocks; //Overhead blocks / clusters in fs. (Huh ? This field is always zero, which means that the kernel calculates it dynamically.)
        public readonly uint SuperblockBackupBlockGroup; //Block groups containing superblock backups(if sparse_super2)
        public readonly ENCRYPTION_ALGORITHMS EncryptionAlgorithms; //Encryption algorithms in use.There can be up to four algorithms in use at any time
        public readonly byte EncryptPasswordSalt; //Salt for the string2key algorithm for encryption.
        public readonly uint LostAndFoundInode; //Inode number of lost + found
        public readonly uint ProjectQuotaInode; //Inode that tracks project quotas.
        public readonly uint ChecksumSeed; //Checksum seed used for metadata_csum calculations. This value is crc32c(~0, $orig_fs_uuid).
        public readonly uint Checksum; //Superblock checksum.

        #endregion Properties

        #region Constructors

        private Superblock(byte[] bytes)
        {
            InodeCount = BitConverter.ToUInt32(bytes, 0x00);
            BlockCount = (uint)1024 << BitConverter.ToInt32(bytes, 0x04);
            //BitConverter.ToUInt32(bytes, 0x150) << 32 | BitConverter.ToUInt32(bytes, 0x04);
            RootBlockCount = BitConverter.ToUInt32(bytes, 0x154) << 32 | BitConverter.ToUInt32(bytes, 0x08);
            FreeBlockCount = BitConverter.ToUInt32(bytes, 0x158) << 32 | BitConverter.ToUInt32(bytes, 0x0C);
            FreeInodeCount = BitConverter.ToUInt32(bytes, 0x10);
            FirstDataBlock = BitConverter.ToUInt32(bytes, 0x14);
            BlockSize = (uint)Math.Pow(2, (10 + BitConverter.ToUInt32(bytes, 0x18)));
            ClusterSize = (uint)Math.Pow(2, BitConverter.ToUInt32(bytes, 0x1C));
            BlocksPerGroup = BitConverter.ToUInt32(bytes, 0x20);
            ClustersPerGroup = BitConverter.ToUInt32(bytes, 0x24);
            InodesPerGroup = BitConverter.ToUInt32(bytes, 0x28);
            MountTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x2C));
            WriteTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x30));
            MountCount = BitConverter.ToUInt16(bytes, 0x34);
            MaxMountCount = BitConverter.ToUInt16(bytes, 0x36);
            Magic = BitConverter.ToUInt16(bytes, 0x38);
            State = (STATE)BitConverter.ToUInt16(bytes, 0x3A);
            Errors = (ERRORS)BitConverter.ToUInt16(bytes, 0x3C);
            MinorRevisionLevel = BitConverter.ToUInt16(bytes, 0x3E);
            LastCheck = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x40));
            CheckInterval = BitConverter.ToUInt32(bytes, 0x44);
            CreatorOs = (CREATOR_OS)BitConverter.ToUInt32(bytes, 0x48);
            RevisionLevel = (REVISION_LEVEL)BitConverter.ToUInt32(bytes, 0x4C);
            DefaultUserId = BitConverter.ToUInt16(bytes, 0x50);
            DefaultGroupId = BitConverter.ToUInt16(bytes, 0x52);
            FirstInode = BitConverter.ToUInt32(bytes, 0x54);
            InodeSize = BitConverter.ToUInt16(bytes, 0x58);
            BlockGroupNumber = BitConverter.ToUInt16(bytes, 0x5A);
            FeatureCompat = (FEATURE_COMPAT)BitConverter.ToUInt32(bytes, 0x5C);
            FeatureIncompat = (FEATURE_INCOMPAT)BitConverter.ToUInt32(bytes, 0x60);
            FeatureRoCompat = (FEATURE_RO_COMPAT)BitConverter.ToUInt32(bytes, 0x64);
            Uuid = new Guid(Helper.GetSubArray(bytes, 0x68, 0x10));
            VolumeName = Encoding.ASCII.GetString(bytes, 0x78, 0x10).TrimEnd('\0');
            LastMountedDirectory = Encoding.ASCII.GetString(bytes, 0x88, 0x40).TrimEnd('\0');
            AlgorithmUsageBitmap = BitConverter.ToUInt32(bytes, 0xC8);
            PreallocatedBlocks = bytes[0xCC];
            PreallocatedDirectoryBlocks = bytes[0xCD];
            ReservedGdtBlocks = BitConverter.ToUInt16(bytes, 0xCE);
            JournalUuid = new Guid(Helper.GetSubArray(bytes, 0xD0, 0x10));
            JournalInode = BitConverter.ToUInt32(bytes, 0xE0);
            JournalDevice = BitConverter.ToUInt32(bytes, 0xE4);
            LastOrphanedList = BitConverter.ToUInt32(bytes, 0xE8);
            HashSeed = BitConverter.ToUInt32(bytes, 0xEC);
            DefaultHashVersion = (DEFAULT_HASH_VERSION)bytes[0xFC];
            JournalBackupType = bytes[0xFD];
            GroupDescriptorSize = BitConverter.ToUInt16(bytes, 0xFE);
            DefaultMountOptions = (DEFAULT_MOUNT_OPTIONS)BitConverter.ToUInt32(bytes, 0x100);
            FirstMetablockBlockGroup = BitConverter.ToUInt32(bytes, 0x104);
            MkfsTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x108));
            JournalBlocks = BitConverter.ToUInt32(bytes, 0x10C);
            MinimumExtraInodeSize = BitConverter.ToUInt16(bytes, 0x15C);
            DesiredInodeSize = BitConverter.ToUInt16(bytes, 0x15E);
            Flags = (FLAGS)BitConverter.ToUInt32(bytes, 0x160);
            RaidStride = BitConverter.ToUInt16(bytes, 0x164);
            MmpInterval = BitConverter.ToUInt16(bytes, 0x166);
            MmpBlock = BitConverter.ToUInt64(bytes, 0x168);
            RaidStipeWidth = BitConverter.ToUInt32(bytes, 0x170);
            FlexibleBlockGroupSize = (uint)Math.Pow(2, bytes[0x174]);
            ChecksumType = (CHECKSUM_TYPE)bytes[0x175];
            KBytesWritten = BitConverter.ToUInt64(bytes, 0x178);
            SnapshotInode = BitConverter.ToUInt32(bytes, 0x180);
            SnapshotId = BitConverter.ToUInt32(bytes, 0x184);
            ReservedSnapshotBlockCount = BitConverter.ToUInt64(bytes, 0x188);
            SnapshotListInode = BitConverter.ToUInt32(bytes, 0x190);
            ErrorCount = BitConverter.ToUInt32(bytes, 0x194);
            FirstErrorTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x198));
            FirstErrorInode = BitConverter.ToUInt32(bytes, 0x19C);
            FirstErrorBlock = BitConverter.ToUInt64(bytes, 0x1A0);
            FirstErrorFunction = Encoding.ASCII.GetString(bytes, 0x1A8, 0x20).TrimEnd('\0');
            FirstErrorLine = BitConverter.ToUInt32(bytes, 0x1C8);
            LastErrorTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x1CC));
            LastErrorInode = BitConverter.ToUInt32(bytes, 0x1D0);
            LastErrorLine = BitConverter.ToUInt32(bytes, 0x1D4);
            LastErrorBlock = BitConverter.ToUInt64(bytes, 0x1D8);
            LastErrorFunction = Encoding.ASCII.GetString(bytes, 0x1E0, 0x20).TrimEnd('\0');
            MountOptions = Encoding.ASCII.GetString(bytes, 0x200, 0x40).TrimEnd('\0');
            UserQuotaInode = BitConverter.ToUInt32(bytes, 0x240);
            GroupQuotaInode = BitConverter.ToUInt32(bytes, 0x244);
            OverheadBlocks = BitConverter.ToUInt32(bytes, 0x248);
            SuperblockBackupBlockGroup = BitConverter.ToUInt32(bytes, 0x24C);
            EncryptionAlgorithms = (ENCRYPTION_ALGORITHMS)BitConverter.ToUInt32(bytes, 0x254);
            EncryptPasswordSalt = bytes[0x258];
            LostAndFoundInode = BitConverter.ToUInt32(bytes, 0x268);
            ProjectQuotaInode = BitConverter.ToUInt32(bytes, 0x26C);
            ChecksumSeed = BitConverter.ToUInt32(bytes, 0x270);
            Checksum = BitConverter.ToUInt32(bytes, 0x3FC);
        }

        #endregion Constructors

        #region StaticMethods

        public static Superblock Get(string volumenName)
        {
            return new Superblock(Helper.readDrive(volumenName, 0x400, 0x400));
        }

        #endregion StaticMethods
    }
}
