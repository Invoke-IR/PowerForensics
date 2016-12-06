using System;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexEntry
    {
        #region Properties

        /// <summary>
        /// Low 6B: MFT record index, High 2B: MFT record sequence number
        /// </summary>
        public ulong RecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public bool Directory;

        /// <summary>
        /// 
        /// </summary>
        public string Filename;

        /// <summary>
        /// 
        /// </summary>
        public string FullName;

        internal ushort Size;

        internal ushort StreamSize;

        internal byte Flags;

        internal byte[] Stream;

        internal FileName Entry;
        
        #endregion Properties

        #region Constructors

        internal IndexEntry(byte[] bytes)
        {
            RecordNumber = (BitConverter.ToUInt64(bytes, 0x00) & 0x0000FFFFFFFFFFFF);
            Size = BitConverter.ToUInt16(bytes, 0x08);
            StreamSize = BitConverter.ToUInt16(bytes, 0x0A);
            Flags = bytes[0x0C];
            Stream = Helper.GetSubArray(bytes, 0x10, this.StreamSize);

            if (!(this.Stream.Length == 0))
            {
                // Instantiate a FileName Object from IndexEntry Stream
                Entry = new FileName(this.Stream);
                Filename = Entry.Filename;
            }
        }

        internal IndexEntry(byte[] bytes, int offset)
        {
            RecordNumber = (BitConverter.ToUInt64(bytes, 0x00 + offset) & 0x0000FFFFFFFFFFFF);
            Size = BitConverter.ToUInt16(bytes, 0x08 + offset);
            StreamSize = BitConverter.ToUInt16(bytes, 0x0A + offset);
            Flags = bytes[0x0C + offset];
            Stream = Helper.GetSubArray(bytes, 0x10 + offset, this.StreamSize);

            if (!(this.Stream.Length == 0))
            {
                // Instantiate a FileName Object from IndexEntry Stream
                Entry = new FileName(this.Stream);
                Filename = Entry.Filename;
            }
        }

        private IndexEntry(FileRecord record)
        {
            RecordNumber = record.RecordNumber;
            Filename = record.Name;
            FullName = record.FullName;
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IndexEntry Get(string path)
        {
            string[] paths = path.TrimEnd('\\').Split('\\');

            // Determine Volume Name
            string volume = Helper.GetVolumeFromPath(path);

            // Test volume path
            Helper.getVolumeName(ref volume);

            int index = -1;

            List<IndexEntry> indexEntryList = new List<IndexEntry>();

            for (int i = 0; i < paths.Length; i++)
            {
                if (index == -1)
                {
                    index = 5;
                }
                else{
                    bool match = false;
                    
                    foreach (IndexEntry entry in indexEntryList)
                    {
                        if (entry.Entry.Filename.ToUpper() == paths[i].ToUpper())
                        {
                            index = (int)entry.RecordNumber;
                            match = true;
                        }
                    }
                    if (!(match))
                    {
                        throw new Exception("Path " + path + " not found.");
                    }
                }

                FileRecord record = FileRecord.Get(volume, index, false);

                indexEntryList.Clear();

                if (i < paths.Length - 1)
                {
                    foreach (FileRecordAttribute attr in record.Attribute)
                    {
                        if (attr.Name == FileRecordAttribute.ATTR_TYPE.INDEX_ROOT)
                        {
                            foreach (IndexEntry entry in (attr as IndexRoot).Entries)
                            {
                                if (entry.Entry.Namespace != 0x02)
                                {
                                    indexEntryList.Add(entry);
                                }
                            }
                        }
                        else if (attr.Name == FileRecordAttribute.ATTR_TYPE.INDEX_ALLOCATION)
                        {
                            // Get INDEX_ALLOCATION bytes
                            IndexAllocation IA = new IndexAllocation(attr as NonResident, volume);

                            foreach (IndexEntry entry in IA.Entries)
                            {
                                if (entry.Entry.Namespace != 0x02)
                                {
                                    indexEntryList.Add(entry);
                                }
                            }
                        }
                    }
                }
                else
                {
                    return new IndexEntry(record);
                }
            }
            throw new Exception("The IndexEntry object for the specified path could not be found.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IndexEntry[] GetInstances(string path)
        {
            string[] paths = path.TrimEnd('\\').Split('\\');

            // Determine Volume Name
            string volume = Helper.GetVolumeFromPath(path);

            // Test volume path
            Helper.getVolumeName(ref volume);

            int index = -1;

            List<IndexEntry> indexEntryList = new List<IndexEntry>();

            for (int i = 0; i < paths.Length; i++)
            {
                if (index == -1)
                {
                    index = 5;
                }
                else
                {
                    bool match = false;

                    foreach (IndexEntry entry in indexEntryList)
                    {

                        if (entry.Entry.Filename.ToUpper() == paths[i].ToUpper())
                        {
                            index = (int)entry.RecordNumber;
                            match = true;
                        }
                    }
                    if (!(match))
                    {
                        throw new Exception("Path " + path + " not found.");
                    }
                }

                FileRecord record = FileRecord.Get(volume, index, true);

                indexEntryList.Clear();

                if (record.Directory)
                {
                    foreach (FileRecordAttribute attr in record.Attribute)
                    {
                        if (attr.Name == FileRecordAttribute.ATTR_TYPE.INDEX_ROOT)
                        {
                            try
                            {
                                foreach (IndexEntry entry in (attr as IndexRoot).Entries)
                                {
                                    if (entry.Entry.Namespace != 0x02)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.Append(path.TrimEnd('\\'));
                                        sb.Append("\\");
                                        sb.Append(entry.Filename);
                                        entry.FullName = sb.ToString();
                                        indexEntryList.Add(entry);
                                    }
                                }
                            }
                            catch
                            {
                                return null;
                            }
                        }
                        else if (attr.Name == FileRecordAttribute.ATTR_TYPE.INDEX_ALLOCATION)
                        {
                            // Get INDEX_ALLOCATION bytes
                            IndexAllocation IA = new IndexAllocation(attr as NonResident, volume);

                            foreach (IndexEntry entry in IA.Entries)
                            {
                                if (entry.Entry.Namespace != 0x02)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append(path.TrimEnd('\\'));
                                    sb.Append("\\");
                                    sb.Append(entry.Filename);
                                    entry.FullName = sb.ToString();
                                    indexEntryList.Add(entry);
                                }
                            }
                        }
                    }
                }
                else
                {
                    IndexEntry[] indexArray = new IndexEntry[1];
                    indexArray[0] = new IndexEntry(record);
                    return indexArray;
                }
            }

            return indexEntryList.ToArray();
        }
        
        #endregion Static Methods
    }
}