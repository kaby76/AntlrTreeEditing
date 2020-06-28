using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using org.eclipse.wst.xml.xpath2.processor.@internal.ast;

namespace xpath.org.eclipse.wst.xml.xpath2.processor.@internal.ast
{
    public class PostfixExpr : StepExpr
    {
        private PrimaryExpr _pexpr;
        private ICollection<Expr> _exprs;

        public virtual PrimaryExpr primary()
        {
            return _pexpr;
        }

        public IEnumerator GetEnumerator()
        {
            return _exprs.GetEnumerator();
        }

        public PostfixExpr(object pexpr, ICollection<Expr> exprs)
        {
            _pexpr = (PrimaryExpr)pexpr;
            _exprs = exprs;
        }

        public override object accept(XPathVisitor v)
        {
            return v.visit(this);
        }

        public virtual IEnumerator<Expr> iterator()
        {
            return _exprs.GetEnumerator();
        }

        public virtual int predicate_count()
        {
            return _exprs.Count;
        }

    }
}
