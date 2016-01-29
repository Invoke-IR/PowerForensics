using System;

namespace PowerForensics.Ntfs
{
    #region CommonHeaderClass

    class CommonHeader
    {
        #region Constants

        internal const byte RESIDENT = 0x00;
        internal const byte NONRESIDENT = 0x01;

        #endregion Constants

        #region Properties

        internal uint ATTRType;			// Attribute Type
        internal uint TotalSize;		// Length (including this header)
        internal bool NonResident;	    // 0 - resident, 1 - non resident
        internal byte NameLength;		// name length in words
        internal ushort NameOffset;		// offset to the name
        internal ushort Flags;			// Flags
        internal ushort Id;				// Attribute Id

        #endregion Properties

        #region Constructors

        internal CommonHeader(byte[] bytes)
        {
            ATTRType = BitConverter.ToUInt32(bytes, 0x00);
            TotalSize = BitConverter.ToUInt32(bytes, 0x04);
            NonResident = (bytes[0x08] == NONRESIDENT);
            NameLength = bytes[0x09];
            NameOffset = BitConverter.ToUInt16(bytes, 0x0A);
            Flags = BitConverter.ToUInt16(bytes, 0x0C);
            Id = BitConverter.ToUInt16(bytes, 0x0E);
        }

        internal CommonHeader(byte[] bytes, int offset)
        {
            ATTRType = BitConverter.ToUInt32(bytes, 0x00 + offset);
            TotalSize = BitConverter.ToUInt32(bytes, 0x04 + offset);
            NonResident = (bytes[0x08 + offset] == NONRESIDENT);
            NameLength = bytes[0x09 + offset];
            NameOffset = BitConverter.ToUInt16(bytes, 0x0A + offset);
            Flags = BitConverter.ToUInt16(bytes, 0x0C + offset);
            Id = BitConverter.ToUInt16(bytes, 0x0E + offset);
        }

        #endregion Constructors
    }

    #endregion CommonHeaderClass
}
