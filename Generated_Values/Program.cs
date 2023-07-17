// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

GeneratedValueDbContext context = new();

#region Generated Value Nedir ?
// EF Core da üretilen değerler ile ilgili çeşitli modellerin ayrıntılarını yapılandırmamızı sağlayan bir konfigürasyondur.
#endregion

#region Default Values

// ef core'da herhangi bir tablonun herhangi bir colomn'una yazılım tarafından herhangi bir değer gönderilmediği taktdirde bu colomn'a hangi değerin(default value) üretilip yazdırılacağını belirleyen yapılanmalardır.

//Person p = new()
//{
//    //PersonId = 1,
//    Name = "Berk",
//    Surname = "Acar",
//    // Salary = 200,
//    Premium = 15,
//    TotalGain = 25,
//    //PersonCode = 2
//};

//await context.AddAsync(p);
//await context.SaveChangesAsync();

#region HasDefaultValue
// static veri veriyor.
#endregion

#region HasDefaultValueSql
// sql cümleciği veriliyor.
#endregion

#endregion

#region Computed Columns

#region HasComputedColumnSql
// Tablo içerisindeki kolonlar üzerinde yapılan aritmetik işlemler neticesinde üretilen kolondur.
#endregion

#endregion

#region Value Generation

#region Primary Keys
// Herhangi bir tablodaki satırları kimlik vari şekilde tanımlayan, tekil(unique) olan sütun veya sütunlardır.
#endregion

#region Identity
// Idendity, yanlızca otomatik olarak artan bir  sütündur, Bir stun, pk olmaksızın identity olarak tanımlanabilir.
// Bir tablo içersinde idendiy kolonu sadece tek bir tane olarak tanımlanabilir.
#endregion

// bu iki özellik genellikle birlikte kullanılmaktadırlar. o yüzden ef core pk olan bir kolonu otomatik olarak ıdentity olacak şekilde yapılandıraktadır. Ancak böyle olması için bir gereklilik yoktur.

#region DatabaseGenerated

#region DatabaseGeneratedOption.None - ValueGeneratedNever
// Bir kolonda değer üretilmeyecekse eğer None ile işaretliyoruz.
// EF Corun default olarak primary key kolonlarında getirdiği ındedity özelliğini kaldırmak istiyor isek None'ı kullanabiliriz.
#endregion

#region DatabaseGeneratedOption.Identity - ValueGeneratedOnAdd
// Herhangi bir koluna Idendity özelliğini vermemizi sağlayan bir konfigurasyondur.

#region Sayısal Türlerde
// Eğerki Identity özelliği bşr tabloda sayısal olan bir kolonda kullanılacaksa o durumda ilgili tabloda pk olan kolonda özellikle/iradeli bir şekilde identity özelliğin kaldırılması gerekmektedir.(None)
#endregion

#region Sayısal olmayan türlerde
// Eğer ki Identity özelliği bir tabloda sayısal olmayan bir kolonda kullanılacaksa o durumda ilgili tablodaki pk olan kolondan iradeli bir şekilde identity özelliğinin kaldırılmasına gerek yoktur.
#endregion

#endregion

#region DatabaseGeneratedOption.Computed - ValueGeneratedOnAddOrUpdate
// { Computed : Hesaplanmış }
// ef core üzerinde bir kolon computed kolon ise ister cpmputed olarak belirleyebiliriz istersek de belirlemeden kullanmaya devam edebiliriz. -- gereksiz gibi.

#endregion

#endregion

#endregion

public class Person
{
    //[DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int PersonId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Premium { get; set; }
    public int Salary { get; set; }
    public int TotalGain { get; set; }
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PersonCode { get; set; }
}

public class GeneratedValueDbContext : DbContext 
{
    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=GeneratedValueDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Default Values
        #region HasDefaultValue
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Salary)
        //    .HasDefaultValue(100);
        #endregion

        #region HasDefaultValueSql
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Salary)
        //    .HasDefaultValueSql("floor(rand() * 1000)");
        #endregion

        #endregion

        #region HasComputedColumnSql
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.TotalGain)
        //    .HasComputedColumnSql("([Salary] + [PersonCode]) * 10");
        #endregion

        #region DatabaseGeneratedOption.None - ValueGeneratedNever
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.PersionId)
        //    .ValueGeneratedNever();
        #endregion

        #region DatabaseGeneratedOption.Identity - ValueGeneratedOnAdd :: Sayısal olmayan türlerde

        //modelBuilder.Entity<Person>()
        //    .Property(p => p.PersonId)
        //    .ValueGeneratedNever();

        //modelBuilder.Entity<Person>()
        //    .Property(p => p.PersonCode)
        //    .HasDefaultValueSql("NEWID()"); //veritabanı ilgili kolonun özelliğine göre benzersiz id üretir(int, gui)

        //modelBuilder.Entity<Person>()
        //    .Property(p => p.PersonCode)
        //    .ValueGeneratedOnAdd();
        #endregion

        #region DatabaseGeneratedOption.Computed - ValueGeneratedOnAddOrUpdate
        modelBuilder.Entity<Person>()
            .Property(p => p.TotalGain)
            .HasComputedColumnSql("([Salary] + [PersonCode]) * 10")
            .ValueGeneratedOnAddOrUpdate();
        #endregion
    }
}