// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using System.Transactions;

Console.WriteLine("Hello, World!");
TransactionDbContext context = new();

#region Transaction Nedir
// Transaction veritabanındaki kümilatif işlemleri atomik bir şekilde gerçekleştirmemizi sağlayan bir özelliktir.
// Bir transaction içerisindeki tüm işlemler commit edildiği taktirde veritabanına fiziksel oalrak yansıtılaaktır. Ya da Rollback edilirse tüm işlemler geri alınacak ve fiziksel olarak veritabanında herhangi bir verisel değişiklik durumu söz konusu olmıyacaktır.
// Transactionun genel amacı veritabnındaki tutarlılık durumunu korumaktır yada bir başka deyişle veritabanındaki tutarsılık durumlarına karşı önlem almaktır.
//
#endregion
#region Default Transaction Davranışı
// EF Core'da varsayılan olarak, yapılan tüm işlemler savechanges fonksiyonu ile veritbanına fiziksel olarak uygulanır.
// Çünkü savechanges default olarak bir transakctona sahiptir.
// Eğer ki bu süreçte bir problem/hata/başarısızlık durumu söz konusu olursa tüm işlemler geri alınır(rollback) ve işlemlerin hiçbiri veritabanına uygulanır.
// Böylece SaveChanges tüm işlemlerin ya tamamen başarılı olacağını ya da bir hata oluştursa veritabanının değiştirmeden işlemleri sonlandıracağını ifade etmektedir.
#endregion
#region Transaction Kontrolünü Manuel Sağlama

//IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(); //savechanges dediğimimzide artık veritabanına commit yapılşmıyacaktır biz : iradeli bir şekilde yapmamız gerekir.
// EF Core'da transaction kontrolünün iradeli bir şekikde manuel sağlamak yani elde etmek istiyorsak eğer BeginTransactionAsync fonksiyonu çağrılmalıdır.

//Person p = new() { Name = "Abuzer" };
//await context.Persons.AddAsync(p);
//await context.SaveChangesAsync();
//await transaction.CommitAsync(); // veritabanına işl,iyoruz

#endregion
#region Savepoints
//EF Core 5.0 sürümüyle gelmiştir.
// Savepoints veritabanı işlemleri sürecinde bir hata oluşursa veya başka bir nedenle yapılan işlemlerin geri alınması gerekiyorsa transaction içersinde dönüş yapılabilecek noktaları ifade eden bir özelliktir.

#region CreateSavepoint
// Transaction içersinde geri dönüş noktasıı oluşturmamızı sağlayan bir fonskyiondur
#endregion
#region RollBackToSavepoint
// Transaction içerisinde geri dönüş noktasına(Savepoint'e) rollback yapmamızı sağlayan fonksiyondur
#endregion

//IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
//Person p13 = await context.Persons.FindAsync(13);
//Person p11 = await context.Persons.FindAsync(11);
//context.Persons.RemoveRange(p13,p11);
//await context.SaveChangesAsync();

//transaction.CreateSavepointAsync("t1");
//Person P10 = await context.Persons.FindAsync(10);
//context.Persons.Remove(P10);
//await context.SaveChangesAsync();
//// transaction.RollbackAsync(); // bütün işlemleri geri alma
//await transaction.RollbackToSavepointAsync("t1"); // t1 noktasına kadar geri al işlemleri. : 10 id li veri silinmeyecektir.
//await transaction.CommitAsync();

// Savepoints özelliği bir transaction içerisinde istenildiği kadar kullanılabilir.

#endregion
#region Connection'ın Harici olarak ayarlanması
// 
#endregion
#region Connection ve Transaction'ı Paylaşma - UseTransaction

#endregion
#region TransactionScope
// veritabanı işlemelrini bir grup olarak yapmamızı sağlayan bir sınıftır.
// ADO.NET ile de kullanılabilir.

//using TransactionScope transactionScope = new();
//// Veritabanı işlemleri..
//// ..
//// ..
//transactionScope.Complete(); // Complate fonksiyonu yapılan veritabanı işlemelrinin commit edilmesini sağlar.
// Eğer ki rollback yapacaksak complete fonksiyonunun tetiklenmemesi yeterlidir.
// rollback : burda using bittiğinde olacaktır otomatik.

#region Complete

#endregion
#endregion

public class Person
{
    public int PersonId { get; set;}
    public string Name { get; set;}
    public ICollection<Order> Orders { get; set;}
}
public class Order
{
    public int OrderId { get; set;}
    public int PersonId { get; set;}
    public string Description { get; set;}
    public Person Person { get; set;}
}

class TransactionDbContext : DbContext
{
    public DbSet<Person> Persons { get; set;}
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TransactionDb;User Id=SA;Password=123!;TrustServerCertificate=True");
    }
}