using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{
    public class ObjectId : Attr
    {

        struct ATTR_OBJECT_ID
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal byte[] ObjectId;
            internal byte[] BirthVolumeId;
            internal byte[] BirthObjectId;
            internal byte[] BirthDomainId;

            internal ATTR_OBJECT_ID(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                ObjectId = bytes.Skip(24).Take(16).ToArray();
                BirthVolumeId = null;
                BirthObjectId = null;
                BirthDomainId = null;
            }

        }

        #region Properties

        public readonly byte[] ObjectIdBytes;

        #endregion Properties

        #region Constructors

        internal ObjectId(byte[] AttrBytes, string AttrName)
        {
            ATTR_OBJECT_ID objectId = new ATTR_OBJECT_ID(AttrBytes);

            Name = Enum.GetName(typeof(ATTR_TYPE), objectId.header.commonHeader.ATTRType);
            NameString = AttrName;
            NonResident = objectId.header.commonHeader.NonResident;
            AttributeId = objectId.header.commonHeader.Id;
            ObjectIdBytes = objectId.ObjectId;
        }

        #endregion Constructors

    }

}
