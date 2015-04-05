using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{

    public class VolumeData
    {

        internal struct NTFS_VOLUME_DATA_BUFFER
        {
            internal ulong VolumeSerialNumber;
            internal ulong NumberSectors;
            internal ulong TotalClusters;
            internal ulong FreeClusters;
            internal ulong TotalReserved;
            internal int BytesPerSector;
            internal int BytesPerCluster;
            internal int BytesPerFileRecordSegment;
            internal int ClustersPerFileRecordSegment;
            internal ulong MftValidDataLength;
            internal ulong MftStartLcn;
            internal ulong Mft2StartLcn;
            internal ulong MftZoneStart;
            internal ulong MftZoneEnd;

            internal NTFS_VOLUME_DATA_BUFFER(byte[] bytes)
            {
                VolumeSerialNumber = BitConverter.ToUInt64(bytes, 0);
                NumberSectors = BitConverter.ToUInt64(bytes, 8);
                TotalClusters = BitConverter.ToUInt64(bytes, 16);
                FreeClusters = BitConverter.ToUInt64(bytes, 24);
                TotalReserved = BitConverter.ToUInt64(bytes, 32);
                BytesPerSector = BitConverter.ToInt32(bytes, 40);
                BytesPerCluster = BitConverter.ToInt32(bytes, 44);
                BytesPerFileRecordSegment = BitConverter.ToInt32(bytes, 48);
                ClustersPerFileRecordSegment = BitConverter.ToInt32(bytes, 52);
                MftValidDataLength = BitConverter.ToUInt64(bytes, 56);
                MftStartLcn = BitConverter.ToUInt64(bytes, 64);
                Mft2StartLcn = BitConverter.ToUInt64(bytes, 72);
                MftZoneStart = BitConverter.ToUInt64(bytes, 80);
                MftZoneEnd = BitConverter.ToUInt64(bytes, 88);
            }
        }

        #region Properties

        public ulong VolumeSize_MB;
        public ulong TotalSectors;
        public ulong TotalClusters;
        public ulong FreeClusters;
        public ulong FreeSpace_MB;
        public int BytesPerSector;
        public int BytesPerCluster;
        public int BytesPerMFTRecord;
        public int ClustersPerMFTRecord;
        public ulong MFTSize_MB;
        public ulong MFTSize;
        public ulong MFTStartCluster;
        public ulong MFTZoneStartCluster;
        public ulong MFTZoneEndCluster;
        public ulong MFTZoneSize;
        public ulong MFTMirrorStartCluster;

        #endregion Properties

        #region Constructors

        internal VolumeData(IntPtr hDrive)
        {
            // Create a byte array the size of the NTFS_VOLUME_DATA_BUFFER struct
            byte[] ntfsVolData = new byte[96];
            // Instatiate an integer to accept the amount of bytes read
            int buf = new int();

            // Return the NTFS_VOLUME_DATA_BUFFER struct
            var status = NativeMethods.DeviceIoControl(
                hDevice: hDrive,
                dwIoControlCode: NativeMethods.NTFS_VOLUME_DATA_BUFFER,
                InBuffer: null,
                nInBufferSize: 0,
                OutBuffer: ntfsVolData,
                nOutBufferSize: ntfsVolData.Length,
                lpBytesReturned: ref buf,
                lpOverlapped: IntPtr.Zero);

            NTFS_VOLUME_DATA_BUFFER ntfsVD = new NTFS_VOLUME_DATA_BUFFER(ntfsVolData);
            
            VolumeSize_MB = (ntfsVD.TotalClusters * (ulong)ntfsVD.BytesPerCluster) / 0x100000;
            TotalSectors = ntfsVD.NumberSectors;
            TotalClusters = ntfsVD.TotalClusters;
            FreeClusters = ntfsVD.FreeClusters;
            FreeSpace_MB = ((ntfsVD.TotalClusters - ntfsVD.FreeClusters) * (ulong)ntfsVD.BytesPerCluster) / 0x100000;
            BytesPerSector = ntfsVD.BytesPerSector;
            BytesPerCluster = ntfsVD.BytesPerCluster;
            BytesPerMFTRecord = ntfsVD.BytesPerFileRecordSegment;
            ClustersPerMFTRecord = ntfsVD.ClustersPerFileRecordSegment;
            MFTSize_MB = (ntfsVD.MftValidDataLength) / 0x100000;
            MFTSize = ntfsVD.MftValidDataLength;
            MFTStartCluster = ntfsVD.MftStartLcn;
            MFTZoneStartCluster = ntfsVD.MftZoneStart;
            MFTZoneEndCluster = ntfsVD.MftZoneEnd;
            MFTZoneSize = (ntfsVD.MftZoneEnd - ntfsVD.MftZoneStart) * (ulong)ntfsVD.BytesPerCluster;
            MFTMirrorStartCluster = ntfsVD.Mft2StartLcn;
        }

        #endregion Constructors

    }

}
