using System;

namespace PowerForensics.Ntfs
{
    #region VolumeInformationClass

    public class VolumeInformation : Attr
    {
        #region Enums

        [Flags]
        public enum ATTR_VOLINFO
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

        #region Properties

        public readonly Version Version;
        public readonly ATTR_VOLINFO Flags;

        #endregion Properties

        #region Constructors

        internal VolumeInformation(ResidentHeader header, byte[] bytes, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            Version = new Version(bytes[0x08], bytes[0x09]);
            Flags = (ATTR_VOLINFO)BitConverter.ToInt16(bytes, 0x0A);
        }

        internal VolumeInformation(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            Version = new Version(bytes[0x08], bytes[0x09 + offset]);
            Flags = (ATTR_VOLINFO)BitConverter.ToInt16(bytes, 0x0A + offset);
        }

        #endregion Constructors

        #region StaticMethods

        public static VolumeInformation Get(string volume)
        {
            FileRecord record = FileRecord.Get(volume, MftIndex.VOLUME_INDEX, true);
            return Get(record);
        }

        public static VolumeInformation GetByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return Get(record);
        }

        private static VolumeInformation Get(FileRecord fileRecord)
        {
            foreach (Attr attr in fileRecord.Attribute)
            {
                if (attr.Name == Attr.ATTR_TYPE.VOLUME_INFORMATION)
                {
                    return attr as VolumeInformation;
                }
            }
            throw new Exception("No VOLUME_INFORMATION attribute found.");
        }

        #endregion StaticMethods
    }

    #endregion VolumeInformationClass
}
