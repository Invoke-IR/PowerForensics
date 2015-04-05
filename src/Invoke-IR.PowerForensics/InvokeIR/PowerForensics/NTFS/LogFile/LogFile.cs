using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace InvokeIR.PowerForensics.NTFS
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

        public struct RESTART_AREA_HEADER
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
            public string Signature;
            public ushort USOffset;
            public ushort USCount;
            public ulong CheckDiskLSN;
            public uint SystemPageSize;
            public uint LogPageSize;
            public ushort RestartOffset;
            public ushort MinorVersion;
            public ushort MajorVersion;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 18)]
            public byte[] USArray;
            public ulong CurrentLSN;
            public uint LogClient;
            public uint ClientList;
            public ulong Flags;

            internal RESTART_AREA_HEADER(byte[] bytes)
            {
                Signature = Encoding.ASCII.GetString(bytes.Take(4).ToArray());
                USOffset = BitConverter.ToUInt16(bytes, 4);
                USCount = BitConverter.ToUInt16(bytes, 6);
                CheckDiskLSN = BitConverter.ToUInt64(bytes, 8);
                SystemPageSize = BitConverter.ToUInt32(bytes, 16);
                LogPageSize = BitConverter.ToUInt32(bytes, 20);
                RestartOffset = BitConverter.ToUInt16(bytes, 24);
                MinorVersion = BitConverter.ToUInt16(bytes, 26);
                MajorVersion = BitConverter.ToUInt16(bytes, 28);
                USArray = bytes.Skip(30).Take(18).ToArray();
                CurrentLSN = BitConverter.ToUInt64(bytes, 48);
                LogClient = BitConverter.ToUInt32(bytes, 56);
                ClientList = BitConverter.ToUInt32(bytes, 60);
                Flags = BitConverter.ToUInt64(bytes, 64);
            }
        }

        public struct PAGE_HEADER
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
            public string Signature;
            public ushort USOffset;
            public ushort USCount;
            public ulong LastLSN;
            public uint Flags;
            public ushort PageCount;
            public ushort PagePosition;
            public ushort NextRecordOffset;
            ushort WORDAlign;
            uint DWORDAlign;
            public ulong LastEndLSN;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] USArray;

            internal PAGE_HEADER(byte[] bytes)
            {
                Signature = Encoding.ASCII.GetString(bytes.Take(4).ToArray());
                USOffset = BitConverter.ToUInt16(bytes, 4);
                USCount = BitConverter.ToUInt16(bytes, 6);
                LastLSN = BitConverter.ToUInt64(bytes, 8);
                Flags = BitConverter.ToUInt32(bytes, 16);
                PageCount = BitConverter.ToUInt16(bytes, 20);
                PagePosition = BitConverter.ToUInt16(bytes, 22);
                NextRecordOffset = BitConverter.ToUInt16(bytes, 24);
                WORDAlign = BitConverter.ToUInt16(bytes, 26);
                DWORDAlign = BitConverter.ToUInt32(bytes, 28);
                LastEndLSN = BitConverter.ToUInt64(bytes, 32);
                USArray = bytes.Skip(40).Take(24).ToArray();
            }
        }

        public struct OPERATION_RECORD
        {
            public ulong LSN;
            public ulong PreviousLSN;
            public ulong ClientUndoLSN;
            public uint ClientDataLength;
            public uint ClientID;
            public uint RecordType;
            public uint TransactionID;
            public ushort Flags;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6)]
            byte[] Padding;
            public ushort RedoOP;
            public ushort UndoOP;
            public ushort RedoOffset;
            public ushort RedoLength;
            public ushort UndoOffset;
            public ushort UndoLength;
            public ushort TargetAttribute;
            public ushort LCNtoFollow;
            public ushort RecordOffset;
            public ushort AttrOffset;
            public ushort MFTClusterIndex;
            ushort Padding1;
            public uint TargetVCN;
            uint Padding2;
            public uint TargetLCN;
            uint Padding3;

            internal OPERATION_RECORD(byte[] bytes)
            {
                LSN = BitConverter.ToUInt64(bytes, 0);
                PreviousLSN = BitConverter.ToUInt64(bytes, 8);
                ClientUndoLSN = BitConverter.ToUInt64(bytes, 16);
                ClientDataLength = BitConverter.ToUInt32(bytes, 24);
                ClientID = BitConverter.ToUInt32(bytes, 28);
                RecordType= BitConverter.ToUInt32(bytes, 32);
                TransactionID = BitConverter.ToUInt32(bytes, 36);
                Flags = BitConverter.ToUInt16(bytes, 40);
                Padding = bytes.Skip(42).Take(6).ToArray();
                RedoOP = BitConverter.ToUInt16(bytes, 48);
                UndoOP = BitConverter.ToUInt16(bytes, 50);
                RedoOffset = BitConverter.ToUInt16(bytes, 52);
                RedoLength = BitConverter.ToUInt16(bytes, 54);
                UndoOffset = BitConverter.ToUInt16(bytes, 56);
                UndoLength = BitConverter.ToUInt16(bytes, 58);
                TargetAttribute = BitConverter.ToUInt16(bytes, 60);
                LCNtoFollow = BitConverter.ToUInt16(bytes, 62);
                RecordOffset = BitConverter.ToUInt16(bytes, 64);
                AttrOffset = BitConverter.ToUInt16(bytes, 66);
                MFTClusterIndex = BitConverter.ToUInt16(bytes, 68);
                Padding1 = BitConverter.ToUInt16(bytes, 70);
                TargetVCN = BitConverter.ToUInt32(bytes, 72);
                Padding2 = BitConverter.ToUInt32(bytes, 76);
                TargetLCN = BitConverter.ToUInt32(bytes, 80);
                Padding3 = BitConverter.ToUInt32(bytes, 84);
            }
        }

        public class Restart
        {
            public RESTART_AREA_HEADER restartHeader;
            public OPERATION_RECORD opRecord;

            public Restart(byte[] bytes)
            {
                restartHeader = new RESTART_AREA_HEADER(bytes.Take(72).ToArray());
                opRecord = new OPERATION_RECORD(bytes.Skip(72).Take(88).ToArray());
            }

            public static Restart[] Get(byte[] bytes)
            {
                Restart[] restartArray = new Restart[2];
                
                byte[] restart1 = new byte[0x1000];
                Array.Copy(bytes, 0, restart1, 0, restart1.Length);

                byte[] restart2 = new byte[0x1000];
                Array.Copy(bytes, 0x1000, restart2, 0, restart2.Length);

                restartArray[0] = new Restart(restart1);
                restartArray[1] = new Restart(restart2);

                return restartArray;
            }
        }

        public static byte[] getBytes(string volume)
        {

            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            MFTRecord logFileRecord = MFTRecord.Get(mftBytes, 2, null, null);

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
