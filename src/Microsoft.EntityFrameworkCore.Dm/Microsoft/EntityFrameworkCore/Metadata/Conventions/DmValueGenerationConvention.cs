using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
	public class DmValueGenerationConvention : RelationalValueGenerationConvention
	{
		public DmValueGenerationConvention([NotNull] ProviderConventionSetBuilderDependencies dependencies, [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
		}

		public override void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
		{
			if (name == "Dm:ValueGenerationStrategy")
			{
				propertyBuilder.ValueGenerated(base.GetValueGenerated(propertyBuilder.Metadata), false);
			}
			else
			{
				((RelationalValueGenerationConvention)this).ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, context);
			}
		}

		protected override ValueGenerated? GetValueGenerated(IConventionProperty property)
		{
			return base.GetValueGenerated(property);
		}

		public static ValueGenerated? GetValueGenerated([NotNull] IProperty property)
		{
			return ValueGenerationConvention.GetValueGenerated((IReadOnlyProperty)(object)property) ?? ((((IReadOnlyProperty)(object)property).GetValueGenerationStrategy() != 0) ? new ValueGenerated?((ValueGenerated)1) : null);
		}
	}
}
