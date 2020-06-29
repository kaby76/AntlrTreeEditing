using System;
using System.Linq;
using Antlr4.Runtime;
using AntlrDOM;
using Microsoft.Build.Framework;
using org.eclipse.wst.xml.xpath2.processor;
using org.eclipse.wst.xml.xpath2.processor.util;
using DynamicContext = org.eclipse.wst.xml.xpath2.api.DynamicContext;
using ResultSequence = org.eclipse.wst.xml.xpath2.api.ResultSequence;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            org.eclipse.wst.xml.xpath2.processor.Engine engine = new Engine();
            var input = System.IO.File.ReadAllText(args[0]);
            var (tree, parser) = AntlrDOM.Parse.Try(input);
            AntlrDynamicContext dynamicContext = AntlrDOM.ConvertToDOM.Try(tree, parser);

            // Tests.
            {
                var expression = engine.parseExpression("//ruleSpec", new StaticContextBuilder());
                System.Console.WriteLine(expression.ToString());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                if (rs.size() != 64) throw new Exception();
                NewMethod(rs, parser);
            }
            {
                var expression = engine.parseExpression("//atom", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                if (rs.size() != 241) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//ASSIGN", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                if (rs.size() != 1) throw new Exception();
            }
            {
                // Selects ( atom ( terminal ...))
                var expression = engine.parseExpression("//terminal/ancestor::atom", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                if (rs.size() != 111) throw new Exception();
                var first = rs.first();
                var ant = first.NativeValue as AntlrDOM.AntlrElement;
                if (!(ant.AntlrIParseTree is ANTLRv4Parser.AtomContext)) throw new Exception();
                //NewMethod(rs, parser);
            }
            {
                // Selects all nodes.
                var expression = engine.parseExpression("//*", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                if (rs.size() != 2053) throw new Exception();
            }

            {
                var expression = engine.parseExpression("//*[@SourceInterval]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
            }

            {
                var expression = engine.parseExpression("//*[@SourceInterval='[222,222]']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 2) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            {
                var expression = engine.parseExpression("//ruleSpec//OR", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 51) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            if (false)
            { // Not working.
                var expression = engine.parseExpression("//ruleSpec//'|'", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 51) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            {
                var expression = engine.parseExpression("//OR", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
            }
            {
                var expression = engine.parseExpression("//!OR", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
            }

        }

        private static void NewMethod(ResultSequence rs, Parser parser)
        {
            for (var i = rs.iterator(); i.MoveNext();)
            {
                var r = i.Current;
                var node = r.NativeValue as AntlrNode;
                var iparsetree = node?.AntlrIParseTree;
                if (iparsetree is ParserRuleContext)
                {
                    System.Console.WriteLine(
                        AntlrDOM.Output.OutputTree(iparsetree, parser.InputStream as CommonTokenStream).ToString());
                }
            }
        }
    }
}
