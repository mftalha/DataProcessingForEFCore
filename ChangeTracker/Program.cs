// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
ChangeTrackerContext context = new ChangeTrackerContext();

#region Change Tracking Neydi?
// Context nesnesi üzerinden gelen tün nesneler/veriler otomatik olarak bir takip mekanizması tarafından izlenirler. İşte bu takip mekanizmasına Change Tracker denir. Change Tracker ile nesneler üzerindeki değişiklerin işlemler takip edilerek netice itibarıyla bu işlemlerin fıtratına uygun sql sorgucukları generate edilir. İşte bu işleme'de Change Tracking denir.
#endregion

#region ChangeTracker Propertysi

// Takip edilen nesnelere erişebilmemizi sağlayan ve gerektiği taktirde işlemler gerçekleştirmemizi sağlayan bir propertydir.
// Contexr sınıfının base class'ı olan DbContext sınıfının bir member'ıdır.
/*
var products = await context.Products.ToListAsync();

products[3].Price = 123; //Update
context.Products.Remove(products[4]); // Delete
products[5].ProductName = "asdasd"; // Update

var datas = context.ChangeTracker.Entries(); //takip edilen tüm nesneleri getirir. ;; Entries() : methodunu çağırdığımızda'da DetectChanges() methodu gibi en güncel verileri çağırma işlemi gerçekleştirir arka planda. yani DetextChanges()'de yapar.
await context.SaveChangesAsync();
Console.WriteLine("");
*/
#region DetectChanges Methodu
// EF Core context nesnesi tarafından izlenen tüm nesnelerdeki değişiklikleri change tracker sayesinde takip edebilmkete ve nesnelerde olan verisel değişiklikler yakalanarak bunların anlık görüntülerini(snapshot)'ini oluşturabilir.
// yapılan değişikliklerin veritabanına gönderilmeden önce algınlandığından emin olmak gerekir. Savechanges fonksiyonu çağrıldığı anda nesneler EF Core tarafından otomatik kontrol edilirler.
// Ancak yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişikliklerin algılanmasında opsiyonel olarak gerçekleştirmek isteyebiliriz. İşte bunun için DetectChanges fonksiyonu kullanulabilir ve her ne kadar EF Core değişikleri otomatik algılıyor olsa da biz yine de irademizle kontrol'e zorlıyabiliriz.
/*
var products = await context.Products.FirstOrDefaultAsync(f => f.Id == 3);
products.Price = 145;
context.ChangeTracker.DetectChanges(); // kontrole zorluyoruz => değişikliği algılamaması durumuna karşılık => bazı durumlarda genelde async çalışmalarda EF Core'un kaçırdıgı durumlar olabiliyor : o zaman bu komutla kontrol'e zorlarız.
await context.SaveChangesAsync();
*/
#endregion

#region AutoDetectChangesEnabled Property'si
// ilgili methotlar(SaveChanges, Entries) tarafından DetectChanges methodunun otomatik olarak tetiklenmesinin configurasyonunu yapmamızı sağlayan proportydir.
// SaveChanges fonksiyonu tetiklendiğinde DetectChanges metodunu içerisinde default olarak çağırmaktadır. Bu durumda DetectChanges fonksiyonunun kullanımını irademizle yönetmek ve maliyet/performans optimizasyonu yapmak istediğimiz durumlarda AutoDetextChangesEnabled özelliğini kapatabiliriz.
#endregion

#region Entries Metodu
// Context'te ki Entry metodunun koleksiyonel versiyonudur.
// Changes Tracker mekanşizması tarafından izlenen her entity nesnesinin bilgisini EntityEntry türünden elde etmemizi sağlar ve belirli işlemler yapabilmemize olanak sağlar.
//Entries metodu, DetectChanges metodunu tetikler. Bu durum da tıpkı SaveChanges'de olduğu gibi bir maliyettir.
// Buradaki maliyetten kaçınmak için AutoDetextChangesEnabled özelliğine false değeri verilebilir.

