// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;


QueringClass context = new();

#region En temel basit bir sorgulama nasıl yapılır?
#region Method Syntex 
// Link fonksiyonları
//var products = await context.Products.ToListAsync();
#endregion
#region Query Syntax
// Link Query

//var products2 = await (from product in context.Products select product).ToListAsync();
#endregion
#endregion
#region Sorguları Execute etmek için ne yapmamız gerekmektedir ?

#region ToListAsync
#endregion

/*
int productId = 5;
var products = from product in context.Products
               where product.Id > productId
               select product;

productId = 200;

//geciktirilmiş sorgu(yürütme) : buraya kadar sorguyu göndermiyordu. : çünkü daha çağrılmamıştı : diferred execution(ertelenmiş çalışma)
*/
#region Foreach
/*
foreach (Product product in products) 
{
    Console.WriteLine(product.ProductName);
}
*/

#region Deferred Execution(Ertelenmiş çalışma)
/*
   - IQueryable çalışmalarında ilgili kod yazıldığı noktada tetiklenemez/çalıştırılmaz yani ilgili kod yazıldığı noktada soruguyu generate etmez! Nerede eder? Çalıştırıldığı/execute edildiği noktada tetiklenir! işte bu durumda ertelenmiş çalışma denir!
   - ertelenmiş çalışmanın mantığı : veriler Linq ile sorgulanduğında değilde : ilgili veriler kullanılacağı zaman çekilir. => kullanma mantığıda data'yı Enumarable'ye çevirme anı veya for each içinde kullanma anıdır.
*/
#endregion
#endregion
#endregion
#region IQueryable ve IEnumerable Nedir? Basit olarak!

//var products = await (from product in context.Products select product).ToListAsync(); // ToListAsync => ıquaryable sorgusundaki verileri almak için
#region IQueryable
/*
    Sorguya karşılık gelir
    EF Core üzerinden yapılmış olan sorgunun execute edilmemiş halini ifade eder
*/
#endregion
#region IEnumerable
/*
    Sorgunun çalıştırılıp/execute edilip verilerin in memoruye yüklenmiş halini efade eder.
*/
#endregion
#endregion
#region Çoğul veri getiren sorgulama fonksiyonları

#region ToListAsync
// Üretilen sorguyu execute ettirmemizi saglayan fonksiyondur => IQueryable -> IEnumarable çevirme işlemi
// toList , ...
#endregion
#region Where
// oluşturulan sorguya where şartı eklememizi sağlıyan bir fonksiyondur.
//var urunler = context.Products.Where(f => f.ProductName.EndsWith("..."));
#endregion

#endregion
#region Tekil veri getiren sorgulama fonksiyonları
// Yapılan sorguda sade ve sadece tek bir verinin gelmesi amaçlanıyor ise Single ya da SingleOrDefault fonksiyonları kullanılabilir.
#region SingleAsync
// Eğer ki, sorgu neticesinde birden fazla veri geliyorsa ya da hiç gelmiyorsa her iki durumda da exception fırlatılır.
#region Tek kayıt geldiginde
// Eğer ki sorgu neticesinde birden fazla veri geliyor ise exception fırlatır, hiç veri gelmiyor ise null döner.
/*
var product = await context.Products.SingleAsync(f => f.ProductName == "ProductB");
Console.WriteLine();
*/
#endregion
#region Hiç kayıt gelmediğinde
// var product = await context.Products.SingleAsync(f => f.ProductName == "ProductK"); // bu şartı saglayan veri olmadığından patlıyor proje burada.
#endregion
#region Çok kayıt geldiğinde
//var product = await context.Products.SingleAsync(f => f.ProductName == "ProductA"); //birden fazla veri geldiği için hata verir.
#endregion
#endregion

#region SingleOrDefaultAsync
// Eğer ki sorgu neticesinde birden fazla veri geliyor ise exception fırlatır, hiç veri gelmiyor ise null döner.
#region Tek kayıt geldiginde
/*
var product = await context.Products.SingleOrDefaultAsync(f => f.ProductName == "ProductB");
Console.WriteLine();
*/
#endregion
#region Hiç kayıt gelmediğinde
/*
 var product = await context.Products.SingleOrDefaultAsync(f => f.ProductName == "ProductK"); // bu şartı saglayan veri olmadığında null döndürür.
Console.WriteLine();
*/
#endregion
#region Çok kayıt geldiğinde
//var product = await context.Products.SingleOrDefaultAsync(f => f.ProductName == "ProductA"); //birden fazla veri geldiği için hata verir.
#endregion
#endregion

//Not : eğerki ben projemde ilgili özellikten id mesela veritabanında 1 tane olması gerekiyor ve ben buna göre 1 tane veri çekmek istiyor isem SingleAsync - SingleOrDefaultAsync methodları kullanılmalı ; ilgili şartı sağlayan birden fazla veri gelebilir ama ben bunlardan sadece ilkini almak istiyorum der isem de First - FirstOrDefault methotlarını kullanabilirim.
#region First - FirstOrDefault
// gelen veri birden fazla iseve kaç adet olursa olsun sadece 1 tanesini döndürür : verilerden.
#endregion

#region FindAsync
// önce in memory'e bakar eğerki ilgili veriyi bulamaz ise db'ye gidip sorgulama yapıyor : bu sadece findasync'e özel.
//Product product = await context.Products.FirstOrDefaultAsync(f => f.Id == 3);

// FindAsync: primary key column'una özel arama yapmamızı sağlayan bir column'dur. üstteki gibi lamda ile yazmamıza gerek yok.
//Product product = await context.Products.FindAsync(3);
//Console.WriteLine();

