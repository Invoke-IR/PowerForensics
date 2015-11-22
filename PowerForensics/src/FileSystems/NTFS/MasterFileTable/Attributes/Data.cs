using System;

namespace PowerForensics.Ntfs
{
    #region DataClass

    public class Data : Attr
    {
        #region Properties

        public readonly byte[] RawData;

        #endregion Properties

        #region Constructors

        internal Data(ResidentHeader header, byte[] attrBytes, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            RawData = Util.GetSubArray(attrBytes, 0x00, (uint)header.AttrSize);
        }

        internal Data(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            Array.Copy(bytes, offset, RawData, 0x00, header.AttrSize);
        }

        #endregion Constructors
    }

    #endregion DataClass
}
