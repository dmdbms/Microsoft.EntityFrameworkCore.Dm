using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Dm.Design.Internal
{
	public class DmDesignTimeServices : IDesignTimeServices
	{
		public virtual void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
		{
			ServiceCollectionServiceExtensions.AddSingleton<IAnnotationCodeGenerator, DmAnnotationCodeGenerator>(ServiceCollectionServiceExtensions.AddSingleton<IProviderConfigurationCodeGenerator, DmCodeGenerator>(ServiceCollectionServiceExtensions.AddSingleton<IDatabaseModelFactory, DmDatabaseModelFactory>(ServiceCollectionServiceExtensions.AddSingleton<IRelationalTypeMappingSource, DmTypeMappingSource>(serviceCollection))));
		}
	}
}
