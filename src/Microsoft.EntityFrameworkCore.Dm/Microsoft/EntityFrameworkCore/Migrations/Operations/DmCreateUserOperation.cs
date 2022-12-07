// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Migrations.Operations.DmCreateUserOperation
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;



namespace Microsoft.EntityFrameworkCore.Migrations.Operations
{
  public class DmCreateUserOperation : MigrationOperation
  {
    public virtual string UserName { get; [param: NotNull] set; }

    public virtual string Password { get; [param: NotNull] set; }
  }
}
