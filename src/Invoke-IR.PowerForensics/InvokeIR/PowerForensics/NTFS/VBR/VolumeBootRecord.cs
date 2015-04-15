using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{
    #region VolumeBootRecordClass

    public class VolumeBootRecord
    {

        #region Structs

        struct NTFS_VOLUME_BOOT_RECORD
        {
            // jump instruction
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] Jmp;

            // signature
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
            internal string Signature;

            // BPB and extended BPB
            internal ushort BytesPerSector;
            internal byte SectorsPerCluster;
            internal ushort ReservedSectors;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] Zeros1;
            internal ushort NotUsed1;
            internal byte MediaDescriptor;
            internal ushort Zeros2;
            internal ushort SectorsPerTrack;
            internal ushort NumberOfHeads;
            internal uint HiddenSectors;
            internal uint NotUsed2;
            internal uint NotUsed3;
            internal ulong TotalSectors;
            internal ulong LCN_MFT;
            internal ulong LCN_MFTMirr;
            internal byte ClustersPerFileRecord;
            internal byte[] NotUsed4;
            internal byte ClustersPerIndexBlock;
            internal byte[] NotUsed5;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
            internal string VolumeSN;

            // boot code
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 430)]
            internal byte[] Code;

            //0xAA55
            internal byte _AA;
            internal byte _55;

            internal NTFS_VOLUME_BOOT_RECORD(byte[] bytes)
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
                VolumeSN = BitConverter.ToString(bytes.Skip(72).Take(8).ToArray()).Replace("-", "");
                Code = bytes.Skip(80).Take(430).ToArray();
                _AA = bytes[510];
                _55 = bytes[511];

            }

        }

        #endregion Structs

        #region Properties

        public readonly ushort BytesPerSector;
        public readonly byte SectorsPerCluster;
        public readonly ushort ReservedSectors;
        public readonly ushort SectorsPerTrack;
        public readonly ushort NumberOfHeads;
        public readonly uint HiddenSectors;
        public readonly ulong TotalSectors;
        public readonly ulong MFTStartIndex;
        public readonly ulong MFTMirrStartIndex;
        public readonly byte ClustersPerFileRecord;
        public readonly byte ClustersPerIndexBlock;
        public readonly string VolumeSN;
        public readonly byte[] CodeSection;

        #endregion Properties

        #region Constructors

        internal VolumeBootRecord(string volumeName)
        {
            // Instantiate a NTFS_VOLUME_BOOT_RECORD struct
            NTFS_VOLUME_BOOT_RECORD structVBR = new NTFS_VOLUME_BOOT_RECORD(getBytes(volumeName));

            // Assign object properties
            BytesPerSector = structVBR.BytesPerSector;
            SectorsPerCluster = structVBR.SectorsPerCluster;
            ReservedSectors = structVBR.ReservedSectors;
            NumberOfHeads = structVBR.NumberOfHeads;
            HiddenSectors = structVBR.HiddenSectors;
            TotalSectors = structVBR.TotalSectors;
            MFTStartIndex = structVBR.LCN_MFT;
            MFTMirrStartIndex = structVBR.LCN_MFTMirr;
            ClustersPerFileRecord = structVBR.ClustersPerFileRecord;
            ClustersPerIndexBlock = structVBR.ClustersPerIndexBlock;
            VolumeSN = structVBR.VolumeSN;
            CodeSection = structVBR.Code;
        }

        #endregion Constructors

        #region InternalMethods

        internal static byte[] getBytes(string volumeName)
        {
            // Get correct volume name from user input
            NativeMethods.getVolumeName(ref volumeName);

            // Get handle to Logical Volume
            IntPtr hVolume = NativeMethods.getHandle(volumeName);

            // Create a FileStream object for the Volume
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {
                return NativeMethods.readDrive(streamToRead, 0, 512);
            }
        }

        #endregion InternalMethods

    }

    #endregion VolumeBootRecordClass
}
