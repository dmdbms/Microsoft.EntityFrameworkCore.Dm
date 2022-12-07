// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.DmModelExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;



namespace Microsoft.EntityFrameworkCore
{
  public static class DmModelExtensions
  {
    public const string DefaultHiLoSequenceName = "EntityFrameworkHiLoSequence";

    public static string GetHiLoSequenceName([NotNull] this IReadOnlyModel model) => (string) ((IReadOnlyAnnotatable) model)["Dm:HiLoSequenceName"] ?? "EntityFrameworkHiLoSequence";

    public static void SetHiLoSequenceName([NotNull] this IMutableModel model, [CanBeNull] string name)
    {
      Check.NullButNotEmpty(name, nameof (name));
      ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:HiLoSequenceName", (object) name);
    }

    public static void SetHiLoSequenceName(
      [NotNull] this IConventionModel model,
      [CanBeNull] string name,
      bool fromDataAnnotation = false)
    {
      Check.NullButNotEmpty(name, nameof (name));
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:HiLoSequenceName", (object) name, fromDataAnnotation);
    }

    public static ConfigurationSource? GetHiLoSequenceNameConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:HiLoSequenceName")?.GetConfigurationSource();
    }

    public static string GetHiLoSequenceSchema([NotNull] this IReadOnlyModel model) => (string) ((IReadOnlyAnnotatable) model)["Dm:HiLoSequenceSchema"];

    public static void SetHiLoSequenceSchema([NotNull] this IMutableModel model, [CanBeNull] string value)
    {
      Check.NullButNotEmpty(value, nameof (value));
      ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", (object) value);
    }

    public static void SetHiLoSequenceSchema(
      [NotNull] this IConventionModel model,
      [CanBeNull] string value,
      bool fromDataAnnotation = false)
    {
      Check.NullButNotEmpty(value, nameof (value));
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", (object) value, fromDataAnnotation);
    }

    public static ConfigurationSource? GetHiLoSequenceSchemaConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:HiLoSequenceSchema")?.GetConfigurationSource();
    }

    public static int GetIdentitySeed([NotNull] this IReadOnlyModel model) => (int?) ((IReadOnlyAnnotatable) model)["Dm:IdentitySeed"] ?? 1;

    public static void SetIdentitySeed([NotNull] this IMutableModel model, int? seed) => ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:IdentitySeed", (object) seed);

    public static void SetIdentitySeed(
      [NotNull] this IConventionModel model,
      int? seed,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:IdentitySeed", (object) seed, fromDataAnnotation);
    }

    public static ConfigurationSource? GetIdentitySeedConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:IdentitySeed")?.GetConfigurationSource();
    }

    public static int GetIdentityIncrement([NotNull] this IReadOnlyModel model) => (int?) ((IReadOnlyAnnotatable) model)["Dm:IdentityIncrement"] ?? 1;

    public static void SetIdentityIncrement([NotNull] this IMutableModel model, int? increment) => ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:IdentityIncrement", (object) increment);

    public static void SetIdentityIncrement(
      [NotNull] this IConventionModel model,
      int? increment,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:IdentityIncrement", (object) increment, fromDataAnnotation);
    }

    public static ConfigurationSource? GetIdentityIncrementConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:IdentityIncrement")?.GetConfigurationSource();
    }

    public static DmValueGenerationStrategy? GetValueGenerationStrategy(
      [NotNull] this IReadOnlyModel model)
    {
      return (DmValueGenerationStrategy?) ((IReadOnlyAnnotatable) model)["Dm:ValueGenerationStrategy"];
    }

    public static void SetValueGenerationStrategy(
      [NotNull] this IMutableModel model,
      DmValueGenerationStrategy? value)
    {
      ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", (object) value);
    }

    public static void SetValueGenerationStrategy(
      [NotNull] this IConventionModel model,
      DmValueGenerationStrategy? value,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", (object) value, fromDataAnnotation);
    }

    public static ConfigurationSource? GetValueGenerationStrategyConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:ValueGenerationStrategy")?.GetConfigurationSource();
    }

    public static string GetDatabaseMaxSize([NotNull] this IReadOnlyModel model) => (string) ((IReadOnlyAnnotatable) model)["Dm:DatabaseMaxSize"];

    public static void SetDatabaseMaxSize([NotNull] this IMutableModel model, [CanBeNull] string value) => ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:DatabaseMaxSize", (object) value);

    public static void SetDatabaseMaxSize(
      [NotNull] this IConventionModel model,
      [CanBeNull] string value,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:DatabaseMaxSize", (object) value, fromDataAnnotation);
    }

    public static ConfigurationSource? GetDatabaseMaxSizeConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:DatabaseMaxSize")?.GetConfigurationSource();
    }

    public static string GetServiceTierSql([NotNull] this IReadOnlyModel model) => (string) ((IReadOnlyAnnotatable) model)["Dm:ServiceTierSql"];

    public static void SetServiceTierSql([NotNull] this IMutableModel model, [CanBeNull] string value) => ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:ServiceTierSql", (object) value);

    public static void SetServiceTierSql(
      [NotNull] this IConventionModel model,
      [CanBeNull] string value,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:ServiceTierSql", (object) value, fromDataAnnotation);
    }

    public static ConfigurationSource? GetServiceTierSqlConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:ServiceTierSql")?.GetConfigurationSource();
    }

    public static string GetPerformanceLevelSql([NotNull] this IReadOnlyModel model) => (string) ((IReadOnlyAnnotatable) model)["Dm:PerformanceLevelSql"];

    public static void SetPerformanceLevelSql([NotNull] this IMutableModel model, [CanBeNull] string value) => ((IMutableAnnotatable) model).SetOrRemoveAnnotation("Dm:PerformanceLevelSql", (object) value);

    public static void SetPerformanceLevelSql(
      [NotNull] this IConventionModel model,
      [CanBeNull] string value,
      bool fromDataAnnotation = false)
    {
      ((IConventionAnnotatable) model).SetOrRemoveAnnotation("Dm:PerformanceLevelSql", (object) value, fromDataAnnotation);
    }

    public static ConfigurationSource? GetPerformanceLevelSqlConfigurationSource(
      [NotNull] this IConventionModel model)
    {
      return ((IConventionAnnotatable) model).FindAnnotation("Dm:PerformanceLevelSql")?.GetConfigurationSource();
    }
  }
}
