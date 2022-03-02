using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmExecutionStrategy : IExecutionStrategy
	{
		private ExecutionStrategyDependencies Dependencies { get; }

		public virtual bool RetriesOnFailure => false;

		public DmExecutionStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
		{
			Dependencies = dependencies;
		}

		public virtual TResult Execute<TState, TResult>(TState state, Func<DbContext, TState, TResult> operation, Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded)
		{
			try
			{
				return operation(Dependencies.CurrentContext.Context, state);
			}
			catch (Exception ex) when (ExecutionStrategy.CallOnWrappedException(ex, DmTransientExceptionDetector.ShouldRetryOn))
			{
				throw new InvalidOperationException(DmStrings.TransientExceptionDetected, ex);
			}
		}

		public virtual async Task<TResult> ExecuteAsync<TState, TResult>(TState state, Func<DbContext, TState, CancellationToken, Task<TResult>> operation, Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded, CancellationToken cancellationToken)
		{
			try
			{
				return await operation(Dependencies.CurrentContext.Context, state, cancellationToken);
			}
			catch (Exception ex) when (ExecutionStrategy.CallOnWrappedException(ex, DmTransientExceptionDetector.ShouldRetryOn))
			{
				throw new InvalidOperationException(DmStrings.TransientExceptionDetected, ex);
			}
		}
	}
}
