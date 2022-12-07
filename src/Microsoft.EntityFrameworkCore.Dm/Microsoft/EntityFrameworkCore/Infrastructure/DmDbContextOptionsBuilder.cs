using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
	public class DmDbContextOptionsBuilder : RelationalDbContextOptionsBuilder<DmDbContextOptionsBuilder, DmOptionsExtension>
	{
		public DmDbContextOptionsBuilder([NotNull] DbContextOptionsBuilder optionsBuilder)
			: base(optionsBuilder)
		{
		}

		public virtual DmDbContextOptionsBuilder EnableRetryOnFailure()
		{
			return ((RelationalDbContextOptionsBuilder<DmDbContextOptionsBuilder, DmOptionsExtension>)this).ExecutionStrategy((Func<ExecutionStrategyDependencies, IExecutionStrategy>)((ExecutionStrategyDependencies c) => (IExecutionStrategy)(object)new DmRetryingExecutionStrategy(c)));
		}

		public virtual DmDbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount)
		{
			return ((RelationalDbContextOptionsBuilder<DmDbContextOptionsBuilder, DmOptionsExtension>)this).ExecutionStrategy((Func<ExecutionStrategyDependencies, IExecutionStrategy>)((ExecutionStrategyDependencies c) => (IExecutionStrategy)(object)new DmRetryingExecutionStrategy(c, maxRetryCount)));
		}

		public virtual DmDbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount, TimeSpan maxRetryDelay, [NotNull] ICollection<int> errorNumbersToAdd)
		{
			ICollection<int> errorNumbersToAdd2 = errorNumbersToAdd;
			return ((RelationalDbContextOptionsBuilder<DmDbContextOptionsBuilder, DmOptionsExtension>)this).ExecutionStrategy((Func<ExecutionStrategyDependencies, IExecutionStrategy>)((ExecutionStrategyDependencies c) => (IExecutionStrategy)(object)new DmRetryingExecutionStrategy(c, maxRetryCount, maxRetryDelay, errorNumbersToAdd2)));
		}
	}
}
