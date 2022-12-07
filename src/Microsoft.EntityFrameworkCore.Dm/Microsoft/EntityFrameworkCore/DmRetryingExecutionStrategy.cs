// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.DmRetryingExecutionStrategy
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;



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
      : this(context, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, (ICollection<int>) null)
    {
    }

    public DmRetryingExecutionStrategy(
      [NotNull] ExecutionStrategyDependencies dependencies,
      int maxRetryCount)
      : this(dependencies, maxRetryCount, ExecutionStrategy.DefaultMaxDelay, (ICollection<int>) null)
    {
    }

    public DmRetryingExecutionStrategy(
      [NotNull] DbContext context,
      int maxRetryCount,
      TimeSpan maxRetryDelay,
      [CanBeNull] ICollection<int> errorNumbersToAdd)
      : base(context, maxRetryCount, maxRetryDelay)
    {
      this._additionalErrorNumbers = errorNumbersToAdd;
    }

    public DmRetryingExecutionStrategy(
      [NotNull] ExecutionStrategyDependencies dependencies,
      int maxRetryCount,
      TimeSpan maxRetryDelay,
      [CanBeNull] ICollection<int> errorNumbersToAdd)
      : base(dependencies, maxRetryCount, maxRetryDelay)
    {
      this._additionalErrorNumbers = errorNumbersToAdd;
    }

    protected override bool ShouldRetryOn(Exception exception) => this._additionalErrorNumbers != null && exception is DmException dmException && this._additionalErrorNumbers.Contains(((ExternalException) dmException).ErrorCode) || DmTransientExceptionDetector.ShouldRetryOn(exception);

    protected override TimeSpan? GetNextDelay(Exception lastException)
    {
      TimeSpan? nextDelay = base.GetNextDelay(lastException);
      if (!nextDelay.HasValue)
        return new TimeSpan?();
      return ExecutionStrategy.CallOnWrappedException<bool>(lastException, new Func<Exception, bool>(this.IsMemoryOptimizedError)) ? new TimeSpan?(TimeSpan.FromMilliseconds(nextDelay.Value.TotalSeconds)) : nextDelay;
    }

    private bool IsMemoryOptimizedError(Exception exception) => false;
  }
}
