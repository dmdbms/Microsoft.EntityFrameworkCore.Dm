using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
	public static class DmModelBuilderExtensions
	{
		public static ModelBuilder UseHiLo([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string name = null, [JetBrains.Annotations.CanBeNull] string schema = null)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
			IMutableModel model = modelBuilder.Model;
			if (name == null)
			{
				name = "EntityFrameworkHiLoSequence";
			}
			if (model.FindSequence(name, schema) == null)
			{
				modelBuilder.HasSequence(name, schema).IncrementsBy(10);
			}
			model.SetValueGenerationStrategy(DmValueGenerationStrategy.SequenceHiLo);
			model.SetHiLoSequenceName(name);
			model.SetHiLoSequenceSchema(schema);
			model.SetIdentitySeed(null);
			model.SetIdentityIncrement(null);
			return modelBuilder;
		}

		public static IConventionSequenceBuilder HasHiLoSequence([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			if (!modelBuilder.CanSetHiLoSequence(name, schema))
			{
				return null;
			}
			modelBuilder.Metadata.SetHiLoSequenceName(name, fromDataAnnotation);
			modelBuilder.Metadata.SetHiLoSequenceSchema(schema, fromDataAnnotation);
			return (name == null) ? null : modelBuilder.HasSequence(name, schema, fromDataAnnotation);
		}

		public static bool CanSetHiLoSequence([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
			Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
			return modelBuilder.CanSetAnnotation("Dm:HiLoSequenceName", name, fromDataAnnotation) && modelBuilder.CanSetAnnotation("Dm:HiLoSequenceSchema", schema, fromDataAnnotation);
		}

		public static ModelBuilder UseIdentityColumns([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, int seed = 1, int increment = 1)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			IMutableModel model = modelBuilder.Model;
			model.SetValueGenerationStrategy(DmValueGenerationStrategy.IdentityColumn);
			model.SetIdentitySeed(seed);
			model.SetIdentityIncrement(increment);
			model.SetHiLoSequenceName(null);
			model.SetHiLoSequenceSchema(null);
			return modelBuilder;
		}

		public static IConventionModelBuilder HasIdentityColumnSeed([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? seed, bool fromDataAnnotation = false)
		{
			if (modelBuilder.CanSetIdentityColumnSeed(seed, fromDataAnnotation))
			{
				modelBuilder.Metadata.SetIdentitySeed(seed, fromDataAnnotation);
				return modelBuilder;
			}
			return null;
		}

		public static bool CanSetIdentityColumnSeed([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? seed, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			return modelBuilder.CanSetAnnotation("Dm:IdentitySeed", seed, fromDataAnnotation);
		}

		public static IConventionModelBuilder HasIdentityColumnIncrement([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? increment, bool fromDataAnnotation = false)
		{
			if (modelBuilder.CanSetIdentityColumnIncrement(increment, fromDataAnnotation))
			{
				modelBuilder.Metadata.SetIdentityIncrement(increment, fromDataAnnotation);
				return modelBuilder;
			}
			return null;
		}

		public static bool CanSetIdentityColumnIncrement([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? increment, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			return modelBuilder.CanSetAnnotation("Dm:IdentityIncrement", increment, fromDataAnnotation);
		}

		public static IConventionModelBuilder HasValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, DmValueGenerationStrategy? valueGenerationStrategy, bool fromDataAnnotation = false)
		{
			if (modelBuilder.CanSetValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation))
			{
				modelBuilder.Metadata.SetValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
				if (valueGenerationStrategy != DmValueGenerationStrategy.IdentityColumn)
				{
					modelBuilder.HasIdentityColumnSeed(null, fromDataAnnotation);
					modelBuilder.HasIdentityColumnIncrement(null, fromDataAnnotation);
				}
				if (valueGenerationStrategy != DmValueGenerationStrategy.SequenceHiLo)
				{
					modelBuilder.HasHiLoSequence(null, null, fromDataAnnotation);
				}
				return modelBuilder;
			}
			return null;
		}

		public static bool CanSetValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, DmValueGenerationStrategy? valueGenerationStrategy, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			return modelBuilder.CanSetAnnotation("Dm:ValueGenerationStrategy", valueGenerationStrategy, fromDataAnnotation);
		}

		public static ModelBuilder HasDatabaseMaxSize([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string maxSize)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(maxSize, "maxSize");
			modelBuilder.Model.SetDatabaseMaxSize(maxSize);
			return modelBuilder;
		}

		public static IConventionModelBuilder HasDatabaseMaxSize([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string maxSize, bool fromDataAnnotation = false)
		{
			if (modelBuilder.CanSetDatabaseMaxSize(maxSize, fromDataAnnotation))
			{
				modelBuilder.Metadata.SetDatabaseMaxSize(maxSize, fromDataAnnotation);
				return modelBuilder;
			}
			return null;
		}

		public static bool CanSetDatabaseMaxSize([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string maxSize, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			return modelBuilder.CanSetAnnotation("Dm:DatabaseMaxSize", maxSize, fromDataAnnotation);
		}

		public static ModelBuilder HasServiceTier([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string serviceTier)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceTier, "serviceTier");
			modelBuilder.Model.SetServiceTierSql("'" + serviceTier.Replace("'", "''") + "'");
			return modelBuilder;
		}

		public static ModelBuilder HasServiceTierSql([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string serviceTier)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceTier, "serviceTier");
			modelBuilder.Model.SetServiceTierSql(serviceTier);
			return modelBuilder;
		}

		public static IConventionModelBuilder HasServiceTierSql([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string serviceTier, bool fromDataAnnotation = false)
		{
			if (modelBuilder.CanSetServiceTierSql(serviceTier, fromDataAnnotation))
			{
				modelBuilder.Metadata.SetServiceTierSql(serviceTier, fromDataAnnotation);
				return modelBuilder;
			}
			return null;
		}

		public static bool CanSetServiceTierSql([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string serviceTier, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			return modelBuilder.CanSetAnnotation("Dm:ServiceTierSql", serviceTier, fromDataAnnotation);
		}

		public static ModelBuilder HasPerformanceLevel([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string performanceLevel)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(performanceLevel, "performanceLevel");
			modelBuilder.Model.SetPerformanceLevelSql("'" + performanceLevel.Replace("'", "''") + "'");
			return modelBuilder;
		}

		public static ModelBuilder HasPerformanceLevelSql([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string performanceLevel)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(performanceLevel, "performanceLevel");
			modelBuilder.Model.SetPerformanceLevelSql(performanceLevel);
			return modelBuilder;
		}

		public static IConventionModelBuilder HasPerformanceLevelSql([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string performanceLevel, bool fromDataAnnotation = false)
		{
			if (modelBuilder.CanSetPerformanceLevelSql(performanceLevel, fromDataAnnotation))
			{
				modelBuilder.Metadata.SetPerformanceLevelSql(performanceLevel, fromDataAnnotation);
				return modelBuilder;
			}
			return null;
		}

		public static bool CanSetPerformanceLevelSql([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string performanceLevel, bool fromDataAnnotation = false)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
			return modelBuilder.CanSetAnnotation("Dm:PerformanceLevelSql", performanceLevel, fromDataAnnotation);
		}

		[Obsolete("Use UseHiLo")]
		public static ModelBuilder ForSqlServerUseSequenceHiLo([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string name = null, [JetBrains.Annotations.CanBeNull] string schema = null)
		{
			return modelBuilder.UseHiLo(name, schema);
		}

		[Obsolete("Use HasHiLoSequence")]
		public static IConventionSequenceBuilder ForSqlServerHasHiLoSequence([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string name, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
		{
			return modelBuilder.HasHiLoSequence(name, schema, fromDataAnnotation);
		}

		[Obsolete("Use UseIdentityColumns")]
		public static ModelBuilder ForSqlServerUseIdentityColumns([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, int seed = 1, int increment = 1)
		{
			return modelBuilder.UseIdentityColumns(seed, increment);
		}

		[Obsolete("Use HasIdentityColumnSeed")]
		public static IConventionModelBuilder ForSqlServerHasIdentitySeed([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? seed, bool fromDataAnnotation = false)
		{
			return modelBuilder.HasIdentityColumnSeed(seed, fromDataAnnotation);
		}

		[Obsolete("Use HasIdentityColumnIncrement")]
		public static IConventionModelBuilder ForSqlServerHasIdentityIncrement([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? increment, bool fromDataAnnotation = false)
		{
			return modelBuilder.HasIdentityColumnIncrement(increment, fromDataAnnotation);
		}

		[Obsolete("Use HasValueGenerationStrategy")]
		public static IConventionModelBuilder ForSqlServerHasValueGenerationStrategy([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, DmValueGenerationStrategy? valueGenerationStrategy, bool fromDataAnnotation = false)
		{
			return modelBuilder.HasValueGenerationStrategy(valueGenerationStrategy, fromDataAnnotation);
		}
	}
}
