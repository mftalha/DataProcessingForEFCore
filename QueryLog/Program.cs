// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

Console.WriteLine("Hello, World!");
QueryLogDbContext context = new();

#region Query Log Nedir?
// LINQ sorguları neticesinde generate edilen sorguları izleyebilmek ve olası teknik hataları ayıklayabilmek amacı ile query log mekanizmasından istifade etmekteyiz.
#endregion
#region Nasıl Konfigüre Edilir?
// Microsoft.Extensions.Logging.Console => kütüphanesini nugetten indiriyoruz : projemize

await context.Persons.ToListAsync();

await context.Persons
    .Include(p => p.Orders)
    .Where(p => p.Name.Contains("a"))
    .Select(p => new { p.Name, p.PersonId })
    .ToListAsync();

#endregion
#region Filtreleme Nasıl Yapılır?
// 
#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public ICollection<Order> Orders { get; set;}
}

public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Person Person { get; set; }
}

class QueryLogDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
    // tüm veritabanı biz AddConsole seçtiğimizden console yazacaktır : onconfigure içindede gerekli düzeltmeden sorna
    //readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    #region Filtreleme Nasıl Yapılır?
    // LogLevel.Information seviyesindeki uyarıları logluyoruz.
    readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder
    .AddFilter((category, level) =>
    {
       return category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
    })
    .AddConsole());
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=QueryLogDb;Trusted_Connection=True;TrustServerCertificate=True");

        optionsBuilder.UseLoggerFactory(loggerFactory);
    }
}