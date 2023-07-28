// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
SqlQueries context = new();

//Eğer ki, sorgumuzu LINQ ile  ifade edemiyorsak yahut LİNQ'ın ürettği sorguya nazaran daha optimize bir sorguyu manuel geliştirmek ve EF Core üzerinden execute etmek istiyorisek EF Core'un bu favranışı desteklediğini bilmeliyiz.

// Manuel bir şekilde/tarafımızca oluşturulmulş olan sorguları EF Core tarafından execute edebilmek için o sorgunun sonucunu karşılayacak bir entity model'ın tasarlanmış ve bunun DbSet olarak context nesnesine tanımlanım olması gerekiyor.

#region FromSqlInterpolated
// { Interpolated : enterpolasyonlu}
// from sql'den önce bu yapı kullanılıyordu.
// EF Core 7.0 ürümünden önce ham sorguları execute edebildiğimiz fonksiyondur
//FormattableString test = $"asd";// FormattableString type değişken değerini string interpreter yönyemi ile vermeyiz yoksa hata verecektir. FromSqlInterpolated'da FormattableString type'dadır.

//var persons = await context.Persons.FromSqlInterpolated($"SELECT * FROM Persons").ToListAsync();
#endregion

#region FromSql - EF Core 7.0
// EF Core 7.0 ile gelen metottur.

#region Query Execute
//var persons = await context.Persons.FromSql($"SELECT * FROM Persons").ToListAsync();
#endregion
#region Stored Procedure Execute
// {Store: Depo; }
//var persons = context.Persons.FromSql($"Execute dbo.sp_GetAllPersons NULL"); // ilgili store procedure Execute ile çalıştırıyoruz; NULL ilede paretre veriyoruz(parametreye null verdik)
// store procedure kodu sayfa altında
// var persons = context.Persons.FromSql($"Execute dbo.sp_GetAllPersons @PersonId {3}"); // @PersonId : storeprocedurdaki parametre ismi : ismi belirtmeden parametreytide versem çalışır.
#endregion
#region Parametreli sorgu oluşturma
#region Example 1
//int personId = 3;
//var persons = await context.Persons.FromSql($"SELECT * FROM PERSONS Where PersonId = {personId}").ToListAsync();
#endregion
#region Example 2 *
// sql sorgusunda kullanacağımız değişkenleri SqlParameter ile oluşturmak daha doğrudur.
// hız açısındanda bu yömntem daha hızlıdır.
// Burada sorguya geçirilen personId değişkeni arkaplanda bir DbParamater türüne dönüştürülerek o şekilde sorguya dahil edilmektedir.
//SqlParameter personId = new("PersonId", "3");
//personId.DbType = System.Data.DbType.Int32; //tipi int
//personId.Direction = System.Data.ParameterDirection.Input; //bu veri input veri
//var persons = await context.Persons.FromSql($"SELECT * FROM Persons Where PersonId = {personId}").ToListAsync();
#endregion
#endregion
#endregion

#region Dynamic SQL oluşturma ve parametre girme - FromSqlRaw
//string columnName = "PersonId", value = "3";
//var persons = await context.Persons.FromSql($"Select * From Persons Where {columnName} = {value}").ToListAsync();

// EF Core dinamik olarak oluşturulan sorgularda özellikle column isimleri parametreleştirilmiş ise o sorguyu çalıştırmayacaktır.

//string columnName = "PersonId";
//SqlParameter value = new("PersonId", "3"); //@PersonId burdaki isim ne ise aşşağıya onu yazmalıyım
//var persons = await context.Persons.FromSqlRaw($"Select * From Persons Where {columnName} = @PersonId", value).ToListAsync();

// FromSql ve FromSqlInterpolated scalar metotlarında SQL Injection vs. gibi güvenlik önlemleri alınmıl vaziyettedir. Lakin dinamik olarak sorguları oluşturuyorusak eğer burada güvenlikden geliştirici sorumludur. Yani gelen sorguda/veri yorumlar, noktalı virgüller yahut SQL'E özel karekterlerin algılanması ve bunların temizlenmesi geliştirici tarafından gerekmektedir.
#endregion

#region SqlQuery - Entiy olmayan scalar sorguların çalıştırılması - Non Entity - Ef Core 7.0
// Entity'si olmayan scalar sorguların çalıştırılıp sonucunu elde etmemizi sağlayan yeni bir fonksiyondur.
//var datas = await context.Database.SqlQuery<int>($"SELECT PersonId From Persons").ToListAsync(); // PersonelId leri al.

//var persons = await context.Persons.FromSql($"Select * from Persons").Where(p => p.Name.Contains("a")).ToListAsync(); //bu çalışır 

//var datas = await context.Database.SqlQuery<int>($"SELECT PersonId From Persons").Where(x => x > 5).ToListAsync(); //hata verecektir.
/*var datas = await context.Database.SqlQuery<int>($"SELECT PersonId Value From Persons").Where(x => x > 5).ToListAsync();*/ // gelen ismi bilmediğinden benden column ismini Value olarak değiştirmemi istiyor: veriyi çekerken : bu şekilde çalışır.
// SqlQuery'de LINQ operatörleri ile sorguya ekstradan katkıda bulunmak istiyorsak eğer bu sorgu neticesinde gelecek olan kolonun adını value olarak bildirmemiz gerekmektedir. Çünkü, SqlQuery metodu sorguyu bir subquery olarak generate etmektedir. Haliyle bu durumdan dolayı LINQ ile verilen şart ifadeleri statik olarak value kolonuna göre tasarlanmıştır. O yüzden bu şekilde bir çalışma zorunlu gerekmektedir.
#endregion

#region ExecuteSql
// Insert, Update, delete
//await context.Database.ExecuteSqlAsync($"Update Persons Set Name = 'Fatma' Where PersonId = 1");
#endregion
#region Sınırlılıklar
//// Queryler entity türününü tüm özellikleri için kolonlarda değer döndürmelidir. // Select * veya Select .. diyip tüm kolonları girmeliyim. : çalışması için.
//var persons = await context.Persons.FromSql($"Select Name From Persons").ToListAsync(); // hata verir

//Sütun isimleri property isimleri ile aynı olmalıdır. => entity'deki değişken ismi ile column adı aynı olmalı(anladığım). : yani entitydekinden farklı bir isim ile db'ye kaydedilmiş columnlar'da hata verecektir(anladıgım).

// SQL Sorgusu Join yapısı içereemz!!! Haliyle bu tarz ihtiytaç noktalarında Include fonksiyonu kullanılmamalıdır!
//var persons = await context.Persons.FromSql($"Select * From Persons Join Orders On Persons.PersonId = Orders.PersonId").ToListAsync(); //join yapılanmasını ham sorgularda kullanamıyoruz : hata verir ki içindeki sorguyu db'de direk versek çalışır : ef core sadece bu şekidle desteklemiyor. aşşağıdaki gibi kullanmalıyız.

// Join işlemlerini aşşağıdaki gibi yapmalıyız ef core'da
//var persons = await context.Persons.FromSql($"Select * From Persons")
//    .Include(p => p.Orders)
//    .ToListAsync();
#endregion

Console.WriteLine("");

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public ICollection<Order> Orders { get; set; }
}

public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public Person Person { get; set; }  
}

class SqlQueries : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SqlQueries;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
}



//CREATE PROC sp_GetAllPersons
//(
//	@PersonId INT NULL
//)AS
//BEGIN
//	IF @PersonId IS NULL
//		SELECT * FROM Persons
//	ELSE
//		SELECT * FROM Persons WHERE PersonId = @PersonId
//END


//select* from Persons
//select * from Orders