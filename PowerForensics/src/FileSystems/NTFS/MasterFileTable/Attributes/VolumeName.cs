using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region VolumeNameClass

    public class VolumeName : Attr
    {
        #region Properties

        public readonly string VolumeNameString;

        #endregion Properties

        #region Constructors

        internal VolumeName(ResidentHeader header, byte[] bytes, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            VolumeNameString = Encoding.Unicode.GetString(bytes, 0x00, (int)header.AttrSize);
        }

        internal VolumeName(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

            VolumeNameString = Encoding.Unicode.GetString(bytes, 0x00 + offset, (int)header.AttrSize);
        }

        #endregion Constructors

        #region StaticMethods

        public static VolumeName Get(string volume)
        {
            FileRecord record = FileRecord.Get(volume, MftIndex.VOLUME_INDEX, true);
            return Get(record);
        }

        public static VolumeName GetByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return Get(record);
        }

        private static VolumeName Get(FileRecord fileRecord)
        {
            foreach (Attr attr in fileRecord.Attribute)
            {
                if (attr.Name == Attr.ATTR_TYPE.VOLUME_NAME)
                {
                    return attr as VolumeName;
                }
            }
            throw new Exception("No VOLUME_NAME attribute found.");
        }

        #endregion StaticMethods
    }
    
    #endregion VolumeNameClass
}
