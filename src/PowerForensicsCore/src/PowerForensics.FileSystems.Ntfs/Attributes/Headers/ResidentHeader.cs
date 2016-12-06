using System;

namespace PowerForensics.FileSystems.Ntfs
{
    class ResidentHeader
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        internal CommonHeader commonHeader;

        /// <summary>
        /// 
        /// </summary>
        internal uint AttrSize;

        /// <summary>
        /// 
        /// </summary>
        internal ushort AttrOffset;

        /// <summary>
        /// 
        /// </summary>
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

        internal ResidentHeader(byte[] bytes, CommonHeader common, int offset)
        {
            commonHeader = common;
            AttrSize = BitConverter.ToUInt32(bytes, 0x00 + offset);
            AttrOffset = BitConverter.ToUInt16(bytes, 0x04 + offset);
            IndexedFlag = bytes[0x06 + offset];
        }

        #endregion Constructors
    }
}