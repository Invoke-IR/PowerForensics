using System;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexRoot : FileRecordAttribute
    {
        // ATTR_HEADER_RESIDENT
        // IndexRoot
        // IndexHeader
        // IndexEntry[]

        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum INDEX_ROOT_FLAGS
        {

            /// <summary>
            /// 
            /// </summary>
            INDEX_ROOT_ONLY = 0x00,

            /// <summary>
            /// 
            /// </summary>
            INDEX_ALLOCATION = 0x01
        }

        #endregion Enums

        #region Properties

        // Index Root
        /// <summary>
        /// 
        /// </summary>
        public readonly ATTR_TYPE AttributeType;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CollationSortingRule;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint IndexSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte ClustersPerIndexRecord;

        // IndexHeader
        private readonly int StartOffset;

        private readonly int TotalSize;
        
        private readonly int AllocatedSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly INDEX_ROOT_FLAGS Flags;

        // IndexEntry[]
        private readonly byte[] EntryBytes;

        /// <summary>
        /// 
        /// </summary>
        public readonly IndexEntry[] Entries;

        #endregion Properties

        #region Constructors

        internal IndexRoot(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            // Get ResidentHeader (includes Common Header)
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // IndexRoot
            AttributeType = (ATTR_TYPE)BitConverter.ToUInt32(bytes, 0x00 + offset);
            CollationSortingRule = BitConverter.ToUInt32(bytes, 0x04 + offset);
            IndexSize = BitConverter.ToUInt32(bytes, 0x08 + offset);
            ClustersPerIndexRecord = bytes[0x0C + offset];

            // IndexHeader
            StartOffset = (BitConverter.ToInt32(bytes, 0x10 + offset) + 0x10 + offset);  // Add 0x10 bytes to start offset to account for its offset
            TotalSize = BitConverter.ToInt32(bytes, 0x14 + offset);
            AllocatedSize = BitConverter.ToInt32(bytes, 0x18 + offset);
            Flags = ((INDEX_ROOT_FLAGS)BitConverter.ToUInt32(bytes, 0x1C + offset));

            // IndexEntry[]
            EntryBytes = Helper.GetSubArray(bytes, StartOffset, TotalSize);

            // Iterate through IndexEntry object
            int indexEntryOffset = 0;

            if (AttributeType == ATTR_TYPE.FILE_NAME)
            {
                // Instantiate empty IndexEntry List
                List<IndexEntry> entryList = new List<IndexEntry>();

                while (indexEntryOffset < (EntryBytes.Length - 0x10))
                {
                    // There has to be a better way
                    if(BitConverter.ToUInt16(EntryBytes, 0x0A + indexEntryOffset) == 0)
                    {
                        break;
                    }

                    // Instantiate an IndexEntry Object
                    IndexEntry indexEntry = new IndexEntry(EntryBytes, indexEntryOffset);

                    // Add IndexEntry Object to FileName List
                    entryList.Add(indexEntry);

                    // Increment indexEntryOffset
                    indexEntryOffset += indexEntry.Size;
                }

                Entries = entryList.ToArray();
            }
        }

        #endregion Constructors
    }
}