/*
var products = await context.Products.ToListAsync();
products.FirstOrDefault(f => f.Id == 2).Price = 654; // update
context.Products.Remove(products.FirstOrDefault(f => f.Id == 6)); // Delete
products.FirstOrDefault(f => f.Id == 7).ProductName = "Test"; //update

context.ChangeTracker.Entries().ToList().ForEach(e =>
{
    if(e.State == EntityState.Unchanged)
    {
        // verilerde degisiklik yapılmadı ise buraya
    }
    else if(e.State == EntityState.Deleted)
    {
        // silinen verilerde buraya gir.
    }
});
*/
#endregion

#region AcceptAllChanges Metodu
// SaveChangesAsync() veya SaveChangesAsync(true) tetiklendiğinde EF Core herşeyin yolunda olduğunu varsayarak track ettiği verilerin takini keser 'AcceptAllChanges()' yeni değişikliklerin takip edilmesini bekler. Böyle bir durumda beklenmeyen bir durum/olası bir hata söz konusu olursa eğer EF Core takip ettiği nesneleri bırakacağı için bir düzeltme mevzu bahis olamacaktır.
// Haliyle bu durumda devreye SaveChanges(false) ve AcceptAllChanges metotları girecektir.
// SaveChanges(False), EF Cor'a gerekli veritabanı komutlarını yürütmesini söyler ancak gerektiğinde yeniden oynatılabilmesi için değişiklikleri beklemeye/ nesneleri takip etmeye devam eder. Taki AcceptAllChanges metodunu irademiz ile çağırana kadar.
// SaveChanges(false) ile işlemin başarılı olduğundan emin olursanız AcceptAllChanges metodu ile nesnelerden takibi kesebilirsiniz.
/*
var products = await context.Products.ToListAsync();
products.FirstOrDefault(f => f.Id == 2).Price = 654; // update
context.Products.Remove(products.FirstOrDefault(f => f.Id == 6)); // Delete
products.FirstOrDefault(f => f.Id == 7).ProductName = "Test"; //update

//await context.SaveChangesAsync(); // await context.SaveChangesAsync(true); // varsayılan olarak true paremetresi alır zaten ; AcceptAllChanges() methodunu otomatik çağırır.

await context.SaveChangesAsync(false);
context.ChangeTracker.AcceptAllChanges();
*/
#endregion

#region HasChanges Metodu
// Takip edilen nesneler arasında değişiklik yapılanların olıup olmadığı bilgisini verir.
// arka planda DetectChanges metodonu tetikler.
//var result = context.ChangeTracker.HasChanges(); // true , false döndürecektir değişiklik olup olmamasına göre
#endregion
#endregion

#region Entity States
// Entity nesnelerinin durumunu ifade eder

#region Detached {müstakil , bağımsız}
// Nesnelerin change tracker mekanizması tarafından takip edilmediğini ifade eder.
/*
Product product = new Product();
Console.WriteLine(context.Entry(product).State);
product.ProductName = "test";
await context.SaveChangesAsync();
*/
#endregion

#region Added
//Veritabanına eklenecek nesneyi ifade eder. Added henüz veritabanına işlenmeyen veriyi ifade eder. SaveChanges Fonksiyonu çağrıldığında insert sorgusu oluşturulacağı anlamına gelir.
/*
Product product = new Product() { ProductName = "Product 10", Price = 5};
Console.WriteLine(context.Entry(product).State); //Detached
await context.Products.AddAsync(product);
Console.WriteLine(context.Entry(product).State); //Added
await context.SaveChangesAsync();
product.Price = 9;
Console.WriteLine(context.Entry(product).State); //Modified
*/
#endregion

#region UnChanged
// Veritabanından sorgulandığından beri nesne üzerinde herhangi bir değişiklik yapılmadığını ifade eder. Sorgu neticesinde elde edilen tüm nesneler başlangıçta bu state değerindedir.
/*
var products = await context.Products.ToListAsync();
var data = context.ChangeTracker.Entries();
Console.WriteLine(  );
*/
#endregion

