using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS.MFT.Attributes
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

        public byte[] RawData;

        internal Data(uint ATTRType, string name, bool nonResident, byte[] bytes)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            RawData = bytes;
        }

        internal static Data Get(byte[] AttrBytes, string attrName)
        {

            ATTR_DATA data = new ATTR_DATA(AttrBytes);

            return new Data(
                data.header.commonHeader.ATTRType,
                attrName,
                data.header.commonHeader.NonResident,
                data.RawBytes);
        }

    }

}
