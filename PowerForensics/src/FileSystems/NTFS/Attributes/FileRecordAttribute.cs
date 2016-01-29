using System;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region AttrClass
    
    public class FileRecordAttribute
    {
        #region Constants

        internal const byte RESIDENT = 0x00;
        private const int COMMONHEADERSIZE = 0x10;
        private const int RESIDENTHEADERSIZE = 0x08;
        private const int NONRESIDENTHEADERSIZE = 0x30;

        #endregion Constants

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

        #region StaticMethods

        internal static FileRecordAttribute Get(byte[] bytes, string volume)
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
                ResidentHeader residentHeader = new ResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE, RESIDENTHEADERSIZE), commonHeader);

                #endregion ResidentHeader

                #region AttributeBytes

                // Create a byte[] representing the attribute itself
                int headerSize = COMMONHEADERSIZE + RESIDENTHEADERSIZE + (int)NameLength;
                byte[] attributeBytes = Helper.GetSubArray(bytes, headerSize, (int)commonHeader.TotalSize - headerSize);

                #endregion AttributeBytes

                #region ATTRSwitch

                switch (residentHeader.commonHeader.ATTRType)
                {
                    case (Int32)FileRecordAttribute.ATTR_TYPE.STANDARD_INFORMATION:
                        return new StandardInformation(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.ATTRIBUTE_LIST:
                        return new AttributeList(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.FILE_NAME:
                        return new FileName(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.OBJECT_ID:
                        return new ObjectId(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.VOLUME_NAME:
                        return new VolumeName(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.VOLUME_INFORMATION:
                        return new VolumeInformation(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.DATA:
                        return new Data(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.INDEX_ROOT:
                        return new IndexRoot(residentHeader, attributeBytes, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.EA:
                        //Console.WriteLine("EA");
                        return null;

                    case (Int32)FileRecordAttribute.ATTR_TYPE.EA_INFORMATION:
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
                NonResidentHeader nonresidentHeader = new NonResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE, NONRESIDENTHEADERSIZE), commonHeader);

                #endregion NonResidentHeader

                #region DataRun

                int headerSize = 0x00;

                if (commonHeader.NameOffset != 0x00)
                {
                    headerSize = commonHeader.NameOffset + (int)NameLength + ((int)NameLength % 8);
                }
                else
                {
                    headerSize = COMMONHEADERSIZE + NONRESIDENTHEADERSIZE;
                }

                return new NonResident(nonresidentHeader, Helper.GetSubArray(bytes, headerSize, (int)commonHeader.TotalSize - headerSize), attributeName);

                #endregion DataRun
            }
            #endregion NonResidentAttribute
        }

        internal static FileRecordAttribute GetTest(byte[] bytes, int offset, string volume)
        {
            #region CommonHeader

            if (bytes.Length == 0)
            {
                return null;
            }

            // Instantiate a Common Header Object
            CommonHeader commonHeader = new CommonHeader(bytes, offset);

            #endregion CommonHeader

            uint NameLength = (uint)commonHeader.NameLength * 2;

            // Decode Name byte[] into Unicode String
            string attributeName = Encoding.Unicode.GetString(bytes, (int)commonHeader.NameOffset + offset, (int)NameLength);

            // Determine if Attribute is Resident or NonResident
            bool resident = (bytes[0x08 + offset] == 0x00);

            #region ResidentAttribute

            // If Attribute is Resident
            if (resident)
            {
                #region ResidentHeader

                // Instantiate a Resident Header Object
                ResidentHeader residentHeader = new ResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE + offset, RESIDENTHEADERSIZE), commonHeader);

                #endregion ResidentHeader

                #region AttributeBytes

                int headerSize = COMMONHEADERSIZE + RESIDENTHEADERSIZE + (int)NameLength;
                int attributeoffset = headerSize + offset;
                //byte[] attributeBytes = Helper.GetSubArray(bytes, (uint)(headerSize + offset), commonHeader.TotalSize - (uint)headerSize);

                #endregion AttributeBytes

                #region ATTRSwitch

                switch (residentHeader.commonHeader.ATTRType)
                {
                    case (Int32)FileRecordAttribute.ATTR_TYPE.STANDARD_INFORMATION:
                        return new StandardInformation(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.ATTRIBUTE_LIST:
                        return new AttributeList(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.FILE_NAME:
                        return new FileName(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.OBJECT_ID:
                        return new ObjectId(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.VOLUME_NAME:
                        return new VolumeName(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.VOLUME_INFORMATION:
                        return new VolumeInformation(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.DATA:
                        return new Data(residentHeader, bytes, attributeoffset, attributeName);

                    case (Int32)FileRecordAttribute.ATTR_TYPE.INDEX_ROOT:
                        //return new IndexRoot(residentHeader, bytes, attributeoffset, attributeName);
                        return null;

                    case (Int32)FileRecordAttribute.ATTR_TYPE.EA:
                        //Console.WriteLine("EA");
                        return null;

                    case (Int32)FileRecordAttribute.ATTR_TYPE.EA_INFORMATION:
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
                NonResidentHeader nonresidentHeader = new NonResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE, NONRESIDENTHEADERSIZE), commonHeader);

                #endregion NonResidentHeader

                #region DataRun

                int headerSize = 0x00;

                if (commonHeader.NameOffset != 0x00)
                {
                    headerSize = commonHeader.NameOffset + (int)NameLength + ((int)NameLength % 8);
                }
                else
                {
                    headerSize = COMMONHEADERSIZE + NONRESIDENTHEADERSIZE;
                }

                //return new NonResident(nonresidentHeader, Helper.GetSubArray(bytes, (uint)headerSize, commonHeader.TotalSize - (uint)headerSize), attributeName);
                return null;

                #endregion DataRun
            }
            #endregion NonResidentAttribute
        }


        #endregion StaticMethods
    }

    #endregion AttrClass
}
