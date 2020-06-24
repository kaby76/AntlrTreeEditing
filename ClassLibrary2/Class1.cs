using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Antlr4.Runtime.Tree;
using java.net;
using java.util;
using javax.xml.datatype;
using javax.xml.@namespace;
using org.eclipse.wst.xml.xpath2.api;
using org.eclipse.wst.xml.xpath2.processor.@internal.utils;
using org.w3c.dom;
using DynamicContext = org.eclipse.wst.xml.xpath2.api.DynamicContext;

namespace ClassLibrary2
{
    public class ConvertToDom
    {
        public static DynamicContext Try(IParseTree tree)
        {
            // Perform bottom up traversal to derive equivalent tree in "dom".
            var converted_tree = BottomUpConvert(tree);
            var document = new AntlrDocument();
            AntlrNodeList nl = new AntlrNodeList();
            nl.Add(converted_tree);
            document.ChildNodes = nl;
            AntlrDynamicContext result = new AntlrDynamicContext();
            result.Document = document;
            return result;
        }

        private static AntlrNode BottomUpConvert(IParseTree tree)
        {
            if (tree is TerminalNodeImpl)
            {
                var result = new AntlrText();
                result.Data = tree.GetText();
                return result;
            }
            else
            {
                var result = new AntlrElement();
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
                    var convert = BottomUpConvert(child);
                    nl.Add(convert);
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
