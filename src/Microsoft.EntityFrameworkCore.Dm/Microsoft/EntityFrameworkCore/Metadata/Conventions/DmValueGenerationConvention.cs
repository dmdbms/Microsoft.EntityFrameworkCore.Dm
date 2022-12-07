// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Metadata.Conventions.DmValueGenerationConvention
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;



namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
  public class DmValueGenerationConvention : RelationalValueGenerationConvention
  {
    public DmValueGenerationConvention(
      [NotNull] ProviderConventionSetBuilderDependencies dependencies,
      [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
      : base(dependencies, relationalDependencies)
    {
    }

    public virtual void ProcessPropertyAnnotationChanged(
      IConventionPropertyBuilder propertyBuilder,
      string name,
      IConventionAnnotation annotation,
      IConventionAnnotation oldAnnotation,
      IConventionContext<IConventionAnnotation> context)
    {
      if (name == "Dm:ValueGenerationStrategy")
        propertyBuilder.ValueGenerated(base.GetValueGenerated(propertyBuilder.Metadata), false);
      else
        base.ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, context);
    }

    protected virtual ValueGenerated? GetValueGenerated(IConventionProperty property) => base.GetValueGenerated(property);

    public static ValueGenerated? GetValueGenerated([NotNull] IProperty property)
    {
      ValueGenerated? valueGenerated = ValueGenerationConvention.GetValueGenerated((IReadOnlyProperty) property);
      if (valueGenerated.HasValue)
        return valueGenerated;
      return ((IReadOnlyProperty) property).GetValueGenerationStrategy() == DmValueGenerationStrategy.None ? new ValueGenerated?() : new ValueGenerated?((ValueGenerated) 1);
    }
  }
}
