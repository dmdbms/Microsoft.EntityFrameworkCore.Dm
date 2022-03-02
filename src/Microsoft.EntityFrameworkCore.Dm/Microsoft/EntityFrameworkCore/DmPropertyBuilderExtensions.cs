using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
	public static class DmPropertyBuilderExtensions
	{
		public static PropertyBuilder UseHiLo([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name = null, [JetBrains.Annotations.CanBeNull] string schema = null)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
			IMutableProperty metadata = propertyBuilder.Metadata;
			if (name == null)
			{
				name = "EntityFrameworkHiLoSequence";
			}
			IMutableModel model = metadata.DeclaringEntityType.Model;
			if (model.FindSequence(name, schema) == null)
			{
				model.AddSequence(name, schema).IncrementBy = 10;
			}
			metadata.SetValueGenerationStrategy(DmValueGenerationStrategy.SequenceHiLo);
			metadata.SetHiLoSequenceName(name);
			metadata.SetHiLoSequenceSchema(schema);
			metadata.SetIdentitySeed(null);
			metadata.SetIdentityIncrement(null);
			return propertyBuilder;
		}

		public static PropertyBuilder<TProperty> UseHiLo<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] string name = null, [JetBrains.Annotations.CanBeNull] string schema = null)
		{
			return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).UseHiLo(name, schema);
		}

		public static IConventionSequenceBuilder HasHiLoSequence([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			if (!propertyBuilder.CanSetHiLoSequence(name, schema, fromDataAnnotation))
			{
				return null;
			}
			propertyBuilder.Metadata.SetHiLoSequenceName(name, fromDataAnnotation);
			propertyBuilder.Metadata.SetHiLoSequenceSchema(schema, fromDataAnnotation);
			return (name == null) ? null : propertyBuilder.Metadata.DeclaringEntityType.Model.Builder.HasSequence(name, schema, fromDataAnnotation);
		}

		public static bool CanSetHiLoSequence([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
			return propertyBuilder.CanSetAnnotation("Dm:HiLoSequenceName", name, fromDataAnnotation) && propertyBuilder.CanSetAnnotation("Dm:HiLoSequenceSchema", schema, fromDataAnnotation);
		}

		public static PropertyBuilder UseIdentityColumn([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, int seed = 1, int increment = 1)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
			IMutableProperty metadata = propertyBuilder.Metadata;
			metadata.SetValueGenerationStrategy(DmValueGenerationStrategy.IdentityColumn);
			metadata.SetIdentitySeed(seed);
			metadata.SetIdentityIncrement(increment);
			metadata.SetHiLoSequenceName(null);
			metadata.SetHiLoSequenceSchema(null);
			return propertyBuilder;
		}

		public static PropertyBuilder<TProperty> UseIdentityColumn<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, int seed = 1, int increment = 1)
		{
			return (PropertyBuilder<TProperty>)((PropertyBuilder)propertyBuilder).UseIdentityColumn(seed, increment);
		}

		public static IConventionPropertyBuilder HasIdentityColumnSeed([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, int? seed, bool fromDataAnnotation = false)
		{
			if (propertyBuilder.CanSetIdentityColumnSeed(seed, fromDataAnnotation))
			{
				propertyBuilder.Metadata.SetIdentitySeed(seed, fromDataAnnotation);
				return propertyBuilder;
			}
			return null;
		}

		public static bool CanSetIdentityColumnSeed([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, int? seed, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
			return propertyBuilder.CanSetAnnotation("Dm:IdentitySeed", seed, fromDataAnnotation);
		}

		public static IConventionPropertyBuilder HasIdentityColumnIncrement([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, int? increment, bool fromDataAnnotation = false)
		{
			if (propertyBuilder.CanSetIdentityColumnIncrement(increment, fromDataAnnotation))
			{
				propertyBuilder.Metadata.SetIdentityIncrement(increment, fromDataAnnotation);
				return propertyBuilder;
			}
			return null;
		}

		public static bool CanSetIdentityColumnIncrement([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, int? increment, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
			return propertyBuilder.CanSetAnnotation("Dm:IdentityIncrement", increment, fromDataAnnotation);
		}

		public static IConventionPropertyBuilder HasValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, DmValueGenerationStrategy? valueGenerationStrategy, bool fromDataAnnotation = false)
		{
			if (propertyBuilder.CanSetAnnotation("Dm:ValueGenerationStrategy", valueGenerationStrategy, fromDataAnnotation))
			{
				propertyBuilder.Metadata.SetValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
				if (valueGenerationStrategy != DmValueGenerationStrategy.IdentityColumn)
				{
					propertyBuilder.HasIdentityColumnSeed(null, fromDataAnnotation);
					propertyBuilder.HasIdentityColumnIncrement(null, fromDataAnnotation);
				}
				if (valueGenerationStrategy != DmValueGenerationStrategy.SequenceHiLo)
				{
					propertyBuilder.HasHiLoSequence(null, null, fromDataAnnotation);
				}
				return propertyBuilder;
			}
			return null;
		}

		public static bool CanSetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, DmValueGenerationStrategy? valueGenerationStrategy, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBuilder, "propertyBuilder");
			return (!valueGenerationStrategy.HasValue || DmPropertyExtensions.IsCompatibleWithValueGeneration(propertyBuilder.Metadata)) && propertyBuilder.CanSetAnnotation("Dm:ValueGenerationStrategy", valueGenerationStrategy, fromDataAnnotation);
		}

		[Obsolete("Use UseHiLo")]
		public static PropertyBuilder ForSqlServerUseSequenceHiLo([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name = null, [JetBrains.Annotations.CanBeNull] string schema = null)
		{
			return propertyBuilder.UseHiLo(name, schema);
		}

		[Obsolete("Use UseHiLo")]
		public static PropertyBuilder<TProperty> ForSqlServerUseSequenceHiLo<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, [JetBrains.Annotations.CanBeNull] string name = null, [JetBrains.Annotations.CanBeNull] string schema = null)
		{
			return propertyBuilder.UseHiLo(name, schema);
		}

		[Obsolete("Use HasHiLoSequence")]
		public static IConventionSequenceBuilder ForSqlServerHasHiLoSequence([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			return propertyBuilder.HasHiLoSequence(name, schema);
		}

		[Obsolete("Use UseIdentityColumn")]
		public static PropertyBuilder UseSqlServerIdentityColumn([JetBrains.Annotations.NotNull] this PropertyBuilder propertyBuilder, int seed = 1, int increment = 1)
		{
			return propertyBuilder.UseIdentityColumn(seed, increment);
		}

		[Obsolete("Use UseIdentityColumn")]
		public static PropertyBuilder<TProperty> UseSqlServerIdentityColumn<TProperty>([JetBrains.Annotations.NotNull] this PropertyBuilder<TProperty> propertyBuilder, int seed = 1, int increment = 1)
		{
			return propertyBuilder.UseIdentityColumn(seed, increment);
		}

		[Obsolete("Use HasIdentityColumnSeed")]
		public static IConventionPropertyBuilder ForSqlServerHasIdentitySeed([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, int? seed, bool fromDataAnnotation = false)
		{
			return propertyBuilder.HasIdentityColumnSeed(seed, fromDataAnnotation);
		}

		[Obsolete("Use HasIdentityColumnIncrement")]
		public static IConventionPropertyBuilder ForSqlServerHasIdentityIncrement([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, int? increment, bool fromDataAnnotation = false)
		{
			return propertyBuilder.HasIdentityColumnIncrement(increment, fromDataAnnotation);
		}

		[Obsolete("Use HasValueGenerationStrategy")]
		public static IConventionPropertyBuilder ForSqlServerHasValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionPropertyBuilder propertyBuilder, DmValueGenerationStrategy? valueGenerationStrategy, bool fromDataAnnotation = false)
		{
			return propertyBuilder.HasValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
		}
	}
}
