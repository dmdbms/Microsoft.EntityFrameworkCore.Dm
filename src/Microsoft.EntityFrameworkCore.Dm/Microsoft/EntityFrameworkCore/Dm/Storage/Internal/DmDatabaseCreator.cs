// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmDatabaseCreator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmDatabaseCreator : RelationalDatabaseCreator
  {
    private readonly IDmRelationalConnection _connection;
    private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

    public DmDatabaseCreator(
      [NotNull] RelationalDatabaseCreatorDependencies dependencies,
      [NotNull] IDmRelationalConnection connection,
      [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder)
      : base(dependencies)
    {
      this._connection = connection;
      this._rawSqlCommandBuilder = rawSqlCommandBuilder;
    }

    public virtual TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(500.0);

    public virtual TimeSpan RetryTimeout { get; set; } = TimeSpan.FromMinutes(1.0);

    public override void Create()
    {
      using (IDmRelationalConnection masterConnection = this._connection.CreateMasterConnection())
        this.Dependencies.MigrationCommandExecutor.ExecuteNonQuery((IEnumerable<MigrationCommand>) this.CreateCreateOperations(), (IRelationalConnection) masterConnection);
    }

    public override async Task CreateAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      using (IDmRelationalConnection masterConnection = this._connection.CreateMasterConnection())
        await this.Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync((IEnumerable<MigrationCommand>) this.CreateCreateOperations(), (IRelationalConnection) masterConnection, cancellationToken);
    }

    public override bool HasTables() => ExecutionStrategyExtensions.Execute<IDmRelationalConnection, bool>(this.Dependencies.ExecutionStrategyFactory.Create(), this._connection, (Func<IDmRelationalConnection, bool>) (connection => (int) this.CreateHasTablesCommand().ExecuteScalar(new RelationalCommandParameterObject((IRelationalConnection) connection, (IReadOnlyDictionary<string, object>) null, (IReadOnlyList<ReaderColumn>) null, this.Dependencies.CurrentContext.Context, this.Dependencies.CommandLogger)) != 0));

    public override Task<bool> HasTablesAsync(CancellationToken cancellationToken = default (CancellationToken)) => ExecutionStrategyExtensions.ExecuteAsync<IDmRelationalConnection, bool>(this.Dependencies.ExecutionStrategyFactory.Create(), this._connection, (Func<IDmRelationalConnection, CancellationToken, Task<bool>>) (async (connection, ct) =>
    {
      object obj = await this.CreateHasTablesCommand().ExecuteScalarAsync(new RelationalCommandParameterObject((IRelationalConnection) connection, (IReadOnlyDictionary<string, object>) null, (IReadOnlyList<ReaderColumn>) null, this.Dependencies.CurrentContext.Context, this.Dependencies.CommandLogger), ct);
      return (int) obj != 0;
    }), cancellationToken);

    private IRelationalCommand CreateHasTablesCommand() => this._rawSqlCommandBuilder.Build("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END FROM SYS.SYSOBJECTS WHERE SCHID = CURRENT_SCHID() AND TYPE$='SCHOBJ' AND SUBTYPE$='UTAB'");

    private IReadOnlyList<MigrationCommand> CreateCreateOperations()
    {
      DmConnectionStringBuilder connectionStringBuilder = new DmConnectionStringBuilder(((IRelationalConnection) this._connection).DbConnection.ConnectionString);
      return this.Dependencies.MigrationsSqlGenerator.Generate((IReadOnlyList<MigrationOperation>) new DmCreateUserOperation[1]
      {
        new DmCreateUserOperation()
        {
          UserName = connectionStringBuilder.User,
          Password = connectionStringBuilder.Password
        }
      }, (IModel) null, (MigrationsSqlGenerationOptions) 0);
    }

    public override bool Exists()
    {
      try
      {
        ((IRelationalConnection) this._connection).Open(true);
        ((IRelationalConnection) this._connection).Close();
        return true;
      }
      catch (DmException ex)
      {
        return false;
      }
    }

    public override async Task<bool> ExistsAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      try
      {
        int num = await ((IRelationalConnection) this._connection).OpenAsync(cancellationToken, true) ? 1 : 0;
        ((IRelationalConnection) this._connection).Close();
        return true;
      }
      catch (DmException ex)
      {
        return false;
      }
    }

    private static bool IsDoesNotExist(DmException exception) => exception.Number == 4060 || exception.Number == 1832 || exception.Number == 5120;

    private bool RetryOnExistsFailure(DmException exception) => exception.Number == 233 || exception.Number == -2 || exception.Number == 4060 || exception.Number == 1832 || exception.Number == 5120;

    public override void Delete()
    {
      using (IDmRelationalConnection masterConnection = this._connection.CreateMasterConnection())
        this.Dependencies.MigrationCommandExecutor.ExecuteNonQuery((IEnumerable<MigrationCommand>) this.CreateDropCommands(), (IRelationalConnection) masterConnection);
    }

    public override async Task DeleteAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      using (IDmRelationalConnection masterConnection = this._connection.CreateMasterConnection())
        await this.Dependencies.MigrationCommandExecutor.ExecuteNonQueryAsync((IEnumerable<MigrationCommand>) this.CreateDropCommands(), (IRelationalConnection) masterConnection, cancellationToken);
    }

    private IReadOnlyList<MigrationCommand> CreateDropCommands()
    {
      string user = new DmConnectionStringBuilder(((IRelationalConnection) this._connection).DbConnection.ConnectionString).User;
      if (string.IsNullOrEmpty(user))
        throw new InvalidOperationException(DmStrings.NoUserId);
      return this.Dependencies.MigrationsSqlGenerator.Generate((IReadOnlyList<MigrationOperation>) new MigrationOperation[1]
      {
        (MigrationOperation) new DmDropUserOperation()
        {
          UserName = user
        }
      }, (IModel) null, (MigrationsSqlGenerationOptions) 0);
    }
  }
}
