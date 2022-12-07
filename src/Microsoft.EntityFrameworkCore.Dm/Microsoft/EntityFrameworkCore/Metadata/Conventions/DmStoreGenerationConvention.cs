// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Metadata.Conventions.DmStoreGenerationConvention
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;



namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
  public class DmStoreGenerationConvention : StoreGenerationConvention
  {
    public DmStoreGenerationConvention(
      [NotNull] ProviderConventionSetBuilderDependencies dependencies,
      [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
      : base(dependencies, relationalDependencies)
    {
    }

    public override void ProcessPropertyAnnotationChanged(
      IConventionPropertyBuilder propertyBuilder,
      string name,
      IConventionAnnotation annotation,
      IConventionAnnotation oldAnnotation,
      IConventionContext<IConventionAnnotation> context)
    {
      if (annotation == null || ((IAnnotation) oldAnnotation)?.Value != null)
        return;
      bool fromDataAnnotation = annotation.GetConfigurationSource() != (ConfigurationSource)2;
      string str = name;
      if (!(str == "Relational:DefaultValue"))
      {
        if (!(str == "Relational:DefaultValueSql"))
        {
          if (!(str == "Relational:ComputedColumnSql"))
          {
            if (str == "Dm:ValueGenerationStrategy" && RelationalPropertyBuilderExtensions.HasDefaultValue(propertyBuilder, (object) null, fromDataAnnotation) == null | RelationalPropertyBuilderExtensions.HasDefaultValueSql(propertyBuilder, (string) null, fromDataAnnotation) == null | RelationalPropertyBuilderExtensions.HasComputedColumnSql(propertyBuilder, (string) null, fromDataAnnotation) == null && propertyBuilder.HasValueGenerationStrategy(new DmValueGenerationStrategy?(), fromDataAnnotation) != null)
            {
              ((IConventionContext) context).StopProcessing();
              return;
            }
          }
          else if (propertyBuilder.HasValueGenerationStrategy(new DmValueGenerationStrategy?(), fromDataAnnotation) == null && RelationalPropertyBuilderExtensions.HasComputedColumnSql(propertyBuilder, (string) null, fromDataAnnotation) != null)
          {
            ((IConventionContext) context).StopProcessing();
            return;
          }
        }
        else if (propertyBuilder.HasValueGenerationStrategy(new DmValueGenerationStrategy?(), fromDataAnnotation) == null && RelationalPropertyBuilderExtensions.HasDefaultValueSql(propertyBuilder, (string) null, fromDataAnnotation) != null)
        {
          ((IConventionContext) context).StopProcessing();
          return;
        }
      }
      else if (propertyBuilder.HasValueGenerationStrategy(new DmValueGenerationStrategy?(), fromDataAnnotation) == null && RelationalPropertyBuilderExtensions.HasDefaultValue(propertyBuilder, (object) null, fromDataAnnotation) != null)
      {
        ((IConventionContext) context).StopProcessing();
        return;
      }
      base.ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, context);
    }

    protected override void Validate(
      IConventionProperty property,
      in StoreObjectIdentifier storeObject)
    {
      if (property.GetValueGenerationStrategyConfigurationSource().HasValue && ((IReadOnlyProperty) property).GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.None)
        base.Validate(property, in storeObject);
      else
        base.Validate(property, in storeObject);
    }
  }
}
