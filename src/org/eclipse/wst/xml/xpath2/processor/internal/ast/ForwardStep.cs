﻿using System.Diagnostics;

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

namespace org.eclipse.wst.xml.xpath2.processor.@internal.ast
{


	/// <summary>
	/// Class for Forward stepping support for Step operations.
	/// </summary>
	public class ForwardStep : Step
	{
		/// <summary>
		/// Set internal value for NONE.
		/// </summary>
		public const int NONE = 0;
		/// <summary>
		/// Set internal value for CHILD.
		/// </summary>
		public const int CHILD = 1;
		/// <summary>
		/// Set internal value for DESCENDANT.
		/// </summary>
		public const int DESCENDANT = 2;
		/// <summary>
		/// Set internal value for ATTRIBUTE.
		/// </summary>
		public const int ATTRIBUTE = 3;
		/// <summary>
		/// Set internal value for SELF.
		/// </summary>
		public const int SELF = 4;
		/// <summary>
		/// Set internal value for DESCENDANT_OR_SELF.
		/// </summary>
		public const int DESCENDANT_OR_SELF = 5;
		/// <summary>
		/// Set internal value for FOLLOWING_SIBLING.
		/// </summary>
		public const int FOLLOWING_SIBLING = 6;
		/// <summary>
		/// Set internal value for FOLLOWING.
		/// </summary>
		public const int FOLLOWING = 7;
		/// <summary>
		/// Set internal value for NAMESPACE.
		/// </summary>
		public const int NAMESPACE = 8;
		/// <summary>
		/// Set internal value for AT_SYM.
		/// </summary>
		public const int AT_SYM = 9;

		private int _axis;

		// XXX: we should get rid of the int axis... and make only this the axis
		private ForwardAxis _iterator;

		// XXX: needs to be fixed
		private void update_iterator()
		{
			switch (_axis)
			{
			case NONE:
				if (node_test() is AttributeTest)
				{
					_iterator = new AttributeAxis();
				}
				else
				{
					_iterator = new ChildAxis();
				}
				break;

			case CHILD:
				_iterator = new ChildAxis();
				break;

			case DESCENDANT:
				_iterator = new DescendantAxis();
				break;

			case FOLLOWING_SIBLING:
				_iterator = new FollowingSiblingAxis();
				break;

			case FOLLOWING:
				_iterator = new FollowingAxis();
				break;

			case AT_SYM:
			case ATTRIBUTE:
				_iterator = new AttributeAxis();
				break;

			case SELF:
				_iterator = new SelfAxis();
				break;

			case DESCENDANT_OR_SELF:
				_iterator = new DescendantOrSelfAxis();
				break;

			case NAMESPACE:
				throw new StaticError("XPST0010", "namespace axis not implemented");

			default:
				Debug.Assert(false);
				break;
			}
		}

		/// <summary>
		/// Constructor for ForwardStep.
		/// </summary>
		/// <param name="axis">
		///            Axis number. </param>
		/// <param name="node_test">
		///            Node test. </param>
		public ForwardStep(int axis, NodeTest node_test) : base(node_test)
		{

			_axis = axis;

			update_iterator();
		}

		/// <summary>
		/// Support for Visitor interface.
		/// </summary>
		/// <returns> Result of Visitor operation. </returns>
		public override object accept(XPathVisitor v)
		{
			return v.visit(this);
		}

		/// <summary>
		/// Support for Axis interface.
		/// </summary>
		/// <returns> Result of Axis operation. </returns>
		public virtual int axis()
		{
			return _axis;
		}

		/// <summary>
		/// Set Axis to current.
		/// </summary>
		/// <param name="axis">
		///            Axis to set. </param>
		public virtual void set_axis(int axis)
		{
			_axis = axis;
			update_iterator();
		}

		/// <summary>
		/// Support for Iterator interface.
		/// </summary>
		/// <returns> Result of Iterator operation. </returns>
		public virtual ForwardAxis iterator()
		{
			return _iterator;
		}
	}

}