// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmSqlTranslatingExpressionVisitorFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmSqlTranslatingExpressionVisitorFactory : 
    IRelationalSqlTranslatingExpressionVisitorFactory
  {
    private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

    public DmSqlTranslatingExpressionVisitorFactory(
      [NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies)
    {
      this._dependencies = dependencies;
    }

    public virtual RelationalSqlTranslatingExpressionVisitor Create(
      QueryCompilationContext queryCompilationContext,
      QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
    {
      return (RelationalSqlTranslatingExpressionVisitor) new DmSqlTranslatingExpressionVisitor(this._dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor);
    }
  }
}
