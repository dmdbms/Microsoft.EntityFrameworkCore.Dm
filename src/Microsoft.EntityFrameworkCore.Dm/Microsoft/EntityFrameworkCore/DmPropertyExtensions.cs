using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
	public static class DmPropertyExtensions
	{
		public static string GetHiLoSequenceName([NotNull] this IReadOnlyProperty property)
		{
			return (string)((IReadOnlyAnnotatable)property)["Dm:HiLoSequenceName"];
		}

		public static void SetHiLoSequenceName([NotNull] this IMutableProperty property, [CanBeNull] string name)
		{
			((IMutableAnnotatable)property).SetOrRemoveAnnotation("Dm:HiLoSequenceName", (object)Check.NullButNotEmpty(name, "name"));
		}

		public static void SetHiLoSequenceName([NotNull] this IConventionProperty property, [CanBeNull] string name, bool fromDataAnnotation = false)
		{
			((IConventionAnnotatable)property).SetOrRemoveAnnotation("Dm:HiLoSequenceName", (object)Check.NullButNotEmpty(name, "name"), fromDataAnnotation);
		}

		public static ConfigurationSource? GetHiLoSequenceNameConfigurationSource([NotNull] this IConventionProperty property)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			IConventionAnnotation obj = ((IConventionAnnotatable)property).FindAnnotation("Dm:HiLoSequenceName");
			return (obj != null) ? new ConfigurationSource?(obj.GetConfigurationSource()) : null;
		}

		public static string GetHiLoSequenceSchema([NotNull] this IReadOnlyProperty property)
		{
			return (string)((IReadOnlyAnnotatable)property)["Dm:HiLoSequenceSchema"];
		}

		public static void SetHiLoSequenceSchema([NotNull] this IMutableProperty property, [CanBeNull] string schema)
		{
			((IMutableAnnotatable)property).SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", (object)Check.NullButNotEmpty(schema, "schema"));
		}

		public static void SetHiLoSequenceSchema([NotNull] this IConventionProperty property, [CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			((IConventionAnnotatable)property).SetOrRemoveAnnotation("Dm:HiLoSequenceSchema", (object)Check.NullButNotEmpty(schema, "schema"), fromDataAnnotation);
		}

		public static ConfigurationSource? GetHiLoSequenceSchemaConfigurationSource([NotNull] this IConventionProperty property)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			IConventionAnnotation obj = ((IConventionAnnotatable)property).FindAnnotation("Dm:HiLoSequenceSchema");
			return (obj != null) ? new ConfigurationSource?(obj.GetConfigurationSource()) : null;
		}

		public static IReadOnlySequence FindHiLoSequence([NotNull] this IReadOnlyProperty property)
		{
			IReadOnlyModel model = ((IReadOnlyTypeBase)property.DeclaringEntityType).Model;
			if (property.GetValueGenerationStrategy() != DmValueGenerationStrategy.SequenceHiLo)
			{
				return null;
			}
			string text = property.GetHiLoSequenceName() ?? model.GetHiLoSequenceName();
			string text2 = property.GetHiLoSequenceSchema() ?? model.GetHiLoSequenceSchema();
			return RelationalModelExtensions.FindSequence(model, text, text2);
		}

		public static int? GetIdentitySeed([NotNull] this IReadOnlyProperty property)
		{
			return (int?)((IReadOnlyAnnotatable)property)[("Dm:IdentitySeed")];
		}

		public static void SetIdentitySeed([NotNull] this IMutableProperty property, int? seed)
		{
			((IMutableAnnotatable)property).SetOrRemoveAnnotation("Dm:IdentitySeed", (object)seed);
		}

		public static void SetIdentitySeed([NotNull] this IConventionProperty property, int? seed, bool fromDataAnnotation = false)
		{
			((IConventionAnnotatable)property).SetOrRemoveAnnotation("Dm:IdentitySeed", (object)seed, fromDataAnnotation);
		}

		public static ConfigurationSource? GetIdentitySeedConfigurationSource([NotNull] this IConventionProperty property)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			IConventionAnnotation obj = ((IConventionAnnotatable)property).FindAnnotation("Dm:IdentitySeed");
			return (obj != null) ? new ConfigurationSource?(obj.GetConfigurationSource()) : null;
		}

		public static int? GetIdentityIncrement([NotNull] this IReadOnlyProperty property)
		{
			return (int?)((IReadOnlyAnnotatable)property)[("Dm:IdentityIncrement")];
		}

		public static void SetIdentityIncrement([NotNull] this IMutableProperty property, int? increment)
		{
			((IMutableAnnotatable)property).SetOrRemoveAnnotation("Dm:IdentityIncrement", (object)increment);
		}

		public static void SetIdentityIncrement([NotNull] this IConventionProperty property, int? increment, bool fromDataAnnotation = false)
		{
			((IConventionAnnotatable)property).SetOrRemoveAnnotation("Dm:IdentityIncrement", (object)increment, fromDataAnnotation);
		}

		public static ConfigurationSource? GetIdentityIncrementConfigurationSource([NotNull] this IConventionProperty property)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			IConventionAnnotation obj = ((IConventionAnnotatable)property).FindAnnotation("Dm:IdentityIncrement");
			return (obj != null) ? new ConfigurationSource?(obj.GetConfigurationSource()) : null;
		}

		public static DmValueGenerationStrategy GetValueGenerationStrategy([NotNull] this IReadOnlyProperty property)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			IAnnotation val = ((IReadOnlyAnnotatable)property).FindAnnotation("Dm:ValueGenerationStrategy");
			if (val != null)
			{
				return (DmValueGenerationStrategy)val.Value;
			}
			if ((int)property.ValueGenerated != 1 || property.IsForeignKey() || RelationalPropertyExtensions.GetDefaultValue(property) != null || RelationalPropertyExtensions.GetDefaultValueSql(property) != null || RelationalPropertyExtensions.GetComputedColumnSql(property) != null)
			{
				return DmValueGenerationStrategy.None;
			}
			return GetDefaultValueGenerationStrategy(property);
		}

		public static DmValueGenerationStrategy GetValueGenerationStrategy([NotNull] this IReadOnlyProperty property, in StoreObjectIdentifier storeObject)
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Invalid comparison between Unknown and I4
			IAnnotation val = ((IReadOnlyAnnotatable)property).FindAnnotation("Dm:ValueGenerationStrategy");
			if (val != null)
			{
				return (DmValueGenerationStrategy)val.Value;
			}
			IReadOnlyProperty val2 = RelationalPropertyExtensions.FindSharedStoreObjectRootProperty(property, in storeObject);
			if (val2 != null)
			{
				return (val2.GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.IdentityColumn && !property.GetContainingForeignKeys().Any((IReadOnlyForeignKey fk) => !fk.IsBaseLinking())) ? DmValueGenerationStrategy.IdentityColumn : DmValueGenerationStrategy.None;
			}
			if ((int)property.ValueGenerated != 1 || property.GetContainingForeignKeys().Any((IReadOnlyForeignKey fk) => !fk.IsBaseLinking()) || RelationalPropertyExtensions.GetDefaultValue(property, in storeObject) != null || RelationalPropertyExtensions.GetDefaultValueSql(property, in storeObject) != null || RelationalPropertyExtensions.GetComputedColumnSql(property, in storeObject) != null)
			{
				return DmValueGenerationStrategy.None;
			}
			return GetDefaultValueGenerationStrategy(property);
		}

		private static DmValueGenerationStrategy GetDefaultValueGenerationStrategy(IReadOnlyProperty property)
		{
			DmValueGenerationStrategy? valueGenerationStrategy = ((IReadOnlyTypeBase)property.DeclaringEntityType).Model.GetValueGenerationStrategy();
			if (valueGenerationStrategy == DmValueGenerationStrategy.SequenceHiLo && IsCompatibleWithValueGeneration(property))
			{
				return DmValueGenerationStrategy.SequenceHiLo;
			}
			return (valueGenerationStrategy == DmValueGenerationStrategy.IdentityColumn && IsCompatibleWithValueGeneration(property)) ? DmValueGenerationStrategy.IdentityColumn : DmValueGenerationStrategy.None;
		}

		public static void SetValueGenerationStrategy([NotNull] this IMutableProperty property, DmValueGenerationStrategy? value)
		{
			CheckValueGenerationStrategy((IReadOnlyProperty)(object)property, value);
			((IMutableAnnotatable)property).SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", (object)value);
		}

		public static void SetValueGenerationStrategy([NotNull] this IConventionProperty property, DmValueGenerationStrategy? value, bool fromDataAnnotation = false)
		{
			CheckValueGenerationStrategy((IReadOnlyProperty)(object)property, value);
			((IConventionAnnotatable)property).SetOrRemoveAnnotation("Dm:ValueGenerationStrategy", (object)value, fromDataAnnotation);
		}

		private static void CheckValueGenerationStrategy(IReadOnlyProperty property, DmValueGenerationStrategy? value)
		{
			if (value.HasValue)
			{
				Type clrType = ((IReadOnlyPropertyBase)property).ClrType;
				if (value == DmValueGenerationStrategy.IdentityColumn && !IsCompatibleWithValueGeneration(property))
				{
					throw new ArgumentException(DmStrings.IdentityBadType(((IReadOnlyPropertyBase)property).Name, ((IReadOnlyTypeBase)property.DeclaringEntityType).DisplayName(), TypeExtensions.ShortDisplayName(clrType)));
				}
				if (value == DmValueGenerationStrategy.SequenceHiLo && !IsCompatibleWithValueGeneration(property))
				{
					throw new ArgumentException(DmStrings.SequenceBadType(((IReadOnlyPropertyBase)property).Name, ((IReadOnlyTypeBase)property.DeclaringEntityType).DisplayName(), TypeExtensions.ShortDisplayName(clrType)));
				}
			}
		}

		public static ConfigurationSource? GetValueGenerationStrategyConfigurationSource([NotNull] this IConventionProperty property)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			IConventionAnnotation obj = ((IConventionAnnotatable)property).FindAnnotation("Dm:ValueGenerationStrategy");
			return (obj != null) ? new ConfigurationSource?(obj.GetConfigurationSource()) : null;
		}

		public static bool IsCompatibleWithValueGeneration([NotNull] IReadOnlyProperty property)
		{
			Type clrType = ((IReadOnlyPropertyBase)property).ClrType;
			int result;
			if (clrType.IsInteger() || clrType == typeof(decimal))
			{
				CoreTypeMapping obj = property.FindTypeMapping();
				result = (((((obj != null) ? obj.Converter : null) ?? property.GetValueConverter()) == null) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}
}
