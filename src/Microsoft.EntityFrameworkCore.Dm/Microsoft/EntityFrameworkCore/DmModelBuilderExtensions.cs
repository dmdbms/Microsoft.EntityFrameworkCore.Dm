// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.DmModelBuilderExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;
using System;



namespace Microsoft.EntityFrameworkCore
{
  public static class DmModelBuilderExtensions
  {
    public static ModelBuilder UseHiLo(
      [NotNull] this ModelBuilder modelBuilder,
      [CanBeNull] string name = null,
      [CanBeNull] string schema = null)
    {
      Check.NotNull<ModelBuilder>(modelBuilder, nameof (modelBuilder));
      Check.NullButNotEmpty(name, nameof (name));
      Check.NullButNotEmpty(schema, nameof (schema));
      IMutableModel model = modelBuilder.Model;
      if (name == null)
        name = "EntityFrameworkHiLoSequence";
      if (RelationalModelExtensions.FindSequence(model, name, schema) == null)
        RelationalModelBuilderExtensions.HasSequence(modelBuilder, name, schema).IncrementsBy(10);
      model.SetValueGenerationStrategy(new DmValueGenerationStrategy?(DmValueGenerationStrategy.SequenceHiLo));
      model.SetHiLoSequenceName(name);
      model.SetHiLoSequenceSchema(schema);
      model.SetIdentitySeed(new int?());
      model.SetIdentityIncrement(new int?());
      return modelBuilder;
    }

    public static IConventionSequenceBuilder HasHiLoSequence(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string name,
      [CanBeNull] string schema,
      bool fromDataAnnotation = false)
    {
      if (!modelBuilder.CanSetHiLoSequence(name, schema))
        return (IConventionSequenceBuilder) null;
      modelBuilder.Metadata.SetHiLoSequenceName(name, fromDataAnnotation);
      modelBuilder.Metadata.SetHiLoSequenceSchema(schema, fromDataAnnotation);
      return name == null ? (IConventionSequenceBuilder) null : RelationalModelBuilderExtensions.HasSequence(modelBuilder, name, schema, fromDataAnnotation);
    }

    public static bool CanSetHiLoSequence(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string name,
      [CanBeNull] string schema,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionModelBuilder>(modelBuilder, nameof (modelBuilder));
      Check.NullButNotEmpty(name, nameof (name));
      Check.NullButNotEmpty(schema, nameof (schema));
      return ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:HiLoSequenceName", (object) name, fromDataAnnotation) && ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:HiLoSequenceSchema", (object) schema, fromDataAnnotation);
    }

    public static ModelBuilder UseIdentityColumns(
      [NotNull] this ModelBuilder modelBuilder,
      int seed = 1,
      int increment = 1)
    {
      Check.NotNull<ModelBuilder>(modelBuilder, nameof (modelBuilder));
      IMutableModel model = modelBuilder.Model;
      model.SetValueGenerationStrategy(new DmValueGenerationStrategy?(DmValueGenerationStrategy.IdentityColumn));
      model.SetIdentitySeed(new int?(seed));
      model.SetIdentityIncrement(new int?(increment));
      model.SetHiLoSequenceName((string) null);
      model.SetHiLoSequenceSchema((string) null);
      return modelBuilder;
    }

    public static IConventionModelBuilder HasIdentityColumnSeed(
      [NotNull] this IConventionModelBuilder modelBuilder,
      int? seed,
      bool fromDataAnnotation = false)
    {
      if (!modelBuilder.CanSetIdentityColumnSeed(seed, fromDataAnnotation))
        return (IConventionModelBuilder) null;
      modelBuilder.Metadata.SetIdentitySeed(seed, fromDataAnnotation);
      return modelBuilder;
    }

    public static bool CanSetIdentityColumnSeed(
      [NotNull] this IConventionModelBuilder modelBuilder,
      int? seed,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionModelBuilder>(modelBuilder, nameof (modelBuilder));
      return ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:IdentitySeed", (object) seed, fromDataAnnotation);
    }

    public static IConventionModelBuilder HasIdentityColumnIncrement(
      [NotNull] this IConventionModelBuilder modelBuilder,
      int? increment,
      bool fromDataAnnotation = false)
    {
      if (!modelBuilder.CanSetIdentityColumnIncrement(increment, fromDataAnnotation))
        return (IConventionModelBuilder) null;
      modelBuilder.Metadata.SetIdentityIncrement(increment, fromDataAnnotation);
      return modelBuilder;
    }

