using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region AttributeFactory

    class AttributeFactory
    {
        #region Constants

        internal const byte RESIDENT = 0x00;
        private const int COMMONHEADERSIZE = 0x10;
        private const int RESIDENTHEADERSIZE = 0x08;
        private const int NONRESIDENTHEADERSIZE = 0x30;

        #endregion Constants

        #region StaticMethods

        internal static Attr Get(byte[] bytes, string volume)
        {
            #region CommonHeader

            if (bytes.Length == 0)
            {
                return null;
            }

            // Instantiate a Common Header Object
            CommonHeader commonHeader = new CommonHeader(bytes);

            #endregion CommonHeader

            uint NameLength = (uint)commonHeader.NameLength * 2;

            // Decode Name byte[] into Unicode String
            string attributeName = Encoding.Unicode.GetString(bytes, (int)commonHeader.NameOffset, (int)NameLength);
            
            // Determine if Attribute is Resident or NonResident
            bool resident = (bytes[8] == 0x00);

            #region ResidentAttribute

            // If Attribute is Resident
            if (resident)
            {
                #region ResidentHeader
                
                // Instantiate a Resident Header Object
                ResidentHeader residentHeader = new ResidentHeader(Util.GetSubArray(bytes, COMMONHEADERSIZE, RESIDENTHEADERSIZE), commonHeader);

                #endregion ResidentHeader

                #region AttributeBytes

                // Create a byte[] representing the attribute itself
                int headerSize = COMMONHEADERSIZE + RESIDENTHEADERSIZE + (int)NameLength;
                byte[] attributeBytes = Util.GetSubArray(bytes, (uint)headerSize, commonHeader.TotalSize - (uint)headerSize);

                #endregion AttributeBytes
                
                #region ATTRSwitch

                switch (residentHeader.commonHeader.ATTRType)
                {
                    case (Int32)Attr.ATTR_TYPE.STANDARD_INFORMATION:
                        return new StandardInformation(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.ATTRIBUTE_LIST:
                        return new AttributeList(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.FILE_NAME:
                        return new FileName(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.OBJECT_ID:
                        return new ObjectId(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.VOLUME_NAME:
                        return new VolumeName(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.VOLUME_INFORMATION:
                        return new VolumeInformation(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.DATA:
                        return new Data(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.INDEX_ROOT:
                        return new IndexRoot(residentHeader, attributeBytes, attributeName);

                    case (Int32)Attr.ATTR_TYPE.EA:
                        //Console.WriteLine("EA");
                        return null;

                    case (Int32)Attr.ATTR_TYPE.EA_INFORMATION:
                        //Console.WriteLine("EA_INFORMATION");
                        return null;

                    default:
                        return null;
                }

                #endregion ATTRSwitch
            }

            #endregion ResidentAttribute

            #region NonResidentAttribute
            // Else Attribute is Non-Resident
            else
            {
                #region NonResidentHeader

                // Instantiate a Resident Header Object
                NonResidentHeader nonresidentHeader = new NonResidentHeader(Util.GetSubArray(bytes, COMMONHEADERSIZE, NONRESIDENTHEADERSIZE), commonHeader);

                #endregion NonResidentHeader

                #region DataRun

                int headerSize = 0;

                if (commonHeader.NameOffset != 0) 
                {
                    headerSize = commonHeader.NameOffset + (int)NameLength + ((int)NameLength % 8);
                }
                else
                {
                    headerSize = COMMONHEADERSIZE + NONRESIDENTHEADERSIZE;
                }

                return new NonResident(nonresidentHeader, Util.GetSubArray(bytes, (uint)headerSize, commonHeader.TotalSize - (uint)headerSize), attributeName);

                #endregion DataRun
            }
            #endregion NonResidentAttribute
        }

        #endregion StaticMethods
    }

    #endregion AttributeFactory
}
