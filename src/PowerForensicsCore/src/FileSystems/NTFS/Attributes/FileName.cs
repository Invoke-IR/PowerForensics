using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class FileName : FileRecordAttribute
    {
        #region Constants

        private const byte ATTR_FILENAME_NAMESPACE_POSIX = 0x00;
        private const byte ATTR_FILENAME_NAMESPACE_WIN32 = 0x01;
        private const byte ATTR_FILENAME_NAMESPACE_DOS = 0x02;

        #endregion Constants

        #region Enums

        /// <summary>
        /// 
        /// </summary>
        enum ATTR_FILENAME_FLAG
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
            ENCRYPTED = 0x00004000,

            /// <summary>
            /// 
            /// </summary>
            DIRECTORY = 0x10000000,

            /// <summary>
            /// 
            /// </summary>
            INDEXVIEW = 0x20000000
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Filename;

        /// <summary>
        /// 
        /// </summary>
        public readonly ushort ParentSequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong ParentRecordNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly int Namespace;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong AllocatedSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong RealSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ER;

        private readonly byte NameLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModifiedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccessedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ChangedTime;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime BornTime;

        #endregion Properties

        #region Constructors

        internal FileName(byte[] bytes)
        {
            try
            {
                // FILE_NAME Attribute
                ParentRecordNumber = (BitConverter.ToUInt64(bytes, 0x00) & 0x0000FFFFFFFFFFFF);
                ParentSequenceNumber = (BitConverter.ToUInt16(bytes, 0x06));
                BornTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x08));
                ChangedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x10));
                ModifiedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x18));
                AccessedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x20));
                AllocatedSize = BitConverter.ToUInt64(bytes, 0x28);
                RealSize = BitConverter.ToUInt64(bytes, 0x30);
                Flags = BitConverter.ToUInt32(bytes, 0x38);
                ER = BitConverter.ToUInt32(bytes, 0x3C);
                NameLength = bytes[0x40];
                Namespace = Convert.ToInt32(bytes[0x41]);
                Filename = Encoding.Unicode.GetString(bytes, 0x42, NameLength * 2).TrimEnd('\0');
            }
            catch
            {

            }
        }

        internal FileName(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            // FILE_NAME Attribute
            ParentRecordNumber = (BitConverter.ToUInt64(bytes, 0x00 + offset) & 0x0000FFFFFFFFFFFF);
            ParentSequenceNumber = (BitConverter.ToUInt16(bytes, 0x06 + offset));
            BornTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x08 + offset));
            ChangedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x10 + offset));
            ModifiedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x18 + offset));
            AccessedTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x20 + offset));
            AllocatedSize = BitConverter.ToUInt64(bytes, 0x28 + offset);
            RealSize = BitConverter.ToUInt64(bytes, 0x30 + offset);
            Flags = BitConverter.ToUInt32(bytes, 0x38 + offset);
            ER = BitConverter.ToUInt32(bytes, 0x3C + offset);
            NameLength = bytes[0x40 + offset];
            Namespace = Convert.ToInt32(bytes[0x41 + offset]);

            // Get FileName
            Filename = Encoding.Unicode.GetString(bytes, 0x42 + offset, NameLength * 2).TrimEnd('\0');
        }

        #endregion Constructors
    }
}