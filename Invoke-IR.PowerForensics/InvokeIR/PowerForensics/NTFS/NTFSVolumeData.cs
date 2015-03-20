using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{

    public class NTFSVolumeData
    {

        struct NTFS_VOLUME_DATA_BUFFER
        {
            internal ulong VolumeSerialNumber;
            internal long NumberSectors;
            internal long TotalClusters;
            internal long FreeClusters;
            internal long TotalReserved;
            internal int BytesPerSector;
            internal int BytesPerCluster;
            internal int BytesPerFileRecordSegment;
            internal int ClustersPerFileRecordSegment;
            internal long MftValidDataLength;
            internal long MftStartLcn;
            internal long Mft2StartLcn;
            internal long MftZoneStart;
            internal long MftZoneEnd;

            internal NTFS_VOLUME_DATA_BUFFER(byte[] bytes)
            {
                VolumeSerialNumber = BitConverter.ToUInt64(bytes, 0);
                NumberSectors = BitConverter.ToInt64(bytes, 8);
                TotalClusters = BitConverter.ToInt64(bytes, 16);
                FreeClusters = BitConverter.ToInt64(bytes, 24);
                TotalReserved = BitConverter.ToInt64(bytes, 32);
                BytesPerSector = BitConverter.ToInt32(bytes, 40);
                BytesPerCluster = BitConverter.ToInt32(bytes, 44);
                BytesPerFileRecordSegment = BitConverter.ToInt32(bytes, 48);
                ClustersPerFileRecordSegment = BitConverter.ToInt32(bytes, 52);
                MftValidDataLength = BitConverter.ToInt64(bytes, 56);
                MftStartLcn = BitConverter.ToInt64(bytes, 64);
                Mft2StartLcn = BitConverter.ToInt64(bytes, 72);
                MftZoneStart = BitConverter.ToInt64(bytes, 80);
                MftZoneEnd = BitConverter.ToInt64(bytes, 88);
            }
        }

        public long VolumeSize_MB;
        public long TotalSectors;
        public long TotalClusters;
        public long FreeClusters;
        public long FreeSpace_MB;
        public int BytesPerSector;
        public int BytesPerCluster;
        public int BytesPerMFTRecord;
        public int ClustersPerMFTRecord;
        public long MFTSize_MB;
        public long MFTStartCluster;
        public long MFTZoneClusterStart;
        public long MFTZoneClusterEnd;
        public long MFTZoneSize;
        public long MFTMirrorStart;

        internal NTFSVolumeData(long totalSectors, long totalClusters, long freeClusters, int bytesPerSector, int bytesPerCluster, int bytesPerMFTRecord, int clustersPerMFTRecord, long mftValidDataLength, long mftStartCluster, long mftZoneClusterStart, long mftZoneClusterEnd, long mftMirrorStart)
        {
            VolumeSize_MB = (totalClusters * bytesPerCluster) / 0x100000;
            TotalSectors = totalSectors;
            TotalClusters = totalClusters;
            FreeClusters = freeClusters;
            FreeSpace_MB = ((totalClusters - freeClusters) * bytesPerCluster) / 0x100000;
            BytesPerSector = bytesPerSector;
            BytesPerCluster = bytesPerCluster;
            BytesPerMFTRecord = bytesPerMFTRecord;
            ClustersPerMFTRecord = clustersPerMFTRecord;
            MFTSize_MB = (mftValidDataLength) / 0x100000;
            MFTStartCluster = mftStartCluster;
            MFTZoneClusterStart = mftZoneClusterStart;
            MFTZoneClusterEnd = mftZoneClusterEnd;
            MFTZoneSize = (mftZoneClusterEnd - mftZoneClusterStart) * bytesPerCluster;
            MFTMirrorStart = mftMirrorStart;
        }

        public static NTFSVolumeData Get(IntPtr hDrive)
        {

            // Create a byte array the size of the NTFS_VOLUME_DATA_BUFFER struct
            byte[] ntfsVolData = new byte[96];
            // Instatiate an integer to accept the amount of bytes read
            int buf = new int();

            // Return the NTFS_VOLUME_DATA_BUFFER struct
            var status = Win32.DeviceIoControl(
                hDevice: hDrive,
                dwIoControlCode: (uint)0x00090064,
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
