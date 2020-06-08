using System;

/// <summary>
///*****************************************************************************
/// Copyright (c) 2009, 2011 Standards for Technology in Automotive Retail and others.
/// All rights reserved. This program and the accompanying materials
/// are made available under the terms of the Eclipse Public License 2.0
/// which accompanies this distribution, and is available at
/// https://www.eclipse.org/legal/epl-2.0/
/// 
/// SPDX-License-Identifier: EPL-2.0
/// 
/// Contributors:
///     David Carver - bug 262765 - initial API and implementation
///     Mukul Gandhi - bug 280798 - PsychoPath support for JDK 1.4
/// ******************************************************************************
/// </summary>
namespace org.eclipse.wst.xml.xpath2.processor.@internal.function
{


	using QName = org.eclipse.wst.xml.xpath2.processor.@internal.types.QName;

	public abstract class AbstractRegExFunction : Function
	{
		protected internal const string validflags = "smix";

		public AbstractRegExFunction(QName name, int arity) : base(name, arity)
		{
		}

		public AbstractRegExFunction(QName name, int min_arity, int max_arity) : base(name, min_arity, max_arity)
		{
		}

		protected internal static bool matches(string pattern, string flags, string src)
		{
			bool fnd = false;
			if (pattern.IndexOf("-[", StringComparison.Ordinal) != -1)
			{
				pattern = pattern.replaceAll("\\-\\[", "&&[^");
			}
			Matcher m = compileAndExecute(pattern, flags, src);
			while (m.find())
			{
				fnd = true;
			}
			return fnd;
		}

		protected internal static Matcher regex(string pattern, string flags, string src)
		{
			Matcher matcher = compileAndExecute(pattern, flags, src);
			return matcher;
		}

		private static Matcher compileAndExecute(string pattern, string flags, string src)
		{
			int flag = Pattern.UNIX_LINES;
			if (!string.ReferenceEquals(flags, null))
			{
				if (flags.IndexOf("m", StringComparison.Ordinal) >= 0)
				{
					flag = flag | Pattern.MULTILINE;
				}
				if (flags.IndexOf("s", StringComparison.Ordinal) >= 0)
				{
					flag = flag | Pattern.DOTALL;
				}
				if (flags.IndexOf("i", StringComparison.Ordinal) >= 0)
				{
					flag = flag | Pattern.CASE_INSENSITIVE;
				}

				if (flags.IndexOf("x", StringComparison.Ordinal) >= 0)
				{
					flag = flag | Pattern.COMMENTS;
				}
			}

			Pattern p = Pattern.compile(pattern, flag);
			return p.matcher(src);
		}

	}

}