using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS.MFT.Attributes
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

        public byte[] ObjectIdBytes;

        internal ObjectId(uint ATTRType, string name, bool nonResident, byte[] objectId)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            ObjectIdBytes = objectId;
        }

        internal static ObjectId Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_OBJECT_ID objectId = new ATTR_OBJECT_ID(AttrBytes);

            return new ObjectId(
                objectId.header.commonHeader.ATTRType,
                AttrName,
                objectId.header.commonHeader.NonResident,
                objectId.ObjectId);

        }

    }

}
