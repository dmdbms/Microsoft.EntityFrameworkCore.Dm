using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microsoft.EntityFrameworkCore.Utilities
{
	[DebuggerStepThrough]
	internal static class Check
	{
		[JetBrains.Annotations.ContractAnnotation("value:null => halt")]
		public static T NotNull<T>([JetBrains.Annotations.NoEnumeration] T value, [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
		{
			if (value == null)
			{
				NotEmpty(parameterName, "parameterName");
				throw new ArgumentNullException(parameterName);
			}
			return value;
		}

		[JetBrains.Annotations.ContractAnnotation("value:null => halt")]
		public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
		{
			NotNull(value, parameterName);
			if (value.Count == 0)
			{
				NotEmpty(parameterName, "parameterName");
				throw new ArgumentException(AbstractionsStrings.CollectionArgumentIsEmpty(parameterName));
			}
			return value;
		}

		[JetBrains.Annotations.ContractAnnotation("value:null => halt")]
		public static string NotEmpty(string value, [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
		{
			Exception ex = null;
			if (value == null)
			{
				ex = new ArgumentNullException(parameterName);
			}
			else if (value.Trim().Length == 0)
			{
				ex = new ArgumentException(AbstractionsStrings.ArgumentIsEmpty(parameterName));
			}
			if (ex != null)
			{
				NotEmpty(parameterName, "parameterName");
				throw ex;
			}
			return value;
		}

		public static string NullButNotEmpty(string value, [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName)
		{
			if (value != null && value.Length == 0)
			{
				NotEmpty(parameterName, "parameterName");
				throw new ArgumentException(AbstractionsStrings.ArgumentIsEmpty(parameterName));
			}
			return value;
		}

		public static IReadOnlyList<T> HasNoNulls<T>(IReadOnlyList<T> value, [JetBrains.Annotations.InvokerParameterName][JetBrains.Annotations.NotNull] string parameterName) where T : class
		{
			NotNull(value, parameterName);
			if (value.Any((T e) => e == null))
			{
				NotEmpty(parameterName, "parameterName");
				throw new ArgumentException(parameterName);
			}
			return value;
		}
	}
}