    public static bool CanSetIdentityColumnIncrement(
      [NotNull] this IConventionModelBuilder modelBuilder,
      int? increment,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionModelBuilder>(modelBuilder, nameof (modelBuilder));
      return ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:IdentityIncrement", (object) increment, fromDataAnnotation);
    }

    public static IConventionModelBuilder HasValueGenerationStrategy(
      [NotNull] this IConventionModelBuilder modelBuilder,
      DmValueGenerationStrategy? valueGenerationStrategy,
      bool fromDataAnnotation = false)
    {
      if (!modelBuilder.CanSetValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation))
        return (IConventionModelBuilder) null;
      modelBuilder.Metadata.SetValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
      DmValueGenerationStrategy? nullable1 = valueGenerationStrategy;
      DmValueGenerationStrategy generationStrategy1 = DmValueGenerationStrategy.IdentityColumn;
      if (!(nullable1.GetValueOrDefault() == generationStrategy1 & nullable1.HasValue))
      {
        modelBuilder.HasIdentityColumnSeed(new int?(), fromDataAnnotation);
        modelBuilder.HasIdentityColumnIncrement(new int?(), fromDataAnnotation);
      }
      DmValueGenerationStrategy? nullable2 = valueGenerationStrategy;
      DmValueGenerationStrategy generationStrategy2 = DmValueGenerationStrategy.SequenceHiLo;
      if (!(nullable2.GetValueOrDefault() == generationStrategy2 & nullable2.HasValue))
        modelBuilder.HasHiLoSequence((string) null, (string) null, fromDataAnnotation);
      return modelBuilder;
    }

    public static bool CanSetValueGenerationStrategy(
      [NotNull] this IConventionModelBuilder modelBuilder,
      DmValueGenerationStrategy? valueGenerationStrategy,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionModelBuilder>(modelBuilder, nameof (modelBuilder));
      return ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:ValueGenerationStrategy", (object) valueGenerationStrategy, fromDataAnnotation);
    }

    public static ModelBuilder HasDatabaseMaxSize(
      [NotNull] this ModelBuilder modelBuilder,
      [NotNull] string maxSize)
    {
      Check.NotNull<ModelBuilder>(modelBuilder, nameof (modelBuilder));
      Check.NotNull<string>(maxSize, nameof (maxSize));
      modelBuilder.Model.SetDatabaseMaxSize(maxSize);
      return modelBuilder;
    }

    public static IConventionModelBuilder HasDatabaseMaxSize(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string maxSize,
      bool fromDataAnnotation = false)
    {
      if (!modelBuilder.CanSetDatabaseMaxSize(maxSize, fromDataAnnotation))
        return (IConventionModelBuilder) null;
      modelBuilder.Metadata.SetDatabaseMaxSize(maxSize, fromDataAnnotation);
      return modelBuilder;
    }

    public static bool CanSetDatabaseMaxSize(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string maxSize,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionModelBuilder>(modelBuilder, nameof (modelBuilder));
      return ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:DatabaseMaxSize", (object) maxSize, fromDataAnnotation);
    }

    public static ModelBuilder HasServiceTier(
      [NotNull] this ModelBuilder modelBuilder,
      [NotNull] string serviceTier)
    {
      Check.NotNull<ModelBuilder>(modelBuilder, nameof (modelBuilder));
      Check.NotNull<string>(serviceTier, nameof (serviceTier));
      modelBuilder.Model.SetServiceTierSql("'" + serviceTier.Replace("'", "''") + "'");
      return modelBuilder;
    }

    public static ModelBuilder HasServiceTierSql(
      [NotNull] this ModelBuilder modelBuilder,
      [NotNull] string serviceTier)
    {
      Check.NotNull<ModelBuilder>(modelBuilder, nameof (modelBuilder));
      Check.NotNull<string>(serviceTier, nameof (serviceTier));
      modelBuilder.Model.SetServiceTierSql(serviceTier);
      return modelBuilder;
    }

    public static IConventionModelBuilder HasServiceTierSql(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string serviceTier,
      bool fromDataAnnotation = false)
    {
      if (!modelBuilder.CanSetServiceTierSql(serviceTier, fromDataAnnotation))
        return (IConventionModelBuilder) null;
      modelBuilder.Metadata.SetServiceTierSql(serviceTier, fromDataAnnotation);
      return modelBuilder;
    }

    public static bool CanSetServiceTierSql(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string serviceTier,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionModelBuilder>(modelBuilder, nameof (modelBuilder));
      return ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:ServiceTierSql", (object) serviceTier, fromDataAnnotation);
    }

    public static ModelBuilder HasPerformanceLevel(
      [NotNull] this ModelBuilder modelBuilder,
      [NotNull] string performanceLevel)
    {
      Check.NotNull<ModelBuilder>(modelBuilder, nameof (modelBuilder));
      Check.NotNull<string>(performanceLevel, nameof (performanceLevel));
      modelBuilder.Model.SetPerformanceLevelSql("'" + performanceLevel.Replace("'", "''") + "'");
      return modelBuilder;
    }

    public static ModelBuilder HasPerformanceLevelSql(
      [NotNull] this ModelBuilder modelBuilder,
      [NotNull] string performanceLevel)
    {
      Check.NotNull<ModelBuilder>(modelBuilder, nameof (modelBuilder));
      Check.NotNull<string>(performanceLevel, nameof (performanceLevel));
      modelBuilder.Model.SetPerformanceLevelSql(performanceLevel);
      return modelBuilder;
    }

    public static IConventionModelBuilder HasPerformanceLevelSql(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string performanceLevel,
      bool fromDataAnnotation = false)
    {
      if (!modelBuilder.CanSetPerformanceLevelSql(performanceLevel, fromDataAnnotation))
        return (IConventionModelBuilder) null;
      modelBuilder.Metadata.SetPerformanceLevelSql(performanceLevel, fromDataAnnotation);
      return modelBuilder;
    }

    public static bool CanSetPerformanceLevelSql(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string performanceLevel,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionModelBuilder>(modelBuilder, nameof (modelBuilder));
      return ((IConventionAnnotatableBuilder) modelBuilder).CanSetAnnotation("Dm:PerformanceLevelSql", (object) performanceLevel, fromDataAnnotation);
    }

    [Obsolete("Use UseHiLo")]
    public static ModelBuilder ForSqlServerUseSequenceHiLo(
      [NotNull] this ModelBuilder modelBuilder,
      [CanBeNull] string name = null,
      [CanBeNull] string schema = null)
    {
      return modelBuilder.UseHiLo(name, schema);
    }

    [Obsolete("Use HasHiLoSequence")]
    public static IConventionSequenceBuilder ForSqlServerHasHiLoSequence(
      [NotNull] this IConventionModelBuilder modelBuilder,
      [CanBeNull] string name,
      [CanBeNull] string schema,
      bool fromDataAnnotation = false)
    {
      return modelBuilder.HasHiLoSequence(name, schema, fromDataAnnotation);
    }

    [Obsolete("Use UseIdentityColumns")]
    public static ModelBuilder ForSqlServerUseIdentityColumns(
      [NotNull] this ModelBuilder modelBuilder,
      int seed = 1,
      int increment = 1)
    {
      return modelBuilder.UseIdentityColumns(seed, increment);
    }

    [Obsolete("Use HasIdentityColumnSeed")]
    public static IConventionModelBuilder ForSqlServerHasIdentitySeed(
      [NotNull] this IConventionModelBuilder modelBuilder,
      int? seed,
      bool fromDataAnnotation = false)
    {
      return modelBuilder.HasIdentityColumnSeed(seed, fromDataAnnotation);
    }

    [Obsolete("Use HasIdentityColumnIncrement")]
    public static IConventionModelBuilder ForSqlServerHasIdentityIncrement(
      [NotNull] this IConventionModelBuilder modelBuilder,
      int? increment,
      bool fromDataAnnotation = false)
    {
      return modelBuilder.HasIdentityColumnIncrement(increment, fromDataAnnotation);
    }

    [Obsolete("Use HasValueGenerationStrategy")]
    public static IConventionModelBuilder ForSqlServerHasValueGenerationStrategy(
      [NotNull] this IConventionModelBuilder modelBuilder,
      DmValueGenerationStrategy? valueGenerationStrategy,
      bool fromDataAnnotation = false)
    {
      return modelBuilder.HasValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
    }
  }
}
