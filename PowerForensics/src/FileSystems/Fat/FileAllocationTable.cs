using System;
using System.Collections.Generic;

namespace PowerForensics.Fat
{
    public class FileAllocationTable
    {
        #region StaticMethods

        public static byte[] GetBytes(string volume)
        {
            return GetBytes(volume, FileSystems.VolumeBootRecord.Get(volume) as VolumeBootRecord);
        }
        
        internal static byte[] GetBytes(string volume, VolumeBootRecord vbr)
        {
            ulong DirectoryEntryOffset = (ulong)(vbr.ReservedSectors * vbr.BytesPerSector);
            return Helper.readDrive(volume, DirectoryEntryOffset, (vbr.SectorsPerFat * vbr.BytesPerSector));
        }

        #endregion StaticMethods
    }

    public class FileAllocationTableEntry
    {
        #region Properties

        public static int StartSector;
        public static int EndSector;

        #endregion Properties

        #region Constructors

        internal FileAllocationTableEntry(int startSector, int endSector)
        {
            StartSector = startSector;
            EndSector = endSector;
        }

        #endregion Constructors

        #region StaticMethods

        public static FileAllocationTableEntry Get(string volume, int sector)
        {
            VolumeBootRecord vbr = FileSystems.VolumeBootRecord.Get(volume) as VolumeBootRecord;
            byte[] bytes = FileAllocationTable.GetBytes(volume, vbr);

            int endSector = 0;

            switch (vbr.FileSystemType)
            {
                case "FAT12":
                    endSector = parseFat12(bytes, sector);
                    break;
                case "FAT16":
                    endSector = parseFat16(bytes, sector);
                    break;
                case "FAT32":
                    endSector = parseFat32(bytes, sector);
                    break;
            }

            return new FileAllocationTableEntry(sector, endSector);
        }

        private static int parseFat12(byte[] bytes, int sector)
        {
            return 0;
        }

        private static int parseFat16(byte[] bytes, int sector)
        {

            return 0;
        }

        private static int parseFat32(byte[] bytes, int sector)
        {
            List<int> list = new List<int>();
            int offset = sector * 4;
            int endSector = 0;
            do
            {
                
            } while (endSector >= 0);
            return 0;
        }

        #endregion StaticMethods
    }
}
