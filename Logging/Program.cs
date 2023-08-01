// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Hello, World!");
LoggingDbContext context = new();

var datas = await context.Persons.ToListAsync();

// {Log : kayıt, günlük}
#region Neden Loglama yaparız ?
// Çalışan bir sistemin Runtime'da nasıl davranış gerçekleştirdiğini gözlemleyebilmek için log mekanizmalarından istifade ederiz.
#endregion
#region Neleri Loglarız?
// yapılan sorguların çalışma süreçlerindeki davranışları.
// Gerekirse hassas veriler üzerindede loglama işlemleri gerçekleştiriyoruz.
#endregion
#region Basit olarak loglama nasıl yapılır?
//Minimum yapılandırma gerektirmesi
// Herhangi bir nuget paketine ihtiyaç duyulmaksınız loglamanın yapılabilmesi.

#region Debug penceresine log nasıl atılır ?
//var datas = await context.Persons.ToListAsync();
#endregion
#region Bir Dosyaya log nasıl atılır?
// Normalde console yahut debug pencerelerine atılan loglar pek takip edilebilir nitelikte olmamaktadır.
// Logları kalıcı hale getirmek istediğim,iz durumlarda en basit haliyle bu logları harici bir dosyaya atmak isteyebiliriz.
#endregion

#endregion
#region Hassas verilerin loglanması - EnableSensitiveDataLogging
// Defaut olarak EF Core log mesajlarında herhangi bir verinin değerini içermemektedir. Bunun nedeni, gizlilik teşkil edebilecek verilerin loglama sürecinde yanlışlıklada olsa açığa çıkmamasıdır.
// Bazen alınan hatalardan verinin değerini bilmek hatayı debug edebilmek için oldukça yardımcı olabilmektedir. Bu durumda hassas verilerinde loglamasını sağlayabiliyoruz.
#endregion
#region Exception Ayrıntısını Loglama - EnableDetailedErrors
// error mesajlarının detaylandırılması için
#endregion
#region Log Levels
// LogLevel.Information => diyerek Information ve üstü seviydeki hataları görecez (debuf - info - warning - error gibi mantık var galiba.)
// EF Core dafault olarak Debug seviyesi ve üstündeki tüm davranışları loglar.
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

class LoggingDbContext : DbContext
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
    // uygulamanın debug klasörüne logs.txt yoksa oluşturur var ise üstüne yazar(append: true)
    StreamWriter _log = new("logs.txt" , append: true);
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LoggingDb;Trusted_Connection=True;TrustServerCertificate=True");

        #region Basit olarak loglama nasıl yapılır?
        #region Debug penceresine log nasıl atılır ?
        //optionsBuilder.LogTo(Console.WriteLine); // direk çalışan consola basar logları
        #endregion
        #region Output alanına log nasıl atılır ?
        //optionsBuilder.LogTo(message => Debug.WriteLine(message)); // output kısmına yazar logları
        #endregion
        #region Bir Dosyaya log nasıl atılır?
        //optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message));
        #endregion
        #endregion
        #region Hassas verilerin loglanması - EnableSensitiveDataLogging
        //optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message))
        //    .EnableSensitiveDataLogging();
        #endregion
        #region Exception Ayrıntısını Loglama - EnableDetailedErrors
        //optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message))
        //    .EnableSensitiveDataLogging()
        //    .EnableDetailedErrors();
        #endregion
        #region Log Levels
        optionsBuilder.LogTo(async message => await _log.WriteLineAsync(message),LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        #endregion

    }
    // stream işlemlerinde kullanılan dosya vs şeyleri işimiz bitince kapatmalyıız : yoksa diğer uygulamlar tarafından erişiminde hatalar olabiliyormuş.
    public override void Dispose()
    {
        base.Dispose();
        _log.Dispose();
    }

    public override async  ValueTask DisposeAsync()
    {
        await _log.DisposeAsync();
        await base.DisposeAsync();
    }

}