using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region AttrRefClass

    public class AttrRef
    {
        #region Properties

        public readonly string Name;
        internal readonly ushort RecordLength;
        internal readonly byte AttributeNameLength;
        internal readonly byte AttributeNameOffset;
        internal readonly ulong LowestVCN;
        public readonly ulong RecordNumber;
        public readonly ushort SequenceNumber;
        public readonly string NameString;

        #endregion Properties

        #region Constructors

        internal AttrRef(byte[] bytes, int offset)
        {
            Name = Enum.GetName(typeof(FileRecordAttribute.ATTR_TYPE), BitConverter.ToInt32(bytes, 0x00 + offset));
            RecordLength = BitConverter.ToUInt16(bytes, 0x04 + offset);
            AttributeNameLength = bytes[0x06 + offset];
            AttributeNameOffset = bytes[0x07 + offset];
            LowestVCN = BitConverter.ToUInt64(bytes, 0x08 + offset);
            RecordNumber = BitConverter.ToUInt64(bytes, 0x10 + offset) & 0x0000FFFFFFFFFFFF;
            SequenceNumber = BitConverter.ToUInt16(bytes, 0x16 + offset);
            NameString = Encoding.Unicode.GetString(bytes, AttributeNameOffset + offset, AttributeNameLength * 2);
        }

        #endregion Constructors
    }

    #endregion AttrRefClass
}
