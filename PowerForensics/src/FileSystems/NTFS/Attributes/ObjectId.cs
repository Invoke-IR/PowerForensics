using System;

namespace PowerForensics.Ntfs
{
    #region ObjectIdClass

    public class ObjectId : FileRecordAttribute
    {
        #region Properties

        public readonly Guid ObjectIdGuid;
        public readonly Guid BirthVolumeId;
        public readonly Guid BirthObjectId;
        public readonly Guid BirthDomainId;

        #endregion Properties

        #region Constructors

        internal ObjectId(ResidentHeader header, byte[] bytes, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;


            ObjectIdGuid = new Guid(Helper.GetSubArray(bytes, 0x00, 0x10));
            
            if (!(bytes.Length < 0x20))
            {
                BirthVolumeId = new Guid(Helper.GetSubArray(bytes, 0x10, 0x10));
                
                if (!(bytes.Length < 0x30))
                {
                    BirthObjectId = new Guid(Helper.GetSubArray(bytes, 0x20, 0x10));

                    if(bytes.Length == 0x40)
                    {
                        BirthDomainId = new Guid(Helper.GetSubArray(bytes, 0x30, 0x10));
                    }
                }
            }
        }

        internal ObjectId(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

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

    #endregion ObjectIdClass
}
