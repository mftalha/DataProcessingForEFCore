// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
LoadingRelatedDataDbContext context = new();

#region Loading Related Data

#region Eager Loading
// {Eager: İstekli}
// eager loading generate edilen bir sorguya ilişkisel verilerin parça parça eklenmesini sağlayan ve bunu yaparken iradeli/istekli bir şekilde yapmamızı sağlayan bir yöntemdir.
// Eager loading arkaplanda üretilen sorguya join ekler.


#region Include
// eager loading işlemini yapmamızı sağlayan bir fonksiyondur
// yani üretilen bir sorguya diğer ilişkisel tabloların dahil edilmesini sağlayan bir eşleve sahiptir.

//var employee = await context.Employees.Include("Orders").ToListAsync();
// tek bir join
//var employee = await context.Employees
//    .Include(e => e.Orders)
//    .ToListAsync();

// birden fazla join işlemi için
//var employee = await context.Employees
//    .Include(e => e.Orders)
//    .Include(e => e.Region)
//    .ToListAsync();

//var employee = await context.Employees
//    .Where(e => e.Orders.Count > 2) // WHER'İson sorguda yazabilriiz ilk sorguda ef core arka planda aynı sorguyu atacaktır.
//    .Include(e => e.Orders)
//    .Include(e => e.Region)
//    .ToListAsync();
#endregion

#region ThenInclude
// ThenInclud, üretilen sorguda Include edilen tabloların ilişkili olduğu diğer tablolarında sorguya ekleyebilmek için kullanılan bir fonskiyondur.
// Eğer ki, üreitelen sorguya include edilen navigation property koleksiyonel bir properyse işte o zaman bu properyy üzerinden diğer ilişkisel tabloya erişim gösterilememektedir. Böyle bir durumda koleksiyonel propertlerin türlerine erişip, o tür ile ilişkili diğer tablolarıda sorguya eklememizi sağlayan fonksiyondur.

// bağlantı tekil olduğunda bu bağlantıyı kullanabiliyoruz.
//var orders = await context.Orders
//    //.Include(o => o.Employee) // order'dan employa geçiyoruz
//    .Include(O => O.Employee.Region) // employ tablosundan - region tablosuna geçiyoruz. : tek başına bu şekildede kullanabiliriz.
//    .ToListAsync();

// 2. bağlantıda n bağlantılı ilişkiye erişmek istiyor isek bu  şekilde bi kullanım yapmalıyız 
//var regions = await context.Regions
//    .Include(r => r.Employees)
//    .ThenInclude(e => e.Orders).ToListAsync();

#endregion

#region Filtered Include
// Sorgulama süreçlerinde Include yaparken sonuçlar üzerinde filtreleme ve sıralama gerçekleştirebilmemizi sağlayan bir özelliktir.

//var employees = await context.Regions
//    .Include(r => r.Employees.Where(e => e.Name.Contains("a")).OrderByDescending(e => e.Surname)).ToListAsync();

// Desteklenen fonksyionlar: Where, OrderBy, OrderByDescending, ThenBy, ThenByDescending, Skip, Take
// Change Tracker'ın aktif olduğu durumlarda Incude edilmiş sorgular üzerinde filtreleme sonuçları beklenmeyen olabilir. Bu durum daha önce sorgulanmış ve change tracker tarafından takip edilmiş veriler arasında filtrenin gereksinimi dışında kalan veriler için söz konusu olacaktır. Bundan dolayı sağlıklı bir filtered include operasyonu için change tracker'ın kullanılmadığı sorguları tercih etmeyi düşünebilririz.
#endregion

#region Eager Loading için kritik bir bilgi
// EF Core önceden üretilmiş ve execute edilerek verileri belleğe alınmış olan sorguların verilerini, sonraki sorgularda kullanılır.

//var orders = await context.Orders.ToListAsync();

//var employees = await context.Employees.ToListAsync();

#endregion

#region AutoInclude - EF Core 6
// uygulama seviyesinde bir entity'e karşılık tüm sorgulamalar'da 'kesinlikle' bir tabloya Include işlemi gerçekleştirilecekse eğer bunu her bir sorgu için tek tek yapmaktansa merkezi bir hale getiremizi sağlayan biz özelliktir.

//var employees = await context.Employees.ToListAsync();
#endregion

#region IgnoreAutoIncludes
// AutoInclude konfigurasyonunu sorgu seviyesinde pasivize edebilmek için kullandığımız fonksiyondur.
//var employees = await context.Employees.IgnoreAutoIncludes().ToListAsync();
#endregion

#region Birbirlerinden türetilmiş entity'ler arasında Include
// birbirini miras alan tablolar : tph , tpt , tpc
// person tablosu ile emptloye tablosu ilişkili olmadığından hata verecektir ama : employees tablosu persons tablosunu miras aldığından aşşağıdaki yöntemler ile include gerçekleştirebiliriz.

#region Cast Operatör ile Include
//var persons = await context.Persons.Include(p => ((Employee)p).Orders).ToListAsync();
#endregion

#region as Operatör ile Include
//var persons2 = await context.Persons.Include(p => ((p as Employee)).Orders).ToListAsync();
#endregion

#region 2. Override ile include
//var persons3 = await context.Persons.Include("Orders").ToListAsync();
#endregion

#endregion

#endregion

#region Explicit Loading
// {Explicit : açık - belirgin}
#endregion

#region Lazy Loading
#endregion
#endregion

Console.WriteLine("");
public class Person
{
    public int Id { get; set; }
}
public class Employee //: Person
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
    public List<Order> Orders { get; set; }
    public Region Region { get; set; }
}

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Employee> Employees { get; set;}
}

public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public Employee Employee { get; set; }
}

class LoadingRelatedDataDbContext : DbContext 
{ 
    //public DbSet<Person> Persons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Region> Regions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LoadingRelatedDataDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region AutoInclude - EF Core 6
        // employees tablosunu çekerken regions tablosundaki verilerde çek beraberinde
        modelBuilder.Entity<Employee>().Navigation(e => e.Region).AutoInclude();
        #endregion
    }

}


/*

select * from Employees
select * from Orders
select * from Regions 

 */