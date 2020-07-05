using System.Collections.Generic;
using org.eclipse.wst.xml.xpath2.api;

namespace ConsoleApp1
{
    using Antlr4.Runtime;
    using AntlrDOM;
    using org.eclipse.wst.xml.xpath2.processor;
    using org.eclipse.wst.xml.xpath2.processor.util;
    using System;
    using System.Linq;
    using ResultSequence = org.eclipse.wst.xml.xpath2.api.ResultSequence;

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
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                if (rs.size() != 65) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//atom", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                if (rs.size() != 244) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//ASSIGN", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                if (rs.size() != 1) throw new Exception();
            }
            {
                // Selects ( atom ( terminal ...))
                var expression = engine.parseExpression("//terminal/ancestor::atom", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                if (rs.size() != 113) throw new Exception();
                var first = rs.first();
                var ant = first.NativeValue as AntlrDOM.AntlrElement;
                if (!(ant.AntlrIParseTree is ANTLRv4Parser.AtomContext)) throw new Exception();
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
                OutputResultSet(expression, rs, parser);
            }

            {
                var expression = engine.parseExpression("//*[@Start]//atom", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
            }

            {
                var expression = engine.parseExpression("//*[@Start='222']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                OutputResultSet(expression, rs, parser);
                if (num != 7) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//*[@End='222']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                int num = rs.size();
                OutputResultSet(expression, rs, parser);
                if (num != 3) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//*[@Start='222' and @End='222']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 3) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//ruleSpec//OR", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 52) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            if (false)
            { // Not working.
                var expression = engine.parseExpression("//ruleSpec//'|'", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 51) throw new Exception();
                // atom and ruleref nodes in the parse tree.
            }
            {
                var expression = engine.parseExpression("//OR", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 52) throw new Exception();
            }
            {
                var expression = engine.parseExpression("//*[not(self::OR)]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
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
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 3) throw new Exception();
            }
            
            {
                // Select first RHS parser rule symbol of rule.
                var expression = engine.parseExpression("//parserRuleSpec/ruleBlock/ruleAltList/labeledAlt/alternative/*[name()='element'][1]/atom/ruleref/*[1]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 55) throw new Exception();
                OutputResultSet(expression, rs, parser);
            }
            {
                // Select rules that have LHS name = 'atom'.
                var expression = engine.parseExpression("//parserRuleSpec[RULE_REF/text() = 'atom']", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 1) throw new Exception();
                OutputResultSet(expression, rs, parser);
            }
            {
                // Check for any rules that have direct left recursion!
                var expression = engine.parseExpression("//parserRuleSpec[RULE_REF/text() = ruleBlock/ruleAltList/labeledAlt/alternative/*[name()='element'][1]/atom/ruleref/*[1]/text()]", new StaticContextBuilder());
                object[] contexts = new object[] { dynamicContext.Document };
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 1) throw new Exception();
            }
            {
                // Find parserRuleSpec, pick the first, then find RULE_REF under parserRuleSpec.
                var e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = (new List<object>() { rs1.Select(t => (object)(t.NativeValue)).First() }).ToArray();
                var expression = engine.parseExpression("RULE_REF", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                if (num != 1) throw new Exception();
            }
            {
                // Find parserRuleSpec, picking all, then find RULE_REF under each parserRuleSpec.
                var e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = rs1.Select(t => (object)(t.NativeValue)).ToArray();
                var expression = engine.parseExpression("RULE_REF", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                // Expect 1 item.
            }
            {
                // Find parserRuleSpec, picking all, then find RULE_REF under each parserRuleSpec.
                var e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = rs1.Select(t => (object)(t.NativeValue)).ToArray();
                var expression = engine.parseExpression("/RULE_REF", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                //if (num != 4) throw new Exception();
            }
            {
                // Check for any rules that have direct left recursion!
                var e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = rs1.Select(t => (object)(t.NativeValue)).ToArray();
                var expression = engine.parseExpression("/*[RULE_REF/text() = ruleBlock/ruleAltList/labeledAlt/alternative/*[name()='element'][1]/atom/ruleref/*[1]/text()]", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
                //if (num != 1) throw new Exception();
            }
            if (true) // does not work.
            {
                // Find parserRuleSpec, picking all, then find RULE_REF under each parserRuleSpec.
                XPath2Expression e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = new object[] { rs1.First().NativeValue };
                var expression = engine.parseExpression(".", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
            }
            if (true) // does not work.
            {
                // Find parserRuleSpec, picking all, then find RULE_REF under each parserRuleSpec.
                var e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = new object[] { rs1.First().NativeValue };
                // Starts way back at the top of the whole tree, not parserRuleSpec!
                var expression = engine.parseExpression("/", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
            }
            if (true) // does not work.
            {
                // Find parserRuleSpec, picking all, then find RULE_REF under each parserRuleSpec.
                var e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = new object[] { rs1.First().NativeValue };
                var expression = engine.parseExpression("RULE_REF", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
            }
            {
                // Find parserRuleSpec, picking all, then find RULE_REF under each parserRuleSpec.
                var e1 = engine.parseExpression("//parserRuleSpec", new StaticContextBuilder());
                object[] c1 = new object[] { dynamicContext.Document };
                var rs1 = e1.evaluate(dynamicContext, c1);
                object[] contexts = rs1.Select(t => (object)(t.NativeValue)).ToArray();
                var expression = engine.parseExpression("RULE_REF", new StaticContextBuilder());
                var rs = expression.evaluate(dynamicContext, contexts);
                OutputResultSet(expression, rs, parser);
                int num = rs.size();
            }

        }

        private static void OutputResultSet(XPath2Expression expression, ResultSequence rs, Parser parser)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("==============================");
            System.Console.WriteLine("Result set for \"" + expression.Expression + "\"");
            System.Console.WriteLine("result size " + rs.size());
            for (var i = rs.iterator(); i.MoveNext();)
            {
                var r = i.Current;
                var node = r.NativeValue as AntlrNode;
                var iparsetree = node?.AntlrIParseTree;
                System.Console.WriteLine(
                    AntlrDOM.Output.OutputTree(iparsetree, parser.InputStream as CommonTokenStream).ToString());
            }
        }
    }
}
