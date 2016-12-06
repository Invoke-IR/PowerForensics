using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using PowerForensics.FileSystems;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class UsnJrnl
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum USN_REASON : uint
        {
            /// <summary>
            /// 
            /// </summary>
            DATA_OVERWRITE = 0x00000001,

            /// <summary>
            /// 
            /// </summary>
            DATA_EXTEND = 0x00000002,

            /// <summary>
            /// 
            /// </summary>
            DATA_TRUNCATION = 0x00000004,

            /// <summary>
            /// 
            /// </summary>
            NAMED_DATA_OVERWRITE = 0x00000010,

            /// <summary>
            /// 
            /// </summary>
            NAMED_DATA_EXTEND = 0x00000020,

            /// <summary>
            /// 
            /// </summary>
            NAMED_DATA_TRUNCATION = 0x00000040,

            /// <summary>
            /// 
            /// </summary>
            FILE_CREATE = 0x00000100,

            /// <summary>
            /// 
            /// </summary>
            FILE_DELETE = 0x00000200,

            /// <summary>
            /// 
            /// </summary>
            EA_CHANGE = 0x00000400,

            /// <summary>
            /// 
            /// </summary>
            SECURITY_CHANGE = 0x00000800,

            /// <summary>
            /// 
            /// </summary>
            RENAME_OLD_NAME = 0x00001000,

            /// <summary>
            /// 
            /// </summary>
            RENAME_NEW_NAME = 0x00002000,

            /// <summary>
            /// 
            /// </summary>
            INDEXABLE_CHANGE = 0x00004000,

            /// <summary>
            /// 
            /// </summary>
            BASIC_INFO_CHANGE = 0x00008000,

            /// <summary>
            /// 
            /// </summary>
            HARD_LINK_CHANGE = 0x00010000,

            /// <summary>
            /// 
            /// </summary>
            COMPRESSION_CHANGE = 0x00020000,

            /// <summary>
            /// 
            /// </summary>
            ENCRYPTION_CHANGE = 0x00040000,

            /// <summary>
            /// 
            /// </summary>
            OBJECT_ID_CHANGE = 0x00080000,

            /// <summary>
            /// 
            /// </summary>
            REPARSE_POINT_CHANGE = 0x00100000,

            /// <summary>
            /// 
            /// </summary>
            STREAM_CHANGE = 0x00200000,

            /// <summary>
            /// 
            /// </summary>
            CLOSE = 0x80000000
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum USN_SOURCE : uint
        {
            /// <summary>
            /// 
            /// </summary>
            DATA_MANAGEMENT = 0x00000001,

            /// <summary>
            /// 
            /// </summary>
            AUXILIARY_DATA = 0x00000002,

            /// <summary>
            /// 
            /// </summary>
            REPLICATION_MANAGEMENT = 0x00000004
        }

        #endregion Enums

        #region Properties

        private readonly static Version USN40Version = new Version(4, 0);

        /// <summary>
        /// 
        /// </summary>
        public readonly string VolumePath;

        /// <summary>
        /// 
        /// </summary>
        public readonly Version Version;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort FileSequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong ParentFileRecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ParentFileSequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly long Usn;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime TimeStamp;

        /// <summary>
        /// 
        /// </summary>
        public readonly USN_REASON Reason;

        /// <summary>
        /// 
        /// </summary>
        public readonly USN_SOURCE SourceInfo;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SecurityId;

        /// <summary>
        /// 
        /// </summary>
        public readonly StandardInformation.ATTR_STDINFO_PERMISSION FileAttributes;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FileName;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FullName;

        #endregion Properties

        #region Constructors

        private UsnJrnl(byte[] bytes, string volume, ref int offset)
        {
            int RecordLength = BitConverter.ToInt32(bytes, (0x00 + offset));
            VolumePath = volume;
            Version = new System.Version(BitConverter.ToUInt16(bytes, (0x04 + offset)), BitConverter.ToUInt16(bytes, (0x06 + offset)));
            RecordNumber = (BitConverter.ToUInt64(bytes, (0x08 + offset)) & 0x0000FFFFFFFFFFFF);
            FileSequenceNumber = ParentFileSequenceNumber = BitConverter.ToUInt16(bytes, (0x0E + offset));
            ParentFileRecordNumber = (BitConverter.ToUInt64(bytes, (0x10 + offset)) & 0x0000FFFFFFFFFFFF);
            ParentFileSequenceNumber = BitConverter.ToUInt16(bytes, (0x16 + offset));
            Usn = BitConverter.ToInt64(bytes, (0x18 + offset));
            TimeStamp = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, (0x20 + offset)));
            Reason = ((USN_REASON)BitConverter.ToUInt32(bytes, (0x28 + offset)));
            SourceInfo = ((USN_SOURCE)BitConverter.ToUInt32(bytes, (0x2C + offset)));
            SecurityId = BitConverter.ToUInt32(bytes, (0x30 + offset));
            FileAttributes = ((StandardInformation.ATTR_STDINFO_PERMISSION)BitConverter.ToUInt32(bytes, (0x34 + offset)));
            ushort fileNameLength = BitConverter.ToUInt16(bytes, (0x38 + offset));
            ushort fileNameOffset = BitConverter.ToUInt16(bytes, (0x3A + offset));
            FileName = Encoding.Unicode.GetString(bytes, 0x3C + offset, fileNameLength); 
            offset += RecordLength;
            //FullName = this.GetFileRecord().FullName;
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="usn"></param>
        /// <returns></returns>
        public static UsnJrnl Get(string volume, long usn)
        {
            Helper.getVolumeName(ref volume);
            return GetByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl", usn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="usn"></param>
        /// <returns></returns>
        public static UsnJrnl GetByPath(string path, long usn)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return Get(volume, (int)entry.RecordNumber, usn);
        }

        private static UsnJrnl Get(string volume, int recordnumber, long usn)
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
            long SparseLength = J.DataRun[0].ClusterLength * VBR.BytesPerCluster;

            if (usn > SparseLength)
            {
                // Subtract length of sparse data from desired usn offset
                long usnOffset = usn - SparseLength;

                // Iterate through each data run
                for (int i = 1; i < J.DataRun.Length; i++)
                {
                    // Determine length of current DataRun
                    long dataRunLength = J.DataRun[i].ClusterLength * VBR.BytesPerCluster;

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
                        byte[] fragmentBytes = Helper.readDrive(streamToRead, (J.DataRun[i].StartCluster * VBR.BytesPerCluster), (J.DataRun[i].ClusterLength * VBR.BytesPerCluster));

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static UsnJrnl[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            IndexEntry entry = IndexEntry.Get(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
            return GetInstances(volume, (int)entry.RecordNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UsnJrnl[] GetInstancesByPath(string path)
        {
            string volume = Helper.GetVolumeFromPath(path);
            IndexEntry entry = IndexEntry.Get(path);
            return GetInstances(volume, (int)entry.RecordNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UsnJrnl[] GetTestInstances(string path)
        {
            byte[] bytes = FileRecord.GetContentBytes(path, "$J");

            string volume = Helper.GetVolumeFromPath(path);

            VolumeBootRecord VBR = VolumeBootRecord.Get(volume);

            List<UsnJrnl> usnList = new List<UsnJrnl>();

            for(int i = 0;  i < bytes.Length; i += VBR.BytesPerCluster)
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
            // Get VolumeBootRecord object for logical addressing
            VolumeBootRecord VBR = VolumeBootRecord.Get(volume);

            // Get FileRecord for C:\$Extend\$UsnJrnl
            FileRecord record = FileRecord.Get(volume, recordnumber, true);

            // Get the $J Data attribute (contains UsnJrnl records)
            NonResident J = UsnJrnl.GetJStream(record);

            // Instatiate a List of UsnJrnl entries
            List<UsnJrnl> usnList = new List<UsnJrnl>();

            for (int i = 0; i < J.DataRun.Length; i++)
            {
                if (!(J.DataRun[i].Sparse))
                {
                    long clusterCount = J.DataRun[i].ClusterLength;

                    byte[] fragmentBytes = Helper.readDrive(volume, (J.DataRun[i].StartCluster * VBR.BytesPerCluster), (clusterCount * VBR.BytesPerCluster));

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

            // Return usnList as a UsnJrnl[]
            return usnList.ToArray();
        }

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
                            FileRecord record = FileRecord.Get(fileRecord.VolumePath, (int)ar.RecordNumber, true);
                            return GetJStream(record);
                        }
                    }
                }
            }
            throw new Exception("No $J attribute found.");
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("UsnJrnl for {0} ({1}) Reason: {2}", this.FileName, this.RecordNumber, this.Reason); ;
        }

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class UsnJrnlInformation
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ulong MaxSize;

        /// <summary>
        /// 
        /// </summary>
        public ulong AllocationDelta;

        /// <summary>
        /// 
        /// </summary>
        public ulong UsnId;

        /// <summary>
        /// 
        /// </summary>
        public ulong LowestUsn;

        #endregion Properties

        #region Constructors

        private UsnJrnlInformation(byte[] maxBytes)
        {
            MaxSize = BitConverter.ToUInt64(maxBytes, 0x00);
            AllocationDelta = BitConverter.ToUInt64(maxBytes, 0x08);
            UsnId = BitConverter.ToUInt64(maxBytes, 0x10);
            LowestUsn = BitConverter.ToUInt64(maxBytes, 0x18);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static UsnJrnlInformation Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UsnJrnlInformation GetByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return new UsnJrnlInformation(record.GetContent(@"$Max"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetBytesByPath(Helper.GetVolumeLetter(volume) + @"\$Extend\$UsnJrnl");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetBytesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return record.GetContent(@"$Max");
        }

        #endregion Static Methods
    }
}