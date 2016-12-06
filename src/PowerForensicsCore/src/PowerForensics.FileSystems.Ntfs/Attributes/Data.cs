using System;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class Data : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] RawData;

        #endregion Properties

        #region Constructors

        internal Data(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;
            RawData = Helper.GetSubArray(bytes, (0x00 + offset), (int)header.AttrSize);
        }

        #endregion Constructors
    }
}
