// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmMemberTranslatorProvider
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmMemberTranslatorProvider : RelationalMemberTranslatorProvider
  {
    public DmMemberTranslatorProvider(
      [NotNull] RelationalMemberTranslatorProviderDependencies dependencies)
      : base(dependencies)
    {
      ISqlExpressionFactory expressionFactory = dependencies.SqlExpressionFactory;
      this.AddTranslators((IEnumerable<IMemberTranslator>) new IMemberTranslator[2]
      {
        (IMemberTranslator) new DmDateTimeMemberTranslator(expressionFactory),
        (IMemberTranslator) new DmStringMemberTranslator(expressionFactory)
      });
    }
  }
}
