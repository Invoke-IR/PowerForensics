using System;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{

    public class VolumeInformation : Attr
    {

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

        struct ATTR_VOLUME_INFORMATION
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal byte[] Reserved1;	// Always 0 ?
            internal byte MajorVersion;	// Major version
            internal byte MinorVersion;	// Minor version
            internal byte[] Flags;		// Flags

            internal ATTR_VOLUME_INFORMATION(byte[] bytes)
            {
                header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                Reserved1 = bytes.Skip(24).Take(8).ToArray();
                MajorVersion = bytes[32];
                MinorVersion = bytes[33];
                Flags = bytes.Skip(34).Take(2).ToArray();
            }
        }

        #region Properties

        public readonly uint Major;
        public readonly uint Minor;
        public readonly string Flags;

        #endregion Properties

        #region Constructors

        internal VolumeInformation(byte[] AttrBytes, string AttrName)
        {
            ATTR_VOLUME_INFORMATION volInfo = new ATTR_VOLUME_INFORMATION(AttrBytes);

            Int16 flags = BitConverter.ToInt16(volInfo.Flags, 0);

            #region volInfoFlags

            StringBuilder volumeFlags = new StringBuilder();
            if (flags != 0)
            {
                if ((flags & (int)ATTR_VOLINFO.FLAG_DIRTY) == (int)ATTR_VOLINFO.FLAG_DIRTY)
                {
                    volumeFlags.Append("Dirty, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_RLF) == (int)ATTR_VOLINFO.FLAG_RLF)
                {
                    volumeFlags.Append("Resize Logfile, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_UOM) == (int)ATTR_VOLINFO.FLAG_UOM)
                {
                    volumeFlags.Append("Upgrade on Mount, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_MONT) == (int)ATTR_VOLINFO.FLAG_MONT)
                {
                    volumeFlags.Append("Mounted on NT4, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_DUSN) == (int)ATTR_VOLINFO.FLAG_DUSN)
                {
                    volumeFlags.Append("Delete USN Underway, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_ROI) == (int)ATTR_VOLINFO.FLAG_ROI)
                {
                    volumeFlags.Append("Repair Object Ids, ");
                }
                if ((flags & (int)ATTR_VOLINFO.FLAG_MBC) == (int)ATTR_VOLINFO.FLAG_MBC)
                {
                    volumeFlags.Append("Modified By ChkDisk, ");
                }
                volumeFlags.Length -= 2;
            }

            #endregion volInfoFlags

            Name = Enum.GetName(typeof(ATTR_TYPE), volInfo.header.commonHeader.ATTRType);
            NameString = AttrName;
            NonResident = volInfo.header.commonHeader.NonResident;
            AttributeId = volInfo.header.commonHeader.Id;
            Major = volInfo.MajorVersion;
            Minor = volInfo.MinorVersion;
            Flags = volumeFlags.ToString();
        }

        #endregion Constructors

    }

}
