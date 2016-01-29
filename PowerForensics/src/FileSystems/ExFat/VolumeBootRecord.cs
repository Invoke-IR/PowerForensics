using System;
using System.IO;
using System.Text;

namespace PowerForensics.ExFat
{
    #region VolumeBootRecordClass

    public class VolumeBootRecord : PowerForensics.FileSystems.VolumeBootRecord
    {
        #region Properties

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

        internal VolumeBootRecord(byte[] bytes, string signature)
        {
            Signature = signature;
            BytesPerSector = BitConverter.ToUInt16(bytes, 0x0B);
            SectorsPerCluster = bytes[0x0D];
            BytesPerCluster = (uint)BytesPerSector * (uint)SectorsPerCluster;
            ReservedSectors = BitConverter.ToUInt16(bytes, 0x0E);
            TotalFats = bytes[0x10];
            RootDirectoryEntries = BitConverter.ToUInt16(bytes, 0x11);
            #region TotalSectors
            ushort TotalSectors16 = BitConverter.ToUInt16(bytes, 0x13);

            if (TotalSectors16 != 0)
            {
                TotalSectors = TotalSectors16;
            }
            else
            {
                TotalSectors = BitConverter.ToUInt32(bytes, 0x20);
            }
            #endregion TotalSectors
            MediaDescriptor = (MEDIA_DESCRIPTOR)bytes[0x15];
            #region SectorsPerFat
            ushort SectorsPerFat16 = BitConverter.ToUInt16(bytes, 0x16);

            if (SectorsPerFat16 != 0)
            {
                SectorsPerFat = SectorsPerFat16;
            }
            else
            {
                SectorsPerFat = BitConverter.ToUInt32(bytes, 0x24);
            }
            #endregion SectorsPerFat
            SectorsPerTrack = BitConverter.ToUInt16(bytes, 0x18);
            NumberOfHeads = BitConverter.ToUInt16(bytes, 0x1A);
            HiddenSectors = BitConverter.ToUInt32(bytes, 0x1C);
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
            PhysicalDriveNumber = bytes[0x24];
            BootSignature = bytes[0x26];
            VolumeSerialNumber = BitConverter.ToUInt32(bytes, 0x27);
            VolumeLabel = Encoding.ASCII.GetString(bytes, 0x2B, 0x0B);
            FileSystemType = Encoding.ASCII.GetString(bytes, 0x36, 0x08);
            CodeSection = Helper.GetSubArray(bytes, 0x3E, 0x200 - 0x40);
            checkFooter(bytes);
        }

        #endregion Constructors
    }

    #endregion VolumeBootRecordClass
}