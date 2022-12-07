using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public class DmSequenceValueGeneratorState : HiLoValueGeneratorState
	{
		public virtual IReadOnlySequence Sequence { get; }

		public DmSequenceValueGeneratorState([NotNull] IReadOnlySequence sequence)
			: base(Check.NotNull<IReadOnlySequence>(sequence, "sequence").IncrementBy)
		{
			Sequence = sequence;
		}
	}
}
