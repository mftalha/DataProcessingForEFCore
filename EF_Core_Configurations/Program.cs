// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// {Flight: Uçuş, Airport: Havalimanı}
Console.WriteLine("Hello, World!");
ConfigurationDbContext context = new();

#region EF Core'da neden yapılandırmalara ihtiyacımız olur ?
// Default davranışları yeri geldiğinde geçersiz kılmak ve özelleştirmek isteyebiriiz bundan dolayı yapılandırmalara ihtiyacımız olacaktır.
#endregion

#region onModelCreating Metodu
// EF Core'da yapılandırma diyince akla ilk gelen methot onModelCreating metodur.
// Bu method dbcontext methodu içinde virtual olarak ayarlanmış bir methoddur
// Bizler bu methodu kullanarak model'lerimiz ile ilgili temel konfigurasyonel davranışları(Fluent API) sergileyebilriz.
// Bir model'ın yaratılışı ile ilgili tüm konfigurasyonları burada gerçekleştirebilmekteyiz.

#region GetEntityTypes
// EF Core'da kullanılan entity'leri elde etmek, programatik olarak öğrenmek istiyor isek eğer GetEntityTypes fonksiyonunu kullanabiliriz.
#endregion

#endregion

#region Configurations | Data Annotations & Fluent API
// Data Annotations - Fluent API (region - mantığı)
#region Table - ToTable
// Generate edilecek tablonun ismini belirlememizi sağlayan yapılandırmadır.
// EF Core normal şartlarda generate edeceği tablonun ismini DbSet property'sinden almaktadır. Bizler eğer ki bunu özelleştirmek istiyorsak Table attribute'unu yahut ToTable api'ını kullanabiliriz.
#endregion

#region Column - HasColumnName, HasColumnType, HasColunOrder
// EF Core'da tabloların kolonları entşty sınıfları içerisinde'ki propery'lere karşılk gelmektedir
// default olarak porpery adı, kolon adı iken, türleri/tipleri kolon türleridir.
// eğerki generate edilecek kolon isimlerine ve türlerine müdahale etmek istiyorsak bu konfigurasyon kullanılır.
#endregion

#region ForeignKey  - HasForeignKey
// İlişkisel tablo tasarımlarında, bağımlı tabloda esas tabloya karşılık gelecek verilerin tutulduğu kolonu foreign key olarak etmsil etmekteyiz.
// EF Core'da foreign key kolonu genellikle Entity tanımlama kuralları gereği default yapılanmalarla oluşturulur.
// ForeignKey Data Annotions Attribute'unu direkt kullanabilirsiniz. Lakin Fluent api ile bu konfigürasyonu yapacak isek  iki entity arasındaki ilişkiyide modellememiz gerekmektedir. Aksi taktirde fluent api üzerinde HasForeignKey fonksiyonunu kullanamayız!
#endregion

#region NotMapped - Ignore
// EF Core entity sınıfları içersindeki tüm propertleri default olarak modellenene tabloya kolon şeklinde migrate eder.
// Bazen bizler entity sınıfları içersinde tablolada bir kolona karşılk gelmeyen propertyler tanımlamak mecburiyetinde kalabiliyoruz.
// Bu propert'lerin ef core tarafından kolon olarak map edilmesini istemediğimizi bildirmek için NotMapped ya da Ignore kullanabiliriz.
#endregion

#region Key - HasKey
// EF Core'da default convention olarak bir entity'nin içerisindeki Id, ID, EntityId(Entity ismi), EntityID vs. şeklinde tanımlanan tüm properyler varsayılan olarak primary key constraint uygulanır.
// Key ya da  HasKey yapılanmalarıyla istediğimiz herhangi bir porpery'e default convention dışında pk uygulayabiliriz.
// EF Core'da bir entity içerisinde kesinlikle pk veya pk'yi temsil edecek bir propert bulunmalıdır. Aksi taktirde EF Core migration oluştururken hata verecektir.
// Eğerki tablonun pk'yi yok ise bildirilmesi gerekir.
#endregion

