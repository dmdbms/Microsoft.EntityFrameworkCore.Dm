using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Dm.Design.Internal
{
	public class DmAnnotationCodeGenerator : AnnotationCodeGenerator
	{
		public DmAnnotationCodeGenerator([JetBrains.Annotations.NotNull] AnnotationCodeGeneratorDependencies dependencies)
			: base(dependencies)
		{
		}

		protected override bool IsHandledByConvention(IModel model, IAnnotation annotation)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotation, "annotation");
			if (annotation.Name == "Relational:DefaultSchema" && string.Equals("SYSDBA", (string)annotation.Value))
			{
				return true;
			}
			return false;
		}
	}
}
