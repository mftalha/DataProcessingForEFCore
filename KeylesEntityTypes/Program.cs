// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
KeylessDbContext context = new();

#region Keyless Entity Types
// {keyless : anahtarsız }
// Normal entity taglara ek olarak primary key içermeyen querylere karşı veritabanı sorguları yürütmek için kullanılan bir özelliktir KET.
// Genellikle aggreate operasyonların yapıldığı group by yahut pivot table gibi işlemler neticesinde elde edilen istatistiksel sonuçlar primary key kolonu barındırmazlar. Bizler byu tarz sorguları Keyless  Entity Types ile sanki bir entity'e karşılık geliyormuş gibi çalıştırabiliyoruz.
#endregion

#region Keyless Entity Types Tanımlama
// 1. Hangi sorgu olursa olsun EF Core üzerinden bu sorgunun bir entity'e karşılık geliyormuş gibi işleme/execute'a/çalıştırmaya tabi tutulabilemsi için o sorgunun sonucunda bir entity'in yine'de tasarlanması gerekmektedir. : PersonOrderCount entity'in oluşturulması
// 2. Bu entity'nin DbSet property'si olarak DbContext nesnesine eklenmesi gerekmektedir.
// 3. Tanoımlamış olduğumuz entity'e onModelCreating fonksiyonu içine girip bunun bir key'i olmadığını bildirmeli(HasNoKey) ve hangi sorgunun çalıştırılacağı'da ToView vs. gibi işlemlerle ifade edilmelidir.

//var datas = await context.PersonOrderCounts.ToListAsync();
//Console.WriteLine();

#region Keyless Attribute'u
//[Keyless] //(data attribute) : Fluent API' deki  = HasNoKey  => aynı işe yararlar.
// Primary key kolonu olmaz!
// Change Tracker mekanizması aktif olmıyacaktır. : update , add , delete gibi property'ler olmıyacaktır.
// TPH olarak  entity hiyerarşisinde kullanılabilir lakin diğer kalıtımsal ilişkilerde kullanılamaz!
#endregion
#endregion

#region Keyless Entity Types Özellikleri Nelerdir?

#endregion

// [Keyless] //(data attribute) : Fluent API' deki  = HasNoKey 
public class PersonOrderCount
{
    public string Name { get; set;}
    public int Count { get; set; }
}

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

class KeylessDbContext : DbContext 
{ 
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PersonOrderCount> PersonOrderCounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);

        //view'e karşılık bir tablo oldugunu belirttiğimiz için ef core db'de PersonOrderCount için bir tablo oluşturmayacaktır. hasnokey deme sebebimiz'de bir pimary key'inin olmamasıdır.
        // ilgili view mig_2 migration'unun içinde oluşturuldu.
        modelBuilder.Entity<PersonOrderCount>()
            .HasNoKey()
            .ToView("vw_PersonOrderCount");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=KeylessDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}


// ilgili view mig_2 migration'unun içinde oluşturuldu.