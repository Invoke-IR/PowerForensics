using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace InvokeIR.PowerForensics.NTFS
{
    class AttrDef
    {

        internal enum ATTR_DEF_ENTRY
        {
            INDEX = 0x02,
            ALWAYS_RESIDENT = 0x40,
            ALWAYS_NONRESIDENT = 0x80
        }
        
        internal struct ATTR_DEF
        {
            internal string Name;
            internal uint TypeIdentified;
            internal uint DisplayRule;
            internal uint CollationRule;
            internal uint Flags;
            internal ulong MinSize;
            internal ulong MaxSize;

            internal ATTR_DEF(byte[] bytes)
            {
                Name = Encoding.Unicode.GetString(bytes.Take(0x80).ToArray()).TrimEnd('\0');
                TypeIdentified = BitConverter.ToUInt32(bytes, 0x80);
                DisplayRule = BitConverter.ToUInt32(bytes, 0x84);
                CollationRule = BitConverter.ToUInt32(bytes, 0x88);
                Flags = BitConverter.ToUInt32(bytes, 0x8C);
                MinSize = BitConverter.ToUInt64(bytes, 0x90);
                MaxSize = BitConverter.ToUInt64(bytes, 0x98);
            }

        }

        #region Properties

        public string Name;
        public uint Type;
        public string Flags;
        public ulong MinSize;
        public ulong MaxSize;

        #endregion Properties

        #region Constructors

        internal AttrDef(byte[] bytes)
        {
            ATTR_DEF attrDefStruct = new ATTR_DEF(bytes);

            #region attrDefFlags

            StringBuilder flags = new StringBuilder();
            if (attrDefStruct.Flags != 0)
            {
                if ((attrDefStruct.Flags & (uint)ATTR_DEF_ENTRY.INDEX) == (uint)ATTR_DEF_ENTRY.INDEX)
                {
                    flags.Append("Index, ");
                }
                if ((attrDefStruct.Flags & (uint)ATTR_DEF_ENTRY.ALWAYS_RESIDENT) == (uint)ATTR_DEF_ENTRY.ALWAYS_RESIDENT)
                {
                    flags.Append("Always Resident, ");
                }
                if ((attrDefStruct.Flags & (uint)ATTR_DEF_ENTRY.ALWAYS_NONRESIDENT) == (uint)ATTR_DEF_ENTRY.ALWAYS_NONRESIDENT)
                {
                    flags.Append("Always Non-Resident, ");
                }
                if (flags.Length > 2)
                {
                    flags.Length -= 2;

                }
            }
            #endregion stdInfoFlags

            Name = attrDefStruct.Name;
            Type = attrDefStruct.TypeIdentified;
            Flags = flags.ToString();
            MinSize = attrDefStruct.MinSize;
            MaxSize = attrDefStruct.MaxSize;
        }

        #endregion Constructors

    }

}
