using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
	public class DmConventionSetBuilder : RelationalConventionSetBuilder
	{
		private readonly ISqlGenerationHelper _sqlGenerationHelper;

		public DmConventionSetBuilder([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
			: base(dependencies, relationalDependencies)
		{
			_sqlGenerationHelper = sqlGenerationHelper;
		}

		public override ConventionSet CreateConventionSet()
		{
			ConventionSet conventionSet = base.CreateConventionSet();
			DmValueGenerationStrategyConvention item = new DmValueGenerationStrategyConvention(Dependencies, RelationalDependencies);
			conventionSet.ModelInitializedConventions.Add(item);
			conventionSet.ModelInitializedConventions.Add(new RelationalMaxIdentifierLengthConvention(128, Dependencies, RelationalDependencies));
			ValueGenerationConvention valueGenerationConvention = new DmValueGenerationConvention(Dependencies, RelationalDependencies);
			ReplaceConvention(conventionSet.EntityTypeBaseTypeChangedConventions, valueGenerationConvention);
			ReplaceConvention(conventionSet.EntityTypeAnnotationChangedConventions, (RelationalValueGenerationConvention)valueGenerationConvention);
			ReplaceConvention(conventionSet.EntityTypePrimaryKeyChangedConventions, valueGenerationConvention);
			ReplaceConvention(conventionSet.ForeignKeyAddedConventions, valueGenerationConvention);
			ReplaceConvention(conventionSet.ForeignKeyRemovedConventions, valueGenerationConvention);
			StoreGenerationConvention newConvention = new DmStoreGenerationConvention(Dependencies, RelationalDependencies);
			ReplaceConvention(conventionSet.PropertyAnnotationChangedConventions, newConvention);
			ReplaceConvention(conventionSet.PropertyAnnotationChangedConventions, (RelationalValueGenerationConvention)valueGenerationConvention);
			conventionSet.ModelFinalizingConventions.Add(item);
			ReplaceConvention(conventionSet.ModelFinalizingConventions, newConvention);
			return conventionSet;
		}

		public static ConventionSet Build()
		{
			ServiceProvider provider = new ServiceCollection().AddEntityFrameworkDm().AddDbContext<DbContext>(delegate(IServiceProvider p, DbContextOptionsBuilder o)
			{
				o.UseDm("Server=.").UseInternalServiceProvider(p);
			}).BuildServiceProvider();
			using IServiceScope serviceScope = ServiceProviderServiceExtensions.GetRequiredService<IServiceScopeFactory>(provider).CreateScope();
			using DbContext context = ServiceProviderServiceExtensions.GetService<DbContext>(serviceScope.ServiceProvider);
			return ConventionSet.CreateConventionSet(context);
		}
	}
}
