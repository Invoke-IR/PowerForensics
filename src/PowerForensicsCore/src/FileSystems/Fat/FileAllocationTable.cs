using System;
using PowerForensics.Generic;
using PowerForensics.Utilities;

namespace PowerForensics.Fat
{
    public class FileAllocationTable
    {
        #region StaticMethods

        public static byte[] GetBytes(string volume)
        {
            return GetBytes(volume, VolumeBootRecord.Get(volume) as FatVolumeBootRecord);
        }

        internal static byte[] GetBytes(string volume, FatVolumeBootRecord vbr)
        {
            long DirectoryEntryOffset = vbr.ReservedSectors * vbr.BytesPerSector;
            //return Helper.readDrive(volume, DirectoryEntryOffset, (vbr.SectorsPerFat * vbr.BytesPerSector));
            return null;
        }

        #endregion StaticMethods
    }
}
