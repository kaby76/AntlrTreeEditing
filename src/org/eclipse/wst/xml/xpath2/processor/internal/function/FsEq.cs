﻿using System;
using System.Diagnostics;
using System.Collections;

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
///     Mukul Gandhi - bug 276134 - improvements to schema aware primitive type support
///                                 for attribute/element nodes 
///     David Carver - bug 262765 - fixed comparison on sequence range values.
///     Jesper S Moller - bug 283214 - fix eq for untyped atomic values
///     Jesper Steen Moeller - bug 285145 - implement full arity checking
///     Jesper Steen Moeller - bug 280555 - Add pluggable collation support
///     Mukul Gandhi - bug 280798 - PsychoPath support for JDK 1.4
/// ******************************************************************************
/// </summary>

namespace org.eclipse.wst.xml.xpath2.processor.@internal.function
{


	using DynamicContext = org.eclipse.wst.xml.xpath2.api.DynamicContext;
	using EvaluationContext = org.eclipse.wst.xml.xpath2.api.EvaluationContext;
	using Item = org.eclipse.wst.xml.xpath2.api.Item;
	using ResultBuffer = org.eclipse.wst.xml.xpath2.api.ResultBuffer;
	using ResultSequence = org.eclipse.wst.xml.xpath2.api.ResultSequence;
	using AnyType = org.eclipse.wst.xml.xpath2.processor.@internal.types.AnyType;
	using NumericType = org.eclipse.wst.xml.xpath2.processor.@internal.types.NumericType;
	using QName = org.eclipse.wst.xml.xpath2.processor.@internal.types.QName;
	using XSBoolean = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSBoolean;
	using XSDouble = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSDouble;
	using XSString = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSString;
	using XSUntypedAtomic = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSUntypedAtomic;

	/// <summary>
	/// Class for the Equality function.
	/// </summary>
	public class FsEq : Function
	{
		/// <summary>
		/// Constructor for FsEq.
		/// </summary>
		public FsEq() : base(new QName("eq"), 2)
		{
		}

		/// <summary>
		/// Evaluate arguments.
		/// </summary>
		/// <param name="args">
		///            argument expressions. </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of evaluation. </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public org.eclipse.wst.xml.xpath2.api.ResultSequence evaluate(java.util.Collection args, org.eclipse.wst.xml.xpath2.api.EvaluationContext ec) throws org.eclipse.wst.xml.xpath2.processor.DynamicError
		public override ResultSequence evaluate(ICollection args, EvaluationContext ec)
		{
			Debug.Assert(args.Count >= min_arity() && args.Count <= max_arity());

			return fs_eq_value(args, ec.DynamicContext);
		}

		/// <summary>
		/// Converts arguments to values.
		/// </summary>
		/// <param name="args">
		///            Result from expressions evaluation. </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of conversion. </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static java.util.Collection value_convert_args(java.util.Collection args) throws org.eclipse.wst.xml.xpath2.processor.DynamicError
		private static ICollection value_convert_args(ICollection args)
		{
			var result = new ArrayList(args.Count);

			// atomize arguments
			for (IEnumerator i = args.GetEnumerator(); i.MoveNext();)
			{
				ResultSequence rs = (ResultSequence) i.Current;

				//FnData.fast_atomize(rs);
				rs = FnData.atomize(rs);

				if (rs.empty())
				{
					return new ArrayList();
				}

				if (rs.size() > 1)
				{
					throw new DynamicError(TypeError.invalid_type(null));
				}

				Item arg = rs.first();

				if (arg is XSUntypedAtomic)
				{
					arg = new XSString(arg.StringValue);
				}

				result.Add(arg);
			}

			return result;
		}

		/// <summary>
		/// Conversion operation for the values of the arguments.
		/// </summary>
		/// <param name="args">
		///            Result from convert value operation. </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of conversion. </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.eclipse.wst.xml.xpath2.api.ResultSequence fs_eq_value(java.util.Collection args, org.eclipse.wst.xml.xpath2.api.DynamicContext context) throws org.eclipse.wst.xml.xpath2.processor.DynamicError
		public static ResultSequence fs_eq_value(ICollection args, DynamicContext context)
		{
			return do_cmp_value_op(args, typeof(CmpEq), "eq", context);
		}

