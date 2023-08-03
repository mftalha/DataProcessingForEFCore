// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

//{ concurrency: eşzamanlılık }
Console.WriteLine("Hello, World!");
DataConcurrencyDbContext context = new();

#region Data Concurrency Nedir?
// Geliştirdiğimiz uygulamalarda ister istemez verisel olarak tutarsızlılklar meydana gelebilmektedir. Örneğin; birden fazla uygulamanın yahut client'in aynı veritabanı üzerinde eşzamanlı olarak çalıştığı durumlarda verisel anlamda uygulamadan uygulamaya yahut client'tan clienta tutarsılızklar meydana gelebilri.
// Data Concurrency kavramı, uygulamalardaki veri tutarsızlığı durumlarına karşılık yönetilebilirliği sağlayacak olan davranışları kapsayan bir kavramdır.
// bir uygulamada veri tutarsızlığının olması demek o uygulamayı kullanan kullanıcıları yanıltmak demektir.
// Veri tutarsılzığınıun olduğu uygulamalarda istatiksel olarak yanlış sonuçlar elde edilebilir.
#endregion
#region Stale & Dirty(Bayat, Kirli) Data Nedir
// Stale Data: Veri tutarsızlığına sebebiyet verebilecek güncellenmeiş yahut zamanı geçmiş verileri ifade etmektedir. Örneğin; Bir ürünün stok durumu sıfırlandığı halde arayüz üzerinde bunu ifade eden bir güncelleme söz konusu değilse işte bu stale data durumuna bir örnektir.
// Dirty Data: Veri tutarsızlığına sebebiyet verebilecek verinin hatalı yahut yanlış olduğunu ifade etmektedir. Örneğin; Adı 'ahmet' olan bir kullanıcının veritabanında 'Mehmet' olarak tutulması dirty data örneğidir.
#endregion
#region Last In Wins (Son Gelen Kazanır)
// Bir veri yapısında son yapılan aksiyona göre en güncel verinin en üstte bulunmasını/varlıpını korumasını ifade eden bir deyimsel terimdir.
#endregion
#region Pessimistic Lock (Kötümser Kilitleme)
// Bu yöntem tavsiye edilmiyor : optimistic lock kullanmak tavsiye ediliyor.
// Bir transaction sürecinde elde edilen veriler üzerinde farklı sorgularla değişiklik yapılmasını engellemek için ilgili verilerin kilitlenmesi(locking) sağlayarak değişikliğe karşı direnç oluşturulmasını ifade eden bir yöntemdir.

//Bu verilerin kilitlenmesi durumu ilgili transaction'ın commit ya da rollback edilmesi ile sınırlandırılır.

#region Deadlock Nedir?
// Kitlenmiş olan bir verinin veritabanı seviyesinde meydana gelen sistemsel bir hatadan dolayı kilidinin çözülememsi yahut döngüzel olarak kilitlenme durumunun meydana gelmesini ifade eden bir terimdir.

// Pessimistic Lock yönteminde deadlock durumu yaşamamız bir ihtimaldir. O yüzden değerlendirilmesi gereken ve iyi düşünülerek tercih edilmesi gereken bir yaklaşımdır pessimistic lock yaklaşımı.
#endregion
#region Kilitleme Çıkmazı - Ölüm Kilitlenmesi Nedir?

#endregion
#region WITH (XLOCK)
//var transaction = await context.Database.BeginTransactionAsync();
//var data = await context.Persons.FromSql($"Select * from Persons WITH (XLOCK) where PersonId = 5")
//    .ToListAsync();
//Console.WriteLine();
//transaction.Commit();
#endregion
#endregion
#region Optimistic Lock (İyimser Kilitleme)

