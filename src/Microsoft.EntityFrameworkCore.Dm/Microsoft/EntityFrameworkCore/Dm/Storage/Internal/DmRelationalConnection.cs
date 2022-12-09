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

        protected override DbConnection CreateDbConnection() {
            DmConnection conn = new DmConnection(this.ConnectionString, true);
            //在HistoryRepository.Exists()中，如果没有提前打开Connection，而是在ExecuteScalar之前才打开，
            //那么执行ExecuteScalar的时候就会报异常“Source array was not long enough.”
            //估计是DmProvider的Bug，不好定位和修复，所以这里就强制创建Connection就打开连接吧
            conn.Open();
            return conn;
    }

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
