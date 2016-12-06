using System;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.FileSystems.HFSPlus.BTree
{
    /// <summary>
    /// 
    /// </summary>
    public class Node
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private readonly string VolumeName;

        /// <summary>
        /// 
        /// </summary>
        private readonly string FileName;

        /// <summary>
        /// 
        /// </summary>
        private readonly uint NodeNumber;

        /// <summary>
        /// 
        /// </summary>
        public NodeDescriptor NodeDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public Record[] Records;

        #endregion Properties

        #region Constructors

        private Node(byte[] bytes, string volumeName, string fileName, uint nodeNumber)
        {
            VolumeName = volumeName;
            FileName = fileName;
            NodeNumber = nodeNumber;

            NodeDescriptor = NodeDescriptor.Get(bytes, 0x00, volumeName, fileName);

            List<Record> recordList = new List<Record>();

            switch (NodeDescriptor.Kind)
            {
                case NodeDescriptor.NODE_KIND.kBTHeaderNode:
                    recordList.Add(HeaderRecord.Get(bytes, Helper.SwapEndianness(BitConverter.ToUInt16(bytes, bytes.Length - 0x02)), volumeName, fileName));
                    recordList.Add(UserDataRecord.Get(bytes, Helper.SwapEndianness(BitConverter.ToUInt16(bytes, bytes.Length - 0x04))));
                    //recordList.Add(Helper.GetSubArray(bytes, Helper.SwapEndianness(BitConverter.ToUInt16(bytes, bytes.Length - 0x06)), ));
                    break;

                case NodeDescriptor.NODE_KIND.kBTIndexNode:
                    for (int i = 1; i <= NodeDescriptor.NumRecords; i++)
                    {
                        ushort recordOffset = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, bytes.Length - (0x02 * i)));
                        recordList.Add(PointerRecord.Get(bytes, recordOffset, volumeName, fileName));
                    }
                    break;

                case NodeDescriptor.NODE_KIND.kBTLeafNode:
                    for (int i = 1; i <= NodeDescriptor.NumRecords; i++)
                    {
                        ushort recordOffset = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, bytes.Length - (0x02 * i)));

                        switch (fileName)
                        {
                            case "Catalog":
                                switch (DataRecord.GetRecordType(bytes, recordOffset))
                                {
                                    case DataRecord.RECORD_TYPE.kHFSPlusFolderRecord:
                                        recordList.Add(CatalogFolderRecord.Get(bytes, recordOffset, volumeName, fileName));
                                        break;
                                    case DataRecord.RECORD_TYPE.kHFSPlusFileRecord:
                                        recordList.Add(CatalogFileRecord.Get(bytes, recordOffset, volumeName, fileName));
                                        break;
                                    case DataRecord.RECORD_TYPE.kHFSPlusFolderThreadRecord:
                                        recordList.Add(CatalogThread.Get(bytes, recordOffset, volumeName, fileName));
                                        break;
                                    case DataRecord.RECORD_TYPE.kHFSPlusFileThreadRecord:
                                        recordList.Add(CatalogThread.Get(bytes, recordOffset, volumeName, fileName));
                                        break;
                                }
                                break;
                            case "ExtentsOverflow":
                                recordList.Add(ExtentsOverflowRecord.Get(bytes, recordOffset, volumeName, fileName));
                                break;
                        }
                    }
                    break;
                case NodeDescriptor.NODE_KIND.kBTMapNode:
                    break;
            }

            Records = recordList.ToArray();
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        internal static Node Get(string volumeName, string fileName, uint nodeNumber)
        {
            VolumeHeader volHeader = VolumeHeader.Get(volumeName);

            Node headerNode = null;

            switch (fileName)
            {
                case "Catalog":
                    headerNode = CatalogFile.GetHeaderNode(volumeName);
                    break;
                case "Attributes":
                    headerNode = AttributesFile.GetHeaderNode(volumeName);
                    break;
                case "ExtentsOverflow":
                    headerNode = ExtentsOverflowFile.GetHeaderNode(volumeName);
                    break;
            }

            HeaderRecord headerRecord = headerNode.Records[0] as HeaderRecord;
            return Get(GetBytes(volumeName, fileName, nodeNumber), volumeName, fileName, nodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        private static Node Get(byte[] bytes, string volumeName, string fileName, uint nodeNumber)
        {
            return new Node(bytes, volumeName, fileName, nodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static Node GetHeaderNode(string volumeName, string fileName)
        {
            return Get(GetHeaderBytes(volumeName, fileName), volumeName, fileName, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static byte[] GetHeaderBytes(string volumeName, string fileName)
        {
            VolumeHeader volHeader = VolumeHeader.Get(volumeName);

            ExtentDescriptor extent = null;

            switch (fileName)
            {
                case "Catalog":
                    extent = volHeader.CatalogFile.Extents[0];
                    break;
                case "Attributes":
                    extent = volHeader.AttributesFile.Extents[0];
                    break;
                case "ExtentsOverflow":
                    extent = volHeader.ExtentsOverflowFile.Extents[0];
                    break;
            }

            // Read the smallest possible amount of bytes
            byte[] firstSectorBytes = Helper.readDrive(volumeName, extent.StartBlock * volHeader.BlockSize, 0x200);
            // Parse HeaderRecord to determine NodeSize
            HeaderRecord headerRecord = HeaderRecord.Get(firstSectorBytes, 0x0E, volumeName, fileName);

            // Read the full Header Node
            return Helper.readDrive(volumeName, extent.StartBlock * volHeader.BlockSize, headerRecord.NodeSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        internal static byte[] GetBytes(string volumeName, string fileName, uint nodeNumber)
        {
            VolumeHeader volHeader = VolumeHeader.Get(volumeName);

            ExtentDescriptor[] extents = null;

            switch (fileName)
            {
                case "Catalog":
                    extents = volHeader.CatalogFile.Extents;
                    break;
                case "Attributes":
                    extents = volHeader.AttributesFile.Extents;
                    break;
                case "ExtentsOverflow":
                    extents = volHeader.ExtentsOverflowFile.Extents;
                    break;
            }

            Node headerNode = GetHeaderNode(volumeName, fileName);
            HeaderRecord headerRecord = headerNode.Records[0] as HeaderRecord;

            // Determine which blocks contain desired node's bytes
            uint blockNumber = nodeNumber * (headerRecord.NodeSize / volHeader.BlockSize);

            // Starting Position within the extents
            uint extentPosition = 0;

            // Iterate through extents to determine which extent conatians the Node
            foreach (ExtentDescriptor extent in extents)
            {
                uint relBlock = blockNumber - extentPosition;

                if (relBlock < extent.BlockCount)
                {
                    return Helper.readDrive(volumeName, (long)(extent.StartBlock + relBlock) * volHeader.BlockSize, headerRecord.NodeSize);
                }
                else
                {
                    extentPosition += extent.BlockCount;
                }
            }

            // Need to throw an "Invalid Node Number" error
            return null;
        }

        #endregion Static Methods

        #region Instance Methods

        /*public byte[] GetSlack()
        {
            VolumeHeader volHeader = VolumeHeader.Get(VolumeName);

            HeaderRecord headerRecord = GetHeaderNode(VolumeName, FileName).Records[0] as HeaderRecord;

            byte[] bytes = GetBytes(VolumeName, FileName, NodeNumber);
            Node node = Get(bytes, VolumeName, FileName, NodeNumber);

            int indexOffset = (node.NodeDescriptor.NumRecords + 1) * 0x02;
            uint freespaceOffset = 
            uint freespaceLength = 

            Console.WriteLine(freespaceOffset);
            Console.WriteLine(freespaceLength);

            return Helper.GetSubArray(bytes, freespaceOffset, freespaceLength);
        }*/

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class NodeDescriptor
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum NODE_KIND
        {
            /// <summary>
            /// 
            /// </summary>
            kBTLeafNode = -1,

            /// <summary>
            /// 
            /// </summary>
            kBTIndexNode = 0,

            /// <summary>
            /// 
            /// </summary>
            kBTHeaderNode = 1,

            /// <summary>
            /// 
            /// </summary>
            kBTMapNode = 2
        }

        #endregion Enums

        #region Properties

        private readonly string VolumeName;

        private readonly string FileName;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint fLink;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint bLink;

        /// <summary>
        /// 
        /// </summary>
        public readonly NODE_KIND Kind;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte Height;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort NumRecords;

        #endregion Properties

        #region Constructors

        private NodeDescriptor(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeName = volumeName;
            FileName = fileName;

            fLink = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset));
            bLink = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x04));
            Kind = (NODE_KIND)(sbyte)bytes[offset + 0x08];
            Height = bytes[offset + 0x09];
            NumRecords = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x0A));
        }

        #endregion Constructors

        #region Static Methods

        internal static NodeDescriptor Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new NodeDescriptor(bytes, offset, volumeName, fileName);
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node GetfLink()
        {
            if (fLink == 0)
            {
                throw new Exception("No fLink. This is the last node.");
            }
            else
            {
                return Node.Get(VolumeName, FileName, fLink);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node GetbLink()
        {
            if (bLink == 0)
            {
                throw new Exception("No bLink. This is the first node.");
            }
            else
            {
                return Node.Get(VolumeName, FileName, bLink);
            }
        }

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class Record
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class HeaderRecord : Record
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum BTREE_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            kHFSBTreeType = 0,

            /// <summary>
            /// 
            /// </summary>
            kUserBTreeType = 128,

            /// <summary>
            /// 
            /// </summary>
            kReservedBTreeType = 255
        }

        /// <summary>
        /// 
        /// </summary>
        public enum BTREE_KEYCOMPARE
        {
            /// <summary>
            /// 
            /// </summary>
            kHFSBinaryCompare = 0xBC, //Case Sensitive

            /// <summary>
            /// 
            /// </summary>
            kHFSCaseFolding = 0xCF //Case-Insensitive
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum BTREE_ATTRIBUTE
        {
            /// <summary>
            /// 
            /// </summary>
            kBTBadCloseMask = 0x00000001,

            /// <summary>
            /// 
            /// </summary>
            kBTBigKeysMask = 0x00000002,

            /// <summary>
            /// 
            /// </summary>
            kBTVariableIndexKeysMask = 0x00000004
        }

        #endregion Enums

        #region Properties

        private readonly string VolumeName;

        private readonly string FileName;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort TreeDepth;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RootNode;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LeafRecords;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FirstLeafNode;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LastLeafNode;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort NodeSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort MaxKeyLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint TotalNodes;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint FreeNodes;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClumpSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly BTREE_TYPE BTreeType;

        /// <summary>
        /// 
        /// </summary>
        public readonly BTREE_KEYCOMPARE KeyCompareType;

        /// <summary>
        /// 
        /// </summary>
        public readonly BTREE_ATTRIBUTE Attributes;

        #endregion Properties

        #region Constructors

        private HeaderRecord(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeName = volumeName;
            FileName = fileName;
            TreeDepth = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            RootNode = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x02));
            LeafRecords = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x06));
            FirstLeafNode = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x0A));
            LastLeafNode = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x0E));
            NodeSize = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x12));
            MaxKeyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x14));
            TotalNodes = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x16));
            FreeNodes = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x1A));
            ClumpSize = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x20));
            BTreeType = (BTREE_TYPE)bytes[offset + 0x24];
            KeyCompareType = (BTREE_KEYCOMPARE)bytes[offset + 0x25];
            Attributes = (BTREE_ATTRIBUTE)Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x26));
        }

        #endregion Constructors

        #region Static Methods

        internal static HeaderRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new HeaderRecord(bytes, offset, volumeName, fileName);
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node GetRootNode()
        {
            return Node.Get(VolumeName, FileName, RootNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node GetFirstLeafNode()
        {
            return Node.Get(VolumeName, FileName, FirstLeafNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node GetLastLeafNode()
        {
            return Node.Get(VolumeName, FileName, LastLeafNode);
        }

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserDataRecord : Record
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] UserData;

        #endregion Properties

        #region Constructors

        private UserDataRecord(byte[] bytes, int offset)
        {
            UserData = Helper.GetSubArray(bytes, offset, 0x80);
        }

        #endregion Constructors

        #region Static Methods

        internal static UserDataRecord Get(byte[] bytes, int offset)
        {
            return new UserDataRecord(bytes, offset);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class MapRecord : Record
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class KeyedRecord : Record
    {
        #region Properties

        internal string VolumeName;

        internal string FileName;

        internal ushort KeyLength;

        /// <summary>
        /// 
        /// </summary>
        public uint ParentCatalogNodeId;

        /// <summary>
        /// 
        /// </summary>
        public string Name;

        #endregion Properties

        #region Static Methods

        internal static string GetHfsString(byte[] bytes, int offset)
        {
            ushort length = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            return Encoding.BigEndianUnicode.GetString(bytes, offset + 0x02, length * 2);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class PointerRecord : KeyedRecord
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly uint NodeNumber;

        #endregion Properties

        #region Constructors

        private PointerRecord(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeName = volumeName;
            FileName = fileName;
            KeyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            ParentCatalogNodeId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x02));
            Name = GetHfsString(bytes, offset + 0x06);
            NodeNumber = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x02 + KeyLength));
        }

        #endregion Constructors

        #region Static Methods

        internal static PointerRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new PointerRecord(bytes, offset, volumeName, fileName);
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Node GetChildNodes()
        {
            return Node.Get(VolumeName, FileName, NodeNumber);
        }

        #endregion Instance Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataRecord : KeyedRecord
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum RECORD_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            kHFSPlusFolderRecord = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            kHFSPlusFileRecord = 0x0002,

            /// <summary>
            /// 
            /// </summary>
            kHFSPlusFolderThreadRecord = 0x0003,

            /// <summary>
            /// 
            /// </summary>
            kHFSPlusFileThreadRecord = 0x0004
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum RECORD_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            kHFSFileLockedBit = 0x0000,

            /// <summary>
            /// 
            /// </summary>
            kHFSFileLockedMask = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            kHFSThreadExistsBit = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            kHFSThreadExistsMask = 0x0002
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public RECORD_TYPE RecordType;

        #endregion Properties

        #region Constructors

        /*internal DataRecord(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeName = volumeName;
            FileName = fileName;
            KeyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            ParentCnid = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x02));
            Name = GetHfsString(bytes, offset + 0x06);
            RecordType = (RECORD_TYPE)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + KeyLength + 0x02));
        }*/

        #endregion Constructors

        #region Static Methods

        /*internal static DataRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new DataRecord(bytes, offset, volumeName, fileName);
        }*/

        internal static RECORD_TYPE GetRecordType(byte[] bytes, int offset)
        {
            ushort keyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset));
            return (RECORD_TYPE)Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + keyLength + 0x02));
        }

        #endregion Static Methods
    }
}
