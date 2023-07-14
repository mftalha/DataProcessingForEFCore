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

#region Unicode | IsUnicode
// kolon içerisinde unicode karekterler kullanılacakise bu yapılandırmadan istifade edilebilir.

#endregion

#region Comment - HasComment
// EF Core üzerinden oluşturulmuş olan veritabanı nesneleri üzerinde bir açıkalma/yorum yapmak istiyorsak Comment'i kullanabilriz,
// mssql'de tablo için yorum bakma : Database - Tablo -  sag tık - properties - sol seçeneklerden Extended Properties - MS_Description alanının valuesinde ilgili açıklama yazacaktır.
// mssql'de column için yorum bakma : Database - Tablo - columns - ilgili satır'a sag tık - properties - sol seçeneklerden Extended Properties - MS_Description alanının valuesinde ilgili açıklama yazacaktır.
#endregion

#region ConcurrencyCheck - IsConcurrencyToken
// İleride/Sonraki derslerde veri tutarlılığı ile ilgili bir ders yapacağız.
// Bu derste bir satırdaki verinini bütünsel olarak tutarlılığını sağlıyacak bir concurrency token yapılanmasından bahsedeceğiz.
#endregion

#region InverseProperty
// {Inverse : Ters }
// iki entity arasında birden fazla ilkişki var ise eğer bu ilişkilerin hangi navigation  property'ler üzerinden olacağını ayarlamamızı sağlayan bir configurasyondur.
#endregion

#endregion

#region Configurations | Fluent API

#region Composite Key
//Tablolarda birden fazla kolonu kümalitf olarak primary key yapmak istiyr isek buna composite key denir
#endregion

#region HasDefaultSchema
// EF Core üzerinden inşa edilen herhangi bir veritabanı nesnesi default olarak dbo şemasına sahiptir. Bunu özellştirebilmek için configurasyondur.
#endregion

#region Property

#region HasDefaultValue
// Tablodaki herhangi bir alan'a defer gönderilmediğinde defult olarak hangi değerin alınacağını belirler.
#endregion

#region HasDefaultValueSql
// Tablodaki herhangi bir alan'a defer gönderilmediğinde defult olarak hangi sql cümleciğinden değerin alınacağını belirler.
#endregion

#endregion

#endregion

#region HasComputedColumnSql
// {computed: hesaplanmış}
// Tablolarda' ki birden fazla kolonu işleyerek değerini oluşturan kolonlara Computed column denmektedir. Ef Core üzerinde bu tarz computed column oluşturabilmek için kullanılan bir yapılandırmadır.
#endregion

#region HasConstraintName
// ef core üzerinden oluşturulan constraintlere default isim yerine özelleştirilmiş bir isim verebilmek için kullanılan yapılandırmadır.
#endregion

#region HasData
// {Seed : Tohum}
// sonraki derslerimizde Seed Data isimli bir konuyu incelecyeceğiz bu konuda migrate sürecinde veritabnını inşa eder iken bir yandan da yazılım üzerinden hazır veriler oluşturmak istiyor isek eğer bu konun yöntemini inceleyeceğiz.
// işte HasData konfigurasyonu bu operasyonun yapılandırma ayağıdır.
// HasData ile migrate sürecinde oluşturulacak olan verilerin pk olan id kolonlarına iradeli bir şekilde değerlerin girilmesi zorunludur.
#endregion

#region HasDiscriminator
// {discriminator : ayrımcı}
// İleride entityler arasında kalıtımsal ilişkilerin olduğu TPT VE TPH isminde kopnuları inceliyor olacağız. İşte bu konularla ilgili yapılandırmalarımız HasDiscriminator ve HasValue fonksiyonlardır.

#region HasValue

#endregion

//A a = new()
//{
//    X = "A dan",
//    Y = 1
//};

//B b = new()
//{
//    X = "B den",
//    Z = 2
//};

//Entity entity = new()
//{
//    X = "Entity'den"
//};

//await context.As.AddAsync(a);
//await context.Bs.AddAsync(b);
//await context.Entities.AddAsync(entity);

//await context.SaveChangesAsync();

#endregion

#region HasField
// Backing field özelliğini kullanmamızı sağlayan bir yapılanmadır.
#endregion

#region HasNoKey
// Normal şartlarda ef core'da tüm entitylerin primary key colomn'u olmak zorundadır. Eğerki entity'de pk kolonu olmıyacaksa bunun bildirilmesi gerekmektedir! işte bunun için kullanılan fonksiyondur.
#endregion

#region HasIndex
// Sonraki derslerde ef core üzerinden Index yapılanmasını detaylı olarak inceliyor olacağız.
// bu yapılanamya dair configurasyonlarımız HasIndex ve Index attribute'dur.
#endregion

#region HasQueryFilter
// ileride göreceğimiz Global Query Query Filter dersimizin yapılandırmasıdır.
// temeldeki görevi bir entity'e karşılık uygulama bazında global bir filtre koymaktır.
#endregion

#region DatabaseGenerated - ValueGeneratedOnAddOrUpdate, ValueGeneratedOnAdd, ValueGeneratedNever

#endregion

//[Table("Kisiler")]
class Person
{
    //[Key]
    public int Id { get; set; }
    //public int Id2 { get; set; }

    //[ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; } // DepartmentId

    //[Column("Adi", TypeName = "metin", Order = 7)] // TypeName = string'di ; olusturulurken :7. sırada olusturulsun
    //[Unicode]

