// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
FunctionDbContext context = new();

#region Scalar functions nedir ?
// Geriye herhangi bir türde değer döndüren fonksiyonlardır(db'de)
#endregion
#region Scalar Function oluşturma
// 1. Adım : boş bir migration oluşturulmalı
// 2. Adım : bu migration içerisinde Up metodunda Sql metodu eşliğinde fonksiyonun create kodları yazılacak, Down metodu içersiinde de Drop kodları yazılacaktır.
// 3. Adım : Migrate edilmeli
#endregion
#region Scalar Function'ı EF Core'a Entegre Etme

#region HasDbFunction
// Veritabanı seviyesindeki herhangi bir fonksiyonu ef core/yazılım kısmındaki bir methoda bind etmemizi sağlayan bir fonskyiondur. 
#endregion
// GetPersonTotalOrderPrice => c# daki bu methodu tetikleyrek veritabanındaki fonksiyonu tetikledik.
//var persons = await (from person in context.Persons
//                     where context.GetPersonTotalOrderPrice(person.PersonId) > 500
//                     select person).ToListAsync();
//Console.WriteLine();
#endregion

#region Inline Function Nedir?
// Geriye bir değer değil, tablo döndüren fonksiyonlardır.
#endregion
#region Inline Funtion Oluşturma
// 1. adım: boş bir migration oluşturulur.
// 2. adım: bu migration içersindeki Up fonksiyonunda create işlemi, down fonksiyonunda ise drop işlemlerini gerçekleştiririz.
// 3. adım: migrate ederiz.
#endregion
#region Inline Function'ı EF Core'a Entegre Etme
var persons = await context.BestSellingStaff(500).ToListAsync();
Console.WriteLine();
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

public class BestSellingStaff
{
    public string Name { get; set; }
    public int OrderCount { get; set; }
    public int TotalOrderPrice { get; set; }
}

class FunctionDbContext : DbContext 
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Scalar
        // veritabanı fonksiyon ile burada oluşturduğumuz fonksiyonu eşleştiriyoruz.
        modelBuilder.HasDbFunction(typeof(FunctionDbContext).GetMethod(nameof(FunctionDbContext.GetPersonTotalOrderPrice), new[] {typeof(int) }))
            .HasName("getPersonTotalOrderPrice");
        #endregion

        #region Inline
        modelBuilder.HasDbFunction(typeof(FunctionDbContext).GetMethod(nameof(FunctionDbContext.BestSellingStaff), new[] { typeof(int) }))
            .HasName("bestSellingStaff");

        modelBuilder.Entity<BestSellingStaff>()
            .HasNoKey();
        #endregion

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }

    #region Scalar Functions
    //veritabanındaki GetPersonTotalOrderPrice fonskiyonunu karşılamak için bir imza koyuyoruz ortaya
    public int GetPersonTotalOrderPrice(int personId)
            => throw new Exception();
    #endregion
    #region Inline Functions
    // veritabanındaki bestSellingStaff fonskiyonunu karşılamak için bir imza koyuyoruz ortaya
    public IQueryable<BestSellingStaff> BestSellingStaff(int totalOrderPrice = 0)
        => FromExpression(() => BestSellingStaff(totalOrderPrice));
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FunctionDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}

// mig_1 : temel tabloların oluşturulması
// mig_2 : Scalar Functions oluşturma ve kaldırma için gerekli fonsiyonlar eklendi
// mig_3 : Inline Functions oluşturma ve kaldırma için gerekli fonsiyonlar eklendi