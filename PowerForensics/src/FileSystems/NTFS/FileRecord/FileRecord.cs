using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Utilities;

namespace PowerForensics.Ntfs
{
    #region FileRecordClass
    
    public class FileRecord
    {
        #region Enums

        private enum FILE_RECORD_FLAG
        {
            INUSE = 0x01,	// File record is in use
            DIR = 0x02	    // File record is a directory
        }
        
        #endregion Enums

        #region Properties

        public readonly string VolumePath;              // Path to Volume

        // Signature
        private readonly string Signature;              // "FILE" 

        // File Record Header
        private readonly ushort OffsetOfUS;             // Offset of Update Sequence
        private readonly  ushort SizeOfUS;		        // Size in words of Update Sequence Number & Array
        public readonly ulong LogFileSequenceNumber;    // $LogFile Sequence Number
        public readonly ushort SequenceNumber;          // Sequence number
        public readonly ushort Hardlinks;               // Hard link count
        private ushort OffsetOfAttribute;               // Offset of the first Attribute
        private readonly ushort Flags;                  // Flags
        public readonly bool Deleted;
        public readonly bool Directory;
        public readonly uint RealSize;                  // Real size of the FILE record
        public readonly uint AllocatedSize;             // Allocated size of the FILE record
        public readonly ulong ReferenceToBase;          // File reference to the base FILE record
        private readonly ushort NextAttrId;             // Next Attribute Id
        public readonly uint RecordNumber;              // Index number of this MFT Record

        // Attribute Array
        public readonly FileRecordAttribute[] Attribute;

        // $STANDARD_INFORMATION
        public readonly DateTime ModifiedTime;
        public readonly DateTime AccessedTime;
        public readonly DateTime ChangedTime;
        public readonly DateTime BornTime;
        public readonly StandardInformation.ATTR_STDINFO_PERMISSION Permission;

        // $FILE_NAME
        public readonly string FullName;
        public readonly string Name;
        public readonly ushort ParentSequenceNumber;
        public readonly ulong ParentRecordNumber;
        public readonly DateTime FNModifiedTime;
        public readonly DateTime FNAccessedTime;
        public readonly DateTime FNChangedTime;
        public readonly DateTime FNBornTime;

        #endregion Properties

        #region Constructors

