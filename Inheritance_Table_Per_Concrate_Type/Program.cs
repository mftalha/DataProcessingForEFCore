// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

// sadece concrate class lar için tablo oluişturulur. Concrate olmayıp miras alınan soyut class lar için tablo oluşturulmaz. Miras alınan veriler direk her tablonun kendi içersindedir.
Console.WriteLine("Hello, World!");
TablePerConcrateDbContext context = new();

#region Table Per Concrete Type (TPC) Nedir?
// TPC davranışı, kalıtımsal ilişkiye sahip olan entitylerin olduğu çalışmalarda sadece concrete/ somut olan entity'lere karşılık bir tablo oluşturacak davranış modelidir.
// TPC, TPT'nin daha performansılı versiyonudur.
#endregion

#region TPC Nasıl Uygulanır?
// Hiyerarşik düzlemde abstract olan yapılar üzerinden Entity fonksiyonu ile konfigurasyona girip ardından UsTpcMappingStrategy fonksiyonu eşliğinde davranışın TPC olacağını belirleyebilririz.
#endregion

#region TPC Veri ekleme
// 
//await context.Technicians.AddAsync(new() { Name = "Talha", Surname = "Satir", Branch= "Software", Department = "Software Department" });
//await context.Technicians.AddAsync(new() { Name = "Mustafa", Surname = "uzay", Branch= "Software", Department = "Software Department" });
//await context.Technicians.AddAsync(new() { Name = "Hilmi", Surname = "boncuk", Branch= "Software", Department = "Software Department" });
//await context.SaveChangesAsync();
#endregion

#region TPC'de veri silme
//Technician technician = await context.Technicians.FindAsync(2);
//context.Technicians.Remove(technician);
//await context.SaveChangesAsync();
#endregion

#region TPC'de veri güncelleme
//Technician? technician = await context.Technicians.FindAsync(1);
//technician!.Name = "Test";
//await context.SaveChangesAsync();
#endregion

#region TPC'de veri sorgulama
var datas = await context.Technicians.ToListAsync();
Console.WriteLine();
#endregion

abstract class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}

class Employee: Person
{
    public string? Department { get; set; }
}

class Customer : Person
{
    public string? CompanyName { get; set; }
}

class Technician : Employee
{
    public string? Branch { get; set; }
}
class TablePerConcrateDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Technician> Technicians { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TablePerConcrateDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region TPC Nasıl Uygulanır?
        modelBuilder.Entity<Person>().UseTpcMappingStrategy();
        #endregion
    }
}