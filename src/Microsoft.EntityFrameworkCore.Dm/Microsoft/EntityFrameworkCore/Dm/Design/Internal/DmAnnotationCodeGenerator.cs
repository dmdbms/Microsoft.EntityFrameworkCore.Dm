// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Design.Internal.DmAnnotationCodeGenerator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

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
      Check.NotNull<IModel>(model, nameof (model));
      Check.NotNull<IAnnotation>(annotation, nameof (annotation));
      return annotation.Name == "Relational:DefaultSchema" && string.Equals("SYSDBA", (string) annotation.Value);
    }
  }
}
