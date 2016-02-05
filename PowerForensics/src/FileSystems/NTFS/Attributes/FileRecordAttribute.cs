using System;
using System.Collections.Generic;
using System.Text;

namespace PowerForensics.Ntfs
{
    #region AttrClass
    
    public class FileRecordAttribute
    {
        #region Constants

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

        public static FileRecordAttribute[] GetInstances(byte[] bytes, int offset, int bytesPerFileRecord, string volume)
        {
            List<FileRecordAttribute> AttributeList = new List<FileRecordAttribute>();

            int i = offset;

            while (i < offset + bytesPerFileRecord)
            {
                // Get attribute size
                int attrSize = BitConverter.ToInt32(bytes, i + 0x04);

                if((attrSize == 0) || (attrSize + i > offset + bytesPerFileRecord))
                {
                    break;
                }

                FileRecordAttribute attr = Get(bytes, i, volume);

                i += attrSize;

                if (attr != null)
                {
                    AttributeList.Add(attr);
                }
            } 

            return AttributeList.ToArray();
        }

        /*internal static FileRecordAttribute Get(byte[] bytes, string volume)
        {
            #region CommonHeader

            if (bytes.Length == 0)
            {
                return null;
            }

            // Instantiate a Common Header Object
            CommonHeader commonHeader = new CommonHeader(bytes);
            
            // Decode Name byte[] into Unicode String
            string attributeName = Encoding.Unicode.GetString(bytes, commonHeader.NameOffset, commonHeader.NameLength);

            #endregion CommonHeader

            #region NonResidentAttribute

            // If Attribute is NonResident
            if (commonHeader.NonResident)
            {
                #region NonResidentHeader

                // Instantiate a Resident Header Object
                NonResidentHeader nonresidentHeader = new NonResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE, NONRESIDENTHEADERSIZE), commonHeader);

                #endregion NonResidentHeader

                #region DataRun

                int headerSize = 0x00;

                if (commonHeader.NameOffset != 0x00)
                {
                    headerSize = commonHeader.NameOffset + commonHeader.NameLength + (commonHeader.NameLength % 8);
                }
                else
                {
                    headerSize = COMMONHEADERSIZE + NONRESIDENTHEADERSIZE;
                }

                return new NonResident(nonresidentHeader, Helper.GetSubArray(bytes, headerSize, (int)commonHeader.TotalSize - headerSize), attributeName);

                #endregion DataRun
            }

            #endregion NonResidentAttribute

            #region ResidentAttribute
            // Else Attribute is Resident
            else
            {
                #region ResidentHeader

                // Instantiate a Resident Header Object
                ResidentHeader residentHeader = new ResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE, RESIDENTHEADERSIZE), commonHeader);

                #endregion ResidentHeader

                #region AttributeBytes

                // Create a byte[] representing the attribute itself
                int headerSize = COMMONHEADERSIZE + RESIDENTHEADERSIZE + commonHeader.NameLength;
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
        }*/

        internal static FileRecordAttribute Get(byte[] bytes, int offset, string volume)
        {
            #region CommonHeader

            // Instantiate a Common Header Object
            CommonHeader commonHeader = new CommonHeader(bytes, offset);

            // Decode Name byte[] into Unicode String
            string attributeName = Encoding.Unicode.GetString(bytes, commonHeader.NameOffset + offset, commonHeader.NameLength);

            #endregion CommonHeader

            #region NonResidentAttribute

            // If Attribute is NonResident
            if (commonHeader.NonResident)
            {
                #region NonResidentHeader

                // Instantiate a Resident Header Object
                NonResidentHeader nonresidentHeader = new NonResidentHeader(bytes, commonHeader, COMMONHEADERSIZE + offset);

                #endregion NonResidentHeader

                #region DataRun

                int headerSize = 0x00;

                if (commonHeader.NameOffset != 0x00)
                {
                    headerSize = commonHeader.NameOffset + commonHeader.NameLength + (commonHeader.NameLength % 8);
                }
                else
                {
                    headerSize = COMMONHEADERSIZE + NONRESIDENTHEADERSIZE;
                }

                int attributeoffset = headerSize + offset;

                return new NonResident(nonresidentHeader, bytes, attributeoffset, attributeName);

                #endregion DataRun
            }

            #endregion NonResidentAttribute

            #region ResidentAttribute
            // Else Attribute is Resident
            else
            {
                #region ResidentHeader

                // Instantiate a Resident Header Object
                ResidentHeader residentHeader = new ResidentHeader(Helper.GetSubArray(bytes, COMMONHEADERSIZE + offset, RESIDENTHEADERSIZE), commonHeader);

                #endregion ResidentHeader
                
                int headerSize = COMMONHEADERSIZE + RESIDENTHEADERSIZE + commonHeader.NameLength;
                int attributeoffset = headerSize + offset;

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
                        return new IndexRoot(residentHeader, bytes, attributeoffset, attributeName);

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
        }

        #endregion StaticMethods
    }

    #endregion AttrClass
}
