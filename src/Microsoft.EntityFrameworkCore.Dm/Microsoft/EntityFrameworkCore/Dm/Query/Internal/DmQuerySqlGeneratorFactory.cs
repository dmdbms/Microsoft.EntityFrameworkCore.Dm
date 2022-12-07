// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmQuerySqlGeneratorFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Query;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
  {
    private readonly QuerySqlGeneratorDependencies _dependencies;

    public DmQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies) => this._dependencies = dependencies;

    public virtual QuerySqlGenerator Create() => (QuerySqlGenerator) new DmQuerySqlGenerator(this._dependencies);
  }
}
