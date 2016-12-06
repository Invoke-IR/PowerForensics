using System;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectId : FileRecordAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid ObjectIdGuid;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid BirthVolumeId;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid BirthObjectId;

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid BirthDomainId;

        #endregion Properties

        #region Constructors

        internal ObjectId(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            ObjectIdGuid = new Guid(Helper.GetSubArray(bytes, 0x00 + offset, 0x10));

            if (!(bytes.Length < 0x20))
            {
                BirthVolumeId = new Guid(Helper.GetSubArray(bytes, 0x10 + offset, 0x10));

                if (!(bytes.Length < 0x30))
                {
                    BirthObjectId = new Guid(Helper.GetSubArray(bytes, 0x20 + offset, 0x10));

                    if (bytes.Length == 0x40)
                    {
                        BirthDomainId = new Guid(Helper.GetSubArray(bytes, 0x30 + offset, 0x10));
                    }
                }
            }
        }

        #endregion Constructors
    }
}