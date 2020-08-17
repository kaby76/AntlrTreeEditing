namespace AntlrTreeEditing.AntlrDOM
{
    using Antlr4.Runtime.Tree;
    using org.w3c.dom;
    using System;

    public class AntlrNode : Node, IAntlrObserver
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

        public void OnParentDisconnect(IParseTree value)
        {
            if (ParentNode != null)
            {
                AntlrNodeList children = ParentNode.ChildNodes as AntlrNodeList;
                children.Delete(this);
            }
            ParentNode = null;
        }

        public void OnParentConnect(IParseTree value)
        {
        }

        public void OnChildDisconnect(IParseTree value)
        {
        }

        public void OnChildConnect(IParseTree value)
        {
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ObserverParserRuleContext value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            var tree = AntlrIParseTree as ObserverParserRuleContext;
            if (tree != null)
            {
                tree.Unsubscribe(this);
            }
            if (ChildNodes != null)
            {
                for (int i = 0; i < ChildNodes.Length; ++i)
                {
                    Node c = ChildNodes.item(i);
                    var cc = c as AntlrNode;
                    cc?.Dispose();
                }
            }
        }
    }
}
