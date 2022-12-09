using Tests.NET6;
using TestDbContext ctx = new TestDbContext();;
//ctx.Database.Migrate();
Console.WriteLine(ctx.Persons.Count());