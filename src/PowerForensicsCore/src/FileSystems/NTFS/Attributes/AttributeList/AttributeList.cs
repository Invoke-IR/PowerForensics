using System.Collections.Generic;

namespace PowerForensics.Ntfs
{
    #region AttributeListClass

    public class AttributeList : FileRecordAttribute
    {
        #region Properties

        public readonly AttrRef[] AttributeReference;

        #endregion Properties

        #region Constructors

        internal AttributeList(ResidentHeader header, byte[] bytes, int offset, string attrName)
        {
            Name = (FileRecordAttribute.ATTR_TYPE)header.commonHeader.ATTRType;
            NameString = attrName;
            NonResident = header.commonHeader.NonResident;
            AttributeId = header.commonHeader.Id;
            AttributeSize = header.commonHeader.TotalSize;

            #region AttributeReference

            int i = offset;
            List<AttrRef> refList = new List<AttrRef>();
            
            while (i < offset + header.AttrSize)
            {
                AttrRef attrRef = new AttrRef(bytes, i);
                refList.Add(attrRef);
                i += attrRef.RecordLength;
            }
            AttributeReference = refList.ToArray();

            #endregion AttributeReference
        }

        internal AttributeList(NonResident nonRes)
        {
            Name = (FileRecordAttribute.ATTR_TYPE)nonRes.Name;
            NameString = nonRes.NameString;
            NonResident = nonRes.NonResident;
            AttributeId = nonRes.AttributeId;
            AttributeSize = nonRes.AttributeSize;

            #region AttributeReference

            List<AttrRef> refList = new List<AttrRef>();

            byte[] bytes = nonRes.GetBytes();

            int i = 0;

            while (i < bytes.Length)
            {
                AttrRef attrRef = new AttrRef(bytes, i);
                refList.Add(attrRef);
                i += attrRef.RecordLength;
            }
            AttributeReference = refList.ToArray();

            #endregion AttributeReference
        }

        #endregion Constructors
    }

    #endregion AttributeListClass
}
