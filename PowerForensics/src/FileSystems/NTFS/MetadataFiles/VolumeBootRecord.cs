using System;
using System.IO;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region VolumeBootRecordClass

    public class VolumeBootRecord : PowerForensics.FileSystems.VolumeBootRecord
    {
        #region Properties

        public readonly double BytesPerFileRecord;
        public readonly double BytesPerIndexBlock;
        public readonly ulong TotalSectors;
        public readonly ulong MFTStartIndex;
        public readonly ulong MFTMirrStartIndex;
        public readonly string VolumeSerialNumber;

        #endregion Properties

        #region Constructors

        internal VolumeBootRecord(byte[] bytes)
        {
            // Get VolumeBootRecord Signature to determine File System Type
            Signature = Encoding.ASCII.GetString(bytes, 0x03, 0x08);

            // Check if NTFS Partition
            if (Signature == "NTFS    ")
            {
                BytesPerSector = BitConverter.ToUInt16(bytes, 0x0B);
                BytesPerCluster = (uint)(bytes[0x0D] * BytesPerSector);
                ReservedSectors = BitConverter.ToUInt16(bytes, 0x0E);
                MediaDescriptor = (MEDIA_DESCRIPTOR)bytes[0x15];
                SectorsPerTrack = BitConverter.ToUInt16(bytes, 0x18);
                NumberOfHeads = BitConverter.ToUInt16(bytes, 0x1A);
                HiddenSectors = BitConverter.ToUInt32(bytes, 0x1C);
                TotalSectors = BitConverter.ToUInt64(bytes, 0x28);
                MFTStartIndex = BitConverter.ToUInt64(bytes, 0x30);
                MFTMirrStartIndex = BitConverter.ToUInt64(bytes, 0x38);
                #region BytesPerFileRecord

                sbyte clustersPerFileRecord = (sbyte)bytes[0x40];
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

                sbyte clustersPerIndexBlock = (sbyte)bytes[0x44];
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

                byte[] serialNumberBytes = Helper.GetSubArray(bytes, 0x48, 0x04);
                Array.Reverse(serialNumberBytes);
                VolumeSerialNumber = BitConverter.ToString(serialNumberBytes).Remove(2, 1).Remove(7, 1);

                #endregion VolumeSerialNumber
                CodeSection = Helper.GetSubArray(bytes, 0x50, 0x1AE);
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
            Helper.getVolumeName(ref volume);
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
            Helper.getVolumeName(ref volume);
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

    #endregion VolumeBootRecordClass
}
