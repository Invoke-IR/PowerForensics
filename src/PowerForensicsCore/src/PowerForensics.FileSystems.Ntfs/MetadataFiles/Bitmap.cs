using System;
using System.IO;
using PowerForensics.FileSystems;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class Bitmap
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public long Cluster;

        /// <summary>
        /// 
        /// </summary>
        public bool InUse;
        
        #endregion Properties

        #region Constructors

        private Bitmap(long cluster, bool inUse)
        {
            Cluster = cluster;
            InUse = inUse;
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public static Bitmap Get(string volume, long cluster)
        {
            Helper.getVolumeName(ref volume);
            return Get(volume, MftIndex.BITMAP_INDEX, cluster);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public static Bitmap GetByPath(string path, long cluster)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return Get(volume, (int)entry.RecordNumber, cluster);
        }

        private static Bitmap Get(string volume, int recordNumber, long cluster)
        {
            long sectorOffset = cluster / 4096;

            // Check for valid Volume name
            Helper.getVolumeName(ref volume);

            // Set up FileStream to read volume
            FileStream streamToRead = Helper.getFileStream(volume);

            // Get VolumeBootRecord object for logical addressing
            VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

            // Get the Data attribute
            NonResident dataStream = Bitmap.GetDataStream(FileRecord.Get(volume, recordNumber, true));

            // Calulate the offset of the Bitmap file's data
            long dataRunOffset = dataStream.DataRun[0].StartCluster * VBR.BytesPerCluster;

            // Calculate the offset of the sector that contains the entry for the specific cluster
            long offset = dataRunOffset + (VBR.BytesPerSector * sectorOffset);

            // Read appropriate sector
            byte[] bytes = Helper.readDrive(streamToRead, offset, VBR.BytesPerSector);

            return Get(bytes, cluster);
        }

        private static Bitmap Get(byte[] bytes, long cluster)
        {
            long byteOffset = (cluster % 4096) / 8;

            byte b = bytes[byteOffset];

            bool inUse = false;

            switch (cluster % 8)
            {
                case 0:
                    if ((b & 0x01) > 0) { inUse = true; }
                    break;
                case 1:
                    if ((b & 0x02) > 0) { inUse = true; }
                    break;
                case 2:
                    if ((b & 0x04) > 0) { inUse = true; }
                    break;
                case 3:
                    if ((b & 0x08) > 0) { inUse = true; }
                    break;
                case 4:
                    if ((b & 0x10) > 0) { inUse = true; }
                    break;
                case 5:
                    if ((b & 0x20) > 0) { inUse = true; }
                    break;
                case 6:
                    if ((b & 0x40) > 0) { inUse = true; }
                    break;
                case 7:
                    if ((b & 0x80) > 0) { inUse = true; }
                    break;
            }
            return new Bitmap(cluster, inUse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap[] GetInstancesByPath(string path)
        {
            // Get Volume string from specified path
            string volume = Helper.GetVolumeFromPath(path);

            // Determine Record Number for specified file
            IndexEntry entry = IndexEntry.Get(path);

            // Get the proper data stream from the FileRecord
            NonResident dataStream = Bitmap.GetDataStream(FileRecord.Get(volume, MftIndex.BITMAP_INDEX, true));

            // Call GetInstances to return all associated Bitmap Values
            return GetInstances(dataStream.GetBytes());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static Bitmap[] GetInstances(string volume)
        {
            // Get the proper data stream from the FileRecord
            NonResident dataStream = Bitmap.GetDataStream(FileRecord.Get(volume, MftIndex.BITMAP_INDEX, true));

            // Call GetInstances to return all associated Bitmap Values
            return GetInstances(dataStream.GetBytes());
        }

        internal static Bitmap[] GetInstances(byte[] bytes)
        {
            Bitmap[] bitmapArray = new Bitmap[bytes.Length * 8];

            for (int j = 0; j < bytes.Length; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    bool inUse = false;
                    int index = ((j * 8) + k);

                    switch (k)
                    {
                        case 0:
                            if ((bytes[j] & 0x01) > 0) { inUse = true; }
                            break;
                        case 1:
                            if ((bytes[j] & 0x02) > 0) { inUse = true; }
                            break;
                        case 2:
                            if ((bytes[j] & 0x04) > 0) { inUse = true; }
                            break;
                        case 3:
                            if ((bytes[j] & 0x08) > 0) { inUse = true; }
                            break;
                        case 4:
                            if ((bytes[j] & 0x10) > 0) { inUse = true; }
                            break;
                        case 5:
                            if ((bytes[j] & 0x20) > 0) { inUse = true; }
                            break;
                        case 6:
                            if ((bytes[j] & 0x40) > 0) { inUse = true; }
                            break;
                        case 7:
                            if ((bytes[j] & 0x80) > 0) { inUse = true; }
                            break;
                    }
                    bitmapArray[index] = new Bitmap(index, inUse);
                }
            }

            return bitmapArray;
        }

        internal static NonResident GetDataStream(FileRecord fileRecord)
        {
            foreach (FileRecordAttribute attr in fileRecord.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    return attr as NonResident;
                }
            }
            throw new Exception("No DATA attribute found.");
        }
        
        #endregion Static Methods
    }
}