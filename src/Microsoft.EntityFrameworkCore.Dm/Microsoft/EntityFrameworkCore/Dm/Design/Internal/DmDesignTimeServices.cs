using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;



namespace Microsoft.EntityFrameworkCore.Dm.Design.Internal
{
  public class DmDesignTimeServices : IDesignTimeServices
  {
    public virtual void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
    {
      Check.NotNull<IServiceCollection>(serviceCollection, nameof (serviceCollection));
      serviceCollection.AddEntityFrameworkDm();
      new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection).TryAdd<IAnnotationCodeGenerator, DmAnnotationCodeGenerator>().TryAdd<IDatabaseModelFactory, DmDatabaseModelFactory>().TryAdd<IProviderConfigurationCodeGenerator, DmCodeGenerator>().TryAddCoreServices();
    }
  }
}
