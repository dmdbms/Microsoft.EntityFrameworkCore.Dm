using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Tests.NET6
{
    internal class TestDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new DmDbCommandInterceptor());
            optionsBuilder.LogTo(msg => {
                if(msg.Contains("__EFMigrationsHistory"))
                    Console.WriteLine(msg);
            });
            optionsBuilder.UseDm("SERVER=127.0.0.1;PORT=52360;USER=SYSDBA;PASSWORD=123456789");
        }
    }

    class DmDbCommandInterceptor: DbCommandInterceptor
    {
        public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
        {
            base.CommandFailed(command, eventData);
        }

    }
}
