// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmCompiledQueryCacheKeyGenerator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmCompiledQueryCacheKeyGenerator : RelationalCompiledQueryCacheKeyGenerator
  {
    public DmCompiledQueryCacheKeyGenerator(
      [NotNull] CompiledQueryCacheKeyGeneratorDependencies dependencies,
      [NotNull] RelationalCompiledQueryCacheKeyGeneratorDependencies relationalDependencies)
      : base(dependencies, relationalDependencies)
    {
    }

    public override object GenerateCacheKey(Expression query, bool async) => (object) new DmCompiledQueryCacheKeyGenerator.DmCompiledQueryCacheKey(this.GenerateCacheKeyCore(query, async));

    private struct DmCompiledQueryCacheKey
    {
      private readonly RelationalCompiledQueryCacheKeyGenerator.RelationalCompiledQueryCacheKey _relationalCompiledQueryCacheKey;

      public DmCompiledQueryCacheKey(
        RelationalCompiledQueryCacheKeyGenerator.RelationalCompiledQueryCacheKey relationalCompiledQueryCacheKey)
      {
        this._relationalCompiledQueryCacheKey = relationalCompiledQueryCacheKey;
      }

      public override bool Equals(object obj) => obj != null && obj is DmCompiledQueryCacheKeyGenerator.DmCompiledQueryCacheKey other && this.Equals(other);

      private bool Equals(
        DmCompiledQueryCacheKeyGenerator.DmCompiledQueryCacheKey other)
      {
        return ((RelationalCompiledQueryCacheKeyGenerator.RelationalCompiledQueryCacheKey) this._relationalCompiledQueryCacheKey).Equals(other._relationalCompiledQueryCacheKey);
      }

      public override int GetHashCode() => this._relationalCompiledQueryCacheKey.GetHashCode();
    }
  }
}
