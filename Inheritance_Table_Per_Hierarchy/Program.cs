// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
InheritanceTablePerHierarchyDbContext context = new();

#region Table Per Hierarchy (TPH) Nedir?
// Kalıtımsal ilişkiye sahip olan entiylerin olduğu senaryolarda her bir hiyerarşiye karşılık bir tablo oluşturan davranıştır.
#endregion

#region Neden Table Per Hierarchy yaklaşımında bir tabloya ihtiyacımız olsun?
// içerisinde benzer alanlara sahip olan entityleri migrate ettiğimizde her entity'e karşılık bir tablo oluşturmaktan sa bu entity'leri tek bir tabloda modelleyebilmek isteyebilir ve bu tablodaki kayıtları discriminator kolonu üzerinden ayırabiliriz. işte bu tarz bir tablonun oluşturulması ve bu tarz bir tabloya göre sorgulama veri ekleme, sileme vs gibi operasyonların şekillendirilmesi için tph dabranışını kullanabiliriz.
#endregion

#region TPH Nasıl Uygulanır?
// ef core'da entiyler arasında temel bir kalıtımsal ilişki söz konusu ise default olarak kabul edilen davranıştır.
// bu yüzden herhangi bir configurasyon gerektirmez.
// Entityler kendi aralarında kalıtımsal ilişkiye sahhip olmalı ve bu entitylerin hepsi DbContext nesnesine DbSet olarak eklenmelidir.
#endregion

#region Discriminator Kolonu Nedir?
// Table per hierarchy yaklaşımı neticesinde kümalit olarak inşa edilmiş tablonun hangi entity'e karşılık veri tuttuğunu ayırt edebilememizi sağlayan kolondur.
// Ef core tarafından otomatik olarak tabloya yerleştirilir.
// default olarak içerisinde entity isimlerini tutar
// discriminator kolonunu komple özelleştirebiliriz.
#endregion

#region Discriminator kolon adı nasıl değiştirilir?
// öncelikle hierarcyin başında hangi entity var ise onun fluent api'ada konfigurasyonuna gidilmeli
// ardından HasDiscriminator<string>("ayiraci"); fonksiyonu ile özelleitirilmeli
#endregion


#region Discriminator Değerleri Nasıl değerlendirilir?
// 29:17
#endregion

#region TPH'da veri ekleme

#endregion

#region Tph'da veri silme

#endregion

#region TPH'da Veri güncelleme

#endregion

#region TPH'da Veri sorgulama

#endregion

#region Farklı Entity'ler de aynı isimde sütunların olduğu durumlar

#endregion

#region IsComplete Konfigürasyonu

#endregion

abstract class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname {  get; set;}

}
class Employee : Person
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

class InheritanceTablePerHierarchyDbContext : DbContext
{
    public DbSet<Person> Persons { get; set;}
    public DbSet<Employee> Employees { get; set;}
    public DbSet<Customer> Customers { get; set;}
    public DbSet<Technician>Technicians { get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=InheritanceTablePerHierarchyDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("ayiraci");// Discriminator kolonun type ve ismini değiştirebilriiz.
    }
}