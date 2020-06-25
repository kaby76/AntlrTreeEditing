using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using java.net;
using java.util;
using javax.xml.datatype;
using javax.xml.@namespace;
using org.eclipse.wst.xml.xpath2.api;
using org.eclipse.wst.xml.xpath2.processor.@internal.utils;
using org.w3c.dom;
using DynamicContext = org.eclipse.wst.xml.xpath2.api.DynamicContext;

namespace AntlrDOM
{
    public class ConvertToDOM
    {
        public static AntlrDynamicContext Try(IParseTree tree, Parser parser)
        {
            // Perform bottom up traversal to derive equivalent tree in "dom".
            var converted_tree = BottomUpConvert(tree, parser);
            var document = new AntlrDocument();
            document.NodeType = NodeConstants.DOCUMENT_NODE;
            AntlrNodeList nl = new AntlrNodeList();
            nl.Add(converted_tree);
            document.ChildNodes = nl;
            AntlrDynamicContext result = new AntlrDynamicContext();
            result.Document = document;
            return result;
        }

        private static AntlrNode BottomUpConvert(IParseTree tree, Parser parser)
        {
            if (tree is TerminalNodeImpl)
            {
                var result = new AntlrElement();
                result.AntlrIParseTree = tree;
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                Interval interval = tok.SourceInterval;
                result.NodeType = NodeConstants.ELEMENT_NODE;
                var common_token_stream = parser.InputStream as CommonTokenStream;
                var lexer = common_token_stream.TokenSource as Lexer;
                var fixed_name = parser.Vocabulary.GetSymbolicName(tok.Symbol.Type);
                result.LocalName = fixed_name;
                var nl = new AntlrNodeList();

                var child = new AntlrText();
                child.AntlrIParseTree = tree;
                child.NodeType = NodeConstants.TEXT_NODE;
                child.Data = Output.PerformEscapes(tree.GetText());
                child.ParentNode = result;

                nl.Add(child);
                child.ChildNodes = nl;

                return result;
            }
            else
            {
                var result = new AntlrElement();
                result.AntlrIParseTree = tree;
                result.NodeType = NodeConstants.ELEMENT_NODE;
                var fixed_name = tree.GetType().ToString()
                    .Replace("Antlr4.Runtime.Tree.", "");
                fixed_name = Regex.Replace(fixed_name, "^.*[+]", "");
                fixed_name = fixed_name.Substring(0, fixed_name.Length - "Context".Length);
                fixed_name = fixed_name[0].ToString().ToLower()
                             + fixed_name.Substring(1);
                result.LocalName = fixed_name;
                var nl = new AntlrNodeList();
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var child = tree.GetChild(i);
                    var convert = BottomUpConvert(child, parser);
                    nl.Add(convert);
                    convert.ParentNode = result;
                }
                result.ChildNodes = nl;
                return result;
            }
        }
    }

    public class AntlrDynamicContext : DynamicContext
    {
        public Node LimitNode { get; }
        public Document Document { get; set; }
        public ResultSequence getVariable(QName name)
        {
            throw new NotImplementedException();
        }

        public URI resolveUri(string uri)
        {
            throw new NotImplementedException();
        }

        public GregorianCalendar CurrentDateTime { get; }
        public Duration TimezoneOffset { get; }
        public Document getDocument(URI uri)
        {
            return Document;
        }

        public IDictionary<string, IList<Document>> Collections { get; }
        public IList<Document> DefaultCollection { get; }
        public CollationProvider CollationProvider { get; }
    }

    public class AntlrNodeList : NodeList
    {
        public List<AntlrNode> _node_list = new List<AntlrNode>();

        public int Length
        {
            get { return _node_list.Count; }
            set { throw new Exception(); }
        }

        public Node item(int i)
        {
            return _node_list[i];
        }

        public void Add(AntlrNode e)
        {
            _node_list.Add(e);
        }
    }

    public class AntlrText : AntlrNode, Text
    {
        public string Data { get; set; }
    }

    public class AntlrAttr : AntlrNode, Attr
    {
        public string Prefix { get; set; }
        public object Name { get; set; }
        public string Value { get; set; }
        public Node OwnerElement { get; set; }
        public TypeInfo SchemaTypeInfo { get; set; }
    }

    public class AntlrDocument : AntlrNode, Document
    {
        public string DocumentURI { get; set; }
        public NodeList getElementsByTagNameNS(string ns, string local)
        {
            throw new NotImplementedException();
        }

        public bool isSupported(string core, string s)
        {
            throw new NotImplementedException();
        }
    }

    public class AntlrElement : AntlrNode, Element
    {
        public object getAttributeNS(string sCHEMA_INSTANCE, string nIL_ATTRIBUTE)
        {
            throw new NotImplementedException();
        }

        public string Prefix { get; set; }
        public TypeInfo SchemaTypeInfo { get; set; }
        public string lookupNamespaceURI(string prefix)
        {
            throw new NotImplementedException();
        }

        public bool isDefaultNamespace(object elementNamespaceUri)
        {
            throw new NotImplementedException();
        }
    }

    public class AntlrNode : Node
    {
        public IParseTree AntlrIParseTree { get; set; }
        public short NodeType { get; set; }
        public string LocalName { get; set; }
        public Document OwnerDocument { get; set; }
        public NodeList ChildNodes { get; set; }
        public Node NextSibling { get; set; }
        public string BaseURI { get; set; }
        public NamedNodeMap Attributes { get; set; }
        public object NodeValue { get; set; }
        public string NamespaceURI { get; set; }
        public object NodeName { get; set; }
        public Node ParentNode { get; set; }
        public Node PreviousSibling { get; set; }
        public bool isSameNode(Node nodeValue)
        {
            throw new NotImplementedException();
        }

        public short compareDocumentPosition(Node nodeB)
        {
            throw new NotImplementedException();
        }

        public bool isEqualNode(Node node)
        {
            throw new NotImplementedException();
        }

        public bool hasChildNodes()
        {
            throw new NotImplementedException();
        }

        public bool hasAttributes()
        {
            throw new NotImplementedException();
        }
    }
}
