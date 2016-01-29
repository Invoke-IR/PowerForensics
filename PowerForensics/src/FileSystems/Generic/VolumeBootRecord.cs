using System;
using System.IO;
using System.Text;
using PowerForensics.Ntfs;

namespace PowerForensics.FileSystems
{
    public class VolumeBootRecord
    {
        #region Enums

        public enum MEDIA_DESCRIPTOR
        {
            FloppyDisk = 0xF0,
            HardDriveDisk = 0xF8
        }

        #endregion Enums

        #region Properties

        public string Signature;
        public MEDIA_DESCRIPTOR MediaDescriptor;
        public ushort BytesPerSector;
        public byte SectorsPerCluster;
        public uint BytesPerCluster;
        public ushort ReservedSectors;
        public ushort SectorsPerTrack;
        public ushort NumberOfHeads;
        public uint HiddenSectors;
        public byte[] CodeSection;

        #endregion Properties

        #region StaticMethods

        private static string getFileSystem(byte[] bytes)
        {
            string Signature = Encoding.ASCII.GetString(bytes, 0x03, 0x08).TrimEnd('\0');

            if (Signature == "NTFS")
            {
                return "Ntfs";
            }
            else if(Signature == "EXFAT")
            {
                return "ExFat";
            }
            else
            {
                return "Fat";
            }
        }

        internal void checkFooter(byte[] bytes)
        {
            if (BitConverter.ToUInt16(bytes, 0x1FE) != 0xAA55)
            {
                throw new Exception("Invalid VolumeBootRecord Footer.");
            }
        }

        #region Get

        public static VolumeBootRecord Get(string volume)
        {
            return VolumeBootRecord.Get(GetBytes(volume));
        }

        public static VolumeBootRecord GetByPath(string path)
        {
            return VolumeBootRecord.Get(GetBytesByPath(path));
        }

        internal static VolumeBootRecord Get(FileStream streamToRead)
        {
            return VolumeBootRecord.Get(GetBytes(streamToRead));
        }

        private static VolumeBootRecord Get(byte[] bytes)
        {
            string signature = getFileSystem(bytes);
            switch (signature)
            {
                case "Ntfs":
                    return new Ntfs.VolumeBootRecord(bytes);
                case "ExFat":
                    return new ExFat.VolumeBootRecord(bytes, signature);
                default:
                    return new Fat.VolumeBootRecord(bytes);
            }
        }

        #endregion Get

        #region GetBytes

        public static byte[] GetBytes(string volume)
        {
            return Helper.readDrive(volume, 0x00, 0x200);
        }

        private static byte[] GetBytes(FileStream streamToRead)
        {
            return Helper.readDrive(streamToRead, 0x00, 0x200);
        }

        public static byte[] GetBytesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return record.GetContent();
        }

        #endregion GetBytes

        #endregion StaticMethods
    }
}
