// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Internal.DmModelValidator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Internal
{
  public class DmModelValidator : RelationalModelValidator
  {
    public DmModelValidator(
      [NotNull] ModelValidatorDependencies dependencies,
      [NotNull] RelationalModelValidatorDependencies relationalDependencies)
      : base(dependencies, relationalDependencies)
    {
    }

    public override void Validate(
      IModel model,
      IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
      base.Validate(model, logger);
      this.ValidateDefaultDecimalMapping(model, logger);
      this.ValidateByteIdentityMapping(model, logger);
      this.ValidateNonKeyValueGeneration(model, logger);
    }

    protected virtual void ValidateDefaultDecimalMapping(
      [NotNull] IModel model,
      [NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
      foreach (IProperty property in model.GetEntityTypes().SelectMany<IEntityType, IProperty>((Func<IEntityType, IEnumerable<IProperty>>) (t => t.GetDeclaredProperties())).Where<IProperty>((Func<IProperty, bool>) (p => ((IReadOnlyPropertyBase) p).ClrType.UnwrapNullableType() == typeof (Decimal) && !((IReadOnlyProperty) p).IsForeignKey())))
      {
        ConfigurationSource? nullable1 = property is IConventionProperty iconventionProperty1 ? RelationalPropertyExtensions.GetColumnTypeConfigurationSource(iconventionProperty1) : new ConfigurationSource?();
        ConfigurationSource? nullable2 = property is IConventionProperty iconventionProperty2 ? iconventionProperty2.GetTypeMappingConfigurationSource() : new ConfigurationSource?();
        if (!nullable1.HasValue && ConfigurationSourceExtensions.Overrides((ConfigurationSource) 2, nullable2) || nullable1.HasValue && ConfigurationSourceExtensions.Overrides((ConfigurationSource) 2, nullable1))
          logger.DecimalTypeDefaultWarning(property);
      }
    }

    protected virtual void ValidateByteIdentityMapping(
      [NotNull] IModel model,
      [NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
      foreach (IProperty property in model.GetEntityTypes().SelectMany<IEntityType, IProperty>((Func<IEntityType, IEnumerable<IProperty>>) (t => t.GetDeclaredProperties())).Where<IProperty>((Func<IProperty, bool>) (p => ((IReadOnlyPropertyBase) p).ClrType.UnwrapNullableType() == typeof (byte) && ((IReadOnlyProperty) p).GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn)))
        logger.ByteIdentityColumnWarning(property);
    }

    protected virtual void ValidateNonKeyValueGeneration(
      [NotNull] IModel model,
      [NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
      using (IEnumerator<IProperty> enumerator = model.GetEntityTypes().SelectMany<IEntityType, IProperty>((Func<IEntityType, IEnumerable<IProperty>>) (t => t.GetDeclaredProperties())).Where<IProperty>((Func<IProperty, bool>) (p =>
      {
        if (((IReadOnlyProperty) p).GetValueGenerationStrategy() != DmValueGenerationStrategy.SequenceHiLo || !((IConventionProperty) p).GetValueGenerationStrategyConfigurationSource().HasValue || ((IReadOnlyProperty) p).IsKey() || ((IReadOnlyProperty) p).ValueGenerated == null)
          return false;
        return !(((IReadOnlyAnnotatable) p).FindAnnotation("Dm:ValueGenerationStrategy") is ConventionAnnotation annotation2) || !ConfigurationSourceExtensions.Overrides((ConfigurationSource) 2, new ConfigurationSource?(annotation2.GetConfigurationSource()));
      })).GetEnumerator())
      {
        if (((IEnumerator) enumerator).MoveNext())
        {
          IProperty current = enumerator.Current;
          throw new InvalidOperationException(DmStrings.NonKeyValueGeneration((object) ((IReadOnlyPropertyBase) current).Name, (object) ((IReadOnlyTypeBase) current.DeclaringEntityType).DisplayName()));
        }
      }
    }

    protected override void ValidateSharedColumnsCompatibility(
      IReadOnlyList<IEntityType> mappedTypes,
      in StoreObjectIdentifier storeObject,
      IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
      base.ValidateSharedColumnsCompatibility(mappedTypes, storeObject, logger);
      Dictionary<string, IProperty> dictionary = new Dictionary<string, IProperty>();
      foreach (IProperty property in ((IEnumerable<IEntityType>) mappedTypes).SelectMany<IEntityType, IProperty>((Func<IEntityType, IEnumerable<IProperty>>) (et => et.GetDeclaredProperties())))
      {
        if (((IReadOnlyProperty) property).GetValueGenerationStrategy(in storeObject) == DmValueGenerationStrategy.IdentityColumn)
        {
          string columnName = RelationalPropertyExtensions.GetColumnName((IReadOnlyProperty) property, in storeObject);
          if (columnName != null)
            dictionary[columnName] = property;
        }
      }
      if (dictionary.Count > 1)
        throw new InvalidOperationException(DmStrings.MultipleIdentityColumns((object) new StringBuilder().AppendJoin(((IEnumerable<IProperty>) dictionary.Values).Select<IProperty, string>((Func<IProperty, string>) (p => "'" + ((IReadOnlyTypeBase) p.DeclaringEntityType).DisplayName() + "." + ((IReadOnlyPropertyBase) p).Name + "'"))), (object) ((StoreObjectIdentifier)  storeObject).DisplayName()));
    }
  }
}
