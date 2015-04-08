using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{
    #region EAInformationClass

    class EAInformation : Attr
    {
        internal struct ATTR_EA_INFORMATION
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal ushort PackedSize;
            internal ushort EACount;
            internal uint UnpackedSize;

            internal ATTR_EA_INFORMATION(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                PackedSize = BitConverter.ToUInt16(bytes, 24);
                EACount = BitConverter.ToUInt16(bytes, 26);
                UnpackedSize = BitConverter.ToUInt32(bytes, 28);
            }
        }

        #region Properties

        public readonly ushort PackedSize;
        public readonly ushort EACount;
        public readonly uint UnpackedSize;

        #endregion Properties

        #region Constructors

        internal EAInformation(byte[] bytes, string attrName)
        {
            ATTR_EA_INFORMATION ea = new ATTR_EA_INFORMATION(bytes);

            Name = Enum.GetName(typeof(ATTR_TYPE), ea.header.commonHeader.ATTRType);
            NameString = attrName;
            NonResident = ea.header.commonHeader.NonResident;
            AttributeId = ea.header.commonHeader.Id;
            PackedSize = ea.PackedSize;
            EACount = ea.EACount;
            UnpackedSize = ea.UnpackedSize;
        }

        #endregion Constructors
    }

    #endregion EAInformationClass
}
