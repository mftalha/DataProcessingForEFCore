// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
// SelectSorgularınıGüçlendirmeTeknikleri
// {Efficient : verimli}
EfficientQueryDbContext context = new();

#region EF Core Select sorgularını güçlendirme teknikleri

#region IQuerable - IEnumerable farkları

//IQuerable, Bu arayüz üzerinde yapılan işlemler direkt generate edilecek olan sorguya yansıtılacaktır.
//IEnumerable, Bu arayüz üzerinde yapılan işlemler temel sorgu neticesinde gelen ve in-memory'e yüklenen instanceler üzerinde gerçekleştirilir. Yani sorguya yansıtılmaz.


// IQuerable ile yapılan sorgulama çalışmalarında sql surgusu hedef verileri elde edecek şekilde generate edilecekken, IEnumerable ile ypılan sorgulama çalışmalarında sql daha geniş verielri getirebilecekk şekilde execute edilerek hedef verilerin in-memory'de ayıklar.

// IQuerable hedef verileri getirirken, IEnumerable hedef verilerden daha fazlasını getirip in-memory2de ayıklar.

// IQuerable ve IEnumerable davranışsal olarak aralarında farklılıkllar barındırsada her ikiside Deffered Execution(geciktirmeli çalışma) davranışı sergiler.
// yani her iki arayüz üzerindende oluşturulan işlemi execute edebilmek için .toList gibi tetikleyici fonksiyonları yahut foreach gibi tetikleyici işlemelri gerçekleştirmemiz gerekmektedir.

#region IQuerable

//var persons = await context.Persons
//    .Where(p => p.Name.Contains("a"))
//    .Take(3)
//    .ToListAsync();

//exec sp_executesql N'SELECT TOP(@__p_0) [p].[PersonId], [p].[Name]
//FROM [Persons] AS[p]
//WHERE[p].[Name] LIKE N''%a%''',N'@__p_0 int',@__p_0=3

//--------

//var persons = await context.Persons
//    .Where(p => p.Name.Contains("a"))
//    .Where(p => p.PersonId > 3)
//    .Take(3)
//    .Skip(3)
//    .ToListAsync();

#endregion
#region IEnumerable
//var persons = context.Persons
//    .Where(p => p.Name.Contains("a"))
//    .AsEnumerable()
//    .Take(3)
//    .ToList();

// take sorgusunu veritabanına atmaz : in- memory'de yapar take işlemini
//SELECT[p].[PersonId], [p].[Name]
//FROM[Persons] AS[p]
//WHERE[p].[Name] LIKE N'%a%'

#endregion

#region AsQueryable

#endregion
#region AsEnumarable

#endregion

#endregion

#region Yanlızca İhtiyaç olan kolonları Listeleyin - Select
//var persons = context.Persons.Select(P => new
//{
//    P.Name
//}).ToListAsync();
#endregion

#region Result'ı limitleyin - Take
// tüm verileri direkt çekmeyip parça parça çekmek => sayfada gösterileceği kadar +  + : skip take
//context.Persons.Take(50).ToListAsync();
#endregion

#region Join sorgularında Eager Loading sürecinde verileri filtreleyin
//var persons = await context.Persons.Include(p => p.Orders)
//    .ToListAsync();

//foreach (var person in persons)
//{
//    var orders = person.Orders.Where(o => o.OrderId > 5);
//}

// yukarıdaki yerine aşşağıdaki gibi yazmalıyız.
//var persons = await context.Persons.Include(p => p.Orders
//                                            .Where(o => o.PersonId > 5 )
//                                            .Take(3))
//    .ToListAsync();
#endregion

#region Şartlara bağlı join yapılacaksa eğer explict loading kullanınlmalı
//var person = await context.Persons.Include(p => p.Orders).FirstOrDefaultAsync(p => p.PersonId == 1);

//if(person.Name == "Ayşe")
//{
//    // Order'ları getir...
//}

// yukarıdaki yerine => eğer ilk entity şartı sağlıyorsa ilk entity'e ilişkiel ilgili tabloyu ekle dioruz.

//var person = await context.Persons.FirstOrDefaultAsync(p => p.PersonId == 1);
//if (person.Name == "Ayşe")
//{
//    context.Entry(person).Collection(p => p.Orders).LoadAsync();
//}
#endregion

#region Lazy loading kullanırken dikkatli olun!
#region Riskli durum!
// database bağlantına ekliyoruz => .UseLazyLoadingProxy.. gibi => ilgili derste mevcut
//var persons = await context.Persons.ToListAsync();

//foreach(var person in persons)
//{
//    foreach(var order in person.Orders)
//    {
//        Console.WriteLine($"{person.Name} - {order.OrderId}");
//    }
//    Console.WriteLine("*************");
//}
// üstteki durumda her foreach'de tekrardan join sorgusu atma gibi bir mantık oluyor db'ye => çok maliyetli oluyor.
#endregion
#region İdeal Durum
// bu şekilde yaptığımda 1 kere çekecektir ve tekrar tekrar join sorgusunu db'ye atmıyacaktır.
//var persons = await context.Persons.Select(p => new {p.Name, p.Orders}).ToListAsync();

//foreach (var person in persons)
//{
//    foreach (var order in person.Orders)
//    {
//        Console.WriteLine($"{person.Name} - {order.OrderId}");
//    }
//    Console.WriteLine("*************");
//}
#endregion

#endregion

#region İhtiyaç noktalarında ham sql kullanın - FromSql
// karmaşık linq sorguları olabilir veya store procedure - view lerde kullanılabilir.
#endregion

#region Asenkron Fonksiyonları tercih edin
// Veri tabanı tarafında olmasada proje tarafında hızlandıracaktır.
//context.Persons.ToListAsync
#endregion

#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Order> Orders { get; set; }

}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public virtual Person Person { get; set; }
}

class EfficientQueryDbContext : DbContext
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
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=EfficientQueryDb;User Id=SA;Password=123!;TrustServerCertificate=True");
    }

}