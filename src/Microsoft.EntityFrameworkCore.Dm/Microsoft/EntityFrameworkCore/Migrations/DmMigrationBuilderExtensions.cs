﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Migrations.DmMigrationBuilderExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using System;
using System.Reflection;



namespace Microsoft.EntityFrameworkCore.Migrations
{
  public static class DmMigrationBuilderExtensions
  {
    public static bool IsDm([NotNull] this MigrationBuilder migrationBuilder) => string.Equals(migrationBuilder.ActiveProvider, typeof (DmOptionsExtension).GetTypeInfo().Assembly.GetName().Name, StringComparison.Ordinal);
  }
}
