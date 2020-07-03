/// <summary>
///*****************************************************************************
/// Copyright (c) 2005, 2011 Andrea Bittau, University College London, and others
/// All rights reserved. This program and the accompanying materials
/// are made available under the terms of the Eclipse Public License 2.0
/// which accompanies this distribution, and is available at
/// https://www.eclipse.org/legal/epl-2.0/
/// 
/// SPDX-License-Identifier: EPL-2.0
/// 
/// Contributors:
///     Andrea Bittau - initial API and implementation from the PsychoPath XPath 2.0 
///     Mukul Gandhi - bug 280798 - PsychoPath support for JDK 1.4
/// ******************************************************************************
/// </summary>

using System.Collections.Generic;
using System.Text;

namespace org.eclipse.wst.xml.xpath2.processor.@internal.ast
{

	/// <summary>
	/// The comparison of expression operator takes the value of its left operand and
	/// compares (dependant on type) against its right operand, according to the
	/// rules of the particular comparison rule
	/// </summary>
	public class CmpExpr : BinExpr
	{
		/// <summary>
		/// Set internal value for EQUALS operation.
		/// </summary>
		public const int EQUALS = 0;
		/// <summary>
		/// Set internal value for NOTEQUALS operation.
		/// </summary>
		public const int NOTEQUALS = 1;
		/// <summary>
		/// Set internal value for LESSTHAN operation.
		/// </summary>
		public const int LESSTHAN = 2;
		/// <summary>
		/// Set internal value for LESSEQUAL operation.
		/// </summary>
		public const int LESSEQUAL = 3;
		/// <summary>
		/// Set internal value for GREATER operation.
		/// </summary>
		public const int GREATER = 4;
		/// <summary>
		/// Set internal value for GREATEREQUAL operation.
		/// </summary>
		public const int GREATEREQUAL = 5;
		/// <summary>
		/// Set internal value for EQ operation.
		/// </summary>
		public const int EQ = 6;
		/// <summary>
		/// Set internal value for NE operation.
		/// </summary>
		public const int NE = 7;
		/// <summary>
		/// Set internal value for LT operation.
		/// </summary>
		public const int LT = 8;
		/// <summary>
		/// Set internal value for LE operation.
		/// </summary>
		public const int LE = 9;
		/// <summary>
		/// Set internal value for GT operation.
		/// </summary>
		public const int GT = 10;
		/// <summary>
		/// Set internal value for GE operation.
		/// </summary>
		public const int GE = 11;
		/// <summary>
		/// Set internal value for IS operation.
		/// </summary>
		public const int IS = 12;
		/// <summary>
		/// Set internal value for LESS_LESS operation.
		/// </summary>
		public const int LESS_LESS = 13;
		/// <summary>
		/// Set internal value for GREATER_GREATER operation.
		/// </summary>
		public const int GREATER_GREATER = 14;

		private int _type;

		/// <summary>
		/// Constructor for CmpExpr
		/// </summary>
		/// <param name="l">
		///            input xpath left expression/variable </param>
		/// <param name="r">
		///            input xpath right expression/variable </param>
		/// <param name="type">
		///            what comparison to use l against r. </param>
		public CmpExpr(Expr l, Expr r, int type) : base(l, r)
		{

			_type = type;
		}

		/// <summary>
		/// Support for Visitor interface.
		/// </summary>
		/// <returns> Result of Visitor operation. </returns>
		public override object accept(XPathVisitor v)
		{
			return v.visit(this);
		}

		/// <returns> comparison type </returns>
		public virtual int type()
		{
			return _type;
		}

        public override ICollection<XPathNode> GetAllChildren()
        {
            throw new System.NotImplementedException();
        }
	}

}