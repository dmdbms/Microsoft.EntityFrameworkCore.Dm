using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
	public class DmValueGenerationStrategyConvention : IModelInitializedConvention, IConvention, IModelFinalizingConvention
	{
		protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

		public DmValueGenerationStrategyConvention([NotNull] ProviderConventionSetBuilderDependencies dependencies, [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		{
			Dependencies = dependencies;
		}

		public virtual void ProcessModelInitialized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
		{
			modelBuilder.HasValueGenerationStrategy(DmValueGenerationStrategy.IdentityColumn);
		}

		public virtual void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
			{
				foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
				{
					DmValueGenerationStrategy? dmValueGenerationStrategy = null;
					string tableName = RelationalEntityTypeExtensions.GetTableName((IReadOnlyEntityType)(object)entityType);
					if (tableName != null)
					{
						StoreObjectIdentifier storeObject2 = StoreObjectIdentifier.Table(tableName, RelationalEntityTypeExtensions.GetSchema((IReadOnlyEntityType)(object)entityType));
						dmValueGenerationStrategy = ((IReadOnlyProperty)(object)declaredProperty).GetValueGenerationStrategy(in storeObject2);
						if (dmValueGenerationStrategy == DmValueGenerationStrategy.None && !IsStrategyNoneNeeded((IReadOnlyProperty)(object)declaredProperty, storeObject2))
						{
							dmValueGenerationStrategy = null;
						}
					}
					else
					{
						string viewName = RelationalEntityTypeExtensions.GetViewName((IReadOnlyEntityType)(object)entityType);
						if (viewName != null)
						{
							StoreObjectIdentifier storeObject3 = StoreObjectIdentifier.View(viewName, RelationalEntityTypeExtensions.GetViewSchema((IReadOnlyEntityType)(object)entityType));
							dmValueGenerationStrategy = ((IReadOnlyProperty)(object)declaredProperty).GetValueGenerationStrategy(in storeObject3);
							if (dmValueGenerationStrategy == DmValueGenerationStrategy.None && !IsStrategyNoneNeeded((IReadOnlyProperty)(object)declaredProperty, storeObject3))
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
			static bool IsStrategyNoneNeeded(IReadOnlyProperty property, StoreObjectIdentifier storeObject)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Invalid comparison between Unknown and I4
				if ((int)property.ValueGenerated == 1 && RelationalPropertyExtensions.GetDefaultValue(property, in storeObject) == null && RelationalPropertyExtensions.GetDefaultValueSql(property, in storeObject) == null && RelationalPropertyExtensions.GetComputedColumnSql(property, in storeObject) == null && ((IReadOnlyTypeBase)property.DeclaringEntityType).Model.GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn)
				{
					object obj = property.GetValueConverter();
					if (obj == null)
					{
						RelationalTypeMapping obj2 = RelationalPropertyExtensions.FindRelationalTypeMapping(property, in storeObject);
						obj = ((obj2 != null) ? ((CoreTypeMapping)obj2).Converter : null);
					}
					Type type = ((obj != null) ? ((ValueConverter)obj).ProviderClrType.UnwrapNullableType() : null);
					return type != null && (type.IsInteger() || type == typeof(decimal));
				}
				return false;
			}
		}
	}
}
