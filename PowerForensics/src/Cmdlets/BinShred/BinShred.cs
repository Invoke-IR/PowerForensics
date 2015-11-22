using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using static BinShredParser;
using System.Runtime.InteropServices;

namespace PowerForensics
{
    public class BinShred
    {
        public static OrderedDictionary Shred(byte[] content, string template)
        {
            Stream inputStream = new MemoryStream(Encoding.ASCII.GetBytes(template));
            AntlrInputStream input = new AntlrInputStream(inputStream);
            BinShredLexer lexer = new BinShredLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new BinShredErrorListener<int>());

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BinShredParser parser = new BinShredParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new BinShredErrorListener<IToken>());

            BinShredParser.TemplateContext context = parser.template();

            ParseTreeWalker walker = new ParseTreeWalker();
            BinShredAnalyzer contentAnalyzer = new BinShredAnalyzer(content);

            walker.Walk(contentAnalyzer, context);
            contentAnalyzer.Run();

            return contentAnalyzer.Result;
        }
    }

    class BinShredErrorListener<T> : IAntlrErrorListener<T>
    {
        public void SyntaxError(IRecognizer recognizer, T offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            string message = String.Format("Could not parse template. There was a syntax error at line {0}, column {1}: {2}", line, charPositionInLine, msg);
            throw new ParseException(message);
        }
    }

    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        { }
    }

    class BinShredAnalyzer : BinShredBaseListener
    {
        public BinShredAnalyzer(byte[] content)
        {
            this.content = content;
            this.contentPosition = 0;
            parseActions = new Dictionary<string, List<ParseAction>>(StringComparer.OrdinalIgnoreCase);
            processingActions = new Stack<string>();

            lookupTables = new Dictionary<string, Dictionary<Object, string>>(StringComparer.OrdinalIgnoreCase);

            results = new Stack<OrderedDictionary>();
            OrderedDictionary currentResult = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
            results.Push(currentResult);
        }

        byte[] content;
        int contentPosition;
        string startAction;
        Stack<string> processingActions;
        Stack<OrderedDictionary> results;

        delegate int ParseAction(byte[] content, int contentPosition);
        Dictionary<string, List<ParseAction>> parseActions;

        Dictionary<string, Dictionary<Object, string>> lookupTables;
        string currentLookupTable = null;

        public OrderedDictionary Result
        {
            get
            {
                return results.Peek();
            }
        }

        public void Run()
        {
            Run(startAction);
        }

        int Run(string definition)
        {
            int startingContentPosition = this.contentPosition;

            List<ParseAction> definitionActions = null;
            if (parseActions.TryGetValue(definition, out definitionActions))
            {
                foreach (ParseAction action in definitionActions)
                {
                    int consumed = action.Invoke(this.content, this.contentPosition);
                    this.contentPosition += consumed;
                }
            }
            else
            {
                throw new ParseException(
                    String.Format("Could not process with definition '{0}'. No definition with this name exists.", definition));
            }

            return this.contentPosition - startingContentPosition;
        }

        public override void EnterParseRule([NotNull] BinShredParser.ParseRuleContext context)
        {
            string regionName = context.label().GetText();
            parseActions[regionName] = new List<ParseAction>();
            processingActions.Push(regionName);

            if (String.IsNullOrEmpty(startAction))
            {
                startAction = regionName;
            }

            foreach (RuleBodyContext body in context.ruleBody())
            {
                // This is a rule that describes additional actions to be taken from something
                // already parsed
                // "(additional properties identified by propertyName from lookupTableName)"
                if (body.ADDITIONAL() != null)
                {
                    string propertyName = body.propertyName().GetText();
                    string lookupTableName = body.lookupTableName().GetText();

                    ProcessAdditionalProperties(propertyName, lookupTableName, regionName);
                    continue;
                }

                // Get the rule's label and descriptive comment (/** */)
                string ruleLabel = body.label().GetText();
                string ruleComment = null;

                ITerminalNode docComment = body.DOC_COMMENT();
                if (docComment != null)
                {
                    ruleComment = docComment.GetText();
                    ruleComment = ruleComment.Substring(3, ruleComment.Length - 5).Trim();
                }

                // Get the rule's options
                RuleOptionsContext options = body.ruleOptions();
                if (options != null)
                {
                    // This is a rule with actual options (i.e.: "(4 bytes as int32)")
                    ByteOptionContext byteOptions = options.byteOption();

                    int bytes = 0;
                    String byteLabel = String.Empty;

                    if (byteOptions.sizeReference().INT() != null)
                    {
                        bytes = Int32.Parse(byteOptions.sizeReference().INT().GetText());
                    }
                    else
                    {
                        byteLabel = byteOptions.sizeReference().label().GetText();
                    }

                    // This is a rule that extracts bytes in some format
                    ProcessByteExtraction(regionName, ruleLabel, ruleComment, byteOptions, bytes, byteLabel);
                }
                else
                {
                    // This is a rule with a reference to another rule
                    if (body.ITEMS() != null)
                    {
                        // This is a rule which creates several records of a given record type
                        // (i.e.: (12 items) or (byteCount items)

                        int items = 0;
                        string itemLabel = String.Empty;

                        if (body.sizeReference().INT() != null)
                        {
                            items = Int32.Parse(body.sizeReference().INT().GetText());
                        }
                        else
                        {
                            itemLabel = body.sizeReference().label().GetText();
                        }

                        ProcessCountedRule(regionName, ruleLabel, ruleComment, items, itemLabel);
                    }
                    else
                    {
                        this.parseActions[regionName].Add(
                        new ParseAction((content, contentPosition) =>
                        {
                            OrderedDictionary currentResult = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
                            results.Peek().Add(ruleLabel, currentResult);

                            RunRule(ruleComment, ruleLabel, currentResult);
                            return 0;
                        }
                        ));
                    }
                }
            }
        }

        private void ProcessByteExtraction(string regionName, string ruleLabel, string ruleComment, ByteOptionContext byteOptions, int bytes, string byteLabel)
        {
            // See if it has a "described by" annotation, which we'll use
            // as a lookup table to augment the comment.
            string describedBy = null;
            if (byteOptions.label() != null)
            {
                describedBy = byteOptions.label().GetText();
            }

            // Process the bytes based on the byte format (i.e.: "as ASCII")
            ByteFormatContext byteFormat = byteOptions.byteFormat();
            if (byteFormat != null)
            {
                ProcessByByteFormat(byteFormat, (int)bytes, byteLabel, regionName, ruleLabel, describedBy, ruleComment);
            }
            else
            {
                // This has no byte format. It is just bytes.
                this.parseActions[regionName].Add(
                    new ParseAction((content, contentPosition) =>
                    {
                        if (!String.IsNullOrEmpty(byteLabel))
                        {
                            bytes = GetByteCountFromLabel(byteLabel);
                        }

                        byte[] element = new byte[bytes];
                        Array.Copy(content, (int)contentPosition, element, 0, bytes);

                        ParseElement(ruleLabel, element, describedBy, ruleComment);
                        return (int)bytes;
                    }
                    ));
            }
        }

        private void ProcessCountedRule(string regionName, string ruleLabel, string ruleComment, int bytes, string byteLabel)
        {
            // This is a rule with counted elements of another rule type
            this.parseActions[regionName].Add(
                new ParseAction((content, contentPosition) =>
                {
                    int items = bytes;

                    if (!String.IsNullOrEmpty(byteLabel))
                    {
                        items = GetByteCountFromLabel(byteLabel);
                    }

                    object[] ruleItems = new object[items];
                    results.Peek().Add(ruleLabel, ruleItems);

                    for (int itemNumber = 0; itemNumber < items; itemNumber++)
                    {
                        OrderedDictionary currentResult = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
                        ruleItems[itemNumber] = currentResult;
                        RunRule(ruleComment, ruleLabel, currentResult);
                    }
                    
                    return 0;
                }
                ));
        }

        static Type[] FromStringConversions = new Type[]
        {
            typeof(Byte), typeof(Byte[]), typeof(UInt16), typeof(Int16), typeof(UInt32), typeof(Int32),
            typeof(UInt64), typeof(Int64), typeof(float), typeof(double), typeof(String)
        };

        // Do direct processing of further properties based on the value of an
        // existing property
        void ProcessAdditionalProperties(string propertyName, string lookupTableName, string regionName)
        {
            this.parseActions[regionName].Add(
                new ParseAction((content, contentPosition) =>
                {
                    Dictionary<object, string> lookupTable = null;
                    if (lookupTables.TryGetValue(lookupTableName, out lookupTable))
                    {
                        Object lookupTableKey = results.Peek()[propertyName];
                        string processingAction = null;

                        byte[] lookupTableKeyAsArray = lookupTableKey as byte[];
                        if(lookupTableKeyAsArray != null)
                        {
                            lookupTableKey = String.Join(",", lookupTableKeyAsArray);
                        }

                        if (lookupTable.TryGetValue(lookupTableKey.ToString(), out processingAction))
                        {
                            Run(processingAction);

                            // TODO - Write unit test to demonstrate this bug
                            // return Run(processingAction);
                            return 0;
                        }

                        // Could not find the element in a lookup table.
                        throw new ParseException(
                            String.Format("Could not process additional properties from definition '{0}'. " +
                                "The value '{1}' does not exist in '{2}'.", regionName, lookupTableKey, lookupTableName));
                    }
                    else
                    {
                        throw new ParseException(
                            String.Format("Could not process additional properties from definition '{0}'. " +
                                "No definition with this name exists.", lookupTableName));
                    }
                }
                ));
        }

        // Store an element and its comments
        void ParseElement(string ruleLabel, object element, string describedBy, string ruleComment)
        {
            // If this element is "described by" a lookup table, fetch
            // that description.
            if (!String.IsNullOrEmpty(describedBy))
            {
                Dictionary<object, string> lookupTable = lookupTables[describedBy];

                string additionalComment = null;
                lookupTable.TryGetValue(element, out additionalComment);

                // If we found a lookup entry, add it to the comment (or use it
                // as the comment if there wasn't one already).
                if (!String.IsNullOrEmpty(additionalComment))
                {
                    if (String.IsNullOrEmpty(ruleComment))
                    {
                        ruleComment = additionalComment;
                    }
                    else
                    {
                        ruleComment += ": " + additionalComment;
                    }
                }
            }

            if (!String.IsNullOrEmpty(ruleComment))
            {
                results.Peek().Add(ruleLabel + ".description", ruleComment);
            }

            results.Peek().Add(ruleLabel, element);
        }

        OrderedDictionary RunRule(string ruleComment, string ruleLabel, OrderedDictionary currentResult)
        {
            if (!String.IsNullOrEmpty(ruleComment))
            {
                results.Peek().Add(ruleLabel + ".description", ruleComment);
            }

            results.Push(currentResult);
            Run(ruleLabel);
            results.Pop();
            
            return currentResult;
        }

        static Dictionary<String, Tuple<Type, Func<byte[], int, int, Object>>> ByteParsers =
            new Dictionary<String, Tuple<Type, Func<byte[], int, int, Object>>>(StringComparer.OrdinalIgnoreCase) {
                { "ASCII", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(String), (content, contentPosition, bytes) => Encoding.ASCII.GetString(content, contentPosition, bytes)) },
                { "Unicode", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(String), (content, contentPosition, bytes) => Encoding.Unicode.GetString(content, contentPosition, bytes)) },
                { "UTF8", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(String), (content, contentPosition, bytes) => Encoding.UTF8.GetString(content, contentPosition, bytes)) },
                { "UInt64", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(UInt64), (content, contentPosition, bytes) => BitConverter.ToUInt64(content, contentPosition)) },
                { "Int64", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(Int64), (content, contentPosition, bytes) => BitConverter.ToInt64(content, contentPosition)) },
                { "UInt32", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(UInt32), (content, contentPosition, bytes) => BitConverter.ToUInt32(content, contentPosition)) },
                { "Int32", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(Int32), (content, contentPosition, bytes) => BitConverter.ToInt32(content, contentPosition)) },
                { "UInt16", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(UInt16), (content, contentPosition, bytes) => BitConverter.ToUInt16(content, contentPosition)) },
                { "Int16", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(Int16), (content, contentPosition, bytes) => BitConverter.ToInt16(content, contentPosition)) },
                { "Single", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(Single), (content, contentPosition, bytes) => BitConverter.ToSingle(content, contentPosition)) },
                { "Float", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(Single), (content, contentPosition, bytes) => BitConverter.ToSingle(content, contentPosition)) },
                { "Double", new Tuple<Type, Func<byte[], int, int, object>>(
                    typeof(Double), (content, contentPosition, bytes) => BitConverter.ToDouble(content, contentPosition)) },
            };

        // Extract content from the document at the current position, converting it to
        // the appropriate content type if requested.
        void ProcessByByteFormat(
            ByteFormatContext byteFormat, int bytes, string byteLabel,
            string regionName, string ruleLabel, string describedBy, string ruleComment)
        {
            this.parseActions[regionName].Add(
                new ParseAction((content, contentPosition) =>
                {
                    if (!String.IsNullOrEmpty(byteLabel))
                    {
                        bytes = GetByteCountFromLabel(byteLabel);
                    }

                    ValidateRuleByteSize(ruleLabel, bytes, ByteParsers[byteFormat.GetText()].Item1);

                    object element = ByteParsers[byteFormat.GetText()].Item2(content, (int)contentPosition, (int)bytes);
                    ParseElement(ruleLabel, element, describedBy, ruleComment);

                    return (int)bytes;
                }
                ));
        }

        void ValidateRuleByteSize(string ruleLabel, int bytes, Type targetType)
        {
            if(targetType == typeof(String))
            {
                return;
            }

            if (bytes != Marshal.SizeOf(targetType))
            {
                throw new ParseException(
                    String.Format("Could not parse definition for '{0}'. A {1} property must be {2} bytes.",
                        ruleLabel, targetType.Name, Marshal.SizeOf(targetType)));
            }
        }

        int GetByteCountFromLabel(string byteLabel)
        {
            OrderedDictionary labelData;

            if (byteLabel.IndexOf('.') == -1)
            {
                labelData = results.Peek();

                if (!labelData.Contains(byteLabel))
                {
                    throw new ParseException(
                        String.Format("Could not parse byte length. The '{0}' property has not been parsed yet.", byteLabel));
                }
            }
            else
            {
                OrderedDictionary[] parseHierarcy = results.ToArray();
                labelData = parseHierarcy[parseHierarcy.Length - 1];
                string labelSoFar = String.Empty;

                string[] labelHierarchy = byteLabel.Split('.');
                labelSoFar = labelHierarchy[0];

                for(int labelItem = 1; labelItem < labelHierarchy.Length - 1; labelItem++)
                {
                    labelSoFar += "." + labelHierarchy[labelItem];

                    if (!labelData.Contains(labelHierarchy[labelItem]))
                    {
                        throw new ParseException(
                            String.Format("Could not parse byte length. The '{0}' property has not been parsed yet.", labelSoFar));
                    }

                    labelData = labelData[labelHierarchy[labelItem]] as OrderedDictionary;
                    if(labelData == null)
                    {
                        throw new ParseException(
                            String.Format("Could not parse byte length. '{0}' does not have any child properties.", labelSoFar));
                    }
                }

                byteLabel = labelHierarchy[labelHierarchy.Length - 1];
            }


            if(! labelData.Contains(byteLabel))
            {
                throw new ParseException(
                    String.Format("Could not parse byte length. The '{0}' property has not been parsed yet.", byteLabel));
            }

            try
            {
                string byteLabelValue = labelData[byteLabel].ToString();
                int result = Int32.Parse(byteLabelValue);

                if (result >= 0) { return result; }
                else { return 0; }
            }
            catch(Exception e)
            {
                if(e is FormatException || e is OverflowException)
                {
                    throw new ParseException(
                        String.Format("Could not parse byte length. The length '{0}' does not represent a number.", byteLabel));
                }
                else
                {
                    throw;
                }
            }
        }

        public override void ExitParseRule([NotNull] BinShredParser.ParseRuleContext context)
        {
            processingActions.Pop();
        }

        public override void EnterLookupTable([NotNull] LookupTableContext context)
        {
            currentLookupTable = context.lookupTableName().GetText();
            lookupTables[currentLookupTable] = new Dictionary<object, string>();
            return;
        }

        public override void ExitLookupTable([NotNull] LookupTableContext context)
        {
            currentLookupTable = null;
            return;
        }

        public override void EnterLookupTableEntry([NotNull] LookupTableEntryContext context)
        {
            string label = String.Empty;

            foreach(LookupTableEntryKeyContext lookupTableEntryKey in context.lookupTableEntryKey())
            {
                string labelElement = lookupTableEntryKey.GetText();
                if(lookupTableEntryKey.HEXADECIMAL() != null)
                {
                    // Convert the constant to its integral value
                    labelElement = Convert.ToUInt64(labelElement, 16).ToString();
                }

                if(! String.IsNullOrEmpty(label))
                {
                    label += ",";
                }

                label += labelElement;
            }

            string value = null;

            if (context.QUOTEDVALUE() != null)
            {
                value = context.QUOTEDVALUE().GetText();
                value = value.Substring(1, value.Length - 2).Trim();
            }
            else
            {
                value = context.label().GetText();
            }

            lookupTables[currentLookupTable][label] = value;

            return;
        }
    }
}