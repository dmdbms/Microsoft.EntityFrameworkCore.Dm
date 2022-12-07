// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.DmPropertyBuilderExtensions
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
  public static class DmPropertyBuilderExtensions
  {
    public static PropertyBuilder UseHiLo(
      [NotNull] this PropertyBuilder propertyBuilder,
      [CanBeNull] string name = null,
      [CanBeNull] string schema = null)
    {
      Check.NotNull<PropertyBuilder>(propertyBuilder, nameof (propertyBuilder));
      Check.NullButNotEmpty(name, nameof (name));
      Check.NullButNotEmpty(schema, nameof (schema));
      IMutableProperty metadata = propertyBuilder.Metadata;
      if (name == null)
        name = "EntityFrameworkHiLoSequence";
      IMutableModel model = metadata.DeclaringEntityType.Model;
      if (RelationalModelExtensions.FindSequence(model, name, schema) == null)
        RelationalModelExtensions.AddSequence(model, name, schema).IncrementBy = 10;
      metadata.SetValueGenerationStrategy(new DmValueGenerationStrategy?(DmValueGenerationStrategy.SequenceHiLo));
      metadata.SetHiLoSequenceName(name);
      metadata.SetHiLoSequenceSchema(schema);
      metadata.SetIdentitySeed(new int?());
      metadata.SetIdentityIncrement(new int?());
      return propertyBuilder;
    }

    public static PropertyBuilder<TProperty> UseHiLo<TProperty>(
      [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
      [CanBeNull] string name = null,
      [CanBeNull] string schema = null)
    {
      return (PropertyBuilder<TProperty>) ((PropertyBuilder) propertyBuilder).UseHiLo(name, schema);
    }

    public static IConventionSequenceBuilder HasHiLoSequence(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      [CanBeNull] string name,
      [CanBeNull] string schema,
      bool fromDataAnnotation = false)
    {
      if (!propertyBuilder.CanSetHiLoSequence(name, schema, fromDataAnnotation))
        return (IConventionSequenceBuilder) null;
      propertyBuilder.Metadata.SetHiLoSequenceName(name, fromDataAnnotation);
      propertyBuilder.Metadata.SetHiLoSequenceSchema(schema, fromDataAnnotation);
      return name == null ? (IConventionSequenceBuilder) null : RelationalModelBuilderExtensions.HasSequence(propertyBuilder.Metadata.DeclaringEntityType.Model.Builder, name, schema, fromDataAnnotation);
    }

    public static bool CanSetHiLoSequence(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      [CanBeNull] string name,
      [CanBeNull] string schema,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionPropertyBuilder>(propertyBuilder, nameof (propertyBuilder));
      Check.NullButNotEmpty(name, nameof (name));
      Check.NullButNotEmpty(schema, nameof (schema));
      return ((IConventionAnnotatableBuilder) propertyBuilder).CanSetAnnotation("Dm:HiLoSequenceName", (object) name, fromDataAnnotation) && ((IConventionAnnotatableBuilder) propertyBuilder).CanSetAnnotation("Dm:HiLoSequenceSchema", (object) schema, fromDataAnnotation);
    }

    public static PropertyBuilder UseIdentityColumn(
      [NotNull] this PropertyBuilder propertyBuilder,
      int seed = 1,
      int increment = 1)
    {
      Check.NotNull<PropertyBuilder>(propertyBuilder, nameof (propertyBuilder));
      IMutableProperty metadata = propertyBuilder.Metadata;
      metadata.SetValueGenerationStrategy(new DmValueGenerationStrategy?(DmValueGenerationStrategy.IdentityColumn));
      metadata.SetIdentitySeed(new int?(seed));
      metadata.SetIdentityIncrement(new int?(increment));
      metadata.SetHiLoSequenceName((string) null);
      metadata.SetHiLoSequenceSchema((string) null);
      return propertyBuilder;
    }

    public static PropertyBuilder<TProperty> UseIdentityColumn<TProperty>(
      [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
      int seed = 1,
      int increment = 1)
    {
      return (PropertyBuilder<TProperty>) ((PropertyBuilder) propertyBuilder).UseIdentityColumn(seed, increment);
    }

    public static IConventionPropertyBuilder HasIdentityColumnSeed(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      int? seed,
      bool fromDataAnnotation = false)
    {
      if (!propertyBuilder.CanSetIdentityColumnSeed(seed, fromDataAnnotation))
        return (IConventionPropertyBuilder) null;
      propertyBuilder.Metadata.SetIdentitySeed(seed, fromDataAnnotation);
      return propertyBuilder;
    }

    public static bool CanSetIdentityColumnSeed(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      int? seed,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionPropertyBuilder>(propertyBuilder, nameof (propertyBuilder));
      return ((IConventionAnnotatableBuilder) propertyBuilder).CanSetAnnotation("Dm:IdentitySeed", (object) seed, fromDataAnnotation);
    }

    public static IConventionPropertyBuilder HasIdentityColumnIncrement(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      int? increment,
      bool fromDataAnnotation = false)
    {
      if (!propertyBuilder.CanSetIdentityColumnIncrement(increment, fromDataAnnotation))
        return (IConventionPropertyBuilder) null;
      propertyBuilder.Metadata.SetIdentityIncrement(increment, fromDataAnnotation);
      return propertyBuilder;
    }

    public static bool CanSetIdentityColumnIncrement(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      int? increment,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionPropertyBuilder>(propertyBuilder, nameof (propertyBuilder));
      return ((IConventionAnnotatableBuilder) propertyBuilder).CanSetAnnotation("Dm:IdentityIncrement", (object) increment, fromDataAnnotation);
    }

    public static IConventionPropertyBuilder HasValueGenerationStrategy(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      DmValueGenerationStrategy? valueGenerationStrategy,
      bool fromDataAnnotation = false)
    {
      if (!((IConventionAnnotatableBuilder) propertyBuilder).CanSetAnnotation("Dm:ValueGenerationStrategy", (object) valueGenerationStrategy, fromDataAnnotation))
        return (IConventionPropertyBuilder) null;
      propertyBuilder.Metadata.SetValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
      DmValueGenerationStrategy? nullable = valueGenerationStrategy;
      DmValueGenerationStrategy generationStrategy1 = DmValueGenerationStrategy.IdentityColumn;
      if (!(nullable.GetValueOrDefault() == generationStrategy1 & nullable.HasValue))
      {
        propertyBuilder.HasIdentityColumnSeed(new int?(), fromDataAnnotation);
        propertyBuilder.HasIdentityColumnIncrement(new int?(), fromDataAnnotation);
      }
      nullable = valueGenerationStrategy;
      DmValueGenerationStrategy generationStrategy2 = DmValueGenerationStrategy.SequenceHiLo;
      if (!(nullable.GetValueOrDefault() == generationStrategy2 & nullable.HasValue))
        propertyBuilder.HasHiLoSequence((string) null, (string) null, fromDataAnnotation);
      return propertyBuilder;
    }

    public static bool CanSetValueGenerationStrategy(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      DmValueGenerationStrategy? valueGenerationStrategy,
      bool fromDataAnnotation = false)
    {
      Check.NotNull<IConventionPropertyBuilder>(propertyBuilder, nameof (propertyBuilder));
      return (!valueGenerationStrategy.HasValue || DmPropertyExtensions.IsCompatibleWithValueGeneration((IReadOnlyProperty) propertyBuilder.Metadata)) && ((IConventionAnnotatableBuilder) propertyBuilder).CanSetAnnotation("Dm:ValueGenerationStrategy", (object) valueGenerationStrategy, fromDataAnnotation);
    }

    [Obsolete("Use UseHiLo")]
    public static PropertyBuilder ForSqlServerUseSequenceHiLo(
      [NotNull] this PropertyBuilder propertyBuilder,
      [CanBeNull] string name = null,
      [CanBeNull] string schema = null)
    {
      return propertyBuilder.UseHiLo(name, schema);
    }

    [Obsolete("Use UseHiLo")]
    public static PropertyBuilder<TProperty> ForSqlServerUseSequenceHiLo<TProperty>(
      [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
      [CanBeNull] string name = null,
      [CanBeNull] string schema = null)
    {
      return propertyBuilder.UseHiLo<TProperty>(name, schema);
    }

    [Obsolete("Use HasHiLoSequence")]
    public static IConventionSequenceBuilder ForSqlServerHasHiLoSequence(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      [CanBeNull] string name,
      [CanBeNull] string schema,
      bool fromDataAnnotation = false)
    {
      return propertyBuilder.HasHiLoSequence(name, schema);
    }

    [Obsolete("Use UseIdentityColumn")]
    public static PropertyBuilder UseSqlServerIdentityColumn(
      [NotNull] this PropertyBuilder propertyBuilder,
      int seed = 1,
      int increment = 1)
    {
      return propertyBuilder.UseIdentityColumn(seed, increment);
    }

    [Obsolete("Use UseIdentityColumn")]
    public static PropertyBuilder<TProperty> UseSqlServerIdentityColumn<TProperty>(
      [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
      int seed = 1,
      int increment = 1)
    {
      return propertyBuilder.UseIdentityColumn<TProperty>(seed, increment);
    }

    [Obsolete("Use HasIdentityColumnSeed")]
    public static IConventionPropertyBuilder ForSqlServerHasIdentitySeed(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      int? seed,
      bool fromDataAnnotation = false)
    {
      return propertyBuilder.HasIdentityColumnSeed(seed, fromDataAnnotation);
    }

    [Obsolete("Use HasIdentityColumnIncrement")]
    public static IConventionPropertyBuilder ForSqlServerHasIdentityIncrement(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      int? increment,
      bool fromDataAnnotation = false)
    {
      return propertyBuilder.HasIdentityColumnIncrement(increment, fromDataAnnotation);
    }

    [Obsolete("Use HasValueGenerationStrategy")]
    public static IConventionPropertyBuilder ForSqlServerHasValueGenerationStrategy(
      [NotNull] this IConventionPropertyBuilder propertyBuilder,
      DmValueGenerationStrategy? valueGenerationStrategy,
      bool fromDataAnnotation = false)
    {
      return propertyBuilder.HasValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
    }
  }
}
