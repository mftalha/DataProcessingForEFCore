// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

Console.WriteLine("Hello, World!");
QueryTagDbContext context = new();

#region Query Tags Nedir
// EF Core ile generate edilen sorgulara açıklama eklememizi sağlıyarak; SQL Profiler, Query Log vs. gibi yapılarda bu açıklamlar eşliğinde sorguları gözlemlememizi sağlayan bir özelliktir.

//await context.Persons.ToListAsync();

#endregion
#region TagWith Metodu
// sorgu daha IQuaryable iken TagWith kullanılabilir.
// profiler gibi veriyi izlediğimiz yerlerde'de bu açıklamayı görmekteyiz.
//await context.Persons.TagWith("Örnek bir açıklama...").ToListAsync();

#endregion
#region Multiple TagWith
//await context.Persons.TagWith("Tüm personeller çekilmiştir").Include(p => p.Orders).TagWith("Personelerin yapğtığı satışlar sorguya eklenmiştir").ToListAsync();
#endregion
#region TagWithCallSite Metodu
// Oluşturulan sorguya açıklama satırı ekler ve ek olarak bu sorgunun bu dosyada(.cs) hangi satırda üretildiğinin bilgisinide verir.
await context.Persons.TagWithCallSite("Tüm personeller çekilmiştir").Include(p => p.Orders).TagWithCallSite("Personelerin yapğtığı satışlar sorguya eklenmiştir").ToListAsync();
#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public ICollection<Order> Orders { get; set; }
}

public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Person Person { get; set; }
}

class QueryTagDbContext : DbContext
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
    readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder
    .AddFilter((category, level) =>
    {
        return category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
    })
    .AddConsole());
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=QueryTagDb;Trusted_Connection=True;TrustServerCertificate=True");
        optionsBuilder.UseLoggerFactory(loggerFactory);
    }
}