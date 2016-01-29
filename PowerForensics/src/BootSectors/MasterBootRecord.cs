using System;
using System.IO;
using System.Collections.Generic;
using PowerForensics.Utilities;
using System.Management.Automation;

namespace PowerForensics
{
    #region MasterBootRecordClass

    class MasterBootRecord
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

        private const int PARTITION_ENTRY_SIZE = 16;

        #endregion Constants

        #region Properties

        public readonly string MbrSignature;
        public readonly string DiskSignature;
        public readonly byte[] CodeSection;
        public readonly PartitionEntry[] PartitionTable;

        #endregion Properties

        #region Constructor

        internal MasterBootRecord(byte[] bytes)
        {
            // Copy MBR sub-array into CodeSection
            CodeSection = Helper.GetSubArray(bytes, 0x00, 0x1B8);

            // Check MBR Code Section against a list of known signatures
            #region MD5Signature

            /*switch (Hash.Get(CodeSection, CodeSection.Length, "MD5"))
            {
                case WINDOWS5_X:
                    MD5Signature = "Windows 5.X";
                    break;
                case WINDOWS6_0:
                    MD5Signature = "Windows 6.0";
                    break;
                case WINDOWS6_1:
                    MD5Signature = "Windows 6.1+";
                    break;
                case GRUB:
                    MD5Signature = "GRUB";
                    break;
                case NYANCAT:
                    MD5Signature = "BOOTKIT Nyan Cat";
                    break;
                case STONEDv2:
                    MD5Signature = "BOOTKIT Stonedv2";
                    break;
                case STONEDv2_TRUE_CRYPT:
                    MD5Signature = "BOOTKIT Stonedv2";
                    break;
                default:
                    MD5Signature = "UNKNOWN";
                    break;
            }*/

            #endregion MD5Signature

            // Instantiate a blank Partition List
            List<PartitionEntry> partitionList = new List<PartitionEntry>();

            // Set object properties
            DiskSignature = BitConverter.ToString(Helper.GetSubArray(bytes, 0x1B8, 0x04)).Replace("-", "");
            //MbrSignature = MD5Signature;

            for (int i = 0x1BE; i <= 0x1DE; i += PARTITION_ENTRY_SIZE)
            {
                PartitionEntry entry = new PartitionEntry(Helper.GetSubArray(bytes, i, PARTITION_ENTRY_SIZE));

                if(entry.SystemId == "MS_EXTENDED")
                {
                    partitionList.AddRange(GetExtended(entry));
                }
                else if (entry.SystemId != "EMPTY")
                {
                    partitionList.Add(entry);
                }
                
            }

            PartitionTable = partitionList.ToArray();
        }

        #endregion Constructor

        #region StaticMethods

        public static byte[] GetBytes(string drivePath)
        {
            return Helper.readDrive(drivePath, 0x00, 0x200);
        }

        public static MasterBootRecord Get(string drivePath)
        {
            // Read Master Boot Record (first 512 bytes) from disk
            return new MasterBootRecord(MasterBootRecord.GetBytes(drivePath));
        }

        private static List<PartitionEntry> GetExtended(PartitionEntry entry)
        {
            List<PartitionEntry> pList = new List<PartitionEntry>();
            return null;
        }

        private static List<PartitionEntry> GetExtended(FileStream streamToRead, uint startSector)
        {
            List<PartitionEntry> pList = new List<PartitionEntry>();

            ulong offset = 0x200 * (ulong)startSector;

            byte[] extendedMBR = Helper.readDrive(streamToRead, offset, 0x200);

            pList.Add(new PartitionEntry(Helper.GetSubArray(extendedMBR, 0x1BE, PARTITION_ENTRY_SIZE), startSector));

            PartitionEntry secondEntry = new PartitionEntry(Helper.GetSubArray(extendedMBR, 0x1CE, PARTITION_ENTRY_SIZE), startSector);
            pList.Add(secondEntry);

            if (secondEntry.SystemId == "MS_EXTENDED")
            {
                pList.AddRange(GetExtended(streamToRead, secondEntry.StartSector));
            }

            return pList;
        }

        #endregion StaticMethods

        #region InstanceMethods

        public PartitionEntry[] GetPartitionTable()
        {
            return this.PartitionTable;
        }

        #endregion InstanceMethods
    }

    #endregion MasterBootRecordClass

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

        internal PartitionEntry(byte[] bytes)
        {
            Bootable = (bytes[0] == BOOTABLE);
            startingHeadNumber = bytes[1];
            startingSectorNumber = bytes[2];// &0xFC;
            startingCylinderHigh2 = bytes[2];// &0x03;
            startingCylinderLow8 = bytes[3];
            SystemId = Enum.GetName(typeof(PARTITION_TYPE), bytes[4]);
            endingHeadNumber = bytes[5];
            endingSectorNumber = bytes[6];// &0xFC;
            endingCylinderHigh2 = bytes[6];// &0x03;
            endingCylinderHigh8 = bytes[7];
            RelativeSector = BitConverter.ToUInt32(bytes, 8);
            TotalSectors = BitConverter.ToUInt32(bytes, 12);
            StartSector = RelativeSector;
            EndSector = StartSector + TotalSectors - 1;
        }

        internal PartitionEntry(byte[] bytes, uint extendedStartSector)
        {
            Bootable = (bytes[0] == BOOTABLE);
            startingHeadNumber = bytes[1];
            startingSectorNumber = bytes[2];// &0xFC;
            startingCylinderHigh2 = bytes[2];// &0x03;
            startingCylinderLow8 = bytes[3];
            SystemId = Enum.GetName(typeof(PARTITION_TYPE), bytes[4]);
            endingHeadNumber = bytes[5];
            endingSectorNumber = bytes[6];// &0xFC;
            endingCylinderHigh2 = bytes[6];// &0x03;
            endingCylinderHigh8 = bytes[7];
            RelativeSector = BitConverter.ToUInt32(bytes, 8);
            TotalSectors = BitConverter.ToUInt32(bytes, 12);
            if (SystemId != "EMPTY")
            {
                StartSector = extendedStartSector + RelativeSector;
            }
            else
            {
                StartSector = RelativeSector;
            }
            EndSector = StartSector + TotalSectors;
        }

        #endregion Constructors
    }

    #endregion PartitionEntryClass
}
