using System;
using System.Linq;
using System.Text;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.NTFS.MFT.Attributes
{

    public class StandardInformation : Attr
    {

        enum ATTR_STDINFO_PERMISSION
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
            ENCRYPTED = 0x00004000
        }

        struct ATTR_STANDARD_INFORMATION
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal DateTime CreateTime;	// File creation time
            internal DateTime AlterTime;	// File altered time
            internal DateTime MFTTime;	    // MFT changed time
            internal DateTime ReadTime;	    // File read time
            internal uint Permission;	    // Dos file permission
            internal uint MaxVersionNo;	    // Maximum number of file versions
            internal uint VersionNo;	    // File version number
            internal uint ClassId;		    // Class Id
            internal uint OwnerId;		    // Owner Id
            internal uint SecurityId;	    // Security Id
            internal ulong QuotaCharged;	// Quota charged
            internal ulong USN;			    // USN Journel

            internal ATTR_STANDARD_INFORMATION(byte[] bytes, int length)
            {
                if (length == 120)
                {
                    header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                    CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 24));
                    AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 32));
                    MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 40));
                    ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 48));
                    Permission = BitConverter.ToUInt32(bytes, 56);
                    MaxVersionNo = BitConverter.ToUInt32(bytes, 60);
                    VersionNo = BitConverter.ToUInt32(bytes, 64);
                    ClassId = BitConverter.ToUInt32(bytes, 68);
                    OwnerId = BitConverter.ToUInt32(bytes, 72);
                    SecurityId = BitConverter.ToUInt32(bytes, 76);
                    QuotaCharged = BitConverter.ToUInt64(bytes, 80);
                    USN = BitConverter.ToUInt64(bytes, 88);
                }
                else
                {
                    header = new AttrHeader.ATTR_HEADER_RESIDENT(bytes.Take(24).ToArray());
                    CreateTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 24));
                    AlterTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 32));
                    MFTTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 40));
                    ReadTime = DateTime.FromFileTime(BitConverter.ToInt64(bytes, 48));
                    Permission = BitConverter.ToUInt32(bytes, 56);
                    MaxVersionNo = 0;
                    VersionNo = 0;
                    ClassId = 0;
                    OwnerId = 0;
                    SecurityId = 0;
                    QuotaCharged = 0;
                    USN = 0;
                }
            }
        }

        public string Flags;
        public uint Permission;
        public uint OwnerId;
        public uint SecurityId;
        public DateTime CreateTime;
        public DateTime FileModifiedTime;
        public DateTime MFTModifiedTime;
        public DateTime AccessTime;

        internal StandardInformation(uint ATTRType, string name, bool nonResident, ushort attributeId, string flags, uint permission, uint ownerId, uint securityId, DateTime createTime, DateTime alterTime, DateTime mftTime, DateTime readTime)
        {
            Name = Enum.GetName(typeof(ATTR_TYPE), ATTRType);
            NameString = name;
            NonResident = nonResident;
            AttributeId = attributeId;
            Flags = flags;
            Permission = permission;
            OwnerId = ownerId;
            SecurityId = securityId;
            CreateTime = createTime;
            FileModifiedTime = alterTime;
            MFTModifiedTime = mftTime;
            AccessTime = readTime;
        }

        internal static StandardInformation Get(byte[] AttrBytes, string AttrName)
        {

            ATTR_STANDARD_INFORMATION stdInfo = new ATTR_STANDARD_INFORMATION(AttrBytes, AttrBytes.Length);

            #region stdInfoFlags

            StringBuilder permissionFlags = new StringBuilder();
            if (stdInfo.Permission != 0)
            {
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.READONLY) == (uint)ATTR_STDINFO_PERMISSION.READONLY)
                {
                    permissionFlags.Append("READONLY, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.HIDDEN) == (uint)ATTR_STDINFO_PERMISSION.HIDDEN)
                {
                    permissionFlags.Append("HIDDEN, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.SYSTEM) == (uint)ATTR_STDINFO_PERMISSION.SYSTEM)
                {
                    permissionFlags.Append("SYSTEM, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.ARCHIVE) == (uint)ATTR_STDINFO_PERMISSION.ARCHIVE)
                {
                    permissionFlags.Append("ARCHIVE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.DEVICE) == (uint)ATTR_STDINFO_PERMISSION.DEVICE)
                {
                    permissionFlags.Append("DEVICE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.NORMAL) == (uint)ATTR_STDINFO_PERMISSION.NORMAL)
                {
                    permissionFlags.Append("NORMAL, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.TEMP) == (uint)ATTR_STDINFO_PERMISSION.TEMP)
                {
                    permissionFlags.Append("TEMP, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.SPARSE) == (uint)ATTR_STDINFO_PERMISSION.SPARSE)
                {
                    permissionFlags.Append("SPARSE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.REPARSE) == (uint)ATTR_STDINFO_PERMISSION.REPARSE)
                {
                    permissionFlags.Append("REPARSE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.COMPRESSED) == (uint)ATTR_STDINFO_PERMISSION.COMPRESSED)
                {
                    permissionFlags.Append("COMPRESSED, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.OFFLINE) == (uint)ATTR_STDINFO_PERMISSION.OFFLINE)
                {
                    permissionFlags.Append("OFFLINE, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.NCI) == (uint)ATTR_STDINFO_PERMISSION.NCI)
                {
                    permissionFlags.Append("NCI, ");
                }
                if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.ENCRYPTED) == (uint)ATTR_STDINFO_PERMISSION.ENCRYPTED)
                {
                    permissionFlags.Append("ENCRYPTED, ");
                }
                if (permissionFlags.Length > 2)
                {
                    permissionFlags.Length -= 2;

                }
            }
            #endregion stdInfoFlags

            return new StandardInformation(
                stdInfo.header.commonHeader.ATTRType,
                AttrName,
                stdInfo.header.commonHeader.NonResident,
                stdInfo.header.commonHeader.Id,
                permissionFlags.ToString(),
                stdInfo.Permission,
                stdInfo.OwnerId,
                stdInfo.SecurityId,
                stdInfo.CreateTime,
                stdInfo.AlterTime,
                stdInfo.MFTTime,
                stdInfo.ReadTime);

        }

    }

}
