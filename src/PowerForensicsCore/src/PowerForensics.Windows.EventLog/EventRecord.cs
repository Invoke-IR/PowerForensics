using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.Windows.EventLog
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum FILEFLAGS
    {
        /// <summary>
        /// 
        /// </summary>
        IsDirty = 0x00,

        /// <summary>
        /// 
        /// </summary>
        IsFull = 0x01
    }

    class EventLogHeader
    {
        #region Properties

        // File Header
        private readonly string Signature;
        internal readonly ulong FirstChunkNumber;
        internal readonly ulong LastChunkNumber;
        internal readonly ulong NextRecordIdentifier;
        internal readonly uint HeaderSize;
        private readonly ushort MinorVersion;
        private readonly ushort MajorVersion;
        internal readonly Version Version;
        internal readonly ushort HeaderBlockSize;
        internal readonly ushort NumberOfChunks;
        internal readonly FILEFLAGS FileFlags;
        private readonly uint Checksum;

        #endregion Properties

        #region Constructors

        internal EventLogHeader(byte[] bytes)
        {
            // File Header
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x08);
            if (Signature == "ElfFile\0")
            {
                FirstChunkNumber = BitConverter.ToUInt64(bytes, 0x08);
                LastChunkNumber = BitConverter.ToUInt64(bytes, 0x10);
                NextRecordIdentifier = BitConverter.ToUInt64(bytes, 0x18);
                HeaderSize = BitConverter.ToUInt32(bytes, 0x20);
                MinorVersion = BitConverter.ToUInt16(bytes, 0x24);
                MajorVersion = BitConverter.ToUInt16(bytes, 0x26);
                Version = new Version(MajorVersion, MinorVersion);
                HeaderBlockSize = BitConverter.ToUInt16(bytes, 0x28);
                NumberOfChunks = BitConverter.ToUInt16(bytes, 0x2A);
                FileFlags = (FILEFLAGS)BitConverter.ToUInt32(bytes, 0x78);
                Checksum = BitConverter.ToUInt32(bytes, 0x7C);
            }
            else
            {
                throw new Exception("Invalid EventLogHeader");
            }
        }

        #endregion Constructors
    }

    class ChunkHeader
    {
        #region Properties

        internal readonly string Signature;
        internal readonly long FirstEventRecordNumber;
        internal readonly long LastEventRecordNumber;
        internal readonly long FirstEventRecordIdentifier;
        internal readonly long LastEventRecordIdentifier;
        internal readonly uint HeaderSize;
        internal readonly uint LastEventRecordDataOffset;
        internal readonly uint FreeSpaceOffset;
        internal readonly uint EventRecordsChecksum;
        internal readonly uint Checksum;

        #endregion Properties

        #region Constructors

        internal ChunkHeader(byte[] bytes, int offset)
        {
            Signature = Encoding.ASCII.GetString(bytes, offset, 0x08);
            if (Signature == "ElfChnk\0")
            {
                FirstEventRecordNumber = BitConverter.ToInt64(bytes, offset + 0x08);
                LastEventRecordNumber = BitConverter.ToInt64(bytes, offset + 0x10);
                FirstEventRecordIdentifier = BitConverter.ToInt64(bytes, offset + 0x18);
                LastEventRecordIdentifier = BitConverter.ToInt64(bytes, offset + 0x20);
                HeaderSize = BitConverter.ToUInt32(bytes, offset + 0x28);
                LastEventRecordDataOffset = BitConverter.ToUInt32(bytes, offset + 0x2C);
                FreeSpaceOffset = BitConverter.ToUInt32(bytes, offset + 0x30);
                EventRecordsChecksum = BitConverter.ToUInt32(bytes, offset + 0x34);
                Checksum = BitConverter.ToUInt32(bytes, offset + 0x7C);
            }
            else
            {
                throw new Exception("Invalid ChunkHeader");
            }
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class EventRecord
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string LogPath;

        internal readonly uint Signature;
        internal readonly uint Size;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong EventRecordId;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime WriteTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly BinaryXml EventData;

        internal readonly uint CopyOfSize;

        #endregion Properties

        #region Constructors

        // Check Signature
        internal EventRecord(byte[] bytes, int chunkOffset, int recordOffset, string path)
        {
            LogPath = path;
            Signature = BitConverter.ToUInt32(bytes, recordOffset);
            if (Signature == 10794)
            {
                Size = BitConverter.ToUInt32(bytes, recordOffset + 0x04);
                EventRecordId = BitConverter.ToUInt64(bytes, recordOffset + 0x08);
                WriteTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, recordOffset + 0x10));;
                EventData = new BinaryXml(bytes, chunkOffset, recordOffset + 0x18, (int)Size - 0x1C);
                CopyOfSize = BitConverter.ToUInt32(bytes, recordOffset + (int)Size - 0x04);
            }
            else
            {
                throw new Exception("Invalid EventRecord object");
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static EventRecord[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<EventRecord> recordList = new List<EventRecord>();

            string volLetter = Helper.GetVolumeLetter(volume);

            string EventLogPath = volLetter + @"\Windows\system32\winevt\Logs";

            IndexEntry[] entries = IndexEntry.GetInstances(EventLogPath);
            
            foreach (IndexEntry entry in entries)
            {
                try
                {
                    EventRecord[] records = Get(entry.FullName);
                    recordList.AddRange(records);
                }
                catch
                {
                    Console.WriteLine(entry.FullName);
                }
            }

            return recordList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static EventRecord[] Get(string path)
        {
            List<EventRecord> recordList = new List<EventRecord>();

            // Get Content of EventLog
            FileRecord fileRecord = FileRecord.Get(path, true);
            byte[] bytes = fileRecord.GetContent();

            // Get EventLog Header
            EventLogHeader evtxHeader = new EventLogHeader(bytes);

            int chunkOffset = 0x1000;

            // Iterate through chunks
            for (int i = 0; i < evtxHeader.NumberOfChunks; i++)
            {
                // Get Chunk Header
                ChunkHeader chunkHeader = new ChunkHeader(bytes, chunkOffset);
                if(chunkHeader.LastEventRecordNumber == -1)
                {
                    break;
                }

                int recordOffset = chunkOffset + 0x200;

                // Iterate through EventRecords
                for (long j = chunkHeader.FirstEventRecordNumber; j <= chunkHeader.LastEventRecordNumber; j++)
                {
                    EventRecord eventRecord = new EventRecord(bytes, chunkOffset, recordOffset, path);
                    recordList.Add(eventRecord);
                    recordOffset += (int)eventRecord.Size;
                }

                // Increment Chunk Offset to point to next chunk
                chunkOffset += 0x10000;
            }
            
            return recordList.ToArray();
        }

        #endregion Static Methods

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("({0}) Record Number: {1} Log: {2}", this.EventData.EventId, this.EventRecordId, this.LogPath);
        }

        #endregion Override Methods
    }
}
