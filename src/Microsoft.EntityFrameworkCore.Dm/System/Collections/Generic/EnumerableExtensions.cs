using System.Diagnostics;
using System.Linq;

namespace System.Collections.Generic
{
	[DebuggerStepThrough]
	internal static class EnumerableExtensions
	{
		private sealed class DynamicEqualityComparer<T> : IEqualityComparer<T> where T : class
		{
			private readonly Func<T, T, bool> _func;

			public DynamicEqualityComparer(Func<T, T, bool> func)
			{
				_func = func;
			}

			public bool Equals(T x, T y)
			{
				return _func(x, y);
			}

			public int GetHashCode(T obj)
			{
				return 0;
			}
		}

		public static string Join(this IEnumerable<object> source, string separator = ", ")
		{
			return string.Join(separator, source);
		}

		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer) where T : class
		{
			return source.Distinct(new DynamicEqualityComparer<T>(comparer));
		}
	}
}
