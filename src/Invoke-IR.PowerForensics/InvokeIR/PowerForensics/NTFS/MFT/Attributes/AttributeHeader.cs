using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{
    class AttrHeader
    {

        private const byte RESIDENT = 0x00;
        private const byte NONRESIDENT = 0x01;

        internal struct ATTR_HEADER_COMMON
        {
            
            internal uint ATTRType;			// Attribute Type
            internal uint TotalSize;		// Length (including this header)
            internal bool NonResident;	    // 0 - resident, 1 - non resident
            internal byte NameLength;		// name length in words
            internal ushort NameOffset;		// offset to the name
            internal ushort Flags;			// Flags
            internal ushort Id;				// Attribute Id

            internal ATTR_HEADER_COMMON(byte[] bytes)
            {
        
                ATTRType = BitConverter.ToUInt32(bytes, 0);
                TotalSize = BitConverter.ToUInt32(bytes, 4);
                NonResident = (bytes[8] == NONRESIDENT);
                NameLength = bytes[9];
                NameOffset = BitConverter.ToUInt16(bytes, 10);
                Flags = BitConverter.ToUInt16(bytes, 12);
                Id = BitConverter.ToUInt16(bytes, 14);
            
            }
        
        }

        internal struct ATTR_HEADER_RESIDENT
        {
            
            internal ATTR_HEADER_COMMON commonHeader;	// Common data structure
            internal uint AttrSize;		                // Length of the attribute body
            internal ushort AttrOffset;		            // Offset to the Attribute
            internal byte IndexedFlag;	                // Indexed flag
            internal byte Padding;		                // Padding

            internal ATTR_HEADER_RESIDENT(byte[] bytes)
            {
            
                commonHeader = new ATTR_HEADER_COMMON(bytes.Take(16).ToArray());
                AttrSize = BitConverter.ToUInt32(bytes, 16);
                AttrOffset = BitConverter.ToUInt16(bytes, 20);
                IndexedFlag = bytes[22];
                Padding = bytes[23];
        
            }
    
        }
    
    }

}
