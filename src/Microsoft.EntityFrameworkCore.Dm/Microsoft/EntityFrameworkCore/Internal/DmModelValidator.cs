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

namespace Microsoft.EntityFrameworkCore.Internal
{
	public class DmModelValidator : RelationalModelValidator
	{
		public DmModelValidator([JetBrains.Annotations.NotNull] ModelValidatorDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalModelValidatorDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
		}

		public override void Validate(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
		{
			base.Validate(model, logger);
			ValidateDefaultDecimalMapping(model, logger);
			ValidateByteIdentityMapping(model, logger);
			ValidateNonKeyValueGeneration(model, logger);
		}

		protected virtual void ValidateDefaultDecimalMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
		{
			foreach (IProperty item in from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties())
				where p.ClrType.UnwrapNullableType() == typeof(decimal) && !p.IsForeignKey()
				select p)
			{
				ConfigurationSource? oldConfigurationSource = (item as IConventionProperty)?.GetColumnTypeConfigurationSource();
				ConfigurationSource? oldConfigurationSource2 = (item as IConventionProperty)?.GetTypeMappingConfigurationSource();
				if ((!oldConfigurationSource.HasValue && ConfigurationSource.Convention.Overrides(oldConfigurationSource2)) || (oldConfigurationSource.HasValue && ConfigurationSource.Convention.Overrides(oldConfigurationSource)))
				{
					logger.DecimalTypeDefaultWarning(item);
				}
			}
		}

		protected virtual void ValidateByteIdentityMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
		{
			foreach (IProperty item in from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties())
				where p.ClrType.UnwrapNullableType() == typeof(byte) && p.GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn
				select p)
			{
				logger.ByteIdentityColumnWarning(item);
			}
		}

		protected virtual void ValidateNonKeyValueGeneration([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
		{
			using IEnumerator<IProperty> enumerator = model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties()).Where(delegate(IProperty p)
			{
				int result;
				if (p.GetValueGenerationStrategy() == DmValueGenerationStrategy.SequenceHiLo && ((IConventionProperty)p).GetValueGenerationStrategyConfigurationSource().HasValue && !p.IsKey() && p.ValueGenerated != 0)
				{
					ConventionAnnotation conventionAnnotation = p.FindAnnotation("Dm:ValueGenerationStrategy") as ConventionAnnotation;
					result = ((conventionAnnotation == null || !ConfigurationSource.Convention.Overrides(conventionAnnotation.GetConfigurationSource())) ? 1 : 0);
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
				throw new InvalidOperationException(DmStrings.NonKeyValueGeneration(current.Name, current.DeclaringEntityType.DisplayName()));
			}
		}

		protected override void ValidateSharedColumnsCompatibility(IReadOnlyList<IEntityType> mappedTypes, in StoreObjectIdentifier storeObject, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
		{
			base.ValidateSharedColumnsCompatibility(mappedTypes, in storeObject, logger);
			Dictionary<string, IProperty> dictionary = new Dictionary<string, IProperty>();
			foreach (IProperty item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredProperties()))
			{
				if (item.GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.IdentityColumn)
				{
					string columnName = item.GetColumnName(in storeObject);
					if (columnName != null)
					{
						dictionary[columnName] = item;
					}
				}
			}
			if (dictionary.Count > 1)
			{
				StringBuilder properties = new StringBuilder().AppendJoin(dictionary.Values.Select((IProperty p) => "'" + p.DeclaringEntityType.DisplayName() + "." + p.Name + "'"));
				throw new InvalidOperationException(DmStrings.MultipleIdentityColumns(properties, storeObject.DisplayName()));
			}
		}
	}
}
