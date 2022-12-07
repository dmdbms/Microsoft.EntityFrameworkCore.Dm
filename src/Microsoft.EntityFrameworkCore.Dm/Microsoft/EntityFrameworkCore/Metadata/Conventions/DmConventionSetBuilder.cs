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

		public DmConventionSetBuilder([NotNull] ProviderConventionSetBuilderDependencies dependencies, [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies, [NotNull] ISqlGenerationHelper sqlGenerationHelper)
			: base(dependencies, relationalDependencies)
		{
			_sqlGenerationHelper = sqlGenerationHelper;
		}

		public override ConventionSet CreateConventionSet()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			ConventionSet val = base.CreateConventionSet();
			DmValueGenerationStrategyConvention item = new DmValueGenerationStrategyConvention(base.Dependencies, base.RelationalDependencies);
			val.ModelInitializedConventions.Add((IModelInitializedConvention)(object)item);
			val.ModelInitializedConventions.Add((IModelInitializedConvention)new RelationalMaxIdentifierLengthConvention(128, base.Dependencies, base.RelationalDependencies));
			ValueGenerationConvention val2 = (ValueGenerationConvention)(object)new DmValueGenerationConvention(base.Dependencies, base.RelationalDependencies);
			base.ReplaceConvention<IEntityTypeBaseTypeChangedConvention, ValueGenerationConvention>(val.EntityTypeBaseTypeChangedConventions, val2);
			base.ReplaceConvention<IEntityTypeAnnotationChangedConvention, RelationalValueGenerationConvention>(val.EntityTypeAnnotationChangedConventions, (RelationalValueGenerationConvention)val2);
			base.ReplaceConvention<IEntityTypePrimaryKeyChangedConvention, ValueGenerationConvention>(val.EntityTypePrimaryKeyChangedConventions, val2);
			base.ReplaceConvention<IForeignKeyAddedConvention, ValueGenerationConvention>(val.ForeignKeyAddedConventions, val2);
			base.ReplaceConvention<IForeignKeyRemovedConvention, ValueGenerationConvention>(val.ForeignKeyRemovedConventions, val2);
			StoreGenerationConvention val3 = (StoreGenerationConvention)(object)new DmStoreGenerationConvention(base.Dependencies, base.RelationalDependencies);
			base.ReplaceConvention<IPropertyAnnotationChangedConvention, StoreGenerationConvention>(val.PropertyAnnotationChangedConventions, val3);
			base.ReplaceConvention<IPropertyAnnotationChangedConvention, RelationalValueGenerationConvention>(val.PropertyAnnotationChangedConventions, (RelationalValueGenerationConvention)val2);
			val.ModelFinalizingConventions.Add((IModelFinalizingConvention)(object)item);
			base.ReplaceConvention<IModelFinalizingConvention, StoreGenerationConvention>(val.ModelFinalizingConventions, val3);
			return val;
		}

		public static ConventionSet Build()
		{
			ServiceProvider provider = EntityFrameworkServiceCollectionExtensions.AddDbContext<DbContext>(new ServiceCollection().AddEntityFrameworkDm(), (Action<IServiceProvider, DbContextOptionsBuilder>)delegate(IServiceProvider p, DbContextOptionsBuilder o)
			{
				o.UseDm("Server=.").UseInternalServiceProvider(p);
			}, ServiceLifetime.Scoped, ServiceLifetime.Scoped).BuildServiceProvider();
			using IServiceScope serviceScope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
			DbContext service = serviceScope.ServiceProvider.GetService<DbContext>();
			try
			{
				return ConventionSet.CreateConventionSet(service);
			}
			finally
			{
				((IDisposable)service)?.Dispose();
			}
		}
	}
}
