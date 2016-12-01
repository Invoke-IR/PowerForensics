using System;
using System.IO;
using System.Text;
using PowerForensics.Generic;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class LogFile
    {
        #region Static Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        internal static FileRecord GetFileRecord(string volume)
        {
            return FileRecord.Get(volume, MftIndex.LOGFILE_INDEX, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileRecord"></param>
        /// <returns></returns>
        internal static NonResident GetDataAttr(FileRecord fileRecord)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static byte[] getBytes(string volume)
        {   
            // Get filestream based on hVolume
            using (FileStream streamToRead = Helper.getFileStream(volume))
            {
                VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

                FileRecord logFileRecord = GetFileRecord(volume);

                NonResident data = GetDataAttr(logFileRecord);

                return data.GetBytes();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static OperationRecord Get(string volume)
        {
            Helper.getVolumeName(ref volume);

            // Get Page Bytes
            byte[]pageBytes = getBytes(volume);

            // Get Page Header
            PageHeader pageHeader = new PageHeader(Helper.GetSubArray(pageBytes, 0x00, 0x40));
            return new OperationRecord(Helper.GetSubArray(pageBytes, 0x40, 0x58));
            
            /*return pageHeader;
            
            for(int i = 0x40; i < (pageBytes.Length - (0x40 + 0x58)); i += 0x58)
            {
                new
            }*/
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class RestartAreaHeader
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Signature;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort USOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort USCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong CheckDiskLSN;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SystemPageSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LogPageSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort RestartOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MinorVersion;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MajorVersion;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] USArray;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong CurrentLSN;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LogClient;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClientList;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong Flags;

        #endregion Properties

        #region Constructors

        internal RestartAreaHeader(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x04);
            
            if (Signature == "RSTR")
            {
                USOffset = BitConverter.ToUInt16(bytes, 0x04);
                USCount = BitConverter.ToUInt16(bytes, 0x06);
                CheckDiskLSN = BitConverter.ToUInt64(bytes, 0x08);
                SystemPageSize = BitConverter.ToUInt32(bytes, 0x10);
                LogPageSize = BitConverter.ToUInt32(bytes, 20);
                RestartOffset = BitConverter.ToUInt16(bytes, 0x18);
                MinorVersion = BitConverter.ToUInt16(bytes, 26);
                MajorVersion = BitConverter.ToUInt16(bytes, 28);
                //USArray = bytes.Skip(30).Take(18).ToArray();
                CurrentLSN = BitConverter.ToUInt64(bytes, 48);
                LogClient = BitConverter.ToUInt32(bytes, 56);
                ClientList = BitConverter.ToUInt32(bytes, 60);
                Flags = BitConverter.ToUInt64(bytes, 0x40);
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static RestartAreaHeader[] Get(string volume)
        {
            return RestartAreaHeader.Get(LogFile.getBytes(volume));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal static RestartAreaHeader[] Get(byte[] bytes)
        {
            RestartAreaHeader[] headerArray = new RestartAreaHeader[0x02];
            headerArray[0] = new RestartAreaHeader(Helper.GetSubArray(bytes, 0x00, 0x1000));
            headerArray[1] = new RestartAreaHeader(Helper.GetSubArray(bytes, 0x1000, 0x1000));
            return headerArray;
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class Restart
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public RestartAreaHeader[] RestartHeader;

        #endregion Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        public Restart(byte[] bytes)
        {
            RestartHeader = RestartAreaHeader.Get(bytes);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Restart[] Get(byte[] bytes)
        {
            Restart[] restartArray = new Restart[2];

            restartArray[0] = new Restart(Helper.GetSubArray(bytes, 0x00, 0x1000));
            restartArray[1] = new Restart(Helper.GetSubArray(bytes, 0x1000, 0x1000));

            return restartArray;
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class PageHeader
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Signature;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort USOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort USCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong LastLSN;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort PageCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort PagePosition;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort NextRecordOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong LastEndLSN;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort USN;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] USArray;

        #endregion Properties

        #region Constructors

        internal PageHeader(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x04);
            
            if (Signature == "RCRD")
            {
                USOffset = BitConverter.ToUInt16(bytes, 4);
                USCount = BitConverter.ToUInt16(bytes, 6);
                LastLSN = BitConverter.ToUInt64(bytes, 8);
                Flags = BitConverter.ToUInt32(bytes, 16);
                PageCount = BitConverter.ToUInt16(bytes, 20);
                PagePosition = BitConverter.ToUInt16(bytes, 22);
                NextRecordOffset = BitConverter.ToUInt16(bytes, 24);
                LastLSN = BitConverter.ToUInt64(bytes, 32);
                //USN = ;
                //USArray = ;
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static PageHeader Get(string volume)
        {
            return new PageHeader(LogFile.getBytes(volume));
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class OperationRecord
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum OPERATION_CODE
        {
            /// <summary>
            /// 
            /// </summary>
            Noop = 0x00,

            /// <summary>
            /// 
            /// </summary>
            CompensationlogRecord = 0x01,

            /// <summary>
            /// 
            /// </summary>
            InitializeFileRecordSegment = 0x02,

            /// <summary>
            /// 
            /// </summary>
            DeallocateFileRecordSegment = 0x03,

            /// <summary>
            /// 
            /// </summary>
            WriteEndofFileRecordSegement = 0x04,

            /// <summary>
            /// 
            /// </summary>
            CreateAttribute = 0x05,

            /// <summary>
            /// 
            /// </summary>
            DeleteAttribute = 0x06,

            /// <summary>
            /// 
            /// </summary>
            UpdateResidentValue = 0x07,

            /// <summary>
            /// 
            /// </summary>
            UpdataeNonResidentValue = 0x08,

            /// <summary>
            /// 
            /// </summary>
            UpdateMappingPairs = 0x09,

            /// <summary>
            /// 
            /// </summary>
            DeleteDirtyClusters = 0x0A,

            /// <summary>
            /// 
            /// </summary>
            SetNewAttributeSizes = 0x0B,

            /// <summary>
            /// 
            /// </summary>
            AddindexEntryRoot = 0x0C,

            /// <summary>
            /// 
            /// </summary>
            DeleteindexEntryRoot = 0x0D,

            /// <summary>
            /// 
            /// </summary>
            AddIndexEntryAllocation = 0x0F,

            /// <summary>
            /// 
            /// </summary>
            SetIndexEntryVenAllocation = 0x12,

            /// <summary>
            /// 
            /// </summary>
            UpdateFileNameRoot = 0x13,

            /// <summary>
            /// 
            /// </summary>
            UpdateFileNameAllocation = 0x14,

            /// <summary>
            /// 
            /// </summary>
            SetBitsInNonresidentBitMap = 0x15,

            /// <summary>
            /// 
            /// </summary>
            ClearBitsInNonresidentBitMap = 0x16,

            /// <summary>
            /// 
            /// </summary>
            PrepareTransaction = 0x19,

            /// <summary>
            /// 
            /// </summary>
            CommitTransaction = 0x1A,

            /// <summary>
            /// 
            /// </summary>
            ForgetTransaction = 0x1B,

            /// <summary>
            /// 
            /// </summary>
            OpenNonresidentAttribute = 0x1C,

            /// <summary>
            /// 
            /// </summary>
            DirtyPageTableDump = 0x1F,

            /// <summary>
            /// 
            /// </summary>
            TransactionTableDump = 0x20,

            /// <summary>
            /// 
            /// </summary>
            UpdateRecordDataRoot = 0x21
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong LSN;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong PreviousLSN;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong ClientUndoLSN;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClientDataLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClientID;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RecordType;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint TransactionID;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly OPERATION_CODE RedoOP;

        /// <summary>
        /// 
        /// </summary>
        public readonly OPERATION_CODE UndoOP;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort RedoOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort RedoLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort UndoOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort UndoLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort TargetAttribute;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort LCNtoFollow;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort RecordOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort AttrOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MFTClusterIndex;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint TargetVCN;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint TargetLCN;

        #endregion Properties

        #region Constructors

        internal OperationRecord(byte[] bytes)
        {
            LSN = BitConverter.ToUInt64(bytes, 0);
            PreviousLSN = BitConverter.ToUInt64(bytes, 8);
            ClientUndoLSN = BitConverter.ToUInt64(bytes, 16);
            ClientDataLength = BitConverter.ToUInt32(bytes, 24);
            ClientID = BitConverter.ToUInt32(bytes, 28);
            RecordType = BitConverter.ToUInt32(bytes, 32);
            TransactionID = BitConverter.ToUInt32(bytes, 36);
            Flags = BitConverter.ToUInt16(bytes, 40);
            RedoOP = (OPERATION_CODE)BitConverter.ToUInt16(bytes, 48);
            UndoOP = (OPERATION_CODE)BitConverter.ToUInt16(bytes, 50);
            RedoOffset = BitConverter.ToUInt16(bytes, 52);
            RedoLength = BitConverter.ToUInt16(bytes, 54);
            UndoOffset = BitConverter.ToUInt16(bytes, 56);
            UndoLength = BitConverter.ToUInt16(bytes, 58);
            TargetAttribute = BitConverter.ToUInt16(bytes, 60);
            LCNtoFollow = BitConverter.ToUInt16(bytes, 62);
            RecordOffset = BitConverter.ToUInt16(bytes, 64);
            AttrOffset = BitConverter.ToUInt16(bytes, 66);
            MFTClusterIndex = BitConverter.ToUInt16(bytes, 68);
            TargetVCN = BitConverter.ToUInt32(bytes, 72);
            TargetLCN = BitConverter.ToUInt32(bytes, 80);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static OperationRecord[] GetInstances(string volume)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal static OperationRecord Get(byte[] bytes)
        {
            return new OperationRecord(bytes);
        }

        #endregion Static Methods
    }

}