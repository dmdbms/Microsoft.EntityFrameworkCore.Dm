#define DEBUG
using System.Collections.Concurrent;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public class DmValueGeneratorCache : ValueGeneratorCache, IDmValueGeneratorCache, IValueGeneratorCache
	{
		private readonly ConcurrentDictionary<string, DmSequenceValueGeneratorState> _sequenceGeneratorCache = new ConcurrentDictionary<string, DmSequenceValueGeneratorState>();

		public DmValueGeneratorCache([NotNull] ValueGeneratorCacheDependencies dependencies)
			: base(dependencies)
		{
		}

		public virtual DmSequenceValueGeneratorState GetOrAddSequenceState(IProperty property)
		{
			Check.NotNull<IProperty>(property, "property");
			IReadOnlySequence sequence = ((IReadOnlyProperty)(object)property).FindHiLoSequence();
			Debug.Assert(sequence != null);
			return _sequenceGeneratorCache.GetOrAdd(GetSequenceName(sequence), (string sequenceName) => new DmSequenceValueGeneratorState(sequence));
		}

		private static string GetSequenceName(IReadOnlySequence sequence)
		{
			return ((sequence.Schema == null) ? "" : (sequence.Schema + ".")) + sequence.Name;
		}
	}
}
