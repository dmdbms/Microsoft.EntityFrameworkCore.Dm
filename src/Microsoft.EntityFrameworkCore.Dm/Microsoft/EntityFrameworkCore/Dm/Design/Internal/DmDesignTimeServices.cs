using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Dm.Design.Internal
{
	public class DmDesignTimeServices : IDesignTimeServices
	{
		public virtual void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Check.NotNull(serviceCollection, "serviceCollection");
			serviceCollection.AddEntityFrameworkDm();
			((EntityFrameworkServicesBuilder)new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection)).TryAdd<IAnnotationCodeGenerator, DmAnnotationCodeGenerator>().TryAdd<IDatabaseModelFactory, DmDatabaseModelFactory>().TryAdd<IProviderConfigurationCodeGenerator, DmCodeGenerator>()
				.TryAddCoreServices();
		}
	}
}