        internal FileRecord(byte[] recordBytes, string volume, bool fast)
        {
            VolumePath = volume;

            Signature = Encoding.ASCII.GetString(recordBytes, 0x00, 0x04);

            if (Signature == "FILE")
            {
                // Parse File Record Header
                OffsetOfUS = BitConverter.ToUInt16(recordBytes, 4);
                SizeOfUS = BitConverter.ToUInt16(recordBytes, 6);
                LogFileSequenceNumber = BitConverter.ToUInt64(recordBytes, 8);
                SequenceNumber = BitConverter.ToUInt16(recordBytes, 16);
                Hardlinks = BitConverter.ToUInt16(recordBytes, 18);
                OffsetOfAttribute = BitConverter.ToUInt16(recordBytes, 20);
                Flags = BitConverter.ToUInt16(recordBytes, 22);
                #region Deleted

                if ((Flags & (ushort)FILE_RECORD_FLAG.INUSE) == (ushort)FILE_RECORD_FLAG.INUSE)
                {
                    Deleted = false;
                }
                else
                {
                    Deleted = true;
                }

                #endregion Deleted
                #region Directory

                if ((Flags & (ushort)FILE_RECORD_FLAG.DIR) == (ushort)FILE_RECORD_FLAG.DIR)
                {
                    Directory = true;
                }
                else
                {
                    Directory = false;
                }

                #endregion Directory
                RealSize = BitConverter.ToUInt32(recordBytes, 24);
                AllocatedSize = BitConverter.ToUInt32(recordBytes, 28);
                ReferenceToBase = BitConverter.ToUInt64(recordBytes, 32);
                NextAttrId = BitConverter.ToUInt16(recordBytes, 40);
                RecordNumber = BitConverter.ToUInt32(recordBytes, 44);
                #region Attribute

                // Create a byte array representing the attribute array
                byte[] attrArrayBytes = new byte[RealSize - OffsetOfAttribute];
                Array.Copy(recordBytes, OffsetOfAttribute, attrArrayBytes, 0, attrArrayBytes.Length);

                // Instantiate an empty list of Attr Objects (We don't know how many attributes the record contains)
                List<FileRecordAttribute> AttributeList = new List<FileRecordAttribute>();

                // Initialize the offset value to 0
                int currentOffset = 0;

                if (currentOffset < (attrArrayBytes.Length - 8))
                {
                    do
                    {
                        // Get attribute size
                        int attrSizeOffset = currentOffset + 4;
                        int attrSize = BitConverter.ToInt32(attrArrayBytes, attrSizeOffset);

                        // Create new byte array with just current attribute's bytes
                        byte[] currentAttrBytes = new byte[attrSize];
                        Array.Copy(attrArrayBytes, currentOffset, currentAttrBytes, 0, currentAttrBytes.Length);

                        // Increment currentOffset
                        currentOffset += attrSize;

                        FileRecordAttribute attr = FileRecordAttribute.Get(currentAttrBytes, volume);

                        if (attr != null)
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

                            AttributeList.Add(attr);
                        }
                    } while (currentOffset < (attrArrayBytes.Length - 8));
                }

                Attribute = AttributeList.ToArray();

                #endregion Attribute
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
                        FullName = volLetter + '\\';
                    }
                    else
                    {
                        FileRecord parent = new FileRecord(FileRecord.GetRecordBytes(volume, (int)ParentRecordNumber), volume, false);
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

        private FileRecord(ref FileRecord[] array, byte[] mftBytes, byte[] recordBytes, int bytesPerFileRecord, string volume)
        {
            Signature = Encoding.ASCII.GetString(recordBytes, 0x00, 0x04);

            if (Signature == "FILE")
            {
                // Parse File Record Header
                OffsetOfUS = BitConverter.ToUInt16(recordBytes, 4);
                SizeOfUS = BitConverter.ToUInt16(recordBytes, 6);
                LogFileSequenceNumber = BitConverter.ToUInt64(recordBytes, 8);
                SequenceNumber = BitConverter.ToUInt16(recordBytes, 16);
                Hardlinks = BitConverter.ToUInt16(recordBytes, 18);
                OffsetOfAttribute = BitConverter.ToUInt16(recordBytes, 20);
                Flags = BitConverter.ToUInt16(recordBytes, 22);
                Deleted = IsDeleted(Flags);
                Directory = IsDirectory(Flags);
                RealSize = BitConverter.ToUInt32(recordBytes, 24);
                AllocatedSize = BitConverter.ToUInt32(recordBytes, 28);
                ReferenceToBase = BitConverter.ToUInt64(recordBytes, 32);
                NextAttrId = BitConverter.ToUInt16(recordBytes, 40);
                RecordNumber = BitConverter.ToUInt32(recordBytes, 44);
                
                #region Attribute

                // Create a byte array representing the attribute array
                byte[] attrArrayBytes = new byte[RealSize - OffsetOfAttribute];

                ApplyFixup(ref recordBytes);

                Array.Copy(recordBytes, OffsetOfAttribute, attrArrayBytes, 0, attrArrayBytes.Length);

                // Instantiate an empty list of Attr Objects (We don't know how many attributes the record contains)
                List<FileRecordAttribute> AttributeList = new List<FileRecordAttribute>();

                // Initialize the offset value to 0
                int currentOffset = 0;

                if (currentOffset < (attrArrayBytes.Length - 8))
                {
                    do
                    {
                        // Get attribute size
                        int attrSizeOffset = currentOffset + 4;
                        int attrSize = BitConverter.ToInt32(attrArrayBytes, attrSizeOffset);

                        // Create new byte array with just current attribute's bytes
                        byte[] currentAttrBytes = new byte[attrSize];
                        Array.Copy(attrArrayBytes, currentOffset, currentAttrBytes, 0, currentAttrBytes.Length);

                        // Increment currentOffset
                        currentOffset += attrSize;

                        FileRecordAttribute attr = FileRecordAttribute.Get(currentAttrBytes, volume);

                        if (attr != null)
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

                            AttributeList.Add(attr);
                        }
                    } while (currentOffset < (attrArrayBytes.Length - 8));
                }

                Attribute = AttributeList.ToArray();

                #endregion Attribute

                #region FullName

                StringBuilder sb = new StringBuilder();
                string volLetter = Helper.GetVolumeLetter(volume);

                // Record 5 is the root of the drive
                if (RecordNumber == 5)
                {
                    sb.Append(volLetter);
                }
                else
                {
                    // Derive Path by looking at ParentRecord's FullName
                    if (array[(int)ParentRecordNumber] != null)
                    {
                        if (ParentSequenceNumber == array[(int)ParentRecordNumber].SequenceNumber)
                        {
                            sb.Append(array[(int)ParentRecordNumber].FullName);
                        }
                        else
                        {
                            sb.Append(@"$OrphanFiles\");
                        }
                    }

                    // If record for Parent does not already exist then instantiate it and add it to the array
                    else
                    {
                        byte[] parentBytes = Helper.GetSubArray(mftBytes, bytesPerFileRecord * (int)ParentRecordNumber, bytesPerFileRecord);
                        array[(int)ParentRecordNumber] = new FileRecord(ref array, mftBytes, parentBytes, bytesPerFileRecord, volume);

                        if (ParentSequenceNumber == array[(int)ParentRecordNumber].SequenceNumber)
                        {
                            sb.Append(array[(int)ParentRecordNumber].FullName);
                        }
                        else
                        {
                            sb.Append(@"$OrphanFiles\");
                        }
                    }

                    // Add file name to end of path
                    sb.Append(Name);
                }

                // Add trailing \ to any file that is a directory
                if (Directory)
                {
                    sb.Append('\\');
                }

                // Figure out a way to have record 15 not have a name of $MFT...

                FullName = sb.ToString();

                #endregion FullName
            }
        }

        #endregion Constructors

        #region StaticMethods

        #region GetMethod

        public static FileRecord Get(string path, bool fast)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return new FileRecord(GetRecordBytes(volume, (int)entry.RecordNumber), volume, fast);
        }

        public static FileRecord Get(string volume, int index, bool fast)
        {
            return new FileRecord(FileRecord.GetRecordBytes(volume, index), volume, fast);
        }

        #endregion GetMethod

        #region GetInstancesMethod

        public static FileRecord[] GetInstances(string volume)
        {
            FileRecord record = FileRecord.Get(volume, MftIndex.MFT_INDEX, true);
            byte[] mftBytes = record.GetContent();
            return GetInstances(mftBytes, volume);
        }

        public static FileRecord[] GetInstancesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            byte[] mftBytes = record.GetContent();
            return GetInstances(mftBytes, Helper.GetVolumeFromPath(path));
        }

