using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    #region UsnJrnlClass

    public class UsnJrnl
    {
        #region Enums

        [FlagsAttribute]
        public enum USN_REASON : uint
        {
            DATA_OVERWRITE = 0x00000001,
            DATA_EXTEND = 0x00000002,
            DATA_TRUNCATION = 0x00000004,
            NAMED_DATA_OVERWRITE = 0x00000010,
            NAMED_DATA_EXTEND = 0x00000020,
            NAMED_DATA_TRUNCATION = 0x00000040,
            FILE_CREATE = 0x00000100,
            FILE_DELETE = 0x00000200,
            EA_CHANGE = 0x00000400,
            SECURITY_CHANGE = 0x00000800,
            RENAME_OLD_NAME = 0x00001000,
            RENAME_NEW_NAME = 0x00002000,
            INDEXABLE_CHANGE = 0x00004000,
            BASIC_INFO_CHANGE = 0x00008000,
            HARD_LINK_CHANGE = 0x00010000,
            COMPRESSION_CHANGE = 0x00020000,
            ENCRYPTION_CHANGE = 0x00040000,
            OBJECT_ID_CHANGE = 0x00080000,
            REPARSE_POINT_CHANGE = 0x00100000,
            STREAM_CHANGE = 0x00200000,
            CLOSE = 0x80000000
        }

        [FlagsAttribute]
        public enum USN_SOURCE : uint
        {
            DATA_MANAGEMENT = 0x00000001,
            AUXILIARY_DATA = 0x00000002,
            REPLICATION_MANAGEMENT = 0x00000004
        }

        #endregion Enums

        #region Properties

        private readonly static Version USN40Version = new Version(4, 0);
        
        public readonly string VolumePath;
        public readonly Version Version;
        public readonly ulong RecordNumber;
        public readonly ushort FileSequenceNumber;
        public readonly ulong ParentFileRecordNumber;
        public readonly ushort ParentFileSequenceNumber;
        public readonly ulong Usn;
        public readonly DateTime TimeStamp;
        public readonly USN_REASON Reason;
        public readonly USN_SOURCE SourceInfo;
        public readonly uint SecurityId;
        public readonly StandardInformation.ATTR_STDINFO_PERMISSION FileAttributes;
        public readonly string FileName;

        #endregion Properties

        #region Constructors

        private UsnJrnl(byte[] bytes, string volume, ref int offset)
        {
            uint RecordLength = RecordLength = BitConverter.ToUInt32(bytes, (0x00 + offset));
            VolumePath = volume;
            Version = new System.Version(BitConverter.ToUInt16(bytes, (0x04 + offset)), BitConverter.ToUInt16(bytes, (0x06 + offset)));
            RecordNumber = (BitConverter.ToUInt64(bytes, (0x08 + offset)) & 0x0000FFFFFFFFFFFF);
            FileSequenceNumber = ParentFileSequenceNumber = BitConverter.ToUInt16(bytes, (0x0E + offset));
            ParentFileRecordNumber = (BitConverter.ToUInt64(bytes, (0x10 + offset)) & 0x0000FFFFFFFFFFFF);
            ParentFileSequenceNumber = BitConverter.ToUInt16(bytes, (0x16 + offset));
            Usn = BitConverter.ToUInt64(bytes, (0x18 + offset));
            TimeStamp = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, (0x20 + offset)));
            Reason = ((USN_REASON)BitConverter.ToUInt32(bytes, (0x28 + offset)));
            SourceInfo = ((USN_SOURCE)BitConverter.ToUInt32(bytes, (0x2C + offset)));
            SecurityId = BitConverter.ToUInt32(bytes, (0x30 + offset));
            FileAttributes = ((StandardInformation.ATTR_STDINFO_PERMISSION)BitConverter.ToUInt32(bytes, (0x34 + offset)));
            ushort fileNameLength = BitConverter.ToUInt16(bytes, (0x38 + offset));
            ushort fileNameOffset = BitConverter.ToUInt16(bytes, (0x3A + offset));
            FileName = Encoding.Unicode.GetString(bytes, 0x3C + offset, fileNameLength); 
            offset += (int)RecordLength;
        }

        #endregion Constructors

        #region StaticMethods

        #region GetMethods

        public static UsnJrnl Get(string volume, ulong usn)
        {
            Helper.getVolumeName(ref volume);
            return GetByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl", usn);
        }

        public static UsnJrnl GetByPath(string path, ulong usn)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return Get(volume, (int)entry.RecordNumber, usn);
        }

        private static UsnJrnl Get(string volume, int recordnumber, ulong usn)
        {
            // Check for valid Volume name
            Helper.getVolumeName(ref volume);

            // Set up FileStream to read volume
            FileStream streamToRead = Helper.getFileStream(volume);

            // Get VolumeBootRecord object for logical addressing
            VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

            FileRecord record = FileRecord.Get(volume, recordnumber, true);

            // Get the $J Data attribute (contains UsnJrnl details
            NonResident J = UsnJrnl.GetJStream(record);

            // Determine the length of the initial sparse pages
            ulong SparseLength = (ulong)J.DataRun[0].ClusterLength * VBR.BytesPerCluster;

            if (usn > SparseLength)
            {
                // Subtract length of sparse data from desired usn offset
                ulong usnOffset = usn - SparseLength;

                // Iterate through each data run
                for (int i = 1; i < J.DataRun.Length; i++)
                {
                    // Determine length of current DataRun
                    ulong dataRunLength = (ulong)J.DataRun[i].ClusterLength * VBR.BytesPerCluster;

                    // Check if usnOffset resides in current DataRun
                    if (dataRunLength <= usnOffset)
                    {
                        // If not, subtract length of DataRun from usnOffset
                        usnOffset -= dataRunLength;
                    }

                    // If usnOffset resides within DataRun, parse associated UsnJrnl Entry
                    else
                    {
                        // Read DataRun from disk
                        byte[] fragmentBytes = Helper.readDrive(streamToRead, ((ulong)J.DataRun[i].StartCluster * VBR.BytesPerCluster), ((ulong)J.DataRun[i].ClusterLength * VBR.BytesPerCluster));

                        // Instatiate a byte array that is the size of a single cluster
                        byte[] clusterBytes = new byte[VBR.BytesPerCluster];

                        // Iterate through the clusters in the DataRun
                        for (int j = 0; j < J.DataRun[i].ClusterLength; j++)
                        {
                            // If usnOffset is not in current cluster, then subtract cluster size from offset and iterate
                            if (VBR.BytesPerCluster <= usnOffset)
                            {
                                usnOffset -= VBR.BytesPerCluster;
                            }
                            // Else if usnOffset is in current cluster
                            else
                            {
                                // Copy current cluster bytes to clusterBytes variable
                                Array.Copy(fragmentBytes, (int)(j * VBR.BytesPerCluster), clusterBytes, 0, clusterBytes.Length);

                                // Parse desired UsnJrnl entry from cluster
                                int offset = (int)usnOffset;
                                return new UsnJrnl(clusterBytes, volume, ref offset);
                            }
                        }
                    }
                }
                return null;
            }
            else
            {
                throw new Exception("UsnJrnl entry has has been overwritten");
            }
        }

        #endregion GetMethods

        #region GetInstancesMethods

        public static UsnJrnl[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            IndexEntry entry = IndexEntry.Get(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
            return GetInstances(volume, (int)entry.RecordNumber);
        }

        public static UsnJrnl[] GetInstancesByPath(string path)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return GetInstances(volume, (int)entry.RecordNumber);
        }

        public static UsnJrnl[] GetTestInstances(string path)
        {
            byte[] bytes = FileRecord.GetContentBytes(path, "$J");
            string volume = Helper.GetVolumeFromPath(path);

            VolumeBootRecord VBR = VolumeBootRecord.Get(volume);
            List<UsnJrnl> usnList = new List<UsnJrnl>();

            for(int i = 0;  i < bytes.Length; i += (int)VBR.BytesPerCluster)
            {
                int clusteroffset = i;

                do
                {
                    // Break if there are no more UsnJrnl entries in cluster
                    if (bytes[clusteroffset] == 0)
                    {
                        break;
                    }

                    try
                    {
                        UsnJrnl usn = new UsnJrnl(bytes, volume, ref clusteroffset);
                        if (usn.Version > USN40Version)
                        {
                            break;
                        }
                        usnList.Add(usn);
                    }
                    catch
                    {
                        break;
                    }

                } while (clusteroffset >= 0 && clusteroffset < bytes.Length);
            }
            return usnList.ToArray();
        }

        private static UsnJrnl[] GetInstances(string volume, int recordnumber)
        {
            // Set up FileStream to read volume
            FileStream streamToRead = Helper.getFileStream(volume);

            // Get VolumeBootRecord object for logical addressing
            VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

            FileRecord record = FileRecord.Get(volume, recordnumber, true);

            // Get the $J Data attribute (contains UsnJrnl details
            NonResident J = UsnJrnl.GetJStream(record);

            List<UsnJrnl> usnList = new List<UsnJrnl>();

            for (int i = 0; i < J.DataRun.Length; i++)
            {
                if (!(J.DataRun[i].Sparse))
                {
                    long clusterCount = J.DataRun[i].ClusterLength;

                    byte[] fragmentBytes = Helper.readDrive(streamToRead, ((ulong)J.DataRun[i].StartCluster * VBR.BytesPerCluster), ((ulong)clusterCount * VBR.BytesPerCluster));

                    byte[] clusterBytes = new byte[VBR.BytesPerCluster];

                    for (int j = 0; j < clusterCount; j++)
                    {
                        Array.Copy(fragmentBytes, (int)(j * VBR.BytesPerCluster), clusterBytes, 0, clusterBytes.Length);

                        int offset = 0;

                        do
                        {
                            if (clusterBytes[offset] == 0)
                            {
                                break;
                            }

                            try
                            {
                                UsnJrnl usn = new UsnJrnl(clusterBytes, volume, ref offset);
                                if (usn.Version > USN40Version)
                                {
                                    break;
                                }
                                usnList.Add(usn);
                            }
                            catch
                            {
                                break;
                            }

                        } while (offset >= 0 && offset < clusterBytes.Length);
                    }
                }
            }
            return usnList.ToArray();
        }

        #endregion GetInstancesMethods

        internal static NonResident GetJStream(FileRecord fileRecord)
        {
            foreach (FileRecordAttribute attr in fileRecord.Attribute)
            {
                if (attr.NameString == "$J")
                {
                    return attr as NonResident;
                }
                
                AttributeList attrList = attr as AttributeList;
                if (attrList != null)
                {
                    foreach (AttrRef ar in attrList.AttributeReference)
                    {
                        if (ar.NameString == "$J")
                        {
                            FileRecord record = new FileRecord(FileRecord.GetRecordBytes(fileRecord.VolumePath, (int)ar.RecordNumber), fileRecord.VolumePath, true);
                            return GetJStream(record);
                        }
                    }
                }
            }
            throw new Exception("No $J attribute found.");
        }

        #endregion StaticMethods

        #region InstanceMethods

        public FileRecord GetFileRecord()
        {
            FileRecord record = FileRecord.Get(this.VolumePath, (int)this.RecordNumber, false);

            if (record.SequenceNumber == this.FileSequenceNumber)
            {
                return record;
            }
            else
            {
                throw new Exception("Desired FileRecord has been overwritten");
            }
        }

        public FileRecord GetParentFileRecord()
        {
            FileRecord record = FileRecord.Get(this.VolumePath, (int)this.ParentFileRecordNumber, false);

            if (record.SequenceNumber == this.ParentFileSequenceNumber)
            {
                return record;
            }
            else
            {
                throw new Exception("Desired FileRecord has been overwritten");
            }
        }

        #region ToStringOverride

        public override string ToString()
        {
            return String.Format("UsnJrnl for {0} ({1}) Reason: {2}", this.FileName, this.RecordNumber, this.Reason); ;
        }

        #endregion ToStringOverride

        #endregion InstanceMethods
    }

    #endregion USNJrnlClass

    #region UsnJrnlDetailClass

    public class UsnJrnlDetail
    {
        #region Properties

        public ulong MaxSize;
        public ulong AllocationDelta;
        public ulong UsnId;
        public ulong LowestUsn;

        #endregion Properties

        #region Constructors

        private UsnJrnlDetail(byte[] maxBytes)
        {
            MaxSize = BitConverter.ToUInt64(maxBytes, 0x00);
            AllocationDelta = BitConverter.ToUInt64(maxBytes, 0x08);
            UsnId = BitConverter.ToUInt64(maxBytes, 0x10);
            LowestUsn = BitConverter.ToUInt64(maxBytes, 0x18);
        }

        #endregion Constructors

        #region StaticMethods

        #region GetMethod

        public static UsnJrnlDetail Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
        }
        
        public static UsnJrnlDetail GetByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return new UsnJrnlDetail(record.GetContent(@"$Max"));
        }

        #endregion GetMethod

        #region GetBytesMethod

        public static byte[] GetBytes(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetBytesByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
        }

        public static byte[] GetBytesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return record.GetContent(@"$Max");
        }

        #endregion GetBytesMethod

        #endregion StaticMethods
    }

    #endregion UsnJrnlDetailClass
}
