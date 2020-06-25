using System;
using Antlr4.Runtime;
using ClassLibrary2;
using Microsoft.Build.Framework;
using org.eclipse.wst.xml.xpath2.processor;
using org.eclipse.wst.xml.xpath2.processor.util;
using DynamicContext = org.eclipse.wst.xml.xpath2.api.DynamicContext;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            org.eclipse.wst.xml.xpath2.processor.Engine engine = new Engine();
            var expression = engine.parseExpression("//ruleSpec", new StaticContextBuilder());
            System.Console.WriteLine(expression.ToString());
            var input = System.IO.File.ReadAllText(
                @"C:\Users\kenne\Documents\xpath-csharp\ClassLibrary1\ANTLRv4Parser.g4");
            var (tree, parser) = Antlr.Parse.Try(input);
            AntlrDynamicContext dynamicContext = ConvertToDom.Try(tree);
            object[] contexts = new object[] { dynamicContext.Document };
            var rs = expression.evaluate(dynamicContext, contexts);
            foreach (var r in rs)
            {
                var node = r.NativeValue as AntlrNode;
                var iparsetree = node?.AntlrIParseTree;
                if (iparsetree is ParserRuleContext)
                {
                    System.Console.WriteLine(
                        Antlr.Output.OutputTree(iparsetree, parser.InputStream as CommonTokenStream).ToString());
                }
            }
        }
    }
}
