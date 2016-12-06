using System;
using System.Text;

namespace PowerForensics.Windows.EventLog
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryXml
    {
        #region Enums
        
        /// <summary>
        /// 
        /// </summary>
        public enum TOKEN_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenEOF = 0x00,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenOpenStartElementTag = 0x01,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenCloseStartElementTag = 0x02,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenCloseEmptyElementTag = 0x03,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenEndElementTag = 0x04,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenValue = 0x05,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenAttribute = 0x06,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenCDATASection = 0x07,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenCharRef = 0x08,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenEntityRef = 0x09,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenPITarget = 0x0A,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenPIData = 0x0B,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenTemplateInstance = 0x0C,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenNormalSubstitution = 0x0D,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenOptionalSubstitution = 0x0E,

            /// <summary>
            /// 
            /// </summary>
            BinXmlFragmentHeaderToken = 0x0F,

            // Need to differentiate these

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenOpenStartElementTag_AttributeList = 0x41,
            //BinXmlTokenValue = 0x45,

            /// <summary>
            /// 
            /// </summary>
            BinXmlTokenAttribute_Additional = 0x46,
            //BinXmlTokenCDATASection = 0x47,
            //BinXmlTokenCharRef = 0x48,
            //BinXmlTokenEntityRef = 0x49
        }

        /// <summary>
        /// 
        /// </summary>
        public enum VALUE_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            NullType = 0x00,

            /// <summary>
            /// 
            /// </summary>
            StringType = 0x01,

            /// <summary>
            /// 
            /// </summary>
            AnsiStringType = 0x02,

            /// <summary>
            /// 
            /// </summary>
            Int8Type = 0x03,

            /// <summary>
            /// 
            /// </summary>
            UInt8Type = 0x04,

            /// <summary>
            /// 
            /// </summary>
            Int16Type = 0x05,

            /// <summary>
            /// 
            /// </summary>
            UInt16Type = 0x06,

            /// <summary>
            /// 
            /// </summary>
            Int32Type = 0x07,

            /// <summary>
            /// 
            /// </summary>
            UInt32Type = 0x08,

            /// <summary>
            /// 
            /// </summary>
            Int64Type = 0x09,

            /// <summary>
            /// 
            /// </summary>
            UInt64Type = 0x0A,

            /// <summary>
            /// 
            /// </summary>
            Real32Type = 0x0B,

            /// <summary>
            /// 
            /// </summary>
            Real64Type = 0x0C,

            /// <summary>
            /// 
            /// </summary>
            BoolType = 0x0D,

            /// <summary>
            /// 
            /// </summary>
            BinaryType = 0x0E,

            /// <summary>
            /// 
            /// </summary>
            GuidType = 0x0F,

            /// <summary>
            /// 
            /// </summary>
            SizeTType = 0x10,

            /// <summary>
            /// 
            /// </summary>
            FileTimeType = 0x11,

            /// <summary>
            /// 
            /// </summary>
            SysTimeType = 0x12,

            /// <summary>
            /// 
            /// </summary>
            SidType = 0x13,

            /// <summary>
            /// 
            /// </summary>
            HexInt32Type = 0x14,

            /// <summary>
            /// 
            /// </summary>
            HexInt64Type = 0x15,

            /// <summary>
            /// 
            /// </summary>
            EvtHandle = 0x20,

            /// <summary>
            /// 
            /// </summary>
            BinXmlType = 0x21,

            /// <summary>
            /// 
            /// </summary>
            EvtXml = 0x23,

            /// <summary>
            /// 
            /// </summary>
            StringType_Array = 0x81,

            /// <summary>
            /// 
            /// </summary>
            AnsiStringType_Array = 0x82,

            /// <summary>
            /// 
            /// </summary>
            Int8Type_Array = 0x83,

            /// <summary>
            /// 
            /// </summary>
            UInt8Type_Array = 0x84,

            /// <summary>
            /// 
            /// </summary>
            Int16Type_Array = 0x85,

            /// <summary>
            /// 
            /// </summary>
            UInt16Type_Array = 0x86,

            /// <summary>
            /// 
            /// </summary>
            Int32Type_Array = 0x87,

            /// <summary>
            /// 
            /// </summary>
            UInt32Type_Array = 0x88,

            /// <summary>
            /// 
            /// </summary>
            Int64Type_Array = 0x89,

            /// <summary>
            /// 
            /// </summary>
            UInt64Type_Array = 0x8A,

            /// <summary>
            /// 
            /// </summary>
            Real32Type_Array = 0x8B,

            /// <summary>
            /// 
            /// </summary>
            Real64Type_Array = 0x8C,

            /// <summary>
            /// 
            /// </summary>
            BoolType_Array = 0x8D,

            /// <summary>
            /// 
            /// </summary>
            BinaryType_Array = 0x8E,

            /// <summary>
            /// 
            /// </summary>
            GuidType_Array = 0x8F,

            /// <summary>
            /// 
            /// </summary>
            SizeTType_Array = 0x90,

            /// <summary>
            /// 
            /// </summary>
            FileTimeType_Array = 0x91,

            /// <summary>
            /// 
            /// </summary>
            SysTimeType_Array = 0x92,

            /// <summary>
            /// 
            /// </summary>
            SidType_Array = 0x93,

            /// <summary>
            /// 
            /// </summary>
            HexInt32Type_Array = 0x94,

            /// <summary>
            /// 
            /// </summary>
            HexInt64Type_Array = 0x95
        }

        #endregion Enums

        #region Properties

        internal readonly BinXmlTemplateInstance TemplateInstance;

        /// <summary>
        /// 
        /// </summary>
        public readonly object ProviderName;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Guid;

        /// <summary>
        /// 
        /// </summary>
        public readonly object EventId;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Verson;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Level;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Task;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Opcode;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Keywords;

        /// <summary>
        /// 
        /// </summary>
        public readonly object TimeCreated;

        /// <summary>
        /// 
        /// </summary>
        public readonly object EventRecordId;

        /// <summary>
        /// 
        /// </summary>
        public readonly object ActivityId;

        /// <summary>
        /// 
        /// </summary>
        public readonly object ProcessId;

        /// <summary>
        /// 
        /// </summary>
        public readonly object ThreadId;

        /// <summary>
        /// 
        /// </summary>
        public readonly object Channel;

        /// <summary>
        /// 
        /// </summary>
        public readonly object UserId;

        #endregion Properties

        #region Constructors

        internal BinaryXml(byte[] bytes, int chunkOffset, int offset, int size)
        {
            TemplateInstance = new BinXmlTemplateInstance(bytes, chunkOffset, offset + 0x04);
            ProviderName = TemplateInstance.TemplateInstanceData.ValueArray[14];
            Guid = TemplateInstance.TemplateInstanceData.ValueArray[15];
            EventId = TemplateInstance.TemplateInstanceData.ValueArray[3];
            Verson = TemplateInstance.TemplateInstanceData.ValueArray[11];
            Level = TemplateInstance.TemplateInstanceData.ValueArray[0];
            Task = TemplateInstance.TemplateInstanceData.ValueArray[2];
            Opcode = TemplateInstance.TemplateInstanceData.ValueArray[1];
            Keywords = TemplateInstance.TemplateInstanceData.ValueArray[4];
            TimeCreated = TemplateInstance.TemplateInstanceData.ValueArray[6];
            EventRecordId = TemplateInstance.TemplateInstanceData.ValueArray[10];
            ActivityId = TemplateInstance.TemplateInstanceData.ValueArray[7];
            ProcessId = TemplateInstance.TemplateInstanceData.ValueArray[8];
            ThreadId = TemplateInstance.TemplateInstanceData.ValueArray[9];
            Channel = TemplateInstance.TemplateInstanceData.ValueArray[16];
            UserId = TemplateInstance.TemplateInstanceData.ValueArray[12];
        }

        #endregion Constructors

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("EventId: {0}", EventId);
        }

        #endregion Override Methods
    }

    class BinXmlTemplateInstance
    {
        #region Properties

        internal readonly BinaryXml.TOKEN_TYPE Token;
        internal readonly BinXmlTemplateDefinition TemplateDefinition;
        internal readonly BinXmlTemplateInstanceData TemplateInstanceData;

        #endregion Properties

        #region Constructors

        internal BinXmlTemplateInstance(byte[] bytes, int chunkOffset, int offset)
        {
            Token = (BinaryXml.TOKEN_TYPE)bytes[offset];
            TemplateDefinition = new BinXmlTemplateDefinition(bytes, chunkOffset, offset + 0x01);

            if (TemplateDefinition.DataOffset + chunkOffset > offset)
            {
                TemplateInstanceData = new BinXmlTemplateInstanceData(bytes, chunkOffset, offset + 0x01 + TemplateDefinition.DataSize + 0x21);
            }
            else
            {
                TemplateInstanceData = new BinXmlTemplateInstanceData(bytes, chunkOffset, offset + 0x0A);
            }
                //TemplateInstanceData = new BinXmlTemplateInstanceData(bytes, chunkOffset, offset + 0x01 + TemplateDefinition.DataSize + 0x21);
            
                // Figure out what to do here...
                //TemplateInstanceData = new BinXmlTemplateInstanceData(bytes, chunkOffset, offset + 0x0A);
        }

        #endregion Constructors
    }

    class BinXmlTemplateDefinition
    {
        #region Properties

        internal readonly int DataOffset;
        internal readonly Guid TemplateId;
        internal readonly int DataSize;
        internal readonly BinaryXml.TOKEN_TYPE EOFToken;

        #endregion Properties

        #region Constructors

        internal BinXmlTemplateDefinition(byte[] bytes, int chunkOffset, int offset)
        {
            DataOffset = BitConverter.ToInt32(bytes, offset + 0x05);

            int dataoffset = DataOffset + chunkOffset;

            TemplateId = new Guid(Helper.GetSubArray(bytes, dataoffset + 0x04, 0x10));
            DataSize = BitConverter.ToInt32(bytes, dataoffset + 0x14);

            /*List<BinXmlElement> elementList = new List<BinXmlElement>();

            while (elementoffset < endoffset)
            {
                Console.WriteLine(elementoffset);
                if ((BinaryXml.TOKEN_TYPE)bytes[offset + 0x01] == BinaryXml.TOKEN_TYPE.BinXmlTokenOpenStartElementTag)
                {
                    BinXmlElementNoAttribute element = new BinXmlElementNoAttribute(bytes, chunkOffset, ref elementoffset);
                    elementList.Add(element);
                }
                else
                {
                    BinXmlElementAttribute elementattr = new BinXmlElementAttribute(bytes, chunkOffset, ref elementoffset);
                    elementList.Add(elementattr);
                }
            }

            Elements = elementList.ToArray();*/


            EOFToken = (BinaryXml.TOKEN_TYPE)bytes[dataoffset + DataSize + 0x18 - 0x01];
        }
        
        #endregion Constructors
    }

    class BinXmlTemplateInstanceData
    {
        #region Properties

        internal readonly int ValueCount;
        internal readonly BinXmlTemplateValueDescriptor[] ValueTypeArray;
        internal readonly Object[] ValueArray;

        #endregion Properties

        #region Constructors

        internal BinXmlTemplateInstanceData(byte[] bytes, int chunkOffset, int offset)
        {
            ValueCount = BitConverter.ToInt32(bytes, offset);
            
            // Remove this if eventually
            if (ValueCount != 1376264)
            {
                ValueTypeArray = new BinXmlTemplateValueDescriptor[ValueCount];
                ValueArray = new Object[ValueCount];

                offset += 0x04;
                for (int i = 0; i < ValueCount; i++)
                {
                    ValueTypeArray[i] = new BinXmlTemplateValueDescriptor(bytes, offset);
                    offset += 0x04;
                }

                for (int i = 0; i < ValueCount; i++)
                {
                    switch(ValueTypeArray[i].ValueType)
                    {
                        case BinaryXml.VALUE_TYPE.NullType:
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.StringType:
                            ValueArray[i] = Encoding.Unicode.GetString(bytes, offset, ValueTypeArray[i].ValueSize);
                            break;
                        case BinaryXml.VALUE_TYPE.AnsiStringType:
                            ValueArray[i] = Encoding.ASCII.GetString(bytes, offset, ValueTypeArray[i].ValueSize);
                            break;
                        case BinaryXml.VALUE_TYPE.Int8Type:
                            ValueArray[i] = bytes[offset];
                            break;
                        case BinaryXml.VALUE_TYPE.UInt8Type:
                            ValueArray[i] = bytes[offset];
                            break;
                        case BinaryXml.VALUE_TYPE.Int16Type:
                            ValueArray[i] = BitConverter.ToInt16(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.UInt16Type:
                            ValueArray[i] = BitConverter.ToUInt16(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.Int32Type:
                            ValueArray[i] = BitConverter.ToInt32(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.UInt32Type:
                            ValueArray[i] = BitConverter.ToUInt32(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.Int64Type:
                            ValueArray[i] = BitConverter.ToInt64(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.UInt64Type:
                            ValueArray[i] = BitConverter.ToUInt64(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.Real32Type:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.Real64Type:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.BoolType:
                            int value = BitConverter.ToInt32(bytes, offset);
                            if (value > 0x00)
                            {
                                ValueArray[i] = true;
                            }
                            else
                            {
                                ValueArray[i] = false;
                            }
                            break;
                        case BinaryXml.VALUE_TYPE.BinaryType:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.GuidType:
                            ValueArray[i] = new Guid(Helper.GetSubArray(bytes, offset, 0x10));
                            break;
                        case BinaryXml.VALUE_TYPE.SizeTType:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.FileTimeType:
                            ValueArray[i] = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, offset));
                            break;
                        case BinaryXml.VALUE_TYPE.SysTimeType:
                            ValueArray[i] = Systemtime.Get(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.SidType:
                            //ValueArray[i] = new SecurityIdentifier(bytes, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.HexInt32Type:
                            ValueArray[i] = String.Format("0x{0:X}", BitConverter.ToInt32(bytes, offset));
                            break;
                        case BinaryXml.VALUE_TYPE.HexInt64Type:
                            ValueArray[i] = String.Format("0x{0:X}", BitConverter.ToInt64(bytes, offset));;
                            break;
                        case BinaryXml.VALUE_TYPE.BinXmlType:
                            //Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            //ValueArray[i] = NativeMethods.GetSubArray(bytes, (uint)offset, (uint)ValueTypeArray[i].ValueSize);
                            //ValueArray[i] = new BinXmlTemplateInstance(bytes, chunkOffset, offset);
                            break;
                        case BinaryXml.VALUE_TYPE.StringType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.AnsiStringType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.Int8Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.UInt8Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.Int16Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.UInt16Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.Int32Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.UInt32Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.Int64Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.UInt64Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.Real32Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.Real64Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.BoolType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.BinaryType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.GuidType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.SizeTType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.FileTimeType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.SysTimeType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.SidType_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.HexInt32Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                        case BinaryXml.VALUE_TYPE.HexInt64Type_Array:
                            Console.WriteLine("Type: {0}, Size: {1}", ValueTypeArray[i].ValueType, ValueTypeArray[i].ValueSize);
                            ValueArray[i] = null;
                            break;
                    }
                    offset += ValueTypeArray[i].ValueSize;
                }
            }
        }

        #endregion Constructors
    }

    class BinXmlTemplateValueDescriptor
    {
        #region Properties

        internal readonly short ValueSize;
        internal readonly BinaryXml.VALUE_TYPE ValueType;
        
        #endregion Properties

        #region Constructors

        internal BinXmlTemplateValueDescriptor(byte[] bytes, int offset)
        {
            ValueSize = BitConverter.ToInt16(bytes, offset);
            ValueType = (BinaryXml.VALUE_TYPE)bytes[offset + 0x02];
        }

        #endregion Constructors
    }

    // Not Done
    /// <summary>
    /// 
    /// </summary>
    public class BinXmlAttributeList
    {
        #region Properties

        internal readonly uint DataSize;
        internal readonly BinXmlAttribute AttributeArray;

        #endregion Properties

        #region Constructors

        internal BinXmlAttributeList(byte[] bytes, int chunkOffset, int offset)
        {
            DataSize = BitConverter.ToUInt32(bytes, offset);
            AttributeArray = new BinXmlAttribute(bytes, chunkOffset, offset);

            /*List<BinXmlAttribute> attributeList = new List<BinXmlAttribute>();
            
            BinXmlAttribute attribute = null;

            do
            {
                attribute = new BinXmlAttribute(bytes, chunkOffset, ref offset);
            } while (attribute.Token == BinaryXml.TOKEN_TYPE.BinXmlTokenAttribute_Additional);
            
            AttributeArray = attributeList.ToArray();*/
        }

        #endregion Constructors
    }

    // Not Done
    /// <summary>
    /// 
    /// </summary>
    public class BinXmlAttribute
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly BinaryXml.TOKEN_TYPE Token;

        /// <summary>
        /// 
        /// </summary>
        public readonly int AttributeNameOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly BinXmlName Name;

        #endregion Properties

        #region Constructors

        internal BinXmlAttribute(byte[] bytes, int chunkOffset, int offset)
        {
            Token = (BinaryXml.TOKEN_TYPE)bytes[offset];
            AttributeNameOffset = BitConverter.ToInt32(bytes, offset + 0x01);
            Name = new BinXmlName(bytes, chunkOffset + AttributeNameOffset);
        }

        #endregion Constructors
    }

    /// <summary>
    /// 
    /// </summary>
    public class BinXmlName
    {
        #region Constants

        internal const int HeaderSize = 0x08;

        #endregion Constants

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly short NameSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;
        
        #endregion Properties

        #region Constructors

        internal BinXmlName(byte[] bytes, int offset)
        {
            NameSize = BitConverter.ToInt16(bytes, offset + 0x06);
            Name = Encoding.Unicode.GetString(bytes, offset + 0x08, NameSize * 2);
        }

        #endregion Constructors

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion Override Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class BinXmlValueText
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly BinaryXml.TOKEN_TYPE ValueToken;

        /// <summary>
        /// 
        /// </summary>
        public readonly BinaryXml.VALUE_TYPE ValueType;

        /// <summary>
        /// 
        /// </summary>
        public readonly string ValueData;

        #endregion Properties

        #region Constructors

        internal BinXmlValueText(byte[] bytes, int offset)
        {
            ValueToken = (BinaryXml.TOKEN_TYPE)bytes[offset];
            ValueType = (BinaryXml.VALUE_TYPE)bytes[offset + 0x01];
            ValueData = Encoding.Unicode.GetString(bytes, offset + 0x04, BitConverter.ToInt16(bytes, offset + 0x02));
        }

        #endregion Constructors
    }

    // Not Done
    class BinXmlCharacterEntityReference
    {
        #region Properties

        internal readonly BinaryXml.TOKEN_TYPE Token;
        internal readonly byte[] Value;

        #endregion Properties

        #region Constructors

        internal BinXmlCharacterEntityReference(byte[] bytes, int offset)
        {
            Token = (BinaryXml.TOKEN_TYPE)bytes[offset];
            Value = Helper.GetSubArray(bytes, offset + 0x01, 0x02);
        }

        #endregion Constructors
    }

    // Not Done
    class BinXmlEntityReference
    {
        #region Properties

        internal readonly BinaryXml.TOKEN_TYPE Token;
        internal readonly uint NameOffset;

        #endregion Properties

        #region Constructors

        internal BinXmlEntityReference(byte[] bytes, int offset)
        {
            Token = (BinaryXml.TOKEN_TYPE)bytes[offset];
            NameOffset = BitConverter.ToUInt32(bytes, offset + 0x01);
        }

        #endregion Constructors
    }

    internal class Systemtime
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static DateTime Get(byte[] bytes, int offset)
        {
            ushort Year = BitConverter.ToUInt16(bytes, offset);
            ushort Month = BitConverter.ToUInt16(bytes, offset + 0x02);
            ushort DayOfWeek = BitConverter.ToUInt16(bytes, offset + 0x04);
            ushort Day = BitConverter.ToUInt16(bytes, offset + 0x06);
            ushort Hour = BitConverter.ToUInt16(bytes, offset + 0x08);
            ushort Minute = BitConverter.ToUInt16(bytes, offset + 0x0A);
            ushort Second = BitConverter.ToUInt16(bytes, offset + 0x0C);
            ushort Millisecond = BitConverter.ToUInt16(bytes, offset + 0x0E);

            return new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
        }
    }
}
