using System;

namespace PowerForensics.Ext
{
    public class BlockGroupDescriptor
    {
        #region Enums

        [Flags]
        public enum FLAGS
        {
            EXT4_BG_INODE_UNINIT = 0x1,
            EXT4_BG_BLOCK_UNINIT = 0x2,
            EXT4_BG_INODE_ZEROED = 0x4
        }

        #endregion Enums

        #region Properties

        public readonly ulong BlockBitmap;
        public readonly ulong InodeBitmap;
        public readonly ulong InodeTable;
        public readonly uint FreeBlockCount;
        public readonly uint FreeInodeCount;
        public readonly uint DirectoryCount;
        public readonly ulong SnapshotExclusionBitmap;
        public readonly uint BlockBitmapChecksum;
        public readonly uint InodeBitmapChecksum;
        public readonly uint UnusedInodeCount;
        public readonly FLAGS Flags;
        public readonly ushort Checksum;

        #endregion Properties

        #region Constructors

        private BlockGroupDescriptor(byte[] bytes, bool x64)
        {
            if (x64)
            {
                BlockBitmap = BitConverter.ToUInt32(bytes, 0x20) << 32 | BitConverter.ToUInt32(bytes, 0x00);
                InodeBitmap = BitConverter.ToUInt32(bytes, 0x24) << 32 | BitConverter.ToUInt32(bytes, 0x04);
                InodeTable = BitConverter.ToUInt32(bytes, 0x28) << 32 | BitConverter.ToUInt32(bytes, 0x08);
                FreeBlockCount = (uint)BitConverter.ToUInt16(bytes, 0x2C) << 16 | BitConverter.ToUInt16(bytes, 0x0C);
                FreeInodeCount = (uint)BitConverter.ToUInt16(bytes, 0x2E) << 16 | BitConverter.ToUInt16(bytes, 0x0E);
                DirectoryCount = (uint)BitConverter.ToUInt16(bytes, 0x30) << 16 | BitConverter.ToUInt16(bytes, 0x10);
                SnapshotExclusionBitmap = BitConverter.ToUInt32(bytes, 0x34) << 32 | BitConverter.ToUInt32(bytes, 0x14);
                BlockBitmapChecksum = (uint)BitConverter.ToUInt16(bytes, 0x38) << 16 | BitConverter.ToUInt16(bytes, 0x18);
                InodeBitmapChecksum = (uint)BitConverter.ToUInt16(bytes, 0x3A) << 16 | BitConverter.ToUInt16(bytes, 0x1C);
                UnusedInodeCount = (uint)BitConverter.ToUInt16(bytes, 0x32) << 16 | BitConverter.ToUInt16(bytes, 0x1E);
            }
            else
            {
                BlockBitmap = BitConverter.ToUInt32(bytes, 0x00);
                InodeBitmap = BitConverter.ToUInt32(bytes, 0x04);
                InodeTable = BitConverter.ToUInt32(bytes, 0x08);
                FreeBlockCount = BitConverter.ToUInt16(bytes, 0x0C);
                FreeInodeCount = BitConverter.ToUInt16(bytes, 0x0E);
                DirectoryCount = BitConverter.ToUInt16(bytes, 0x10);
                SnapshotExclusionBitmap = BitConverter.ToUInt32(bytes, 0x14);
                BlockBitmapChecksum = BitConverter.ToUInt16(bytes, 0x18);
                InodeBitmapChecksum = BitConverter.ToUInt16(bytes, 0x1C);
                UnusedInodeCount = BitConverter.ToUInt16(bytes, 0x1E);
            }

            Flags = (FLAGS)BitConverter.ToUInt16(bytes, 0x12);
            Checksum = BitConverter.ToUInt16(bytes, 0x1E);
        }

        #endregion Constructors

        #region StaticMethods

        public static BlockGroupDescriptor Get(byte[] bytes)
        {
            return new BlockGroupDescriptor(bytes, false);
        }

        public static BlockGroupDescriptor Get(string volumeName, uint blockGroup)
        {
            Superblock superblock = Superblock.Get(volumeName);
            return new BlockGroupDescriptor(Utilities.DD.Get(volumeName, (superblock.FirstDataBlock + (blockGroup * superblock.BlocksPerGroup)) * superblock.BlockSize, superblock.BlockSize, 1), true);
        }

        public static BlockGroupDescriptor GetInstances(string volumeName)
        {
            Superblock superblock = Superblock.Get(volumeName);
            //superblock.group
            return null;
        }

        #endregion StaticMethods
    }
}
