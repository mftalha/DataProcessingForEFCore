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

#endregion
#region Method Syntax

#endregion
#endregion

#region Group Join - GroupBy Değil!

#endregion
#endregion

#region Left Join

#endregion

#region Right Join

#endregion

#region Full Join

#endregion

#region Cross Join

#endregion

#region Collection Selector'da Where kullanma durumu

#endregion

#region Cross Apply

#endregion

#region Outer Apply

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