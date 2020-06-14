using System;
using System.Collections;

/// <summary>
///*****************************************************************************
/// Copyright (c) 2009, 2011 Mukul Gandhi, and others
/// All rights reserved. This program and the accompanying materials
/// are made available under the terms of the Eclipse Public License 2.0
/// which accompanies this distribution, and is available at
/// https://www.eclipse.org/legal/epl-2.0/
/// 
/// SPDX-License-Identifier: EPL-2.0
/// 
/// Contributors:
///     Mukul Gandhi - bug 281822 - initial API and implementation
///     David Carver - bug 282223 - implementation of xs:duration 
///     Mukul Gandhi - bug 280798 - PsychoPath support for JDK 1.4
/// ******************************************************************************
/// </summary>

namespace org.eclipse.wst.xml.xpath2.processor.@internal.function
{


	using EvaluationContext = org.eclipse.wst.xml.xpath2.api.EvaluationContext;
	using ResultBuffer = org.eclipse.wst.xml.xpath2.api.ResultBuffer;
	using ResultSequence = org.eclipse.wst.xml.xpath2.api.ResultSequence;
	using StaticContext = org.eclipse.wst.xml.xpath2.api.StaticContext;
	using QName = org.eclipse.wst.xml.xpath2.processor.@internal.types.QName;
	using XSDate = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSDate;
	using XSDateTime = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSDateTime;
	using XSDuration = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSDuration;
	using XSTime = org.eclipse.wst.xml.xpath2.processor.@internal.types.XSTime;

	/// <summary>
	/// A special constructor function for constructing a xs:dateTime value from a xs:date
	/// value and a xs:time value.
	/// ref: Section 5.2 of the F&O spec, http://www.w3.org/TR/xpath-functions/.
	/// </summary>
	public class FnDateTime : Function
	{
		private static ArrayList _expected_args = null;

		/// <summary>
		/// Constructor for FnDateTime.
		/// </summary>
		public FnDateTime() : base(new QName("dateTime"), 2)
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
			return dateTime(args, ec.StaticContext);
		}

		/// <summary>
		/// Evaluate the function using the arguments passed.
		/// </summary>
		/// <param name="args">
		///            Result from the expressions evaluation. </param>
		/// <param name="sc">
		///            Result of static context operation. </param>
		/// <exception cref="DynamicError">
		///             Dynamic error. </exception>
		/// <returns> Result of the fn:dateTime operation. </returns>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.eclipse.wst.xml.xpath2.api.ResultSequence dateTime(java.util.Collection args, org.eclipse.wst.xml.xpath2.api.StaticContext sc) throws org.eclipse.wst.xml.xpath2.processor.DynamicError
		public static ResultSequence dateTime(ICollection args, StaticContext sc)
		{

			ICollection cargs = Function.convert_arguments(args, expected_args());

			// get args
			IEnumerator argiter = cargs.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			ResultSequence arg1 = (ResultSequence) argiter.next();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			ResultSequence arg2 = (ResultSequence) argiter.next();

			// if either of the parameter is an empty sequence, the result
			// is an empty sequence
			if (arg1.empty() || arg2.empty())
			{
				  return ResultBuffer.EMPTY;
			}
			XSDate param1 = (XSDate)arg1.first();
			XSTime param2 = (XSTime)arg2.first();

			DateTime cal = new DateTime();
			cal = new DateTime(param1.year(), param1.month() - 1, param1.day());
			cal.set(DateTime.HOUR_OF_DAY, param2.hour());
			cal.set(DateTime.MINUTE, param2.minute());
			cal.set(DateTime.SECOND, ((new double?(Math.Floor(param2.second()))).intValue()));
			cal.set(DateTime.MILLISECOND, 0);

			XSDuration dateTimeZone = param1.tz();
			XSDuration timeTimeZone = param2.tz();
			if ((dateTimeZone != null && timeTimeZone != null) && !dateTimeZone.StringValue.Equals(timeTimeZone.StringValue))
			{
			  // it's an error, if the arguments have different timezones
			  throw DynamicError.inconsistentTimeZone();
			}
			else if (dateTimeZone == null && timeTimeZone != null)
			{
			   return new XSDateTime(cal, timeTimeZone);
			}
			else if (dateTimeZone != null && timeTimeZone == null)
			{
			   return new XSDateTime(cal, dateTimeZone);
			}
			else if ((dateTimeZone != null && timeTimeZone != null) && dateTimeZone.StringValue.Equals(timeTimeZone.StringValue))
			{
			   return new XSDateTime(cal, dateTimeZone);
			}
			else
			{
			   return new XSDateTime(cal, null);
			}
		}

		/// <summary>
		/// Obtain a list of expected arguments.
		/// </summary>
		/// <returns> Result of operation. </returns>
		public static ICollection expected_args()
		{
			lock (typeof(FnDateTime))
			{
				if (_expected_args == null)
				{
					_expected_args = new ArrayList();
					_expected_args.Add(new SeqType(new XSDate(), SeqType.OCC_QMARK));
					_expected_args.Add(new SeqType(new XSTime(), SeqType.OCC_QMARK));
				}
        
				return _expected_args;
			}
		}
	}


}