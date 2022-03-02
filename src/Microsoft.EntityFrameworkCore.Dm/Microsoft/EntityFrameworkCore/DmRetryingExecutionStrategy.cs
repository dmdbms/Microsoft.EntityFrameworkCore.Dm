using System;
using System.Collections.Generic;
using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore
{
	public class DmRetryingExecutionStrategy : ExecutionStrategy
	{
		private readonly ICollection<int> _additionalErrorNumbers;

		public DmRetryingExecutionStrategy([JetBrains.Annotations.NotNull] DbContext context)
			: this(context, ExecutionStrategy.DefaultMaxRetryCount)
		{
		}

		public DmRetryingExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
			: this(dependencies, ExecutionStrategy.DefaultMaxRetryCount)
		{
		}

		public DmRetryingExecutionStrategy([JetBrains.Annotations.NotNull] DbContext context, int maxRetryCount)
			: this(context, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, null)
		{
		}

		public DmRetryingExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies, int maxRetryCount)
			: this(dependencies, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, null)
		{
		}

		public DmRetryingExecutionStrategy([JetBrains.Annotations.NotNull] DbContext context, int maxRetryCount, TimeSpan maxRetryDelay, [JetBrains.Annotations.CanBeNull] ICollection<int> errorNumbersToAdd)
			: base(context, maxRetryCount, maxRetryDelay)
		{
			_additionalErrorNumbers = errorNumbersToAdd;
		}

		public DmRetryingExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay, [JetBrains.Annotations.CanBeNull] ICollection<int> errorNumbersToAdd)
			: base(dependencies, maxRetryCount, maxRetryDelay)
		{
			_additionalErrorNumbers = errorNumbersToAdd;
		}

		protected override bool ShouldRetryOn(Exception exception)
		{
			if (_additionalErrorNumbers != null)
			{
				DmException ex = exception as DmException;
				if (ex != null && _additionalErrorNumbers.Contains(ex.ErrorCode))
				{
					return true;
				}
			}
			return DmTransientExceptionDetector.ShouldRetryOn(exception);
		}

		protected override TimeSpan? GetNextDelay(Exception lastException)
		{
			TimeSpan? nextDelay = base.GetNextDelay(lastException);
			if (!nextDelay.HasValue)
			{
				return null;
			}
			if (ExecutionStrategy.CallOnWrappedException(lastException, IsMemoryOptimizedError))
			{
				return TimeSpan.FromMilliseconds(nextDelay.Value.TotalSeconds);
			}
			return nextDelay;
		}

		private bool IsMemoryOptimizedError(Exception exception)
		{
			return false;
		}
	}
}
