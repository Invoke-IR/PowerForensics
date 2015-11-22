using System;

namespace PowerForensics.Ntfs
{
    #region ResidentHeaderClass

    class ResidentHeader
    {
        #region Properties

        internal CommonHeader commonHeader;
        internal uint AttrSize;
        internal ushort AttrOffset;
        internal byte IndexedFlag;

        #endregion Properties

        #region Constructors

        internal ResidentHeader(byte[] bytes, CommonHeader common)
        {
            commonHeader = common;
            AttrSize = BitConverter.ToUInt32(bytes, 0);
            AttrOffset = BitConverter.ToUInt16(bytes, 4);
            IndexedFlag = bytes[6];
        }

        #endregion Constructors
    }

    #endregion ResidentHeaderClass
}
