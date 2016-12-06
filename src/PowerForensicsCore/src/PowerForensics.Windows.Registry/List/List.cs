using System;
using System.Text;

namespace PowerForensics.Windows.Registry
{
    /// <summary>
    /// 
    /// </summary>
    public class List
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int Size;

        /// <summary>
        /// 
        /// </summary>
        public string Signature;

        /// <summary>
        /// 
        /// </summary>
        public bool Allocated;

        /// <summary>
        /// 
        /// </summary>
        public ushort Count;

        /// <summary>
        /// 
        /// </summary>
        public uint[] Offset;

        #endregion Properties

        #region Factory

        internal static List Factory(byte[] bytes, byte[] subKeyListBytes, string type)
        {
            if (type == "lf")
            {
                return new Leaf(subKeyListBytes);
            }
            else if (type == "lh")
            {
                return new HashedLeaf(subKeyListBytes);
            }
            else if (type == "li")
            {
                return new LeafItem(subKeyListBytes);
            }
            else if (type == "ri")
            {
                List ri = new ReferenceItem(subKeyListBytes);

                List[] listArray = new List[ri.Count];

                for (int i = 0; i < ri.Offset.Length; i++)
                {
                    byte[] sublistBytes = Helper.GetSubArray(bytes, (int)ri.Offset[i], Math.Abs(BitConverter.ToInt32(bytes, (int)ri.Offset[i])));
                    string subtype = Encoding.ASCII.GetString(sublistBytes, 0x04, 0x02);

                    listArray[i] = List.Factory(bytes, sublistBytes, subtype);
                }

                ushort aggCount = 0;
                foreach (List l in listArray)
                {
                    aggCount += l.Count;
                }

                uint[] aggOffset = new uint[aggCount];
                int j = 0;
                foreach (List l in listArray)
                {
                    for (int k = 0; (k < l.Count) && (j < aggCount); k++)
                    {
                        aggOffset[j] = l.Offset[k];
                        j++;
                    }
                }

                return new ReferenceItem(aggCount, aggOffset);
            }
            else
            {
                return null;
            }
        }

        #endregion Factory
    }
}
