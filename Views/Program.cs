// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
ViewDbContext context = new();

#region View Nedir ?
// Oluşturduğumuz kompleks sorguları ihtiyaç durumlarında daha rahat bir şekilde kullanabilmek için basitleştiren bir veritabanı objesidir. 
// proje rapoları için kullanılabiliyor.
// bir tablo gibi kullanılır
#endregion

#region EF Core ile view kullanımı

#region View Oluşturma
// 1. adım : boş bir migration oluşturulmalıdır(değişiklik yapmadan migrate işlemi gerçekleştirme. : mig_1 oluşacak onun içinde Up içinde view sorgusu yacaz.).
// 2. adım : migration içerisindeki Up fonksiyonunda view'in create komutlarını, down fonksiyonunda ise drop fonksiyoununda ise drop komutları yazılmalıdır.
// 3. adım : migrate edilmeli
#endregion

#region View'i DbSet olarak ayarlama
// View'i EF Core üzerinden sorgulayabilmek için view neticesini karşılayabilecek bir entity oluşturulması ve bu entity türünden DbSet propery'sinin eklenmesi gerekmektedir.
#endregion

#region DbSet'in bir view olduğunu bildirmek

#endregion

//var personOrders = await context.PersonOrders
//    .Where(po => po.Count > 10)
//    .ToListAsync();

#region EF Core'da view'lerin özellikleri
// Viewlerde Primary key olmaz! Bu yüzden İlgili DbSet'in HasNoKey ile işaretlenmesi gerekmektedir.
// View neticesidne gelen veriler Change Tracker ile takip edilmezler. Haliyle üzerinde yapılan değişiklikleri ef core veritabanına yansıtmaz.

// ef core etki etmeyecektir.
//var personOrder = await context.PersonOrders.FirstAsync();
//personOrder.Name = "Abuzer";
//await context.SaveChangesAsync();
#endregion

#endregion

Console.WriteLine();


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

public class PersonOrder
{
    public string Name { get; set; }
    public int Count { get; set; }
}

public class ViewDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PersonOrder> PersonOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region DbSet'in bir view olduğunu bildirmek
            modelBuilder.Entity<PersonOrder>()
                .ToView("vm_PersonOrders")
                .HasNoKey();
        #endregion

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ViewDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}




/*
 public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                Create VIEW vm_PersonOrders
                As
	                select Top 100 p.Name, Count(*) [Count] from persons p
	                inner join Orders o
		                ON p.PersonId = o.PersonId
	                group by p.Name
	                Order by [Count] Desc
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"DROP VIEW vm_PersonOrders");
        }
    }
 */


/* Db'de Veiw 
 -- view oluşturma
Create VIEW vm_PersonOrders
As
	select Top 100 p.Name, Count(*) [Count] from persons p
	inner join Orders o
		ON p.PersonId = o.PersonId
	group by p.Name
	Order by [Count] Desc

-- view'i silme
DROP VIEW vm_PersonOrders

-- view sorgulama 
select * from vm_PersonOrders
*/