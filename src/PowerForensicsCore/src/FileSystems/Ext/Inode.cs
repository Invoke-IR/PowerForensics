using System;

namespace PowerForensics.Ext
{
    public class Inode
    {
        #region Enums

        [Flags]
        public enum FILE_MODE
        {
            S_IXOTH = 0x0001,
            S_IWOTH = 0x0002,
            S_IROTH = 0x0004,
            S_IXGRP = 0x0008,
            S_IWGRP = 0x0010,
            S_IRGRP = 0x0020,
            S_IXUSR = 0x0040,
            S_IWUSR = 0x0080,
            S_IRUSR = 0x0100,
            S_ISVTX = 0x0200,
            S_ISGID = 0x0400,
            S_ISUID = 0x0800,
            S_IFIFO = 0x1000,
            S_IFCHR = 0x2000,
            S_IFDIR = 0x4000,
            S_IFBLK = 0x6000,
            S_IFREG = 0x8000,
            S_IFLNK = 0xA000,
            S_IFSOCK = 0xC000
        }

        [Flags]
        public enum FLAGS : uint
        {
            EXT4_SECRM_FL = 0x00000001,
            EXT4_UNRM_FL = 0x00000002,
            EXT4_COMPR_FL = 0x00000004,
            EXT4_SYNC_FL = 0x00000008,
            EXT4_IMMUTABLE_FL = 0x00000010,
            EXT4_APPEND_FL = 0x00000020,
            EXT4_NODUMP_FL = 0x00000040,
            EXT4_NOATIME_FL = 0x00000080,
            EXT4_DIRTY_FL = 0x00000100,
            EXT4_COMPRBLK_FL = 0x00000200,
            EXT4_NOCOMPR_FL = 0x00000400,
            EXT4_ENCRYPT_FL = 0x00000800,
            EXT4_INDEX_FL = 0x00001000,
            EXT4_IMAGIC_FL = 0x00002000,
            EXT4_JOURNAL_DATA_FL = 0x00004000,
            EXT4_NOTAIL_FL = 0x00008000,
            EXT4_DIRSYNC_FL = 0x00010000,
            EXT4_TOPDIR_FL = 0x00020000,
            EXT4_HUGE_FILE_FL = 0x00040000,
            EXT4_EXTENTS_FL = 0x00080000,
            EXT4_EA_INODE_FL = 0x00200000,
            EXT4_EOFBLOCKS_FL = 0x00400000,
            EXT4_SNAPFILE_FL = 0x01000000,
            EXT4_SNAPFILE_DELETED_FL = 0x04000000,
            EXT4_SNAPFILE_SHRUNK_FL = 0x08000000,
            EXT4_INLINE_DATA_FL = 0x10000000,
            EXT4_PROJINHERIT_FL = 0x20000000,
            EXT4_RESERVED_FL = 0x80000000
        }

        #endregion Enums

        #region Properties

