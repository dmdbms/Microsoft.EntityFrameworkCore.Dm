// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Update.Internal.DmModificationCommandBatchFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;
using System.Collections;
using System.Linq;



namespace Microsoft.EntityFrameworkCore.Dm.Update.Internal
{
  public class DmModificationCommandBatchFactory : IModificationCommandBatchFactory
  {
    private readonly ModificationCommandBatchFactoryDependencies _dependencies;
    private readonly IDbContextOptions _options;

    public DmModificationCommandBatchFactory(
      [NotNull] ModificationCommandBatchFactoryDependencies dependencies,
      [NotNull] IDbContextOptions options)
    {
      Check.NotNull<ModificationCommandBatchFactoryDependencies>(dependencies, nameof (dependencies));
      Check.NotNull<IDbContextOptions>(options, nameof (options));
      this._dependencies = dependencies;
      this._options = options;
    }

    public virtual ModificationCommandBatch Create() => (ModificationCommandBatch) new DmModificationCommandBatch(this._dependencies, (int?) ((IEnumerable) this._options.Extensions).OfType<DmOptionsExtension>().FirstOrDefault<DmOptionsExtension>()?.MaxBatchSize);
  }
}
