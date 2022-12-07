// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmQueryTranslationPostprocessor
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmQueryTranslationPostprocessor : RelationalQueryTranslationPostprocessor
  {
    public DmQueryTranslationPostprocessor(
      QueryTranslationPostprocessorDependencies dependencies,
      RelationalQueryTranslationPostprocessorDependencies relationalDependencies,
      QueryCompilationContext queryCompilationContext)
      : base(dependencies, relationalDependencies, queryCompilationContext)
    {
    }

    public override Expression Process(Expression query)
    {
      query = base.Process(query);
      query = ((ExpressionVisitor) new SearchConditionConvertingExpressionVisitor(this.RelationalDependencies.SqlExpressionFactory)).Visit(query);
      return query;
    }
  }
}
