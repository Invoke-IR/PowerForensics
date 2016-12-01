using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class AttrRef
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 
        /// </summary>
        internal readonly ushort RecordLength;

        /// <summary>
        /// 
        /// </summary>
        internal readonly byte AttributeNameLength;

        /// <summary>
        /// 
        /// </summary>
        internal readonly byte AttributeNameOffset;

        /// <summary>
        /// 
        /// </summary>
        internal readonly ulong LowestVCN;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort SequenceNumber;

        /// <summary>
        /// 
        /// </summary>
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
            if(LowestVCN == 0)
            {
                RecordNumber = BitConverter.ToUInt64(bytes, 0x10 + offset) & 0x0000FFFFFFFFFFFF;
                SequenceNumber = BitConverter.ToUInt16(bytes, 0x16 + offset);
                NameString = Encoding.Unicode.GetString(bytes, AttributeNameOffset + offset, AttributeNameLength * 2);
            }
        }

        #endregion Constructors
    }
}