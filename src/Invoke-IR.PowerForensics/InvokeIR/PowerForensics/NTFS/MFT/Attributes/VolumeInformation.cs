using System;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{
    #region VolumeInformationClass

    public class VolumeInformation : Attr
    {

        #region Enums

        [FlagsAttribute]
        enum ATTR_VOLINFO
        {
            FLAG_DIRTY = 0x0001,	// Dirty
            FLAG_RLF = 0x0002,	    // Resize logfile
            FLAG_UOM = 0x0004,	    // Upgrade on mount
            FLAG_MONT = 0x0008,	    // Mounted on NT4
            FLAG_DUSN = 0x0010,	    // Delete USN underway
            FLAG_ROI = 0x0020,	    // Repair object Ids
            FLAG_MBC = 0x8000	    // Modified by chkdsk
        }

        #endregion Enums

        #region Structs

        struct ATTR_VOLUME_INFORMATION
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal byte[] Reserved1;	// Always 0 ?
            internal byte MajorVersion;	// Major version
            internal byte MinorVersion;	// Minor version
            internal short Flags;		// Flags

            internal ATTR_VOLUME_INFORMATION(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                Reserved1 = bytes.Skip(24).Take(8).ToArray();
                MajorVersion = bytes[32];
                MinorVersion = bytes[33];
                Flags = BitConverter.ToInt16(bytes, 34);
            }
        }

        #endregion Structs

        #region Properties

        public readonly uint Major;
        public readonly uint Minor;
        public readonly string Flags;

        #endregion Properties

        #region Constructors

        internal VolumeInformation(byte[] AttrBytes, string AttrName)
        {
            ATTR_VOLUME_INFORMATION volInfo = new ATTR_VOLUME_INFORMATION(AttrBytes);

            Name = Enum.GetName(typeof(ATTR_TYPE), volInfo.header.commonHeader.ATTRType);
            NameString = AttrName;
            NonResident = volInfo.header.commonHeader.NonResident;
            AttributeId = volInfo.header.commonHeader.Id;
            Major = volInfo.MajorVersion;
            Minor = volInfo.MinorVersion;
            Flags = ((ATTR_VOLINFO)volInfo.Flags).ToString();
        }

        #endregion Constructors

    }

    #endregion VolumeInformationClass
}
