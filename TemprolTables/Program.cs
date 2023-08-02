// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

// { temporal : geçici}
Console.WriteLine("Hello, World!");
TemporalTableDbContext context = new();

#region Temprol Tables Nedir?
// Veri değişikliği süreçlerinde kayıtları depolayan ve zaman içerisinde farklı noktalardaki tablo verilerinin analizi için kullanılan ve sistem tarafından yönetilen tablolardır.
// EF Core 6.0 ile desteklenmektedir.
#endregion
#region Temprol Tables Özelliğiyle Nasıl çalışılır ?
// EF Core'daki migration yapıları sayesinde tampral table'lar oluşturulp veritabanında üretilebilmektedr.
// Mvcut tabloları migrationlar aracalığıyla temporal table'lara dönüştürülebilmektedir.
// Herhangi bir tablonun verisel olarak geçmişini rahatlıkla sorgulayabiliriz.
// Herhangi bir tablodaki bir verinin geçmişteki herhangi bir T anındaki hali/durumu/verielri getirilebilmektedir.
#endregion
#region Temprol Table Nasıl Uygulanır

#region IsTemoral Yapılandırması
// EF Core bu yapılandırma fonsiyonu sayesinde ilgili entity'e karşılık üretilecek tabloda temprol table oluşturacağını anlamaktadır. Yahut eğer önceden ilgili tablo üretilmisse eğer onu temprol table'a dönüştürecektir.
#endregion
#region Temprol Table için Üretilen Migration'ın incelenmesi

#endregion
#endregion

#region Temporal Table'ı test edilmesi

#region Veri Eklerken
// Tempral Table'a veri ekleme süreçlerinde herhangi bir kayıt atılmaz! Temprol Table'ın yapısı, var olan veriler üzerndek,i zamansal değişimleri takip etmek üzeredir.

//var person = new List<Person>()
//{
//      new Person() { Name = "Talha", Surname= "Satır", BirtDate= DateTime.Now },
//      new Person() { Name = "Ahmet", Surname= "Satır", BirtDate= DateTime.Now },
//      new Person() { Name = "Mehmet", Surname= "Satır", BirtDate= DateTime.Now },
//      new Person() { Name = "Sinan", Surname= "Satır", BirtDate= DateTime.Now },
//      new Person() { Name = "Kerem", Surname= "Satır", BirtDate= DateTime.Now }
//};
//await context.Persons.AddRangeAsync(person);
//context.SaveChanges();

#endregion
#region Veri Güncellerken
//var data = await context.Persons.FindAsync(3);
//data.Name = "Deniz";
//context.SaveChanges();
#endregion
#region Veri Silerken
//var person = await context.Persons.FindAsync(3);
//context.Persons.Remove(person);
//await context.SaveChangesAsync();   
#endregion
#endregion

#region Temporal Table üzerinde geçmiş verisel izleri sorgulama

#region TemporalAsOf
// Belirli bir zaman için değişikliğe uğrayan tüm öğeleri döndüren bir fonksiyondur.

// şuanın tarihinin girersem şuanki gerçek tabloda olan verileri getirir => silinmemiş ve son güncel hallerinin veya eklenen verilerin tutulduğu tablo : asıl tablodaki veriler.
//var datas = await context.Persons.TemporalAsOf(DateTime.UtcNow).Select(p => new {
// şuanki aktif tablodaki verileri getirir ve history tablosundki verdiğim tarih(end column) saniye deki veriyi getirir
//var datas = await context.Persons.TemporalAsOf(new DateTime(2023, 08, 02, 10, 23,44)).Select(p => new {
//    p.Id,
//    p.Name,
//    PeriondStart = EF.Property<DateTime>(p, "PeriodStart"),
//    PeriondEnd = EF.Property<DateTime>(p, "PeriodEnd")
//}).ToListAsync();

//foreach(var data in datas)
//{
//    Console.WriteLine($"{data.Id} - {data.Name} | {data.PeriondStart} - {data.PeriondEnd}");
//}

#endregion
#region TemporalAll
//Güncellenmiş yahut silinmiş tüm verilerin geçmiş sürümlerini veya geçerli durumnlarını döndüren fonksiyondur.

// başta fiziksel tablodaki veriler gelecek : sonra history tablosundaki verilerde altta gelecek.
//var datas = await context.Persons.TemporalAll().Select(p => new {
//    p.Id,
//    p.Name,
//    PeriondStart = EF.Property<DateTime>(p, "PeriodStart"),
//    PeriondEnd = EF.Property<DateTime>(p, "PeriodEnd")
//}).ToListAsync();

//foreach (var data in datas)
//{
//    Console.WriteLine($"{data.Id} - {data.Name} | {data.PeriondStart} - {data.PeriondEnd}");
//}
#endregion
#region TemporalFromTo
// Belirli bir zaman aralığı içerisindeki verileri döndüren fonksiyondur. Başlangıç ve bitiş zamanı dahil değildir.

// staqrt date : 2023-08-02 10:21:30.8647859
//var start = new DateTime(2023, 08, 02, 10, 21, 30);

//// end date: 2023-08-02 10:25:50.3668227
//var end = new DateTime(2023, 08, 02, 10, 25, 50);

