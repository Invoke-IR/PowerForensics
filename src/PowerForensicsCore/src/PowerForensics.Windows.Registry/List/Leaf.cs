using System;
using System.Text;

namespace PowerForensics.Windows.Registry
{
    /// <summary>
    /// 
    /// </summary>
    public class Leaf : List
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string[] HashValue;

        #endregion Properties

        #region Constructors

        internal Leaf(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x04, 0x02);

            if (Signature == "lf")
            {
                #region ListHeader

                Size = BitConverter.ToInt32(bytes, 0x00);

                if (Size >= 0)
                {
                    Allocated = false;
                }
                else
                {
                    Allocated = true;
                }

                Count = BitConverter.ToUInt16(bytes, 0x06);

                #endregion ListHeader

                uint[] offsetArray = new uint[Count];
                string[] hashArray = new string[Count];

                for (int i = 0; i < Count; i++)
                {
                    offsetArray[i] = (BitConverter.ToUInt32(bytes, (i * 0x08) + 0x08) + RegistryHeader.HBINOFFSET);
                    hashArray[i] = Encoding.ASCII.GetString(bytes, (i * 0x08) + 0x0C, 0x04);
                }

                Offset = offsetArray;
                HashValue = hashArray;
            }
            else
            {
                throw new Exception("List is not a valid Leaf");
            }
        }

        #endregion Constructors
    }
}