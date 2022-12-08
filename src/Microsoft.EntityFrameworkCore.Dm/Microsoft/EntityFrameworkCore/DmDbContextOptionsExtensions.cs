using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class DmDbContextOptionsExtensions
{
    public static DbContextOptionsBuilder UseDm([NotNull] this DbContextOptionsBuilder optionsBuilder, [NotNull] string connectionString, [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
    {
        Check.NotNull<DbContextOptionsBuilder>(optionsBuilder, "optionsBuilder");
        Check.NotEmpty(connectionString, "connectionString");
        DmOptionsExtension dmOptionsExtension = (DmOptionsExtension)(object)((RelationalOptionsExtension)GetOrCreateExtension(optionsBuilder)).WithConnectionString(connectionString);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension<DmOptionsExtension>(dmOptionsExtension);
        ConfigureWarnings(optionsBuilder);
        dmOptionsAction?.Invoke(new DmDbContextOptionsBuilder(optionsBuilder));
        return optionsBuilder;
    }

    public static DbContextOptionsBuilder UseDm([NotNull] this DbContextOptionsBuilder optionsBuilder, [NotNull] DbConnection connection, [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
    {
        Check.NotNull<DbContextOptionsBuilder>(optionsBuilder, "optionsBuilder");
        Check.NotNull(connection, "connection");
        DmOptionsExtension dmOptionsExtension = (DmOptionsExtension)(object)((RelationalOptionsExtension)GetOrCreateExtension(optionsBuilder)).WithConnection(connection);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension<DmOptionsExtension>(dmOptionsExtension);
        ConfigureWarnings(optionsBuilder);
        dmOptionsAction?.Invoke(new DmDbContextOptionsBuilder(optionsBuilder));
        return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseDm<TContext>([NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [NotNull] string connectionString, [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null) where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)(object)((DbContextOptionsBuilder)(object)optionsBuilder).UseDm(connectionString, dmOptionsAction);
    }

    public static DbContextOptionsBuilder<TContext> UseDm<TContext>([NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [NotNull] DbConnection connection, [CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null) where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)(object)((DbContextOptionsBuilder)(object)optionsBuilder).UseDm(connection, dmOptionsAction);
    }

    private static DmOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.Options.FindExtension<DmOptionsExtension>() ?? new DmOptionsExtension();
    }

    private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
    {
        //IL_0010: Unknown result type (might be due to invalid IL or missing references)
        CoreOptionsExtension val = (CoreOptionsExtension)(((object)optionsBuilder.Options.FindExtension<CoreOptionsExtension>()) ?? ((object)new CoreOptionsExtension()));
        val = val.WithWarningsConfiguration(val.WarningsConfiguration.TryWithExplicit(RelationalEventId.AmbientTransactionWarning, (WarningBehavior)2));
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension<CoreOptionsExtension>(val);
    }
}
