using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.BootSectors
{
    /// <summary>
    /// 
    /// </summary>
    public class GuidPartitionTable
    {
        #region Constants

        private const string SIGNATURE_STRING = "EFI PART";
        private const int GPT_OFFSET = 512;
        internal const int SECTOR_SIZE = 512;

        #endregion Constants

        #region Properties

        internal readonly string Signature;

        /// <summary>
        /// 
        /// </summary>
        public readonly Version Revision;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint HeaderSize;

        //internal readonly uint HeaderCRC32;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong MyLBA;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong AlternateLBA;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong FirstUsableLBA;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong LastUsableLBA;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid DiskGuid;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong PartitionEntryLBA;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint NumberOfPartitionEntries;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SizeOfPartitionEntry;

        //internal readonly byte[] PartitionEntryArrayCRC32;

        /// <summary>
        /// 
        /// </summary>
        public readonly GuidPartitionTableEntry[] PartitionTable;

        #endregion Properties

        #region Constructors

        private GuidPartitionTable(string devicePath)
        {
            byte[] bytes = GuidPartitionTable.GetBytes(devicePath);

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
                DiskGuid = new Guid(Helper.GetSubArray(bytes, 0x38, 0x10));
                PartitionEntryLBA = BitConverter.ToUInt64(bytes, 0x48);
                NumberOfPartitionEntries = BitConverter.ToUInt32(bytes, 0x50);
                SizeOfPartitionEntry = BitConverter.ToUInt32(bytes, 0x54);
                // Get PartitionEntryArrayCRC32 Value
            }
            else
            {
                throw new Exception("Invalid GPT Signature");
            }

            PartitionTable = GuidPartitionTableEntry.GetInstances(bytes, NumberOfPartitionEntries, SizeOfPartitionEntry);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devicePath"></param>
        /// <returns></returns>
        public static GuidPartitionTable Get(string devicePath)
        {
            return new GuidPartitionTable(devicePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devicePath"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string devicePath)
        {
            MasterBootRecord mbr = MasterBootRecord.Get(devicePath);

            List<byte> byteList = new List<byte>();

            if (mbr.PartitionTable[0].SystemId == "EFI_GPT_DISK")
            {
                using (FileStream streamToRead = Helper.getFileStream(devicePath))
                {
                    byte[] headerBytes = Helper.readDrive(streamToRead, GPT_OFFSET, SECTOR_SIZE);

                    long partitionTableOffset = BitConverter.ToInt64(headerBytes, 0x48);
                    uint partitionCount = BitConverter.ToUInt32(headerBytes, 0x50);
                    uint partitionSize = BitConverter.ToUInt32(headerBytes, 0x54);

                    long partitionBufferSize = partitionCount * partitionSize;
                    
                    if(!((partitionBufferSize % 512) == 0))
                    {
                        partitionBufferSize = partitionBufferSize + (SECTOR_SIZE - (partitionBufferSize % SECTOR_SIZE));
                    }

                    byte[] partitionBytes = Helper.readDrive(streamToRead, partitionTableOffset * SECTOR_SIZE, partitionBufferSize);

                    byteList.AddRange(headerBytes);
                    byteList.AddRange(partitionBytes);
                }

                return byteList.ToArray();
            }
            else
            {
                throw new Exception("No GPT found. Please use Get-MBR cmdlet");
            }
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GuidPartitionTableEntry[] GetPartitionTable()
        {
            return this.PartitionTable;
        }

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class GuidPartitionTableEntry
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum PARTITION_ATTRIBUTE
        {
            /// <summary>
            /// 
            /// </summary>
            RequirePartition = 0x01,

            /// <summary>
            /// 
            /// </summary>
            NoBlockIOProtocol = 0x02,

            /// <summary>
            /// 
            /// </summary>
            LegacyBIOSBootable = 0x04
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string PartitionType;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly Guid UniquePartitionGuid;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly ulong StartingLBA;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong EndingLBA;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly PARTITION_ATTRIBUTE Attributes;

        /// <summary>
        /// 
        /// </summary>
        public readonly string PartitionName;

        #endregion Properties

        #region Constructors

        private GuidPartitionTableEntry(byte[] bytes, int i)
        {
            Dictionary<string, string> partitionTypeDictionary = new Dictionary<string, string>()
            {
                { "00000000-0000-0000-0000-000000000000", "Unused entry" },
                { "024dee41-33e7-11d3-9d69-0008c781f39f", "MBR partition scheme" },
                { "c12a7328-f81f-11d2-ba4b-00a0c93ec93b", "EFI System partition" },
                { "21686148-6449-6e6f-744e-656564454649", "BIOS Boot partition" },
                { "d3bfe2de-3daf-11df-ba40-e3a556d89593", "Intel Fast Flash(iFFS) partition(for Intel Rapid Start technology)" },
                { "f4019732-066e-4e12-8273-346c5641494f", "Sony boot partition" },
                { "bfbfafe7-a34f-448a-9a5b-6213eb736c22", "Lenovo boot partition" },
                { "e3c9e316-0b5c-4db8-817d-f92df00215ae", "Windows Microsoft Reserved Partition (MSR)" },
                { "ebd0a0a2-b9e5-4433-87c0-68b6b72699c7", "Windows Basic data partition" },
                { "5808c8aa-7e8f-42e0-85d2-e1e90434cfb3", "Windows Logical Disk Manager(LDM) metadata partition" },
                { "af9b60a0-1431-4f62-bc68-3311714a69ad", "Windows Logical Disk Manager data partition" },
                { "de94bba4-06d1-4d40-a16a-bfd50179d6ac", "Windows Windows Recovery Environment" },
                { "37affc90-ef7d-4e96-91c3-2d7ae055b174", "Windows IBM General Parallel File System(GPFS) partition" },
                { "e75caf8f-f680-4cee-afa3-b001e56efc2d", "Windows Storage Spaces partition" },
                { "75894c1e-3aeb-11d3-b7c1-7b03a0000000", "HP-UX Data partition" },
                { "e2a1e728-32e3-11d6-a682-7b03a0000000", "HP-UX Service Partition" },
                { "0fc63daf-8483-4772-8e79-3d69d8477de4", "Linux filesystem data" },
                { "a19d880f-05fc-4d3b-a006-743f0f84911e", "Linux RAID partition" },
                { "44479540-f297-41b2-9af7-d131d5f0458a", "Linux Root partition(x86)" },
                { "4f68bce3-e8cd-4db1-96e7-fbcaf984b709", "Linux Root partition(x8664)" },
                { "69dad710-2ce4-4e3c-b16c-21a1d49abed3", "Linux Root partition(32bit ARM)" },
                { "b921b045-1df0-41c3-af44-4c6f280d3fae", "Linux Root partition(64bit ARM / AArch64)" },
                { "0657fd6d-a4ab-43c4-84e5-0933c84b4f4f", "Linux Swap partition" },
                { "e6d6d379-f507-44c2-a23c-238f2a3df928", "Linux Logical Volume Manager(LVM) partition" },
                { "933ac7e1-2eb4-4f13-b844-0e14e2aef915", "Linux / home partition" },
                { "3b8f8425-20e0-4f3b-907f-1a25a76f98e8", "Linux / srv(server data) partition" },
                { "7ffec5c9-2d00-49b7-8941-3ea10a5586b7", "Linux Plain dmcrypt partition" },
                { "ca7d7ccb-63ed-4c53-861c-1742536059cc", "Linux LUKS partition" },
                { "8da63339-0007-60c0-c436-083ac8230908", "Linux Reserved" },
                { "83bd6b9d-7f41-11dc-be0b-001560b84f0f", "FreeBSD Boot partition" },
                { "516e7cb4-6ecf-11d6-8ff8-00022d09712b", "FreeBSD Data partition" },
                { "516e7cb5-6ecf-11d6-8ff8-00022d09712b", "FreeBSD Swap partition" },
                { "516e7cb6-6ecf-11d6-8ff8-00022d09712b", "FreeBSD Unix File System(UFS) partition" },
                { "516e7cb8-6ecf-11d6-8ff8-00022d09712b", "FreeBSD Vinum volume manager partition" },
                { "516e7cba-6ecf-11d6-8ff8-00022d09712b", "FreeBSD ZFS partition" },
                { "48465300-0000-11aa-aa11-00306543ecac", "OSX Hierarchical File System Plus(HFS +) partition" },
                { "55465300-0000-11aa-aa11-00306543ecac", "OSX Apple UFS" },
                { "6a898cc3-1dd2-11b2-99a6-080020736631", "OSX ZFS" },
                { "52414944-0000-11aa-aa11-00306543ecac", "OSX Apple RAID partition" },
                { "52414944-5f4f-11aa-aa11-00306543ecac", "OSX Apple RAID partition, offline" },
                { "426f6f74-0000-11aa-aa11-00306543ecac", "OSX Apple Boot partition(Recovery HD)" },
                { "4c616265-6c00-11aa-aa11-00306543ecac", "OSX Apple Label" },
                { "5265636f-7665-11aa-aa11-00306543ecac", "OSX Apple TV Recovery partition" },
                { "53746f72-6167-11aa-aa11-00306543ecac", "OSX Apple Core Storage(i.e.Lion FileVault) partition" },
                { "6a82cb45-1dd2-11b2-99a6-080020736631", "Solaris Boot partition" },
                { "6a85cf4d-1dd2-11b2-99a6-080020736631", "Solaris Root partition" },
                { "6a87c46f-1dd2-11b2-99a6-080020736631", "Solaris Swap partition" },
                { "6a8b642b-1dd2-11b2-99a6-080020736631", "Solaris Backup partition" },
                { "6a8ef2e9-1dd2-11b2-99a6-080020736631", "Solaris / var partition" },
                { "6a90ba39-1dd2-11b2-99a6-080020736631", "Solaris / home partition" },
                { "6a9283a5-1dd2-11b2-99a6-080020736631", "Solaris Alternate sector" },
                { "6a945a3b-1dd2-11b2-99a6-080020736631", "Solaris Reserved partition 1" },
                { "6a9630d1-1dd2-11b2-99a6-080020736631", "Solaris Reserved partition 2" },
                { "6a980767-1dd2-11b2-99a6-080020736631", "Solaris Reserved partition 3" },
                { "6a96237f-1dd2-11b2-99a6-080020736631", "Solaris Reserved partition 4" },
                { "6a8d2ac7-1dd2-11b2-99a6-080020736631", "Solaris Reserved partition 5" },
                { "49f48d32-b10e-11dc-b99b-0019d1879648", "NetBSD Swap partition" },
                { "49f48d5a-b10e-11dc-b99b-0019d1879648", "NetBSD FFS partition" },
                { "49f48d82-b10e-11dc-b99b-0019d1879648", "NetBSD LFS partition" },
                { "49f48daa-b10e-11dc-b99b-0019d1879648", "NetBSD RAID partition" },
                { "2db519c4-b10f-11dc-b99b-0019d1879648", "NetBSD Concatenated partition" },
                { "2db519ec-b10f-11dc-b99b-0019d1879648", "NetBSD Encrypted partition" },
                { "fe3a2a5d-4f32-41a7-b725-accc3285a309", "ChromeOS kernel" },
                { "3cb8e202-3b7e-47dd-8a3c-7ff2a13cfcec", "ChromeOS rootfs" },
                { "2e0a753d-9e48-43b0-8337-b15192cb1b5e", "ChromeOS future use" },
                { "42465331-3ba3-10f1-802a-4861696b7521", "Haiku BFS" },
                { "85d5e45e-237c-11e1-b4b3-e89a8f7fc3a7", "MidnightBSD Boot partition" },
                { "85d5e45a-237c-11e1-b4b3-e89a8f7fc3a7", "MidnightBSD Data partition" },
                { "85d5e45b-237c-11e1-b4b3-e89a8f7fc3a7", "MidnightBSD Swap partition" },
                { "0394ef8b-237e-11e1-b4b3-e89a8f7fc3a7", "MidnightBSD Unix File System(UFS) partition" },
                { "85d5e45c-237c-11e1-b4b3-e89a8f7fc3a7", "MidnightBSD Vinum volume manager partition" },
                { "85d5e45d-237c-11e1-b4b3-e89a8f7fc3a7", "MidnightBSD ZFS partition" },
                { "45b0969e-9b03-4f30-b4c6-b4b80ceff106", "Ceph Journal" },
                { "45b0969e-9b03-4f30-b4c6-5ec00ceff106", "Ceph dmcrypt Encrypted Journal" },
                { "4fbd7e29-9d25-41b8-afd0-062c0ceff05d", "Ceph OSD" },
                { "4fbd7e29-9d25-41b8-afd0-5ec00ceff05d", "Ceph dmcrypt OSD" },
                { "89c57f98-2fe5-4dc0-89c1-f3ad0ceff2be", "Ceph disk in creation" },
                { "89c57f98-2fe5-4dc0-89c1-5ec00ceff2be", "Ceph dmcrypt disk in creation" },
                { "824cc7a0-36a8-11e3-890a-952519ad3f61", "OpenBSD Data partition" },
                { "cef5a9ad-73bc-4601-89f3-cdeeeee321a1", "QNX Powersafe(QNX6) file system" },
                { "c91818f9-8025-47af-89d2-f030d7000c2c", "Plan 9 partition" },
                { "9d275380-40ad-11db-bf97-000c2911d1b8", "VMware ESX vmkcore(coredump partition)" },
                { "aa31e02a-400f-11db-9590-000c2911d1b8", "VMware ESX VMFS filesystem partition" },
                { "9198effc-31c0-11db-8f78-000c2911d1b8", "VMware ESX VMware Reserved" },
                { "2568845d-2332-4675-bc39-8fa5a4748d15", "Android-IA Bootloader" },
                { "114eaffe-1552-4022-b26e-9b053604cf84", "Android-IA Bootloader2" },
                { "49a4d17f-93a3-45c1-a0de-f50b2ebe2599", "Android-IA Boot" },
                { "4177c722-9e92-4aab-8644-43502bfd5506", "Android-IA Recovery" },
                { "ef32a33b-a409-486c-9141-9ffb711f6266", "Android-IA Misc" },
                { "20ac26be-20b7-11e3-84c5-6cfdb94711e9", "Android-IA Metadata" },
                { "38f428e6-d326-425d-9140-6e0ea133647c", "Android-IA System" },
                { "a893ef21-e428-470a-9e55-0668fd91a2d9", "Android-IA Cache" },
                { "dc76dda9-5ac1-491c-af42-a82591580c0d", "Android-IA Data" },
                { "ebc597d0-2053-4b15-8b64-e0aac75f4db1", "Android-IA Persistent" },
                { "8f68cc74-c5e5-48da-be91-a0c8c15e9c80", "Android-IA Factory" },
                { "767941d0-2085-11e3-ad3b-6cfdb94711e9", "Android-IA Fastboot / Tertiary" },
                { "ac6d7924-eb71-4df8-b48d-e267b27148ff", "Android-IA OEM" },
                { "7412f7d5-a156-4b13-81dc-867174929325", "ONIE Boot" },
                { "d4e6e2cd-4469-46f3-b5cb-1bff57afc149", "ONIE Config" },
                { "9e1a2d38-c612-4316-aa26-8b49521e5a8b", "PowerPC PreP boot" },
                { "bc13c2ff-59e6-4262-a352-b275fd6f7172", "Freedesktop Extended Boot Partition($BOOT)" }
            };

            string partitionTypeGuidString = new Guid(Helper.GetSubArray(bytes, 0x00 + i, 0x10)).ToString();

            if(partitionTypeDictionary.ContainsKey(partitionTypeGuidString))
            {
                PartitionType = partitionTypeDictionary[partitionTypeGuidString];
            }
            else
            {
                PartitionType = partitionTypeGuidString;
            }
            
            UniquePartitionGuid = new Guid(Helper.GetSubArray(bytes, 0x10 + i, 0x10));
            StartingLBA = BitConverter.ToUInt64(bytes, 0x20 + i);
            EndingLBA = BitConverter.ToUInt64(bytes, 0x28 + i);
            Attributes = (PARTITION_ATTRIBUTE)BitConverter.ToUInt64(bytes, 0x30 + i);
            PartitionName = Encoding.Unicode.GetString(bytes, 0x38 + i, 0x48).Split('\0')[0];
        }

        #endregion Constructors

        #region Static Methods

        internal static GuidPartitionTableEntry Get(byte[] bytes, int i)
        {
            return new GuidPartitionTableEntry(bytes, i);
        }

        internal static GuidPartitionTableEntry[] GetInstances(byte[] bytes, uint partitionCount, uint partitionSize)
        {
            // Get PartitionTable
            List<GuidPartitionTableEntry> partitionList = new List<GuidPartitionTableEntry>();

            // Iterate through Partition Entries
            for (int i = GuidPartitionTable.SECTOR_SIZE; i < bytes.Length; i += (int)partitionSize)
            {
                // Instantiate a GuidPartitionTableEntry object
                GuidPartitionTableEntry entry = new GuidPartitionTableEntry(bytes, i);

                // If entry's StartingLBA is 0 then it is not a partition
                if (entry.StartingLBA == 0)
                {
                    break;
                }

                partitionList.Add(entry);
            }

            return partitionList.ToArray();
        }

        #endregion Static Methods
    }
}
