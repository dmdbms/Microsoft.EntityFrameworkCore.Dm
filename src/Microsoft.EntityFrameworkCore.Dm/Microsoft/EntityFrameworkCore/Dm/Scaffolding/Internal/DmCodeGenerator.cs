// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal.DmCodeGenerator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;



namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
  public class DmCodeGenerator : ProviderCodeGenerator
  {
    public DmCodeGenerator([NotNull] ProviderCodeGeneratorDependencies dependencies)
      : base(dependencies)
    {
    }

    public override MethodCallCodeFragment GenerateUseProvider(
      string connectionString,
      MethodCallCodeFragment providerOptions)
    {
      object[] objArray;
      if (providerOptions != null)
        objArray = new object[2]
        {
          (object) connectionString,
          (object) new NestedClosureCodeFragment("x", providerOptions)
        };
      else
        objArray = new object[1]
        {
          (object) connectionString
        };
      return new MethodCallCodeFragment("UseDm", objArray);
    }
  }
}
