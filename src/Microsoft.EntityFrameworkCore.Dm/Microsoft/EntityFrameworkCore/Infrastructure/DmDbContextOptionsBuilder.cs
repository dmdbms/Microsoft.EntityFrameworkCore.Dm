using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
	public class DmDbContextOptionsBuilder : RelationalDbContextOptionsBuilder<DmDbContextOptionsBuilder, DmOptionsExtension>
	{
		public DmDbContextOptionsBuilder([JetBrains.Annotations.NotNull] DbContextOptionsBuilder optionsBuilder)
			: base(optionsBuilder)
		{
		}

		public virtual DmDbContextOptionsBuilder EnableRetryOnFailure()
		{
			return ExecutionStrategy((ExecutionStrategyDependencies c) => new DmRetryingExecutionStrategy(c));
		}

		public virtual DmDbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount)
		{
			return ExecutionStrategy((ExecutionStrategyDependencies c) => new DmRetryingExecutionStrategy(c, maxRetryCount));
		}

		public virtual DmDbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount, TimeSpan maxRetryDelay, [JetBrains.Annotations.NotNull] ICollection<int> errorNumbersToAdd)
		{
			return ExecutionStrategy((ExecutionStrategyDependencies c) => new DmRetryingExecutionStrategy(c, maxRetryCount, maxRetryDelay, errorNumbersToAdd));
		}
	}
}
