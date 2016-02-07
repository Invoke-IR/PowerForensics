using System;
using System.IO;
using System.Collections.Generic;
using PowerForensics.Utilities;

namespace PowerForensics
{
    #region MasterBootRecordClass

    class MasterBootRecord
    {
        #region MbrSignatures

        private const string WINDOWS5_X = "8F558EB6672622401DA993E1E865C861";
        private const string WINDOWS6_0 = "5C616939100B85E558DA92B899A0FC36";
        private const string WINDOWS6_1 = "A36C5E4F47E84449FF07ED3517B43A31";
        //private const string LILO = "";
        private const string GRUB = "A6C7E63CA46F1CB2307E0F10AD897BDE";
        private const string NYANCAT = "B40C0E49689A0ABD2A51379FED1800F3";
        private const string STONEDv2 = "72B8CE41AF0DE751C946802B3ED844B4";
        private const string STONEDv2_TRUE_CRYPT = "5C7DE5F58B276CBE84B8B7E25F08318E";

        #endregion MbrSignatures

        #region Constants

        private const int PARTITION_TABLE_OFFSET = 0x1BE;
        private const int PARTITION_ENTRY_SIZE = 0x10;

        #endregion Constants

        #region Properties

        public readonly string DiskSignature;
        public readonly byte[] CodeSection;
        public readonly string MbrSignature;
        public readonly PartitionEntry[] PartitionTable;

        #endregion Properties

        #region Constructor

        private MasterBootRecord(byte[] bytes, string drivePath)
        {
            DiskSignature = BitConverter.ToString(Helper.GetSubArray(bytes, 0x1B8, 0x04)).Replace("-", "");
            CodeSection = Helper.GetSubArray(bytes, 0x00, 0x1B8);
            MbrSignature = GetMbrSignature(CodeSection);
            PartitionTable = GetPartitions(bytes, 0, drivePath);
        }

        #endregion Constructor

        #region StaticMethods

        public static byte[] GetBytes(string drivePath)
        {
            return Helper.readSector(drivePath, 0x00, 0x01);
        }

        public static MasterBootRecord Get(string drivePath)
        {
            // Read Master Boot Record (first 512 bytes) from disk
            return new MasterBootRecord(MasterBootRecord.GetBytes(drivePath), drivePath);
        }

        #endregion StaticMethods

        #region PrivateMethods

        private static string GetMbrSignature(byte[] bytes)
        {
            /*switch (Hash.Get(bytes, bytes.Length, "MD5"))
            {
                case WINDOWS5_X:
                    return "Windows 5.X";
                case WINDOWS6_0:
                    return "Windows 6.0";
                case WINDOWS6_1:
                    return "Windows 6.1+";
                case GRUB:
                    return "GRUB";
                case NYANCAT:
                    return "BOOTKIT Nyan Cat";
                case STONEDv2:
                    return "BOOTKIT Stonedv2";
                case STONEDv2_TRUE_CRYPT:
                    return "BOOTKIT Stonedv2";
                default:
                    return "UNKNOWN";
            }*/

            return null;
        }

        private static PartitionEntry[] GetPartitions(byte[] bytes, uint startSector, string drivePath)
        {
            // Instantiate a blank Partition List
            List<PartitionEntry> partitionList = new List<PartitionEntry>();

            for (int i = PARTITION_TABLE_OFFSET; i <= 0x1EE; i += PARTITION_ENTRY_SIZE)
            {
                PartitionEntry entry = new PartitionEntry(bytes, startSector, i);

                try
                {
                    if (entry.SystemId.Contains("EXTENDED"))
                    {
                        partitionList.AddRange(GetExtended(entry, drivePath));
                    }
                    else if (entry.SystemId != "EMPTY")
                    {
                        partitionList.Add(entry);
                    }
                }
                catch
                {

                }
            }

            return partitionList.ToArray();
        }

        private static PartitionEntry[] GetExtended(PartitionEntry entry, string drivePath)
        {
            List<PartitionEntry> pList = new List<PartitionEntry>();

            byte[] extendedBytes = Helper.readSector(drivePath, entry.StartSector, 0x01);
            pList.AddRange(GetPartitions(extendedBytes, entry.StartSector, drivePath));

            return pList.ToArray();
        }

        #endregion PrivateMethods

        #region InstanceMethods

        public PartitionEntry[] GetPartitionTable()
        {
            return this.PartitionTable;
        }

        #endregion InstanceMethods
    }

    #endregion MasterBootRecordClass
}
