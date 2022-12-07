// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Infrastructure.DmDbContextOptionsBuilder
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;



namespace Microsoft.EntityFrameworkCore.Infrastructure
{
  public class DmDbContextOptionsBuilder : 
    RelationalDbContextOptionsBuilder<DmDbContextOptionsBuilder, DmOptionsExtension>
  {
    public DmDbContextOptionsBuilder([NotNull] DbContextOptionsBuilder optionsBuilder)
      : base(optionsBuilder)
    {
    }

    public virtual DmDbContextOptionsBuilder EnableRetryOnFailure() => this.ExecutionStrategy((Func<ExecutionStrategyDependencies, IExecutionStrategy>) (c => (IExecutionStrategy) new DmRetryingExecutionStrategy(c)));

    public virtual DmDbContextOptionsBuilder EnableRetryOnFailure(
      int maxRetryCount)
    {
      return this.ExecutionStrategy((Func<ExecutionStrategyDependencies, IExecutionStrategy>) (c => (IExecutionStrategy) new DmRetryingExecutionStrategy(c, maxRetryCount)));
    }

    public virtual DmDbContextOptionsBuilder EnableRetryOnFailure(
      int maxRetryCount,
      TimeSpan maxRetryDelay,
      [NotNull] ICollection<int> errorNumbersToAdd)
    {
      return this.ExecutionStrategy((Func<ExecutionStrategyDependencies, IExecutionStrategy>) (c => (IExecutionStrategy) new DmRetryingExecutionStrategy(c, maxRetryCount, maxRetryDelay, errorNumbersToAdd)));
    }
  }
}
