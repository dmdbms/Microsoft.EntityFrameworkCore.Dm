using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Dm.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Dm.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Dm.Query.Internal;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Dm.Update.Internal;
using Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class DmServiceCollectionExtensions
	{
		public static IServiceCollection AddEntityFrameworkDm([NotNull] this IServiceCollection serviceCollection)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			Check.NotNull(serviceCollection, "serviceCollection");
			EntityFrameworkServicesBuilder val = ((EntityFrameworkServicesBuilder)new EntityFrameworkRelationalServicesBuilder(serviceCollection)).TryAdd<LoggingDefinitions, DmLoggingDefinitions>().TryAdd<IDatabaseProvider, DatabaseProvider<DmOptionsExtension>>().TryAdd<IRelationalAnnotationProvider, DmAnnotationProvider>()
				.TryAdd<IValueGeneratorCache>((Func<IServiceProvider, IValueGeneratorCache>)((IServiceProvider p) => (IValueGeneratorCache)p.GetService<IDmValueGeneratorCache>()))
				.TryAdd<IRelationalTypeMappingSource, DmTypeMappingSource>()
				.TryAdd<ISqlGenerationHelper, DmSqlGenerationHelper>()
				.TryAdd<IMigrationsAnnotationProvider, DmMigrationsAnnotationProvider>()
				.TryAdd<IModelValidator, DmModelValidator>()
				.TryAdd<IProviderConventionSetBuilder, DmConventionSetBuilder>()
				.TryAdd<IUpdateSqlGenerator>((Func<IServiceProvider, IUpdateSqlGenerator>)((IServiceProvider p) => (IUpdateSqlGenerator)p.GetService<IDmUpdateSqlGenerator>()))
				.TryAdd<IModificationCommandBatchFactory, DmModificationCommandBatchFactory>()
				.TryAdd<IValueGeneratorSelector, DmValueGeneratorSelector>()
				.TryAdd<IRelationalConnection>((Func<IServiceProvider, IRelationalConnection>)((IServiceProvider p) => (IRelationalConnection)p.GetService<IDmRelationalConnection>()))
				.TryAdd<IRelationalTransactionFactory, DmTransactionFactory>()
				.TryAdd<IMigrationsSqlGenerator, DmMigrationsSqlGenerator>()
				.TryAdd<IRelationalDatabaseCreator, DmDatabaseCreator>()
				.TryAdd<IHistoryRepository, DmHistoryRepository>()
				.TryAdd<ICompiledQueryCacheKeyGenerator, DmCompiledQueryCacheKeyGenerator>()
				.TryAdd<IExecutionStrategyFactory, DmExecutionStrategyFactory>()
				.TryAdd<IMethodCallTranslatorProvider, DmMethodCallTranslatorProvider>()
				.TryAdd<IMemberTranslatorProvider, DmMemberTranslatorProvider>()
				.TryAdd<IQuerySqlGeneratorFactory, DmQuerySqlGeneratorFactory>()
				.TryAdd<IQueryTranslationPostprocessorFactory, DmQueryTranslationPostprocessorFactory>()
				.TryAdd<IRelationalSqlTranslatingExpressionVisitorFactory, DmSqlTranslatingExpressionVisitorFactory>()
				.TryAdd<IRelationalParameterBasedSqlProcessorFactory, DmRelationalParameterBasedSqlProcessorFactory>()
				.TryAddProviderSpecificServices((Action<ServiceCollectionMap>)delegate(ServiceCollectionMap b)
				{
					b.TryAddSingleton<IDmValueGeneratorCache, DmValueGeneratorCache>().TryAddSingleton<IDmUpdateSqlGenerator, DmUpdateSqlGenerator>().TryAddSingleton<IDmSequenceValueGeneratorFactory, DmSequenceValueGeneratorFactory>()
						.TryAddScoped<IDmRelationalConnection, DmRelationalConnection>();
				});
			val.TryAddCoreServices();
			return serviceCollection;
		}
	}
}
