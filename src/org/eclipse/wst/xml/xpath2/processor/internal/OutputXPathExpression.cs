using org.eclipse.wst.xml.xpath2.processor.@internal.ast;
using org.w3c.dom;

namespace xpath.org.eclipse.wst.xml.xpath2.processor.@internal
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;

    internal static class OutputXPathExpression
    {
        private static int changed = 0;
        private static bool first_time = true;

        public static StringBuilder OutputTree(XPathNode tree)
        {
            var sb = new StringBuilder();
            changed = 0;
            first_time = true;
            ParenthesizedAST(tree, sb);
            return sb;
        }

        private static void ParenthesizedAST(XPathNode tree, StringBuilder sb, int level = 0)
        {
            {
                var fixed_name = tree.GetType().ToString();
                fixed_name = Regex.Replace(fixed_name, "^.*[+]", "");
                fixed_name = fixed_name.Substring(0, fixed_name.Length - "Context".Length);
                fixed_name = fixed_name[0].ToString().ToLower()
                             + fixed_name.Substring(1);
                StartLine(sb, tree, level);
                sb.Append("( " + fixed_name);
                sb.AppendLine();
            }
            switch (tree)
            {
                case AxisStep n:
                    break;
                case StepExpr n:
                    break;
                case Expr n:
                    break;
                case ForwardStep n:
                    break;
                case ItemType n:
                    break;
                case NodeTest n:
                    break;
                case PrimaryExpr n:
                    break;
                case ReverseStep n:
                    break;
                case SingleType n:
                    break;
                case SequenceType n:
                    break;


            }

            //for (int i = 0; i < tree; ++i)
            //{
            //    var c = tree.GetChild(i);
            //    c.ParenthesizedAST(sb, stream, level + 1);
            //}
            if (level == 0)
            {
                for (int k = 0; k < 1 + changed - level; ++k) sb.Append(") ");
                sb.AppendLine();
                changed = 0;
            }
        }

        private static void StartLine(StringBuilder sb, XPathNode tree, int level = 0)
        {
            if (changed - level >= 0)
            {
                if (!first_time)
                {
                    for (int j = 0; j < level; ++j) sb.Append("  ");
                    for (int k = 0; k < 1 + changed - level; ++k) sb.Append(") ");
                    sb.AppendLine();
                }
                changed = 0;
                first_time = false;
            }
            changed = level;
            for (int j = 0; j < level; ++j) sb.Append("  ");
        }

        private static string ToLiteral(this string input)
        {
            using (var writer = new StringWriter())
            {
                var literal = input;
                literal = literal.Replace("\\", "\\\\");
                literal = literal.Replace("\b", "\\b");
                literal = literal.Replace("\n", "\\n");
                literal = literal.Replace("\t", "\\t");
                literal = literal.Replace("\r", "\\r");
                literal = literal.Replace("\f", "\\f");
                literal = literal.Replace("\"", "\\\"");
                literal = literal.Replace(string.Format("\" +{0}\t\"", Environment.NewLine), "");
                return literal;
            }
        }
    }
}