		/// <summary>
		/// A fast Equality operation, no conversion for the inputs performed.
		/// </summary>
		/// <param name="one">
		///            input1 of any type. </param>
		/// <param name="two">
		///            input2 of any type. </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of Equality operation. </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static boolean fs_eq_fast(org.eclipse.wst.xml.xpath2.processor.internal.types.AnyType one, org.eclipse.wst.xml.xpath2.processor.internal.types.AnyType two, org.eclipse.wst.xml.xpath2.api.DynamicContext context) throws org.eclipse.wst.xml.xpath2.processor.DynamicError
		public static bool fs_eq_fast(AnyType one, AnyType two, DynamicContext context)
		{

			one = FnData.atomize((Item)one);
			two = FnData.atomize((Item)two);

			if (one is XSUntypedAtomic)
			{
				one = new XSString(one.StringValue);
			}

			if (two is XSUntypedAtomic)
			{
				two = new XSString(two.StringValue);
			}

			if (!(one is CmpEq))
			{
				DynamicError.throw_type_error();
			}

			CmpEq cmpone = (CmpEq) one;

			return cmpone.eq(two, context);
		}

		/// <summary>
		/// Making sure that the types are the same before comparing the inputs.
		/// </summary>
		/// <param name="a">
		///            input1 of any type. </param>
		/// <param name="b">
		///            input2 of any type. </param>
		/// <param name="dc">
		///              Dynamic Context </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of Equality operation. </returns>
		private static bool do_general_pair(AnyType a, AnyType b, IComparer comparator, DynamicContext ec)
		{

			// section 3.5.2

			// rule a
			// if one is untyped and other is numeric, cast untyped to
			// double
			if ((a is XSUntypedAtomic && b is NumericType) || (b is XSUntypedAtomic && a is NumericType))
			{
				if (a is XSUntypedAtomic)
				{
					a = new XSDouble(a.StringValue);
				}
				else
				{
					b = new XSDouble(b.StringValue);
				}

			}

			// rule b
			// if one is untyped and other is string or untyped, then cast
			// untyped to string
			else if ((a is XSUntypedAtomic && (b is XSString || b is XSUntypedAtomic) || (b is XSUntypedAtomic && (a is XSString || a is XSUntypedAtomic))))
			{

				if (a is XSUntypedAtomic)
				{
					a = new XSString(a.StringValue);
				}
				if (b is XSUntypedAtomic)
				{
					b = new XSString(b.StringValue);
				}
			}

			// rule c
			// if one is untyped and other is not string,untyped,numeric
			// cast untyped to dynamic type of other

			// XXX?
			// TODO: This makes no sense as implemented before
			else if (a is XSUntypedAtomic)
			{
	//			ResultSequence converted = ResultSequenceFactory.create_new(a);
	//			assert converted.size() == 1;
	//			a = converted.first();
			}
			else if (b is XSUntypedAtomic)
			{
	//			ResultSequence converted = ResultSequenceFactory.create_new(b);
	//			assert converted.size() == 1;
	//			b = converted.first();
			}

			// rule d
			// if value comparison is true, return true.

			ResultSequence one = ResultSequenceFactory.create_new(a);
			ResultSequence two = ResultSequenceFactory.create_new(b);

			var args = new ArrayList();
			args.Add(one);
			args.Add(two);

			object[] margs = new object[] {args, ec};

			ResultSequence result = null;
			try
			{
				throw new Exception();
				//result = (ResultSequence) comparator.Compare(args, ec);
			}
			catch (Exception ex)
            {
                throw;
            }

			if (((XSBoolean) result.first()).value())
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// A general equality function.
		/// </summary>
		/// <param name="args">
		///            input arguments. </param>
		/// <param name="dc">
		///         Dynamic context </param>
		/// <returns> Result of general equality operation. </returns>
		public static ResultSequence fs_eq_general(ICollection args, DynamicContext dc)
		{
			return do_cmp_general_op(args, typeof(FsEq), "fs_eq_value", dc);
		}

		// voodoo 3
		/// <summary>
		/// Actual equality operation for fs_eq_general.
		/// </summary>
		/// <param name="args">
		///            input arguments. </param>
		/// <param name="type">
		///            type of the arguments. </param>
		/// <param name="mname">
		///            Method name for template simulation. </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of the operation. </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.eclipse.wst.xml.xpath2.api.ResultSequence do_cmp_general_op(java.util.Collection args, Class type, String mname, org.eclipse.wst.xml.xpath2.api.DynamicContext dc) throws org.eclipse.wst.xml.xpath2.processor.DynamicError
		public static ResultSequence do_cmp_general_op(ICollection args, Type type, string mname, DynamicContext dc)
		{

			// do the voodoo
			IComparer comparator = null;

			try
			{
				Type[] margsdef = new Type[] {typeof(ICollection), typeof(DynamicContext)};

//				comparator = type.GetMethod(mname, margsdef);

			}
			catch
			{
				throw new Exception("Can't find method : " + mname);
			}

			// sanity check args and get them
			if (args.Count != 2)
			{
				DynamicError.throw_type_error();
			}

			IEnumerator argiter = args.GetEnumerator();

            argiter.MoveNext();
            ResultSequence one = (ResultSequence) argiter.Current;
            argiter.MoveNext();
			ResultSequence two = (ResultSequence) argiter.Current;

			// XXX ?
			if (one.empty() || two.empty())
			{
				return ResultSequenceFactory.create_new(new XSBoolean(false));
			}

			// atomize
			one = FnData.atomize(one);
			two = FnData.atomize(two);

			// we gotta find a pair that satisfied the condition
			for (IEnumerator i = one.iterator(); i.MoveNext();)
			{
				AnyType a = (AnyType) i.Current;
				for (IEnumerator j = two.iterator(); j.MoveNext();)
				{
					AnyType b = (AnyType) j.Current;

					if (do_general_pair(a, b, comparator, dc))
					{
						return ResultSequenceFactory.create_new(new XSBoolean(true));
					}
				}
			}

			return ResultSequenceFactory.create_new(new XSBoolean(false));
		}

		// voodoo 2
		/// <summary>
		/// Actual equality operation for fs_eq_value.
		/// </summary>
		/// <param name="args">
		///            input arguments. </param>
		/// <param name="type">
		///            type of the arguments. </param>
		/// <param name="mname">
		///            Method name for template simulation. </param>
		/// <param name="dynamicContext"> 
		///             Dynamic error. </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of the operation. </returns>
		public static ResultSequence do_cmp_value_op(ICollection args, Type type, string mname, DynamicContext context)
		{

			// sanity check args + convert em
			if (args.Count != 2)
			{
				DynamicError.throw_type_error();
			}

			ICollection cargs = value_convert_args(args);

			if (cargs.Count == 0)
			{
				return ResultBuffer.EMPTY;
			}

			// make sure arugments are comparable by equality
			IEnumerator argi = cargs.GetEnumerator();
            argi.MoveNext();

			Item arg = ((ResultSequence) argi.Current).first();
            ResultSequence arg2 = (ResultSequence) argi.Current;

			if (arg2.size() != 1)
			{
				DynamicError.throw_type_error();
			}

			if (!(type.IsInstanceOfType(arg)))
			{
				DynamicError.throw_type_error();
			}
			throw new Exception();
			try
			{
				//Type[] margsdef = new Type[] {typeof(AnyType), typeof(DynamicContext)};
				//IComparer method = null;

				//method = type.GetMethod(mname, margsdef);

				//object[] margs = new object[] {arg2.first(), context};
				//bool? cmpres = (bool?) method.Compare(arg, margs);

				//return ResultSequenceFactory.create_new(new XSBoolean(cmpres.Value));
			}
			catch 
			{
				Debug.Assert(false);
				throw new Exception("cannot compare using method " + mname);
			}
		}
	}

}