using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Runtime;

namespace CTree
{
    public class Class1
    {
        public void Parse(string ast_string)
        {
            var ast_stream = CharStreams.fromstring(ast_string);
            var ast_lexer = new AstLexer(ast_stream);
            var ast_tokens = new CommonTokenStream(ast_lexer);
            var ast_parser = new AstParserParser(ast_tokens);
            ast_parser.BuildParseTree = true;
            var listener = new ErrorListener<IToken>();
            ast_parser.AddErrorListener(listener);
            IParseTree ast_tree = ast_parser.ast();
            if (listener.had_error) throw new Exception();
        }
    }
}
