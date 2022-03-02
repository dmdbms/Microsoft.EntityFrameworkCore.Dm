using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
	public class DmValueGenerationConvention : RelationalValueGenerationConvention
	{
		public DmValueGenerationConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
		}

		public override void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
		{
			if (name == "Dm:ValueGenerationStrategy")
			{
				propertyBuilder.ValueGenerated(GetValueGenerated(propertyBuilder.Metadata));
			}
			else
			{
				base.ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, context);
			}
		}

		protected override ValueGenerated? GetValueGenerated(IConventionProperty property)
		{
			return GetValueGenerated(property);
		}

		public new static ValueGenerated? GetValueGenerated([JetBrains.Annotations.NotNull] IProperty property)
		{
			return ValueGenerationConvention.GetValueGenerated(property) ?? ((property.GetValueGenerationStrategy() != 0) ? new ValueGenerated?(ValueGenerated.OnAdd) : null);
		}
	}
}
