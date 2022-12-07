// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmExecutionStrategy
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmExecutionStrategy : IExecutionStrategy
  {
    private ExecutionStrategyDependencies Dependencies { get; }

    public DmExecutionStrategy([NotNull] ExecutionStrategyDependencies dependencies) => this.Dependencies = dependencies;

    public virtual bool RetriesOnFailure => false;

    public virtual TResult Execute<TState, TResult>(
      TState state,
      Func<DbContext, TState, TResult> operation,
      Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded)
    {
      try
      {
        return operation(this.Dependencies.CurrentContext.Context, state);
      }
      catch (Exception ex) when (ExecutionStrategy.CallOnWrappedException<bool>(ex, new Func<Exception, bool>(DmTransientExceptionDetector.ShouldRetryOn)))
      {
        throw new InvalidOperationException(DmStrings.TransientExceptionDetected, ex);
      }
    }

    public virtual async Task<TResult> ExecuteAsync<TState, TResult>(
      TState state,
      Func<DbContext, TState, CancellationToken, Task<TResult>> operation,
      Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded,
      CancellationToken cancellationToken)
    {
      TResult result;
      try
      {
        result = await operation(this.Dependencies.CurrentContext.Context, state, cancellationToken);
      }
      catch (Exception ex) when (ExecutionStrategy.CallOnWrappedException<bool>(ex, new Func<Exception, bool>(DmTransientExceptionDetector.ShouldRetryOn)))
      {
        throw new InvalidOperationException(DmStrings.TransientExceptionDetected, ex);
      }
      return result;
    }
  }
}
