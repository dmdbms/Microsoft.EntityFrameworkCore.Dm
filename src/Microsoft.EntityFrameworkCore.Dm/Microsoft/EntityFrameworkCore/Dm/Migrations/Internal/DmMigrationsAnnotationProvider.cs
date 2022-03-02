using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.Dm.Migrations.Internal
{
	public class DmMigrationsAnnotationProvider : MigrationsAnnotationProvider
	{
		public DmMigrationsAnnotationProvider([JetBrains.Annotations.NotNull] MigrationsAnnotationProviderDependencies dependencies)
			: base(dependencies)
		{
		}
	}
}
