using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
	public static class DmPropertyExtensions
	{
		public static string GetHiLoSequenceName([JetBrains.Annotations.NotNull] this IProperty property)
		{
			return (string)property["Dm:HiLoSequenceName"];
		}

		public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string name)
		{
			property.SetOrRemoveAnnotation("Dm:HiLoSequenceName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"));
		}

		public static void SetHiLoSequenceName([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
		{
			property.SetOrRemoveAnnotation("Dm:HiLoSequenceName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name"), fromDataAnnotation);
		}

		public static ConfigurationSource? GetHiLoSequenceNameConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
		{
			return property.FindAnnotation("Dm:HiLoSequenceName")?.GetConfigurationSource();
		}

		public static string GetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IProperty property)
		{
			return (string)property["Dm:HiLoSequenceSchema"];
		}

		public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IMutableProperty property, [JetBrains.Annotations.CanBeNull] string schema)
		{
			property.SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema"));
		}

		public static void SetHiLoSequenceSchema([JetBrains.Annotations.NotNull] this IConventionProperty property, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			property.SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema"), fromDataAnnotation);
		}

		public static ConfigurationSource? GetHiLoSequenceSchemaConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
		{
			return property.FindAnnotation("Dm:HiLoSequenceSchema")?.GetConfigurationSource();
		}

		public static ISequence FindHiLoSequence([JetBrains.Annotations.NotNull] this IProperty property)
		{
			IModel model = property.DeclaringEntityType.Model;
			if (property.GetValueGenerationStrategy() != DmValueGenerationStrategy.SequenceHiLo)
			{
				return null;
			}
			string name = property.GetHiLoSequenceName() ?? model.GetHiLoSequenceName();
			string schema = property.GetHiLoSequenceSchema() ?? model.GetHiLoSequenceSchema();
			return model.FindSequence(name, schema);
		}

		public static int? GetIdentitySeed([JetBrains.Annotations.NotNull] this IProperty property)
		{
			return (int?)property["Dm:IdentitySeed"];
		}

		public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IMutableProperty property, int? seed)
		{
			property.SetOrRemoveAnnotation("Dm:IdentitySeed", seed);
		}

		public static void SetIdentitySeed([JetBrains.Annotations.NotNull] this IConventionProperty property, int? seed, bool fromDataAnnotation = false)
		{
			property.SetOrRemoveAnnotation("Dm:IdentitySeed", seed, fromDataAnnotation);
		}

		public static ConfigurationSource? GetIdentitySeedConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
		{
			return property.FindAnnotation("Dm:IdentitySeed")?.GetConfigurationSource();
		}

		public static int? GetIdentityIncrement([JetBrains.Annotations.NotNull] this IProperty property)
		{
			return (int?)property["Dm:IdentityIncrement"];
		}

		public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IMutableProperty property, int? increment)
		{
			property.SetOrRemoveAnnotation("Dm:IdentityIncrement", increment);
		}

		public static void SetIdentityIncrement([JetBrains.Annotations.NotNull] this IConventionProperty property, int? increment, bool fromDataAnnotation = false)
		{
			property.SetOrRemoveAnnotation("Dm:IdentityIncrement", increment, fromDataAnnotation);
		}

		public static ConfigurationSource? GetIdentityIncrementConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
		{
			return property.FindAnnotation("Dm:IdentityIncrement")?.GetConfigurationSource();
		}

		public static DmValueGenerationStrategy GetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IProperty property)
		{
			IAnnotation annotation = property.FindAnnotation("Dm:ValueGenerationStrategy");
			if (annotation != null)
			{
				return (DmValueGenerationStrategy)annotation.Value;
			}
			if (property.ValueGenerated != ValueGenerated.OnAdd || property.IsForeignKey() || property.GetDefaultValue() != null || property.GetDefaultValueSql() != null || property.GetComputedColumnSql() != null)
			{
				return DmValueGenerationStrategy.None;
			}
			return GetDefaultValueGenerationStrategy(property);
		}

		public static DmValueGenerationStrategy GetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IProperty property, in StoreObjectIdentifier storeObject)
		{
			IAnnotation annotation = property.FindAnnotation("Dm:ValueGenerationStrategy");
			if (annotation != null)
			{
				return (DmValueGenerationStrategy)annotation.Value;
			}
			IProperty property2 = property.FindSharedStoreObjectRootProperty(in storeObject);
			if (property2 != null)
			{
				return (property2.GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.IdentityColumn && !property.GetContainingForeignKeys().Any((IForeignKey fk) => !fk.IsBaseLinking())) ? DmValueGenerationStrategy.IdentityColumn : DmValueGenerationStrategy.None;
			}
			if (property.ValueGenerated != ValueGenerated.OnAdd || property.GetContainingForeignKeys().Any((IForeignKey fk) => !fk.IsBaseLinking()) || property.GetDefaultValue(in storeObject) != null || property.GetDefaultValueSql(in storeObject) != null || property.GetComputedColumnSql(in storeObject) != null)
			{
				return DmValueGenerationStrategy.None;
			}
			return GetDefaultValueGenerationStrategy(property);
		}

		private static DmValueGenerationStrategy GetDefaultValueGenerationStrategy(IProperty property)
		{
			DmValueGenerationStrategy? valueGenerationStrategy = property.DeclaringEntityType.Model.GetValueGenerationStrategy();
			if (valueGenerationStrategy == DmValueGenerationStrategy.SequenceHiLo && IsCompatibleWithValueGeneration(property))
			{
				return DmValueGenerationStrategy.SequenceHiLo;
			}
			return (valueGenerationStrategy == DmValueGenerationStrategy.IdentityColumn && IsCompatibleWithValueGeneration(property)) ? DmValueGenerationStrategy.IdentityColumn : DmValueGenerationStrategy.None;
		}

		public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IMutableProperty property, DmValueGenerationStrategy? value)
		{
			CheckValueGenerationStrategy(property, value);
			property.SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", value);
		}

		public static void SetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionProperty property, DmValueGenerationStrategy? value, bool fromDataAnnotation = false)
		{
			CheckValueGenerationStrategy(property, value);
			property.SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", value, fromDataAnnotation);
		}

		private static void CheckValueGenerationStrategy(IProperty property, DmValueGenerationStrategy? value)
		{
			if (value.HasValue)
			{
				Type clrType = property.ClrType;
				if (value == DmValueGenerationStrategy.IdentityColumn && !IsCompatibleWithValueGeneration(property))
				{
					throw new ArgumentException(DmStrings.IdentityBadType(property.Name, property.DeclaringEntityType.DisplayName(), clrType.ShortDisplayName()));
				}
				if (value == DmValueGenerationStrategy.SequenceHiLo && !IsCompatibleWithValueGeneration(property))
				{
					throw new ArgumentException(DmStrings.SequenceBadType(property.Name, property.DeclaringEntityType.DisplayName(), clrType.ShortDisplayName()));
				}
			}
		}

		public static ConfigurationSource? GetValueGenerationStrategyConfigurationSource([JetBrains.Annotations.NotNull] this IConventionProperty property)
		{
			return property.FindAnnotation("Dm:ValueGenerationStrategy")?.GetConfigurationSource();
		}

		public static bool IsCompatibleWithValueGeneration([JetBrains.Annotations.NotNull] IProperty property)
		{
			Type clrType = property.ClrType;
			return (clrType.IsInteger() || clrType == typeof(decimal)) && (property.FindTypeMapping()?.Converter ?? property.GetValueConverter()) == null;
		}
	}
}
