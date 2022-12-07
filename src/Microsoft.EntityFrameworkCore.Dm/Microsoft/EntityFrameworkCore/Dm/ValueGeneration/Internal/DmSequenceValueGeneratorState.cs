// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal.DmSequenceValueGeneratorState
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;



namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
  public class DmSequenceValueGeneratorState : HiLoValueGeneratorState
  {
    public DmSequenceValueGeneratorState([NotNull] IReadOnlySequence sequence)
      : base(Check.NotNull<IReadOnlySequence>(sequence, nameof (sequence)).IncrementBy)
    {
      this.Sequence = sequence;
    }

    public virtual IReadOnlySequence Sequence { get; }
  }
}
