using System;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class AttributeList : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly AttrRef[] AttributeReference;

        #endregion Properties

        #region Constructors

        internal AttributeList(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (FileRecordAttribute.ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            #region AttributeReference

            int i = offset;
            List<AttrRef> refList = new List<AttrRef>();
            
            while (i < offset + header.AttrSize)
            {
                AttrRef attrRef = new AttrRef(bytes, i);
                refList.Add(attrRef);
                i += attrRef.RecordLength;
            }
            AttributeReference = refList.ToArray();

            #endregion AttributeReference
        }

        internal AttributeList(NonResident nonRes)
        {
            Name = (FileRecordAttribute.ATTR_TYPE)nonRes.Name;
            NameString = nonRes.NameString;
            NonResident = nonRes.NonResident;
            AttributeId = nonRes.AttributeId;
            AttributeSize = nonRes.AttributeSize;

            #region AttributeReference

            List<AttrRef> refList = new List<AttrRef>();

            byte[] bytes = nonRes.GetBytes();

            int i = 0;

            while (i < bytes.Length)
            {
                AttrRef attrRef = new AttrRef(bytes, i);
                refList.Add(attrRef);
                i += attrRef.RecordLength;
            }
            AttributeReference = refList.ToArray();

            #endregion AttributeReference
        }

        #endregion Constructors
    }

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

        internal readonly ushort RecordLength;

        internal readonly byte AttributeNameLength;

        internal readonly byte AttributeNameOffset;

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
            if (LowestVCN == 0)
            {
                RecordNumber = BitConverter.ToUInt64(bytes, 0x10 + offset) & 0x0000FFFFFFFFFFFF;
                SequenceNumber = BitConverter.ToUInt16(bytes, 0x16 + offset);
                NameString = Encoding.Unicode.GetString(bytes, AttributeNameOffset + offset, AttributeNameLength * 2);
            }
        }

        #endregion Constructors
    }
}