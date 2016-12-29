using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.FileSystems;
using PowerForensics.Utilities;

namespace PowerForensics.FileSystems.Fat
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectoryEntry : FileSystemEntry
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum FILE_ATTR
        {
            /// <summary>
            /// 
            /// </summary>
            ATTR_READ_ONLY = 0x01,

            /// <summary>
            /// 
            /// </summary>
            ATTR_HIDDEN = 0x02,

            /// <summary>
            /// 
            /// </summary>
            ATTR_SYSTEM = 0x04,

            /// <summary>
            /// 
            /// </summary>
            ATTR_VOLUME_ID = 0x08,

            /// <summary>
            /// 
            /// </summary>
            ATTR_DIRECTORY = 0x10,

            /// <summary>
            /// 
            /// </summary>
            ATTR_ARCHIVE = 0x20,

            /// <summary>
            /// 
            /// </summary>
            ATTR_LONG_NAME = ATTR_READ_ONLY | ATTR_HIDDEN | ATTR_SYSTEM | ATTR_VOLUME_ID
        }

        #endregion Enums

        #region Properties

        private readonly string Volume;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FileName;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FullName;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Deleted;

        private readonly FILE_ATTR DIR_Attribute;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Directory;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Hidden;

        private readonly byte DIR_CreationTimeTenth;

        private readonly ushort DIR_CreationTime;

        private readonly ushort DIR_CreationDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime CreationTime;

        private readonly ushort DIR_LastAccessDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessTime;

        private readonly ushort DIR_FirstClusterHI;

        private readonly ushort DIR_WriteTime;

        private readonly ushort DIR_WriteDate;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime WriteTime;

        private readonly ushort DIR_FirstClusterLO;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FirstCluster;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FileSize;

        #endregion Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="volume"></param>
        /// <param name="longNameList"></param>
        /// <param name="directoryName"></param>
        public DirectoryEntry(byte[] bytes, int index, string volume, List<LongDirectoryEntry> longNameList, string directoryName)
        {
            Volume = volume;
            FileName = GetShortName(bytes, index);
            FullName = GetLongName(FileName, longNameList, directoryName);
            Deleted = TestIfFree(bytes, index);
            DIR_Attribute = (FILE_ATTR)bytes[11 + index];
            Directory = (DIR_Attribute & FILE_ATTR.ATTR_DIRECTORY) == FILE_ATTR.ATTR_DIRECTORY;
            Hidden = (DIR_Attribute & FILE_ATTR.ATTR_HIDDEN) == FILE_ATTR.ATTR_HIDDEN;
            DIR_CreationTimeTenth = bytes[13 + index];
            DIR_CreationTime = BitConverter.ToUInt16(bytes, 14 + index);
            DIR_CreationDate = BitConverter.ToUInt16(bytes, 16 + index);
            CreationTime = Helper.GetFATTime(DIR_CreationDate, DIR_CreationTime, DIR_CreationTimeTenth);
            DIR_LastAccessDate = BitConverter.ToUInt16(bytes, 18 + index);
            AccessTime = Helper.GetFATTime(DIR_LastAccessDate);
            DIR_FirstClusterHI = BitConverter.ToUInt16(bytes, 20 + index);
            DIR_WriteTime = BitConverter.ToUInt16(bytes, 22 + index);
            DIR_WriteDate = BitConverter.ToUInt16(bytes, 24 + index);
            WriteTime = Helper.GetFATTime(DIR_WriteDate, DIR_WriteTime);
            DIR_FirstClusterLO = BitConverter.ToUInt16(bytes, 26 + index);
            FirstCluster = (uint)(DIR_FirstClusterHI << 16) + DIR_FirstClusterLO;
            FileSize = BitConverter.ToUInt32(bytes, 28 + index);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryEntry Get(string path)
        {
            string[] pathParts = path.ToLower().TrimEnd('\\').Split('\\');

            string volume = Helper.GetVolumeFromPath(path);
            DirectoryEntry[] entries = GetRootDirectory(volume);

            if (pathParts.Length == 1)
            {
                foreach (DirectoryEntry entry in entries)
                {
                    if (entry.FileName == "FAT32")
                    {
                        return entry;
                    }
                }
                return null;
            }
            else
            {
                DirectoryEntry currentEntry = null;

                for (int i = 1; i < pathParts.Length; i++)
                {
                    if (i != 1)
                    {
                        entries = currentEntry.GetChildItem();
                    }

                    foreach (DirectoryEntry entry in entries)
                    {
                        if (entry.FullName.Split('\\')[i].ToLower() == pathParts[i])
                        {
                            currentEntry = entry;
                            break;
                        }
                    }
                }

                if (currentEntry.FullName.ToLower() == path.ToLower())
                {
                    return currentEntry;
                }
                else
                {
                    throw new Exception("Unable to find specificed file path");
                }
            }
        }

        internal static DirectoryEntry Get(byte[] bytes, int index, string volume, List<LongDirectoryEntry> list, string directoryName)
        {
            return new DirectoryEntry(bytes, index, volume, list, directoryName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static DirectoryEntry[] GetInstances(string volume)
        {
            List<DirectoryEntry> entryList = new List<DirectoryEntry>();

            DirectoryEntry[] entries = GetRootDirectory(volume);

            entryList.AddRange(entries);

            foreach (DirectoryEntry entry in entries)
            {
                if (entry.Directory)
                {
                    entryList.AddRange(entry.GetChildItem(true));
                }
            }

            return entryList.ToArray();
        }

        private static DirectoryEntry[] GetInstances(byte[] bytes, string volume, string directoryName)
        {
            List<DirectoryEntry> list = new List<DirectoryEntry>();

            List<LongDirectoryEntry> longList = new List<LongDirectoryEntry>();

            for (int i = 0; i < bytes.Length; i += 0x20)
            {
                FILE_ATTR attr = (FILE_ATTR)bytes[11 + i];

                // If entry is a Long File Name Entry then add it to the List
                if (attr == FILE_ATTR.ATTR_LONG_NAME)
                {
                    LongDirectoryEntry longEntry = LongDirectoryEntry.Get(bytes, i);
                    longList.Add(longEntry);
                }
                // Else the entry is a valid entry
                else
                {
                    DirectoryEntry entry = Get(bytes, i, volume, longList, directoryName);

                    if (entry.DIR_Attribute != 0 && entry.FileName != "." && entry.FileName != "..")
                    {
                        list.Add(entry);
                    }
                    
                    longList.Clear();
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string path)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryEntry[] GetChildItem(string path)
        {
            if (path.TrimEnd('\\').Split('\\').Length == 1)
            {
                string volume = Helper.GetVolumeFromPath(path);
                return GetRootDirectory(volume);
            }
            else
            {
                return Get(path).GetChildItem();
            }
        }

        private static DirectoryEntry[] GetRootDirectory(string volume)
        {
            string volLetter = Helper.GetVolumeLetter(volume);

            FatVolumeBootRecord vbr = VolumeBootRecord.Get(volume) as FatVolumeBootRecord;

            byte[] bytes = DD.Get(volume, vbr.BytesPerSector * vbr.RootDirectorySector, vbr.BytesPerSector, 2);

            return GetInstances(bytes, volume, volLetter);
        }

        private static string GetShortName(byte[] bytes, int index)
        {
            string name = Encoding.ASCII.GetString(bytes, 0 + index, 8).TrimEnd();
            string extension = Encoding.ASCII.GetString(bytes, 8 + index, 3);

            if (((FILE_ATTR)bytes[11 + index] & FILE_ATTR.ATTR_DIRECTORY) == FILE_ATTR.ATTR_DIRECTORY)
            {
                return name;
            }
            else if (name == "FAT32")
            {
                return name;
            }
            else 
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"{0}.{1}", name, extension);
                return sb.ToString();
            }
        }

        private static string GetLongName(string FileName, List<LongDirectoryEntry> list, string directoryName)
        {
            // Check if there are actually Long Name Entires
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    sb.Append(list[i].LDIR_NamePart);
                }

                return String.Format(@"{0}\{1}", directoryName, sb.ToString().TrimEnd((char)0x0000,(char)0xFFFF));
            }
            // else set LongName to DIR_Name
            else
            {
                if (FileName == "FAT32")
                {
                    return String.Format(@"{0}\", directoryName);
                }
                else
                {
                    return String.Format(@"{0}\{1}", directoryName, FileName);
                }
            }
        }

        private static bool TestIfFree(byte[] bytes, int index)
        {
            if (bytes[0 + index] == 0xE5)
            {
                return true;
            }
            else if (bytes[0 + index] == 0x00)
            {
                return true;
            }
            else if (bytes[0 + index] == 0x05)
            {
                return true;
            }
            else
            {
                return false;
            }
            //The following characters are not legal in any bytes of DIR_Name:  • Values less than 0x20 except for the special case of 0x05 in DIR_Name[0] described above. • 0x22, 0x2A, 0x2B, 0x2C, 0x2E, 0x2F, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x5B, 0x5C, 0x5D, and 0x7C.
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DirectoryEntry[] GetChildItem()
        {
            if (this.Directory)
            {
                byte[] bytes = this.GetContent();
                return GetInstances(bytes, this.Volume, this.FullName);
            }
            else
            {
                DirectoryEntry[] entries = new DirectoryEntry[1];
                entries[0] = this;
                return entries;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recurse"></param>
        /// <returns></returns>
        public DirectoryEntry[] GetChildItem(bool recurse)
        {
            if (recurse)
            {
                List<DirectoryEntry> entryList = new List<DirectoryEntry>();

                DirectoryEntry[] entries = this.GetChildItem();

                entryList.AddRange(entries);

                foreach (DirectoryEntry entry in entries)
                {
                    if (entry.Directory && entry.FileName != "." && entry.FileName != "..")
                    {
                        entryList.AddRange(entry.GetChildItem(true));
                    }
                }

                return entryList.ToArray();
            }
            else
            {
                return this.GetChildItem();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetContent()
        {
            FatVolumeBootRecord vbr = VolumeBootRecord.Get(this.Volume) as FatVolumeBootRecord;

            int RootDirSectors = ((vbr.BPB_RootEntryCount * 32) + (vbr.BytesPerSector - 1)) / vbr.BytesPerSector;

            uint FirstDataSector = (uint)(vbr.ReservedSectors + (vbr.BPB_NumberOfFATs * vbr.BPB_FatSize) + RootDirSectors);

            uint FirstSectorofCluster = ((this.FirstCluster - 2) * vbr.SectorsPerCluster) + FirstDataSector;

            byte[] bytes = DD.Get(this.Volume, (long)FirstSectorofCluster * (long)vbr.BytesPerSector, vbr.BytesPerSector, 1);

            if (this.Directory)
            {
                return bytes;
            }
            else
            {
                if (this.FileSize <= bytes.Length)
                {
                    return Helper.GetSubArray(bytes, 0, this.FileSize);
                }
                else
                {
                    List<byte> byteList = new List<byte>();

                    int[] clusters = FileAllocationTable.GetFatEntry(this.Volume, (int)this.FirstCluster);

                    foreach (int cluster in clusters)
                    {
                        long targetCluster = ((cluster - 2) * vbr.SectorsPerCluster) + FirstDataSector;
                        byteList.AddRange(DD.Get(this.Volume, targetCluster * vbr.BytesPerSector, vbr.BytesPerSector, 1));
                    }

                    return Helper.GetSubArray(byteList.ToArray(), 0, this.FileSize);
                }
            }
        }        

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class LongDirectoryEntry
    {
        #region Properties

        internal readonly byte LDIR_Ord;

        private readonly string LDIR_Name1;

        internal readonly DirectoryEntry.FILE_ATTR LDIR_Attr;

        internal readonly byte LDIR_Type;

        internal readonly byte LDIR_Chksum;

        private readonly string LDIR_Name2;

        internal readonly ushort LDIR_FstClusLO;

        private readonly string LDIR_Name3;

        internal readonly string LDIR_NamePart;

        #endregion Properties

        #region Constructors

        private LongDirectoryEntry(byte[] bytes, int index)
        {
            LDIR_Ord = bytes[0 + index];
            LDIR_Name1 = Encoding.Unicode.GetString(bytes, 1 + index, 10);
            LDIR_Attr = (DirectoryEntry.FILE_ATTR)bytes[11 + index];
            LDIR_Type = bytes[12 + index];
            LDIR_Chksum = bytes[13 + index];
            LDIR_Name2 = Encoding.Unicode.GetString(bytes, 14 + index, 12);
            LDIR_FstClusLO = BitConverter.ToUInt16(bytes, 26 + index);
            LDIR_Name3 = Encoding.Unicode.GetString(bytes, 28 + index, 4);
            LDIR_NamePart = String.Format("{0}{1}{2}", LDIR_Name1, LDIR_Name2, LDIR_Name3);
        }

        #endregion Constructors

        #region Static Methods

        internal static LongDirectoryEntry Get(byte[] bytes, int index)
        {
            return new LongDirectoryEntry(bytes, index);
        }

        #endregion Static Methods
    }
}
