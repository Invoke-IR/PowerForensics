using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using PowerForensics;

namespace Tests
{
    [TestClass]
    public class BinShredTests
    {
        [TestMethod]
        public void ByteExtraction()
        {
            string template = "bitmap : data (13 bytes);";
            byte[] content = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            byte[] parsed = result["DATA"] as byte[];

            Assert.IsNotNull(parsed, "Should have BitMap key");
            Assert.AreEqual(13, parsed.Length);

            for (int counter = 0; counter < parsed.Length; counter++)
            {
                Assert.AreEqual(content[counter], parsed[counter], "Parsed data not equal at index {0}", counter);
            }
        }

        [TestMethod]
        public void TypedByteExtractionAscii()
        {
            string template = "pe : magic (2 bytes as ASCII);";
            byte[] content = { 0x4d, 0x5a };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            string parsed = result["magic"] as string;

            Assert.AreEqual("MZ", parsed, "Should have parsed as ASCII");
        }

        [TestMethod]
        public void TypedByteExtractionUnicode()
        {
            string template = "pe : magic (6 bytes as Unicode);";
            byte[] content = { 76, 0, 101, 0, 101, 0 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            string parsed = result["magic"] as string;

            Assert.AreEqual("Lee", parsed, "Should have parsed as Unicode");
        }

        [TestMethod]
        public void TypedByteExtractionUTF8()
        {
            string template = "pe : magic (8 bytes as UTF8);";
            byte[] content = { 76, 97, 200, 157, 97, 109, 111, 110 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            string parsed = result["magic"] as string;

            Assert.AreEqual("Laȝamon", parsed, "Should have parsed as Unicode");
        }

        [TestMethod]
        public void TypedByteExtractionUint16()
        {
            string template = "pe : magic (2 bytes as UINT16);";
            byte[] content = { 22, 219 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            UInt16 parsed = (UInt16)result["magic"];

            Assert.AreEqual((UInt16) 56086, parsed, "Should have parsed as UInt16");
        }

        [TestMethod]
        public void TypedByteExtractionInt16()
        {
            string template = "pe : magic (2 bytes as INT16);";
            byte[] content = { 108, 144 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            Int16 parsed = (Int16) result["magic"];

            Assert.AreEqual(-28564, parsed, "Should have parsed as Int16");
        }

        [TestMethod]
        public void TypedByteExtractionUint32()
        {
            string template = "pe : magic (4 bytes as UINT32);";
            byte[] content = { 22, 219, 104, 7 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            UInt32 parsed = (UInt32)result["magic"];

            Assert.AreEqual((UInt32)124312342, parsed, "Should have parsed as UInt32");
        }

        [TestMethod]
        public void TypedByteExtractionInt32()
        {
            string template = "pe : magic (4 bytes as INT32);";
            byte[] content = { 108, 144, 67, 255 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            int parsed = (int) result["magic"];

            Assert.AreEqual(-12349332, parsed, "Should have parsed as Int32");
        }

        [TestMethod]
        public void TypedByteExtractionUint64()
        {
            string template = "pe : magic (8 bytes as UINT64);";
            byte[] content = { 255, 219, 104, 7, 22, 219, 104, 255 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            UInt64 parsed = (UInt64)result["magic"];

            Assert.AreEqual(18404200764909607935, parsed, "Should have parsed as UInt64");
        }

        [TestMethod]
        public void TypedByteExtractionInt64()
        {
            string template = "pe : magic (8 bytes as INT64);";
            byte[] content = { 255, 144, 67, 255, 22, 219, 104, 255 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            Int64 parsed = (Int64) result["magic"];

            Assert.AreEqual(-42543304641638145, parsed, "Should have parsed as Int64");
        }

        [TestMethod]
        public void TypedByteExtractionSingle()
        {
            string template = "pe : magic (4 bytes as SINGLE);";
            byte[] content = { 195, 245, 72, 64 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            float parsed = (float) result["magic"];

            Assert.AreEqual( (float) 3.14000010490417, parsed, "Should have parsed as Single / Float");
        }

        [TestMethod]
        public void TypedByteExtractionFloat()
        {
            string template = "pe : magic (4 bytes as FLOAT);";
            byte[] content = { 195, 245, 72, 64 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            float parsed = (float)result["magic"];

            Assert.AreEqual((float)3.14000010490417, parsed, "Should have parsed as Single / Float");
        }

        [TestMethod]
        public void TypedByteExtractionDouble()
        {
            string template = "pe : magic (8 bytes as DOUBLE);";
            byte[] content = { 217, 10, 68, 84, 251, 33, 9, 64 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(1, result.Keys.Count, "Should have one key");
            double parsed = (double)result["magic"];

            Assert.AreEqual((double) 3.1415926535859, parsed, "Should have parsed as Single / Float");
        }

        [TestMethod]
        public void CommentedElement()
        {
            string template = @"
                pe :
                    /** The magic file header */
                    magic (2 bytes as ASCII)
                    ;
            ";
            byte[] content = { 0x4d, 0x5a };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(2, result.Keys.Count, "Should have one key");
            string parsed = result["magic"] as string;
            Assert.AreEqual("MZ", parsed, "Should have parsed as ASCII");

            string comment = result["magic.description"] as string;
            Assert.AreEqual("The magic file header", comment, "Should have comment");
        }

        [TestMethod]
        public void MultipleByteExtraction()
        {
            string template = @"
                bitmap :
                    data1 (2 bytes)
                    data2 (2 bytes as ascii)
                    ;
            ";
            byte[] content = { 0, 1, 65, 66, 4 };

            OrderedDictionary result = BinShred.Shred(content, template);

            Assert.AreEqual(2, result.Keys.Count, "Should have two keys");
            byte[] parsed1 = result["data1"] as byte[];
            string parsed2 = result["data2"] as string;

            Assert.IsNotNull(parsed1, "Should have data1 key");
            Assert.IsNotNull(parsed2, "Should have data2 key");

            Assert.AreEqual("AB", parsed2, "Should have parsed second string");
            Assert.AreEqual(2, parsed1.Length);

            for (int counter = 0; counter < parsed1.Length; counter++)
            {
                Assert.AreEqual(content[counter], parsed1[counter], "Parsed data not equal at index {0}", counter);
            }
        }

        [TestMethod]
        public void MemberNesting()
        {
            string template = @"
                pe : header;
                header : magic (2 bytes as ASCII);
            ";
            byte[] content = { 0x4d, 0x5a };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(1, result.Keys.Count, "Should have one key");

            OrderedDictionary header = result["header"] as OrderedDictionary;
            Assert.AreEqual(1, header.Keys.Count, "Should have one key");

            Assert.AreEqual("MZ", header["magic"] as String, "Should have parsed as ASCII");
        }

        [TestMethod]
        public void MemberNestingDoesntInterfere()
        {
            string template = @"
                pe : header other (2 bytes as ASCII);
                header : magic (2 bytes as ASCII);
            ";
            byte[] content = { 0x4d, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            OrderedDictionary header = result["header"] as OrderedDictionary;
            Assert.AreEqual(1, header.Keys.Count, "Should have one key");

            Assert.AreEqual("MZ", header["magic"] as String, "Should have parsed as ASCII");
            Assert.AreEqual("ba", result["other"] as String, "Should have header remainder bytes");
        }

        [TestMethod]
        public void MemberNestingDoesntInterfereDouble()
        {
            string template = @"
                pe :
                    header
                    moreheader;
                header : magic (2 bytes as ASCII);
                moreheader :
                    firstLetter (1 byte as ASCII)
                    secondLetter (1 byte as ASCII);
            ";
            byte[] content = { 0x4d, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            OrderedDictionary header = result["header"] as OrderedDictionary;
            Assert.AreEqual(1, header.Keys.Count, "Should have one key");
            Assert.AreEqual("MZ", header["magic"] as String, "Should have parsed as ASCII");

            OrderedDictionary moreheader = result["moreheader"] as OrderedDictionary;
            Assert.AreEqual(2, moreheader.Keys.Count, "Should have two keys");
            Assert.AreEqual("b", moreheader["firstLetter"] as String, "Should have parsed as ASCII");
            Assert.AreEqual("a", moreheader["secondLetter"] as String, "Should have parsed as ASCII");
        }

        [TestMethod]
        public void MemberDefinitionByteSizeMismatch()
        {
            string template = @"
                pe : item (1 byte as UINT32);";
            byte[] content = { 0x4d, 0x5a, 98, 97 };

            Exception caughtException = null;
            try
            {
                BinShred.Shred(content, template);
            }
            catch(ParseException e)
            {
                caughtException = e;
            }

            Assert.IsNotNull(caughtException);
        }

        [TestMethod]
        public void MemberDefinitionWithByteLookupElementNotFound()
        {
            string template = @"
                pe :
                    item (1 byte)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    0 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x5a, 98, 97 };

            ParseException exceptionThrown = null;
            try
            {
                BinShred.Shred(content, template);
            }
            catch(ParseException e)
            {
                exceptionThrown = e;
            }

            Assert.IsNotNull(exceptionThrown, "Should have thrown exception for missing lookup table element.");
        }

        [TestMethod]
        public void MemberDefinitionWithByteLookup()
        {
            string template = @"
                pe :
                    item (1 byte)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    77 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual(77, ((byte[]) result["item"])[0], "Should have parsed first as BYTE");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithByteArrayLookup()
        {
            string template = @"
                pe :
                    item (2 bytes)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    0x4d, 90 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x5a, 98, 97, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual(77, ((byte[])result["item"])[0], "Should have parsed first as bytes");
            Assert.AreEqual(90, ((byte[])result["item"])[1], "Should have parsed first as bytes");
            Assert.AreEqual("baa", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithUInt16Lookup()
        {
            string template = @"
                pe :
                    item (2 bytes as UINT16)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    77 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual(77, (UInt16) result["item"], "Should have parsed first as UInt16");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithInt16Lookup()
        {
            string template = @"
                pe :
                    item (2 bytes as Int16)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    77 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual(77, (Int16)result["item"], "Should have parsed first as Int16");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithUInt32Lookup()
        {
            string template = @"
                pe :
                    item (4 bytes as UInt32)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    77 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x00, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual((UInt32) 77, (UInt32)result["item"], "Should have parsed first as UInt32");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithInt32Lookup()
        {
            string template = @"
                pe :
                    item (4 bytes as Int32)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    77 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x00, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual(77, (Int32)result["item"], "Should have parsed first as Int32");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithUInt64Lookup()
        {
            string template = @"
                pe :
                    item (8 bytes as UInt64)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    77 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual((UInt64)77, (UInt64)result["item"], "Should have parsed first as UInt64");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithInt64Lookup()
        {
            string template = @"
                pe :
                    item (8 bytes as Int64)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    77 : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual((Int64)77, (Int64)result["item"], "Should have parsed first as Int64");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithAsciiLookup()
        {
            string template = @"
                pe :
                    item (1 byte as ASCII)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    M : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual("M", result["item"] as String, "Should have parsed first as string");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithUnicodeLookup()
        {
            string template = @"
                pe :
                    item (2 bytes as Unicode)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    M : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual("M", result["item"] as String, "Should have parsed first as string");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void MemberDefinitionWithHexLookup()
        {
            string template = @"
                pe :
                    item (8 bytes as Int64)
                    (additional properties identified by item from lookupTable);
                lookupTable :
                    0x4D : helper;
                helper :
                    rest (3 bytes as ASCII);";

            byte[] content = { 0x4d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5a, 98, 97 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual((Int64)77, (Int64)result["item"], "Should have parsed first as Int64");
            Assert.AreEqual("Zba", result["rest"] as String, "Should have picked up from lookup table");
        }

        [TestMethod]
        public void BytesFromExistingProperty()
        {
            string template = @"
                pe :
                    length (4 bytes as Int32)
                    string (length bytes as ASCII);";

            byte[] content = { 2, 0x00, 0x00, 0x00, 72, 101, 108, 108, 111 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have two root keys");

            Assert.AreEqual(2, (int) result["Length"], "Should have parsed length");
            Assert.AreEqual("He", result["string"] as String, "Should have parsed runtime-length string");
        }

        [TestMethod]
        public void BytesFromMissingProperty()
        {
            string template = @"
                pe :
                    length (4 bytes as Int32)
                    string (missing bytes as ASCII);";

            byte[] content = { 2, 0x00, 0x00, 0x00, 72, 101, 108, 108, 111 };

            Exception caughtException = null;
            try
            {
                BinShred.Shred(content, template);
            }
            catch (ParseException e)
            {
                caughtException = e;
            }

            Assert.IsNotNull(caughtException);
        }

        [TestMethod]
        public void MultipleElementsCounted()
        {
            string template = @"
                pe :
                    itemCount (4 bytes as Int32)
                    stuff (3 items);
                stuff :
                    value (2 bytes as ASCII);
            ";
            byte[] content = { 9, 0x00, 0x00, 0x00, 65, 66, 67, 68, 69, 70 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have one key");

            Assert.AreEqual(9, (int)result["itemCount"], "Should have correct count");

            object[] items = (object[])result["stuff"];
            Assert.AreEqual("AB", (string)((OrderedDictionary)items[0])["Value"], "Should have got first entry");
            Assert.AreEqual("CD", (string)((OrderedDictionary)items[1])["Value"], "Should have got second entry");
            Assert.AreEqual("EF", (string)((OrderedDictionary)items[2])["Value"], "Should have got third entry");
        }

        [TestMethod]
        public void MultipleElementsReference()
        {
            string template = @"
                pe :
                    itemCount (4 bytes as Int32)
                    stuff (itemCount items);
                stuff :
                    value (2 bytes as ASCII);
            ";
            byte[] content = { 3, 0x00, 0x00, 0x00, 65, 66, 67, 68, 69, 70 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(2, result.Keys.Count, "Should have one key");

            Assert.AreEqual(3, (int) result["itemCount"], "Should have correct count");

            object[] items = (object[]) result["stuff"];
            Assert.AreEqual("AB", (string)((OrderedDictionary) items[0])["Value"], "Should have got first entry");
            Assert.AreEqual("CD", (string)((OrderedDictionary) items[1])["Value"], "Should have got second entry");
            Assert.AreEqual("EF", (string)((OrderedDictionary) items[2])["Value"], "Should have got third entry");
        }

        [TestMethod]
        public void LabeledNotLocalItemReference()
        {
            string template = @"
                root :
                    pe;
                pe :
                    itemCount (4 bytes as Int32)
                    stuff (root.pe.itemCount items);
                stuff :
                    value (2 bytes as ASCII);
            ";
            byte[] content = { 3, 0x00, 0x00, 0x00, 65, 66, 67, 68, 69, 70 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(1, result.Keys.Count, "Should have one key");

            Assert.AreEqual(2, ((OrderedDictionary)result["pe"]).Keys.Count, "Should have correct count");
            Assert.AreEqual(3, (int)((OrderedDictionary) result["pe"])["itemCount"], "Should have correct count");

            object[] items = (object[])((OrderedDictionary)result["pe"])["stuff"];
            Assert.AreEqual("AB", (string)((OrderedDictionary)items[0])["Value"], "Should have got first entry");
            Assert.AreEqual("CD", (string)((OrderedDictionary)items[1])["Value"], "Should have got second entry");
            Assert.AreEqual("EF", (string)((OrderedDictionary)items[2])["Value"], "Should have got third entry");
        }

        [TestMethod]
        public void LabelsWithKeywords()
        {
            string template = @"
                pe :
                    bytes (4 bytes as Int32)
                    items (1 byte)
                    as (1 bytes);
            ";
            byte[] content = { 3, 0x00, 0x00, 0x00, 65, 66 };

            OrderedDictionary result = BinShred.Shred(content, template);
            Assert.AreEqual(3, result.Keys.Count, "Should have two keys");
        }

        // BUG - Bitmap compression method not incorporating description

        // TODO - Add 'Offset' field
        // TODO - Make SizeReference support hex
        // TODO - Nice error when reading off the end of the byte stream
        // TODO - Fix OOM when allocating item arrays
    }
}