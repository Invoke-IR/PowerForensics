using System;
using System.Text;
using PowerForensics.FileSystems;

namespace PowerForensics.Fat
{
    public class FsInformationSector
    {
        #region Properties

        public readonly int FreeDataClusterCount;
        public readonly int LastAllocatedDataCluster;

        #endregion Properties

        #region Constructors

        internal FsInformationSector(byte[] bytes)
        {
            // 0x52 0x52 0x61 0x41
            if (BitConverter.ToUInt32(bytes, 0x00) != 0x41615252)
            {
                throw new Exception("Invalid FsInformationSector Lead Signature.");
            }

            // 0x72 0x72 0x41 0x61
            if (BitConverter.ToUInt32(bytes, 0x1E4) != 0x61417272)
            {
                throw new Exception("Invalid FsInformationSector Structure Signature.");
            }

            FreeDataClusterCount = BitConverter.ToInt32(bytes, 0x1E8);
            LastAllocatedDataCluster = BitConverter.ToInt32(bytes, 0x1EC);

            // 0x00 0x00 0x55 0xAA
            if (BitConverter.ToUInt32(bytes, 0x1FC) != 0xAA550000)
            {
                throw new Exception("Invalid FsInformationSector Trail Signature.");
            }
        }

        #endregion Constructors

        #region StaticMethods

        public static FsInformationSector Get(string volume)
        {
            return new FsInformationSector(GetBytes(volume));
        }

        public static byte[] GetBytes(string volume)
        {
            Fat.VolumeBootRecord vbr = VolumeBootRecord.Get(volume) as Fat.VolumeBootRecord;
            return Helper.readDrive(volume, (uint)vbr.LocationOfFsInformationSector * vbr.BytesPerSector, vbr.BytesPerSector);
        }

        #endregion StaticMethods
    }
}
