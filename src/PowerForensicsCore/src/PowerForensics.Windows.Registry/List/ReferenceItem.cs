using System;
using System.Text;

namespace PowerForensics.Windows.Registry
{
    /// <summary>
    /// 
    /// </summary>
    public class ReferenceItem : List
    {
        #region Constructors

        internal ReferenceItem(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x04, 0x02);
            
            if (Signature == "ri")
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

                for (int i = 0; i < Count; i++)
                {
                    offsetArray[i] = (BitConverter.ToUInt32(bytes, (i * 0x04) + 0x08) + RegistryHeader.HBINOFFSET);
                }

                Offset = offsetArray;
            }
            else
            {
                throw new Exception("List is not a valid Reference Item");
            }
        }

        internal ReferenceItem(ushort count, uint[] offset)
        {
            Count = count;
            Offset = offset;
        }

        #endregion Constructors
    }
}
