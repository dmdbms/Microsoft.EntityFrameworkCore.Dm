using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public class DmValueGeneratorCache : ValueGeneratorCache, IDmValueGeneratorCache, IValueGeneratorCache
	{
		private readonly ConcurrentDictionary<string, DmSequenceValueGeneratorState> _sequenceGeneratorCache = new ConcurrentDictionary<string, DmSequenceValueGeneratorState>();

		public DmValueGeneratorCache([JetBrains.Annotations.NotNull] ValueGeneratorCacheDependencies dependencies)
			: base(dependencies)
		{
		}

		public virtual DmSequenceValueGeneratorState GetOrAddSequenceState(IProperty property)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
			ISequence sequence = property.FindHiLoSequence();
			return _sequenceGeneratorCache.GetOrAdd(GetSequenceName(sequence), (string sequenceName) => new DmSequenceValueGeneratorState(sequence));
		}

		private static string GetSequenceName(ISequence sequence)
		{
			return ((sequence.Schema == null) ? "" : (sequence.Schema + ".")) + sequence.Name;
		}
	}
}
