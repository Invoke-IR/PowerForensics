using System;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    #region IndexRootClass

    public class IndexRoot : FileRecordAttribute
    {
        // ATTR_HEADER_RESIDENT
        // IndexRoot
        // IndexHeader
        // IndexEntry[]

        #region Enums

        [FlagsAttribute]
        public enum INDEX_ROOT_FLAGS
        {
            INDEX_ROOT_ONLY = 0x00,
            INDEX_ALLOCATION = 0x01
        }

        #endregion Enums

        #region Properties

        // Index Root
        public readonly ATTR_TYPE AttributeType;
        public readonly uint CollationSortingRule;
        public readonly uint IndexSize;
        public readonly byte ClustersPerIndexRecord;

        // IndexHeader
        private readonly uint StartOffset;
        private readonly uint TotalSize;
        private readonly uint AllocatedSize;
        public readonly INDEX_ROOT_FLAGS Flags;
        
        // IndexEntry[]
        private readonly byte[] EntryBytes;
        public readonly IndexEntry[] Entries;

        #endregion Properties

        #region Constructors
        
        internal IndexRoot(ResidentHeader header, byte[] attrBytes, string attrName)
        {
            #region ResidentHeader

            // Get ResidentHeader (includes Common Header)
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            
            #endregion ResidentHeader

            #region IndexRoot

            // IndexRoot
            AttributeType = (ATTR_TYPE)BitConverter.ToUInt32(attrBytes, 0x00);
            CollationSortingRule = BitConverter.ToUInt32(attrBytes, 0x04);
            IndexSize = BitConverter.ToUInt32(attrBytes, 0x08);
            ClustersPerIndexRecord = attrBytes[0x0C];

            #endregion IndexRoot

            #region IndexHeader

            // IndexHeader
            StartOffset = (BitConverter.ToUInt32(attrBytes, 0x10) + 0x10);  // Add 0x10 bytes to start offset to account for its offset
            TotalSize = BitConverter.ToUInt32(attrBytes, 0x14);
            AllocatedSize = BitConverter.ToUInt32(attrBytes, 0x18);
            Flags = ((INDEX_ROOT_FLAGS)BitConverter.ToUInt32(attrBytes, 0x1C));
            
            #endregion IndexHeader

            #region IndexEntryArray

            if(TotalSize > StartOffset){
                // IndexEntry[]
                byte[] EntryBytes = Helper.GetSubArray(attrBytes, (int)StartOffset, (int)TotalSize - (int)StartOffset);


                // Iterate through IndexEntry object
                int indexEntryOffset = 0;

                if (AttributeType == ATTR_TYPE.FILE_NAME)
                {
                    // Instantiate empty IndexEntry List
                    List<IndexEntry> entryList = new List<IndexEntry>();

                    while (indexEntryOffset < (EntryBytes.Length - 0x10))
                    {                
                        // Creat byte array representing IndexEntry Object
                        int indexEntrySizeOffset = indexEntryOffset + 0x08;

                        // Instantiate an IndexEntry Object
                        IndexEntry indexEntry = new IndexEntry(Helper.GetSubArray(EntryBytes, indexEntryOffset, BitConverter.ToUInt16(EntryBytes, indexEntrySizeOffset)));

                        // Add IndexEntry Object to FileName List
                        entryList.Add(indexEntry);

                        // Increment indexEntryOffset
                        indexEntryOffset += indexEntry.Size;
                    } 
                
                    Entries = entryList.ToArray();
                }
            }
            #endregion IndexEntryArray
        }

        #endregion Constuctors

    }

    #endregion IndexRootClass
}
