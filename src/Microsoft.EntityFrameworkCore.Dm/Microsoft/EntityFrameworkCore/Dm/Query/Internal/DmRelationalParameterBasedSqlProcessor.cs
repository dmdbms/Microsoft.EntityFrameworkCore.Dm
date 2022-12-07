// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmRelationalParameterBasedSqlProcessor
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  internal class DmRelationalParameterBasedSqlProcessor : RelationalParameterBasedSqlProcessor
  {
    public DmRelationalParameterBasedSqlProcessor(
      RelationalParameterBasedSqlProcessorDependencies dependencies,
      bool useRelationalNulls)
      : base(dependencies, useRelationalNulls)
    {
    }

    protected override SelectExpression ProcessSqlNullability(
      SelectExpression selectExpression,
      IReadOnlyDictionary<string, object> parametersValues,
      out bool canCache)
    {
      return new DmSqlNullabilityProcessor(this.Dependencies, this.UseRelationalNulls).Process(selectExpression, parametersValues, out canCache);
    }
  }
}
