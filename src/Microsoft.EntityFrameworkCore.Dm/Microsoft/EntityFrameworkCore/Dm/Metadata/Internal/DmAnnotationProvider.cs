using System;
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

		public override IEnumerable<IAnnotation> For(IColumn column, bool designTime)
		{
			StoreObjectIdentifier table = StoreObjectIdentifier.Table(((ITableBase)column.Table).Name, ((ITableBase)column.Table).Schema);
			IProperty property = (from m in column.PropertyMappings
				where ((ITableMappingBase)m.TableMapping).IsSharedTablePrincipal && ((ITableMappingBase)m.TableMapping).EntityType == ((IColumnMappingBase)m).Property.DeclaringEntityType
				select ((IColumnMappingBase)m).Property).FirstOrDefault((Func<IProperty, bool>)((IProperty p) => ((IReadOnlyProperty)(object)p).GetValueGenerationStrategy(in table) == DmValueGenerationStrategy.IdentityColumn));
			if (property != null)
			{
				int? seed = ((IReadOnlyProperty)(object)property).GetIdentitySeed();
				yield return (IAnnotation)new Annotation("Dm:Identity", (object)string.Format(arg1: ((IReadOnlyProperty)(object)property).GetIdentityIncrement() ?? 1, provider: CultureInfo.InvariantCulture, format: "{0}, {1}", arg0: seed ?? 1));
			}
		}
	}
}
