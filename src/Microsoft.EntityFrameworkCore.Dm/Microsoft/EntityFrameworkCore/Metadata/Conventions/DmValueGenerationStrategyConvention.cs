using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
	public class DmValueGenerationStrategyConvention : IModelInitializedConvention, IConvention, IModelFinalizingConvention
	{
		protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

		public DmValueGenerationStrategyConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		{
			Dependencies = dependencies;
		}

		public virtual void ProcessModelInitialized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
		{
			modelBuilder.HasValueGenerationStrategy(DmValueGenerationStrategy.IdentityColumn);
		}

		public virtual void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
		{
			foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
			{
				foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
				{
					DmValueGenerationStrategy? dmValueGenerationStrategy = null;
					string tableName = entityType.GetTableName();
					if (tableName != null)
					{
						StoreObjectIdentifier storeObject2 = StoreObjectIdentifier.Table(tableName, entityType.GetSchema());
						dmValueGenerationStrategy = declaredProperty.GetValueGenerationStrategy(in storeObject2);
						if (dmValueGenerationStrategy == DmValueGenerationStrategy.None && !IsStrategyNoneNeeded(declaredProperty, storeObject2))
						{
							dmValueGenerationStrategy = null;
						}
					}
					else
					{
						string viewName = entityType.GetViewName();
						if (viewName != null)
						{
							StoreObjectIdentifier storeObject3 = StoreObjectIdentifier.View(viewName, entityType.GetViewSchema());
							dmValueGenerationStrategy = declaredProperty.GetValueGenerationStrategy(in storeObject3);
							if (dmValueGenerationStrategy == DmValueGenerationStrategy.None && !IsStrategyNoneNeeded(declaredProperty, storeObject3))
							{
								dmValueGenerationStrategy = null;
							}
						}
					}
					if (dmValueGenerationStrategy.HasValue)
					{
						declaredProperty.Builder.HasValueGenerationStrategy(dmValueGenerationStrategy);
					}
				}
			}
			static bool IsStrategyNoneNeeded(IProperty property, StoreObjectIdentifier storeObject)
			{
				if (property.ValueGenerated == ValueGenerated.OnAdd && property.GetDefaultValue(in storeObject) == null && property.GetDefaultValueSql(in storeObject) == null && property.GetComputedColumnSql(in storeObject) == null && property.DeclaringEntityType.Model.GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn)
				{
					ValueConverter obj = property.GetValueConverter() ?? property.FindRelationalTypeMapping(in storeObject)?.Converter;
					Type type = ((obj != null) ? Dm.Utilities.SharedTypeExtensions.UnwrapNullableType(obj.ProviderClrType) : null);
					return type != null && (Dm.Utilities.SharedTypeExtensions.IsInteger(type) || type == typeof(decimal));
				}
				return false;
			}
		}
	}
}
