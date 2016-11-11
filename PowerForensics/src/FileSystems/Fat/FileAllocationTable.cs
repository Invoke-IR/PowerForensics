using System;
using PowerForensics.Generic;
using PowerForensics.Utilities;

namespace PowerForensics.Fat
{
    public class FileAllocationTable
    {
        #region StaticMethods

        public static byte[] Get(string volume)
        {
            // Get VolumeBootRecord
            FatVolumeBootRecord vbr = VolumeBootRecord.Get(volume) as FatVolumeBootRecord;

            // Determine start sector of FAT
            uint RootDirSectors = (uint)(((vbr.BPB_RootEntryCount * 32) + (vbr.BytesPerSector - 1)) / vbr.BytesPerSector);

            /*if (BPB_FATSz16 != 0)
            {
                FATSz = BPB_FATSz16;
            }
            else
            {
                FATSz = BPB_FATSz32;
            }
            uint FirstDataSector = BPB_ResvdSecCnt + (BPB_NumFATs * FATSz) + RootDirSectors;

            return DD.Get(volume, RootDirSectors, FATSize, 1);*/
            return null;
        }

        #endregion StaticMethods
    }
}
