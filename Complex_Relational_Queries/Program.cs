// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
ComplexRelationalQuerieDbContext context = new();

#region Complext Query Operators

#region Join

#region Query Syntax
//inner join
//var query = from photo in context.Photos
//            join person in context.Persons
//                on photo.PersonId equals person.PersonId
//            select new
//            {
//                person.Name,
//                photo.Url
//            };
//var datas = await query.ToListAsync();
#endregion

#region Method Syntax
//inner join
//var query = context.Photos
//    .Join(context.Persons,
//    photo => photo.PersonId,
//    person => person.PersonId,
//    (photo, person) => new
//    {
//        person.Name,
//        photo.Url
//    });

//var datas = await query.ToListAsync();

#endregion

#region Multiple columns join
// eğerki birden fazla veri üzerinden tabloları eşleştirerek birleştirme işlemi gerçekleştirecek isek.
#region Query Syntax

//var query = from photo in context.Photos
//            join person in context.Persons
//                // Url =person.Name deme sebebimiz  => ilk ananim'de 2. veri olarak photo.Url demisiz o yüzdeb Url isminde bir değişken bekliyor karşılaştırma için.
//                on new { photo.PersonId, photo.Url } equals new { person.PersonId, Url = person.Name }
//            select new
//            {
//                person.Name,
//                photo.Url
//            };
//var datas = await query.ToListAsync();

#endregion

#region Method Syntax
//var query = context.Photos
//    .Join(context.Persons,
//    photo => new
//    {
//        photo.PersonId,
//        photo.Url
//    },
//    person => new
//    {
//        person.PersonId,
//        Url = person.Name
//    },
//    (photo, person) => new
//    {
//        person.Name,
//        photo.Url
//    });

//var datas = await query.ToListAsync();
#endregion

#endregion

#region 2'den fazla tabloyla Join

#region Query Syntax
//var query = from photo in context.Photos
//            join person in context.Persons
//               on photo.PersonId equals person.PersonId
//            join order in context.Orders
//               on person.PersonId equals order.PersonId
//            select new
//            {
//                person.Name,
//                photo.Url,
//                order.Description
//            };
//var datas = query.ToList();
#endregion
#region Method Syntax
//var query = context.Photos
//    .Join(context.Persons,
//    photo => photo.PersonId,
//    person => person.PersonId,
//    (photo, person) => new
//    {
//        person.PersonId,
//        person.Name,
//        photo.Url
//    })
//    .Join(context.Orders,
//    oncekiSorgu => oncekiSorgu.PersonId, //onceki sorgu => new diyip anonim dizide verdiğim verileri çağırabilir.
//    order => order.PersonId,
//    (oncekiSorgu, order) => new
//    {
//        oncekiSorgu.Name,
//        oncekiSorgu.Url,
//        order.Description
//    });
//var datas = await query.ToListAsync();
#endregion
#endregion

#region Group Join - GroupBy Değil!
//var query = from person in context.Persons
//            join order in context.Orders
//                on person.PersonId equals order.OrderId into personOrders //person'a karşılık orderları aldık : personOrders
//            //from order in personOrders : diyerek => order.Description gibi alanlara erişebiliriz, yoksa personOrders ile direk alanlara erişemiyoruz içinde dizi gibi orderları kaçtane ise tutuyor. ve basınca ekrana bize dizi gibi veriyor : personOrders diye assagıda bastıgımız gibi
//            //from order in personOrders => bu yapıyıda left , right joinde kullanacağız.
//            select new
//            {
//                person.Name,
//                Count = personOrders.Count(),
//                personOrders,  
//            };
//var datas = await query.ToListAsync();
#endregion
#endregion

// Left Join , Right Join, Full Join işlemlerini method syntaj ile yapamıyoruz
// DefaultIfEmpty : sorgulama sürecinde ilişkisel olarak karşılığı olmayan verilere default değerini yazdıran yani left join sorgusunu oluşturtan bir fonlsiyondur.

