using System;
using System.Collections.Generic;
using System.Text;
using org.eclipse.wst.xml.xpath2.processor.@internal.ast;

namespace xpath.org.eclipse.wst.xml.xpath2.processor.@internal.ast
{
    public class PostfixExpr : StepExpr
    {
        private object _primary;
        private ICollection<Expr> _exprs;

        public PostfixExpr(object primary, ICollection<Expr> exprs)
        {
            _primary = primary;
            _exprs = exprs;
        }

        public override object accept(XPathVisitor v)
        {
            throw new NotImplementedException();
        }
    }
}
