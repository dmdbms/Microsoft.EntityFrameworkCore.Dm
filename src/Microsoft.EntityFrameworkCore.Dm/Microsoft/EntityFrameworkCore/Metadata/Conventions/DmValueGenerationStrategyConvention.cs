// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Metadata.Conventions.DmValueGenerationStrategyConvention
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;



namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
  public class DmValueGenerationStrategyConvention : 
    IModelInitializedConvention,
    IConvention,
    IModelFinalizingConvention
  {
    public DmValueGenerationStrategyConvention(
      [NotNull] ProviderConventionSetBuilderDependencies dependencies,
      [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
    {
      this.Dependencies = dependencies;
    }

    protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

    public virtual void ProcessModelInitialized(
      IConventionModelBuilder modelBuilder,
      IConventionContext<IConventionModelBuilder> context)
    {
      modelBuilder.HasValueGenerationStrategy(new DmValueGenerationStrategy?(DmValueGenerationStrategy.IdentityColumn));
    }

    public virtual void ProcessModelFinalizing(
      IConventionModelBuilder modelBuilder,
      IConventionContext<IConventionModelBuilder> context)
    {
      foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
      {
        foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
        {
          DmValueGenerationStrategy? valueGenerationStrategy = new DmValueGenerationStrategy?();
          string tableName = RelationalEntityTypeExtensions.GetTableName((IReadOnlyEntityType) entityType);
          if (tableName != null)
          {
            StoreObjectIdentifier storeObject = StoreObjectIdentifier.Table(tableName, RelationalEntityTypeExtensions.GetSchema((IReadOnlyEntityType) entityType));
            valueGenerationStrategy = new DmValueGenerationStrategy?(((IReadOnlyProperty) declaredProperty).GetValueGenerationStrategy(in storeObject));
            DmValueGenerationStrategy? nullable = valueGenerationStrategy;
            DmValueGenerationStrategy generationStrategy = DmValueGenerationStrategy.None;
            if (nullable.GetValueOrDefault() == generationStrategy & nullable.HasValue && !IsStrategyNoneNeeded((IReadOnlyProperty) declaredProperty, storeObject))
              valueGenerationStrategy = new DmValueGenerationStrategy?();
          }
          else
          {
            string viewName = RelationalEntityTypeExtensions.GetViewName((IReadOnlyEntityType) entityType);
            if (viewName != null)
            {
              StoreObjectIdentifier storeObject = StoreObjectIdentifier.View(viewName, RelationalEntityTypeExtensions.GetViewSchema((IReadOnlyEntityType) entityType));
              valueGenerationStrategy = new DmValueGenerationStrategy?(((IReadOnlyProperty) declaredProperty).GetValueGenerationStrategy(in storeObject));
              DmValueGenerationStrategy? nullable = valueGenerationStrategy;
              DmValueGenerationStrategy generationStrategy = DmValueGenerationStrategy.None;
              if (nullable.GetValueOrDefault() == generationStrategy & nullable.HasValue && !IsStrategyNoneNeeded((IReadOnlyProperty) declaredProperty, storeObject))
                valueGenerationStrategy = new DmValueGenerationStrategy?();
            }
          }
          if (valueGenerationStrategy.HasValue)
            declaredProperty.Builder.HasValueGenerationStrategy(valueGenerationStrategy);
        }
      }

      static bool IsStrategyNoneNeeded(
        IReadOnlyProperty property,
        StoreObjectIdentifier storeObject)
      {
        int num;
        if (property.ValueGenerated == (ValueGenerated)1 && RelationalPropertyExtensions.GetDefaultValue(property, in storeObject) == null && RelationalPropertyExtensions.GetDefaultValueSql(property, in storeObject) == null && RelationalPropertyExtensions.GetComputedColumnSql(property, in storeObject) == null)
        {
          DmValueGenerationStrategy? generationStrategy1 = ((IReadOnlyTypeBase) property.DeclaringEntityType).Model.GetValueGenerationStrategy();
          DmValueGenerationStrategy generationStrategy2 = DmValueGenerationStrategy.IdentityColumn;
          num = generationStrategy1.GetValueOrDefault() == generationStrategy2 & generationStrategy1.HasValue ? 1 : 0;
        }
        else
          num = 0;
        if (num == 0)
          return false;
        ValueConverter valueConverter = property.GetValueConverter() ?? ((CoreTypeMapping) RelationalPropertyExtensions.FindRelationalTypeMapping(property, in storeObject))?.Converter;
        Type type = valueConverter != null ? valueConverter.ProviderClrType.UnwrapNullableType() : (Type) null;
        return type != (Type) null && (type.IsInteger() || type == typeof (Decimal));
      }
    }
  }
}
