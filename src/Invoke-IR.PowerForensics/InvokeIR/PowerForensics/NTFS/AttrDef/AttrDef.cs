using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace InvokeIR.PowerForensics.NTFS.AttrDef
{
    class AttrDef
    {

        internal enum ATTR_DEF_ENTRY
        {
            INDEX = 0x20,
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
            internal ulong MaxSize;
            internal ulong MinSize;

            internal ATTR_DEF(byte[] bytes)
            {
                Name = Encoding.Unicode.GetString(bytes.Take(128).ToArray());
                TypeIdentified = BitConverter.ToUInt32(bytes, 128);
                DisplayRule = BitConverter.ToUInt32(bytes, 132);
                CollationRule = BitConverter.ToUInt32(bytes, 136);
                Flags = BitConverter.ToUInt32(bytes, 140);
                MaxSize = BitConverter.ToUInt64(bytes, 144);
                MinSize = BitConverter.ToUInt64(bytes, 152);
            }

        }

        public string Name;
        public uint Type;
        public string Flags;
        public ulong MaxSize;
        public ulong MinSize;

        internal AttrDef(string name, uint type, string flags, ulong maxSize, ulong minSize)
        {
            Name = name;
            Type = type;
            Flags = flags;
            MaxSize = maxSize;
            MinSize = minSize;
        }

        internal static AttrDef Get(byte[] bytes)
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


            return new AttrDef(
                attrDefStruct.Name,
                attrDefStruct.TypeIdentified,
                flags.ToString(),
                attrDefStruct.MaxSize,
                attrDefStruct.MinSize);

        }

    }

}