#region Left Join
// iki tablo var a , b => a tablosunun tamamını getir b'dede a tablosu ile ilişkibi b verilerini getir : yoksa null getirir karşılıgında ama a tablosundaki tüm verileri almış oluruz.
//var query = from person in context.Persons
//            join order in context.Orders
//                on person.PersonId equals order.PersonId into personOrders
//            from order in personOrders.DefaultIfEmpty() // boş olan order'lar için null değeri getir => ef core bu komut sayesinde left join kullanacağını anlar. => a tablosunun  tamamını alacak b tablosundaki ilişkilerini kontrol edecek : eğerki ilişkili veri yok ise b tablosunda boş varsa olanı getirecek her a tablosundaki veri için.
//            select new
//            {
//                person.Name,
//                order.Description
//            };
//var datas = await query.ToListAsync();
#endregion

#region Right Join
// ef core'da direk right join sorgulaması yapılamıyor. => bu yüzden right join yapılacak tablolar tersten verilerek left join yapılıyor tersten verdiğimiz için'de işlem sonucunda left join değilde, right join sonucu dönmüş oluyor.
//var query = from order in context.Orders
//            join person in context.Persons
//                on order.PersonId equals person.PersonId into orderPersons
//            from person in orderPersons.DefaultIfEmpty()
//            select new
//            {
//                person.Name,
//                order.Description
//            };
//var datas = await query.ToListAsync();
#endregion

#region Full Join
//// ef core'da direkt olarak full join yapamıyorruz : left join ve right join'i birleştirerek gerçekleştiriyoruz bu işlemi
////istatiksel olarak left join ve left join tablosunda aynı olmamak şartı ile verileri birleştirdiğimizde full join olur bizde onu yapıyoruz. 
//var leftQuery = from person in context.Persons
//                join order in context.Orders
//                    on person.PersonId equals order.PersonId into personOrders
//                from order in personOrders.DefaultIfEmpty()
//                select new
//                {
//                    person.Name,
//                    order.Description
//                };

//var rightQuery = from order in context.Orders
//                 join person in context.Persons
//                    on order.PersonId equals person.PersonId into orderPersons
//                 from person in orderPersons.DefaultIfEmpty()
//                 select new
//                 {
//                     person.Name,
//                     order.Description
//                 };
//// Union : iki diziyi birleştirme : aynı olan degerleri almaz. => birleştirme işlemi için aynı tür dizi veya listeye sahip olmamız gerekiyor.
//var fullJoin = leftQuery.Union(rightQuery);
//var datas = await fullJoin.ToListAsync(); //tekrarlanan verileri almadıgından left ve right joinden daha az veriye sahip
#endregion

#region Cross Join
// Cross Join =>  ilk tablodaki her satırı ikinci tablodaki her satırla birleştirir.
// ef core : from üstüne from yazdığımızda bunun cross join oldugunu anlar.
//var query = from order in context.Orders
//            from person in context.Persons 
//            select new
//            {
//                order, // tüm orders verilerni getir.
//                person // tüm person verilerini getir.
//            };
//var datas = await query.ToListAsync();
#endregion

#region Collection Selector'da Where kullanma durumu
//// ef core bu yapıyı inner join yapısı olarak kabul eder.
//var query = from order in context.Orders
//            from person in context.Persons.Where(p => p.PersonId == order.PersonId)
//            select new
//            {
//                order,
//                person
//            };
//var datas = await query.ToListAsync();
#endregion

#region Cross Apply
//// Inner join'e benzer
//var query = from person in context.Persons
//            from order in context.Orders.Select(o => person.Name)
//            select new
//            {
//                person,
//                order
//            };
//var datas = await query.ToListAsync();
#endregion

#region Outer Apply
// Left join'e benzer
//var query = from person in context.Persons
//            from order in context.Orders.Select(o => person.Name).DefaultIfEmpty()
//            select new
//            {
//                person,
//                order
//            };
//var datas = await query.ToListAsync();
#endregion
#endregion

Console.WriteLine();

public class Photo
{
    public int PersonId { get; set; }
    public string Url { get; set; }
    public Person Person { get; set; }
}
public enum Gender
{
    Man,
    Woman
}
public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public Photo Photo { get; set; }
    public ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }   
    public int PersonId { get; set; }
    public string Description { get; set; }
    public Person Person { get; set; }
}

class ComplexRelationalQuerieDbContext : DbContext
{
    public DbSet<Photo>Photos { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ComplexRelationalQuerieDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Photo>()
            .HasKey(p => p.PersonId);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Photo)
            .WithOne(p => p.Person)
            .HasForeignKey<Photo>(p => p.PersonId);

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
}