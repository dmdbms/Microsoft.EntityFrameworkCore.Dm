using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public interface IDmValueGeneratorCache : IValueGeneratorCache
	{
		DmSequenceValueGeneratorState GetOrAddSequenceState([NotNull] IProperty property);
	}
}
