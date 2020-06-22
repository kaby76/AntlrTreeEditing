using System;
using org.eclipse.wst.xml.xpath2.processor;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            org.eclipse.wst.xml.xpath2.processor.Engine engine = new Engine();
            var expression = engine.parseExpression("//a/b/c", null);
        }
    }
}
