using System;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class VolumeInformation : FileRecordAttribute
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum ATTR_VOLINFO
        {
            /// <summary>
            /// Dirty
            /// </summary>
            FLAG_DIRTY = 0x0001,

            /// <summary>
            /// Resize logfile
            /// </summary>
            FLAG_RLF = 0x0002,

            /// <summary>
            /// Upgrade on mount
            /// </summary>
            FLAG_UOM = 0x0004,

            /// <summary>
            /// Mounted on NT4
            /// </summary>
            FLAG_MONT = 0x0008,

            /// <summary>
            /// Delete USN underway
            /// </summary>
            FLAG_DUSN = 0x0010,

            /// <summary>
            /// Repair object Ids
            /// </summary>
            FLAG_ROI = 0x0020,

            /// <summary>
            /// Modified by chkdsk
            /// </summary>
            FLAG_MBC = 0x8000 
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly Version Version;

        /// <summary>
        /// 
        /// </summary>
        public readonly ATTR_VOLINFO Flags;

        #endregion Properties

        #region Constructors

        internal VolumeInformation(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            Version = new Version(bytes[0x08 + offset], bytes[0x09 + offset]);
            Flags = (ATTR_VOLINFO)BitConverter.ToInt16(bytes, 0x0A + offset);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static VolumeInformation Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return Get(FileRecord.Get(volume, MftIndex.VOLUME_INDEX, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static VolumeInformation GetByPath(string path)
        {
            return Get(FileRecord.Get(path, true));
        }

        private static VolumeInformation Get(FileRecord record)
        {
            foreach (FileRecordAttribute attr in record.Attribute)
            {
                VolumeInformation volInfo = attr as VolumeInformation;
                if(volInfo != null)
                {
                    return volInfo;
                }
            }
            throw new Exception("No VOLUME_INFORMATION attribute found.");
        }

        #endregion Static Methods
    }
}