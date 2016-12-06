using System;
using System.Collections.Generic;
using System.Text;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class FileRecordAttribute
    {
        #region Constants

        private const int COMMONHEADERSIZE = 0x10;
        private const int RESIDENTHEADERSIZE = 0x08;
        private const int NONRESIDENTHEADERSIZE = 0x30;

        #endregion Constants

        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum ATTR_TYPE
        {

            /// <summary>
            /// 
            /// </summary>
            STANDARD_INFORMATION = 0x10,

            /// <summary>
            /// 
            /// </summary>
            ATTRIBUTE_LIST = 0x20,

            /// <summary>
            /// 
            /// </summary>
            FILE_NAME = 0x30,

            /// <summary>
            /// 
            /// </summary>
            OBJECT_ID = 0x40,

            /// <summary>
            /// 
            /// </summary>
            SECURITY_DESCRIPTOR = 0x50,

            /// <summary>
            /// 
            /// </summary>
            VOLUME_NAME = 0x60,

            /// <summary>
            /// 
            /// </summary>
            VOLUME_INFORMATION = 0x70,

            /// <summary>
            /// 
            /// </summary>
            DATA = 0x80,

            /// <summary>
            /// 
            /// </summary>
            INDEX_ROOT = 0x90,

            /// <summary>
            /// 
            /// </summary>
            INDEX_ALLOCATION = 0xA0,

            /// <summary>
            /// 
            /// </summary>
            BITMAP = 0xB0,

            /// <summary>
            /// 
            /// </summary>
            REPARSE_POINT = 0xC0,

            /// <summary>
            /// 
            /// </summary>
            EA_INFORMATION = 0xD0,

            /// <summary>
            /// 
            /// </summary>
            EA = 0xE0,

            /// <summary>
            /// 
            /// </summary>
            LOGGED_UTILITY_STREAM = 0x100,


            /// <summary>
            /// 
            /// </summary>
            ATTR_FLAG_COMPRESSED = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            ATTR_FLAG_ENCRYPTED = 0x4000,

            /// <summary>
            /// 
            /// </summary>
            ATTR_FLAG_SPARSE = 0x8000
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ATTR_TYPE Name;

        /// <summary>
        /// 
        /// </summary>
        public string NameString;

        internal bool NonResident;

        /// <summary>
        /// 
        /// </summary>
        public ushort AttributeId;

        internal int AttributeSize;

        #endregion Properties

        #region Static Methods

        internal static FileRecordAttribute[] GetInstances(byte[] bytes, int offset, int bytesPerFileRecord, string volume)
        {
            List<FileRecordAttribute> AttributeList = new List<FileRecordAttribute>();

            int i = offset;

            //while (i < offset + bytesPerFileRecord)
            while (i < offset + (bytesPerFileRecord - (offset % bytesPerFileRecord)))
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

                return new NonResident(nonresidentHeader, bytes, attributeoffset, attributeName, volume);

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
                        return null;

                    case (Int32)FileRecordAttribute.ATTR_TYPE.EA_INFORMATION:
                        return null;

                    default:
                        return null;
                }

                #endregion ATTRSwitch
            }
            #endregion ResidentAttribute
        }

        #endregion Static Methods
    }
}