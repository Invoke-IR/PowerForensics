using System;
using System.Linq;
using System.Text;
using System.Security;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{
    #region VolumeDataClass

    public class VolumeData
    {

        #region P/Invoke

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto), SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            UInt32 dwIoControlCode,
            IntPtr lpInBuffer,
            Int32 nInBufferSize,
            out NTFS_VOLUME_DATA_BUFFER lpOutBuffer,
            Int32 nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        #endregion P/Invoke

        #region Structs

        public struct NTFS_VOLUME_DATA_BUFFER
        {
            public ulong VolumeSerialNumber;
            public ulong NumberSectors;
            public ulong TotalClusters;
            public ulong FreeClusters;
            public ulong TotalReserved;
            public int BytesPerSector;
            public int BytesPerCluster;
            public int BytesPerFileRecordSegment;
            public int ClustersPerFileRecordSegment;
            public ulong MftValidDataLength;
            public ulong MftStartLcn;
            public ulong Mft2StartLcn;
            public ulong MftZoneStart;
            public ulong MftZoneEnd;

        }

        #endregion Structs

        #region Properties

        public readonly ulong TotalSectors;
        public readonly ulong TotalClusters;
        public readonly ulong FreeClusters;
        public readonly int BytesPerSector;
        public readonly int BytesPerCluster;
        public readonly int BytesPerMFTRecord;
        public readonly int ClustersPerMFTRecord;
        public readonly ulong MFTSize;
        public readonly ulong MFTStartCluster;
        public readonly ulong MFTZoneStartCluster;
        public readonly ulong MFTZoneEndCluster;
        public readonly ulong MFTMirrorStartCluster;

        #endregion Properties

        #region Constructors

        internal VolumeData(IntPtr hVolume)
        {
            // This constructor expects the caller to close the handle to the volume

            // Create a byte array the size of the NTFS_VOLUME_DATA_BUFFER struct
            NTFS_VOLUME_DATA_BUFFER ntfsVolData = new NTFS_VOLUME_DATA_BUFFER();

            // Instatiate an integer to accept the amount of bytes read
            uint buf = 0;

            // Return the NTFS_VOLUME_DATA_BUFFER struct
            var status = DeviceIoControl(
                hVolume,
                WinIoCtl.FSCTL_GET_NTFS_VOLUME_DATA,
                IntPtr.Zero,
                0,
                out ntfsVolData,
                Marshal.SizeOf(ntfsVolData),
                out buf,
                IntPtr.Zero);

            // Assign object properties
            TotalSectors = ntfsVolData.NumberSectors;
            TotalClusters = ntfsVolData.TotalClusters;
            FreeClusters = ntfsVolData.FreeClusters;
            BytesPerSector = ntfsVolData.BytesPerSector;
            BytesPerCluster = ntfsVolData.BytesPerCluster;
            BytesPerMFTRecord = ntfsVolData.BytesPerFileRecordSegment;
            ClustersPerMFTRecord = ntfsVolData.ClustersPerFileRecordSegment;
            MFTSize = ntfsVolData.MftValidDataLength;
            MFTStartCluster = ntfsVolData.MftStartLcn;
            MFTZoneStartCluster = ntfsVolData.MftZoneStart;
            MFTZoneEndCluster = ntfsVolData.MftZoneEnd;
            MFTMirrorStartCluster = ntfsVolData.Mft2StartLcn;

        }

        #endregion Constructors

    }

    #endregion VolumeDataClass
}