#region Modified
// Nesne üzerinde değişiklik/güncelleme yapıldığını igade eder. SaveChanges fonksiyonu çağrıldığında update sorgusu oluşturulacağı anlamına gelir.
// EF Core değişimlerde verileride kontrol eder o yüzden eğerki verinin ilgili alanında değişim olmadan veriyi güncelle der isek veya veritabanındaki değeride aynı ise : o zman state durumu değişmez(Unchanged kalır) ve veritabanına'da güncelle sorgusunda bulunmaz.
/*
var product = await context.Products.FirstOrDefaultAsync(f => f.Id == 1);
Console.WriteLine(context.Entry(product).State); // Unchanged
product.ProductName = "test1"; 
Console.WriteLine(context.Entry(product).State); // Modified
context.SaveChanges(false);
Console.WriteLine(context.Entry(product).State); // Unchanged // context.SaveChanges(false) yapar isek => burası Modified kalır => AcceptAllChanges()'i kapatıyoruz false vererek. ChangeTracker Propertysi içinde verili.
*/
#endregion

#region Deleted
// nesnenin silindiğini ifade eder. SaveChanges fonksiyonu çağrışdıüı anda delete sorgusu oluşturulacağı anlamına gelir.
/*
var product = await context.Products.FirstOrDefaultAsync(f => f.Id == 1);
Console.WriteLine(context.Entry(product).State); //Unchanged
context.Products.Remove(product);
Console.WriteLine(context.Entry(product).State); //Deleted
context.SaveChanges();
Console.WriteLine(context.Entry(product).State); //Detached
*/
#endregion
#endregion

#region Context Nesnesi Üzerinden Change Tracker
/*
var product = await context.Products.FirstOrDefaultAsync(f => f.Id ==2);
product.Price = 555;
product.ProductName = "Test2"; //Modified | Update
*/
#region Entry Metodu
#endregion

#region OriginalValues Property'si
// üstüne yazılmış veriyi değilde veritabanından çekilen değerini görmek istiyor isek ilgili verinin aşşağıda'ki gibi bir kullanım yapabiliriz.
/*
var price = context.Entry(product).OriginalValues.GetValue<float>(nameof(product.Price));
var productName = context.Entry(product).OriginalValues.GetValue<string>(nameof(product.ProductName));
Console.WriteLine();
*/
#endregion

#region CurrentValues Property'si
// ilgili modelin db'deki karşılığını değil'de bizim projede güncellediğimiz ama daha db'ye kaydetmediğimiz değeri görmek istediğimizde aşşağıdaki kullanımı yaparız => bu kullanım eğer'ki verinin üstüne bir yazma yapmamış isek'de db deki karşılığını getirir.
/*
var productName = context.Entry(product).CurrentValues.GetValue<string>(nameof(product.ProductName));
Console.WriteLine();
*/
#endregion

#region GetDatabaseValues Metodu
// db'deki ilgili verinin karşılığını => db'ye gidip tekrar getirir => base halini elde ederiz ilgili verinin.
/*
var _product = await context.Entry(product).GetDatabaseValuesAsync();
Console.WriteLine(  );
*/
#endregion

#endregion

public class ChangeTrackerContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<ProductPart> ProductParts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;TrustServerCertificate=True");
    }

    //context.SaveChanges() dediğimizde buraya girecektir.
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries();
        foreach(var entry in entries)
        {
            if(entry.State == EntityState.Added) //ekleme işlemlerinde buraya gir 
            {
                /*
                 * mesela ekleme işlemlerinde model'de doldurmadığımız alanlar vardır ve bunları otomatik burda doldurmak isteyebiliriz => tarih gibi => eticaret uygulamasında gerçekleştirdik.
                */
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { //composite primary key oldugunu belirtiyoruz : HasKey ile ileride anlatılacak.
        modelBuilder.Entity<ProductPart>().HasKey(up => new { up.ProductId, up.PartId });
    }
}

public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public float Price { get; set; }
    public ICollection<Part> Parts { get; set; }

}
public class Part
{
    public int Id { get; set; }
    public string PartName { get; set; }
}

public class ProductPart
{
    public int ProductId { get; set; }
    public int PartId { get; set; }
    public Product Product { get; set; }
    public Part Part { get; set; }
}
