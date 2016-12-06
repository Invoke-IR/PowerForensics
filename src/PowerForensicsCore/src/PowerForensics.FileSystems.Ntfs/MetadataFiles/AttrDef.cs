using System;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class AttrDef
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum ATTR_DEF_ENTRY
        {
            /// <summary>
            /// 
            /// </summary>
            INDEX = 0x02,

            /// <summary>
            /// 
            /// </summary>
            ALWAYS_RESIDENT = 0x40,

            /// <summary>
            /// 
            /// </summary>
            ALWAYS_NONRESIDENT = 0x80
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Type;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint DisplayRule;

        /// <summary>
        /// 
        /// </summary>
        public readonly string CollationRule;

        /// <summary>
        /// 
        /// </summary>
        public readonly ATTR_DEF_ENTRY Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong MinSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly ulong MaxSize;

        #endregion Properties

        #region Constructors

        private AttrDef(byte[] bytes, int offset)
        {
            Name = Encoding.Unicode.GetString(bytes, offset, 0x80).TrimEnd('\0');
            Type = BitConverter.ToUInt32(bytes, offset + 0x80);
            DisplayRule = BitConverter.ToUInt32(bytes, offset + 0x84);
            #region CollationRuleSwitch

            switch (BitConverter.ToUInt32(bytes, 0x88))
            {
                case 0x00:
                    CollationRule = "Binary";
                    break;
                case 0x01:
                    CollationRule = "Filename";
                    break;
                case 0x02:
                    CollationRule = "Unicode String";
                    break;
                case 0x10:
                    CollationRule = "Unsigned Long";
                    break;
                case 0x11:
                    CollationRule = "SID";
                    break;
                case 0x12:
                    CollationRule = "Security Hash";
                    break;
                case 0x13:
                    CollationRule = "Multiple Unsigned Longs";
                    break;
                default:
                    CollationRule = "unknown";
                    break;
            }

            #endregion CollationRuleSwitch
            Flags = (ATTR_DEF_ENTRY)BitConverter.ToUInt32(bytes, offset + 0x8C);
            MinSize = BitConverter.ToUInt64(bytes, offset + 0x90);
            MaxSize = BitConverter.ToUInt64(bytes, offset + 0x98);
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static AttrDef[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            FileRecord record = FileRecord.Get(volume, MftIndex.ATTRDEF_INDEX, true);
            return AttrDef.GetInstances(record.GetContent());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AttrDef[] GetInstancesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return AttrDef.GetInstances(record.GetContent());
        }

        private static AttrDef[] GetInstances(byte[] bytes)
        {
            // Instantiate a List of AttrDef objects for output
            List<AttrDef> adList = new List<AttrDef>();

            // Iterate through 160 byte chunks (representing an AttrDef object)
            for (int i = 0; (i < bytes.Length) && (bytes[i] != 0); i += 0xA0)
            {
                // Intantiate a new AttrDef object and add it to the adList List of AttrDef objects
                adList.Add(new AttrDef(bytes, i));
            }
            return adList.ToArray();
        }

        #endregion Static Methods
    }
}