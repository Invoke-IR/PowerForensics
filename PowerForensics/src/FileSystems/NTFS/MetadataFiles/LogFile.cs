using System;
using System.IO;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region LogFileClass

    public class LogFile
    {
        #region StaticMethods

        internal static FileRecord GetFileRecord(string volume)
        {
            return FileRecord.Get(volume, MftIndex.LOGFILE_INDEX, true);
        }

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


        public static byte[] getBytes(string volume)
        {   
            // Get filestream based on hVolume
            using (FileStream streamToRead = Helper.getFileStream(volume))
            {
                VolumeBootRecord VBR = VolumeBootRecord.Get(streamToRead);

                FileRecord logFileRecord = GetFileRecord(volume);

                NonResident data = GetDataAttr(logFileRecord);

                return data.GetBytes(volume);
            }
        }

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

        #endregion StaticMethods
    }

    #endregion LogFileClass

    #region RestartAreaHeaderClass

    public class RestartAreaHeader
    {
        #region Properties

        public readonly string Signature;
        public readonly ushort USOffset;
        public readonly ushort USCount;
        public readonly ulong CheckDiskLSN;
        public readonly uint SystemPageSize;
        public readonly uint LogPageSize;
        public readonly ushort RestartOffset;
        public readonly ushort MinorVersion;
        public readonly ushort MajorVersion;
        public readonly byte[] USArray;
        public readonly ulong CurrentLSN;
        public readonly uint LogClient;
        public readonly uint ClientList;
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

        #region StaticMethods

        public static RestartAreaHeader[] Get(string volume)
        {
            return RestartAreaHeader.Get(LogFile.getBytes(volume));
        }

        internal static RestartAreaHeader[] Get(byte[] bytes)
        {
            RestartAreaHeader[] headerArray = new RestartAreaHeader[0x02];
            headerArray[0] = new RestartAreaHeader(Helper.GetSubArray(bytes, 0x00, 0x1000));
            headerArray[1] = new RestartAreaHeader(Helper.GetSubArray(bytes, 0x1000, 0x1000));
            return headerArray;
        }

        #endregion StaticMethods
    }

    #endregion RestartAreaHeaderClass

    #region RestartClass

    public class Restart
    {
        #region Properties

        public RestartAreaHeader[] RestartHeader;

        #endregion Properties

        #region Constructors

        public Restart(byte[] bytes)
        {
            RestartHeader = RestartAreaHeader.Get(bytes);
        }

        #endregion Constructors

        #region StaticMethods

        public static Restart[] Get(byte[] bytes)
        {
            Restart[] restartArray = new Restart[2];

            restartArray[0] = new Restart(Helper.GetSubArray(bytes, 0x00, 0x1000));
            restartArray[1] = new Restart(Helper.GetSubArray(bytes, 0x1000, 0x1000));

            return restartArray;
        }

        #endregion StaticMethods
    }

    #endregion RestartClass

    #region PageHeaderClass

    public class PageHeader
    {
        #region Properties

        public readonly string Signature;
        public readonly ushort USOffset;
        public readonly ushort USCount;
        public readonly ulong LastLSN;
        public readonly uint Flags;
        public readonly ushort PageCount;
        public readonly ushort PagePosition;
        public readonly ushort NextRecordOffset;
        public readonly ulong LastEndLSN;
        public readonly ushort USN;
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

        #region StaticMethods

        public static PageHeader Get(string volume)
        {
            return new PageHeader(LogFile.getBytes(volume));
        }

        #endregion StaticMethods
    }

    #endregion PageHeaderClass

    #region OperationRecordClass

    public class OperationRecord
    {
        #region Enums

        public enum OPERATION_CODE
        {
            Noop = 0x00,
            CompensationlogRecord = 0x01,
            InitializeFileRecordSegment = 0x02,
            DeallocateFileRecordSegment = 0x03,
            WriteEndofFileRecordSegement = 0x04,
            CreateAttribute = 0x05,
            DeleteAttribute = 0x06,
            UpdateResidentValue = 0x07,
            UpdataeNonResidentValue = 0x08,
            UpdateMappingPairs = 0x09,
            DeleteDirtyClusters = 0x0A,
            SetNewAttributeSizes = 0x0B,
            AddindexEntryRoot = 0x0C,
            DeleteindexEntryRoot = 0x0D,
            AddIndexEntryAllocation = 0x0F,
            SetIndexEntryVenAllocation = 0x12,
            UpdateFileNameRoot = 0x13,
            UpdateFileNameAllocation = 0x14,
            SetBitsInNonresidentBitMap = 0x15,
            ClearBitsInNonresidentBitMap = 0x16,
            PrepareTransaction = 0x19,
            CommitTransaction = 0x1A,
            ForgetTransaction = 0x1B,
            OpenNonresidentAttribute = 0x1C,
            DirtyPageTableDump = 0x1F,
            TransactionTableDump = 0x20,
            UpdateRecordDataRoot = 0x21
        }

        #endregion Enums

        #region Properties

        public readonly ulong LSN;
        public readonly ulong PreviousLSN;
        public readonly ulong ClientUndoLSN;
        public readonly uint ClientDataLength;
        public readonly uint ClientID;
        public readonly uint RecordType;
        public readonly uint TransactionID;
        public readonly ushort Flags;
        public readonly OPERATION_CODE RedoOP;
        public readonly OPERATION_CODE UndoOP;
        public readonly ushort RedoOffset;
        public readonly ushort RedoLength;
        public readonly ushort UndoOffset;
        public readonly ushort UndoLength;
        public readonly ushort TargetAttribute;
        public readonly ushort LCNtoFollow;
        public readonly ushort RecordOffset;
        public readonly ushort AttrOffset;
        public readonly ushort MFTClusterIndex;
        public readonly uint TargetVCN;
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

        #region StaticMethods

        public static OperationRecord[] GetInstances(string volume)
        {
            return null;
        }

        internal static OperationRecord Get(byte[] bytes)
        {
            return new OperationRecord(bytes);
        }

        #endregion StaticMethods
    }

    #endregion OperationRecordClass
}
