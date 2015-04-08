using System;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{
    public class VolumeName : Attr
    {
        struct ATTR_VOLNAME
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal string VolumeNameString;

            internal ATTR_VOLNAME(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                VolumeNameString = Encoding.Unicode.GetString(bytes.Skip(24).ToArray());
            }

        }

        #region Properties

        public readonly string VolumeNameString;

        #endregion Properties

        #region Constructors

        internal VolumeName(byte[] AttrBytes, string AttrName)
        {
            ATTR_VOLNAME volName = new ATTR_VOLNAME(AttrBytes);

            Name = Enum.GetName(typeof(ATTR_TYPE), volName.header.commonHeader.ATTRType);
            NameString = AttrName;
            NonResident = volName.header.commonHeader.NonResident;
            AttributeId = volName.header.commonHeader.Id;
            VolumeNameString = volName.VolumeNameString;
        }

        #endregion Constructors
    }
}
