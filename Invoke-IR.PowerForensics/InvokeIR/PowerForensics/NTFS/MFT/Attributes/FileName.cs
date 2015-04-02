using System;
using System.Linq;
using System.Text;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.NTFS.MFT.Attributes
{

    public class FileName : Attr
    {
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

        private const byte ATTR_FILENAME_NAMESPACE_POSIX = 0x00;
        private const byte ATTR_FILENAME_NAMESPACE_WIN32 = 0x01;
        private const byte ATTR_FILENAME_NAMESPACE_DOS = 0x02;

        internal struct ATTR_FILE_NAME
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal ulong ParentRef;		    // File reference to the parent directory
            internal DateTime CreateTime;		// File creation time
            internal DateTime AlterTime;		// File altered time
            internal DateTime MFTTime;		    // MFT changed time
            internal DateTime ReadTime;		    // File read time
            internal ulong AllocSize;		    // Allocated size of the file
            internal ulong RealSize;		    // Real size of the file
            internal uint Flags;			    // Flags
            internal uint ER;				    // Used by EAs and Reparse
            internal byte NameLength;		    // Filename length in characters
            internal byte NameSpace;		    // Filename space
            internal byte[] Name;		        // Filename

            internal ATTR_FILE_NAME(byte[] bytes)
            {

                if (bytes.Length < 90)
                {
                    header = new AttrHeader.ATTR_HEADER_RESIDENT(new byte[24]);
                    ParentRef = BitConverter.ToUInt64(bytes, 0);
                    CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 8));
                    AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 16));
                    MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 24));
                    ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 32));
                    AllocSize = BitConverter.ToUInt64(bytes, 40);
                    RealSize = BitConverter.ToUInt64(bytes, 48);
                    Flags = BitConverter.ToUInt32(bytes, 56);
                    ER = BitConverter.ToUInt32(bytes, 60);
                    NameLength = bytes[64];
                    NameSpace = bytes[65];
                    Name = bytes.Skip(66).ToArray();
                }

                else
                {
                    header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                    ParentRef = BitConverter.ToUInt64(bytes, 24);
                    CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 32));
                    AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 40));
                    MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 48));
                    ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 56));
                    AllocSize = BitConverter.ToUInt64(bytes, 64);
                    RealSize = BitConverter.ToUInt64(bytes, 72);
                    Flags = BitConverter.ToUInt32(bytes, 80);
                    ER = BitConverter.ToUInt32(bytes, 84);
                    NameLength = bytes[88];
                    NameSpace = bytes[89];
                    Name = bytes.Skip(90).ToArray();
                }
            }
        }

        public string Filename;
        public ulong ParentIndex;
        public DateTime CreateTime;
        public DateTime FileModifiedTime;
        public DateTime MFTModifiedTime;
        public DateTime AccessTime;

        internal FileName(uint AttrType, string attrName, bool nonResident, ushort attributeId, byte[] name, ulong parentIndex, DateTime createTime, DateTime alterTime, DateTime mftTime, DateTime readTime)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), AttrType);
            NameString = attrName;
            NonResident = nonResident;
            AttributeId = attributeId;
            Filename = Encoding.Unicode.GetString(name).TrimEnd('\0');
            ParentIndex = parentIndex;
            CreateTime = createTime;
            FileModifiedTime = alterTime;
            MFTModifiedTime = mftTime;
            AccessTime = readTime;
        }

        internal static FileName Get(byte[] AttrBytes, string attrName)
        {


            ATTR_FILE_NAME fileName = new ATTR_FILE_NAME(AttrBytes);

            return new FileName(
                fileName.header.commonHeader.ATTRType,
                attrName,
                fileName.header.commonHeader.NonResident,
                fileName.header.commonHeader.Id,
                fileName.Name,
                (fileName.ParentRef & 0x000000000000FFFF),
                fileName.CreateTime,
                fileName.AlterTime,
                fileName.MFTTime,
                fileName.ReadTime);

        }

    }

}
