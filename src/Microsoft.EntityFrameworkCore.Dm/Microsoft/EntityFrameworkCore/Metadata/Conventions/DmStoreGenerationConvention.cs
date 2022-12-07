using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
	public class DmStoreGenerationConvention : StoreGenerationConvention
	{
		public DmStoreGenerationConvention([NotNull] ProviderConventionSetBuilderDependencies dependencies, [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
		}

		public override void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			if (annotation == null || ((oldAnnotation != null) ? ((IAnnotation)oldAnnotation).Value : null) != null)
			{
				return;
			}
			ConfigurationSource configurationSource = annotation.GetConfigurationSource();
			bool flag = (int)configurationSource != 2;
			switch (name)
			{
			case "Relational:DefaultValue":
				if (propertyBuilder.HasValueGenerationStrategy(null, flag) == null && RelationalPropertyBuilderExtensions.HasDefaultValue(propertyBuilder, (object)null, flag) != null)
				{
					((IConventionContext)context).StopProcessing();
					return;
				}
				break;
			case "Relational:DefaultValueSql":
				if (propertyBuilder.HasValueGenerationStrategy(null, flag) == null && RelationalPropertyBuilderExtensions.HasDefaultValueSql(propertyBuilder, (string)null, flag) != null)
				{
					((IConventionContext)context).StopProcessing();
					return;
				}
				break;
			case "Relational:ComputedColumnSql":
				if (propertyBuilder.HasValueGenerationStrategy(null, flag) == null && RelationalPropertyBuilderExtensions.HasComputedColumnSql(propertyBuilder, (string)null, flag) != null)
				{
					((IConventionContext)context).StopProcessing();
					return;
				}
				break;
			case "Dm:ValueGenerationStrategy":
				if (((RelationalPropertyBuilderExtensions.HasDefaultValue(propertyBuilder, (object)null, flag) == null) | (RelationalPropertyBuilderExtensions.HasDefaultValueSql(propertyBuilder, (string)null, flag) == null) | (RelationalPropertyBuilderExtensions.HasComputedColumnSql(propertyBuilder, (string)null, flag) == null)) && propertyBuilder.HasValueGenerationStrategy(null, flag) != null)
				{
					((IConventionContext)context).StopProcessing();
					return;
				}
				break;
			}
			((StoreGenerationConvention)this).ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, context);
		}

		protected override void Validate(IConventionProperty property, in StoreObjectIdentifier storeObject)
		{
			if (property.GetValueGenerationStrategyConfigurationSource().HasValue && ((IReadOnlyProperty)(object)property).GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.None)
			{
				base.Validate(property, in storeObject);
			}
			else
			{
				base.Validate(property, in storeObject);
			}
		}
	}
}
