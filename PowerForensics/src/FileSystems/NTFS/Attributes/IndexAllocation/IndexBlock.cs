using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region IndexBlockClass

    internal class IndexBlock
    {
        #region Properties

        // Index Block Header
        internal readonly string Signature;         // "INDX"
        internal ushort OffsetOfUS;                 // Offset of Update Sequence
        internal ushort SizeOfUS;                   // Size in words of Update Sequence Number & Array
        internal readonly ushort UpdateSequenceNumber;
        internal readonly byte[] UpdateSequenceArray;
        internal readonly ulong LSN;                // $LogFile Sequence Number
        internal readonly ulong VCN;                // VCN of this index block in the index allocation

        // Index Header
        internal readonly uint EntryOffset;         // Offset of the index entries, relative to this address(0x18)
        internal readonly uint TotalEntrySize;      // Total size of the index entries
        internal readonly uint AllocEntrySize;      // Allocated size of index entries
        internal readonly byte NotLeaf;             // 1 if not leaf node (has children)

        #endregion Properties

        #region Constructors

        internal IndexBlock(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x04);
            OffsetOfUS = BitConverter.ToUInt16(bytes, 0x04);
            SizeOfUS = BitConverter.ToUInt16(bytes, 0x06);
            UpdateSequenceNumber = BitConverter.ToUInt16(bytes, OffsetOfUS);
            UpdateSequenceArray = Helper.GetSubArray(bytes, (OffsetOfUS + 2), (2 * SizeOfUS) - 2);
            LSN = BitConverter.ToUInt64(bytes, 0x08);
            VCN = BitConverter.ToUInt64(bytes, 0x10);

            // Index Header
            EntryOffset = BitConverter.ToUInt32(bytes, 0x18);
            TotalEntrySize = BitConverter.ToUInt32(bytes, 0x1C);
            AllocEntrySize = BitConverter.ToUInt32(bytes, 0x20);
            NotLeaf = bytes[0x24];
        }

        #endregion Constructors

        #region StaticMethods

        internal static void ApplyFixup(ref byte[] bytes, int offset)
        {
            // Take UpdateSequence into account
            ushort usoffset = BitConverter.ToUInt16(bytes, 4);
            ushort ussize = BitConverter.ToUInt16(bytes, 6);

            if (ussize != 0)
            {
                ushort UpdateSequenceNumber = BitConverter.ToUInt16(bytes, usoffset + offset);
                byte[] UpdateSequenceArray = Helper.GetSubArray(bytes, (usoffset + 2 + offset), (2 * ussize));

                bytes[0x1FE + offset] = UpdateSequenceArray[0];
                bytes[0x1FF + offset] = UpdateSequenceArray[1];
                bytes[0x3FE + offset] = UpdateSequenceArray[2];
                bytes[0x3FF + offset] = UpdateSequenceArray[3];
                bytes[0x5FE + offset] = UpdateSequenceArray[4];
                bytes[0x5FF + offset] = UpdateSequenceArray[5];
                bytes[0x7FE + offset] = UpdateSequenceArray[6];
                bytes[0x7FF + offset] = UpdateSequenceArray[7];
                bytes[0x9FE + offset] = UpdateSequenceArray[8];
                bytes[0x9FF + offset] = UpdateSequenceArray[9];
                bytes[0xBFE + offset] = UpdateSequenceArray[10];
                bytes[0xBFF + offset] = UpdateSequenceArray[11];
                bytes[0xDFE + offset] = UpdateSequenceArray[12];
                bytes[0xDFF + offset] = UpdateSequenceArray[13];
                bytes[0xFFE + offset] = UpdateSequenceArray[14];
                bytes[0xFFF + offset] = UpdateSequenceArray[15];
            }
        }

        #endregion StaticMethods
    }

    #endregion IndexBlockClass
}
