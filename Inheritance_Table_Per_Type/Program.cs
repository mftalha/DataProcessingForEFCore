// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
TablePerTypeDbContext context = new();

// TPT : Maliyet açısından TPH'ye göre daha maliyetlidir ama kullanımsal doğruluk açısından(1 tabloda 1 entity verisi tutma) daha doğrudur.

#region Table Per Type (TPT) Nedir?
// Entitylerin aralarında kalıtımsal ilişkiye sahip olduğu durumlarda her bir türe/entitye/tip/referans karşılık bir tablo generate eden davranıştır.
// her generate edilen bu tablolar hiyerarşik düzlemde kendi aralarında birebir ilişkiye sahiptir.
#endregion

#region TPT Nasıl Uygulanır?
// TPT'yi uygulayabilmek için öncelikle entitylerin kendi aralarında olması gereken mantıkda inşa edimesi gerekmektedir.
// Entityler DbSet olarak  bildirilmelidir.
// Hiyerarlşik olarak aralarında kalıtımsal ilişki olan tüm entityler OnModelCreating ToTable metodu ile konfigure edilmelidir. Böylece ef core kalıtımsal ilişki olan bu tablolar arasında TPT davranışı oldugunu anlıyacaktır.
#endregion

#region TPT'de Veri Ekleme
//Technician technician = new() { Name = "Mehmet", Surname = "yildiz", Department = "Developer", Branch = "EnCoder" };
//await context.Technicians.AddAsync(technician);

//Customer customer = new() { Name = "Hilmi", Surname="calayci", CompanyName= "cay"};
//await context.Customers.AddAsync(customer);
//await context.SaveChangesAsync();

//eklemede ilişkili tablolar ile aynı id'ye sahip olur veri : eğerki ilgili tabloda mesela id değerinde 1 yok ise ama ilişkili tabloda veri 3 id'si ile eklendi ise; ilişkili tabloya ilk eklenecek veride 3 id si ile eklenir : çünkü aynı id değerine sahip olurlar tüm ilişkili tablolar.(e nasıl 3 id li oluyor der isek diğer tablo : direk o tabloya ekleme yapılmıitır mirasın en üstündedir veya o tabloya bağlı başka tablodan ekleme yapılmıştır gibi mantıklar olabilir. : ama alttaki tabloya ekleme yapıldıgında üstteki tablo ile aynı id de olmalı.)
#endregion

#region TPT'de Veri Silme
//Employee employee = await context.Employees.FindAsync(4);
//context.Employees.Remove(employee);
//await context.SaveChangesAsync();
// silinen veriyele ilişki tüm tablolarda ilgili id li veriyi silecektir.

#endregion

#region TPT'de Veri Güncelleme
//Technician? technician = await context.Technicians.FindAsync(2);
//technician!.Name = "Test";
//context.SaveChanges();
#endregion

#region TPT'de Veri Sorgulama

//Employee employee = new() { Name = "Fatih", Surname = "Yavuz", Department = "ABC" };
//await context.AddAsync(employee);
//await context.SaveChangesAsync();

//var technicians = await context.Technicians.ToListAsync();
//var employees = await context.Employees.ToListAsync();
//Console.WriteLine();

// ben employees tablosundalki verileri bile sorgulasam Technicians tablosu ve Technicians tablosu arasındaki ilişkiden dolayı eklenmiş ise  Technicians tablosundaki verielrde gelecektir ama eğerki direk Employees tablosuna veri eklersem veriyi çektiğimde direk Employees tablosundaki veriler gelir sadece.
#endregion

abstract class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }   
    public string? Surname { get; set; }
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

class TablePerTypeDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Technician> Technicians { get; set; }  

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TablePerTypeDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region TPT Nasıl Uygulanır?
        // bu şekilde yapınca tpt oldugunu algılşıyor : yoksa default olan tph ye göre tablo yapısını oluşturur ef core
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Technician>().ToTable("Technicians");
        #endregion
    }

}