#region Composite Primary key durumu => birden fazla primary key var ise

/*
ProductPart productPart = await context.ProductParts.FindAsync(1, 2); // birden fazla primary key'i olan tablolar için. FindAsync
Console.WriteLine();
*/
#endregion

#endregion

#region LastAsync
// son veriyi çekmek için => orderby gereklidir: yoksa hata verir. => ordy by veya  OrderByDescending 'e göre veriyi sıralar sonrasında listenin en arkasında kalan veriyi alır => yani hangi verinin geleceğinde terstenmi, düzmü sıraladığımız önemlidir. => en sonuncu veriyi getirilir.
// LastAsync => hiç veri gelmiyor ise hata fırlatır , LastOrDefaultAsync => veri gelmez ise null döner
/*
var products = await context.Products.OrderByDescending(f => f.ProductName).LastOrDefaultAsync();
//var products = context.Products.OrderBy(f => f.ProductName).LastAsync(f => f.Id > 4); //şeklinde şart'da verebiliriz.
Console.WriteLine();
*/
#endregion

#region LastOrDefaultAsync

#endregion

#endregion
#region Diger sorgulama fonksiyonları
#region LongCountAsync
/*
//olusturulan sorgunun exute edilmesinde neticesinde kaç adet satırın elde edileceğini sayısal olark(long) bizlere bildiren fonksiyondur.
// int deger aralıgı 2 minyol .. bişeydi daha fazla deger var ise count'unu bu şekilde çekebiliriz.
var product = await context.Products.LongCountAsync();
Console.WriteLine();
*/
#endregion

#endregion
#region Sorgu sonucu dönüşüm fonksiyonları
// Bu fonksiyonlar ile sorgu neticesinde elde edilen verileri isteğimiz doğrultusunda farklı türlerde projecsiyon edebiliyoruz.
#region ToDictionaryAsync
/*
// veritabanından elde ettiğimz verileri dictionary olarak elde etmek istediğimzde kullanırız.
// TOList : gelen verielri liste formatında tutacaktır ; dictinory : ToDictionary olarak tutacaktır
var product =  context.Products.GroupBy(f => f.ProductName).ToDictionary(f=> f.Key, f=> f.First().Price);//.ToDictionaryAsync(f => f.ProductName, f => f.Price);
var product2 =  context.Products.GroupBy(f => f.ProductName).ToDictionary(f=> f.Key, f=> f.Last().Price);//.ToDictionaryAsync(f => f.ProductName, f => f.Price);
// yukarıda : groupby ile kullandım bunun sebibi => toDictionary key alanına verdiğim degerin tekrar yapmasından dolayı dictinory'in key alanına aynı ismi 2 kez atıyamazsın hatası veriyor buradada o yüzden bu şekilde 1 column'un 1 kere gelmesini saglıyoruz ve ardından : tekrar yapan row'da first veya last ile : ilk veya son tekrar yapan verinin bilgilerine erişebiliyoruz.
var product3 = context.Products.ToDictionary(f => f.Id, f => f.ProductName);
Console.WriteLine();
*/
#endregion
#region ToArrayAsync
/*
// olusturulan sorguyu dizi olarak elde eder
// ToList ile muadil amaca hizmet eder. Yani sorguyu execute eder lakin gelen sonucu entity dizi olarak elde ederiz
var products = await context.Products.ToArrayAsync();
Console.WriteLine("");
*/
#endregion

#region Select
// Select fonksiyonunun işlevsel olarak birden fazla davranışı söz konusudur,
/*
// 1. Select fonksiyonu, generate edilecek sorgunun çekilecek kolonlarını ayarlamamızı sağlamaktadır.
// model'de sadece verdiğimiz alanları doldurur. modeldeki diğer alanalara null değeri atanır.
var product = context.Products.Select(f => new Product 
{
    Id = f.Id,
    ProductName = f.ProductName
}).ToList();
Console.WriteLine();
*/
/*
//2. Select fonksiyonu, gelen verileri farklı türlerde karşılamamızı sağlar. T,anonim
//anonim olarak atama yapabiliriz. => bir modele eşlemeden direk anonim bir model oluşturup atama => performans olarak iyi
var products = await context.Products.Select(f => new
{
    Id = f.Id,
    Name = f.ProductName
}).ToListAsync();
Console.WriteLine("");
*/
/*
//3. Select fonksiyonu, harici bir modele eşitleyebilirim gelen verileri.
var products = await context.Products.Select(p => new ProductDetail
{
    Id= p.Id,
    Price = p.Price,
}).ToListAsync();
Console.WriteLine("");
*/
#endregion
#region SelectMany
/*
// select ile aynı amaca hizmet eder lakin, ilişkisel tablolar neticesinde gelen koleksiyonel verileri de tekilleştirip projeksion etmemizi sağlar.

//p.Parts diye => product tablosu ile ilişkili olan Part entity'e erişim saglıyoruz => p.Parts deme sebebimiz 2 tablo arasıbda ikişki verir iken product tablosundan Parts ismi ile ilgili tabloyu ilişkilendirmemiz.
// p => Product entites'ini temsil eder; x => Part entites'ini temsil eder.
var products = await context.Products.Include(p => p.Parts).SelectMany(p => p.Parts, (p, x) => new
{
    p.Id,
    p.Price,
    x.PartName
}).ToListAsync();
Console.WriteLine("");
*/
#endregion
#endregion

public class QueringClass : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<ProductPart> ProductParts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;TrustServerCertificate=True");
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

public class ProductDetail
{
    public int Id { get; set; }
    public float Price { get; set; }
}