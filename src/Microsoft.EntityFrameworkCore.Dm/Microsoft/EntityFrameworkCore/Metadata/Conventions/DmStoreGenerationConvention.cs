using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
	public class DmStoreGenerationConvention : StoreGenerationConvention
	{
		public DmStoreGenerationConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
		}

		public override void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
		{
			if (annotation == null || oldAnnotation?.Value != null)
			{
				return;
			}
			ConfigurationSource configurationSource = annotation.GetConfigurationSource();
			bool fromDataAnnotation = configurationSource != ConfigurationSource.Convention;
			switch (name)
			{
			case "Relational:DefaultValue":
				if (propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) == null && propertyBuilder.HasDefaultValue(null, fromDataAnnotation) != null)
				{
					context.StopProcessing();
					return;
				}
				break;
			case "Relational:DefaultValueSql":
				if (propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) == null && propertyBuilder.HasDefaultValueSql(null, fromDataAnnotation) != null)
				{
					context.StopProcessing();
					return;
				}
				break;
			case "Relational:ComputedColumnSql":
				if (propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) == null && propertyBuilder.HasComputedColumnSql(null, fromDataAnnotation) != null)
				{
					context.StopProcessing();
					return;
				}
				break;
			case "Dm:ValueGenerationStrategy":
				if (((propertyBuilder.HasDefaultValue(null, fromDataAnnotation) == null) | (propertyBuilder.HasDefaultValueSql(null, fromDataAnnotation) == null) | (propertyBuilder.HasComputedColumnSql(null, fromDataAnnotation) == null)) && propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) != null)
				{
					context.StopProcessing();
					return;
				}
				break;
			}
			base.ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, context);
		}

		protected override void Validate(IConventionProperty property, in StoreObjectIdentifier storeObject)
		{
			if (property.GetValueGenerationStrategyConfigurationSource().HasValue && property.GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.None)
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
