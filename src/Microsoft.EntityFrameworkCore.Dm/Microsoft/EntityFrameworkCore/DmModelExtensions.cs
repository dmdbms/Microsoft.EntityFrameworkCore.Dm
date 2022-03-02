using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
	public static class DmModelExtensions
	{
		public const string DefaultHiLoSequenceName = "EntityFrameworkHiLoSequence";

		public static string GetHiLoSequenceName([JetBrains.Annotations.NotNull] this IModel model)
		{
			return ((string)model["Dm:HiLoSequenceName"]) ?? "EntityFrameworkHiLoSequence";
		}

		public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string name)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
			model.SetOrRemoveAnnotation("Dm:HiLoSequenceName", name);
		}

		public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
			model.SetOrRemoveAnnotation("Dm:HiLoSequenceName", name, fromDataAnnotation);
		}

		public static ConfigurationSource? GetHiLoSequenceNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:HiLoSequenceName")?.GetConfigurationSource();
		}

		public static string GetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IModel model)
		{
			return (string)model["Dm:HiLoSequenceSchema"];
		}

		public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value");
			model.SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", value);
		}

		public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value");
			model.SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", value, fromDataAnnotation);
		}

		public static ConfigurationSource? GetHiLoSequenceSchemaConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:HiLoSequenceSchema")?.GetConfigurationSource();
		}

		public static int GetIdentitySeed([JetBrains.Annotations.NotNull] this IModel model)
		{
			return ((int?)model["Dm:IdentitySeed"]) ?? 1;
		}

		public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IMutableModel model, int? seed)
		{
			model.SetOrRemoveAnnotation("Dm:IdentitySeed", seed);
		}

		public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IConventionModel model, int? seed, bool fromDataAnnotation = false)
		{
			model.SetOrRemoveAnnotation("Dm:IdentitySeed", seed, fromDataAnnotation);
		}

		public static ConfigurationSource? GetIdentitySeedConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:IdentitySeed")?.GetConfigurationSource();
		}

		public static int GetIdentityIncrement([JetBrains.Annotations.NotNull] this IModel model)
		{
			return ((int?)model["Dm:IdentityIncrement"]) ?? 1;
		}

		public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IMutableModel model, int? increment)
		{
			model.SetOrRemoveAnnotation("Dm:IdentityIncrement", increment);
		}

		public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IConventionModel model, int? increment, bool fromDataAnnotation = false)
		{
			model.SetOrRemoveAnnotation("Dm:IdentityIncrement", increment, fromDataAnnotation);
		}

		public static ConfigurationSource? GetIdentityIncrementConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:IdentityIncrement")?.GetConfigurationSource();
		}

		public static DmValueGenerationStrategy? GetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IModel model)
		{
			return (DmValueGenerationStrategy?)model["Dm:ValueGenerationStrategy"];
		}

		public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IMutableModel model, DmValueGenerationStrategy? value)
		{
			model.SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", value);
		}

		public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionModel model, DmValueGenerationStrategy? value, bool fromDataAnnotation = false)
		{
			model.SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", value, fromDataAnnotation);
		}

		public static ConfigurationSource? GetValueGenerationStrategyConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:ValueGenerationStrategy")?.GetConfigurationSource();
		}

		public static string GetDatabaseMaxSize([JetBrains.Annotations.NotNull] this IModel model)
		{
			return (string)model["Dm:DatabaseMaxSize"];
		}

		public static void SetDatabaseMaxSize([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
		{
			model.SetOrRemoveAnnotation("Dm:DatabaseMaxSize", value);
		}

		public static void SetDatabaseMaxSize([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
		{
			model.SetOrRemoveAnnotation("Dm:DatabaseMaxSize", value, fromDataAnnotation);
		}

		public static ConfigurationSource? GetDatabaseMaxSizeConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:DatabaseMaxSize")?.GetConfigurationSource();
		}

		public static string GetServiceTierSql([JetBrains.Annotations.NotNull] this IModel model)
		{
			return (string)model["Dm:ServiceTierSql"];
		}

		public static void SetServiceTierSql([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
		{
			model.SetOrRemoveAnnotation("Dm:ServiceTierSql", value);
		}

		public static void SetServiceTierSql([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
		{
			model.SetOrRemoveAnnotation("Dm:ServiceTierSql", value, fromDataAnnotation);
		}

		public static ConfigurationSource? GetServiceTierSqlConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:ServiceTierSql")?.GetConfigurationSource();
		}

		public static string GetPerformanceLevelSql([JetBrains.Annotations.NotNull] this IModel model)
		{
			return (string)model["Dm:PerformanceLevelSql"];
		}

		public static void SetPerformanceLevelSql([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
		{
			model.SetOrRemoveAnnotation("Dm:PerformanceLevelSql", value);
		}

		public static void SetPerformanceLevelSql([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
		{
			model.SetOrRemoveAnnotation("Dm:PerformanceLevelSql", value, fromDataAnnotation);
		}

		public static ConfigurationSource? GetPerformanceLevelSqlConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
		{
			return model.FindAnnotation("Dm:PerformanceLevelSql")?.GetConfigurationSource();
		}
	}
}
