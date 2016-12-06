using System;
using System.Collections.Generic;
using PowerForensics.FileSystems;

namespace PowerForensics.FileSystems.Fat
{
    /// <summary>
    /// 
    /// </summary>
    public class FileAllocationTableEntry
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static int StartSector;

        /// <summary>
        /// 
        /// </summary>
        public static int EndSector;

        #endregion Properties

        #region Constructors

        internal FileAllocationTableEntry(int startSector, int endSector)
        {
            StartSector = startSector;
            EndSector = endSector;
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        public static FileAllocationTableEntry Get(string volume, int sector)
        {
            FatVolumeBootRecord vbr = VolumeBootRecord.Get(volume) as FatVolumeBootRecord;
            byte[] bytes = FileAllocationTable.GetBytes(volume, vbr);

            int endSector = 0;

            switch (vbr.FatType)
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

        #endregion Static Methods
    }
}
