// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.IDmRelationalConnection
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public interface IDmRelationalConnection : 
    IRelationalConnection,
    IRelationalTransactionManager,
    IDbContextTransactionManager,
    IResettableService,
    IDisposable,
    IAsyncDisposable
  {
    IDmRelationalConnection CreateMasterConnection();
  }
}
