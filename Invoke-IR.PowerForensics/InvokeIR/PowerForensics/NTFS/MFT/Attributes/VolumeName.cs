using System;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS.MFT.Attributes
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

        public string VolumeNameString;

        internal VolumeName(uint ATTRType, string name, bool nonResident, string volumeName)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            VolumeNameString = volumeName;
        }

        internal static VolumeName Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_VOLNAME volName = new ATTR_VOLNAME(AttrBytes);
            return new VolumeName(
                volName.header.commonHeader.ATTRType,
                AttrName,
                volName.header.commonHeader.NonResident,
                volName.VolumeNameString);

        }

    }
}
