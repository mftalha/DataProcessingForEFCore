// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
GlobalQueryFilterDbContext context = new();

#region Global Query Filters Nedir?
// Bir entity'e özel uygulama seviyesinde genel/ön kabullü şartla oluşturmamızı ve böylece verileri global bir şekilde filtrelememizi sağlayan bir özelliktir.
// Böylece belirtilen entity üzerinden yapılan tüm sorgularda ekstradan bir şart ifadesine gerek kalmaksızın filtreleri otomatik uygulayarak hızlıca sorgulama yapmamızı sağlamaktadır.

// Genellişkle uygulama bazında aktif(IsAktif) gibi verilerle çalısıldığı durumlarda
// MultiTennancy uygulamalarda TenantId tanımlarken vs. kullanılabilir.
#endregion
#region Global Query filters nasıl uygulanır?
//await context.Persons.Where(p => p.IsActive).ToListAsync();
//await context.Persons.ToListAsync();
#endregion
#region Navigation Property üzerinden global query filters kullanımı
//var p = await context.Persons
//        .AsNoTracking()
//        .Include(p => p.Orders)
//        .Where(p => p.Orders.Count > 0)
//        .ToListAsync();

//var p2 = await context.Persons.AsNoTracking().ToListAsync();
//Console.WriteLine();
#endregion
#region Global Query filters nasıl ignore edilir?
//// IgnoreQueryFilters()
//var person = await context.Persons.ToListAsync();
//var person2 = await context.Persons.IgnoreQueryFilters().ToListAsync();
//Console.WriteLine();
#endregion
#region Dikkat edilmesi gereken husus!
// Global Query Filter uygulanmış bir kolona farkında olmaksızın şart uygulanabilmektedir. Bu duruma dikkat edilmelidir. : yani zaten global bir şekilde atılan filtreyi gelipte tekrar atarsak => sorun olmaz ama fazladan aynı filtreyi atıp maliyete girmiş oluruz.
//await context.Persons.Where(p => p.IsActive).ToListAsync();
#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public Person Person { get; set; }
}
class GlobalQueryFilterDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=GlobalQueryFilterDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region Global Query filters nasıl uygulanır? 
        // bu filtreyi tüm Person entity sorgulamalarında otomatik ekle.
        modelBuilder.Entity<Person>().HasQueryFilter(p => p.IsActive);
        #endregion

        #region Navigation Property üzerinden global query filters kullanımı
        // orders sayıları 0 dan fazla ise getir. : personelleri
        //modelBuilder.Entity<Person>().HasQueryFilter(p => p.Orders.Count > 0);
        #endregion

    }
}