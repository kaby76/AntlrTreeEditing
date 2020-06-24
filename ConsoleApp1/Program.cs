using System;
using ClassLibrary2;
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
            var expression = engine.parseExpression("/ruleSpec", new StaticContextBuilder());
            System.Console.WriteLine(expression.ToString());
            var input = System.IO.File.ReadAllText(
                @"C:\Users\kenne\Documents\xpath-csharp\ClassLibrary1\ANTLRv4Parser.g4");
            var tree = Antlr.Parse.Try(input);
            DynamicContext dynamicContext = ConvertToDom.Try(tree);
            expression.evaluate(dynamicContext, Array.Empty<object>());
        }
    }
}
