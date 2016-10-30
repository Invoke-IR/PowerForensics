/*using System;

namespace PowerForensics.Ext
{
    public class Superblock
    {
        #region Enums

        [Flags]
        public enum S_STATE
        {
            CLEANLY_UNMOUNTED = 0x01,
            ERRORS_DETECTED = 0x02,
            ORPHANS_BEING_RECOVERED = 0x04
        }

        public enum S_ERRORS
        {

        }

        public enum S_CREATOR_OS
        {

        }

        public enum S_REV_LEVEL
        {

        }

        [Flags]
        public enum S_FEATURE_COMPAT
        {

        }

        [Flags]
        public enum S_FEATURE_INCOMPAT
        {
            INCOMPAT_COMPRESSION = 0x00001,
            INCOMPAT_FILETYPE = 0x00002,
            INCOMPAT_RECOVER = 0x00004,
            INCOMPAT_JOURNAL_DEV = 0x00008,
            INCOMPAT_META_BG = 0x00010,
            INCOMPAT_EXTENTS = 0x00040,
            INCOMPAT_64BIT = 0x00080,
            INCOMPAT_MMP = 0x00100,
            INCOMPAT_FLEX_BG = 0x00200,
            INCOMPAT_EA_INODE = 0x00400,
            INCOMPAT_DIRDATA = 0x01000,
            INCOMPAT_CSUM_SEED = 0x02000,
            INCOMPAT_LARGEDIR = 0x04000,
            INCOMPAT_INLINE_DATA = 0x08000,
            INCOMPAT_ENCRYPT = 0x10000
        }

        [Flags]
        public enum S_FEATURE_RO_COMPAT
        {
            RO_COMPAT_SPARSE_SUPER = 0x0001,
            RO_COMPAT_LARGE_FILE = 0x0002,
            RO_COMPAT_BTREE_DIR = 0x0004,
            RO_COMPAT_HUGE_FILE = 0x0008,
            RO_COMPAT_GDT_CSUM = 0x0010,
            RO_COMPAT_DIR_NLINK = 0x0020,
            RO_COMPAT_EXTRA_ISIZE = 0x0040,
            RO_COMPAT_HAS_SNAPSHOT = 0x0080,
            RO_COMPAT_QUOTA = 0x0100,
            RO_COMPAT_BIGALLOC = 0x0200,
            RO_COMPAT_METADATA_CSUM = 0x0400,
            RO_COMPAT_REPLICA = 0x0800,
            RO_COMPAT_READONLY = 0x1000,
            RO_COMPAT_PROJECT = 0x2000
        }

        public enum S_DEF_HASH_VERSION
        {
            LEGACY = 0x0,
            HALF_MD4 = 0x1,
            TEA = 0x2,
            UNSIGNED_LEGACY = 0x3,
            UNSIGNED_HALF_MD4 = 0x4,
            UNSIGNED_TEA = 0x5
        }

        [Flags]
        public enum S_DEFAULT_MOUNT_OPTS
        {
            EXT4_DEFM_DEBUG = 0x0001,
            EXT4_DEFM_BSDGROUPS = 0x0002,
            EXT4_DEFM_XATTR_USER = 0x0004,
            EXT4_DEFM_ACL = 0x0008,
            EXT4_DEFM_UID16 = 0x0010,
            EXT4_DEFM_JMODE_DATA = 0x0020,
            EXT4_DEFM_JMODE_ORDERED = 0x0040,
            EXT4_DEFM_JMODE_WBACK = 0x0080,
            EXT4_DEFM_NOBARRIER = 0x0100,
            EXT4_DEFM_BLOCK_VALIDITY = 0x0200,
            EXT4_DEFM_DISCARD = 0x0400,
            EXT4_DEFM_NODELALLOC = 0x0800
        }

        [Flags]
        public enum S_FLAGS
        {   
            SIGNED_DIRECTORY_HASH = 0x01,
            UNSIGNED_DIRECTORY_HASH = 0x02,
            TEST_DEV_CODE = 0x0004
        }

        public enum S_ENCRYPT_ALGOS
        {
            ENCRYPTION_MODE_INVALID = 0x00,
            ENCRYPTION_MODE_AES_256_XTS = 0x01,
            ENCRYPTION_MODE_AES_256_GCM = 0x02,
            ENCRYPTION_MODE_AES_256_CBC = 0x03
        }

        #endregion Enums

        #region Properties

        public readonly uint TotalInodeCount;
        public readonly uint TotalBlockCountLo;
        public readonly uint ReservedBlockCountLo;
        public readonly uint FreeBlockCountLo;
        public readonly uint FreeInodeCount;
        public readonly uint FirstDataBlock;
        public readonly uint LogBlockSize;
        public readonly uint LogClusterSize;
        public readonly uint BlocksPerGroup;
        public readonly uint ClustersPerGroup;
        public readonly uint InodesPerGroup;
        public readonly DateTime MountTime;
        public readonly DateTime WriteTime;
        public readonly ushort MountCount;
        public readonly ushort MaxMountCount;
        public readonly ushort Magic;
        public readonly S_STATE State;
        public readonly S_ERRORS Errors;
        public readonly ushort MinorRevisionLevel;
        public readonly uint LastCheck;
        public readonly uint CheckInterval;
        public readonly uint CreatorOs;
        public readonly uint RevisionLevel;
        public readonly ushort ReservedBlocksDefaultUid;
        public readonly ushort ReservedBlocksDefaultGid;
        public readonly uint FirstInode;
        public readonly ushort InodeSize;
        public readonly ushort BlockGroupNumber;
        public readonly S_FEATURE_COMPAT FeatureCompat;
        public readonly S_FEATURE_INCOMPAT FeatureIncompat;
        public readonly S_FEATURE_RO_COMPAT FeatureRoCompat;
        public readonly Guid Uuid;
        public readonly string VolumeLabel;
        public readonly string LastMountedDirectory;
        public readonly uint AlgorithmUsageBitmap;
        public readonly byte PreallocatedBlockCount;
        public readonly byte PreallocatedDirectoryBlockCount;
        public readonly ushort ReservedGdtBlocks;
        public readonly Guid JournalUuid;
        public readonly uint JournalInode;
        public readonly uint JournalDevice;
        public readonly uint LastOrphanInode;
        public readonly uint[] HashSeed;
        public readonly S_DEF_HASH_VERSION HashVersion;
        public readonly byte JournalBackupType;
        public readonly ushort DescriptorSize;
        public readonly S_DEFAULT_MOUNT_OPTS DefaultMountOptions;
        public readonly uint FirstMetaBlockGroup;
        public readonly uint FsCreatTime;
        public readonly uint[] JournalBlocks;
        public readonly uint BlockCountHi;
        public readonly uint ReservedBlockCountHi;
        public readonly uint FreeBlockCountHi;
        public readonly ushort MinimumInodeSize;
        public readonly ushort WantInodeSize;
        public readonly S_FLAGS Flags;
        public readonly ushort RaidStride;
        public readonly ushort MmpInterval;
        public readonly ulong MmpBlock;
        public readonly uint RaidStripeWidth;
        public readonly byte FlexibleBlockGroupSize;
        public readonly byte ChecksumType;
        public readonly ulong kBytesWritten;
        public readonly uint SnapshotInode;
        public readonly uint SnapshotId;
        public readonly ulong SnapshotReservedBlockCount;
        public readonly uint SnapshotListInode;
        public readonly uint ErrorCount;
        public readonly uint FirstErrorTime;
        public readonly uint FirstErrorInode;
        public readonly ulong FirstErrorBlock;
        public readonly string FirstErrorFunction;
        public readonly uint FirstErrorLine;
        public readonly uint LastErrorTime;
        public readonly uint LastErrorInode;
        public readonly uint LastErrorLine;
        public readonly ulong LastErrorBlock;
        public readonly string LastErrorFunction;
        public readonly string MountOptions;
        public readonly uint UserQuotaInode;
        public readonly uint GroupQuotaInode;
        public readonly uint OverheadBlocks;
        public readonly uint[] BackupBlockGroups;
        public readonly S_ENCRYPT_ALGOS EncryptionAlgorithm;
        public readonly string EncryptedPasswordSalt;
        public readonly uint LostFoundInode;
        public readonly uint ProjectQuota;
        public readonly uint ChecksumSeed;
        public readonly uint Checksum;

        #endregion Properties

        #region Constructors

        private Superblock(byte[] bytes)
        {
            TotalInodeCount;
            TotalBlockCountLo;
            ReservedBlockCountLo;
            FreeBlockCountLo;
            FreeInodeCount;
            FirstDataBlock;
            LogBlockSize;
            LogClusterSize;
            BlocksPerGroup;
            ClustersPerGroup;
            InodesPerGroup;
            MountTime;
            WriteTime;
            MountCount;
            MaxMountCount;
            Magic;
            State;
            Errors;
            MinorRevisionLevel;
            LastCheck;
            CheckInterval;
            CreatorOs;
            RevisionLevel;
            ReservedBlocksDefaultUid;
            ReservedBlocksDefaultGid;
            FirstInode;
            InodeSize;
            BlockGroupNumber;
            FeatureCompat;
            FeatureIncompat;
            FeatureRoCompat;
            Uuid;
            VolumeLabel;
            LastMountedDirectory;
            AlgorithmUsageBitmap;
            PreallocatedBlockCount;
            PreallocatedDirectoryBlockCount;
            ReservedGdtBlocks;
            JournalUuid;
            JournalInode;
            JournalDevice;
            LastOrphanInode;
            HashSeed;
            HashVersion;
            JournalBackupType;
            DescriptorSize;
            DefaultMountOptions;
            FirstMetaBlockGroup;
            FsCreatTime;
            JournalBlocks;
            BlockCountHi;
            ReservedBlockCountHi;
            FreeBlockCountHi;
            MinimumInodeSize;
            WantInodeSize;
            Flags;
            RaidStride;
            MmpInterval;
            MmpBlock;
            RaidStripeWidth;
            FlexibleBlockGroupSize;
            ChecksumType;
            kBytesWritten;
            SnapshotInode;
            SnapshotId;
            SnapshotReservedBlockCount;
            SnapshotListInode;
            ErrorCount;
            FirstErrorTime;
            FirstErrorInode;
            FirstErrorBlock;
            FirstErrorFunction;
            FirstErrorLine;
            LastErrorTime;
            LastErrorInode;
            LastErrorLine;
            LastErrorBlock;
            LastErrorFunction;
            MountOptions;
            UserQuotaInode;
            GroupQuotaInode;
            OverheadBlocks;
            BackupBlockGroups;
            EncryptionAlgorithm;
            EncryptedPasswordSalt;
            LostFoundInode;
            ProjectQuota;
            ChecksumSeed;
            Checksum;
        }

        #endregion Constructors

        #region StaticMethods

        #endregion StaticMethods
    }
}*/