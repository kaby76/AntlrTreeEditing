namespace LanguageServer
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // Class to offer Antlr tree edits, both in-place and out-of-place,
    // and tree copying.
    public class TreeEdits
    {
        public delegate IParseTree Fun(in IParseTree arg1, out bool arg2);

        public static IEnumerable<TerminalNodeImpl> Frontier(IParseTree tree)
        {
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                if (n is TerminalNodeImpl term)
                {
                    yield return term;
                }
                else
                {
                    for (int i = n.ChildCount - 1; i >= 0; --i)
                    {
                        var c = n.GetChild(i);
                        stack.Push(c);
                    }
                }
            }
        }

        public static IEnumerable<IParseTree> FindTopDown(IParseTree tree, Fun find)
        {
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                var found = find(n, out bool @continue);
                if (found != null)
                    yield return found;
                if (!@continue) { }
                else if (n as TerminalNodeImpl != null) { }
                else
                {
                    for (int i = n.ChildCount - 1; i >= 0; --i)
                    {
                        var c = n.GetChild(i);
                        stack.Push(c);
                    }
                }
            }
        }

        public static void Replace(IParseTree tree, Fun find)
        {
            if (tree == null) return;
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                var found = find(n, out bool @continue);
                if (found != null)
                {
                    IParseTree parent = n.Parent;
                    var c = parent as ParserRuleContext;
                    if (c != null)
                    {
                        for (int i = 0; i < c.ChildCount; ++i)
                        {
                            var child = c.children[i];
                            if (child == n)
                            {
                                var temp = c.children[i];
                                if (temp is TerminalNodeImpl)
                                {
                                    var t = temp as TerminalNodeImpl;
                                    t.Parent = null;
                                    c.children[i] = found;
                                    var r = found as TerminalNodeImpl;
                                    r.Parent = c;
                                }
                                else if (temp is ParserRuleContext)
                                {
                                    var t = temp as ParserRuleContext;
                                    t.Parent = null;
                                    c.children[i] = found;
                                    var r = found as ParserRuleContext;
                                    r.Parent = c;
                                }
                                else
                                    throw new Exception("Tree contains something other than TerminalNodeImpl or ParserRuleContext");
                                break;
                            }
                        }
                    }
                }
                if (!@continue) { }
                else if (n as TerminalNodeImpl != null) { }
                else
                {
                    for (int i = n.ChildCount - 1; i >= 0; --i)
                    {
                        var c = n.GetChild(i);
                        stack.Push(c);
                    }
                }
            }
        }

        public static void Replace(IParseTree tree, IParseTree found)
        {
            if (tree == null) return;
            if (found == null) return;
            var n = tree;
            IParseTree parent = n.Parent;
            var c = parent as ParserRuleContext;
            if (c != null)
            {
                for (int i = 0; i < c.ChildCount; ++i)
                {
                    var child = c.children[i];
                    if (child == n)
                    {
                        var temp = c.children[i];
                        if (temp is TerminalNodeImpl)
                        {
                            var t = temp as TerminalNodeImpl;
                            t.Parent = null;
                            c.children[i] = found;
                            var r = found as TerminalNodeImpl;
                            r.Parent = c;
                        }
                        else if (temp is ParserRuleContext)
                        {
                            var t = temp as ParserRuleContext;
                            t.Parent = null;
                            c.children[i] = found;
                            var r = found as ParserRuleContext;
                            r.Parent = c;
                        }
                        else
                            throw new Exception("Tree contains something other than TerminalNodeImpl or ParserRuleContext");
                        break;
                    }
                }
            }
        }

        public static bool InsertAfter(IParseTree tree, Func<IParseTree, IParseTree> insert_point)
        {
            var insert_this = insert_point(tree);
            if (insert_this != null)
            {
                IParseTree parent = tree.Parent;
                var c = parent as ParserRuleContext;
                for (int i = 0; i < c.ChildCount; ++i)
                {
                    var child = c.children[i];
                    if (child == tree)
                    {
                        c.children.Insert(i + 1, insert_this);
                        var r = insert_this as ParserRuleContext;
                        r.Parent = c;
                        break;
                    }
                }
                return true; // done.
            }
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return true;
                else
                    return false;
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    if (InsertAfter(c, insert_point))
                        return true;
                }
            }
            return false;
        }

        public static void Delete(IParseTree tree, Fun find)
        {
            if (tree == null) return;
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                var found = find(n, out bool @continue);
                if (found != null)
                {
                    IParseTree parent = n.Parent;
                    var c = parent as ParserRuleContext;
                    if (c != null)
                    {
                        for (int i = 0; i < c.ChildCount; ++i)
                        {
                            var child = c.children[i];
                            if (child == n)
                            {
                                var temp = c.children[i];
                                if (temp is TerminalNodeImpl)
                                {
                                    var t = temp as TerminalNodeImpl;
                                    t.Parent = null;
                                    c.children.RemoveAt(i);
                                }
                                else if (temp is ParserRuleContext)
                                {
                                    var t = temp as ParserRuleContext;
                                    t.Parent = null;
                                    c.children.RemoveAt(i);
                                }
                                else
                                    throw new Exception("Tree contains something other than TerminalNodeImpl or ParserRuleContext");
                                break;
                            }
                        }
                    }
                }
                if (!@continue) { }
                else if (n as TerminalNodeImpl != null) { }
                else
                {
                    for (int i = n.ChildCount - 1; i >= 0; --i)
                    {
                        var c = n.GetChild(i);
                        stack.Push(c);
                    }
                }
            }
        }

        public static void Delete(IParseTree tree)
        {
            if (tree == null) return;
            var n = tree;
            IParseTree parent = n.Parent;
            var c = parent as ParserRuleContext;
            if (c != null)
            {
                for (int i = 0; i < c.ChildCount; ++i)
                {
                    var child = c.children[i];
                    if (child == n)
                    {
                        var temp = c.children[i];
                        if (temp is TerminalNodeImpl)
                        {
                            var t = temp as TerminalNodeImpl;
                            t.Parent = null;
                            c.children.RemoveAt(i);
                        }
                        else if (temp is ParserRuleContext)
                        {
                            var t = temp as ParserRuleContext;
                            t.Parent = null;
                            c.children.RemoveAt(i);
                        }
                        else
                            throw new Exception("Tree contains something other than TerminalNodeImpl or ParserRuleContext");
                        break;
                    }
                }
            }
        }

        public static void Delete(IEnumerable<IParseTree> trees)
        {
            foreach (var t in trees) Delete(t);
        }

        public static TerminalNodeImpl LeftMostToken(IParseTree tree)
        {
            if (tree is TerminalNodeImpl)
                return tree as TerminalNodeImpl;
            for (int i = 0; i < tree.ChildCount; ++i)
            {
                var c = tree.GetChild(i);
                if (c == null)
                    return null;
                var lmt = LeftMostToken(c);
                if (lmt != null)
                    return lmt;
            }
            return null;
        }

        public static TerminalNodeImpl RightMostToken(IParseTree tree)
        {
            if (tree is TerminalNodeImpl)
                return tree as TerminalNodeImpl;
            for (int i = tree.ChildCount - 1; i >= 0; --i)
            {
                var c = tree.GetChild(i);
                if (c == null)
                    return null;
                var lmt = RightMostToken(c);
                if (lmt != null)
                    return lmt;
            }
            return null;
        }

        public static TerminalNodeImpl NextToken(TerminalNodeImpl leaf)
        {
            if (leaf == null)
                throw new ArgumentNullException(nameof(leaf));
            for (IParseTree v = leaf as IParseTree; v != null; v = v.Parent as IParseTree)
            {
                if (v == null) return null;
                var p = v.Parent as ParserRuleContext;
                int start = -1;
                for (int i = 0; i < p.children.Count; ++i)
                {
                    if (p.children[i] == v && i + 1 < p.children.Count)
                    {
                        start = i + 1;
                        break;
                    }
                }
                if (start < 0) continue;
                for (; start < p.children.Count; ++start)
                {
                    var found = LeftMostToken(p.children[start]);
                    if (found != null)
                        return found;
                }
            }
            return null;
        }

        public static string GetText(IList<IToken> list)
        {
            if (list == null)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (var l in list)
            {
                sb.Append(l.Text);
            }
            return sb.ToString();
        }

        public static (Dictionary<TerminalNodeImpl, string>, List<string>) TextToLeftOfLeaves(BufferedTokenStream stream, IParseTree tree)
        {
            var result = new Dictionary<TerminalNodeImpl, string>();
            var result2 = new List<string>();
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                if (n is TerminalNodeImpl)
                {
                    var nn = n as TerminalNodeImpl;
                    {
                        var p1 = TreeEdits.LeftMostToken(nn);
                        var pp1 = p1.SourceInterval;
                        var pp2 = p1.Payload;
                        var index = pp2.TokenIndex;
                        if (index >= 0)
                        {
                            var p2 = stream.GetHiddenTokensToLeft(index);
                            var p3 = TreeEdits.GetText(p2);
                            result.Add(nn, p3);
                        }
                        result2.Add(nn.GetText());
                    }
                }
                else
                {
                    if (!(n is ParserRuleContext p))
                        continue;
                    if (p.children == null)
                        continue;
                    if (p.children.Count == 0)
                        continue;
                    foreach (var c in p.children.Reverse())
                    {
                        stack.Push(c);
                    }
                }
            }
            return (result, result2);
        }

        public static IParseTree CopyTreeRecursive(IParseTree original, IParseTree parent, Dictionary<TerminalNodeImpl, string> text_to_left)
        {
            if (original == null) return null;
            else if (original is TerminalNodeImpl)
            {
                var o = original as TerminalNodeImpl;
                var new_node = new TerminalNodeImpl(o.Symbol);
                if (text_to_left != null)
                {
                    if (text_to_left.TryGetValue(o, out string value))
                        text_to_left.Add(new_node, value);
                }
                if (parent != null)
                {
                    var parent_rule_context = (ParserRuleContext)parent;
                    new_node.Parent = parent_rule_context;
                    parent_rule_context.AddChild(new_node);
                }
                return new_node;
            }
            else if (original is ParserRuleContext)
            {
                var type = original.GetType();
                var new_node = (ParserRuleContext)Activator.CreateInstance(type, null, 0);
                if (parent != null)
                {
                    var parent_rule_context = (ParserRuleContext)parent;
                    new_node.Parent = parent_rule_context;
                    parent_rule_context.AddChild(new_node);
                }
                int child_count = original.ChildCount;
                for (int i = 0; i < child_count; ++i)
                {
                    var child = original.GetChild(i);
                    CopyTreeRecursive(child, new_node, text_to_left);
                }
                return new_node;
            }
            else return null;
        }

        public static void Reconstruct(StringBuilder sb, IParseTree tree, Dictionary<TerminalNodeImpl, string> text_to_left)
        {
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                text_to_left.TryGetValue(tok, out string inter);
                if (inter == null)
                    sb.Append(" ");
                else
                    sb.Append(inter);
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return;
                sb.Append(tok.GetText());
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    Reconstruct(sb, c, text_to_left);
                }
            }
        }

        public static void AddChildren(IParseTree parent, List<IParseTree> list)
        {
            foreach (var mc in list)
            {
                if (mc is TerminalNodeImpl)
                {
                    var _mc = mc as TerminalNodeImpl;
                    var _mapped_node = parent as ParserRuleContext;
                    _mc.Parent = _mapped_node;
                    _mapped_node.AddChild(_mc);
                }
                else
                {
                    var _mc = mc as ParserRuleContext;
                    var _mapped_node = parent as ParserRuleContext;
                    _mc.Parent = _mapped_node;
                    _mapped_node.AddChild(_mc);
                }
            }

        }

        public static void AddChildren(IParseTree parent, IParseTree child)
        {
            var mc = child;
            {
                if (mc is TerminalNodeImpl)
                {
                    var _mc = mc as TerminalNodeImpl;
                    var _mapped_node = parent as ParserRuleContext;
                    _mc.Parent = _mapped_node;
                    _mapped_node.AddChild(_mc);
                }
                else
                {
                    var _mc = mc as ParserRuleContext;
                    var _mapped_node = parent as ParserRuleContext;
                    _mc.Parent = _mapped_node;
                    _mapped_node.AddChild(_mc);
                }
            }
        }

        public static TerminalNodeImpl Find(IToken token, IParseTree tree)
        {
            if (tree == null) return null;
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                if (n is TerminalNodeImpl term)
                {
                    if (term.Symbol == token)
                        return term;
                }
                else
                {
                    for (int i = n.ChildCount - 1; i >= 0; --i)
                    {
                        var c = n.GetChild(i);
                        stack.Push(c);
                    }
                }
            }
            return null;
        }

        // Insert the string as a token, with the expectation that the entire tree
        // will be printed and re-parsed in the target language.
        public static TerminalNodeImpl InsertBefore(IParseTree node, string arbitrary_string)
        {
            var token = new CommonToken(0) { Line = -1, Column = -1, Text = arbitrary_string };
            var leaf = new TerminalNodeImpl(token);

            IParseTree parent = node.Parent;
            var c = parent as ParserRuleContext;
            for (int i = 0; i < c.ChildCount; ++i)
            {
                var child = c.children[i];
                if (child == node)
                {
                    c.children.Insert(i, leaf);
                    var r = leaf as TerminalNodeImpl;
                    r.Parent = c;
                    break;
                }
	    }
	    return leaf;
        }

        // Insert the string as a token, with the expectation that the entire tree
        // will be printed and re-parsed in the target language.
        public static TerminalNodeImpl InsertAfter(IParseTree node, string arbitrary_string)
        {
            var token = new CommonToken(0) { Line = -1, Column = -1, Text = arbitrary_string };
            var leaf = new TerminalNodeImpl(token);

            IParseTree parent = node.Parent;
            var c = parent as ParserRuleContext;
            for (int i = 0; i < c.ChildCount; ++i)
            {
                var child = c.children[i];
                if (child == node)
                {
                    c.children.Insert(i + 1, leaf);
                    var r = leaf as TerminalNodeImpl;
                    r.Parent = c;
                    break;
                }
	    }
	    return leaf;
        }

        // Insert the string as a token, with the expectation that the entire tree
        // will be printed and re-parsed in the target language.
        public static TerminalNodeImpl Replace(IParseTree node, string arbitrary_string)
        {
            var token = new CommonToken(0) { Line = -1, Column = -1, Text = arbitrary_string };
            var leaf = new TerminalNodeImpl(token);

            IParseTree parent = node.Parent;
            var c = parent as ParserRuleContext;
            for (int i = 0; i < c.ChildCount; ++i)
            {
                var child = c.children[i];
                if (child == node)
                {
                    var temp = c.children[i];
                    if (temp is TerminalNodeImpl)
                    {
                        var t = temp as TerminalNodeImpl;
                        t.Parent = null;
                        c.children[i] = leaf;
                        var r = leaf;
                        r.Parent = c;
                    }
                    else if (temp is ParserRuleContext)
                    {
                        var t = temp as ParserRuleContext;
                        t.Parent = null;
                        c.children[i] = leaf;
                        var r = leaf;
                        r.Parent = c;
                    }
                    else
                        throw new Exception("Tree contains something other than TerminalNodeImpl or ParserRuleContext");
                    break;
                }
	    }
	    return leaf;
        }

        // Insert the tree after another given node.
        public static IParseTree InsertAfter(IParseTree node, IParseTree node_to_insert)
        {
            IParseTree parent = node.Parent;
            var c = parent as ParserRuleContext;
            for (int i = 0; i < c.ChildCount; ++i)
            {
                var child = c.children[i];
                if (child == node)
                {
                    c.children.Insert(i + 1, node_to_insert);
                    var r1 = node_to_insert as TerminalNodeImpl;
                    var r2 = node_to_insert as ParserRuleContext;
                    if (r1 != null) r1.Parent = c;
                    else if (r2 != null) r2.Parent = c;
                    break;
                }
	    }
	    return node_to_insert;
        }

        // Insert the tree after another given node.
        public static IParseTree InsertBefore(IParseTree node, IParseTree node_to_insert)
        {
            IParseTree parent = node.Parent;
            var c = parent as ParserRuleContext;
            for (int i = 0; i < c.ChildCount; ++i)
            {
                var child = c.children[i];
                if (child == node)
                {
                    c.children.Insert(i, node_to_insert);
                    var r1 = node_to_insert as TerminalNodeImpl;
                    var r2 = node_to_insert as ParserRuleContext;
                    if (r1 != null) r1.Parent = c;
                    else if (r2 != null) r2.Parent = c;
                    break;
                }
	    }
	    return node_to_insert;
        }

    }
}
