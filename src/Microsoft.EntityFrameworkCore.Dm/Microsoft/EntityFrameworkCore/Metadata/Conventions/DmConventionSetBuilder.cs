// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Metadata.Conventions.DmConventionSetBuilder
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;



namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
  public class DmConventionSetBuilder : RelationalConventionSetBuilder
  {
    private readonly ISqlGenerationHelper _sqlGenerationHelper;

    public DmConventionSetBuilder(
      [NotNull] ProviderConventionSetBuilderDependencies dependencies,
      [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies,
      [NotNull] ISqlGenerationHelper sqlGenerationHelper)
      : base(dependencies, relationalDependencies)
    {
      this._sqlGenerationHelper = sqlGenerationHelper;
    }

    public virtual ConventionSet CreateConventionSet()
    {
      ConventionSet conventionSet = base.CreateConventionSet();
      DmValueGenerationStrategyConvention strategyConvention = new DmValueGenerationStrategyConvention(base.Dependencies, this.RelationalDependencies);
      ((ICollection<IModelInitializedConvention>) conventionSet.ModelInitializedConventions).Add((IModelInitializedConvention) strategyConvention);
      ((ICollection<IModelInitializedConvention>) conventionSet.ModelInitializedConventions).Add((IModelInitializedConvention) new RelationalMaxIdentifierLengthConvention(128, base.Dependencies, this.RelationalDependencies));
      ValueGenerationConvention generationConvention1 = (ValueGenerationConvention) new DmValueGenerationConvention(base.Dependencies, this.RelationalDependencies);
      base.ReplaceConvention<IEntityTypeBaseTypeChangedConvention, ValueGenerationConvention>(conventionSet.EntityTypeBaseTypeChangedConventions, generationConvention1);
      base.ReplaceConvention<IEntityTypeAnnotationChangedConvention, RelationalValueGenerationConvention>(conventionSet.EntityTypeAnnotationChangedConventions, (RelationalValueGenerationConvention) generationConvention1);
      base.ReplaceConvention<IEntityTypePrimaryKeyChangedConvention, ValueGenerationConvention>(conventionSet.EntityTypePrimaryKeyChangedConventions, generationConvention1);
      base.ReplaceConvention<IForeignKeyAddedConvention, ValueGenerationConvention>(conventionSet.ForeignKeyAddedConventions, generationConvention1);
      base.ReplaceConvention<IForeignKeyRemovedConvention, ValueGenerationConvention>(conventionSet.ForeignKeyRemovedConventions, generationConvention1);
      StoreGenerationConvention generationConvention2 = (StoreGenerationConvention) new DmStoreGenerationConvention(base.Dependencies, this.RelationalDependencies);
      base.ReplaceConvention<IPropertyAnnotationChangedConvention, StoreGenerationConvention>(conventionSet.PropertyAnnotationChangedConventions, generationConvention2);
      base.ReplaceConvention<IPropertyAnnotationChangedConvention, RelationalValueGenerationConvention>(conventionSet.PropertyAnnotationChangedConventions, (RelationalValueGenerationConvention) generationConvention1);
      ((ICollection<IModelFinalizingConvention>) conventionSet.ModelFinalizingConventions).Add((IModelFinalizingConvention) strategyConvention);
      base.ReplaceConvention<IModelFinalizingConvention, StoreGenerationConvention>(conventionSet.ModelFinalizingConventions, generationConvention2);
      return conventionSet;
    }

    public static ConventionSet Build()
    {
      using (IServiceScope scope = EntityFrameworkServiceCollectionExtensions.AddDbContext<DbContext>(new ServiceCollection().AddEntityFrameworkDm(), (Action<IServiceProvider, DbContextOptionsBuilder>) ((p, o) => o.UseDm("Server=.").UseInternalServiceProvider(p)), ServiceLifetime.Scoped, ServiceLifetime.Scoped).BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        using (DbContext service = scope.ServiceProvider.GetService<DbContext>())
          return ConventionSet.CreateConventionSet(service);
      }
    }
  }
}
