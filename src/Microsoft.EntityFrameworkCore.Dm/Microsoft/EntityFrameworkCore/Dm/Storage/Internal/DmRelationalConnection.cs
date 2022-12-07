// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmRelationalConnection
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmRelationalConnection : 
    RelationalConnection,
    IDmRelationalConnection,
    IRelationalConnection,
    IRelationalTransactionManager,
    IDbContextTransactionManager,
    IResettableService,
    IDisposable,
    IAsyncDisposable
  {
    public const string EFPDBAdminUser = "SYSDBA";
    internal const int DefaultMasterConnectionCommandTimeout = 60;

    public DmRelationalConnection([NotNull] RelationalConnectionDependencies dependencies)
      : base(dependencies)
    {
    }

    protected override DbConnection CreateDbConnection() => (DbConnection) new DmConnection(this.ConnectionString, true);

    public virtual IDmRelationalConnection CreateMasterConnection()
    {
      DbContextOptions options = new DbContextOptionsBuilder().UseDm(((DbConnectionStringBuilder) new DmConnectionStringBuilder(this.ConnectionString)
      {
        User = "SYSDBA",
        Password = "SYSDBA"
      }).ConnectionString, (Action<DmDbContextOptionsBuilder>) (b => b.CommandTimeout(new int?(this.CommandTimeout ?? 60)))).Options;
      return (IDmRelationalConnection) new DmRelationalConnection(this.Dependencies with
      {
        ContextOptions = (IDbContextOptions) options
      });
    }

    protected override bool SupportsAmbientTransactions => false;
  }
}
