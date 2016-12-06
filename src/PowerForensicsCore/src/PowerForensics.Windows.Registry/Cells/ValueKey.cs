using System;
using System.Text;
using System.Collections.Generic;

namespace PowerForensics.Windows.Registry
{
    //TODO: Determine if Data is Resident or NonResident
    //TODO: Get Data Buffer
    //TODO: Interpret Data based on Data Type
    /// <summary>
    /// 
    /// </summary>
    public class ValueKey : Cell
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum VALUE_KEY_DATA_TYPES
        {
            /// <summary>
            /// 
            /// </summary>
            REG_NONE = 0x00000000,

            /// <summary>
            /// 
            /// </summary>
            REG_SZ = 0x00000001,

            /// <summary>
            /// 
            /// </summary>
            REG_EXPAND_SZ = 0x00000002,

            /// <summary>
            /// 
            /// </summary>
            REG_BINARY = 0x00000003,

            /// <summary>
            /// 
            /// </summary>
            REG_DWORD = 0x00000004,

            /// <summary>
            /// 
            /// </summary>
            REG_DWORD_BIG_ENDIAN = 0x00000005,

            /// <summary>
            /// 
            /// </summary>
            REG_LINK = 0x00000006,

            /// <summary>
            /// 
            /// </summary>
            REG_MULTI_SZ = 0x00000007,

            /// <summary>
            /// 
            /// </summary>
            REG_RESOURCE_LIST = 0x00000008,

            /// <summary>
            /// 
            /// </summary>
            REG_FULL_RESOURCE_DESCRIPTOR = 0x00000009,

            /// <summary>
            /// 
            /// </summary>
            REG_RESOURCE_REQUIREMENTS_LIST = 0x0000000A,

            /// <summary>
            /// 
            /// </summary>
            REG_QWORD = 0x0000000B
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum VALUE_KEY_FLAGS
        {
            /// <summary>
            /// 
            /// </summary>
            NameIsUnicode = 0x0000,

            /// <summary>
            /// 
            /// </summary>
            NameIsAscii = 0x0001,
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string HivePath;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly string Key;

        internal readonly ushort NameLength;

        internal readonly uint DataLength;

        internal readonly bool ResidentData;

        internal readonly uint DataOffset;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly VALUE_KEY_DATA_TYPES DataType;

        internal readonly VALUE_KEY_FLAGS Flags;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Data;

        #endregion Properties

        #region Constructors

        internal ValueKey(byte[] bytes, string path, string key)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x04, 0x02);
            
            if (Signature == "vk")
            {
                HivePath = path;
                Key = key;
                #region CellHeader

                Size = BitConverter.ToInt32(bytes, 0x00);

                if (Size >= 0)
                {
                    Allocated = false;
                }
                else
                {
                    Allocated = true;
                }

                #endregion CellHeader
                NameLength = BitConverter.ToUInt16(bytes, 0x06);
                #region DataLength

                uint dataLengthRaw = BitConverter.ToUInt32(bytes, 0x08);

                if (dataLengthRaw > 0x80000000)
                {
                    DataLength = dataLengthRaw - 0x80000000;
                    ResidentData = true;
                }
                else
                {
                    DataLength = dataLengthRaw;
                    ResidentData = false;
                }
                
                #endregion DataLength
                DataOffset = BitConverter.ToUInt32(bytes, 0x0C) + RegistryHeader.HBINOFFSET;
                DataType = (VALUE_KEY_DATA_TYPES)BitConverter.ToUInt32(bytes, 0x10);
                Flags = (VALUE_KEY_FLAGS)BitConverter.ToUInt16(bytes, 0x14);
                #region ValueName

                if (NameLength == 0)
                {
                    Name = "(Default)";
                }
                else
                {
                    if (Flags == VALUE_KEY_FLAGS.NameIsAscii)
                    {
                        Name = Encoding.ASCII.GetString(bytes, 0x18, NameLength);
                    }
                    else
                    {
                        Name = Encoding.Unicode.GetString(bytes, 0x18, NameLength);
                    }
                }

                #endregion ValueName

                Data = this.GetData();
            }
            else
            {
                throw new Exception("Cell is not a valid Value Key");
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ValueKey Get(string path, string key, string val)
        {
            byte[] bytes = RegistryHelper.GetHiveBytes(path);

            NamedKey hiveroot = RegistryHelper.GetRootKey(bytes, path);

            NamedKey nk = hiveroot;

            if (key != null)
            {
                foreach (string k in key.Split('\\'))
                {
                    foreach (NamedKey n in nk.GetSubKeys(bytes))
                    {
                        if (n.Name.ToUpper() == k.ToUpper())
                        {
                            nk = n;
                        }
                    }
                }
                if (nk == hiveroot)
                {
                    throw new Exception(string.Format("Cannot find key '{0}' in the '{1}' hive because it does not exist.", key, path));
                }
            }

            ValueKey[] values = nk.GetValues(bytes);

            foreach (ValueKey v in values)
            {
                if (v.Name.ToUpper() == val.ToUpper())
                {
                    return v;
                }
            }

            throw new Exception(string.Format("Cannot find value '{0}' as a value of '{1}' in the '{2}' hive because it does not exist.", val, key, path));
        }

        internal static ValueKey Get(byte[] bytes, string path, string key, string val)
        {
            NamedKey hiveroot = RegistryHelper.GetRootKey(bytes, path);

            NamedKey nk = hiveroot;

            if (key != null)
            {
                foreach (string k in key.Split('\\'))
                {
                    foreach (NamedKey n in nk.GetSubKeys(bytes))
                    {
                        if (n.Name.ToUpper() == k.ToUpper())
                        {
                            nk = n;
                        }
                    }
                }
            }

            ValueKey[] values = nk.GetValues(bytes);

            foreach (ValueKey v in values)
            {
                if (v.Name.ToUpper() == val.ToUpper())
                {
                    return v;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ValueKey[] GetInstances(string path, string key)
        {
            byte[] bytes = RegistryHelper.GetHiveBytes(path);

            NamedKey hiveroot = RegistryHelper.GetRootKey(bytes, path);

            NamedKey nk = hiveroot;

            if (key != null)
            {
                foreach (string k in key.Split('\\'))
                {
                    foreach (NamedKey n in nk.GetSubKeys(bytes))
                    {
                        if (n.Name.ToUpper() == k.ToUpper())
                        {
                            nk = n;
                        }
                    }
                }
            }

            return nk.GetValues(bytes);
        }

        internal static ValueKey[] GetInstances(byte[] bytes, string path, string key)
        {
            NamedKey hiveroot = RegistryHelper.GetRootKey(bytes, path);

            NamedKey nk = hiveroot;

            if (key != null)
            {
                foreach (string k in key.Split('\\'))
                {
                    foreach (NamedKey n in nk.GetSubKeys(bytes))
                    {
                        if (n.Name.ToUpper() == k.ToUpper())
                        {
                            nk = n;
                        }
                    }
                }
            }

            return nk.GetValues(bytes);
        }

        #endregion Static Methods

        #region Instance Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetData()
        {
            return this.GetData(RegistryHelper.GetHiveBytes(this.HivePath));
        }

        internal object GetData(byte[] bytes)
        {
            if (this.ResidentData)
            {
                return BitConverter.GetBytes(this.DataOffset - RegistryHeader.HBINOFFSET);
            }
            else if (Encoding.ASCII.GetString(bytes, (int)this.DataOffset + 0x04, 0x02) == "db")
            {
                return BigData.Get(bytes, this);
            }
            else
            {
                switch (this.DataType)
                {
                    case VALUE_KEY_DATA_TYPES.REG_NONE:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_SZ:
                        return Encoding.Unicode.GetString(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength).TrimEnd('\0');
                    case VALUE_KEY_DATA_TYPES.REG_EXPAND_SZ:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_BINARY:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_DWORD:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_DWORD_BIG_ENDIAN:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_LINK:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_MULTI_SZ:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_RESOURCE_LIST:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_FULL_RESOURCE_DESCRIPTOR:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_RESOURCE_REQUIREMENTS_LIST:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    case VALUE_KEY_DATA_TYPES.REG_QWORD:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                    default:
                        return Helper.GetSubArray(bytes, (int)this.DataOffset + 0x04, (int)this.DataLength);
                }
                
            }
        }

        #endregion Instance Methods
    }

    class BigData
    {
        #region Static Methods

        internal static byte[] Get(byte[] bytes, ValueKey vk)
        {
            List<byte> contents = new List<byte>();
            
            byte[] dataBytes = PowerForensics.Helper.GetSubArray(bytes, (int)vk.DataOffset, Math.Abs(BitConverter.ToInt32(bytes, (int)vk.DataOffset)));

            short offsetCount = BitConverter.ToInt16(dataBytes, 0x06);
            uint offsetOffset = BitConverter.ToUInt32(dataBytes, 0x08) + RegistryHeader.HBINOFFSET;

            byte[] offsetBytes = Helper.GetSubArray(bytes, (int)offsetOffset, Math.Abs(BitConverter.ToInt32(bytes, (int)offsetOffset)));

            for (short i = 1; i <= offsetCount; i++)
            {
                uint segmentOffset = BitConverter.ToUInt32(offsetBytes, i * 0x04) + RegistryHeader.HBINOFFSET;
                contents.AddRange(Helper.GetSubArray(bytes, (int)segmentOffset + 0x04, Math.Abs(BitConverter.ToInt32(bytes, (int)segmentOffset)) - 0x08));
            }

            byte[] b = contents.ToArray();
            return Helper.GetSubArray(b, 0x00, b.Length);
        }

        #endregion Static Methods
    }
}