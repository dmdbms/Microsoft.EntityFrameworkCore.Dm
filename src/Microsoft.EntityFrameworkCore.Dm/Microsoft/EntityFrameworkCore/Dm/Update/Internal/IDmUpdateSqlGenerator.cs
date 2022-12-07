// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Update.Internal.IDmUpdateSqlGenerator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;
using System.Collections.Generic;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Update.Internal
{
  public interface IDmUpdateSqlGenerator : IUpdateSqlGenerator
  {
    ResultSetMapping AppendBulkInsertOperation(
      [NotNull] StringBuilder commandStringBuilder,
      [NotNull] IReadOnlyList<IReadOnlyModificationCommand> modificationCommands,
      int commandPosition);
  }
}
