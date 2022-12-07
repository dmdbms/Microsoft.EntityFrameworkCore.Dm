using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmCompiledQueryCacheKeyGenerator : RelationalCompiledQueryCacheKeyGenerator
	{
		private struct DmCompiledQueryCacheKey
		{
			private readonly RelationalCompiledQueryCacheKey _relationalCompiledQueryCacheKey;

			public DmCompiledQueryCacheKey(RelationalCompiledQueryCacheKey relationalCompiledQueryCacheKey)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_relationalCompiledQueryCacheKey = relationalCompiledQueryCacheKey;
			}

			public override bool Equals(object obj)
			{
				return obj != null && obj is DmCompiledQueryCacheKey && Equals((DmCompiledQueryCacheKey)obj);
			}

			private bool Equals(DmCompiledQueryCacheKey other)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				return ((RelationalCompiledQueryCacheKey)(_relationalCompiledQueryCacheKey)).Equals(other._relationalCompiledQueryCacheKey);
			}

			public override int GetHashCode()
			{
				return ((object)(RelationalCompiledQueryCacheKey)(_relationalCompiledQueryCacheKey)).GetHashCode();
			}
		}

		public DmCompiledQueryCacheKeyGenerator([NotNull] CompiledQueryCacheKeyGeneratorDependencies dependencies, [NotNull] RelationalCompiledQueryCacheKeyGeneratorDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
		}

		public override object GenerateCacheKey(Expression query, bool async)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return new DmCompiledQueryCacheKey(base.GenerateCacheKeyCore(query, async));
		}
	}
}