        private static FileRecord[] GetInstances(byte[] bytes, string volume)
        {
            // Determine the size of an MFT File Record
            int bytesPerFileRecord = (int)(VolumeBootRecord.Get(volume)).BytesPerFileRecord;

            // Calulate the number of entries in the MFT
            int fileCount = bytes.Length / bytesPerFileRecord;

            // Instantiate an array of FileRecord objects
            FileRecord[] recordArray = new FileRecord[fileCount];

            // Instantiate a byte array large enough to store the bytes belonging to a file record
            byte[] recordBytes = new byte[bytesPerFileRecord];

            // Now we need to iterate through all possible index values
            for (int index = 0; index < fileCount; index++)
            {
                // Check if current record has been instantiated
                if (recordArray[index] == null)
                {
                    // Copy filerecord bytes into the recordBytes byte[]
                    Array.Copy(bytes, index * bytesPerFileRecord, recordBytes, 0, recordBytes.Length);

                    // Take UpdateSequence into account
                    ApplyFixup(ref recordBytes);

                    // Instantiate FileRecord object
                    recordArray[index] = new FileRecord(ref recordArray, bytes, recordBytes, bytesPerFileRecord, volume);
                }
            }
            return recordArray;
        }

        #endregion GetInstancesMethod

        #region GetRecordBytesMethod

