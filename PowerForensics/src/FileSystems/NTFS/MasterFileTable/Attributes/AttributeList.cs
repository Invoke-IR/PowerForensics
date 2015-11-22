using System;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    #region AttributeListClass

    public class AttributeList : Attr
    {
        #region Properties

        public readonly AttrRef[] AttributeReference;

        #endregion Properties

        #region Constructors

        internal AttributeList(ResidentHeader header, byte[] attrBytes, string attrName)
        {
            Name = (Attr.ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            #region AttributeReference
            
            int i = 0;
            List<AttrRef> refList = new List<AttrRef>();

            while (i < attrBytes.Length)
            {
                AttrRef attrRef = new AttrRef(Util.GetSubArray(attrBytes, (uint)i, (uint)BitConverter.ToUInt16(attrBytes, i + 0x04)));
                refList.Add(attrRef);
                i += attrRef.RecordLength;
            }
            AttributeReference = refList.ToArray();

            #endregion AttributeReference
        }

        #endregion Constructors
    }

    #endregion AttributeListClass

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

        internal AttrRef(byte[] bytes)
        {
            Name = Enum.GetName(typeof(Attr.ATTR_TYPE), BitConverter.ToInt32(bytes, 0x00));
            RecordLength = BitConverter.ToUInt16(bytes, 0x04);
            AttributeNameLength = bytes[0x06];
            AttributeNameOffset = bytes[0x07];
            LowestVCN = BitConverter.ToUInt64(bytes, 0x08);
            RecordNumber = BitConverter.ToUInt64(bytes, 0x10) & 0x0000FFFFFFFFFFFF;
            SequenceNumber = BitConverter.ToUInt16(bytes, 0x16);
            NameString = Encoding.Unicode.GetString(bytes, AttributeNameOffset, AttributeNameLength * 2);
        }

        #endregion Constructors
    }

    #endregion AttrRefClass
}
