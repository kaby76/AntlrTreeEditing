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
                if (rs.size() != 65) throw new Exception();
                NewMethod(rs, parser);
            }
            {
                var expression = engine.parseExpression("//atom", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                if (rs.size() != 244) throw new Exception();
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
                if (rs.size() != 113) throw new Exception();
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
                if (rs.size() != 2078) throw new Exception();
            }

            {
                var expression = engine.parseExpression("//*[@Start]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
            }

            {
                var expression = engine.parseExpression("//*[@Start='222']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 7) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            {
                var expression = engine.parseExpression("//*[@End='222']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 3) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            {
                var expression = engine.parseExpression("//*[@Start='222' and @End='222']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 3) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            {
                var expression = engine.parseExpression("//ruleSpec//OR", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 52) throw new Exception();
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
                if (num != 52) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//*[not(self::OR)]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 2078 - 52) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//*[text()]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 605) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//*[text()='RBRACE']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 3) throw new Exception();
            }
            
            {
                // Select first RHS parser rule symbol of rule.
                var expression = engine.parseExpression("//parserRuleSpec/ruleBlock/ruleAltList/labeledAlt/alternative/*[name()='element'][1]/atom/ruleref/*[1]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 55) throw new Exception();
                NewMethod(rs, parser);
            }
            {
                // Select rules that have LHS name = 'atom'.
                var expression = engine.parseExpression("//parserRuleSpec[RULE_REF/text() = 'atom']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 1) throw new Exception();
                NewMethod(rs, parser);
            }
            {
                // Check for any rules that have direct left recursion!
                var expression = engine.parseExpression("//parserRuleSpec[RULE_REF/text() = ruleBlock/ruleAltList/labeledAlt/alternative/*[name()='element'][1]/atom/ruleref/*[1]/text()]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                if (num != 1) throw new Exception();
                NewMethod(rs, parser);
            }

        }

        private static void NewMethod(ResultSequence rs, Parser parser)
        {
            for (var i = rs.iterator(); i.MoveNext();)
            {
                var r = i.Current;
                var node = r.NativeValue as AntlrNode;
                var iparsetree = node?.AntlrIParseTree;
                //if (iparsetree is ParserRuleContext)
                {
                    System.Console.WriteLine(
                        AntlrDOM.Output.OutputTree(iparsetree, parser.InputStream as CommonTokenStream).ToString());
                }
            }
        }
    }
}
