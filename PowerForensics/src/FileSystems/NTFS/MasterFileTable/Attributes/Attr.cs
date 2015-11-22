using System;

namespace PowerForensics.Ntfs
{
    #region AttrClass
    
    public class Attr
    {
        #region Enums

        public enum ATTR_TYPE
        {
            STANDARD_INFORMATION = 0x10,
            ATTRIBUTE_LIST = 0x20,
            FILE_NAME = 0x30,
            OBJECT_ID = 0x40,
            SECURITY_DESCRIPTOR = 0x50,
            VOLUME_NAME = 0x60,
            VOLUME_INFORMATION = 0x70,
            DATA = 0x80,
            INDEX_ROOT = 0x90,
            INDEX_ALLOCATION = 0xA0,
            BITMAP = 0xB0,
            REPARSE_POINT = 0xC0,
            EA_INFORMATION = 0xD0,
            EA = 0xE0,
            LOGGED_UTILITY_STREAM = 0x100,

            ATTR_FLAG_COMPRESSED = 0x0001,
            ATTR_FLAG_ENCRYPTED = 0x4000,
            ATTR_FLAG_SPARSE = 0x8000
        }

        #endregion Enums

        #region Properties

        public ATTR_TYPE Name;
        public string NameString;
        internal bool NonResident;
        public ushort AttributeId;

        #endregion Properties
    }

    #endregion AttrClass
}
