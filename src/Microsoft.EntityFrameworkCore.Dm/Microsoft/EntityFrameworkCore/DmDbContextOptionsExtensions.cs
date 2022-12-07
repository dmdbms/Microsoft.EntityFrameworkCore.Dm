// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.DmDbContextOptionsExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Data.Common;



namespace Microsoft.EntityFrameworkCore
{
  public static class DmDbContextOptionsExtensions
  {
    public static DbContextOptionsBuilder UseDm(
      [NotNull] this DbContextOptionsBuilder optionsBuilder,
      [NotNull] string connectionString,
      [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
    {
      Check.NotNull<DbContextOptionsBuilder>(optionsBuilder, nameof (optionsBuilder));
      Check.NotEmpty(connectionString, nameof (connectionString));
      DmOptionsExtension optionsExtension = (DmOptionsExtension) DmDbContextOptionsExtensions.GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
      ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension<DmOptionsExtension>(optionsExtension);
      DmDbContextOptionsExtensions.ConfigureWarnings(optionsBuilder);
      if (dmOptionsAction != null)
        dmOptionsAction(new DmDbContextOptionsBuilder(optionsBuilder));
      return optionsBuilder;
    }

    public static DbContextOptionsBuilder UseDm(
      [NotNull] this DbContextOptionsBuilder optionsBuilder,
      [NotNull] DbConnection connection,
      [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
    {
      Check.NotNull<DbContextOptionsBuilder>(optionsBuilder, nameof (optionsBuilder));
      Check.NotNull<DbConnection>(connection, nameof (connection));
      DmOptionsExtension optionsExtension = (DmOptionsExtension) DmDbContextOptionsExtensions.GetOrCreateExtension(optionsBuilder).WithConnection(connection);
      ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension<DmOptionsExtension>(optionsExtension);
      DmDbContextOptionsExtensions.ConfigureWarnings(optionsBuilder);
      if (dmOptionsAction != null)
        dmOptionsAction(new DmDbContextOptionsBuilder(optionsBuilder));
      return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseDm<TContext>(
      [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
      [NotNull] string connectionString,
      [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
      where TContext : DbContext
    {
      return (DbContextOptionsBuilder<TContext>) ((DbContextOptionsBuilder) optionsBuilder).UseDm(connectionString, dmOptionsAction);
    }

    public static DbContextOptionsBuilder<TContext> UseDm<TContext>(
      [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
      [NotNull] DbConnection connection,
      [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
      where TContext : DbContext
    {
      return (DbContextOptionsBuilder<TContext>) ((DbContextOptionsBuilder) optionsBuilder).UseDm(connection, dmOptionsAction);
    }

    private static DmOptionsExtension GetOrCreateExtension(
      DbContextOptionsBuilder optionsBuilder)
    {
      return optionsBuilder.Options.FindExtension<DmOptionsExtension>() ?? new DmOptionsExtension();
    }

    private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
    {
      CoreOptionsExtension optionsExtension1 = optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
      CoreOptionsExtension optionsExtension2 = optionsExtension1.WithWarningsConfiguration(optionsExtension1.WarningsConfiguration.TryWithExplicit(RelationalEventId.AmbientTransactionWarning, (WarningBehavior) 2));
      ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension<CoreOptionsExtension>(optionsExtension2);
    }
  }
}
