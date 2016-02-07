using System;
using System.IO;

namespace PowerForensics
{
    #region PartitionEntryClass

    internal class PartitionEntry
    {
        #region Enums

        enum PARTITION_TYPE
        {
            EMPTY = 0x00,
            FAT12 = 0x01,
            FAT16_4 = 0x04,
            MS_EXTENDED = 0x05,
            FAT16_6 = 0x06,
            NTFS = 0x07,
            FAT32_CHS = 0x0b,
            FAT32_LBA = 0x0c,
            FAT16_32MB_2GB_LBA = 0x0e,
            MS_EXTENDED_LBA = 0x0f,
            HIDDEN_FAT12_CHS = 0x11,
            HIDDEN_FAT16_16MB_32MB_CHS = 0x14,
            HIDDEN_FAT16_32MB_2GB_CHS = 0x16,
            HIDDEN_FAT32_CHS = 0x1b,
            HIDDEN_FAT32_LBA = 0x1c,
            HIDDEN_FAT16_32MB_2GB_LBA = 0x1e,
            MS_MBR_DYNAMIC_DISK = 0x42,
            SOLARIS_X86 = 0x82,
            LINUX_SWAP = 0x82,
            LINUX = 0x83,
            HIBERNATION = 0x84,
            LINUX_EXTENDED = 0x85,
            NTFS_VOLUME_SET = 0x86,
            NTFS_VOLUME_SET_1 = 0x87,
            HIBERNATION_1 = 0xa0,
            HIBERNATION_2 = 0xa1,
            FREEBSD = 0xa5,
            OPENBSD = 0xa6,
            MACOSX = 0xa8,
            NETBSD = 0xa9,
            MAC_OSX_BOOT = 0xab,
            BSDI = 0xb7,
            BSDI_SWAP = 0xb8,
            EFI_GPT_DISK = 0xee,
            EFI_SYSTEM_PARTITION = 0xef,
            VMWARE_FILE_SYSTEM = 0xfb,
            VMWARE_SWAP = 0xfc
        }

        #endregion Enums

        #region Constants

        private const byte BOOTABLE = 0x80;
        private const byte NON_BOOTABLE = 0x00;

        #endregion Constants

        #region Properties

        public readonly bool Bootable;
        internal readonly byte startingHeadNumber;
        internal readonly byte startingSectorNumber;
        internal readonly byte startingCylinderHigh2;
        internal readonly byte startingCylinderLow8;
        public readonly string SystemId;
        internal readonly byte endingHeadNumber;
        internal readonly byte endingSectorNumber;
        internal readonly byte endingCylinderHigh2;
        internal readonly byte endingCylinderHigh8;
        internal readonly uint RelativeSector;
        internal readonly uint TotalSectors;
        public readonly uint StartSector;
        public readonly uint EndSector;

        #endregion Properties

        #region Constructors

        internal PartitionEntry(byte[] bytes, uint startSector, int offset)
        {
            Bootable = (bytes[0] == BOOTABLE);
            startingHeadNumber = bytes[0x01 + offset];
            startingSectorNumber = bytes[0x02 + offset];
            startingCylinderHigh2 = bytes[0x02 + offset];
            startingCylinderLow8 = bytes[0x03 + offset];
            SystemId = Enum.GetName(typeof(PARTITION_TYPE), bytes[0x04 + offset]);
            endingHeadNumber = bytes[0x05 + offset];
            endingSectorNumber = bytes[0x06 + offset];
            endingCylinderHigh2 = bytes[0x06 + offset];
            endingCylinderHigh8 = bytes[0x07 + offset];
            RelativeSector = BitConverter.ToUInt32(bytes, 0x08 + offset);
            TotalSectors = BitConverter.ToUInt32(bytes, 0x0C + offset);
            StartSector = RelativeSector + startSector;
            EndSector = StartSector + TotalSectors - 1;
        }

        #endregion Constructors
    }

    #endregion PartitionEntryClass
}
