using System;
using System.Text;

namespace PowerForensics.Windows.Registry
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityKey : Cell
    {
        #region Properties

        internal readonly uint Flink;
        internal readonly uint Blink;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint ReferenceCount;

        internal readonly uint DescriptorLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly SecurityDescriptor Descriptor;

        #endregion Properties

        #region Constructors

        internal SecurityKey(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x04, 0x02);
            
            if (Signature == "sk")
            {
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
                Flink = BitConverter.ToUInt32(bytes, 0x08);
                Blink = BitConverter.ToUInt32(bytes, 0x0C);
                ReferenceCount = BitConverter.ToUInt32(bytes, 0x10);
                DescriptorLength = BitConverter.ToUInt32(bytes, 0x14);
                Descriptor = new SecurityDescriptor(Helper.GetSubArray(bytes, 0x18, (int)DescriptorLength));
            }
            else
            {
                throw new Exception("Cell is not a valid Security Key");
            }
        }

        #endregion Constructors
    }
}