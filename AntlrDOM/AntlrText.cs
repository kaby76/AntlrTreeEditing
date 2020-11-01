namespace AntlrTreeEditing.AntlrDOM
{
    using Antlr4.Runtime.Tree;
    using org.w3c.dom;

    public class AntlrText : AntlrNode, Text
    {
        private AntlrText() : base(null) { }
        public AntlrText(IParseTree n) : base(n) { }
        public string Data { get; set; }
    }
}