// Bir verinin stale olup olmadığını anlamak için herhangi bir locking işlemi olmaksızın versiyon mantığında çalışmamızı sağlayan yaklaşımdır
// Optimistic lock yönteminde, Pessmistic lock'da olduğu gibi veriler üzerinde tutartsızlığa mahal olabilecek değişikliklere fiziksel olarak engllenememektedir. Yani veriler tutarsızlığı sağlayacak şekilde değiştirilebilir
// Amma velakin optimistic lock yaklaşımı ile bu veriler üzerindeki tutarsızlık durumunu takip edebilmek için versiyon bilgisini kullanıyoruz. Bunuda şöyle kullanuıyoruz;
// Her bir veriye karşılık bir versiyon üretiliyor. Bu bilgi ister metinsel istersekte sayısal olabilir. Bu versiyon bilgisi veri üzerinde yapılan her bir değişiklik neticesinde güncellenecektir. Dolayısıyla bu güncellemeyi daha kolay bir şekilde gerçekleştirebilmek için sayısal olmasını tercih ederiz.
// EF Core üzerinden verileri sorgularken ilgili verilerin versyion bilgilerinide in-memor'e alıyoruz. Ardından veri üzerinde bir değişiklik yapılırsa eğer bu inmemory'deki versiyon bilgisi ile veritabanındaki versiyon bilgisini karşılaştırıyoruz. Eğer ki bu karşılaştırma doğrulanıyorsa yapılan aksiyon geçerli olacaktır, yok eğer doğrulanmıyorsa demek ki veinin değeri değişmiş anlamoına gelecek yani bir tutarsızlık durumu olduğu anlaşılacaktır. işte bu durumda bir hata fırlatılacak ve aksiyon gerçekleştirilmeyecektir.

// EF Core optimistic lock yaklaşmı için genetiğinde yapısal bir özellik barındırmaktadır.

#region Property Based Configuration (ConcurrencyCheck Attribute)
// Verisel tutarlılığın kontrol edilmek istedindiği propertyler ConcurrencyCheck attribute'u ile işaretlenir. Bu işaretlenme neticesinde her bir entity'nin instance'ı için in-memory'de bir token değeri üretilecek. Üretilen bu token değeri alınan aksiyon sürteçlerinde ef core tarafından doğrulanacak ve eğer ki ehrhangi bir değişiklik yok ise aksiyon başarıyla sonlandırılmış olacaktır. Yok eğer transaction sürecinde ilgi veri üzerinde(ConcurrencyCheck attribute ile işaretlenmiş properylerde) herhangi bir değişiklik durumu söz konusu ise üretilen token'da değiştirilecek ve haliyle doğruluma sürecinde geçerli olmıyacağı için veri tutarsızlığı durumu anlaşılacak ve hata fırlatılacaktır.

//var person = await context.Persons.FindAsync(3);
//context.Entry(person).State = EntityState.Modified;
//await context.SaveChangesAsync(); // burada debug ile durdurup db'de 3 id'li verinin name'inde değişiklik yaparsak ve debug'u çalıştırırsak hata alacazğızdır : artık bu hatayı yakalayıp istediğimiz işelemi gerçekleştirebnilriiz : tutarsızlık durumlarında.

// data attribute [ConcurrencyCheck] ; fluent api = IsConcurrencyToken()
#endregion
#region RowVersion Column
// Bu yaklaşımda ise veritabanındaki her bir satıra karşılık versiyon bilgisi fiziksel olarak oluşturulmaktadır.

//var person = await context.Persons.FindAsync(3);
//context.Entry(person).State = EntityState.Modified;
//await context.SaveChangesAsync(); //burada debug ile durdurup db'de 3 id'li verinin name'inde değişiklik yaparsak ve debug'u çalıştırırsak hata alacazğızdır : artık bu hatayı yakalayıp istediğimiz işelemi gerçekleştirebnilriiz : tutarsızlık durumlarında. : timestamp kolonu bunu otomatik yapacaktır.
//// ilgili column'u db ye migrate etmeliyiz.
#endregion
#endregion

public class Person
{
    public int PersonId { get; set; }
    #region Property Based Configuration
    //[ConcurrencyCheck] // veritabanına gönderirken bunu vermemize gerek yok çünkü in-memory'de çalışıyor.
    //public string Name { get; set; }
    #endregion
    public string Name { get; set; }
    #region RowVersion Column
    //[Timestamp]
    public byte[] RowVersion { get; set; }
    #endregion

}

class DataConcurrencyDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region Property Based Configuration
        //modelBuilder.Entity<Person>().Property(p => p.Name).IsConcurrencyToken();// == [ConcurrencyCheck]
        #endregion

        #region RowVersion Column
        modelBuilder.Entity<Person>().Property(p => p.RowVersion).IsRowVersion(); //  [Timestamp]
        #endregion
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DataConcurrencyDb;User Id=SA;Password=123!;TrustServerCertificate=True");
    }
}