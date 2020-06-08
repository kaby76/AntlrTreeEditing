using System;

/// <summary>
///*****************************************************************************
/// Copyright (c) 2005, 2013 Andrea Bittau, University College London, and others
/// All rights reserved. This program and the accompanying materials
/// are made available under the terms of the Eclipse Public License 2.0
/// which accompanies this distribution, and is available at
/// https://www.eclipse.org/legal/epl-2.0/
/// 
/// SPDX-License-Identifier: EPL-2.0
/// 
/// Contributors:
///     Andrea Bittau - initial API and implementation from the PsychoPath XPath 2.0
///     Bug 338494    - prohibiting xpath expressions starting with / or // to be parsed. 
/// ******************************************************************************
/// </summary>

namespace org.eclipse.wst.xml.xpath2.processor.@internal
{

	using Symbol = java_cup.runtime.Symbol;

	using XPath = org.eclipse.wst.xml.xpath2.processor.ast.XPath;
	using XPathExpr = org.eclipse.wst.xml.xpath2.processor.@internal.ast.XPathExpr;

	/// <summary>
	/// JFlexCupParser parses the xpath expression
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("deprecation") public class InternalXPathParser
	public class InternalXPathParser
	{

		/// <summary>
		/// Tries to parse the xpath expression
		/// </summary>
		/// <param name="xpath">
		///            is the xpath string. </param>
		/// <param name="isRootlessAccess">
		///            if 'true' then PsychoPath engine can't parse xpath expressions starting with / or //. </param>
		/// <exception cref="XPathParserException."> </exception>
		/// <returns> the xpath value.
		/// @since 2.0 </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public org.eclipse.wst.xml.xpath2.processor.ast.XPath parse(String xpath, boolean isRootlessAccess) throws org.eclipse.wst.xml.xpath2.processor.XPathParserException
		public virtual XPath parse(string xpath, bool isRootlessAccess)
		{

			XPathFlex lexer = new XPathFlex(new StringReader(xpath));

			try
			{
				XPathCup p = new XPathCup(lexer);
				Symbol res = p.parse();
				XPath xPath2 = (XPath) res.value;
				if (isRootlessAccess)
				{
					xPath2.accept(new DefaultVisitorAnonymousInnerClass(this));
				}
				return xPath2;
			}
			catch (JFlexError e)
			{
				throw new XPathParserException("JFlex lexer error: " + e.reason());
			}
			catch (CupError e)
			{
				throw new XPathParserException("CUP parser error: " + e.reason());
			}
			catch (StaticError e)
			{
				throw new XPathParserException(e.code(), e.Message);
			}
			catch (Exception e)
			{
				throw new XPathParserException(e.Message);
			}
		}

		private class DefaultVisitorAnonymousInnerClass : DefaultVisitor
		{
			private readonly InternalXPathParser outerInstance;

			public DefaultVisitorAnonymousInnerClass(InternalXPathParser outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override object visit(XPathExpr e)
			{
				if (e.slashes() > 0)
				{
					throw new XPathParserException("Access to root node is not allowed (set by caller)");
				}
				do
				{
					e.expr().accept(this); // check the single step (may have filter with root access)
					e = e.next(); // follow singly linked list of the path, it's all relative past the first one
				} while (e != null);
				return null;
			}
		}
	}

}