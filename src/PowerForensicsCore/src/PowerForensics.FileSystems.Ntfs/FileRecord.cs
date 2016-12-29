using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PowerForensics.FileSystems;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum FILE_RECORD_FLAG
    {
        /// <summary>
        /// File record is in use
        /// </summary>
        INUSE = 0x01,

        /// <summary>
        /// File record is a directory
        /// </summary>
        DIR = 0x02
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileRecord : FileSystemEntry
    {
        #region Properties

        /// <summary>
        /// Path to Volume
        /// </summary>
        public readonly string VolumePath;

        /// <summary>
        /// Offset of Update Sequence
        /// </summary>
        private readonly ushort OffsetOfUS;

        /// <summary>
        /// Size in words of Update Sequence Number and Array
        /// </summary>
        private readonly ushort SizeOfUS;

        /// <summary>
        /// $LogFile Sequence Number
        /// </summary>
        public readonly ulong LogFileSequenceNumber;

        /// <summary>
        /// Sequence number
        /// </summary>
        public readonly ushort SequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort Hardlinks;

        /// <summary>
        /// 
        /// </summary>
        private ushort OffsetOfAttribute;

        /// <summary>
        /// 
        /// </summary>
        public readonly FILE_RECORD_FLAG Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Deleted;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Directory;

        /// <summary>
        /// 
        /// </summary>
        public readonly int RealSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly int AllocatedSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong ReferenceToBase;

        /// <summary>
        /// 
        /// </summary>
        private readonly ushort NextAttrId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly FileRecordAttribute[] Attribute;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifiedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ChangedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BornTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly StandardInformation.ATTR_STDINFO_PERMISSION Permission;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FullName;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ParentSequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong ParentRecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime FNModifiedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime FNAccessedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime FNChangedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime FNBornTime;

        #endregion Properties

        #region Constructors

        private FileRecord(byte[] recordBytes, string volume, int bytesPerFileRecord, bool fast)
        {
            if (Encoding.ASCII.GetString(recordBytes, 0x00, 0x04) == "FILE")
            {
                VolumePath = volume;
                OffsetOfUS = BitConverter.ToUInt16(recordBytes, 0x04);
                SizeOfUS = BitConverter.ToUInt16(recordBytes, 0x06);
                LogFileSequenceNumber = BitConverter.ToUInt64(recordBytes, 0x08);
                SequenceNumber = BitConverter.ToUInt16(recordBytes, 0x10);
                Hardlinks = BitConverter.ToUInt16(recordBytes, 0x12);
                OffsetOfAttribute = BitConverter.ToUInt16(recordBytes, 0x14);
                Flags = (FILE_RECORD_FLAG)BitConverter.ToUInt16(recordBytes, 0x16);
                Deleted = isDeleted(Flags);
                Directory = isDirectory(Flags);
                RealSize = BitConverter.ToInt32(recordBytes, 0x18);
                AllocatedSize = BitConverter.ToInt32(recordBytes, 0x1C);
                ReferenceToBase = BitConverter.ToUInt64(recordBytes, 0x20);
                NextAttrId = BitConverter.ToUInt16(recordBytes, 40);
                RecordNumber = BitConverter.ToUInt32(recordBytes, 44);
                Attribute = FileRecordAttribute.GetInstances(recordBytes, OffsetOfAttribute, bytesPerFileRecord, volume);

                #region AttributeProperties

                foreach (FileRecordAttribute attr in Attribute)
                {
                    if (attr.Name == FileRecordAttribute.ATTR_TYPE.STANDARD_INFORMATION)
                    {
                        StandardInformation stdInfo = attr as StandardInformation;
                        ModifiedTime = stdInfo.ModifiedTime;
                        AccessedTime = stdInfo.AccessedTime;
                        ChangedTime = stdInfo.ChangedTime;
                        BornTime = stdInfo.BornTime;
                        Permission = stdInfo.Permission;
                    }
                    else if (attr.Name == FileRecordAttribute.ATTR_TYPE.ATTRIBUTE_LIST)
                    {
                        if (!(Deleted))
                        {
                            AttributeList attrList = null;
                            List<FileRecordAttribute> list = new List<FileRecordAttribute>();
                            list.AddRange(Attribute);

                            if (attr.NonResident)
                            {
                                attrList = new AttributeList(attr as NonResident);
                            }
                            else
                            {
                                attrList = attr as AttributeList;
                            }

                            foreach (AttrRef attribute in attrList.AttributeReference)
                            {
                                if (attribute.RecordNumber != RecordNumber)
                                {
                                    FileRecord record = FileRecord.Get(volume, (int)attribute.RecordNumber);
                                    list.AddRange(record.Attribute);
                                    list.Remove(attr);
                                }
                            }

                            Attribute = list.ToArray();
                        }
                    }
                    else if (attr.Name == FileRecordAttribute.ATTR_TYPE.FILE_NAME)
                    {
                        FileName fN = attr as FileName;
                        if (!(fN.Namespace == 2))
                        {
                            Name = fN.Filename;
                            ParentSequenceNumber = fN.ParentSequenceNumber;
                            ParentRecordNumber = fN.ParentRecordNumber;
                            FNModifiedTime = fN.ModifiedTime;
                            FNAccessedTime = fN.AccessedTime;
                            FNChangedTime = fN.ChangedTime;
                            FNBornTime = fN.BornTime;
                        }
                    }
                }

                #endregion AttributeProperties

                #region FullName

                if (fast)
                {
                    FullName = Name;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    string volLetter = Helper.GetVolumeLetter(volume);

                    if (RecordNumber == 0)
                    {
                        sb.Append(volLetter);
                        sb.Append('\\');
                        sb.Append(Name);
                        FullName = sb.ToString();
                    }
                    else if (RecordNumber == 5)
                    {
                        FullName = volLetter;
                    }
                    else
                    {
                        FileRecord parent = new FileRecord(GetRecordBytes(volume, (int)ParentRecordNumber), volume, bytesPerFileRecord, false);
                        if (parent.SequenceNumber == this.ParentSequenceNumber)
                        {
                            sb.Append(parent.FullName);
                        }
                        else
                        {
                            sb.Append(@"$OrphanFiles");
                        }

                        if (Name != null)
                        {
                            sb.Append('\\');
                            FullName = sb.Append(Name).ToString();
                        }
                        else
                        {
                            FullName = sb.ToString();
                        }
                    }
                }

                #endregion FullName
            }
        }

        private FileRecord(ref FileRecord[] recordArray, byte[] bytes, int offset, int bytesPerFileRecord, string volume, bool fast)
        {
            if (Encoding.ASCII.GetString(bytes, 0x00 + offset, 0x04) == "FILE")
            {
                OffsetOfUS = BitConverter.ToUInt16(bytes, 0x04 + offset);
                SizeOfUS = BitConverter.ToUInt16(bytes, 0x06 + offset);
                LogFileSequenceNumber = BitConverter.ToUInt64(bytes, 0x08 + offset);
                SequenceNumber = BitConverter.ToUInt16(bytes, 0x10 + offset);
                Hardlinks = BitConverter.ToUInt16(bytes, 0x12 + offset);
                OffsetOfAttribute = BitConverter.ToUInt16(bytes, 0x14 + offset);
                Flags = (FILE_RECORD_FLAG)BitConverter.ToUInt16(bytes, 0x16 + offset);
                Deleted = isDeleted(Flags);
                Directory = isDirectory(Flags);
                RealSize = BitConverter.ToInt32(bytes, 0x18 + offset);
                AllocatedSize = BitConverter.ToInt32(bytes, 0x1C + offset);
                ReferenceToBase = BitConverter.ToUInt64(bytes, 0x20 + offset);
                NextAttrId = BitConverter.ToUInt16(bytes, 0x28 + offset);
                RecordNumber = BitConverter.ToUInt32(bytes, 0x2C + offset);
                Attribute = FileRecordAttribute.GetInstances(bytes, OffsetOfAttribute + offset, bytesPerFileRecord, volume);

                #region AttributeProperties

                foreach (FileRecordAttribute attr in Attribute)
                {
                    if (attr.Name == FileRecordAttribute.ATTR_TYPE.STANDARD_INFORMATION)
                    {
                        StandardInformation stdInfo = attr as StandardInformation;
                        ModifiedTime = stdInfo.ModifiedTime;
                        AccessedTime = stdInfo.AccessedTime;
                        ChangedTime = stdInfo.ChangedTime;
                        BornTime = stdInfo.BornTime;
                        Permission = stdInfo.Permission;
                    }
                    else if (attr.Name == FileRecordAttribute.ATTR_TYPE.ATTRIBUTE_LIST)
                    {
                        if (!(Deleted))
                        {
                            AttributeList attrList = null;
                            List<FileRecordAttribute> list = new List<FileRecordAttribute>();
                            list.AddRange(Attribute);

                            if (attr.NonResident)
                            {
                                attrList = new AttributeList(attr as NonResident);
                            }
                            else
                            {
                                attrList = attr as AttributeList;
                            }

                            foreach (AttrRef attribute in attrList.AttributeReference)
                            {
                                if (attribute.RecordNumber != RecordNumber)
                                {
                                    FileRecord record = null;

                                    // Test if we have already parse the record
                                    if (recordArray[attribute.RecordNumber] != null)
                                    {
                                        record = recordArray[attribute.RecordNumber];
                                    }
                                    else
                                    {
                                        // If not parse it and add it to the array
                                        record = new FileRecord(ref recordArray, bytes, bytesPerFileRecord * (int)attribute.RecordNumber, bytesPerFileRecord, volume, fast);
                                        recordArray[attribute.RecordNumber] = record;
                                    }

                                    // Add the attributes to the attribute array
                                    list.AddRange(record.Attribute);
                                    list.Remove(attr);
                                }
                            }

                            Attribute = list.ToArray();
                        }
                    }
                    else if (attr.Name == FileRecordAttribute.ATTR_TYPE.FILE_NAME)
                    {
                        FileName fN = attr as FileName;
                        if (!(fN.Namespace == 2))
                        {
                            Name = fN.Filename;
                            ParentSequenceNumber = fN.ParentSequenceNumber;
                            ParentRecordNumber = fN.ParentRecordNumber;
                            FNModifiedTime = fN.ModifiedTime;
                            FNAccessedTime = fN.AccessedTime;
                            FNChangedTime = fN.ChangedTime;
                            FNBornTime = fN.BornTime;
                        }
                    }
                }

                #endregion AttributeProperties

                #region FullName

                if (fast)
                {
                    FullName = Name;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    string volLetter = Helper.GetVolumeLetter(volume);

                    if (RecordNumber == 0)
                    {
                        sb.Append(volLetter);
                        sb.Append('\\');
                        sb.Append(Name);
                        FullName = sb.ToString();
                    }
                    else if (RecordNumber == 5)
                    {
                        FullName = volLetter;
                    }
                    else
                    {
                        FileRecord parent = null;

                        if (recordArray[this.ParentRecordNumber] != null)
                        {
                            parent = recordArray[this.ParentRecordNumber];
                        }
                        else
                        {
                            parent = new FileRecord(ref recordArray, bytes, bytesPerFileRecord * (int)this.ParentRecordNumber, bytesPerFileRecord, volume, fast);
                            recordArray[this.ParentRecordNumber] = parent;
                        }

                        if (parent.SequenceNumber == this.ParentSequenceNumber)
                        {
                            sb.Append(parent.FullName);
                        }
                        else
                        {
                            sb.Append(@"$OrphanFiles");
                        }

                        if (Name != null)
                        {
                            sb.Append('\\');
                            FullName = sb.Append(Name).ToString();
                        }
                        else
                        {
                            FullName = sb.ToString();
                        }
                    }
                }

                #endregion FullName
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static FileRecord[] GetInstances(string volume)
        {
            return GetInstances(volume, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileRecord[] GetInstancesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            byte[] mftBytes = record.GetContent();
            return GetInstances(mftBytes, Helper.GetVolumeFromPath(path), false);
        }

        internal static FileRecord[] GetInstances(string volume, bool fast)
        {
            FileRecord record = FileRecord.Get(volume, MftIndex.MFT_INDEX, true);
            byte[] mftBytes = record.GetContent();
            return GetInstances(mftBytes, volume, fast);
        }

        private static FileRecord[] GetInstances(byte[] bytes, string volume, bool fast)
        {
            NtfsVolumeBootRecord vbr = VolumeBootRecord.Get(volume) as NtfsVolumeBootRecord;

            // Determine the size of an MFT File Record
            int bytesPerFileRecord = (int)vbr.BytesPerFileRecord;

            // Calulate the number of entries in the MFT
            int fileCount = bytes.Length / bytesPerFileRecord;

            // Instantiate an array of FileRecord objects
            FileRecord[] recordArray = new FileRecord[fileCount];

            // Apply fixup values across MFT Bytes
            ApplyFixup(ref bytes, bytesPerFileRecord);

            // Now we need to iterate through all possible index values
            for (int index = 0x00; index < fileCount; index++)
            {
                // Check if current record has been instantiated
                if (recordArray[index] == null)
                {
                    // Instantiate FileRecord object
                    recordArray[index] = new FileRecord(ref recordArray, bytes, index * bytesPerFileRecord, bytesPerFileRecord, volume, fast);
                }
            }

            return recordArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileRecord Get(string path)
        {
            return Get(path, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static FileRecord Get(string volume, int index)
        {
            return Get(volume, index, false);
        }

        internal static FileRecord Get(string path, bool fast)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return Get(volume, (int)entry.RecordNumber, fast);
        }

        internal static FileRecord Get(string volume, int index, bool fast)
        {
            NtfsVolumeBootRecord vbr = VolumeBootRecord.Get(volume) as NtfsVolumeBootRecord;
            byte[] bytes = GetRecordBytes(volume, index);
            return Get(bytes, volume, (int)vbr.BytesPerFileRecord, fast);
        }

        internal static FileRecord Get(byte[] bytes, string volume, int bytesPerFileRecord, bool fast)
        {
            return new FileRecord(bytes, volume, bytesPerFileRecord, fast);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetRecordBytes(string path)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return GetRecordBytesPrivate(volume, (int)entry.RecordNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte[] GetRecordBytes(string volume, int index)
        {
            return GetRecordBytesPrivate(volume, index);
        }

        private static byte[] GetRecordBytesPrivate(string volume, int index)
        {
            // Get filestream based on hVolume
            using (FileStream streamToRead = Helper.getFileStream(volume))
            {
                // Get Volume Boot Record
                NtfsVolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead) as NtfsVolumeBootRecord;

                // Determine start of MFT
                long mftStartOffset = VBR.MftStartIndex * VBR.BytesPerCluster;

                // Get FileRecord for $MFT
                FileRecord mftRecord = MasterFileTable.GetRecord(streamToRead, volume);

                // Get $MFT Data Attribute
                NonResident data = null;

                foreach (FileRecordAttribute attr in mftRecord.Attribute)
                {
                    if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                    {
                        data = attr as NonResident;
                        break;
                    }
                }

                // Iterate through fragments of the MFT
                foreach (DataRun dr in data.DataRun)
                {
                    long DataRunRecords = (dr.ClusterLength * VBR.BytesPerCluster) / VBR.BytesPerFileRecord;

                    // Check if index can be found in current DataRun
                    if (index < (int)DataRunRecords)
                    {
                        long recordOffset = (dr.StartCluster * VBR.BytesPerCluster) + (index * VBR.BytesPerFileRecord);
                        byte[] recordBytesRaw = Helper.readDrive(streamToRead, recordOffset, VBR.BytesPerFileRecord);

                        ApplyFixup(ref recordBytesRaw, (int)VBR.BytesPerFileRecord);

                        return recordBytesRaw;
                    }

                    // Decrement index for the number of FileRecords in the current DataRun
                    else
                    {
                        index -= ((int)dr.ClusterLength * VBR.BytesPerCluster) / (int)VBR.BytesPerFileRecord;
                    }
                }
                throw new Exception("Could not find the FileRecord requested...");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetContentBytes(string path)
        {
            return GetContentBytes(path, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="streamName"></param>
        /// <returns></returns>
        public static byte[] GetContentBytes(string path, string streamName)
        {
            FileRecord record = Get(path, true);
            return record.GetContent(streamName);
        }

        private static void ApplyFixup(ref byte[] bytes, int BytesPerFileRecord)
        {
            int offset = 0x00;

            while (offset < bytes.Length)
            {
                // Take UpdateSequence into account
                ushort usoffset = BitConverter.ToUInt16(bytes, 0x04 + offset);
                ushort ussize = BitConverter.ToUInt16(bytes, 0x06 + offset);

                if (ussize != 0)
                {
                    byte[] UpdateSequenceArray = Helper.GetSubArray(bytes, (usoffset + 0x02 + offset), (0x02 * ussize));

                    bytes[0x1FE + offset] = UpdateSequenceArray[0x00];
                    bytes[0x1FF + offset] = UpdateSequenceArray[0x01];
                    bytes[0x3FE + offset] = UpdateSequenceArray[0x02];
                    bytes[0x3FF + offset] = UpdateSequenceArray[0x03];
                }

                offset += BytesPerFileRecord;
            }
        }

        private static bool isDeleted(FILE_RECORD_FLAG flags)
        {
            if ((flags & FILE_RECORD_FLAG.INUSE) == FILE_RECORD_FLAG.INUSE)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool isDirectory(FILE_RECORD_FLAG flags)
        {
            if ((flags & FILE_RECORD_FLAG.DIR) == FILE_RECORD_FLAG.DIR)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Static Methods

        #region Instance Methods

        // TODO: Add Encoding parameter
        // TODO: Add DataStream parameter
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetContent()
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    if (attr.NameString == "")
                    {
                        return GetContent(attr);
                    }
                }
            }
            throw new Exception("Could not locate file contents");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StreamName"></param>
        /// <returns></returns>
        public byte[] GetContent(string StreamName)
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    if (attr.NameString == StreamName)
                    {
                        return GetContent(attr);
                    }
                }
            }
            throw new Exception("Could not locate desired stream");
        }

        internal byte[] GetContent(NtfsVolumeBootRecord VBR)
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    if (attr.NonResident)
                    {
                        return (attr as NonResident).GetBytes(VBR);
                    }
                    else
                    {
                        return (attr as Data).RawData;
                    }
                }
                else if (attr.Name == FileRecordAttribute.ATTR_TYPE.ATTRIBUTE_LIST)
                {
                    AttributeList attrlist = attr as AttributeList;
                    foreach (AttrRef ar in attrlist.AttributeReference)
                    {
                        if (ar.Name == "DATA")
                        {
                            FileRecord record = new FileRecord(FileRecord.GetRecordBytes(this.VolumePath, (int)ar.RecordNumber), this.VolumePath, (int)VBR.BytesPerFileRecord, true);
                            return record.GetContent();
                        }
                    }
                }
            }
            throw new Exception("Could not locate file contents");
        }

        private byte[] GetContent(FileRecordAttribute attribute)
        {
            if (attribute.NonResident)
            {
                return (attribute as NonResident).GetBytes();
            }
            else
            {
                return (attribute as Data).RawData;
            }

            throw new Exception("Could not locate file contents");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Destination"></param>
        public void CopyFile(string Destination)
        {
            this.CopyFile(Destination, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Destination"></param>
        /// <param name="StreamName"></param>
        public void CopyFile(string Destination, string StreamName)
        {
            byte[] fileBytes = this.GetContent(StreamName);

            // Open file for writing
            FileStream streamToWrite = new FileStream(Destination, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            // Writes a block of bytes to this stream using data from a byte array.
            streamToWrite.Write(fileBytes, 0, fileBytes.Length);

            // Close file stream
            streamToWrite.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IndexEntry[] GetChild()
        {
            return IndexEntry.GetInstances(this.FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileRecord GetParent()
        {
            return Get(VolumePath, (int)ParentRecordNumber, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UsnJrnl GetUsnJrnl()
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                StandardInformation SI = attr as StandardInformation;
                if (SI != null)
                {
                    return UsnJrnl.Get(this.VolumePath, SI.UpdateSequenceNumber);

                }
            }
            throw new Exception("No $STANDARD_INFORMATION Attirbute found");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetSlack()
        {
            if (!(this.Directory))
            {
                if (this.Attribute != null)
                {
                    foreach (FileRecordAttribute attr in this.Attribute)
                    {
                        if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                        {
                            if (attr.NonResident)
                            {
                                return (attr as NonResident).GetSlack();
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetMftSlack()
        {
            byte[] bytes = FileRecord.GetRecordBytes(this.VolumePath, (int)this.RecordNumber);
            return Helper.GetSubArray(bytes, this.RealSize - 1, this.AllocatedSize - this.RealSize);
        }

        #endregion Instance Methods

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Directory)
            {
                if (this.Deleted)
                {
                    return String.Format("[Directory] {0} (deleted)", this.FullName);
                }
                else
                {
                    return String.Format("[Directory] {0}", this.FullName);
                }
            }
            else
            {
                ulong size = 0;

                foreach (FileRecordAttribute a in this.Attribute)
                {
                    if (a.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                    {
                        if (a.NonResident)
                        {
                            NonResident d = a as NonResident;
                            size = d.RealSize;
                            break;
                        }
                        else
                        {
                            Data data = a as Data;
                            size = (ulong)data.RawData.Length;
                            break;
                        }
                    }
                }

                if (this.Deleted)
                {
                    return String.Format("[{0}] {1} (deleted)", size, this.FullName);
                }
                else
                {
                    return String.Format("[{0}] {1}", size, this.FullName);
                }

            }
        }

        #endregion Override Methods
    }
}
