// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Channels;

Console.WriteLine("Hello, World!");
EFCTracking context = new();

#region AddDatas
/*
for(int i =  0; i < 5; i++)
{
    User user = new()
    {
        Name = $"User-{i+1}",
        Email = $"user-{i+1}@gmail.com",
        Password = "1",
    };
    UserRol userRol = new()
    {
        RoleID = 1,
    };

    await context.Users.AddAsync(user);
    await context.UserRols.AddAsync(userRol);

    if (i < 3)
    {
        Role role = new()
        {
            RoleName = $"Role{i + 1}"
        };
        await context.Roles.AddAsync(role);
    }
}
await context.SaveChangesAsync();
*/
#endregion

#region AsNoTracking Methodu
//Context üzerinden gelen tüm datalar Change Tracker mekanizması tarafından takip edilmektedir.

//Change Tracker, takip ettiği nesnelerin sayısıyla doğru orantılı olacak şekilde bir maliyete sahiptir. O yüzden üzerinde işlem yapılmayacak verilerin takip edilmesi bizlere lüzümsuz yere bir maliyet ortaya çıkaracaktır.

//AsNoTracking metodu, context üzerinden sorgu neticesinde gelecek olan verilerin Change Tracker tarafından takip edilmesini engeller.

//AsNoTracking metodu ile Change Tracker'ın ihtiyaç olmıyan verilerindeki maliyetini törpülemiş oluruz.

//AsNoTracking fonksiyonu ile yapılan sorgulamalarda, verileri elde edebilir, bu verileri istenilen noktalarda kullanılabilir lakin veriler üzerinde herhangi bir değişiklik/update işlemi yapamayız.

/*
var users = await context.Users.AsNoTracking().ToListAsync(); // AsNoTracking ileçekilen verileri efcore arka planda takip etmesin.
foreach (var user in users)
{
    Console.WriteLine(user.Name);
    user.Name = $"{user.Id}-{user.Name}";
    //context.Users.Update(user); //bu şekilde update işlemini gerçekleştirebiliriz takip olmasa bile : çünkü takip yok iken direk SaveChanges'i çağırır isem takip olmadığından güüncellemeyi görmiyecekti ben değişiklik yapsam bile o yüzden Update methodu ile tüm verileri vererek güncelleme yapabiliriz.
}
await context.SaveChangesAsync(); // takip yok ise tek başına güncelleme yaptıramaz.
*/

#endregion

#region AsNoTrackingWithIdentityResolution
/*
  - change tracker mekanizması sayesinde yinilenen data'lar aynı instance'leri kullanırlar(user - role'de egerki admin rülü 1 kere getirildi ise bir kullanıcı için : 2. kullanıcı için birdaha getirilmeyecektir.).
  - AskNoTracking methodu ile yapılan sorgularda yinilenen datalar farklı instance'lerde karşılanırlar.
  
  - CT(Change Tracker)  mekanizması yinelenen datalar'ı tekil instance olarak getirir. Buradan ekstradan bir performans kazancı söz konusudur.
  - Bizler yaptığımız sorgularda takip mekanizmasının AsNoTracking methodu ile maliyet'ini  kırmak isterken bazen maliyete sebebiyet verebiliriz.(Özellikle ilişkisel tabloları sorgularken bu duruma dikkat etmemiz gerekiyor)
  - AskNoTracking ile elde edilen veriler takip edilmeyeceğinden dolayı yinelenen verilerin ayrı instanceler'de olamasına sebebiyet veriyoruz. çünkü ct mekanizması takip ettiği nesneden bellekte varsa eğer aynı nesneden birdaha oluşturma gereki duymaksızın o nesneyi ayrı noktalardaki ihtiyacı aynı instance üzerinden gidermektedir.
  - Böyle bir durumda hem takip mekanizmasının maliyetini ortadan kaldırmak hemde yinelenen dataları tek bir instance üzerinde karşılamak için AskNoTrackingWithIdentityResolution fonksiyonunu kullanabiliriz.(ct'ye göre maliyette daha zayıftır ama AskNoTrackingWithIdentityResolution : ef cor'un direk takipli veri çekmesinden daha hızlıdır.)
*/

//var books = await context.Books.Include(k => k.Authors).ToListAsync(); // 4 kitap  2 yazar için = 6 instance olusur
//var books = await context.Books.Include(k => k.Authors).AsNoTracking().ToListAsync(); // 4 kitap  2 yazar için = 8 instance olusur
//var books = await context.Books.Include(k => k.Authors).AsNoTrackingWithIdentityResolution().ToListAsync(); // 4 kitap  2 yazar için = 6 instance olusur => ilişkili tablolarda bu kullanılır

// AsNoTrackingWithIdentityResolution fonksiyonu AsNoTracking fonksiyonuna nazaran görece yavaştır/maliyetlidir lakin  CT'a nazaran daha performanslı ve az maliyetlidir


#endregion

#region AsTracking
// Context üzerinden gelen dataların change tracker tarafından takıip edilmesini iradeli bir şekilde ifade etmemizi sağlayan fonksiyondur.
// Niye kullanalım? 
// bir sornraki inceleyeeğimiz UseQueryTrackingBahavior methodunun davranışı geregi uygulama seviyesinde CT'ın default olarak pasif hale getirilirse böyle durumda takip mekaznismasının ihtiyaç oldugu sorgularda AsTracking fonksiyonun kullanabilir ve böylece takip mekanismanı iradeli bir şekilde devreye sokmuş oluruz.
//var books = await context.Books.AsTracking().ToListAsync();
#endregion

#region UseQueryTrackingBehavior
// EF Core seviyesinde/uygulama seviyesinde ilgili contextten gelen verilerin üzerinde CT mekanizmasının davranışını temel seviyede belirlememizi sağlayan fonksiyondur. yani konfigürasyon fonksiyonudur.
#endregion

Console.WriteLine();
public class EFCTracking : DbContext
{
    public DbSet<User> Users { get; set; }
    //public DbSet<UserRol> UserRols {get; set;}
    public DbSet<Role> Roles { get; set; }
    public DbSet<Book> Books { get; set; }
    //public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TrackingDb;Trusted_Connection=True;TrustServerCertificate=True");
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //böyle bir kullanımda takip yapılmasını istediğimiz durum veya tablalar'da UseQueryTrackingBehavior kullanıp takibi aktif hale getirmeliyiz.
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
/*
public class UserRol
{
    public int UserRolId { get; set; }
    public int RoleID { get; set; }
}
*/

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; }
}
public class Book
{
    public Book() => Console.WriteLine("Kitap nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string BookName { get; set; }
    public int PageNumber { get; set; }
    public ICollection<Author> Authors { get; set; }
}

/*
[Keyless]
public class BookAuthor
{
    public int BookId { get; set; }
    public string AuthorId { get; set; }
}
*/

public class Author
{
    public Author() => Console.WriteLine("Yazar nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<Book> Books { get; set; }
}