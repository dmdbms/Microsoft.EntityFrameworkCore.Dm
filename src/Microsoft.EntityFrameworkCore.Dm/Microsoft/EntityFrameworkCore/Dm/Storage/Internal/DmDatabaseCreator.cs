using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmDatabaseCreator : RelationalDatabaseCreator
	{
		private readonly IDmRelationalConnection _connection;

		private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

		public virtual TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(500.0);


		public virtual TimeSpan RetryTimeout { get; set; } = TimeSpan.FromMinutes(1.0);


		public DmDatabaseCreator([NotNull] RelationalDatabaseCreatorDependencies dependencies, [NotNull] IDmRelationalConnection connection, [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder)
			: base(dependencies)
		{
			_connection = connection;
			_rawSqlCommandBuilder = rawSqlCommandBuilder;
		}

		public override void Create()
		{
			using IDmRelationalConnection dmRelationalConnection = _connection.CreateMasterConnection();
			base.Dependencies.MigrationCommandExecutor.ExecuteNonQuery((IEnumerable<MigrationCommand>)CreateCreateOperations(), (IRelationalConnection)dmRelationalConnection);
		}

		public override async Task CreateAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			using IDmRelationalConnection masterConnection = _connection.CreateMasterConnection();
			await base.Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync((IEnumerable<MigrationCommand>)CreateCreateOperations(), (IRelationalConnection)masterConnection, cancellationToken);
		}

		public override bool HasTables()
		{
			return ExecutionStrategyExtensions.Execute<IDmRelationalConnection, bool>(base.Dependencies.ExecutionStrategyFactory.Create(), _connection, (Func<IDmRelationalConnection, bool>)((IDmRelationalConnection connection) => (int)CreateHasTablesCommand().ExecuteScalar(new RelationalCommandParameterObject((IRelationalConnection)connection, (IReadOnlyDictionary<string, object>)null, (IReadOnlyList<ReaderColumn>)null, base.Dependencies.CurrentContext.Context, base.Dependencies.CommandLogger)) != 0));
		}

		public override Task<bool> HasTablesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return ExecutionStrategyExtensions.ExecuteAsync<IDmRelationalConnection, bool>(base.Dependencies.ExecutionStrategyFactory.Create(), _connection, (Func<IDmRelationalConnection, CancellationToken, Task<bool>>)(async (IDmRelationalConnection connection, CancellationToken ct) => (int)(await CreateHasTablesCommand().ExecuteScalarAsync(new RelationalCommandParameterObject((IRelationalConnection)connection, (IReadOnlyDictionary<string, object>)null, (IReadOnlyList<ReaderColumn>)null, base.Dependencies.CurrentContext.Context, base.Dependencies.CommandLogger), ct)) != 0), cancellationToken);
		}

		private IRelationalCommand CreateHasTablesCommand()
		{
			return _rawSqlCommandBuilder.Build("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END FROM SYS.SYSOBJECTS WHERE SCHID = CURRENT_SCHID() AND TYPE$='SCHOBJ' AND SUBTYPE$='UTAB'");
		}

		private IReadOnlyList<MigrationCommand> CreateCreateOperations()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			DmConnectionStringBuilder val = new DmConnectionStringBuilder(((IRelationalConnection)_connection).DbConnection.ConnectionString);
			return base.Dependencies.MigrationsSqlGenerator.Generate((IReadOnlyList<MigrationOperation>)(object)new DmCreateUserOperation[1]
			{
				new DmCreateUserOperation
				{
					UserName = val.User,
					Password = val.Password
				}
			}, (IModel)null, (MigrationsSqlGenerationOptions)0);
		}

		public override bool Exists()
		{
			try
			{
				((IRelationalConnection)_connection).Open(true);
				((IRelationalConnection)_connection).Close();
				return true;
			}
			catch (DmException)
			{
				return false;
			}
		}

		public override async Task<bool> ExistsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				await ((IRelationalConnection)_connection).OpenAsync(cancellationToken, true);
				((IRelationalConnection)_connection).Close();
				return true;
			}
			catch (DmException)
			{
				return false;
			}
		}

		private static bool IsDoesNotExist(DmException exception)
		{
			return exception.Number == 4060 || exception.Number == 1832 || exception.Number == 5120;
		}

		private bool RetryOnExistsFailure(DmException exception)
		{
			if (exception.Number == 233 || exception.Number == -2 || exception.Number == 4060 || exception.Number == 1832 || exception.Number == 5120)
			{
				return true;
			}
			return false;
		}

		public override void Delete()
		{
			using IDmRelationalConnection dmRelationalConnection = _connection.CreateMasterConnection();
			base.Dependencies.MigrationCommandExecutor.ExecuteNonQuery((IEnumerable<MigrationCommand>)CreateDropCommands(), (IRelationalConnection)dmRelationalConnection);
		}

		public override async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			using IDmRelationalConnection masterConnection = _connection.CreateMasterConnection();
			await base.Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync((IEnumerable<MigrationCommand>)CreateDropCommands(), (IRelationalConnection)masterConnection, cancellationToken);
		}

		private IReadOnlyList<MigrationCommand> CreateDropCommands()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			string user = new DmConnectionStringBuilder(((IRelationalConnection)_connection).DbConnection.ConnectionString).User;
			if (string.IsNullOrEmpty(user))
			{
				throw new InvalidOperationException(DmStrings.NoUserId);
			}
			MigrationOperation[] array = (MigrationOperation[])(object)new MigrationOperation[1]
			{
				new DmDropUserOperation
				{
					UserName = user
				}
			};
			return base.Dependencies.MigrationsSqlGenerator.Generate((IReadOnlyList<MigrationOperation>)array, (IModel)null, (MigrationsSqlGenerationOptions)0);
		}
	}
}
