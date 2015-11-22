using System;
using System.IO;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region VolumeBootRecordClass

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

        public readonly string Signature;
        public readonly MEDIA_DESCRIPTOR MediaDescriptor;
        public readonly ushort BytesPerSector;
        public readonly uint BytesPerCluster;
        public readonly double BytesPerFileRecord;
        public readonly double BytesPerIndexBlock;
        public readonly ushort ReservedSectors;
        public readonly ushort SectorsPerTrack;
        public readonly ushort NumberOfHeads;
        public readonly uint HiddenSectors;
        public readonly ulong TotalSectors;
        public readonly ulong MFTStartIndex;
        public readonly ulong MFTMirrStartIndex;
        public readonly string VolumeSerialNumber;
        public readonly byte[] CodeSection;

        #endregion Properties

        #region Constructors

        private VolumeBootRecord(byte[] bytes)
        {
            // Get VolumeBootRecord Signature to determine File System Type
            Signature = Encoding.ASCII.GetString(bytes, 0x03, 0x08);

            // Check if NTFS Partition
            if (Signature == "NTFS    ")
            {
                BytesPerSector = BitConverter.ToUInt16(bytes, 11);
                BytesPerCluster = (uint)(bytes[13] * BytesPerSector);
                ReservedSectors = BitConverter.ToUInt16(bytes, 14);
                MediaDescriptor = (MEDIA_DESCRIPTOR)bytes[21];
                SectorsPerTrack = BitConverter.ToUInt16(bytes, 24);
                NumberOfHeads = BitConverter.ToUInt16(bytes, 26);
                HiddenSectors = BitConverter.ToUInt32(bytes, 28);
                TotalSectors = BitConverter.ToUInt64(bytes, 40);
                MFTStartIndex = BitConverter.ToUInt64(bytes, 48);
                MFTMirrStartIndex = BitConverter.ToUInt64(bytes, 56);
                #region BytesPerFileRecord

                sbyte clustersPerFileRecord = (sbyte)bytes[64];
                if (clustersPerFileRecord < 0)
                {
                    BytesPerFileRecord = Math.Pow(2, Math.Abs(clustersPerFileRecord));
                }
                else
                {
                    BytesPerFileRecord = clustersPerFileRecord * BytesPerCluster;
                }

                #endregion BytesPerFileRecord
                #region BytesPerIndexBlock

                sbyte clustersPerIndexBlock = (sbyte)bytes[68];
                if (clustersPerIndexBlock < 0)
                {
                    BytesPerIndexBlock = Math.Pow(2, Math.Abs(clustersPerIndexBlock));
                }
                else
                {
                    BytesPerIndexBlock = clustersPerIndexBlock * BytesPerCluster;
                }

                #endregion BytesPerIndexBlock
                #region VolumeSerialNumber

                byte[] serialNumberBytes = Util.GetSubArray(bytes, 0x48, 0x04);
                Array.Reverse(serialNumberBytes);
                VolumeSerialNumber = BitConverter.ToString(serialNumberBytes).Remove(2, 1).Remove(7, 1);

                #endregion VolumeSerialNumber
                CodeSection = Util.GetSubArray(bytes, 0x50, 0x1AE);
            }
            else
            {
                throw new Exception("Volume is not NTFS formatted.");
            }
        }

        #endregion Constructors

        #region StaticMethods

        #region Get

        public static VolumeBootRecord Get(string volume)
        {
            return new VolumeBootRecord(GetBytes(volume));
        }

        public static VolumeBootRecord GetByPath(string path)
        {
            return new VolumeBootRecord(GetBytesByPath(path));
        }

        internal static VolumeBootRecord Get(FileStream streamToRead)
        {
            return new VolumeBootRecord(GetBytes(streamToRead));
        }

        #endregion Get

        #region GetBytes

        public static byte[] GetBytes(string volume)
        {
            // Get Handle to Hard Drive
            IntPtr hDrive = Util.getHandle(volume);

            // Create a FileStream to read from hDrive
            using (FileStream streamToRead = Util.getFileStream(hDrive))
            {
                return GetBytes(streamToRead);
            }
        }

        public static byte[] GetBytesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return record.GetContent();
        }

        internal static byte[] GetBytes(FileStream streamToRead)
        {
            return Util.readDrive(streamToRead, 0, 512);
        }

        #endregion GetBytes

        #endregion StaticMethods
    }

    #endregion VolumeBootRecordClass
}
