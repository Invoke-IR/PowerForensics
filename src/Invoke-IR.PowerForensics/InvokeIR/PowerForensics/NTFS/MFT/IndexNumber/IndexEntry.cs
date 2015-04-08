using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InvokeIR.PowerForensics.NTFS
{
    internal class IndexEntry
    {

        enum INDEX_ENTRY_FLAG
        {
            SUBNODE = 0x01,     // Index entry points to a sub-node
            LAST = 0x02         // Last index entry in the node, no Stream
        }

        internal struct INDEX_ENTRY
        {
            internal ulong FileReference;    // Low 6B: MFT record index, High 2B: MFT record sequence number
            internal ushort Size;            // Length of the index entry
            internal ushort StreamSize;      // Length of the stream
            internal byte Flags;             // Flags
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] Padding;         // Padding
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1)]
            internal byte[] Stream;          // Stream
            // VCN of the sub node in Index Allocation, Offset = Size - 8

            internal INDEX_ENTRY(byte[] bytes)
            {
                FileReference = BitConverter.ToUInt64(bytes, 0);
                Size = BitConverter.ToUInt16(bytes, 8);
                StreamSize = BitConverter.ToUInt16(bytes, 10);
                Flags = bytes[12];
                Padding = bytes.Skip(13).Take(3).ToArray();
                Stream = bytes.Skip(16).Take(StreamSize).ToArray();
            }
        }

        struct INDEX_BLOCK
        {
            // Index Block Header
            internal uint Magic;                // "INDX"
            internal ushort OffsetOfUS;           // Offset of Update Sequence
            internal ushort SizeOfUS;             // Size in words of Update Sequence Number & Array
            internal ulong LSN;                  // $LogFile Sequence Number
            internal ulong VCN;                  // VCN of this index block in the index allocation
            // Index Header
            internal uint EntryOffset;          // Offset of the index entries, relative to this address(0x18)
            internal uint TotalEntrySize;       // Total size of the index entries
            internal uint AllocEntrySize;       // Allocated size of index entries
            internal byte NotLeaf;                // 1 if not leaf node (has children)
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] Padding;              // Padding

            internal INDEX_BLOCK(byte[] bytes)
            {
                Magic = BitConverter.ToUInt32(bytes, 0);
                OffsetOfUS = BitConverter.ToUInt16(bytes, 4);
                SizeOfUS = BitConverter.ToUInt16(bytes, 6);
                LSN = BitConverter.ToUInt64(bytes, 8);
                VCN = BitConverter.ToUInt64(bytes, 16);
                EntryOffset = BitConverter.ToUInt32(bytes, 24);
                TotalEntrySize = BitConverter.ToUInt32(bytes, 28);
                AllocEntrySize = BitConverter.ToUInt32(bytes, 32);
                NotLeaf = bytes[36];
                Padding = bytes.Skip(37).Take(3).ToArray();
            }
        }

        internal ulong FileIndex;
        internal string Flags;
        internal string Name;

        internal IndexEntry(INDEX_ENTRY indxEntry, string flag, string name)
        {
            FileIndex = (indxEntry.FileReference & 0x0000FFFFFFFFFFFF);
            Flags = flag;
            Name = name;
        }

        internal static List<IndexEntry> Get(FileStream streamToRead, byte[] MFT, int index)
        {

            MFTRecord fileRecord = MFTRecord.Get(MFT, index, null, null);

            NonResident INDX = null;

            Console.WriteLine("Count: {0}", fileRecord.Attribute.Length);

            foreach (Attr attr in fileRecord.Attribute)
            {

                if (attr.Name == "INDEX_ALLOCATION")
                {

                    if (attr.NonResident)
                    {

                        INDX = (NonResident)attr;

                    }

                }

            }

            byte[] nonResBytes = NonResident.GetContent(streamToRead, INDX);

            List<IndexEntry> indxEntryList = new List<IndexEntry>();

            for (int offset = 0; offset < nonResBytes.Length; offset += 4096)
            {

                byte[] indxBytes = nonResBytes.Skip(offset).Take(4096).ToArray();

                INDEX_BLOCK indxBlock = new INDEX_BLOCK(indxBytes.Take(40).ToArray());

                byte[] IndexAllocEntryBytes = indxBytes.Skip(64).ToArray();

                int offsetIndx = 0;
                int offsetIndxPrev = 1;

                while ((offsetIndx < IndexAllocEntryBytes.Length) && (offsetIndx != offsetIndxPrev))
                {

                    INDEX_ENTRY indxEntryStruct = new INDEX_ENTRY(IndexAllocEntryBytes.Skip(offsetIndx).ToArray());

                    offsetIndxPrev = offsetIndx;
                    offsetIndx += indxEntryStruct.Size;
                    if (indxEntryStruct.Stream.Length > 66)
                    {

                        FileName.ATTR_FILE_NAME fileNameStruct = new FileName.ATTR_FILE_NAME(indxEntryStruct.Stream);

                        #region indxFlags

                        StringBuilder indxFlags = new StringBuilder();
                        if (indxEntryStruct.Flags != 0)
                        {
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.SUBNODE) == (int)INDEX_ENTRY_FLAG.SUBNODE)
                            {
                                indxFlags.Append("Subnode, ");
                            }
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.LAST) == (int)INDEX_ENTRY_FLAG.LAST)
                            {
                                indxFlags.Append("Last Entry, ");
                            }
                            indxFlags.Length -= 2;
                        }

                        #endregion indxFlags

                        string Name = System.Text.Encoding.Unicode.GetString(fileNameStruct.Name);
                        IndexEntry indxEntry = new IndexEntry(indxEntryStruct, indxFlags.ToString(), Name);
                        indxEntryList.Add(indxEntry);

                    }

                }

            }

            return indxEntryList;
        }

        internal static List<IndexEntry> Get(string volume, int index)
        {

            MFTRecord fileRecord = MFTRecord.Get(MasterFileTable.GetBytes(volume), index, null, null);

            NonResident INDX = null;

            foreach (Attr attr in fileRecord.Attribute)
            {

                if (attr.Name == "INDEX_ALLOCATION")
                {
                    if (attr.NonResident)
                    {

                        INDX = (NonResident)attr;

                    }

                }

            }

            List<byte> nonResBytes = NonResident.GetContent(volume, INDX);

            List<IndexEntry> indxEntryList = new List<IndexEntry>();

            for (int offset = 0; offset < nonResBytes.Count; offset += 4096)
            {

                byte[] indxBytes = nonResBytes.Skip(offset).Take(4096).ToArray();

                INDEX_BLOCK indxBlock = new INDEX_BLOCK(indxBytes.Take(40).ToArray());

                byte[] IndexAllocEntryBytes = indxBytes.Skip(64).ToArray();

                int offsetIndx = 0;
                int offsetIndxPrev = 1;

                while ((offsetIndx < IndexAllocEntryBytes.Length) && (offsetIndx != offsetIndxPrev))
                {

                    INDEX_ENTRY indxEntryStruct = new INDEX_ENTRY(IndexAllocEntryBytes.Skip(offsetIndx).ToArray());

                    offsetIndxPrev = offsetIndx;
                    offsetIndx += indxEntryStruct.Size;
                    if (indxEntryStruct.Stream.Length > 66)
                    {

                        FileName.ATTR_FILE_NAME fileNameStruct = new FileName.ATTR_FILE_NAME(indxEntryStruct.Stream);

                        #region indxFlags

                        StringBuilder indxFlags = new StringBuilder();
                        if (indxEntryStruct.Flags != 0)
                        {
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.SUBNODE) == (int)INDEX_ENTRY_FLAG.SUBNODE)
                            {
                                indxFlags.Append("Subnode, ");
                            }
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.LAST) == (int)INDEX_ENTRY_FLAG.LAST)
                            {
                                indxFlags.Append("Last Entry, ");
                            }
                            indxFlags.Length -= 2;
                        }

                        #endregion indxFlags

                        string Name = System.Text.Encoding.Unicode.GetString(fileNameStruct.Name);
                        IndexEntry indxEntry = new IndexEntry(indxEntryStruct, indxFlags.ToString(), Name);
                        indxEntryList.Add(indxEntry);

                    }

                }

            }

            return indxEntryList;
        }

    }

}
