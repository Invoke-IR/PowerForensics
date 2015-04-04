using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{

    public class NTFSVolumeData
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
        public ulong MFTZoneClusterStart;
        public ulong MFTZoneClusterEnd;
        public ulong MFTZoneSize;
        public ulong MFTMirrorStart;

        internal NTFSVolumeData(ulong totalSectors, ulong totalClusters, ulong freeClusters, int bytesPerSector, int bytesPerCluster, int bytesPerMFTRecord, int clustersPerMFTRecord, ulong mftValidDataLength, ulong mftStartCluster, ulong mftZoneClusterStart, ulong mftZoneClusterEnd, ulong mftMirrorStart)
        {
            VolumeSize_MB = (totalClusters * (ulong)bytesPerCluster) / 0x100000;
            TotalSectors = totalSectors;
            TotalClusters = totalClusters;
            FreeClusters = freeClusters;
            FreeSpace_MB = ((totalClusters - freeClusters) * (ulong)bytesPerCluster) / 0x100000;
            BytesPerSector = bytesPerSector;
            BytesPerCluster = bytesPerCluster;
            BytesPerMFTRecord = bytesPerMFTRecord;
            ClustersPerMFTRecord = clustersPerMFTRecord;
            MFTSize_MB = (mftValidDataLength) / 0x100000;
            MFTSize = mftValidDataLength;
            MFTStartCluster = mftStartCluster;
            MFTZoneClusterStart = mftZoneClusterStart;
            MFTZoneClusterEnd = mftZoneClusterEnd;
            MFTZoneSize = (mftZoneClusterEnd - mftZoneClusterStart) * (ulong)bytesPerCluster;
            MFTMirrorStart = mftMirrorStart;
        }

        public static NTFSVolumeData Get(IntPtr hDrive)
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

            // Return the NTFSVolumeData Object
            return new NTFSVolumeData(
                ntfsVD.NumberSectors,
                ntfsVD.TotalClusters,
                ntfsVD.FreeClusters,
                ntfsVD.BytesPerSector,
                ntfsVD.BytesPerCluster,
                ntfsVD.BytesPerFileRecordSegment,
                ntfsVD.ClustersPerFileRecordSegment,
                ntfsVD.MftValidDataLength,
                ntfsVD.MftStartLcn,
                ntfsVD.MftZoneStart,
                ntfsVD.MftZoneEnd,
                ntfsVD.Mft2StartLcn);
        }

    }

}
