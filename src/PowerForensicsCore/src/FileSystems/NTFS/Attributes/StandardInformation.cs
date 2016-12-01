using System;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class StandardInformation : FileRecordAttribute
    {
        #region Enums
        
        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum ATTR_STDINFO_PERMISSION : uint
        {
            /// <summary>
            /// 
            /// </summary>
            READONLY = 0x00000001,

            /// <summary>
            /// 
            /// </summary>
            HIDDEN = 0x00000002,

            /// <summary>
            /// 
            /// </summary>
            SYSTEM = 0x00000004,

            /// <summary>
            /// 
            /// </summary>
            ARCHIVE = 0x00000020,

            /// <summary>
            /// 
            /// </summary>
            DEVICE = 0x00000040,

            /// <summary>
            /// 
            /// </summary>
            NORMAL = 0x00000080,

            /// <summary>
            /// 
            /// </summary>
            TEMP = 0x00000100,

            /// <summary>
            /// 
            /// </summary>
            SPARSE = 0x00000200,

            /// <summary>
            /// 
            /// </summary>
            REPARSE = 0x00000400,

            /// <summary>
            /// 
            /// </summary>
            COMPRESSED = 0x00000800,

            /// <summary>
            /// 
            /// </summary>
            OFFLINE = 0x00001000,

            /// <summary>
            /// 
            /// </summary>
            NCI = 0x00002000,

            /// <summary>
            /// 
            /// </summary>
            ENCRYPTED = 0x00004000
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BornTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifiedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ChangedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly ATTR_STDINFO_PERMISSION Permission;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint MaxVersionNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint VersionNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ClassId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint OwnerId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SecurityId;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong QuotaCharged;

        /// <summary>
        /// 
        /// </summary>
        public readonly long UpdateSequenceNumber;

        #endregion Properties

        #region Constructors

        internal StandardInformation(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            BornTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x00 + offset));
            ModifiedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x08 + offset));
            ChangedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x10 + offset));
            AccessedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x18 + offset));
            Permission = ((ATTR_STDINFO_PERMISSION)BitConverter.ToUInt32(bytes, 0x20 + offset));
            MaxVersionNumber = BitConverter.ToUInt32(bytes, 0x24 + offset);
            VersionNumber = BitConverter.ToUInt32(bytes, 0x28 + offset);
            ClassId = BitConverter.ToUInt32(bytes, 0x2C + offset);

            if (header.AttrSize == 0x48)
            {
                OwnerId = BitConverter.ToUInt32(bytes, 0x30 + offset);
                SecurityId = BitConverter.ToUInt32(bytes, 0x34 + offset);
                QuotaCharged = BitConverter.ToUInt64(bytes, 0x38 + offset);
                UpdateSequenceNumber = BitConverter.ToInt64(bytes, 0x40 + offset);
            }
        }

        #endregion Constructors
    }
}