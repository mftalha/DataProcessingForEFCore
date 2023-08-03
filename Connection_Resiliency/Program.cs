// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

//{resiliency : dayanıklılık, esneklik}
Console.WriteLine("Hello, World!");
ConnectionResiliencyDbContext context = new();

#region Connection Resiliency Nedir?
// EF Core üzerinde yapılan veritabanı çalışmaları sürecinde ister istemez veritabanı yapısında kopuşlar/kesintiler vs. meydana gelebilmektedir.

// Connectionm resiliency ile kopan bağlantıyı tekrar kurmak için gerekli tekrar bağlantı taleplerinde bulunabilir ve biryandan da execution strategy dediğimiz davranış modellleri belirleyerek bağlantıların kopması durumunda tekrar edecek olan sorguları baştan sona yeniden tetikleyebiliriz.
#endregion
#region EnableRetryOnFailure
//Uygulama sürecinde veritabanı bağlantısı koptuğu taktirde bu yapılandırma sayesinde bağlanytıyı tekrardan kurmaya çalışıyoruz.

//while (true)
//{
//    await Task.Delay(2000);
//    var persons = await context.Persons.ToListAsync();
//    persons.ForEach(p => Console.WriteLine(p.Name));
//    Console.WriteLine("**************");
//}

#region MaxRetryCount
// Yeniden bağlantı sağlanması durumunun kaç ker gerçekleştirileceğini bildirmektedir.(kaç kere bağlantı denesin)
// Default değeri 6'dır
#endregion
#region MaxRetryDetal
// yenmiden bağlantı sağlması periyodunu belirtmektedir.(kaç saniyede bir bağlantı denensin)
// default değeri 30 dur
#endregion
#endregion

#region Execution Strategies
// EF Core ile yapılan bir işlem sürecinde veritabanı bağlantısı koptuğu tekrarda yeniden bağlantı denenirken yapılan davranışa/alınan aksiyona Excution Strategies denir.

//Bu strategy'i default değerlerde kullanabileceğimiz gibi custom olarak da kendimize göre özelleştirebilir ve bağlantı koptuğu durumlarda istediğimiz aksiyonları alabiliriz.

#region Default Execution Strategy
// Eğerki Connection Resiliency için EnableRetryOnFailure metodunu kullanıyorsak bu default execution stragy karşılık gelecektir.
// MaxRetryCount : 6
// Delay: 30
// Default değerlerin kullanılabilmesi içi EnableRetryOnFailure metodunun paremetresiz overload'ının kullanılması gerekmektedir.
//

#endregion
#region Custom Execution Strategy

#region Oluşturma

#endregion
#region Kullanma - ExecutionStrategy

//while (true)
//{
//    await Task.Delay(2000);
//    var persons = await context.Persons.ToListAsync();
//    persons.ForEach(p => Console.WriteLine(p.Name));
//    Console.WriteLine("**************");
//}

#endregion

#endregion
#region Bağlantı koptuğu anda Execute edilmesi gereken tüm çalışmalar tekrar işlemek
// EF Core ile yapılan çalışma sürecince veritabnı bağlantısının kesildiği durumlarda bazen bağlantının tekrardan kurulması tek başına yetmemekte : kesintinin olduğu çalışmanın da baştan tekrardan işlenmesi gerekmektedir. İşte bu tarz durumlara karşılık ef core Execute - ExecuteAsync fonksiyonlarını bizlere sunmaktadır.

// Execute fonksiyonu, içerisine vermiş olduğumuz kodları commit edilene kadar işleyecektir. Eğer ki bağlantı kesilmesi meydana gelirse, bağlantının tekrardan kurulması durumunda Execute içerisindeki çalışmalar tekrar baştan işlenecek ve böylece yapılan işlemin tutarlılığı için gerekli çalışma sağlanmış olacaktır.

//var strategy = context.Database.CreateExecutionStrategy();
//await strategy.ExecuteAsync(async () =>
//{
//    using var transaction = await context.Database.BeginTransactionAsync();
//    await context.Persons.AddAsync(new() { Name = "Furkan" });
//    await context.SaveChangesAsync();

//    await context.Persons.AddAsync(new() { Name = "Kasım" });
//    await context.SaveChangesAsync();

//    await transaction.CommitAsync(); // commitleme işlemi için.
//});

#endregion
#region Execution Strategy hangi durumlarda kullanılır?
// Veritabanın şifresi belirli periyodlar ile otomatik olarak değişen uygulamalarda  güncel şifreyle connection string'i sağlayacak bir operasyonu custom execution strategy belirleyerek gerçekleştirebiliriz.
#endregion
#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
}

class ConnectionResiliencyDbContext : DbContext
{
    public DbSet<Person> Persons { get; set;}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ConnectionResiliencyDb;User ID=SA;Password=123!;TrustServerCertificate=True");



        #region Default Execution Strategy
        //Bu işlemeleri deniyebilmek için sql configuration'da sql server'ın pause durumuna getirilmesi gerekiyor => sorun çıkabiliyor arada : paus durumundayken başlatıp restrat yapınca işlemi görebiliyoruz.

        //builder => builder.EnableRetryOnFailure() == 30 saniyede bir 6 kez bağlantıyı tekrar kurmaya çalış => direk bağlantı kopar kopmaz işlem yapan veriyi koparma projede hata fırtlarma.
        //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ConnectionResiliencyDb;User ID=SA;Password=123!;TrustServerCertificate=True", builder => builder.EnableRetryOnFailure(
        //    maxRetryCount: 5,
        //    maxRetryDelay: TimeSpan.FromSeconds(15), //burda maksimum 15 saniyede bir işleyecek : daha erkende olabilir
        //    errorNumbersToAdd: new[] { 4060 }
        //    ))
        //    .LogTo(
        //    filter: (eventId, level) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
        //    logger: eventData =>
        //    {
        //        Console.WriteLine($"Bağlantı tekrar kurulmaktadır.");
        //    });
        #endregion

        #region Custom Execution Strategy
        // Bu işlemeleri deniyebilmek için sql configuration'da sql server'a pause duruma veya stop durumuna getirilmesi gerekiyor

        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ConnectionResiliencyDb;User ID=SA;Password=123!;TrustServerCertificate=True", builder => builder.ExecutionStrategy(dependencies => new CustomExecutionStrategy(dependencies, 10, TimeSpan.FromSeconds(15))));
        //burda maksimum 15 saniyede bir işleyecek : daha erkende olabilir
        #endregion

    }
}

#region Execution Strategies

class CustomExecutionStrategy : ExecutionStrategy
{
    public CustomExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) : base(context, maxRetryCount, maxRetryDelay)
    {
    }

    public CustomExecutionStrategy(ExecutionStrategyDependencies dependencies, int maxRetryCount, TimeSpan maxRetryDelay) : base(dependencies, maxRetryCount, maxRetryDelay)
    {
    }

    int retryCount = 0;
    protected override bool ShouldRetryOn(Exception exception)
    {
        //Yeniden bağlantı durumunun söz konusu olduğu anlarda yapılacak işlemler
        Console.WriteLine($"Bağlantı tekrar kuruluyor....");
        return true;
    }
}

#endregion
