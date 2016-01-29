using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics
{
    #region GuidPartitionTableClass

    public class GuidPartitionTable
    {

        #region Constants

        private const int GPT_OFFSET = 512;
        private const int SECTOR_SIZE = 512;

        #endregion Constants

        #region Properties

        public readonly Version Revision;
        public readonly uint HeaderSize;
        public readonly ulong MyLBA;
        public readonly ulong AlternateLBA;
        public readonly ulong FirstUsableLBA;
        public readonly ulong LastUsableLBA;
        public readonly Guid DiskGuid;
        public readonly ulong PartitionEntryLBA;
        public readonly uint NumberOfPartitionEntries;
        public readonly uint SizeOfPartitionEntry;
        public readonly GuidPartitionTableEntry[] PartitionTable;

        #endregion Properties

        #region Constructors

        internal GuidPartitionTable(string devicePath)
        {
            MasterBootRecord mbr = MasterBootRecord.Get(devicePath);
            if (mbr.PartitionTable[0].SystemId == "EFI_GPT_DISK")
            {
                using (FileStream streamToRead = Helper.getFileStream(devicePath))
                {
                    // Get Header
                    GuidPartitionTableHeader GPTHeader = new GuidPartitionTableHeader(Helper.readDrive(streamToRead, GPT_OFFSET, SECTOR_SIZE));
                    Revision = GPTHeader.Revision;
                    HeaderSize = GPTHeader.HeaderSize;
                    MyLBA = GPTHeader.MyLBA;
                    AlternateLBA = GPTHeader.AlternateLBA;
                    FirstUsableLBA = GPTHeader.FirstUsableLBA;
                    LastUsableLBA = GPTHeader.LastUsableLBA;
                    DiskGuid = GPTHeader.DiskGUID;
                    PartitionEntryLBA = GPTHeader.PartitionEntryLBA;
                    NumberOfPartitionEntries = GPTHeader.NumberOfPartitionEntries;
                    SizeOfPartitionEntry = GPTHeader.SizeOfPartitionEntry;

                    // Get PartitionTable
                    List<GuidPartitionTableEntry> partitionList = new List<GuidPartitionTableEntry>();

                    bool Continue = true;

                    // Iterate through sectors that contain the GPT Entry Array
                    for(ulong j = GPTHeader.PartitionEntryLBA; (j < GPTHeader.PartitionEntryLBA + (GPTHeader.NumberOfPartitionEntries / (SECTOR_SIZE / GPTHeader.SizeOfPartitionEntry))); j++)
                    {
                        // Read one sector
                        byte[] partitionSectorBytes = Helper.readDrive(streamToRead, (SECTOR_SIZE * j), SECTOR_SIZE);
                        
                        // Iterate through Partition Entries in Sector
                        // Sectors (512 bytes) / Partitions (128 bytes) = 4 partitions per sector 
                        for (int i = 0; i < 512; i += (int)GPTHeader.SizeOfPartitionEntry)
                        {
                            // Instantiate a GuidPartitionTableEntry object
                            GuidPartitionTableEntry entry = new GuidPartitionTableEntry(Helper.GetSubArray(partitionSectorBytes, i, (int)GPTHeader.SizeOfPartitionEntry));
                            
                            // If entry's PartitionTypeGUID is 00000000-0000-0000-0000-000000000000 then it is not a partition
                            if (entry.PartitionTypeGuid == new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                Continue = false;
                                break;
                            }
                            partitionList.Add(entry);
                        }

                        if (!Continue)
                        {
                            break;
                        }
                    }

                    PartitionTable = partitionList.ToArray();

                }
            }
            else
            {
                throw new Exception("No GPT found. Please use Get-MBR cmdlet");
            }
        }

        internal GuidPartitionTable(byte[] bytes)
        {
            #region Header

            GuidPartitionTableHeader GPTHeader = new GuidPartitionTableHeader(Helper.GetSubArray(bytes, 0x00, SECTOR_SIZE));
            Revision = GPTHeader.Revision;
            HeaderSize = GPTHeader.HeaderSize;
            MyLBA = GPTHeader.MyLBA;
            AlternateLBA = GPTHeader.AlternateLBA;
            FirstUsableLBA = GPTHeader.FirstUsableLBA;
            LastUsableLBA = GPTHeader.LastUsableLBA;
            DiskGuid = GPTHeader.DiskGUID;
            PartitionEntryLBA = GPTHeader.PartitionEntryLBA;
            NumberOfPartitionEntries = GPTHeader.NumberOfPartitionEntries;
            SizeOfPartitionEntry = GPTHeader.SizeOfPartitionEntry;
            
            #endregion Header

            // Get PartitionTable
            List<GuidPartitionTableEntry> partitionList = new List<GuidPartitionTableEntry>();

            for (int i = 0; i < (bytes.Length - SECTOR_SIZE); i += (int)GPTHeader.SizeOfPartitionEntry)
            {
                // Instantiate a GuidPartitionTableEntry object
                GuidPartitionTableEntry entry = new GuidPartitionTableEntry(Helper.GetSubArray(bytes, (i + SECTOR_SIZE), (int)GPTHeader.SizeOfPartitionEntry));
                // If entry's PartitionTypeGUID is 00000000-0000-0000-0000-000000000000 then it is not a partition
                if (entry.PartitionTypeGuid == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    break;
                }
                partitionList.Add(entry);
            }

            PartitionTable = partitionList.ToArray();
        }

        #endregion Constructors

        #region StaticMethods

        public static GuidPartitionTable Get(string devicePath)
        {
            return new GuidPartitionTable(GuidPartitionTable.GetBytes(devicePath));
        }

        public static byte[] GetBytes(string devicePath)
        {
            MasterBootRecord mbr = MasterBootRecord.Get(devicePath);
            if (mbr.PartitionTable[0].SystemId == "EFI_GPT_DISK")
            {
                //IntPtr hDevice = Helper.getHandle(devicePath);
                //using (FileStream streamToRead = Helper.getFileStream(hDevice))
                using (FileStream streamToRead = Helper.getFileStream(devicePath))
                {
                    return Helper.readDrive(streamToRead, GPT_OFFSET, SECTOR_SIZE);
                }
            }
            else
            {
                throw new Exception("No GPT found. Please use Get-MBR cmdlet");
            }
        }

        

        #endregion StaticMethods

        #region InstanceMethods

        public GuidPartitionTableEntry[] GetPartitionTable()
        {
            return this.PartitionTable;
        }

        #endregion InstanceMethods
    }

    #endregion GuidPartitionTableClass

    #region GuidPartitionTableHeaderClass

    internal class GuidPartitionTableHeader
    {
        #region Constants

        private const string SIGNATURE_STRING = "EFI PART";

        #endregion Constants

        #region Properties

        internal readonly string Signature;
        internal readonly Version Revision;
        internal readonly uint HeaderSize;
        internal readonly uint HeaderCRC32;
        internal readonly ulong MyLBA;
        internal readonly ulong AlternateLBA;
        internal readonly ulong FirstUsableLBA;
        internal readonly ulong LastUsableLBA;
        internal readonly Guid DiskGUID;
        internal readonly ulong PartitionEntryLBA;
        internal readonly uint NumberOfPartitionEntries;
        internal readonly uint SizeOfPartitionEntry;
        internal readonly byte[] PartitionEntryArrayCRC32;

        #endregion Properties

        #region Constructors

        internal GuidPartitionTableHeader(byte[] bytes)
        {
            // Test GPT Signature
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x08);
            
            if (Signature == SIGNATURE_STRING)
            {
                Revision = new Version(BitConverter.ToUInt16(bytes, 0x08), BitConverter.ToUInt16(bytes, 0x0A));
                HeaderSize = BitConverter.ToUInt32(bytes, 0x0C);
                // Get HeaderCRC32 Value
                #region MyLBA
                
                MyLBA = BitConverter.ToUInt64(bytes, 0x18);
                if (MyLBA != 1)
                {
                    throw new Exception("Invalid MyLBA property value");
                }

                #endregion MyLBA
                AlternateLBA = BitConverter.ToUInt64(bytes, 0x20);
                FirstUsableLBA = BitConverter.ToUInt64(bytes, 0x28);
                LastUsableLBA = BitConverter.ToUInt64(bytes, 0x30);
                DiskGUID = new Guid(Helper.GetSubArray(bytes, 0x38, 0x10));
                PartitionEntryLBA = BitConverter.ToUInt64(bytes, 0x48);
                NumberOfPartitionEntries = BitConverter.ToUInt32(bytes, 0x50);
                SizeOfPartitionEntry = BitConverter.ToUInt32(bytes, 0x54);
                // Get PartitionEntryArrayCRC32 Value
            }
            else
            {
                throw new Exception("Invalid GPT Signature");
            }
        }

        #endregion Constructors
    }

    #endregion GuidPartitionTableHeaderClass

    #region GuidPartitionTableEntryClass

    public class GuidPartitionTableEntry
    {
        #region Enums

        /*enum PARTITION_TYPE_GUID : string
        {
            UNUSED_ENTRY = "00000000-0000-0000-0000-000000000000",
            EFI_SYSTEM_PARTITION = "C12A7328-F81F-11D2-BA4B-00A0C93EC93B",
            LEGACY_MBR = "024DEE41-33E7-11D3-9D69-0008C781F39F"
        }*/

        [FlagsAttribute]
        public enum PARTITION_ATTRIBUTE
        {
            RequirePartition = 0x01,
            NoBlockIOProtocol = 0x02,
            LegacyBIOSBootable = 0x04
        }

        #endregion Enums

        #region Properties

        public readonly Guid PartitionTypeGuid;
        public readonly Guid UniquePartitionGuid;
        public readonly ulong StartingLBA;
        public readonly ulong EndingLBA;
        public readonly PARTITION_ATTRIBUTE Attributes;
        public readonly string PartitionName;

        #endregion Properties

        #region Constructors

        internal GuidPartitionTableEntry(byte[] bytes)
        {
            PartitionTypeGuid = new Guid(Helper.GetSubArray(bytes, 0x00, 0x10));
            UniquePartitionGuid = new Guid(Helper.GetSubArray(bytes, 0x10, 0x10));
            StartingLBA = BitConverter.ToUInt64(bytes, 32);
            EndingLBA = BitConverter.ToUInt64(bytes, 40);
            Attributes = (PARTITION_ATTRIBUTE)BitConverter.ToUInt64(bytes, 48);
            PartitionName = Encoding.Unicode.GetString(bytes, 0x38, 0x48).Split('\0')[0];
        }

        #endregion Constructors
    }

    #endregion GuidPartitionTableEntryClass
}
