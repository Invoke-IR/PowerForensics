using System;
using System.IO;
using System.Text;
using PowerForensics.FileSystems;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class NtfsVolumeBootRecord : VolumeBootRecord
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly long BytesPerFileRecord;

        /// <summary>
        /// 
        /// </summary>
        public readonly long BytesPerIndexBlock;

        /// <summary>
        /// 
        /// </summary>
        public readonly long TotalSectors;

        /// <summary>
        /// 
        /// </summary>
        public readonly long MftStartIndex;

        /// <summary>
        /// 
        /// </summary>
        public readonly long MftMirrStartIndex;

        /// <summary>
        /// 
        /// </summary>
        public readonly string VolumeSerialNumber;

        #endregion Properties

        #region Constructors

        internal NtfsVolumeBootRecord(byte[] bytes)
        {
            // Check if NTFS Partition
            if (Encoding.ASCII.GetString(bytes, 0x03, 0x08) == "NTFS    ")
            {
                BytesPerSector = BitConverter.ToUInt16(bytes, 0x0B);
                SectorsPerCluster = bytes[0x0D];
                BytesPerCluster = BytesPerSector * SectorsPerCluster;
                ReservedSectors = BitConverter.ToUInt16(bytes, 0x0E);
                MediaDescriptor = (MEDIA_DESCRIPTOR)bytes[0x15];
                SectorsPerTrack = BitConverter.ToUInt16(bytes, 0x18);
                NumberOfHeads = BitConverter.ToUInt16(bytes, 0x1A);
                HiddenSectors = BitConverter.ToUInt32(bytes, 0x1C);
                TotalSectors = BitConverter.ToInt64(bytes, 0x28);
                MftStartIndex = BitConverter.ToInt64(bytes, 0x30);
                MftMirrStartIndex = BitConverter.ToInt64(bytes, 0x38);
                BytesPerFileRecord = getBytesPerFileRecord(bytes, BytesPerCluster);
                BytesPerIndexBlock = getBytesPerIndexBlock(bytes, BytesPerCluster);
                VolumeSerialNumber = getVolumeSerialNumber(bytes);
                CodeSection = Helper.GetSubArray(bytes, 0x50, 0x1AE);
            }
            else
            {
                throw new Exception("Volume is not NTFS formatted.");
            }
        }

        #endregion Constructors

        #region Static Methods

        private static byte[] GetBytes(FileStream streamToRead)
        {
            return Helper.readDrive(streamToRead, 0x00, 0x200);
        }

        private static long getBytesPerFileRecord(byte[] bytes, int bytesPerCluster)
        {
            sbyte clustersPerFileRecord = (sbyte)bytes[0x40];
            if (clustersPerFileRecord < 0)
            {
                return (long)Math.Pow(2, Math.Abs(clustersPerFileRecord));
            }
            else
            {
                return clustersPerFileRecord * bytesPerCluster;
            }
        }

        private static long getBytesPerIndexBlock(byte[] bytes, int bytesPerCluster)
        {
            sbyte clustersPerIndexBlock = (sbyte)bytes[0x44];
            if (clustersPerIndexBlock < 0)
            {
                return (long)Math.Pow(2, Math.Abs(clustersPerIndexBlock));
            }
            else
            {
                return clustersPerIndexBlock * bytesPerCluster;
            }
        }

        private static string getVolumeSerialNumber(byte[] bytes)
        {
            byte[] serialNumberBytes = Helper.GetSubArray(bytes, 0x48, 0x04);
            Array.Reverse(serialNumberBytes);
            return BitConverter.ToString(serialNumberBytes).Remove(2, 1).Remove(7, 1);
        }

        #endregion Static Methods
    }

}