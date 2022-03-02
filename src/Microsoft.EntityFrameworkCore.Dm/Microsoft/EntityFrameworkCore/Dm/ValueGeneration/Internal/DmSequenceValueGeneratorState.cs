using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public class DmSequenceValueGeneratorState : HiLoValueGeneratorState
	{
		public virtual ISequence Sequence { get; }

		public DmSequenceValueGeneratorState([JetBrains.Annotations.NotNull] ISequence sequence)
			: base(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sequence, "sequence").IncrementBy)
		{
			Sequence = sequence;
		}
	}
}
