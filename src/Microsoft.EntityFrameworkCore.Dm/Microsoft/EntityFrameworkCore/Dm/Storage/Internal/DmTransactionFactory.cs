// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmTransactionFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Data.Common;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmTransactionFactory : IRelationalTransactionFactory
  {
    public DmTransactionFactory(
      RelationalTransactionFactoryDependencies dependencies)
    {
      Check.NotNull<RelationalTransactionFactoryDependencies>(dependencies, nameof (dependencies));
      this.Dependencies = dependencies;
    }

    protected virtual RelationalTransactionFactoryDependencies Dependencies { get; }

    public virtual RelationalTransaction Create(
      IRelationalConnection connection,
      DbTransaction transaction,
      Guid transactionId,
      IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> logger,
      bool transactionOwned)
    {
      return (RelationalTransaction) new DmTransaction(connection, transaction, transactionId, logger, transactionOwned, this.Dependencies.SqlGenerationHelper);
    }
  }
}
