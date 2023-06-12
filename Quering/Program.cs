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

int productId = 5;
var products = from product in context.Products
               where product.Id > productId
               select product;

productId = 200;
//geciktirilmiş sorgu(yürütme) : buraya kadar sorguyu göndermiyordu. : çünkü daha çağrılmamıştı : diferred execution(ertelenmiş çalışma)
#region Foreach
foreach (Product product in products) 
{
    Console.WriteLine(product.ProductName);
}

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
#endregion
#region Tekil veri getiren sorgulama fonksiyonları
#endregion
#region Diger sorgulama fonksiyonları
#endregion
#region Sorgu sonucu dönüşüm fonksiyonları

#endregion

public class QueringClass : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Part> Parts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;TrustServerCertificate=True");
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