//// yien ilk fiziksel verileri getiri sonra verdiğimiz zaman aralığındaki hsitory verilerini getirir.
//var datas = await context.Persons.TemporalFromTo(start, end).Select(p => new {
//    p.Id,
//    p.Name,
//    PeriondStart = EF.Property<DateTime>(p, "PeriodStart"),
//    PeriondEnd = EF.Property<DateTime>(p, "PeriodEnd")
//}).ToListAsync();

//foreach (var data in datas)
//{
//    Console.WriteLine($"{data.Id} - {data.Name} | {data.PeriondStart} - {data.PeriondEnd}");
//}
#endregion
#region TemporalBetween
//// Belirli bir zaman aralığı içerisindeki verileri döndüren fonksiyondur. Başlangıç verisi dahil değil ve bitiş zamanı dahildir.

////staqrt date: 2023 - 08 - 02 10:21:30.8647859
//var start = new DateTime(2023, 08, 02, 10, 21, 30);

//// end date: 2023-08-02 10:25:50.3668227
//var end = new DateTime(2023, 08, 02, 10, 25, 50);

//var datas = await context.Persons.TemporalBetween(start, end).Select(p => new {
//    p.Id,
//    p.Name,
//    PeriondStart = EF.Property<DateTime>(p, "PeriodStart"),
//    PeriondEnd = EF.Property<DateTime>(p, "PeriodEnd")
//}).ToListAsync();

//foreach (var data in datas)
//{
//    Console.WriteLine($"{data.Id} - {data.Name} | {data.PeriondStart} - {data.PeriondEnd}");
//}
#endregion
#region TemporalContainedIn
//// Belirli bir zaman aralığı içerisindeki verileri döndüren fonksiyondur. Başlangıç ve bitiş zamanı dahildir.

////staqrt date: 2023 - 08 - 02 10:21:30.8647859
//var start = new DateTime(2023, 08, 02, 10, 21, 30);

//// end date: 2023-08-02 10:25:50.3668227
//var end = new DateTime(2023, 08, 02, 10, 25, 50);

//var datas = await context.Persons.TemporalContainedIn(start, end).Select(p => new {
//    p.Id,
//    p.Name,
//    PeriondStart = EF.Property<DateTime>(p, "PeriodStart"),
//    PeriondEnd = EF.Property<DateTime>(p, "PeriodEnd")
//}).ToListAsync();

//foreach (var data in datas)
//{
//    Console.WriteLine($"{data.Id} - {data.Name} | {data.PeriondStart} - {data.PeriondEnd}");
//}
#endregion
#endregion

#region Silinmiş bir veriyi Temprol Table'dan geri getirme
// Silinmiş bir veriyi tamporal table'dan getirmebilmek için öçncelikle yapılması gereken ilgili verinin silindiği tarihi bulmamız gerekmektedir. Ardından TemporalAsOf fonksiyonu ile silinen verinin geçmiş değeri elde edilebilir ve fiziksel tabloya bu veri taşınabilir.

// Silindiği tarih
var dateOfDelete = await context.Persons.TemporalAll()
    .Where(p => p.Id == 3)
    .OrderByDescending(p => EF.Property<DateTime>(p, "PeriodEnd"))
    .Select(p => EF.Property<DateTime>(p, "PeriodEnd"))
    .FirstAsync();

// TemporalAsOf ilgili tarihten sonrasına bakar o yüzden 1 mili saniye gerisine alıp ilgili veriyi takalıyacaz
var deletePeroson = await context.Persons
    .TemporalAsOf(dateOfDelete.AddMicroseconds(-1))
    .FirstOrDefaultAsync(p => p.Id ==3);

await context.AddAsync(deletePeroson);

await context.Database.OpenConnectionAsync(); // IDENTITY_INSERT özelliğini uygulayabilmek içni aşşağıda ilk başta bunu açmamız gerekiyor.
// veritabanında identity artış gösteren tablodaki identity olmadan araya girip pk alanına id vermemizi açar
await context.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT dbo.Persons ON");
await context.SaveChangesAsync();
// veritabanında identiy olarak artan pk alanına fiziksel veri atmayı attıysak bunu kapatmamız lazım işimiz bittiğinde. : yoksa veritabanından herkez veri ekleme de id atamayı otomatik atar.
await context.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT dbo.Persons OFF");

#region Set Identity_Insert konfigurasyonu
// Id ile  veri ekliyebilmek için ilgili verinin id sütunun kayıt işleyebilmek için veriyi fiziksel tabloya taşıma işleminden önce veritabanı seviyesinde SET IDENTITY_INSERT komutu eşl,iğinde ID Bazlı veri ekleme işlemi aktifleştirilmelidir çalıştırılmalıdır.
// await context.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT dbo.Persons ON");
#endregion
#endregion

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirtDate { get; set; }
}

class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set;}
}

class TemporalTableDbContext : DbContext 
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Employee> Employees { get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // , builder => builder.IsTemporal() => tablonun tamporal table oldugnu bildirmiş oluyoruz
        modelBuilder.Entity<Employee>().ToTable("Employees", builder => builder.IsTemporal());
        modelBuilder.Entity<Person>().ToTable("Persons", builder => builder.IsTemporal());
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TemporalTableDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}