using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics
{

    class MBR
    {

        #region MBRSignatures

        private const string WINDOWS_6 = "A36C5E4F47E84449FF07ED3517B43A31";
        private const string NYANCAT = "B40C0E49689A0ABD2A51379FED1800F3";
        private const string STONEDv2 = "72B8CE41AF0DE751C946802B3ED844B4";
        private const string STONEDv2_TRUE_CRYPT = "5C7DE5F58B276CBE84B8B7E25F08318E";

        #endregion MBRSignatures

        private const byte BOOTABLE = 0x80;
        private const byte NON_BOOTABLE = 0x00;

        private enum PARTITION_TYPE
        {
            EMPTY = 0x00,
            FAT12 = 0x01,
            FAT16_16MB_32MB = 0x04,
            MS_EXTENDED = 0x05,
            FAT16_32MB_2GB = 0x06,
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

        #region Properties

        public string MBRSignature;
        //public bool Clean;
        public string DiskSignature;
        public byte[] MBRCodeArea;
        private byte[] NotUsed;
        internal List<PARTITION_TABLE_ENTRY> partitionList = new List<PARTITION_TABLE_ENTRY>();
        private byte _AA;
        private byte _55;

        #endregion Properties

        #region Constructor

        internal MBR(byte[] MBRBytes, string mbrCodeSig)
        {
            MBRCodeArea = MBRBytes.Take(440).ToArray();
            DiskSignature = BitConverter.ToString(MBRBytes.Skip(440).Take(4).ToArray()).Replace("-", "");
            MBRSignature = mbrCodeSig;
            NotUsed = MBRBytes.Skip(444).Take(2).ToArray();
            partitionList.Add(new PARTITION_TABLE_ENTRY(MBRBytes.Skip(446).Take(16).ToArray()));
            partitionList.Add(new PARTITION_TABLE_ENTRY(MBRBytes.Skip(462).Take(16).ToArray()));
            partitionList.Add(new PARTITION_TABLE_ENTRY(MBRBytes.Skip(478).Take(16).ToArray()));
            partitionList.Add(new PARTITION_TABLE_ENTRY(MBRBytes.Skip(494).Take(16).ToArray()));
            _AA = MBRBytes[510];
            _55 = MBRBytes[511];
        }

        #endregion Constructor

        internal struct PARTITION_TABLE_ENTRY
        {
            public bool Bootable;
            internal byte startingHeadNumber;
            internal byte startingSectorNumber;
            internal byte startingCylinderHigh2;
            internal byte startingCylinderLow8;
            public string SystemID;
            internal byte endingHeadNumber;
            internal byte endingSectorNumber;
            internal byte endingCylinderHigh2;
            internal byte endingCylinderHigh8;
            internal uint RelativeSector;
            internal uint TotalSectors;
            public uint StartSector;
            public uint EndSector;

            internal PARTITION_TABLE_ENTRY(byte[] bytes)
            {
                Bootable = (bytes[0] == BOOTABLE);
                startingHeadNumber = bytes[1];
                startingSectorNumber = bytes[2];// &0xFC;
                startingCylinderHigh2 = bytes[2];// &0x03;
                startingCylinderLow8 = bytes[3];
                SystemID = Enum.GetName(typeof(PARTITION_TYPE), bytes[4]);
                endingHeadNumber = bytes[5];
                endingSectorNumber = bytes[6];// &0xFC;
                endingCylinderHigh2 = bytes[6];// &0x03;
                endingCylinderHigh8 = bytes[7];
                RelativeSector = BitConverter.ToUInt32(bytes, 8);
                TotalSectors = BitConverter.ToUInt32(bytes, 12);
                StartSector = RelativeSector;
                EndSector = StartSector + TotalSectors;
            }

        }

        internal static MBR Get(FileStream streamToRead)
        {
            // Read MBR (first 512 bytes) from disk
            byte[] MBRBytes = NativeMethods.readDrive(streamToRead, 0, 512);
            
            // Instantiate a byte array to hold 440 bytes (size of MBR Boot Code)
            byte[] mbrCode = new byte[440];
            
            // Copy MBR sub-array into mbrCode
            Array.Copy(MBRBytes, 0, mbrCode, 0, mbrCode.Length);
            
            // Determine boot code signature
            string MD5SignatureHash = Hash.Get(mbrCode, mbrCode.Length, "MD5");

            // Check MBR Code Section against a list of known signatures
            #region MD5Signature

            string MD5Signature = null;

            switch(MD5SignatureHash)
            {
                case WINDOWS_6:
                    MD5Signature = "WINDOWS_6";
                    break;
                case NYANCAT:
                    MD5Signature = "NYANCAT";
                    break;
                case STONEDv2:
                    MD5Signature = "STONEDv2";
                    break;
                case STONEDv2_TRUE_CRYPT:
                    MD5Signature = "STONEDv2_TRUE_CRYPT";
                    break;
                default:
                    MD5Signature = "UNKNOWN";
                    break;
            }

            #endregion MD5Signature

            // Return an MBR Object
            return new MBR(MBRBytes, MD5Signature);

        }

    }

}