#region Timestamp - IsRowVersion
// İleride/Sonraki derslerde veri tutarlılığı ile ilgili bir ders yapacağız.
// Bu derste bir satırdaki verinini bütünsel olarak değişiklişğini takip etmemizi sağlıyacak olan verisyon mantığını konuşuyor olacağız.
// işte bir verinini versiyonunu oluşturmamızı sağlayan yapı bu configurasyonlardır.
#endregion

#region Required - IsRequired
// Bir kolonun nullable olup yada not null olup olmamasını belirleyebiriirz, Bu configurasyon ile
// ef core'da bir propert olarak not null olarka tanımlanır eğerki propert'i nullable yapmak istiyor isek : türünde soru işareti yapmamız gerekir. : string? gibi
#endregion

#region MaxLength - HasMaxLength | StringLength - HasMaxLength
// Bir kolonun max karekter sayısını belirlememizi sağlar.
#endregion

#region Precision - HasPrecision
//{ Precision : Kesinlik }
// Küsüratlı sayılar'da bir kesinlik belirtmemizi sağlayan ve noktanın hanesini bildirmemizi sağlayan bir yapılanmadır.
#endregion

#endregion



//[Table("Kisiler")]
class Person
{
    //[Key]
    public int Id { get; set; }

    //[ForeignKey(nameof(Department))]
    public int DId { get; set; } // DepartmentId

    //[Column("Adi", TypeName = "metin", Order = 7)] // TypeName = string'di ; olusturulurken :7. sırada olusturulsun
    public string Name { get; set; }
    //[Required]
    //[MaxLength(13)]
    //[StringLength(14)]
    public string Surname { get; set; } // string? => nullable özelliği kazandırırız bu şekilde.
    [Precision(5,3)] // 5 sayıdan fazla hane tutmayacak ; noktan sonrada 3 hane tutacak => solda 2 + sagda 3 = 5
    public decimal Salary { get; set; }
    // NotMapped denemesi için olusturuldu
    //[NotMapped]
    //public string MyPropert { get; set; }
    //[Timestamp]
    //public byte[] RowVersion { get; set; }
    public DateTime CreatedDate { get; set; }
    public Department Department { get; set; }
}

class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Person> Persons { get; set; }
}

class ConfigurationDbContext : DbContext 
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Department> Departments { get; set; }
    //public DbSet<Flight>Flights { get; set; }
    //public DbSet<Airport> Airports { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ConfigurationDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region GetEntityTypes
        // migration olustururken package manager console yazacaktır
        //var entities = modelBuilder.Model.GetEntityTypes();
        //foreach (var entity in entities)
        //{
        //    Console.WriteLine(entity.Name);
        //}
        #endregion
        #region ToTable
        //modelBuilder.Entity<Person>().ToTable("test Tablo İsmi");
        #endregion
        #region ColumnName, ColumnType, ColumnOrder
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Name)
        //    .HasColumnName("Adi")
        //    .HasColumnType("asdsad")
        //    .HasColumnOrder(7); //7. sırada olsun tabloda
        #endregion
        #region HasForeignKey
        //modelBuilder.Entity<Person>()
        //    .HasOne(p => p.Department)
        //    .WithMany(d => d.Persons)
        //    .HasForeignKey(p => p.DId);
        #endregion
        #region Ignore
        //modelBuilder.Entity<Person>()
        //    .Ignore(p => p.MyPropert);
        #endregion
        #region HasKey
        //pirimary key belirtme
        //modelBuilder.Entity<Person>()
        //    .HasKey(p => p.Id);
        #endregion
        #region IsRowVersion
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.RowVersion)
        //    .IsRowVersion();
        #endregion
        #region Required
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Surname).IsRequired();
        #endregion
        #region MaxLength
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Surname)
        //    .HasMaxLength(13);
        #endregion
    }
}

public class Flight
{

}
public class Airport
{

}