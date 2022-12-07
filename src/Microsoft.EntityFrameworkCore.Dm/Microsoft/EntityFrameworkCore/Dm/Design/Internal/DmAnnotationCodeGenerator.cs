using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Dm.Design.Internal
{
	public class DmAnnotationCodeGenerator : AnnotationCodeGenerator
	{
		public DmAnnotationCodeGenerator([NotNull] AnnotationCodeGeneratorDependencies dependencies)
			: base(dependencies)
		{
		}

		protected override bool IsHandledByConvention(IModel model, IAnnotation annotation)
		{
			Check.NotNull<IModel>(model, "model");
			Check.NotNull<IAnnotation>(annotation, "annotation");
			if (annotation.Name == "Relational:DefaultSchema" && string.Equals("SYSDBA", (string)annotation.Value))
			{
				return true;
			}
			return false;
		}
	}
}
