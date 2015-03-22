using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace InvokeIR.PowerForensics.NTFS.BootSector
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct NTFS_BPB
    {
        // jump instruction
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        internal byte[] Jmp;

        // signature
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
        public string Signature;

        // BPB and extended BPB
        public ushort BytesPerSector;
        public byte SectorsPerCluster;
        public ushort ReservedSectors;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
        internal byte[] Zeros1;
        internal ushort NotUsed1;
        public byte MediaDescriptor;
        internal ushort Zeros2;
        public ushort SectorsPerTrack;
        public ushort NumberOfHeads;
        public uint HiddenSectors;
        internal uint NotUsed2;
        internal uint NotUsed3;
        public ulong TotalSectors;
        public ulong LCN_MFT;
        public ulong LCN_MFTMirr;
        public byte ClustersPerFileRecord;
        internal byte[] NotUsed4;
        public byte ClustersPerIndexBlock;
        internal byte[] NotUsed5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] VolumeSN;

        // boot code
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 430)]
        public byte[] Code;

        //0xAA55
        internal byte _AA;
        internal byte _55;

        internal NTFS_BPB(byte[] bytes)
        {

            Jmp = bytes.Skip(0).Take(3).ToArray();
            Signature = Encoding.ASCII.GetString(bytes.Skip(3).Take(8).ToArray());
            BytesPerSector = BitConverter.ToUInt16(bytes, 11);
            SectorsPerCluster = bytes[13];
            ReservedSectors = BitConverter.ToUInt16(bytes, 14);
            Zeros1 = bytes.Skip(16).Take(3).ToArray();
            NotUsed1 = BitConverter.ToUInt16(bytes, 19);
            MediaDescriptor = bytes[21];
            Zeros2 = BitConverter.ToUInt16(bytes, 22);
            SectorsPerTrack = BitConverter.ToUInt16(bytes, 24);
            NumberOfHeads = BitConverter.ToUInt16(bytes, 26);
            HiddenSectors = BitConverter.ToUInt32(bytes, 28);
            NotUsed2 = BitConverter.ToUInt32(bytes, 32);
            NotUsed3 = BitConverter.ToUInt32(bytes, 36);
            TotalSectors = BitConverter.ToUInt64(bytes, 40);
            LCN_MFT = BitConverter.ToUInt64(bytes, 48);
            LCN_MFTMirr = BitConverter.ToUInt64(bytes, 56);
            ClustersPerFileRecord = bytes[64];
            NotUsed4 = bytes.Skip(64).Take(3).ToArray();
            ClustersPerIndexBlock = bytes[68];
            NotUsed5 = bytes.Skip(68).Take(3).ToArray();
            VolumeSN = bytes.Skip(72).Take(8).ToArray();
            Code = bytes.Skip(80).Take(430).ToArray();
            _AA = bytes[510];
            _55 = bytes[511];
        
        }

    }

}
