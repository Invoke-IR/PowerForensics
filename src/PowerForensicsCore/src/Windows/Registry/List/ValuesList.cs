using System;

namespace PowerForensics.Registry
{
    #region ValuesListClass
    
    public class ValuesList : Cell
    {
        #region Properties

        public readonly uint[] Offset;

        #endregion Properties

        #region Constructors

        internal ValuesList(byte[] bytes, uint count)
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

            uint[] offsetArray = new uint[count];

            for (int i = 0; i < count; i++)
            {
                int o = (i * 4) + 4;
                offsetArray[i] = BitConverter.ToUInt32(bytes, o) + RegistryHeader.HBINOFFSET;    
            }

            Offset = offsetArray;
        }

        #endregion Constructors
    }

    #endregion ValuesListClass
}
