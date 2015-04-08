using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{

    public class Data : Attr
    {
        struct ATTR_DATA
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal byte[] RawBytes;

            internal ATTR_DATA(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                RawBytes = bytes.Skip(24).ToArray();
            }

        }

        #region Properties

        public readonly byte[] RawData;

        #endregion Properties

        #region Constructors

        internal Data(byte[] AttrBytes, string attrName)
        {
            ATTR_DATA data = new ATTR_DATA(AttrBytes);

            Name = Enum.GetName(typeof(ATTR_TYPE), data.header.commonHeader.ATTRType);
            NameString = attrName;
            NonResident = data.header.commonHeader.NonResident;
            AttributeId = data.header.commonHeader.Id;
            RawData = data.RawBytes;
        }

        #endregion Constructors

    }

}
