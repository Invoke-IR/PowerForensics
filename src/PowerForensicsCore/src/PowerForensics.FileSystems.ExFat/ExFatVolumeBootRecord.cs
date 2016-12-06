using System;
using System.IO;
using System.Text;

namespace PowerForensics.FileSystems.ExFat
{
    #region ExFatVolumeBootRecordClass

    /*public class ExFatVolumeBootRecord : Generic.VolumeBootRecord
    {
        #region Enum

        [FlagsAttribute]
        public enum FLAGS
        {
            ActiveFat 0 1 0 – 1st 1 – 2nd 
            VolumeDirty 1 1 0 – Clean 1 - Dirty 
            MediaFailure 2 1 0 – No Failures 1 – Failures Reported 
            CleartoZero 3 1 No Meaning 
            Reserved 4 12   
        }

        #endregion Enum

        #region Properties

        Must Be Zero 11 53
        Partition Offset 64 8
        Volume Length 72 8
        FAT Offset 80 4
        FAT Length 84 4
        Cluster Heap offset 88 4
        Cluster Count 92 4
        Root Directory First Cluster 96 4
        Volume Serial Number 100 4
        File System Revision 104 2
        Volume Flags 106 2
        Bytes Per Sector 108 1
        Sectors Per Cluster 109 1
        Number of FATS 110 1
        Drive Select 111 1
        Percent In Use 112 1 
        Reserved 113 7  
        Boot Code 120 390
        Boot Signature 510 2 0xAA55 
        Excess 512

        public readonly ulong TotalSectors;
        public readonly uint RootDirectoryCluster;
        public readonly ushort LocationOfFsInformationSector;
        public readonly uint BitmapOffset;
        public readonly uint VolumeSerialNumber;
        public readonly ushort Version;
        public readonly FLAGS Flags;
        public readonly byte ActiveFat;
        public readonly byte TotalFats;
        public readonly byte PercentageInUse;

        #endregion Properties

        #region Constructors

        internal ExFatVolumeBootRecord(byte[] bytes)
        {
            if (Encoding.ASCII.GetString(bytes, 0x03, 0x08) == "EXFAT   ")
            {
                PartitionSectorOffset = BitConverter.ToUInt32(bytes, 0x40);
                TotalSectors = BitConverter.ToUInt64(bytes, 0x48);
                SectorsPerFat = BitConverter.ToUInt32(bytes, 0x50);
                FatSize = BitConverter.ToUInt32(bytes, 0x54);
                BitmapOffset = BitConverter.ToUInt32(bytes, 0x58);
                BitCount = BitConverter.ToUInt32(bytes, 0x5C);
                RootDirectoryCluster = BitConverter.ToUInt32(bytes, 0x60);
                VolumeSerialNumber = BitConverter.ToUInt32(bytes, 0x64);
                Version = BitConverter.ToUInt16(bytes, 0x68);
                Flags = (FLAGS)bytes[0x6A];
                ActiveFat = bytes[0x6B];
                BytesPerSector = bytes[0x6C];
                SectorsPerCluster = bytes[0x6D];
                BytesPerCluster = BytesPerSector * SectorsPerCluster;
                TotalFats = bytes[0x6E];
                PercentageInUse = bytes[0x70];
            }
            else
            {
                throw new Exception("Volume is not EXFAT formatted.");
            }
        }

        #endregion Constructors
    }*/

    #endregion ExFatVolumeBootRecordClass
}