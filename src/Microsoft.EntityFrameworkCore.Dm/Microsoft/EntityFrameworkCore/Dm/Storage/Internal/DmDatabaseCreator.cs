using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
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


		public DmDatabaseCreator([JetBrains.Annotations.NotNull] RelationalDatabaseCreatorDependencies dependencies, [JetBrains.Annotations.NotNull] IDmRelationalConnection connection, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder)
			: base(dependencies)
		{
			_connection = connection;
			_rawSqlCommandBuilder = rawSqlCommandBuilder;
		}

		public override void Create()
		{
			using IDmRelationalConnection connection = _connection.CreateMasterConnection();
			Dependencies.MigrationCommandExecutor.ExecuteNonQuery(CreateCreateOperations(), connection);
		}

		public override async Task CreateAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			using IDmRelationalConnection masterConnection = _connection.CreateMasterConnection();
			await Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync(CreateCreateOperations(), masterConnection, cancellationToken);
		}

		public override bool HasTables()
		{
			return Dependencies.ExecutionStrategyFactory.Create().Execute(_connection, (IDmRelationalConnection connection) => (int)CreateHasTablesCommand().ExecuteScalar(new RelationalCommandParameterObject(connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger)) != 0);
		}

		public override Task<bool> HasTablesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return Dependencies.ExecutionStrategyFactory.Create().ExecuteAsync(_connection, async (IDmRelationalConnection connection, CancellationToken ct) => (int)(await CreateHasTablesCommand().ExecuteScalarAsync(new RelationalCommandParameterObject(connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger), ct)) != 0, cancellationToken);
		}

		private IRelationalCommand CreateHasTablesCommand()
		{
			return _rawSqlCommandBuilder.Build("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END FROM SYS.SYSOBJECTS WHERE SCHID = CURRENT_SCHID() AND TYPE$='SCHOBJ' AND SUBTYPE$='UTAB'");
		}

		private IReadOnlyList<MigrationCommand> CreateCreateOperations()
		{
			DmConnectionStringBuilder dmConnectionStringBuilder = new DmConnectionStringBuilder(_connection.DbConnection.ConnectionString);
			return Dependencies.MigrationsSqlGenerator.Generate(new DmCreateUserOperation[1]
			{
				new DmCreateUserOperation
				{
					UserName = dmConnectionStringBuilder.User,
					Password = dmConnectionStringBuilder.Password
				}
			});
		}

		public override bool Exists()
		{
			try
			{
				_connection.Open(errorsExpected: true);
				_connection.Close();
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
				await _connection.OpenAsync(cancellationToken, errorsExpected: true);
				_connection.Close();
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
			using IDmRelationalConnection connection = _connection.CreateMasterConnection();
			Dependencies.MigrationCommandExecutor.ExecuteNonQuery(CreateDropCommands(), connection);
		}

		public override async Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			using IDmRelationalConnection masterConnection = _connection.CreateMasterConnection();
			await Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync(CreateDropCommands(), masterConnection, cancellationToken);
		}

		private IReadOnlyList<MigrationCommand> CreateDropCommands()
		{
			string user = new DmConnectionStringBuilder(_connection.DbConnection.ConnectionString).User;
			if (string.IsNullOrEmpty(user))
			{
				throw new InvalidOperationException(DmStrings.NoUserId);
			}
			MigrationOperation[] operations = new MigrationOperation[1]
			{
				new DmDropUserOperation
				{
					UserName = user
				}
			};
			return Dependencies.MigrationsSqlGenerator.Generate(operations);
		}
	}
}
