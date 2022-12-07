// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal.DmValueGeneratorCache
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;



namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
  public class DmValueGeneratorCache : 
    ValueGeneratorCache,
    IDmValueGeneratorCache,
    IValueGeneratorCache
  {
    private readonly ConcurrentDictionary<string, DmSequenceValueGeneratorState> _sequenceGeneratorCache = new ConcurrentDictionary<string, DmSequenceValueGeneratorState>();

    public DmValueGeneratorCache([NotNull] ValueGeneratorCacheDependencies dependencies)
      : base(dependencies)
    {
    }

    public virtual DmSequenceValueGeneratorState GetOrAddSequenceState(
      IProperty property)
    {
      Check.NotNull<IProperty>(property, nameof (property));
      IReadOnlySequence sequence = ((IReadOnlyProperty) property).FindHiLoSequence();
      Debug.Assert(sequence != null);
      return this._sequenceGeneratorCache.GetOrAdd(DmValueGeneratorCache.GetSequenceName(sequence), (Func<string, DmSequenceValueGeneratorState>) (sequenceName => new DmSequenceValueGeneratorState(sequence)));
    }

    private static string GetSequenceName(IReadOnlySequence sequence) => (sequence.Schema == null ? "" : sequence.Schema + ".") + sequence.Name;
  }
}
