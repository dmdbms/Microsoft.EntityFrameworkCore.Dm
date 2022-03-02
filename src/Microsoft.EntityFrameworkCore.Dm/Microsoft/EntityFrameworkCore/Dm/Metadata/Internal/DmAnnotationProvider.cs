using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Dm.Metadata.Internal
{
	public class DmAnnotationProvider : RelationalAnnotationProvider
	{
		public DmAnnotationProvider([NotNull] RelationalAnnotationProviderDependencies dependencies)
			: base(dependencies)
		{
		}

		public override IEnumerable<IAnnotation> For(IColumn column)
		{
			StoreObjectIdentifier table = StoreObjectIdentifier.Table(column.Table.Name, column.Table.Schema);
			IProperty property = (from m in column.PropertyMappings
				where m.TableMapping.IsSharedTablePrincipal && m.TableMapping.EntityType == m.Property.DeclaringEntityType
				select m.Property).FirstOrDefault((IProperty p) => p.GetValueGenerationStrategy(in table) == DmValueGenerationStrategy.IdentityColumn);
			if (property != null)
			{
				int? seed = property.GetIdentitySeed();
				yield return new Annotation("Dm:Identity", string.Format(arg1: property.GetIdentityIncrement() ?? 1, provider: CultureInfo.InvariantCulture, format: "{0}, {1}", arg0: seed ?? 1));
			}
		}
	}
}
