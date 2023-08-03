// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
InMemoryDatabaseDbContext context = new();

// In-Memory database üzerinde çalışırken migrate etmeye gerek yoktur
// In-Memory'de oluşturulmış olan database uygulama sona erdiği/kapatıldığı takitirde bellekten silinecektir.
// Dolayısıyla özellikle gerçek uygulamalarda in-memory databas'i kullanıyorsak bunun kalıcı değil geçici yani silinebilir bir özellik olduğunu unutmamalıyız.

#region EF Core'da In-Memory Database ile çalışma gereği nedir?
// Genellikle bu özelliği yeni çıkan EF Core özelliklerini test edebilmek için kullanabiliriz. : veritabanında yeni db olusturmamak için
// EF Core, fiziksel veritabanlarından ziyade in-memory'de Database oluşturup üzerinde birçok işlem yapmamızı sağlayabilmektedir. İşte bu özellik ile gerçek uygulamaların dışında test gibi operasyonları hızlıca yürütebileceğimiz imkanlar elde edebilmekteyiz.
#endregion
#region Avantajları Nelerdir?
// Test ve pre-Prod uygulamalarda gerçek/fiziksel veritabanları olşuturmak ve yapılandırmak yerine tüm veritbanını bellekte modelleyebilir ve gerekli işlemleri sanki gerçek bir veritabanında çalışıyor gibi orada gerçekleştirebiriliz.
// Bellekte çalışmak geçici bir deneyim olacağı için veritabanı serverlarında test amaçlı üretilmiş olan veritbanlarının lüzumsuz yer işgal etmesini engellemiş olacaktır.
// Bellekte veritabanını modellemek kodun hızlı bir şekilde test edilmesini sağlıyacaktır.
#endregion
#region Dezavantajları Nelerdir?
// In-Memory'de yapılacak olan veritabanı işlevlerini ilişkisel modellemeler yapılamamaktadır. Bu durumdan dolayı veri tutarlılığı sekteye uğrayabilir ve istatiksel açıdan yanlış  sonuçlar elde edilebilir.
#endregion
#region Örnek Çalışma
// Microsoft.EntityFrameworkCore.InMemory kütüphanesi uygulamaya yüklenmelidir.

await context.Persons.AddAsync(new() { Name = "Talha", Surname = "Satır" });
//await context.SaveChangesAsync();

var persons = await context.Persons.ToListAsync();
Console.WriteLine();
#endregion

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}

class InMemoryDatabaseDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("exampleDatabase");
    }
}