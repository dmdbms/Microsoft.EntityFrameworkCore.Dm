using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore
{
	public class DmRetryingExecutionStrategy : ExecutionStrategy
	{
		private readonly ICollection<int> _additionalErrorNumbers;

		public DmRetryingExecutionStrategy([NotNull] DbContext context)
			: this(context, ExecutionStrategy.DefaultMaxRetryCount)
		{
		}

		public DmRetryingExecutionStrategy([NotNull] ExecutionStrategyDependencies dependencies)
			: this(dependencies, ExecutionStrategy.DefaultMaxRetryCount)
		{
		}

		public DmRetryingExecutionStrategy([NotNull] DbContext context, int maxRetryCount)
			: this(context, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, null)
		{
		}

		public DmRetryingExecutionStrategy([NotNull] ExecutionStrategyDependencies dependencies, int maxRetryCount)
			: this(dependencies, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, null)
		{
		}

		public DmRetryingExecutionStrategy([NotNull] DbContext context, int maxRetryCount, TimeSpan maxRetryDelay, [CanBeNull] ICollection<int> errorNumbersToAdd)
			: base(context, maxRetryCount, maxRetryDelay)
		{
			_additionalErrorNumbers = errorNumbersToAdd;
		}

		public DmRetryingExecutionStrategy([NotNull] ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay, [CanBeNull] ICollection<int> errorNumbersToAdd)
			: base(dependencies, maxRetryCount, maxRetryDelay)
		{
			_additionalErrorNumbers = errorNumbersToAdd;
		}

		protected override bool ShouldRetryOn(Exception exception)
		{
			if (_additionalErrorNumbers != null)
			{
				DmException val = (DmException)(object)((exception is DmException) ? exception : null);
				if (val != null && _additionalErrorNumbers.Contains(((ExternalException)(object)val).ErrorCode))
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
			if (ExecutionStrategy.CallOnWrappedException<bool>(lastException, (Func<Exception, bool>)IsMemoryOptimizedError))
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
