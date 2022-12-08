using Dm;
using Microsoft.EntityFrameworkCore;
using Tests.NET6;

/*
using DmConnection connection = new DmConnection(SqlHelper.ConnStr);
connection.Open();
using DmCommand cmd = (DmCommand)connection.CreateCommand();
cmd.CommandText = "SELECT ID FROM SYS.SYSOBJECTS WHERE  SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME =  (SELECT USER())) AND NAME = '__EFMigrationsHistory';";
object obj = cmd.ExecuteScalar();
Console.WriteLine(obj);
return;*/

using TestDbContext ctx = new TestDbContext();
ctx.Database.Migrate();
Console.WriteLine(ctx.Persons.Count());