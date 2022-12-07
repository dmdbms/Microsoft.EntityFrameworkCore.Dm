// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmExecutionStrategyFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmExecutionStrategyFactory : RelationalExecutionStrategyFactory
  {
    public DmExecutionStrategyFactory([NotNull] ExecutionStrategyDependencies dependencies)
      : base(dependencies)
    {
    }

    protected override IExecutionStrategy CreateDefaultStrategy(
      ExecutionStrategyDependencies dependencies)
    {
      return (IExecutionStrategy) new DmExecutionStrategy(dependencies);
    }
  }
}
