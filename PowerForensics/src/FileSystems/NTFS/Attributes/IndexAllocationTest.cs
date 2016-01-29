using System;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    #region IndexAllocationClass

    public class IndexAllocationTest : FileRecordAttribute
    {
        #region Properties

        public readonly IndexEntry[] Entries;

        #endregion Properties

        #region Constructors

        internal IndexAllocationTest(NonResident header, string volume)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = header.NameString;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            // Instantiate empty IndexEntry List
            List<IndexEntry> indexEntryList = new List<IndexEntry>();

            foreach (DataRun dr in header.DataRun)
            {
                // Get IndexAllocation Bytes
                byte[] bytes = dr.GetBytes(volume);
                // Detemine size of Update Sequence
                ushort usOffset = BitConverter.ToUInt16(bytes, + 0x04);
                ushort usSize = BitConverter.ToUInt16(bytes, + 0x06);
                int indexBlockSize = usOffset + (usSize * 2);

                if (indexBlockSize == 0)
                {
                    break;
                }

                //IndexBlock.ApplyFixup(ref bytes, offset);

                // Instantiate IndexBlock Object (Header)
                IndexBlock indexBlock = new IndexBlock(Helper.GetSubArray(bytes, 0x00, indexBlockSize));

                // Create byte array for IndexEntry object
                // 0x18 represents the offset of the EntryOffset value, so it must be added on
                byte[] indexEntryBytes = Helper.GetSubArray(bytes, (int)indexBlock.EntryOffset + 0x18, (int)indexBlock.TotalEntrySize);

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
            Entries = indexEntryList.ToArray();
        }

        #endregion Constructors
    }

    #endregion IndexAllocationClass
}