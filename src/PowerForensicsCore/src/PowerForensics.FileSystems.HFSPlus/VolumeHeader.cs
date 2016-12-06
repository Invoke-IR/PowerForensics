using System;
using System.Text;

namespace PowerForensics.FileSystems.HFSPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class VolumeHeader
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum HFS_VERSION
        {
            /// <summary>
            /// 
            /// </summary>
            HFSPLUS = 4,

            /// <summary>
            /// 
            /// </summary>
            HFSX = 5
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Signature;

        /// <summary>
        /// 
        /// </summary>
        public readonly HFS_VERSION Version;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly uint Attributes;

        /// <summary>
        /// 
        /// </summary>
        public readonly string LastMountedVersion;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint JournalInfoBlock;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime CreateData;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifyDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BackupDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime CheckedDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FileCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FolderCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint BlockSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint TotalBlocks;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FreeBlocks;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint NextAllocation;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RsrcClumpSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint DataClumpSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] NextCatalogId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint WriteCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong EncodingBitmap;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray0;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray1;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray2;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray3;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray4;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray5;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray6;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] FinderInfoArray7;

        /// <summary>
        /// 
        /// </summary>
        public readonly ForkData AllocationFile;

        /// <summary>
        /// 
        /// </summary>
        public readonly ForkData ExtentsOverflowFile;

        /// <summary>
        /// 
        /// </summary>
        public readonly ForkData CatalogFile;

        /// <summary>
        /// 
        /// </summary>
        public readonly ForkData AttributesFile;

        /// <summary>
        /// 
        /// </summary>
        public readonly ForkData StartupFile;

        #endregion Properties

        #region Constructors

        private VolumeHeader(byte[] bytes, string volumeName)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x02);
            Version = (HFS_VERSION)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, 0x02));
            Attributes = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x04));
            LastMountedVersion = Encoding.ASCII.GetString(bytes, 0x08, 0x04);
            JournalInfoBlock = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x0C));
            CreateData = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x10)));
            ModifyDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x14)));
            BackupDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x18)));
            CheckedDate = Helper.FromOSXTime(Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x1C)));
            FileCount = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x20));
            FolderCount = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x24));
            BlockSize = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x28));
            TotalBlocks = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x2C));
            FreeBlocks = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x30));
            NextAllocation = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x34));
            RsrcClumpSize = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x38));
            DataClumpSize = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x3C));
            NextCatalogId = Helper.GetSubArray(bytes, 0x40, 0x04);
            WriteCount = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, 0x44));
            EncodingBitmap = Helper.SwapEndianness(BitConverter.ToUInt64(bytes, 0x48));
            FinderInfoArray0 = Helper.GetSubArray(bytes, 0x50, 0x04);
            FinderInfoArray1 = Helper.GetSubArray(bytes, 0x54, 0x04);
            FinderInfoArray2 = Helper.GetSubArray(bytes, 0x58, 0x04);
            FinderInfoArray3 = Helper.GetSubArray(bytes, 0x5C, 0x04);
            FinderInfoArray4 = Helper.GetSubArray(bytes, 0x60, 0x04);
            FinderInfoArray5 = Helper.GetSubArray(bytes, 0x64, 0x04);
            FinderInfoArray6 = Helper.GetSubArray(bytes, 0x68, 0x04);
            FinderInfoArray7 = Helper.GetSubArray(bytes, 0x6C, 0x04);
            AllocationFile = ForkData.Get(bytes, 0x70, volumeName, BlockSize);
            ExtentsOverflowFile = ForkData.Get(bytes, 0xC0, volumeName, BlockSize);
            CatalogFile = ForkData.Get(bytes, 0x110, volumeName, BlockSize);
            AttributesFile = ForkData.Get(bytes, 0x160, volumeName, BlockSize);
            StartupFile = ForkData.Get(bytes, 0x1B0, volumeName, BlockSize);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static VolumeHeader Get(string volumeName)
        {
            byte[] bytes = Helper.readDrive(volumeName, 0x400, 0x200);
            return new VolumeHeader(bytes, volumeName);
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetAllocationFileBytes()
        {
            return AllocationFile.GetContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetExtentsOverflowFileBytes()
        {
            return ExtentsOverflowFile.GetContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetCatalogFileBytes()
        {
            return CatalogFile.GetContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetAttributesFileBytes()
        {
            return AttributesFile.GetContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetStartupFileBytes()
        {
            return StartupFile.GetContent();
        }

        #endregion Instance Methods
    }
}