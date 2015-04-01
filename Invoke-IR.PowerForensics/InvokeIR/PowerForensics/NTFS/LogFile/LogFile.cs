using System;
using System.Runtime.InteropServices;
using InvokeIR.PowerForensics.NTFS.MFT;
using InvokeIR.PowerForensics.NTFS.MFT.Attributes;

namespace InvokeIR.PowerForensics.NTFS.LogFile
{
    public class LogFile
    {

        internal enum OPERATION_CODE
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

        internal struct RESTART_AREA_HEADER
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] Signature;
            ushort USOffset;
            ushort USCount;
            ulong CheckDiskLSN;
            uint SystemPageSize;
            uint LogPageSize;
            ushort RestartOffset;
            ushort MinorVersion;
            ushort MajorVersion;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 18)]
            byte[] USArray;
            ulong CurrentLSN;
            uint LogClient;
            uint ClientList;
            ulong Flags;
        }

        internal struct PAGE_HEADER
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] Signature;
            ushort USOffset;
            ushort USCount;
            ulong LastLSN;
            uint Flags;
            ushort PageCount;
            ushort PagePosition;
            ushort NextRecordOffset;
            ushort WORDAlign;
            uint DWORDAlign;
            ulong LastEndLSN;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 24)]
            byte[] USArray;
        }

        internal struct OPERATION_RECORD
        {
            ulong LSN;
            ulong PreviousLSN;
            ulong ClientUndoLSN;
            uint ClientDataLength;
            uint ClientID;
            uint RecordType;
            uint TransactionID;
            ushort Flags;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6)]
            byte[] Padding;
            ushort RedoOP;
            ushort UndoOP;
            ushort RedoOffset;
            ushort RedoLength;
            ushort UndoOffset;
            ushort UndoLength;
            ushort TargetAttribute;
            ushort LCNtoFollow;
            ushort RecordOffset;
            ushort AttrOffset;
            ushort MFTClusterIndex;
            ushort Padding1;
            uint TargetVCN;
            uint Padding2;
            uint TargetLCN;
            uint Padding3;
        }


        public static byte[] getBytes(string volume)
        {
            
            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            MFTRecord logFileRecord = MFTRecord.Get(mftBytes, 2);

            NonResident data = null;

            foreach(Attr attr in logFileRecord.Attribute)
            {
                if(attr.Name == "DATA")
                {
                    data = attr as NonResident;
                    break;
                }
            }

            return (NonResident.GetContent(volume, data)).ToArray();

        }

    }
}
