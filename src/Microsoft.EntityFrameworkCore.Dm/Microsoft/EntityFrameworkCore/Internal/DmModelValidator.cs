using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Microsoft.EntityFrameworkCore.Internal
{
	public class DmModelValidator : RelationalModelValidator
	{
		public DmModelValidator([NotNull] ModelValidatorDependencies dependencies, [NotNull] RelationalModelValidatorDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
		}

		public override void Validate(IModel model, IDiagnosticsLogger<Validation> logger)
		{
			((RelationalModelValidator)this).Validate(model, logger);
			ValidateDefaultDecimalMapping(model, logger);
			ValidateByteIdentityMapping(model, logger);
			ValidateNonKeyValueGeneration(model, logger);
		}

		protected virtual void ValidateDefaultDecimalMapping([NotNull] IModel model, [NotNull] IDiagnosticsLogger<Validation> logger)
		{
			foreach (IProperty item in from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties())
				where ((IReadOnlyPropertyBase)p).ClrType.UnwrapNullableType() == typeof(decimal) && !((IReadOnlyProperty)p).IsForeignKey()
				select p)
			{
				IProperty obj = ((item is IConventionProperty) ? item : null);
				ConfigurationSource? val = ((obj != null) ? RelationalPropertyExtensions.GetColumnTypeConfigurationSource((IConventionProperty)(object)obj) : null);
				IProperty obj2 = ((item is IConventionProperty) ? item : null);
				ConfigurationSource? val2 = ((obj2 != null) ? ((IConventionProperty)obj2).GetTypeMappingConfigurationSource() : null);
				if ((!val.HasValue && ConfigurationSourceExtensions.Overrides((ConfigurationSource)2, val2)) || (val.HasValue && ConfigurationSourceExtensions.Overrides((ConfigurationSource)2, val)))
				{
					logger.DecimalTypeDefaultWarning(item);
				}
			}
		}

		protected virtual void ValidateByteIdentityMapping([NotNull] IModel model, [NotNull] IDiagnosticsLogger<Validation> logger)
		{
			foreach (IProperty item in from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties())
				where ((IReadOnlyPropertyBase)p).ClrType.UnwrapNullableType() == typeof(byte) && ((IReadOnlyProperty)(object)p).GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn
				select p)
			{
				logger.ByteIdentityColumnWarning(item);
			}
		}

		protected virtual void ValidateNonKeyValueGeneration([NotNull] IModel model, [NotNull] IDiagnosticsLogger<Validation> logger)
		{
			using IEnumerator<IProperty> enumerator = model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties()).Where(delegate(IProperty p)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Expected O, but got Unknown
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				int result;
				if (((IReadOnlyProperty)(object)p).GetValueGenerationStrategy() == DmValueGenerationStrategy.SequenceHiLo && DmPropertyExtensions.GetValueGenerationStrategyConfigurationSource((IConventionProperty)p).HasValue && !((IReadOnlyProperty)p).IsKey() && (int)((IReadOnlyProperty)p).ValueGenerated != 0)
				{
					IAnnotation obj = ((IReadOnlyAnnotatable)p).FindAnnotation("Dm:ValueGenerationStrategy");
					ConventionAnnotation val = (ConventionAnnotation)(object)((obj is ConventionAnnotation) ? obj : null);
					result = ((val == null || !ConfigurationSourceExtensions.Overrides((ConfigurationSource)2, (ConfigurationSource?)val.GetConfigurationSource())) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			})
				.GetEnumerator();
			if (enumerator.MoveNext())
			{
				IProperty current = enumerator.Current;
				throw new InvalidOperationException(DmStrings.NonKeyValueGeneration(current.Name, (current.DeclaringEntityType).DisplayName()));
			}
		}

		protected override void ValidateSharedColumnsCompatibility(IReadOnlyList<IEntityType> mappedTypes, in StoreObjectIdentifier storeObject, IDiagnosticsLogger<Validation> logger)
		{
			base.ValidateSharedColumnsCompatibility(mappedTypes, in storeObject, logger);
			Dictionary<string, IProperty> dictionary = new Dictionary<string, IProperty>();
			foreach (IProperty item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredProperties()))
			{
				if (((IReadOnlyProperty)(object)item).GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.IdentityColumn)
				{
					string columnName = RelationalPropertyExtensions.GetColumnName((IReadOnlyProperty)(object)item, in storeObject);
					if (columnName != null)
					{
						dictionary[columnName] = item;
					}
				}
			}
			if (dictionary.Count > 1)
			{
				StringBuilder properties = new StringBuilder().AppendJoin(dictionary.Values.Select((IProperty p) => "'" + ((IReadOnlyTypeBase)p.DeclaringEntityType).DisplayName() + "." + ((IReadOnlyPropertyBase)p).Name + "'"));
				throw new InvalidOperationException(DmStrings.MultipleIdentityColumns(properties, ((StoreObjectIdentifier)( storeObject)).DisplayName()));
			}
		}
	}
}
