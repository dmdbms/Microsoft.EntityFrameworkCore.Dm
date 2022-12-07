// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.DmPropertyExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Linq;



namespace Microsoft.EntityFrameworkCore
{
  public static class DmPropertyExtensions
  {
    public static string GetHiLoSequenceName([NotNull] this IReadOnlyProperty property) => (string) ((IReadOnlyAnnotatable) property)["Dm:HiLoSequenceName"];

    public static void SetHiLoSequenceName([NotNull] this IMutableProperty property, [CanBeNull] string name) => ((IMutableAnnotatable) property).SetOrRemoveAnnotation("Dm:HiLoSequenceName", (object) Check.NullButNotEmpty(name, nameof (name)));

    public static void SetHiLoSequenceName(
      [NotNull] this IConventionProperty property,
      [CanBeNull] string name,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) property).SetOrRemoveAnnotation("Dm:HiLoSequenceName", (object) Check.NullButNotEmpty(name, nameof (name)), fromDataAnnotation);
    }

    public static ConfigurationSource? GetHiLoSequenceNameConfigurationSource(
      [NotNull] this IConventionProperty property)
    {
      return ((IConventionAnnotatable) property).FindAnnotation("Dm:HiLoSequenceName")?.GetConfigurationSource();
    }

    public static string GetHiLoSequenceSchema([NotNull] this IReadOnlyProperty property) => (string) ((IReadOnlyAnnotatable) property)["Dm:HiLoSequenceSchema"];

    public static void SetHiLoSequenceSchema([NotNull] this IMutableProperty property, [CanBeNull] string schema) => ((IMutableAnnotatable) property).SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", (object) Check.NullButNotEmpty(schema, nameof (schema)));

    public static void SetHiLoSequenceSchema(
      [NotNull] this IConventionProperty property,
      [CanBeNull] string schema,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) property).SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", (object) Check.NullButNotEmpty(schema, nameof (schema)), fromDataAnnotation);
    }

    public static ConfigurationSource? GetHiLoSequenceSchemaConfigurationSource(
      [NotNull] this IConventionProperty property)
    {
      return ((IConventionAnnotatable) property).FindAnnotation("Dm:HiLoSequenceSchema")?.GetConfigurationSource();
    }

    public static IReadOnlySequence FindHiLoSequence(
      [NotNull] this IReadOnlyProperty property)
    {
      IReadOnlyModel model = ((IReadOnlyTypeBase) property.DeclaringEntityType).Model;
      if (property.GetValueGenerationStrategy() != DmValueGenerationStrategy.SequenceHiLo)
        return (IReadOnlySequence) null;
      string str1 = property.GetHiLoSequenceName() ?? model.GetHiLoSequenceName();
      string str2 = property.GetHiLoSequenceSchema() ?? model.GetHiLoSequenceSchema();
      return RelationalModelExtensions.FindSequence(model, str1, str2);
    }

    public static int? GetIdentitySeed([NotNull] this IReadOnlyProperty property) => (int?) ((IReadOnlyAnnotatable) property)["Dm:IdentitySeed"];

    public static void SetIdentitySeed([NotNull] this IMutableProperty property, int? seed) => ((IMutableAnnotatable) property).SetOrRemoveAnnotation("Dm:IdentitySeed", (object) seed);

    public static void SetIdentitySeed(
      [NotNull] this IConventionProperty property,
      int? seed,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) property).SetOrRemoveAnnotation("Dm:IdentitySeed", (object) seed, fromDataAnnotation);
    }

    public static ConfigurationSource? GetIdentitySeedConfigurationSource(
      [NotNull] this IConventionProperty property)
    {
      return ((IConventionAnnotatable) property).FindAnnotation("Dm:IdentitySeed")?.GetConfigurationSource();
    }

    public static int? GetIdentityIncrement([NotNull] this IReadOnlyProperty property) => (int?) ((IReadOnlyAnnotatable) property)["Dm:IdentityIncrement"];

    public static void SetIdentityIncrement([NotNull] this IMutableProperty property, int? increment) => ((IMutableAnnotatable) property).SetOrRemoveAnnotation("Dm:IdentityIncrement", (object) increment);

    public static void SetIdentityIncrement(
      [NotNull] this IConventionProperty property,
      int? increment,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) property).SetOrRemoveAnnotation("Dm:IdentityIncrement", (object) increment, fromDataAnnotation);
    }

    public static ConfigurationSource? GetIdentityIncrementConfigurationSource(
      [NotNull] this IConventionProperty property)
    {
      return ((IConventionAnnotatable) property).FindAnnotation("Dm:IdentityIncrement")?.GetConfigurationSource();
    }

    public static DmValueGenerationStrategy GetValueGenerationStrategy(
      [NotNull] this IReadOnlyProperty property)
    {
      IAnnotation annotation = ((IReadOnlyAnnotatable) property).FindAnnotation("Dm:ValueGenerationStrategy");
      if (annotation != null)
        return (DmValueGenerationStrategy) annotation.Value;
      return property.ValueGenerated != (ValueGenerated)1 || property.IsForeignKey() || RelationalPropertyExtensions.GetDefaultValue(property) != null || RelationalPropertyExtensions.GetDefaultValueSql(property) != null || RelationalPropertyExtensions.GetComputedColumnSql(property) != null ? DmValueGenerationStrategy.None : DmPropertyExtensions.GetDefaultValueGenerationStrategy(property);
    }

    public static DmValueGenerationStrategy GetValueGenerationStrategy(
      [NotNull] this IReadOnlyProperty property,
      in StoreObjectIdentifier storeObject)
    {
      IAnnotation annotation = ((IReadOnlyAnnotatable) property).FindAnnotation("Dm:ValueGenerationStrategy");
      if (annotation != null)
        return (DmValueGenerationStrategy) annotation.Value;
      IReadOnlyProperty objectRootProperty = RelationalPropertyExtensions.FindSharedStoreObjectRootProperty(property,  storeObject);
      if (objectRootProperty != null)
        return objectRootProperty.GetValueGenerationStrategy(in storeObject) != DmValueGenerationStrategy.IdentityColumn || property.GetContainingForeignKeys().Any<IReadOnlyForeignKey>((Func<IReadOnlyForeignKey, bool>) (fk => !fk.IsBaseLinking())) ? DmValueGenerationStrategy.None : DmValueGenerationStrategy.IdentityColumn;
      return property.ValueGenerated != (ValueGenerated)1 || property.GetContainingForeignKeys().Any<IReadOnlyForeignKey>((Func<IReadOnlyForeignKey, bool>) (fk => !fk.IsBaseLinking())) || RelationalPropertyExtensions.GetDefaultValue(property,  storeObject) != null || RelationalPropertyExtensions.GetDefaultValueSql(property,  storeObject) != null || RelationalPropertyExtensions.GetComputedColumnSql(property,  storeObject) != null ? DmValueGenerationStrategy.None : DmPropertyExtensions.GetDefaultValueGenerationStrategy(property);
    }

    private static DmValueGenerationStrategy GetDefaultValueGenerationStrategy(
      IReadOnlyProperty property)
    {
      DmValueGenerationStrategy? generationStrategy1 = ((IReadOnlyTypeBase) property.DeclaringEntityType).Model.GetValueGenerationStrategy();
      DmValueGenerationStrategy? nullable1 = generationStrategy1;
      DmValueGenerationStrategy generationStrategy2 = DmValueGenerationStrategy.SequenceHiLo;
      if (nullable1.GetValueOrDefault() == generationStrategy2 & nullable1.HasValue && DmPropertyExtensions.IsCompatibleWithValueGeneration(property))
        return DmValueGenerationStrategy.SequenceHiLo;
      DmValueGenerationStrategy? nullable2 = generationStrategy1;
      DmValueGenerationStrategy generationStrategy3 = DmValueGenerationStrategy.IdentityColumn;
      return !(nullable2.GetValueOrDefault() == generationStrategy3 & nullable2.HasValue) || !DmPropertyExtensions.IsCompatibleWithValueGeneration(property) ? DmValueGenerationStrategy.None : DmValueGenerationStrategy.IdentityColumn;
    }

    public static void SetValueGenerationStrategy(
      [NotNull] this IMutableProperty property,
      DmValueGenerationStrategy? value)
    {
      DmPropertyExtensions.CheckValueGenerationStrategy((IReadOnlyProperty) property, value);
      ((IMutableAnnotatable) property).SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", (object) value);
    }

    public static void SetValueGenerationStrategy(
      [NotNull] this IConventionProperty property,
      DmValueGenerationStrategy? value,
      bool fromDataAnnotation = false)
    {
      DmPropertyExtensions.CheckValueGenerationStrategy((IReadOnlyProperty) property, value);
      ((IConventionAnnotatable) property).SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", (object) value, fromDataAnnotation);
    }

    private static void CheckValueGenerationStrategy(
      IReadOnlyProperty property,
      DmValueGenerationStrategy? value)
    {
      if (!value.HasValue)
        return;
      Type clrType = ((IReadOnlyPropertyBase) property).ClrType;
      DmValueGenerationStrategy? nullable1 = value;
      DmValueGenerationStrategy generationStrategy1 = DmValueGenerationStrategy.IdentityColumn;
      if (nullable1.GetValueOrDefault() == generationStrategy1 & nullable1.HasValue && !DmPropertyExtensions.IsCompatibleWithValueGeneration(property))
        throw new ArgumentException(DmStrings.IdentityBadType((object) ((IReadOnlyPropertyBase) property).Name, (object) ((IReadOnlyTypeBase) property.DeclaringEntityType).DisplayName(), (object) TypeExtensions.ShortDisplayName(clrType)));
      DmValueGenerationStrategy? nullable2 = value;
      DmValueGenerationStrategy generationStrategy2 = DmValueGenerationStrategy.SequenceHiLo;
      if (nullable2.GetValueOrDefault() == generationStrategy2 & nullable2.HasValue && !DmPropertyExtensions.IsCompatibleWithValueGeneration(property))
        throw new ArgumentException(DmStrings.SequenceBadType((object) ((IReadOnlyPropertyBase) property).Name, (object) ((IReadOnlyTypeBase) property.DeclaringEntityType).DisplayName(), (object) TypeExtensions.ShortDisplayName(clrType)));
    }

    public static ConfigurationSource? GetValueGenerationStrategyConfigurationSource(
      [NotNull] this IConventionProperty property)
    {
      return ((IConventionAnnotatable) property).FindAnnotation("Dm:ValueGenerationStrategy")?.GetConfigurationSource();
    }

    public static bool IsCompatibleWithValueGeneration([NotNull] IReadOnlyProperty property)
    {
      Type clrType = ((IReadOnlyPropertyBase) property).ClrType;
      return (clrType.IsInteger() || clrType == typeof (Decimal)) && (property.FindTypeMapping()?.Converter ?? property.GetValueConverter()) == null;
    }
  }
}
