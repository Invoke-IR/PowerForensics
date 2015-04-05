using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{

    public class Attr
    {

        internal enum ATTR_TYPE
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

        public string Name;
        public string NameString;
        public bool NonResident;
        public ushort AttributeId;

        public static byte[] GetBytes(byte[] recordBytes, uint attribute)
        {

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            MFTRecord.FILE_RECORD_HEADER RecordHeader = new MFTRecord.FILE_RECORD_HEADER(recordBytes);

            int offsetToATTR = RecordHeader.OffsetOfAttr;

            while (offsetToATTR < (RecordHeader.RealSize - 8))
            {
                
                AttrHeader.ATTR_HEADER_COMMON commonAttributeHeader = new AttrHeader.ATTR_HEADER_COMMON(recordBytes.Skip(offsetToATTR).Take(16).ToArray());
                if (commonAttributeHeader.ATTRType == attribute)
                {
                    
                    // Return bytes for Attr
                    return recordBytes.Skip(offsetToATTR).Take((int)commonAttributeHeader.TotalSize).ToArray();
                     
                }

                else
                {
                    
                    // Change offsetToATTR to next Attr
                    offsetToATTR += (int)commonAttributeHeader.TotalSize;
                
                }

            }
            
            // Add some sort of exception handling here...
            return null;

        }

        public static Attr Get(byte[] recordBytes, uint attribute)
        {

            byte[] attrBytes = Attr.GetBytes(recordBytes, attribute);
            int offsetToAttr = 0;
            return AttributeFactory.Get(attrBytes, 0, out offsetToAttr);
            
        }

    }

}
