// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmTransaction
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmTransaction : RelationalTransaction
  {
    private static readonly bool _useOldBehavior;

    public DmTransaction(
      IRelationalConnection connection,
      DbTransaction transaction,
      Guid transactionId,
      IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> logger,
      bool transactionOwned,
      ISqlGenerationHelper sqlGenerationHelper)
      : base(connection, transaction, transactionId, logger, transactionOwned, sqlGenerationHelper)
    {
    }

    public virtual void ReleaseSavepoint(string name)
    {
    }

    static DmTransaction()
    {
      bool isEnabled;
      DmTransaction._useOldBehavior = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue23305", out isEnabled) & isEnabled;
    }
  }
}
