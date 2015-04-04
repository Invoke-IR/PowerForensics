using System;
using System.Linq;
using System.Text;
using InvokeIR.PowerForensics.NTFS.MFT.Attributes;

namespace InvokeIR.PowerForensics.NTFS.MFT.Attributes
{
    class AttributeFactory
    {

        internal static Attr Get(byte[] Bytes, int offset, out int offsetToATTR)
        {
            // This needs to be looked at...
            if (BitConverter.ToUInt32(Bytes.Skip(offset).Take(4).ToArray(), 0) != 0xD0)
            {
                AttrHeader.ATTR_HEADER_COMMON commonAttributeHeader = new AttrHeader.ATTR_HEADER_COMMON(Bytes.Skip(offset).Take(16).ToArray());
                
                // Get byte[] representing the current attribute 
                byte[] AttrBytes = Bytes.Skip(offset).Take((int)commonAttributeHeader.TotalSize).ToArray();
                
                // Get byte[] representing the Attribute Name
                byte[] NameBytes = AttrBytes.Skip(commonAttributeHeader.NameOffset).Take(commonAttributeHeader.NameLength * 2).ToArray();
                
                // Decode byte[] into Unicode String
                string AttrName = Encoding.Unicode.GetString(NameBytes);

                // Update offset value
                offset += (int)commonAttributeHeader.TotalSize;
                
                // Set offset return
                offsetToATTR = offset;

                // If attribute is non-resident
                if (commonAttributeHeader.NonResident)
                {
                    return NonResident.Get(AttrBytes, AttrName);
                }
                
                // If attribute is resident
                else
                {

                    #region ATTRSwitch

                    switch (commonAttributeHeader.ATTRType)
                    {

                        case (Int32)Attr.ATTR_TYPE.STANDARD_INFORMATION:
                            return StandardInformation.Get(AttrBytes, AttrName);

                        case (Int32)Attr.ATTR_TYPE.FILE_NAME:
                            return FileName.Get(AttrBytes, AttrName);

                        case (Int32)Attr.ATTR_TYPE.OBJECT_ID:
                            return ObjectId.Get(AttrBytes, AttrName);

                        case (Int32)Attr.ATTR_TYPE.VOLUME_NAME:
                            return VolumeName.Get(AttrBytes, AttrName);

                        case (Int32)Attr.ATTR_TYPE.VOLUME_INFORMATION:
                            return VolumeInformation.Get(AttrBytes, AttrName);

                        case (Int32)Attr.ATTR_TYPE.DATA:
                            return Data.Get(AttrBytes, AttrName);

                        case (Int32)Attr.ATTR_TYPE.INDEX_ROOT:
                            //IndexRoot indxRootAttr = IndexRoot.Get(AttrBytes, commonAttributeHeader, AttrHeaderResident, AttrName);
                            break;
                        
                        case (Int32)Attr.ATTR_TYPE.EA_INFORMATION:
                            //
                            //Console.WriteLine("Attr: EA_Information {0}", commonAttributeHeader.Id);
                            break;

                        case (Int32)Attr.ATTR_TYPE.EA:
                            //
                            //Console.WriteLine("Attr: EA {0}", commonAttributeHeader.Id);
                            break;

                        default:
                            break;

                    }

                    #endregion ATTRSwitch

                }

                return null;

            }
            else
            {
                offsetToATTR = 1025;
                return null;
            }
        }

    }

}