        public static byte[] GetRecordBytes(string path)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return GetRecordBytesPrivate(volume, (int)entry.RecordNumber);
        }

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
                VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

                // Determine start of MFT
                ulong mftStartOffset = VBR.MFTStartIndex * VBR.BytesPerCluster;

                // Get FileRecord for $MFT
                FileRecord mftRecord = MasterFileTable.GetRecord(streamToRead, volume);

                // Get $MFT Data Attribute
                NonResident data = null;

                foreach (FileRecordAttribute attr in mftRecord.Attribute)
                {
                    if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                    {
                        data = attr as NonResident;
                    }
                }

                // Iterate through fragments of the MFT
                foreach (DataRun dr in data.DataRun)
                {
                    ulong DataRunRecords = ((ulong)dr.ClusterLength * (ulong)VBR.BytesPerCluster) / (ulong)VBR.BytesPerFileRecord;

                    // Check if index can be found in current DataRun
                    if (index < (int)DataRunRecords)
                    {
                        ulong recordOffset = ((ulong)dr.StartCluster * (ulong)VBR.BytesPerCluster) + ((ulong)index * (ulong)VBR.BytesPerFileRecord);
                        byte[] recordBytesRaw = Helper.readDrive(streamToRead, recordOffset, (ulong)VBR.BytesPerFileRecord);

                        ApplyFixup(ref recordBytesRaw);

                        return recordBytesRaw;
                    }

                    // Decrement index for the number of FileRecords in the current DataRun
                    else
                    {
                        index -= ((int)dr.ClusterLength * (int)VBR.BytesPerCluster) / (int)VBR.BytesPerFileRecord;
                    }
                }
                throw new Exception("Could not find the FileRecord requested...");
            }
        }

        #endregion GetRecordBytesMethod

        #region GetContentBytesMethods

        public static byte[] GetContentBytes(string path)
        {
            return GetContentBytes(path, "");
        }

        public static byte[] GetContentBytes(string path, string streamName)
        {
            FileRecord record = Get(path, true);
            return record.GetTestContent(streamName);
        }

        #endregion GetContentBytesMethods

        private static void ApplyFixup(ref byte[] bytes)
        {
            // Take UpdateSequence into account
            ushort usoffset = BitConverter.ToUInt16(bytes, 4);
            ushort ussize = BitConverter.ToUInt16(bytes, 6);

            if (ussize != 0)
            {
                byte[] UpdateSequenceArray = Helper.GetSubArray(bytes, (usoffset + 2), (2 * ussize));

                bytes[0x1FE] = UpdateSequenceArray[0];
                bytes[0x1FF] = UpdateSequenceArray[1];
                bytes[0x3FE] = UpdateSequenceArray[2];
                bytes[0x3FF] = UpdateSequenceArray[3];
            }
        }

        #endregion StaticMethods

        #region InstanceMethods

        // TODO: Add Encoding parameter
        // TODO: Add DataStream parameter
        #region GetContentMethods

        public byte[] GetContent()
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    if (attr.NameString == "")
                    {
                        if (attr.NonResident)
                        {
                            return (attr as NonResident).GetBytes(this.VolumePath);
                        }
                        else
                        {
                            return (attr as Data).RawData;
                        }
                    }
                }
                else if (attr.Name == FileRecordAttribute.ATTR_TYPE.ATTRIBUTE_LIST)
                {
                    AttributeList attrlist = attr as AttributeList;
                    foreach (AttrRef ar in attrlist.AttributeReference)
                    {
                        if (ar.Name == "DATA")
                        {
                            if (ar.NameString == "")
                            {
                                FileRecord record = new FileRecord(FileRecord.GetRecordBytes(this.VolumePath, (int)ar.RecordNumber), this.VolumePath, true);
                                return record.GetContent();
                            }
                        }
                    }
                }
            }
            throw new Exception("Could not locate file contents");
        }

        public byte[] GetContent(string StreamName)
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    if (attr.NameString == StreamName)
                    {
                        if (attr.NonResident)
                        {
                            return (attr as NonResident).GetBytes(this.VolumePath);
                        }
                        else
                        {
                            return (attr as Data).RawData;
                        }
                    }
                }
                /*else if (attr.Name == Attr.ATTR_TYPE.ATTRIBUTE_LIST)
                {
                    AttributeList attrlist = attr as AttributeList;
                    foreach (AttrRef ar in attrlist.AttributeReference)
                    {
                        if (ar.Name == "DATA")
                        {
                            if (attr.NameString == StreamName)
                            {
                                FileRecord record = new FileRecord(FileRecord.GetRecordBytes(this.VolumePath, (int)ar.RecordNumber), this.VolumePath, true);
                                return record.GetContent(StreamName);
                            }
                        }
                    }
                }*/
            }
            throw new Exception("Could not locate desired stream");
        }

        internal byte[] GetContent(VolumeBootRecord VBR)
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    if (attr.NonResident)
                    {
                        return (attr as NonResident).GetBytes(this.VolumePath, VBR);
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
                            FileRecord record = new FileRecord(FileRecord.GetRecordBytes(this.VolumePath, (int)ar.RecordNumber), this.VolumePath, true);
                            return record.GetContent();
                        }
                    }
                }
            }
            throw new Exception("Could not locate file contents");
        }

        #endregion GetContentMethods

        #region GetTestContentMethods

        public byte[] GetTestContent()
        {
            return GetTestContent("");
        }

        public byte[] GetTestContent(string streamName)
        {
            foreach (FileRecordAttribute attr in this.Attribute)
            {
                if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                {
                    if (attr.NameString.ToUpper() == streamName.ToUpper())
                    {
                        if (attr.NonResident)
                        {
                            return (attr as NonResident).GetBytes(this.VolumePath);
                        }
                        else
                        {
                            return (attr as Data).RawData;
                        }
                    }
                }

                AttributeList attrList = attr as AttributeList;
                if (attrList != null)
                {
                    foreach (AttrRef ar in attrList.AttributeReference)
                    {
                        if (ar.Name == "DATA")
                        {
                            FileRecord record = new FileRecord(FileRecord.GetRecordBytes(this.VolumePath, (int)ar.RecordNumber), this.VolumePath, true);
                            return record.GetTestContent(streamName);
                        }
                    }
                }
            }
            throw new Exception("Could not locate desired stream");
        }

        #endregion GetTestContentMethods

        #region CopyFileMethods

        public void CopyFile(string Destination)
        {
            this.CopyFile(Destination, "");
        }

        public void CopyFile(string Destination, string StreamName)
        {
            byte[] fileBytes = this.GetContent(StreamName);

            // Open file for writing
            FileStream streamToWrite = new FileStream(Destination, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            // Writes a block of bytes to this stream using data from a byte array.
            streamToWrite.Write(fileBytes, 0, fileBytes.Length);

            // Close file stream
            streamToWrite.Close();
        }

        #endregion CopyFileMethods

        #region GetChildMethods

        public IndexEntry[] GetChild()
        {
            return IndexEntry.GetInstances(this.FullName);
        }

        #endregion GetChildMethods

        #region GetParentMethods

        public FileRecord GetParent()
        {
            return FileRecord.Get(this.VolumePath, (int)this.ParentRecordNumber, false);
        }

        #endregion GetParentMethods

        #region GetUsnJrnlMethods

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

        #endregion GetUsnJrnlMethods

        #region GetSlackMethods

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
                                return (attr as NonResident).GetSlack(this.VolumePath);
                            }
                            else
                            {
                                return null;
                            }
                        }
                        /*else if (attr.Name == Attr.ATTR_TYPE.ATTRIBUTE_LIST)
                        {
                            AttributeList attrlist = attr as AttributeList;
                            foreach (AttrRef ar in attrlist.AttributeReference)
                            {
                                if (ar.Name == "DATA")
                                {
                                    FileRecord record = new FileRecord(FileRecord.GetRecordBytes(this.VolumePath, (int)ar.RecordNumber), this.VolumePath, true);
                                    return record.GetSlack();
                                }
                            }
                        }*/
                    }
                }
            }
            return null;
        }

        #endregion GetSlackMethods

        #region GetMftSlackMethods

        public byte[] GetMftSlack()
        {
            byte[] bytes = FileRecord.GetRecordBytes(this.VolumePath, (int)this.RecordNumber);
            return Helper.GetSubArray(bytes, (int)this.RealSize - 1, (int)this.AllocatedSize - (int)this.RealSize);
        }

        #endregion GetMftSlackMethods

        #region GetHashMethods

        public string GetHash(string algorithm)
        {
            return PowerForensics.Utilities.Hash.Get(this.GetContent(), algorithm);
        }

        public string GetHash(string algorithm, string stream)
        {
            return PowerForensics.Utilities.Hash.Get(this.GetContent(stream), algorithm);
        }

        #endregion GetHashMethods

        #region ToStringOverride

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

        #endregion ToStringOverride

        #endregion InstanceMethods
















        #region Test

        #region TestConstructor

        private FileRecord(ref FileRecord[] array, byte[] bytes, int offset, string volume, uint bytesPerFileRecord)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00 + offset, 0x04);

            if (Signature == "FILE")
            {
                OffsetOfUS = BitConverter.ToUInt16(bytes, 0x04 + offset);
                SizeOfUS = BitConverter.ToUInt16(bytes, 0x06 + offset);
                LogFileSequenceNumber = BitConverter.ToUInt64(bytes, 0x08 + offset);
                SequenceNumber = BitConverter.ToUInt16(bytes, 0x10 + offset);
                Hardlinks = BitConverter.ToUInt16(bytes, 0x12 + offset);
                OffsetOfAttribute = BitConverter.ToUInt16(bytes, 0x14 + offset);
                Flags = BitConverter.ToUInt16(bytes, 0x16 + offset);
                Deleted = IsDeleted(Flags);
                Directory = IsDirectory(Flags);
                RealSize = BitConverter.ToUInt32(bytes, 0x18 + offset);
                AllocatedSize = BitConverter.ToUInt32(bytes, 0x1C + offset);
                ReferenceToBase = BitConverter.ToUInt64(bytes, 0x20 + offset);
                NextAttrId = BitConverter.ToUInt16(bytes, 0x28 + offset);
                RecordNumber = BitConverter.ToUInt32(bytes, 0x2C + offset);
                
                #region Attribute

                // Instantiate an empty list of Attr Objects (We don't know how many attributes the record contains)
                List<FileRecordAttribute> AttributeList = new List<FileRecordAttribute>();
                int currentOffset = offset + OffsetOfAttribute;

                do
                {
                    FileRecordAttribute attr = FileRecordAttribute.GetTest(bytes, currentOffset, volume);

                    if (attr != null)
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

                        AttributeList.Add(attr);
                    }

                    // Get attribute size
                    int attrSizeOffset = currentOffset + 0x04;
                    int attrSize = BitConverter.ToInt32(bytes, attrSizeOffset);

                    if (attrSize == 0)
                    {
                        break;
                    }

                    // Increment currentOffset
                    currentOffset += attrSize;

                } while ((currentOffset - offset) < bytesPerFileRecord);

                Attribute = AttributeList.ToArray();

                #endregion Attribute
                
                /*#region FullName

                StringBuilder sb = new StringBuilder();

                // Record 5 is the root of the drive
                if (RecordNumber == 5)
                {
                    sb.Append(Helper.GetVolumeLetter(volume));
                }
                else
                {
                    // Derive Path by looking at ParentRecord's FullName
                    if (array[(int)ParentRecordNumber] != null)
                    {
                        if (ParentSequenceNumber == array[(int)ParentRecordNumber].SequenceNumber)
                        {
                            sb.Append(array[(int)ParentRecordNumber].FullName);
                        }
                        else
                        {
                            sb.Append(@"$OrphanFiles\");
                        }
                    }

                    // If record for Parent does not already exist then instantiate it and add it to the array
                    else
                    {
                        int parentOffset = (int)bytesPerFileRecord * (int)ParentRecordNumber;

                        array[(int)ParentRecordNumber] = new FileRecord(ref array, bytes, parentOffset, volume, bytesPerFileRecord);

                        if (ParentSequenceNumber == array[(int)ParentRecordNumber].SequenceNumber)
                        {
                            sb.Append(array[(int)ParentRecordNumber].FullName);
                        }
                        else
                        {
                            sb.Append(@"$OrphanFiles\");
                        }
                    }

                    // Add file name to end of path
                    sb.Append(Name);
                }

                // Add trailing \ to any file that is a directory
                if (Directory)
                {
                    sb.Append('\\');
                }

                // Figure out a way to have record 15 not have a name of $MFT...

                FullName = sb.ToString();

                #endregion FullName*/
            }
        }

        #endregion TestConstructor

        public static FileRecord[] GetInstancesTest(string volume)
        {
            FileRecord record = FileRecord.Get(volume, MftIndex.MFT_INDEX, true);
            byte[] mftBytes = record.GetContent();
            return GetInstancesTest(mftBytes, volume);
        }

        private static FileRecord[] GetInstancesTest(byte[] bytes, string volume)
        {
            // Determine the size of an MFT File Record
            int bytesPerFileRecord = (int)(VolumeBootRecord.Get(volume)).BytesPerFileRecord;

            // Calulate the number of entries in the MFT
            int fileCount = bytes.Length / bytesPerFileRecord;

            // Instantiate an array of FileRecord objects
            FileRecord[] recordArray = new FileRecord[fileCount];

            // Now we need to iterate through all possible index values
            for (int index = 0; index < fileCount; index++)
            {
                // Check if current record has been instantiated
                if (recordArray[index] == null)
                {
                    int offset = index * bytesPerFileRecord;

                    // Take UpdateSequence into account
                    ApplyFixupTest(ref bytes, offset);

                    // Instantiate FileRecord object
                    recordArray[index] = new FileRecord(ref recordArray, bytes, offset, volume, (uint)bytesPerFileRecord);
                }
            }
            return recordArray;
        }

        private static void ApplyFixupTest(ref byte[] bytes, int offset)
        {
            // Take UpdateSequence into account
            ushort usoffset = BitConverter.ToUInt16(bytes, 0x04 + offset);
            ushort ussize = BitConverter.ToUInt16(bytes, 0x06 + offset);

            if (ussize != 0)
            {
                byte[] UpdateSequenceArray = Helper.GetSubArray(bytes, ((usoffset + 2) + offset), (2 * ussize));

                bytes[0x1FE + offset] = UpdateSequenceArray[0];
                bytes[0x1FF + offset] = UpdateSequenceArray[1];
                bytes[0x3FE + offset] = UpdateSequenceArray[2];
                bytes[0x3FF + offset] = UpdateSequenceArray[3];
            }
        }

        private static bool IsDirectory(ushort Flags)
        {
            if ((Flags & (ushort)FILE_RECORD_FLAG.DIR) == (ushort)FILE_RECORD_FLAG.DIR)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsDeleted(ushort Flags)
        {
            if ((Flags & (ushort)FILE_RECORD_FLAG.INUSE) == (ushort)FILE_RECORD_FLAG.INUSE)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion Test

    }

    #endregion FileRecordClass
}