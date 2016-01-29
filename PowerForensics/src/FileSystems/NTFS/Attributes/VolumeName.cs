using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region VolumeNameClass

    public class VolumeName : FileRecordAttribute
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
            Helper.getVolumeName(ref volume);
            return Get(FileRecord.Get(volume, MftIndex.VOLUME_INDEX, true));
        }

        public static VolumeName GetByPath(string path)
        {
            return Get(FileRecord.Get(path, true));
        }

        private static VolumeName Get(FileRecord fileRecord)
        {
            foreach (FileRecordAttribute attr in fileRecord.Attribute)
            {
                VolumeName volName = attr as VolumeName;
                if(volName != null)
                {
                    return volName;
                }
            }
            throw new Exception("No VOLUME_NAME attribute found.");
        }

        #endregion StaticMethods
    }
    
    #endregion VolumeNameClass
}
