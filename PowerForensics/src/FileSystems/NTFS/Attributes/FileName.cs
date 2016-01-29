using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region FileNameClass

    public class FileName : FileRecordAttribute
    {
        #region Constants

        private const byte ATTR_FILENAME_NAMESPACE_POSIX = 0x00;
        private const byte ATTR_FILENAME_NAMESPACE_WIN32 = 0x01;
        private const byte ATTR_FILENAME_NAMESPACE_DOS = 0x02;

        #endregion Constants

        #region Enums

        enum ATTR_FILENAME_FLAG
        {
            READONLY = 0x00000001,
            HIDDEN = 0x00000002,
            SYSTEM = 0x00000004,
            ARCHIVE = 0x00000020,
            DEVICE = 0x00000040,
            NORMAL = 0x00000080,
            TEMP = 0x00000100,
            SPARSE = 0x00000200,
            REPARSE = 0x00000400,
            COMPRESSED = 0x00000800,
            OFFLINE = 0x00001000,
            NCI = 0x00002000,
            ENCRYPTED = 0x00004000,
            DIRECTORY = 0x10000000,
            INDEXVIEW = 0x20000000
        }

        #endregion Enums
           
        #region Properties

        public readonly string Filename;
        public readonly ushort ParentSequenceNumber;
        public readonly ulong ParentRecordNumber;
        public readonly int Namespace;
        public readonly ulong AllocatedSize;
        public readonly ulong RealSize;
        public readonly uint Flags;
        public readonly uint ER;
        private readonly byte NameLength;
        public readonly DateTime ModifiedTime;
        public readonly DateTime AccessedTime;
        public readonly DateTime ChangedTime;
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

        internal FileName(byte[] bytes, int offset)
        {
            try
            {
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
                Filename = Encoding.Unicode.GetString(bytes, 0x42 + offset, NameLength * 2).TrimEnd('\0');
            }
            catch
            {

            }
        }

        internal FileName(ResidentHeader header, byte[] bytes, string attrName)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            
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

            // Get FileName
            Filename = Encoding.Unicode.GetString(bytes, 0x42, NameLength * 2).TrimEnd('\0');
        }

        internal FileName(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            // Headers
            Name = (ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;

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

    #endregion FileNameClass
}
