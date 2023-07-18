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

#region Discriminator Değerleri Nasıl değerlendirilir? :türü
// yine hiyerarşinin başındaki entity konfigirasyonellerine gelip, HasDiscriminator fonksiyonu ile özelleştirmede bulunarak ardından HasValue fonksiyonu ile hangi entity'e karşılık hangi değerin girileceğini belirtilen türde ifade edebiliriz.
#endregion

//Employee employee = new() { Name = "Talha", Surname = "Satir" };
//await context.Employees.AddAsync(employee);
//await context.SaveChangesAsync();

#region TPH'da veri ekleme
// Davranışların hiçbirinde veri eklerken, silerken, güncellerken vs. normal işlemlerin dışında bir işlem yapılmaz.
// hangi davranışı kullanıyor isek ef core ona göre arkaplanda modellemeyi gerçekleştirecektir.
//Employee e1 = new() { Name = "Talha", Surname = "Satir", Department = "Developer" };
//Employee e2 = new() { Name = "Uzay", Surname = "uCAR", Department = "Developer2" };
//Customer c1 = new() { Name = "Uzay", Surname = "Deniz", CompanyName = "Cus1" };
//Customer c2 = new() { Name = "Şuayip", Surname = "XYZ", CompanyName = "Cus2" };
//Technician t1 = new() { Name = "Rıfkı", Surname = "gol", Department = "Muhasebe", Branch = "sofor" };

//await context.Employees.AddAsync(e1);
//await context.Employees.AddAsync(e2);
//await context.Customers.AddAsync(c1);
//await context.Customers.AddAsync(c2);
//await context.Technicians.AddAsync(t1);
//await context.SaveChangesAsync();

#endregion

#region Tph'da veri silme
// TPH davranışında silme operasyonu yine entity üzerinden gerçekleştirilşir.
//Employee e1 = await context.Employees.FindAsync(3);
//context.Employees.Remove(e1);
//await context.SaveChangesAsync();

// customerların hepsini sil.
//var customers = await context.Customers.ToListAsync(); 
//context.Customers.RemoveRange(customers);
//await context.SaveChangesAsync();

#endregion

#region TPH'da Veri güncelleme
// tph davranışında güncelleme işlemi yine entity üzerinden gerçekleştirilir.

//Employee e1 = await context.Employees.FindAsync(8);
//e1.Name = "A";
//await context.SaveChangesAsync();
#endregion

#region TPH'da Veri sorgulama
// Veri sorgulama operasyonu bilinen DbSet propertsi üzerinden sorgulamadır. Ancak burada dikkat edilemsi gereken bir husus vardır. o da şu;
var employees = await context.Employees.ToListAsync();
//var techs = await context.Technicians.ToListAsync();
// kalıtımsal ilişkiye göre yapılan orgulamada üst sınıf alt sınıftaki verileride kapsamaktadır. O yüzden üst sınıfların sorgulamalarında alt sınıfların verileride gelecektir buna dikkat edilmelidir.
// sorgulama süreçlerinde ef core generate edilen sorguya bir where şartı eklemektedir.
#endregion

#region Farklı Entity'ler de aynı isimde sütunların olduğu durumlar
// Entitylerde mükerrer kolonlar olabilişr. Bu kolomnları ef core isimsel olarak özelleştirip ayıracaktır.
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
    public int A { get; set; }
    public string? CompanyName { get; set; }
}
class Technician : Employee
{
    public int A { get; set; }
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
        //modelBuilder.Entity<Person>()
        //    .HasDiscriminator<string>("ayiraci");// Discriminator kolonun type ve ismini değiştirebilriiz.

        #region Discriminator Değerleri Nasıl değerlendirilir? :türü

        //modelBuilder.Entity<Person>()
        //    .HasDiscriminator<string>("ayirici")
        //    .HasValue<Person>("A")
        //    .HasValue<Employee>("B")
        //    .HasValue<Customer>("C")
        //    .HasValue<Technician>("D");

        #endregion
    }
}