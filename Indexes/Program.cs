// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
IndexDbContext context = new();

// Her bir index veritabanında bir maliyeti vardır. o yüzden çok kullanılan kolonlar üzerinde index'leme yapılmalıdır. => her kolonu indexleriz mantığında veritabanı kasacağından genel bir performans sorunu çıkartabilir.

#region Index Nedir?
// Index, bir sütuna dayalı sorgulamarı daha verimli ve performanslı hale getirmek için kullanılan yapıdır.
#endregion

#region Index'leme nasıl yapılır?
// Default olarak PK, FK ve AK olan kolonlar otomatik olarak indexlenir. 

#region Index Attribute'u

#endregion

#region HasIndex Metodu

#endregion

#endregion

#region Composite Index
//context.Employees.Where(e => e.Name == "test" || e.Surname == "");
// biz name ve surname'i beraber composite bir şekilde indexler isek=> name ve surname beraber where'lediğimiz durumlarda bu indexleme sayesinde daha hızlı sonuç alabiliriz; Ama eğer ki sadece name veya sadece surname kolonu üzerinden where şartı uygular isek indexleme var hızlı sonuç alırız diyemeyiz : çünkü biz composite olarak indexleme gerçekleştirmiş oluyoruz.
#endregion

#region Birden fazla index tanımlama

#endregion

#region Index Uniqueness

#endregion

#region Index Sort Order - Sıralama Düzeni (EF Core 7.0)

#region AllDescending - Attribute
// Tüm indexlemlerde descending davranışının bütünsel olarak konfigurasyonunu sağlar.
#endregion

#region IsDescending - Attribute
// Indexleme sürecindeki her bir kolona göre sıralama davranışını husisi ayarlamak istiyorsak kullanılr.
#endregion

#region IsDescending Metodu

#endregion

#endregion

#region Index Name

#endregion

#region Index Filter
// bu işlem ile big data larda sorgulama sırasında hız sağlıyabiliriz.
#region HasFilter Metodu
#endregion

#endregion

#region Included Columns
// biz where şartı ile ilgili indexlenmiş kolonlar üzerinde işlem yaptık sonra select ile indexlenmemiş bazı kolonlarıda çekmek istiyoruz : bu durumda index'lerin yanında diğer kolonlarıda ana tabloya gidip yavaşlamaya maruz kalmadan  getirmek için ilgili özelliği kullanabiliriz.
#region IncludeProperties Metodu

#endregion
#endregion

//[Index(nameof(Name))] //Name kolonuna 1 tane index ata ;; class seviyeside kullanılır.
//[Index(nameof(Name), nameof(Surname))] = composite indexleme
//Birden fazla index olusturma [alt 2 li]
//[Index(nameof(Name))]
//[Index(nameof(Surname))]
//[Index(nameof(Name), IsUnique = true)] // index kolonun benzersiz olması için.
//[Index(nameof(Name), AllDescending = true)] // name değer descending olarak sıralanacktır
//[Index(nameof(Name), nameof(Surname), AllDescending = true)] // 2 değerde descending olarak sıralanacaktır
//[Index(nameof(Name), nameof(Surname), IsDescending = new[] { true, false })] // name descending , surname ascending sıralama ataması gerçekleştirdik.
//[Index(nameof(Name), Name ="name_index")] // ilgili inex'i isimlendirmek için
class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; } 
    public string? Surname { get; set; }
    public int Salary { get; set; }
}

class IndexDbContext : DbContext 
{
    public DbSet<Employee> Employees { get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=IndexDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region HasIndex Metodu
        //[Index(nameof(Name))] : data attribuyte
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Name);
        #endregion

        #region Composite Index
        // [Index(nameof(Name), nameof(Surname))] = data annotions
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => new { e.Name , e.Surname });
        //    //.HasIndex(nameof(Employee.Name), nameof(Employee.Surname));
        #endregion

        #region Birden fazla index tanımlama
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Name);
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Surname);
        #endregion

        #region Index Uniqueness
        // [Index(nameof(Name), IsUnique = true)] // index kolonun benzersiz olması için.
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Name)
        //    .IsUnique();
        #endregion

        #region IsDescending Metodu
        // Indexlemede Name kolonun descending sıralama olacağını belirtiyoruz
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Name)
        //    .IsDescending();

        // Indexlemede Name kolonun ascending sıralama olacağını belirtiyoruz
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Name)
        //    .IsDescending(false);

        // Indexlemede Name= descending, Surname = ascending sıralama davranışı verdik => composite
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => new { e.Name, e.Surname })
        //    .IsDescending(true, false);
        #endregion

        #region Index Name
        //[Index(nameof(Name), Name = "name_index")] // ilgili inex'i isimlendirmek için = data attribute
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Name)
        //    .HasDatabaseName("name_Index");
        #endregion

        #region HasFilter Metodu
        // filtrelenecek verilerde null olmıyanlaır filtrele diyerek hacmi azaltıyruz.
        //modelBuilder.Entity<Employee>()
        //    .HasIndex(e => e.Name)
        //    .HasFilter("[Name] IS NOT NULL");
        #endregion

        #region IncludeProperties Metodu
        modelBuilder.Entity<Employee>()
            .HasIndex(e => new { e.Name, e.Surname })
            .IncludeProperties(e => e.Salary); // select işleminde salary'ide kullabsak yavaşlamaya maruz kalmadan veriyi çekebiliriz.
        #endregion
    }
}