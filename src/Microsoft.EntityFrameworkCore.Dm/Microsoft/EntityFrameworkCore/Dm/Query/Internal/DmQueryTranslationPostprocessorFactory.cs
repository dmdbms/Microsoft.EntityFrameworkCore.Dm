// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmQueryTranslationPostprocessorFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Query;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory
  {
    private readonly QueryTranslationPostprocessorDependencies _dependencies;
    private readonly RelationalQueryTranslationPostprocessorDependencies _relationalDependencies;

    public DmQueryTranslationPostprocessorFactory(
      QueryTranslationPostprocessorDependencies dependencies,
      RelationalQueryTranslationPostprocessorDependencies relationalDependencies)
    {
      this._dependencies = dependencies;
      this._relationalDependencies = relationalDependencies;
    }

    public virtual QueryTranslationPostprocessor Create(
      QueryCompilationContext queryCompilationContext)
    {
      return (QueryTranslationPostprocessor) new DmQueryTranslationPostprocessor(this._dependencies, this._relationalDependencies, queryCompilationContext);
    }
  }
}
