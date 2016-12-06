using System;

namespace PowerForensics.FileSystems.Ext
{
    /// <summary>
    /// 
    /// </summary>
    public class BlockGroupDescriptor
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            EXT4_BG_INODE_UNINIT = 0x1,
            
            /// <summary>
            /// 
            /// </summary>
            EXT4_BG_BLOCK_UNINIT = 0x2,

            /// <summary>
            /// 
            /// </summary>
            EXT4_BG_INODE_ZEROED = 0x4
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong BlockBitmap;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong InodeBitmap;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong InodeTable;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FreeBlockCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FreeInodeCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint DirectoryCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong SnapshotExclusionBitmap;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint BlockBitmapChecksum;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint InodeBitmapChecksum;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint UnusedInodeCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly FLAGS Flags;

        /// <summary>
        /// 
        /// </summary>
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

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static BlockGroupDescriptor Get(byte[] bytes)
        {
            return new BlockGroupDescriptor(bytes, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="blockGroup"></param>
        /// <returns></returns>
        public static BlockGroupDescriptor Get(string volumeName, uint blockGroup)
        {
            Superblock superblock = Superblock.Get(volumeName);
            return new BlockGroupDescriptor(Utilities.DD.Get(volumeName, (superblock.FirstDataBlock + (blockGroup * superblock.BlocksPerGroup)) * superblock.BlockSize, superblock.BlockSize, 1), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static BlockGroupDescriptor GetInstances(string volumeName)
        {
            Superblock superblock = Superblock.Get(volumeName);
            //superblock.group
            return null;
        }

        #endregion Static Methods
    }
}