    //public string Name { get; set; }
    public string _name;
    public string Name { get => _name; set => _name = value; }

    
    //[Required]
    //[MaxLength(13)]
    //[StringLength(14)]
    public string Surname { get; set; } // string? => nullable özelliği kazandırırız bu şekilde.
    //[Precision(5,3)] // 5 sayıdan fazla hane tutmayacak ; noktan sonrada 3 hane tutacak => solda 2 + sagda 3 = 5 : 12,345
    //[Comment("Bu şu işe yaramaktadır.")]
    public decimal Salary { get; set; }
    // NotMapped denemesi için olusturuldu
    //[NotMapped]
    //public string MyPropert { get; set; }
    //[Timestamp]
    //public byte[] RowVersion { get; set; }
    //[ConcurrencyCheck]
    //public int ConcurrencyCheck { get; set; }
    public DateTime CreatedDate { get; set; }
    public Department Department { get; set; }
}

class Exampe
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Computed { get; set; }
}

class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Person> Persons { get; set; }
}

class Entity
{
    public int Id { get; set; }
    public string X { get; set; }
}

class A : Entity
{
    public int Y { get; set; }
}

class B : Entity
{
    public int Z { get; set; }
}

class ConfigurationDbContext : DbContext 
{
    //public DbSet<Entity> Entities { get; set; }
    //public DbSet<A> As { get; set; }
    //public DbSet<B> Bs { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Exampe> Exampes { get; set; }
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
        #region HasPrecision
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Salary)
        //    .HasPrecision(5, 3);
        // 5 sayıdan fazla hane tutmayacak ; noktan sonrada 3 hane tutacak => solda 2 + sagda 3 = 5 : 12,345
        #endregion
        #region IsUnicode
        modelBuilder.Entity<Person>()
            .Property(p => p.Name)
            .IsUnicode();
        #endregion
        #region HasComment
        //modelBuilder.Entity<Person>()
        //    .HasComment("Bu tablo şuna yaramaktadır.")
        //    .Property(p => p.Name)
        //    .HasComment("Bu kolon şuna yaramaktadır.");
        #endregion
        #region IsConcurrencyToken
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.ConcurrencyCheck)
        //    .IsConcurrencyToken();
        #endregion
        #region CompositeKey
        //modelBuilder.Entity<Person>()
        //    .HasKey(p => new { p.Id, p.Id2});
        #endregion
        #region HasDefaultSchema
        //modelBuilder.HasDefaultSchema("ahmet");
        #endregion
        #region Property
        #region HasDefaultValue
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Salary)
        //    .HasDefaultValue(100);
        #endregion
        #region HasDefaultValueSql
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.CreatedDate)
        //    .HasDefaultValueSql("GETDATE()");// GETDATE() : sql'de o anki tarihi çağırma
        #endregion
        #endregion
        #region HasComputedColumnSql
        //modelBuilder.Entity<Exampe>()
        //    .Property(p => p.Computed)
        //    .HasComputedColumnSql("[X] + [Y]");
        #endregion
        #region HasConstraintName
        //modelBuilder.Entity<Person>()
        //    .HasOne(p => p.Department)
        //    .WithMany(d => d.Persons)
        //    .HasForeignKey(p => p.DepartmentId)
        //    .HasConstraintName("ahmet");
        #endregion
        #region HasData

        //modelBuilder.Entity<Department>()
        //    .HasData(
        //    new Department()
        //    {
        //        Id = 1,
        //        Name = "Department A"
        //    });

        //modelBuilder.Entity<Person>()
        //    .HasData(
        //    new Person{
        //        Id = 1,
        //        DepartmentId = 1,
        //        Name = "Ahmet",
        //        Surname = "Yılmaz",
        //        Salary = 100,
        //        CreatedDate = DateTime.Now,
        //    },
        //    new Person{
        //        Id=2,
        //        DepartmentId = 1,
        //        Name = "mehmet",
        //        Surname = "ucar",
        //        Salary = 200,
        //        CreatedDate = DateTime.Now,
        //    }
        //    );
        #endregion
        #region HasDiscriminator
        //modelBuilder.Entity<Entity>()
        ////.HasDiscriminator<string>("Ayirici"); // ayırma için kendi olsuturdugu colomn adını değiştiriyoruz [default: [Discriminator]] ; string dediğimde zaten default olarak bu colun string'dir : hangi entity'den veri eklendi ise onun ismini tutuyor : biz'de burda belirtiyoruz yeni isim ve type
        //.HasDiscriminator<int>("Ayirici") // olsuturuan colomn int olsun
        //.HasValue<A>(1) // A entityînden veri geldiğinde 1 yazsın
        //.HasValue<B>(2)
        //.HasValue<Entity>(3);

        #endregion
        #region HasField
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Name)
        //    .HasField(nameof(Person._name));
        #endregion
        #region HasNoKey
        //modelBuilder.Entity<Exampe>()
        //    .HasNoKey();
        #endregion
        #region HasIndex
        //modelBuilder.Entity<Person>()
        //    .HasIndex(p => new { p.Name, p.Surname });
        #endregion
        #region HasQueryFilter
        //modelBuilder.Entity<Person>()
        //    .HasQueryFilter(p => p.CreatedDate.Year == DateTime.Now.Year);
        #endregion
    }
}

//public class Flight
//{
//    //{ departure : kalkış ; arrival : varış}
//    public int FlightID { get; set; }
//    public int DepartureAirportId { get; set; }
//    public int ArrivalAirportId { get; set; }
//    public string Name { get; set; }
//    public Airport DepartureAirport { get; set; }
//    public Airport ArrivalAirport { get; set; }
//}
//public class Airport
//{
//    public int AirportID { get; set; }
//    public string Name { get; set; }

//    [InverseProperty(nameof(Flight.DepartureAirport))]
//    public virtual ICollection<Flight> DepartingFlights { get; set; }

//    [InverseProperty(nameof(Flight.ArrivalAirport))]
//    public virtual ICollection<Flight> ArrivingFlights { get; set; }
//}