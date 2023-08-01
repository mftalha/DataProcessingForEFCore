// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

Console.WriteLine("Hello, World!");
AdoNetDbContext context = new();

#region Database Property'si
// Database property'si veritabanını temsil eden ve EF Core'un bazı işlevlerininin detaylarına erişmemizi sağlayan bir propertydir.
#endregion
#region BeginTransaction
//IDbContextTransaction transaction = context.Database.BeginTransaction();
// db işlemlerinde artık kontrolü ele alıyoruz.
// EF Core, transaction yönetimini otomatik bir şekilde kendisi gerçekleştirmektedir. Eğer ki transaction yönetimini manuel olarak anlık ele almak istiyorsak BeginTransaction fonksiyonunu kullanabiliriz.
#endregion
#region CommitTransaction
// DB üzerinden yapılan işlemelerde bir commit ihtiyacı varsa bu şekilde kullanabiliyoruz.
// EF Core üzerinde yapılan işlemelrin commit edilebilmesi için kullanılan bir fonksiyondur.(savechangeste aynı işlemi yapar.(biz burada manuel yapıyoruz))
//context.Database.CommitTransaction();
#endregion
#region RollBackTransaction
// EF Core üzerinde yapılan işlemelrin RollBack(geri alma) edilebilmesi için kullanılan bir fonksiyondur.
//context.Database.RollbackTransaction()
#endregion
#region CanConnect
// db ye bağlanıp bağlanamdığının kontrol edilmesi => bağlantı sorumluluğunu biz üstleniriz ama.(mts)
// Verilen connection string'e karşılık bağlantı kurulabilir bir veritabanı var mı yokm mu bunun bilgisini bool türden veren bir fonksiyondur.
//bool connect = context.Database.CanConnect();
// true dönmesi için migrate işlemlerinin gerçekleştirilip db de database olması gerekiyor.
//Console.WriteLine(connect);
#endregion
#region EnsureCreated
// EF Core'da tasarlanan db tasarımını migration kullanmaksızın runtime'da yani kod üzerinde veritabanı sunucusuna inşa edebilmek için kullabnılan bir fonksiyondur.
//context.Database.EnsureCreated();
#endregion
#region EnsureDeleted
// İnşa edilmiş veritabanını runtime'da silmemizi sağlayan bir fonksiyondur
//context.Database.EnsureDeleted();
#endregion
#region GenerateCreateScript
// Context nesnesinde yapılmış olan veritabanı tasarımı her ne ise ona uygun bir SQL Script'ini string olarak veren metottur.
//var script = context.Database.GenerateCreateScript();
//Console.WriteLine(script);
#endregion
#region ExecuteSql
// Veritabanına yapılacak Insert, Update ve Delete sorgularını yazdığımız bir metottur. bu metot işlevsel olarak alacağı parametreleri SQL Injection saldırılarına karşı korumaktadır.(string Interpreter bu mantığı yardımcı olur : girilen veriyi sql data type çevirme gibi bir işlem yapıyor arka planda.)
//string name = Console.ReadLine(); 
//var result = context.Database.ExecuteSql($"Insert Persons Values('{name}')");
#endregion
#region ExecuteSqlRaw
// Veritabanına yapılacak Insert, Update ve Delete sorgularını yazdığımız bir metottur. bu metotta ise sorguyu SQL Injection saldırılarına karşı koruma görevi geliştiricinin sorumluluğundadır.

//string name = Console.ReadLine();
//var result = context.Database.ExecuteSqlRaw("Insert Persons Values('asd')");
#endregion
#region SqlQuery
// SQLQuery fonksiyonu her ne kadar erişilebilir olsa da artık desteklenmektedir. Bunun yerine DbSet propertysi üzerinden erişilebilen FromSql fonksiyonu gelmiştir/kullanılmaktadır.
#endregion
#region SqlQueryRaw
// SqlQueryRaw fonksiyonu her ne kadar erişilebilir olsa da artık desteklenmektedir. Bunun yerine DbSet propertysi üzerinden erişilebilen FromSqlRaw fonksiyonu gelmiştir/kullanılmaktadır.
#endregion
#region GetMigrations
// Uygulamada üretilmiş olan tüm migrationları runtime'da elde etmemizi sağlayan methottur.
//var migs = context.Database.GetMigrations();
//Console.WriteLine();
#endregion
#region GetAppliedMigrations
// { applied : uygulamalı}
// Uygulamada migrate edilmiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur.
//var migs = context.Database.GetAppliedMigrations();
//Console.WriteLine();
#endregion
#region GetPendingMigrations
// { pending : askıda olması, kadar, sırasında}
// Uygulamada migrate edilmemiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur.
//var migs = context.Database.GetPendingMigrations();
//Console.WriteLine( );
#endregion
#region Migrate
// Migration'ları programatik olarak runtime'da migrate etmek için kullanılan bir fonksiyondur.
// ne kadar migration var ise hepsini migrate eder.
// EnsuredCreated fonksiyonu migration'ları kapsamıyacaktır. O yüzden migration'ların içinde yaptığımız çalışmalar bu fonksiyonda işe yaramazken Migrate fonksiyonunda işe yarar.
//context.Database.Migrate();
#endregion
#region OpenConnection
// veritabanı bağlantısını manuel açar
//context.Database.OpenConnection();
#endregion
#region CloseConnection
// veritabanı bağlantısını manuel kapatır.
//context.Database.CloseConnection();
#endregion
#region GetConnectionString
// İlgili context nesnesinin o anda kullandığı connectionstring değeri ne ise onu elde etmemizi sağlar.
//Console.WriteLine(context.Database.GetConnectionString());
#endregion
#region GetDbConnection
// Ef Cor'un kullanmış olduğu Ado.Net altyapısnın kullandığı DbConnection nesnesini elde etmemizi sağlayan bir fonksiyondur yani bizleri Ado Net kanadına götürür. : ef coru aradan çıkartıp Ado Net üzerinden db işlemleri için.
//SqlConnection connection = (SqlConnection)context.Database.GetDbConnection();
//Console.WriteLine();
#endregion
#region SetDbConnection
// Özelleştirilmiş connection nesnelerini ef core mimarisine dahil etmemizi sağlayan bir fonksiyondur
//context.Database.SetDbConnection();
#endregion
#region ProviderName Property'si
// EF Corun kullanmış olduğu provider neyse onun bilgisini getiren bir propery'dir : Database bağlantı kütüphanesi.
//Console.WriteLine(context.Database.ProviderName);
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

class AdoNetDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=AdoNetDb;Trusted_Connection=True;TrustServerCertificate=True");
    }

}