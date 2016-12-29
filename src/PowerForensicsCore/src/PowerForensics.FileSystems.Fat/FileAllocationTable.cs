using System;
using System.Collections.Generic;
using PowerForensics.FileSystems;
using PowerForensics.Utilities;

namespace PowerForensics.FileSystems.Fat
{
    class FileAllocationTable
    {
        #region Constants

        private const int EOF = 0x0FFFFFFF;

        #endregion Constants

        #region Static Methods

        private static byte[] GetBytes(string volume, FatVolumeBootRecord vbr)
        {
            return Helper.readDrive(volume, vbr.ReservedSectors * vbr.BytesPerSector, (vbr.BPB_FatSize * vbr.BytesPerSector));
        }

        internal static int[] GetFatEntry(string volume, int startSector)
        {
            FatVolumeBootRecord vbr = VolumeBootRecord.Get(volume) as FatVolumeBootRecord;
            byte[] fatBytes = GetBytes(volume, vbr);

            switch (vbr.FatType)
            {
                case "FAT12":
                    return parseFat12(fatBytes, startSector);
                case "FAT16":
                    return parseFat16(fatBytes, startSector);
                case "FAT32":
                    return parseFat32(fatBytes, startSector);
                default:
                    throw new Exception("FAT Type could not be determined.");
            }
        }

        private static int[] parseFat12(byte[] bytes, int startSector)
        {
            int[] intArray = new int[1];
            intArray[0] = 0;
            return intArray;
        }

        private static int[] parseFat16(byte[] bytes, int startSector)
        {
            List<int> sectorList = new List<int>();

            int nextSector = 0;

            for (int i = startSector; nextSector != EOF; i = nextSector)
            {
                nextSector = BitConverter.ToInt16(bytes, i * 2);

                if (nextSector == 0)
                {
                    return null;
                }
                else if (nextSector == -1)
                {
                    return null;
                }
                else
                {
                    sectorList.Add(i);
                }
            }

            return sectorList.ToArray();
        }

        private static int[] parseFat32(byte[] bytes, int startSector)
        {
            List<int> sectorList = new List<int>();

            int nextSector = 0;

            for (int i = startSector; nextSector != EOF; i = nextSector)
            {
                nextSector = BitConverter.ToInt32(bytes, i * 4);

                if (nextSector == 0)
                {
                    return null;
                }
                else if (nextSector == -1)
                {
                    return null;
                }
                else
                {
                    sectorList.Add(i);
                }
            }

            return sectorList.ToArray();
        }
    
        #endregion Static Methods
    }
}
