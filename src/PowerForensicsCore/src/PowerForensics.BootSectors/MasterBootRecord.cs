using System;
using System.IO;
using System.Collections.Generic;
using PowerForensics.Utilities;

namespace PowerForensics.BootSectors
{
    /// <summary>
    /// 
    /// </summary>
    public class MasterBootRecord
    {
        #region MbrSignatures

        private const string WINDOWS5_X = "8F558EB6672622401DA993E1E865C861";
        private const string WINDOWS6_0 = "5C616939100B85E558DA92B899A0FC36";
        private const string WINDOWS6_1 = "A36C5E4F47E84449FF07ED3517B43A31";
        //private const string LILO = "";
        private const string GRUB = "A6C7E63CA46F1CB2307E0F10AD897BDE";
        private const string NYANCAT = "B40C0E49689A0ABD2A51379FED1800F3";
        private const string STONEDv2 = "72B8CE41AF0DE751C946802B3ED844B4";
        private const string STONEDv2_TRUE_CRYPT = "5C7DE5F58B276CBE84B8B7E25F08318E";

        #endregion MbrSignatures

        #region Constants

        private const int PARTITION_TABLE_OFFSET = 0x1BE;
        private const int PARTITION_ENTRY_SIZE = 0x10;

        #endregion Constants

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string DiskSignature;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] CodeSection;

        /// <summary>
        /// 
        /// </summary>
        public readonly string MbrSignature;

        /// <summary>
        /// 
        /// </summary>
        public readonly PartitionEntry[] PartitionTable;

        #endregion Properties

        #region Constructor

        private MasterBootRecord(byte[] bytes, string drivePath)
        {
            DiskSignature = BitConverter.ToString(Helper.GetSubArray(bytes, 0x1B8, 0x04)).Replace("-", "");
            CodeSection = Helper.GetSubArray(bytes, 0x00, 0x1B8);
            MbrSignature = GetMbrSignature(CodeSection);
            PartitionTable = GetPartitions(bytes, 0, drivePath);
        }

        #endregion Constructor

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drivePath"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string drivePath)
        {
            return Helper.readSector(drivePath, 0x00, 0x01);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drivePath"></param>
        /// <returns></returns>
        public static MasterBootRecord Get(string drivePath)
        {
            // Read Master Boot Record (first 512 bytes) from disk
            return new MasterBootRecord(MasterBootRecord.GetBytes(drivePath), drivePath);
        }

        #endregion Static Methods

        #region Private Methods

        private static string GetMbrSignature(byte[] bytes)
        {
            /*switch (Hash.Get(bytes, bytes.Length, "MD5"))
            {
                case WINDOWS5_X:
                    return "Windows 5.X";
                case WINDOWS6_0:
                    return "Windows 6.0";
                case WINDOWS6_1:
                    return "Windows 6.1+";
                case GRUB:
                    return "GRUB";
                case NYANCAT:
                    return "BOOTKIT Nyan Cat";
                case STONEDv2:
                    return "BOOTKIT Stonedv2";
                case STONEDv2_TRUE_CRYPT:
                    return "BOOTKIT Stonedv2";
                default:
                    return "UNKNOWN";
            }*/

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startSector"></param>
        /// <param name="drivePath"></param>
        /// <returns></returns>
        private static PartitionEntry[] GetPartitions(byte[] bytes, uint startSector, string drivePath)
        {
            // Instantiate a blank Partition List
            List<PartitionEntry> partitionList = new List<PartitionEntry>();

            for (int i = PARTITION_TABLE_OFFSET; i <= 0x1EE; i += PARTITION_ENTRY_SIZE)
            {
                PartitionEntry entry = new PartitionEntry(bytes, startSector, i);

                try
                {
                    if (entry.SystemId.Contains("EXTENDED"))
                    {
                        partitionList.AddRange(GetExtended(entry, drivePath));
                    }
                    else if (entry.SystemId != "EMPTY")
                    {
                        partitionList.Add(entry);
                    }
                }
                catch
                {

                }
            }

            return partitionList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="drivePath"></param>
        /// <returns></returns>
        private static PartitionEntry[] GetExtended(PartitionEntry entry, string drivePath)
        {
            List<PartitionEntry> pList = new List<PartitionEntry>();

            byte[] extendedBytes = Helper.readSector(drivePath, entry.StartSector, 0x01);
            pList.AddRange(GetPartitions(extendedBytes, entry.StartSector, drivePath));

            return pList.ToArray();
        }

        #endregion Private Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartitionEntry[] GetPartitionTable()
        {
            return this.PartitionTable;
        }

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class PartitionEntry
    {
        #region Constants

        private const byte BOOTABLE = 0x80;
        private const byte NON_BOOTABLE = 0x00;

        #endregion Constants

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

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Bootable;

        internal readonly byte startingHeadNumber;
        internal readonly byte startingSectorNumber;
        internal readonly byte startingCylinderHigh2;
        internal readonly byte startingCylinderLow8;

        /// <summary>
        /// 
        /// </summary>
        public readonly string SystemId;

        internal readonly byte endingHeadNumber;
        internal readonly byte endingSectorNumber;
        internal readonly byte endingCylinderHigh2;
        internal readonly byte endingCylinderHigh8;
        internal readonly uint RelativeSector;
        internal readonly uint TotalSectors;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint StartSector;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint EndSector;

        #endregion Properties

        #region Constructors

        internal PartitionEntry(byte[] bytes, uint startSector, int offset)
        {
            Bootable = (bytes[0 + offset] == BOOTABLE);
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
}