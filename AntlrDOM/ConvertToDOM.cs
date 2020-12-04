namespace AntlrTreeEditing.AntlrDOM
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using org.w3c.dom;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ConvertToDOM
    {
        Dictionary<IParseTree, AntlrNode> nodes = new Dictionary<IParseTree, AntlrNode>();

        public AntlrNode FindDomNode(IParseTree tree)
        {
            nodes.TryGetValue(tree, out AntlrNode result);
            return result;
        }

        public AntlrDynamicContext Try(IParseTree tree, Parser parser)
        {
            // Perform bottom up traversal to derive equivalent tree in "dom".
            var converted_tree = BottomUpConvert(tree, parser);
            Stack<AntlrNode> stack = new Stack<AntlrNode>();
            stack.Push(converted_tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                if (n.AntlrIParseTree != null) nodes[n.AntlrIParseTree] = n;
                var l = n.ChildNodes;
                if (l != null)
                {
                    for (int i = 0; i < l.Length; ++i)
                    {
                        stack.Push((AntlrNode)l.item(i));
                    }
                }
            }
            var document = new AntlrDocument(null);
            document.NodeType = NodeConstants.DOCUMENT_NODE;
            AntlrNodeList nl = new AntlrNodeList();
            nl.Add(converted_tree);
            document.ChildNodes = nl;
            AntlrDynamicContext result = new AntlrDynamicContext();
            result.Document = document;
            return result;
        }

        private AntlrNode BottomUpConvert(IParseTree tree, Parser parser)
        {
            if (tree is TerminalNodeImpl)
            {
                var result = new AntlrElement(tree);
                //result.AntlrIParseTree = tree;
                TerminalNodeImpl t = tree as TerminalNodeImpl;
                Interval interval = t.SourceInterval;
                result.NodeType = NodeConstants.ELEMENT_NODE;
                var fixed_name = parser.Vocabulary.GetSymbolicName(t.Symbol.Type);
                result.LocalName = fixed_name;
                var nl = new AntlrNodeList();
                result.ChildNodes = nl;
                var child = new AntlrText(tree);
                //child.AntlrIParseTree = tree;
                child.NodeType = NodeConstants.TEXT_NODE;
                child.Data = new xpath.org.eclipse.wst.xml.xpath2.processor.@internal.OutputParseTree().PerformEscapes(/*"'" + */ tree.GetText() /*+ "'"*/);
                child.ParentNode = result;
                nl.Add(child);
                {
                    var attr = new AntlrAttr(null);
                    var child_count = t.ChildCount;
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    attr.Name = "ChildCount";
                    attr.Value = child_count.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                }
                {
                    var attr = new AntlrAttr(null);
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    var source_interval = t.SourceInterval;
                    var a = source_interval.a;
                    var b = source_interval.b;
                    attr.Name = "SourceInterval";
                    attr.Value = "[" + a + "," + b + "]";
                    attr.ParentNode = result;
                    nl.Add(attr);
                }
                return result;
            }
            else
            {
                var result = new AntlrElement(tree);
                var t = tree as ObserverParserRuleContext;
                if (t != null) t.Subscribe(result);
                //result.AntlrIParseTree = tree;
                result.NodeType = NodeConstants.ELEMENT_NODE;
                var fixed_name = tree.GetType().ToString()
                    .Replace("Antlr4.Runtime.Tree.", "");
                fixed_name = Regex.Replace(fixed_name, "^.*[+]", "");
                fixed_name = fixed_name.Substring(0, fixed_name.Length - "Context".Length);
                fixed_name = fixed_name[0].ToString().ToLower()
                             + fixed_name.Substring(1);
                result.LocalName = fixed_name;
                var nl = new AntlrNodeList();
                result.ChildNodes = nl;
                var map = new AntlrNamedNodeMap();
                result.Attributes = map;
                {
                    var attr = new AntlrAttr(null);
                    var child_count = tree.ChildCount;
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    attr.Name = "ChildCount";
                    attr.LocalName = "ChildCount";
                    attr.Value = child_count.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                    map.Add(attr);
                }
                {
                    var attr = new AntlrAttr(null);
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    var source_interval = tree.SourceInterval;
                    var a = source_interval.a;
                    var b = source_interval.b;
                    attr.Name = "Start";
                    attr.LocalName = "Start";
                    attr.Value = a.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                    map.Add(attr);
                }
                {
                    var attr = new AntlrAttr(null);
                    attr.NodeType = NodeConstants.ATTRIBUTE_NODE;
                    var source_interval = tree.SourceInterval;
                    var a = source_interval.a;
                    var b = source_interval.b;
                    attr.Name = "End";
                    attr.LocalName = "End";
                    attr.Value = b.ToString();
                    attr.ParentNode = result;
                    nl.Add(attr);
                    map.Add(attr);
                }
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var child = tree.GetChild(i);
                    var convert = BottomUpConvert(child, parser);
                    nl.Add(convert);
                    convert.ParentNode = result;
                }
                for (int i = 0; i < nl.Length; ++i)
                {
                    var x = nl._node_list[i];
                    if (i > 0)
                    {
                        var pre = nl._node_list[i - 1];
                        x.PreviousSibling = pre;
                        pre.NextSibling = x;
                    }
                }
                return result;
            }
        }
    }
}
