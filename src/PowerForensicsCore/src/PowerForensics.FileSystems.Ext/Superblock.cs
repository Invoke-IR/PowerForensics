using System;
using System.Text;

namespace PowerForensics.FileSystems.Ext
{
    /// <summary>
    /// 
    /// </summary>
    public class Superblock
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum STATE
        {
            /// <summary>
            /// 
            /// </summary>
            CleanlyUmounted = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            ErrorsDetected = 0x0002,

            /// <summary>
            /// 
            /// </summary>
            RecoveringOrphans = 0x0004
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ERRORS
        {
            /// <summary>
            /// 
            /// </summary>
            Continue = 1,

            /// <summary>
            /// 
            /// </summary>
            RemountReadOnly = 2,

            /// <summary>
            /// 
            /// </summary>
            Panic = 3
        }

        /// <summary>
        /// 
        /// </summary>
        public enum CREATOR_OS
        {
            /// <summary>
            /// 
            /// </summary>
            Linux = 0,

            /// <summary>
            /// 
            /// </summary>
            Hurd = 1,

            /// <summary>
            /// 
            /// </summary>
            Masix = 2,

            /// <summary>
            /// 
            /// </summary>
            FreeBSD = 3,

            /// <summary>
            /// 
            /// </summary>
            Lites = 4
        }

        /// <summary>
        /// 
        /// </summary>
        public enum REVISION_LEVEL
        {
            /// <summary>
            /// 
            /// </summary>
            Originalformat = 0,

            /// <summary>
            /// 
            /// </summary>
            v2formatwithdynamicinodesizes = 1
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FEATURE_COMPAT
        {
            /// <summary>
            /// 
            /// </summary>
            COMPAT_DIR_PREALLOC = 0x1,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_IMAGIC_INODES = 0x2,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_HAS_JOURNAL = 0x4,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_EXT_ATTR = 0x8,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_RESIZE_INODE = 0x10,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_DIR_INDEX = 0x20,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_LAZY_BG = 0x40,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_EXCLUDE_INODE = 0x80,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_EXCLUDE_BITMAP = 0x100,

            /// <summary>
            /// 
            /// </summary>
            COMPAT_SPARSE_SUPER2 = 0x200
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FEATURE_INCOMPAT
        {
            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_COMPRESSION = 0x1,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_FILETYPE = 0x2,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_RECOVER = 0x4,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_JOURNAL_DEV = 0x8,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_META_BG = 0x10,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_EXTENTS = 0x40,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_64BIT = 0x80,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_MMP = 0x100,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_FLEX_BG = 0x200,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_EA_INODE = 0x400,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_DIRDATA = 0x1000,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_CSUM_SEED = 0x2000,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_LARGEDIR = 0x4000,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_INLINE_DATA = 0x8000,

            /// <summary>
            /// 
            /// </summary>
            INCOMPAT_ENCRYPT = 0x10000
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FEATURE_RO_COMPAT
        {
            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_SPARSE_SUPER = 0x1,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_LARGE_FILE = 0x2,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_BTREE_DIR = 0x4,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_HUGE_FILE = 0x8,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_GDT_CSUM = 0x10,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_DIR_NLINK = 0x20,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_EXTRA_ISIZE = 0x40,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_HAS_SNAPSHOT = 0x80,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_QUOTA = 0x100,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_BIGALLOC = 0x200,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_METADATA_CSUM = 0x400,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_REPLICA = 0x800,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_READONLY = 0x1000,

            /// <summary>
            /// 
            /// </summary>
            RO_COMPAT_PROJECT = 0x2000
        }

        /// <summary>
        /// 
        /// </summary>
        public enum DEFAULT_HASH_VERSION
        {
            /// <summary>
            /// 
            /// </summary>
            Legacy = 0x0,

            /// <summary>
            /// 
            /// </summary>
            HalfMD4 = 0x1,

            /// <summary>
            /// 
            /// </summary>
            Tea = 0x2,

            /// <summary>
            /// 
            /// </summary>
            UnsignedLegacy = 0x3,

            /// <summary>
            /// 
            /// </summary>
            UnsignedHalfMD4 = 0x4,

            /// <summary>
            /// 
            /// </summary>
            UnsignedTea = 0x5
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum DEFAULT_MOUNT_OPTIONS
        {
            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_DEBUG = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_BSDGROUPS = 0x0002,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_XATTR_USER = 0x0004,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_ACL = 0x0008,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_UID16 = 0x0010,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_JMODE_DATA = 0x0020,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_JMODE_ORDERED = 0x0040,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_JMODE_WBACK = 0x0060,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_NOBARRIER = 0x0100,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_BLOCK_VALIDITY = 0x0200,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_DISCARD = 0x0400,

            /// <summary>
            /// 
            /// </summary>
            EXT4_DEFM_NODELALLOC = 0x0800
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            SignedDirectoryHash = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            UnsignedDirectoryHash = 0x0002,

            /// <summary>
            /// 
            /// </summary>
            DevelopmentCode = 0x0004
        }

        /// <summary>
        /// 
        /// </summary>
        public enum CHECKSUM_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            crc32c = 1
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ENCRYPTION_ALGORITHMS
        {
            /// <summary>
            /// 
            /// </summary>
            ENCRYPTION_MODE_INVALID = 0,

            /// <summary>
            /// 
            /// </summary>
            ENCRYPTION_MODE_AES_256_XTS = 1,

            /// <summary>
            /// 
            /// </summary>
            ENCRYPTION_MODE_AES_256_GCM = 2,

            /// <summary>
            /// 
            /// </summary>
            ENCRYPTION_MODE_AES_256_CBC = 3
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly uint InodeCount; //Total inode count.

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong BlockCount; //Total block count.

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RootBlockCount; //This number of blocks can only be allocated by the super - user.

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong FreeBlockCount; //Free block count.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FreeInodeCount; //Free inode count.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FirstDataBlock; //First data block.This must be at least 1 for 1k - block filesystems and is typically 0 for all other block sizes.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint BlockSize; //Block size is 2 ^ (10 + s_log_block_size).

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClusterSize; //Cluster size is (2 ^ s_log_cluster_size) blocks if bigalloc is enabled, zero otherwise.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint BlocksPerGroup; //Blocks per group.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClustersPerGroup; //Clusters per group, if bigalloc is enabled.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint InodesPerGroup; //Inodes per group.

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime MountTime; //Mount time, in seconds since the epoch.

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime WriteTime; //Write time, in seconds since the epoch.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MountCount; //Number of mounts since the last fsck.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MaxMountCount; //Number of mounts beyond which a fsck is needed.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort Magic; //Magic signature, 0xEF53

        /// <summary>
        /// 
        /// </summary>
        public readonly STATE State; //File system state

        /// <summary>
        /// 
        /// </summary>
        public readonly ERRORS Errors; //Behaviour when detecting errors

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MinorRevisionLevel; //Minor revision level.

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime LastCheck; //Time of last check, in seconds since the epoch.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CheckInterval; //Maximum time between checks, in seconds.

        /// <summary>
        /// 
        /// </summary>
        public readonly CREATOR_OS CreatorOs;

        /// <summary>
        /// 
        /// </summary>
        public readonly REVISION_LEVEL RevisionLevel; //Revision level.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort DefaultUserId; //Default uid for reserved blocks.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort DefaultGroupId; //Default gid for reserved blocks.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FirstInode; //First non - reserved inode.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort InodeSize; //Size of inode structure, in bytes.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort BlockGroupNumber; //Block group # of this superblock.

        /// <summary>
        /// 
        /// </summary>
        public readonly FEATURE_COMPAT FeatureCompat; //Compatible feature set flags.Kernel can still read / write this fs even if it doesn't understand a flag; fsck should not do that.

        /// <summary>
        /// 
        /// </summary>
        public readonly FEATURE_INCOMPAT FeatureIncompat; //Incompatible feature set.If the kernel or fsck doesn't understand one of these bits, it should stop.

        /// <summary>
        /// 
        /// </summary>
        public readonly FEATURE_RO_COMPAT FeatureRoCompat; //Readonly - compatible feature set. If the kernel doesn't understand one of these bits, it can still mount read-only. Any of:

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid Uuid; //128 - bit UUID for volume.

        /// <summary>
        /// 
        /// </summary>
        public readonly string VolumeName; //Volume label.

        /// <summary>
        /// 
        /// </summary>
        public readonly string LastMountedDirectory; //Directory where filesystem was last mounted.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint AlgorithmUsageBitmap; //For compression(Not used in e2fsprogs / Linux)

        /// <summary>
        /// 
        /// </summary>
        public readonly byte PreallocatedBlocks; //# of blocks to try to preallocate for ... files? (Not used in e2fsprogs/Linux)

        /// <summary>
        /// 
        /// </summary>
        public readonly byte PreallocatedDirectoryBlocks; //# of blocks to preallocate for directories. (Not used in e2fsprogs/Linux)

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ReservedGdtBlocks; //Number of reserved GDT entries for future filesystem expansion.

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid JournalUuid; //UUID of journal superblock

        /// <summary>
        /// 
        /// </summary>
        public readonly uint JournalInode; //inode number of journal file.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint JournalDevice; //Device number of journal file, if the external journal feature flag is set.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LastOrphanedList; //Start of list of orphaned inodes to delete.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint HashSeed; //HTREE hash seed.

        /// <summary>
        /// 
        /// </summary>
        public readonly DEFAULT_HASH_VERSION DefaultHashVersion; //Default hash algorithm to use for directory hashes. One of:

        /// <summary>
        /// 
        /// </summary>
        public readonly byte JournalBackupType; //If this value is 0 or EXT3_JNL_BACKUP_BLOCKS(1), then the s_jnl_blocks field contains a duplicate copy of the inode's i_block[] array and i_size.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort GroupDescriptorSize; //Size of group descriptors, in bytes, if the 64bit incompat feature flag is set.

        /// <summary>
        /// 
        /// </summary>
        public readonly DEFAULT_MOUNT_OPTIONS DefaultMountOptions; //Default mount options.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FirstMetablockBlockGroup; //First metablock block group, if the meta_bg feature is enabled.

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime MkfsTime; //When the filesystem was created, in seconds since the epoch.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint JournalBlocks; //Backup copy of the journal inode's i_block[] array in the first 15 elements and i_size_high and i_size in the 16th and 17th elements, respectively.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MinimumExtraInodeSize; //All inodes have at least # bytes.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort DesiredInodeSize; //New inodes should reserve # bytes.

        /// <summary>
        /// 
        /// </summary>
        public readonly FLAGS Flags; //Miscellaneous flags.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort RaidStride; //RAID stride. This is the number of logical blocks read from or written to the disk before moving to the next disk.This affects the placement of filesystem metadata, which will hopefully make RAID storage faster.

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MmpInterval; //# seconds to wait in multi-mount prevention (MMP) checking. In theory, MMP is a mechanism to record in the superblock which host and device have mounted the filesystem, in order to prevent multiple mounts. This feature does not seem to be implemented...

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong MmpBlock; //Block # for multi-mount protection data.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RaidStipeWidth; //RAID stripe width.This is the number of logical blocks read from or written to the disk before coming back to the current disk. This is used by the block allocator to try to reduce the number of read-modify - write operations in a RAID5/ 6.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FlexibleBlockGroupSize; //Size of a flexible block group is 2 ^ s_log_groups_per_flex.

        /// <summary>
        /// 
        /// </summary>
        public readonly CHECKSUM_TYPE ChecksumType; //Metadata checksum algorithm type. The only valid value is 1(crc32c).

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong KBytesWritten; //Number of KiB written to this filesystem over its lifetime.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SnapshotInode; //inode number of active snapshot. (Not used in e2fsprogs / Linux.)

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SnapshotId; //Sequential ID of active snapshot. (Not used in e2fsprogs / Linux.)

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong ReservedSnapshotBlockCount; //Number of blocks reserved for active snapshot's future use. (Not used in e2fsprogs/Linux.)

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SnapshotListInode; //inode number of the head of the on - disk snapshot list. (Not used in e2fsprogs / Linux.)

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ErrorCount; //Number of errors seen.

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime FirstErrorTime; //First time an error happened, in seconds since the epoch.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FirstErrorInode; //inode involved in first error.

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong FirstErrorBlock; //Number of block involved of first error.

        /// <summary>
        /// 
        /// </summary>
        public readonly string FirstErrorFunction; //Name of function where the error happened.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FirstErrorLine; //Line number where error happened.

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime LastErrorTime; //Time of most recent error, in seconds since the epoch.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LastErrorInode; //inode involved in most recent error.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LastErrorLine; //Line number where most recent error happened.

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong LastErrorBlock; //Number of block involved in most recent error.

        /// <summary>
        /// 
        /// </summary>
        public readonly string LastErrorFunction; //Name of function where the most recent error happened.

        /// <summary>
        /// 
        /// </summary>
        public readonly string MountOptions; //ASCIIZ string of mount options.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint UserQuotaInode; //Inode number of user quota file.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint GroupQuotaInode; //Inode number of group quota file.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint OverheadBlocks; //Overhead blocks / clusters in fs. (Huh ? This field is always zero, which means that the kernel calculates it dynamically.)

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SuperblockBackupBlockGroup; //Block groups containing superblock backups(if sparse_super2)

        /// <summary>
        /// 
        /// </summary>
        public readonly ENCRYPTION_ALGORITHMS EncryptionAlgorithms; //Encryption algorithms in use.There can be up to four algorithms in use at any time

        /// <summary>
        /// 
        /// </summary>
        public readonly byte EncryptPasswordSalt; //Salt for the string2key algorithm for encryption.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LostAndFoundInode; //Inode number of lost + found

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ProjectQuotaInode; //Inode that tracks project quotas.

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ChecksumSeed; //Checksum seed used for metadata_csum calculations. This value is crc32c(~0, $orig_fs_uuid).

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumenName"></param>
        /// <returns></returns>
        public static Superblock Get(string volumenName)
        {
            return new Superblock(Helper.readDrive(volumenName, 0x400, 0x400));
        }

        #endregion Static Methods
    }
}
