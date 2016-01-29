using System;
using System.IO;
using System.Text;

namespace PowerForensics.Fat
{
    #region VolumeBootRecordClass

    public class VolumeBootRecord : PowerForensics.FileSystems.VolumeBootRecord
    {
        #region Properties

        public readonly string FatType;
        public readonly byte TotalFats;
        public readonly ushort RootDirectoryEntries;
        public readonly uint SectorsPerFat;
        public readonly uint TotalSectors;
        public readonly ushort MirroringFlags;
        public readonly ushort Version;
        public readonly uint RootDirectoryCluster;
        public readonly ushort LocationOfFsInformationSector;
        public readonly ushort LocationOfBackupSectors;
        public readonly string BootFileName;
        public readonly byte PhysicalDriveNumber;
        public readonly byte Flags;
        public readonly byte BootSignature;
        public readonly uint VolumeSerialNumber;
        public readonly string VolumeLabel;
        public readonly string FileSystemType;
        
        #endregion Properties

        #region Constructors

        internal VolumeBootRecord(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x03, 0x08);
            BytesPerSector = BitConverter.ToUInt16(bytes, 0x0B);
            SectorsPerCluster = bytes[0x0D];
            BytesPerCluster = (uint)BytesPerSector * (uint)SectorsPerCluster;
            ReservedSectors = BitConverter.ToUInt16(bytes, 0x0E);
            TotalFats = bytes[0x10];
            RootDirectoryEntries = BitConverter.ToUInt16(bytes, 0x11);
            TotalSectors = getTotalSectors(bytes);
            MediaDescriptor = (MEDIA_DESCRIPTOR)bytes[0x15];
            SectorsPerFat = getSectorsPerFat(bytes);
            FatType = getFatType(BytesPerSector, RootDirectoryEntries, ReservedSectors, TotalFats, TotalSectors, SectorsPerFat, SectorsPerCluster);
            SectorsPerTrack = BitConverter.ToUInt16(bytes, 0x18);
            NumberOfHeads = BitConverter.ToUInt16(bytes, 0x1A);
            HiddenSectors = BitConverter.ToUInt32(bytes, 0x1C);

            if(FatType == "FAT32")
            {
                MirroringFlags = BitConverter.ToUInt16(bytes, 0x28);
                Version = BitConverter.ToUInt16(bytes, 0x2A);
                RootDirectoryCluster = BitConverter.ToUInt32(bytes, 0x2C);
                LocationOfFsInformationSector = BitConverter.ToUInt16(bytes, 0x30);
                LocationOfBackupSectors = BitConverter.ToUInt16(bytes, 0x32);
                BootFileName = Encoding.ASCII.GetString(bytes, 0x34, 0x0C);
                PhysicalDriveNumber = bytes[0x40];
                Flags = bytes[0x41];
                BootSignature = bytes[0x42];
                VolumeSerialNumber = BitConverter.ToUInt32(bytes, 0x43);
                VolumeLabel = Encoding.ASCII.GetString(bytes, 0x47, 0x0B);
                FileSystemType = Encoding.ASCII.GetString(bytes, 0x52, 0x08);
                CodeSection = Helper.GetSubArray(bytes, 0x5A, 0x200 - 0x5C);
            }
            else
            {
                PhysicalDriveNumber = bytes[0x24];
                BootSignature = bytes[0x26];
                VolumeSerialNumber = BitConverter.ToUInt32(bytes, 0x27);
                VolumeLabel = Encoding.ASCII.GetString(bytes, 0x2B, 0x0B);
                FileSystemType = Encoding.ASCII.GetString(bytes, 0x36, 0x08);
                CodeSection = Helper.GetSubArray(bytes, 0x3E, 0x200 - 0x40);
            }

            checkFooter(bytes);
        }

        #endregion Constructors

        #region StaticMethods

        private static string getFatType(ushort BPB_BytsPerSec, ushort BPB_RootEntCnt, ushort BPB_ResvdSecCnt, ushort BPB_NumFATs, uint TotSec, uint FATSz, ushort BPB_SecPerClus)
        {
            uint RootDirSectors = (uint)((BPB_RootEntCnt * 32) + (BPB_BytsPerSec - 1)) / BPB_BytsPerSec;
            uint DataSec = TotSec - (BPB_ResvdSecCnt + (BPB_NumFATs * FATSz) + RootDirSectors);
            uint CountOfClusters = DataSec / BPB_SecPerClus;

            if (CountOfClusters < 4085) 
            {
                return "FAT12";
            } 
            else if (CountOfClusters < 65525)
            {
                return "FAT16";
            }
            else
            {
                return "FAT32";
            }
        }
        
        private static uint getTotalSectors(byte[] bytes)
        {
            ushort TotalSectors16 = BitConverter.ToUInt16(bytes, 0x13);

            if (TotalSectors16 != 0)
            {
                return TotalSectors16;
            }
            else
            {
                return BitConverter.ToUInt32(bytes, 0x20);
            }
        }

        private static uint getSectorsPerFat(byte[] bytes)
        {
            ushort SectorsPerFat16 = BitConverter.ToUInt16(bytes, 0x16);

            if (SectorsPerFat16 != 0)
            {
                return SectorsPerFat16;
            }
            else
            {
                return BitConverter.ToUInt32(bytes, 0x24);
            }
        }

        #endregion StaticMethods
    }

    #endregion VolumeBootRecordClass
}