        public readonly FILE_MODE FileMode; //File mode
        public readonly ushort UserId; //Lower 16-bits of Owner UID.
        public readonly uint i_size_lo; //Lower 32-bits of size in bytes.
        public readonly DateTime AccessTime; //Last access time, in seconds since the epoch.
        public readonly DateTime ChangeTime; //Last inode change time, in seconds since the epoch.
        public readonly DateTime ModifiedTime; //Last data modification time, in seconds since the epoch.
        public readonly DateTime DeletionTime; //Deletion Time, in seconds since the epoch.
        public readonly ushort GroupId; //Lower 16-bits of GID.
        public readonly ushort i_links_count; //Hard link count.
        public readonly uint i_blocks_lo; //Lower 32-bits of "block" count.If the huge_file feature flag is not set on the filesystem, the file consumes i_blocks_lo 512-byte blocks on disk. If huge_file is set and EXT4_HUGE_FILE_FL is NOT set in inode.i_flags, then the file consumes i_blocks_lo + (i_blocks_hi << 32) 512-byte blocks on disk.If huge_file is set and EXT4_HUGE_FILE_FL IS set in inode.i_flags, then this file consumes (i_blocks_lo + i_blocks_hi << 32) filesystem blocks on disk.
        public readonly FLAGS Flags; //Inode flags
        public readonly uint osd1;
        public readonly byte[] i_block; //Block map or extent tree.See the section "The Contents of inode.i_block".
        public readonly uint FileVersion; //File version(for NFS).
        public readonly uint i_file_acl_lo; //Lower 32-bits of extended attribute block.ACLs are of course one of many possible extended attributes; I think the name of this field is a result of the first use of extended attributes being for ACLs.
        public readonly uint i_size_high; //Upper 32-bits of file size. In ext2/3 this field was named i_dir_acl, though it was usually set to zero and never used.
        public readonly uint i_obso_faddr; //(Obsolete) fragment address.
        public readonly byte[] osd2;
        /*linux2
        0x0	__le16 l_i_blocks_high Upper 16-bits of the block count. Please see the note attached to i_blocks_lo.
        0x2	__le16 l_i_file_acl_high Upper 16-bits of the extended attribute block (historically, the file ACL location). See the Extended Attributes section below.
        0x4	__le16 l_i_uid_high Upper 16-bits of the Owner UID.
        0x6	__le16 l_i_gid_high Upper 16-bits of the GID.
        0x8	__le16 l_i_checksum_lo Lower 16-bits of the inode checksum.
        0xA	__le16 l_i_reserved Unused.
        hurd2
        0x0	__le16 h_i_reserved1	??
        0x2	__u16 h_i_mode_high Upper 16-bits of the file mode.
        0x4	__le16 h_i_uid_high Upper 16-bits of the Owner UID.
        0x6	__le16 h_i_gid_high Upper 16-bits of the GID.
        0x8	__u32 h_i_author Author code?
        masix2
        0x0	__le16 h_i_reserved1	??
        0x2	__u16 m_i_file_acl_high Upper 16-bits of the extended attribute block (historically, the file ACL location).
        0x4	__u32 m_i_reserved2 [2]    ??*/
        public readonly ushort i_extra_isize; //Size of this inode - 128. Alternately, the size of the extended inode fields beyond the original ext2 inode, including this field.
        public readonly ushort i_checksum_hi; //Upper 16-bits of the inode checksum.
        public readonly uint ChangeTimeExtra; //Extra change time bits. This provides sub-second precision. See Inode Timestamps section.
        public readonly uint ModifiedTimeExtra; //Extra modification time bits. This provides sub-second precision.
        public readonly uint AccessTimeExtra; //Extra access time bits. This provides sub-second precision.
        public readonly DateTime CreationTime; //File creation time, in seconds since the epoch.
        public readonly uint CreationTimeExtra; //Extra file creation time bits. This provides sub-second precision.
        public readonly uint i_version_hi; //Upper 32-bits for version number.
        public readonly uint ProjectId; //Project ID.

        #endregion Properties

        #region Constructors

        private Inode(byte[] bytes)
        {
            FileMode = (FILE_MODE)BitConverter.ToUInt16(bytes, 0x00);
            UserId = BitConverter.ToUInt16(bytes, 0x02);
            i_size_lo = BitConverter.ToUInt32(bytes, 0x04);
            AccessTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x08));
            ChangeTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x0C));
            ModifiedTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x10));
            DeletionTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x14));
            GroupId = BitConverter.ToUInt16(bytes, 0x18);
            i_links_count = BitConverter.ToUInt16(bytes, 0x1A);
            i_blocks_lo = BitConverter.ToUInt16(bytes, 0x1C);
            Flags = (FLAGS)BitConverter.ToUInt32(bytes, 0x20);
            osd1 = BitConverter.ToUInt32(bytes, 0x24);
            i_block = Helper.GetSubArray(bytes, 0x28, 0x3C);
            FileVersion = BitConverter.ToUInt32(bytes, 0x64);
            i_file_acl_lo = BitConverter.ToUInt32(bytes, 0x68);
            i_size_high = BitConverter.ToUInt32(bytes, 0x6C);
            i_obso_faddr = BitConverter.ToUInt32(bytes, 0x70);
            osd2 = Helper.GetSubArray(bytes, 0x74, 0x0C);
            i_extra_isize = BitConverter.ToUInt16(bytes, 0x80);
            i_checksum_hi = BitConverter.ToUInt16(bytes, 0x82);
            ChangeTimeExtra = BitConverter.ToUInt32(bytes, 0x84);
            ModifiedTimeExtra = BitConverter.ToUInt32(bytes, 0x88);
            AccessTimeExtra = BitConverter.ToUInt32(bytes, 0x8C);
            CreationTime = Helper.FromUnixTime(BitConverter.ToUInt32(bytes, 0x90));
            CreationTimeExtra = BitConverter.ToUInt32(bytes, 0x94);
            i_version_hi = BitConverter.ToUInt32(bytes, 0x98);
            ProjectId = BitConverter.ToUInt32(bytes, 0x9C);
        }

        #endregion Constructors

        #region StaticMethods

        public static Inode Get(byte[] bytes)
        {
            return new Inode(bytes);
        }

        #endregion StaticMethods
    }
}
