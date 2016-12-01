using System;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexAllocation : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly IndexEntry[] Entries;

        #endregion Properties

        #region Constructors

        internal IndexAllocation(NonResident header, string volume)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = header.NameString;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // Get IndexAllocation Bytes
            byte[] bytes = header.GetBytes();

            // Instantiate empty IndexEntry List
            List<IndexEntry> indexEntryList = new List<IndexEntry>();

            // Iterate through IndexBlocks (4096 bytes in size)
            for (int offset = 0; offset < bytes.Length; offset += 4096)
            {
                // Detemine size of Update Sequence
                ushort usOffset = BitConverter.ToUInt16(bytes, offset + 0x04);
                ushort usSize = BitConverter.ToUInt16(bytes, offset + 0x06);
                int indexBlockSize = usOffset + (usSize * 2);

                if (indexBlockSize == 0)
                {
                    break;
                }

                IndexBlock.ApplyFixup(ref bytes, offset);

                // Instantiate IndexBlock Object (Header)
                IndexBlock indexBlock = new IndexBlock(Helper.GetSubArray(bytes, offset, indexBlockSize));

                if (indexBlock.Signature == "INDX")
                {
                    // Create byte array for IndexEntry object
                    // 0x18 represents the offset of the EntryOffset value, so it must be added on
                    byte[] indexEntryBytes = Helper.GetSubArray(bytes, offset + (int)indexBlock.EntryOffset + 0x18, (int)indexBlock.TotalEntrySize);

                    int entryOffset = 0;

                    do
                    {
                        // Instantiate an IndexEntry Object
                        IndexEntry indexEntry = new IndexEntry(Helper.GetSubArray(indexEntryBytes, entryOffset, BitConverter.ToUInt16(indexEntryBytes, entryOffset + 0x08)));
                        entryOffset += indexEntry.Size;

                        // Check if entry is the last in the Entry array
                        if (indexEntry.Flags == 0x02 || indexEntry.Flags == 0x03)
                        {
                            break;
                        }

                        // Add IndexEntry Object to list
                        indexEntryList.Add(indexEntry);

                    } while (entryOffset < indexEntryBytes.Length);
                }
            }
            Entries = indexEntryList.ToArray();
        }

        #endregion Constructors
    }
}