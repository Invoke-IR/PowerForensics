using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InvokeIR.PowerForensics.Test
{

    public class NTFS_VOLUME_DATA_BUFFER
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

        internal NTFS_VOLUME_DATA_BUFFER (byte[] bytes)
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

        internal static NTFS_VOLUME_DATA_BUFFER Get(IntPtr hDrive)
        {

            // Create a byte array the size of the NTFS_VOLUME_DATA_BUFFER struct
            byte[] ntfsVolData = new byte[96];
            // Instatiate an integer to accept the amount of bytes read
            int buf = new int();

            // Return the NTFS_VOLUME_DATA_BUFFER struct
            var status = Win32.DeviceIoControl(
                hDevice: hDrive,
                dwIoControlCode: 0x00090064,
                InBuffer: null,
                nInBufferSize: 0,
                OutBuffer: ntfsVolData,
                nOutBufferSize: ntfsVolData.Length,
                lpBytesReturned: ref buf,
                lpOverlapped: IntPtr.Zero);

            return new NTFS_VOLUME_DATA_BUFFER(ntfsVolData);

        }

    }


    public class NTFSData
    {
        public static long get()
        {
            IntPtr hVolume = Win32.getHandle(@"\\.\C:");

            return (NTFS_VOLUME_DATA_BUFFER.Get(hVolume)).MftStartLcn;
        }

        public static byte[] getMFTBytes()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IntPtr hVolume = Win32.getHandle(@"\\.\C:");
            stopwatch.Stop();
            Console.WriteLine("getHandle: {0}", stopwatch.ElapsedTicks);
            stopwatch.Start();
            FileStream streamToRead = Win32.getFileStream(hVolume);
            stopwatch.Stop();
            Console.WriteLine("getFileStream: {0}", stopwatch.ElapsedTicks);
            stopwatch.Start();            
            NTFS_VOLUME_DATA_BUFFER volData = NTFS_VOLUME_DATA_BUFFER.Get(hVolume);
            stopwatch.Stop();
            Console.WriteLine("NTFS_VOLUME_DATA_BUFFER.Get: {0}", stopwatch.ElapsedTicks);
            stopwatch.Start();

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MftStartLcn);

            // Read bytes belonging to specified MFT Record and store in byte array
            byte[] mftBytes = Win32.readDrive(streamToRead, mftOffset, volData.MftValidDataLength);
            stopwatch.Stop();
            Console.WriteLine("readDrive: {0}", stopwatch.ElapsedMilliseconds);
            
            return mftBytes;
        }
    }

    public class MFTRecord
    {

        enum FILE_RECORD_FLAG
        {
            INUSE = 0x01,	// File record is in use
            DIR = 0x02	    // File record is a directory
        }

        internal uint Magic;			// "FILE"
        internal ushort OffsetOfUS;		// Offset of Update Sequence
        internal ushort SizeOfUS;		// Size in words of Update Sequence Number & Array
        public ulong LSN;			    // $LogFile Sequence Number
        public ushort SeqNo;			// Sequence number
        public ushort Hardlinks;		// Hard link count
        internal ushort OffsetOfAttr;	// Offset of the first Attribute
        public ushort Flags;			// Flags
        internal uint RealSize;		    // Real size of the FILE record
        internal uint AllocSize;		// Allocated size of the FILE record
        internal ulong RefToBase;		// File reference to the base FILE record
        internal ushort NextAttrId;		// Next Attribute Id
        internal ushort Align;			// Align to 4 byte boundary
        public uint RecordNo;		    // Number of this MFT Record

        internal MFTRecord(byte[] bytes)
        {
            Magic = BitConverter.ToUInt32(bytes, 0);
            OffsetOfUS = BitConverter.ToUInt16(bytes, 4);
            SizeOfUS = BitConverter.ToUInt16(bytes, 6);
            LSN = BitConverter.ToUInt64(bytes, 8);
            SeqNo = BitConverter.ToUInt16(bytes, 16);
            Hardlinks = BitConverter.ToUInt16(bytes, 18);
            OffsetOfAttr = BitConverter.ToUInt16(bytes, 20);
            Flags = BitConverter.ToUInt16(bytes, 22);
            RealSize = BitConverter.ToUInt32(bytes, 24);
            AllocSize = BitConverter.ToUInt32(bytes, 28);
            RefToBase = BitConverter.ToUInt64(bytes, 32);
            NextAttrId = BitConverter.ToUInt16(bytes, 40);
            Align = BitConverter.ToUInt16(bytes, 42);
            RecordNo = BitConverter.ToUInt32(bytes, 44);
        }

        private static bool checkMFTRecord(uint magic)
        {
            return magic == 1162627398;
        }

        private static byte[] getMFTRecordBytes(IntPtr hVolume, FileStream streamToRead, uint inode)
        {

            // Gather Data about Volume
            NTFS.NTFSVolumeData volData = NTFS.NTFSVolumeData.Get(hVolume);

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MFTStartCluster);

            // Determine offset to specified MFT Record
            long offsetMFTRecord = (inode * volData.BytesPerMFTRecord) + mftOffset;

            // Read bytes belonging to specified MFT Record and store in byte array
            return Win32.readDrive(streamToRead, offsetMFTRecord, volData.BytesPerMFTRecord);

        }

        public static MFTRecord Get(string volume, uint inode)
        {

            IntPtr hVolume = Win32.getHandle(volume);
            FileStream streamToRead = Win32.getFileStream(hVolume);

            byte[] MFTRecordBytes = getMFTRecordBytes(hVolume, streamToRead, inode);

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            MFTRecord RecordHeader = new MFTRecord(MFTRecordBytes);

            return RecordHeader;

        }

    }

}
