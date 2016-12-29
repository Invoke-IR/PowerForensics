using PowerForensics;
using PowerForensics.FileSystems.ExFat;
using PowerForensics.FileSystems.Ext;
using PowerForensics.FileSystems.Fat;
using PowerForensics.FileSystems.HFSPlus;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.FileSystems
{
    /// <summary>
    /// 
    /// </summary>
    public class FileSystemEntry
    {
        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileSystemEntry Get(string path)
        {
            switch (Helper.GetFileSystemType(Helper.GetVolumeFromPath(path)))
            {
                case Helper.FILE_SYSTEM_TYPE.EXFAT:
                    return null;
                case Helper.FILE_SYSTEM_TYPE.FAT:
                    return DirectoryEntry.Get(path);
                case Helper.FILE_SYSTEM_TYPE.NTFS:
                    return FileRecord.Get(path);
                default:
                    return null;
            }
        }

        public static byte[] GetBytes(string path)
        {
            switch (Helper.GetFileSystemType(Helper.GetVolumeFromPath(path)))
            {
                case Helper.FILE_SYSTEM_TYPE.EXFAT:
                    return null;
                case Helper.FILE_SYSTEM_TYPE.FAT:
                    return DirectoryEntry.GetBytes(path);
                case Helper.FILE_SYSTEM_TYPE.NTFS:
                    return FileRecord.GetRecordBytes(path);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static FileSystemEntry[] GetInstances(string volume)
        {
            switch (Helper.GetFileSystemType(volume))
            {
                case Helper.FILE_SYSTEM_TYPE.EXFAT:
                    return null;
                case Helper.FILE_SYSTEM_TYPE.FAT:
                    return DirectoryEntry.GetInstances(volume);
                case Helper.FILE_SYSTEM_TYPE.NTFS:
                    return FileRecord.GetInstances(volume);
                default:
                    return null;
            }
        }

        #endregion Static Methods
    }
}
