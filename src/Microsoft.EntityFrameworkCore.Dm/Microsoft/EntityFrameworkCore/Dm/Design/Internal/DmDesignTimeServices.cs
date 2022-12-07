// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Design.Internal.DmDesignTimeServices
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

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
      Check.NotNull<IServiceCollection>(serviceCollection, nameof (serviceCollection));
      serviceCollection.AddEntityFrameworkDm();
      ((EntityFrameworkServicesBuilder) new EntityFrameworkRelationalDesignServicesBuilder(serviceCollection)).TryAdd<IAnnotationCodeGenerator, DmAnnotationCodeGenerator>().TryAdd<IDatabaseModelFactory, DmDatabaseModelFactory>().TryAdd<IProviderConfigurationCodeGenerator, DmCodeGenerator>().TryAddCoreServices();
    }
  }
}
