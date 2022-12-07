// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmMethodCallTranslatorProvider
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider
  {
    public DmMethodCallTranslatorProvider(
      [NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies)
      : base(dependencies)
    {
      ISqlExpressionFactory expressionFactory = dependencies.SqlExpressionFactory;
      this.AddTranslators((IEnumerable<IMethodCallTranslator>) new IMethodCallTranslator[9]
      {
        (IMethodCallTranslator) new DmConvertTranslator(expressionFactory),
        (IMethodCallTranslator) new DmDateTimeMethodTranslator(expressionFactory),
        (IMethodCallTranslator) new DmDateDiffFunctionsTranslator(expressionFactory),
        (IMethodCallTranslator) new DmFullTextSearchFunctionsTranslator(expressionFactory),
        (IMethodCallTranslator) new DmIsDateFunctionTranslator(expressionFactory),
        (IMethodCallTranslator) new DmMathTranslator(expressionFactory),
        (IMethodCallTranslator) new DmNewGuidTranslator(expressionFactory),
        (IMethodCallTranslator) new DmObjectToStringTranslator(expressionFactory),
        (IMethodCallTranslator) new DmStringMethodTranslator(expressionFactory)
      });
    }
  }
}
