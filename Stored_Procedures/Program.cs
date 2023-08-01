// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

Console.WriteLine("Hello, World!");
StoredProcedureDbContext context = new();

#region Stored Procedure Nedir ?
// SP, view'ler gibi kompleks sorgularımızı daha basit bir şekilde tekrar kullanılabilir bir hale getirmemizi sağlayan veritbanı nesnesidir.
// Viewler tablo misali bir davranış sergilerken, SP'lar ise fonksiyonel davranış sergilerler.
// Ve türlü türlü artılarıda vardır.
#endregion

#region EF Core ile Stored Procedure kullanımı

#region Stored Procedure Oluşturma
// 1. adım : boş bir migration oluştururuz.
// 2. adım : Migration'un içerisindeki Up fonksiyonuna SP'IN Create komutlarını yazarız , Down fonk. ise Drop komutlarını yazarız.
// 3. adım migrate ederiz.
#endregion

#region Stored Procedure'ü Kullanma
// SP'ı kullanabilmek için bir entity'e ihtiyacımız vardır. Bu entity'in DbSet propertysi olarak context nesnesinede eklenemsi gerekmektedir.
// Bu DbSet propertysi üzerinden FromSql fonksiyionunu kullanarak 'Exec ....' komutu eşliğinde SP yapılşanmasını çalıştırıp sorgu neticesinde elde edebiliriz
#region FromSql
var datas = await context.PersonOrders.FromSql($"EXEC sp_PersonOrders").ToListAsync();
#endregion
#endregion

#region Geriye değer döndüren stored procedure'ü kullanma
//SqlParameter countParameter = new()
//{
//    ParameterName = "Count",
//    SqlDbType = System.Data.SqlDbType.Int,
//    Direction = System.Data.ParameterDirection.Output
//};
//await context.Database.ExecuteSqlRawAsync($"Exec @count = sp_bestSellingStaff", countParameter);
//Console.WriteLine(countParameter.Value);
#endregion

#region Parametreli stored procedure kullanımı
#region Input Parametreli stored procedure'ü kullanma

#endregion
#region Output parametreli stored procedure'ü kullanma

#endregion

SqlParameter nameParameter = new()
{
    ParameterName= "name",
    SqlDbType = System.Data.SqlDbType.NVarChar,
    Direction = System.Data.ParameterDirection.Output,
    Size = 1000
};
await context.Database.ExecuteSqlRawAsync($"Execute sp_PersonOrders2 7, @name OUTPUT", nameParameter);
Console.WriteLine(nameParameter.Value);
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

[NotMapped]
public class PersonOrder
{
    public string Name { get; set; }
    public int Count { get; set; }
}

public class StoredProcedureDbContext : DbContext 
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PersonOrder> PersonOrders { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=StoredProcedureDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<PersonOrder>()
            .HasNoKey();

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
}

// migration 1 tablolar;
// migration 2 : stored procedure çağırıp entity ile eşleştirip verileri okuma
// migration 3 : stored procedure işlemi sonucu return eden tek bir değer elde etme
// migration 4 : stored procedure işleminde paremetreler kullanma


/* sql sorgular
 
---- stored procedure oluşturma komutur
--Create procedure sp_PersonOrders
--As
--	select p.Name, Count(*) [Count] from Persons p
--	Join Orders o
--		ON p.PersonId = o.PersonId
--	Group By p.Name
--	Order by Count(*) Desc

---- stored procedure çalıştırma komutu
--Exec sp_PersonOrders


---- return lu stored procedure oluşturma komutur
--Create procedure sp_bestSellingStaff
--As
--	DECLARE @name NVARCHAR(Max), @count Int
--	select Top 1 @name = p.Name, @count = Count(*) from Persons p
--	Join Orders o
--		ON p.PersonId = o.PersonId
--	Group By p.Name
--	Order by Count(*) Desc
--	RETURN @count
	
---- return lu stored procedure çalıştırma komutu
--Go
--Exec sp_PersonOrders

--Go

--Declare @count INT
--Exec @count = sp_bestSellingStaff
--SELECT @count


-- parametreli stored procedure oluşturma komutur
Create procedure sp_PersonOrders2
(
	@PersonId INT,
	@Name NVARCHAR(Max) Output
)
As
	select @Name = p.Name from Persons p
	Join Orders o
		ON p.PersonId = o.PersonId
	WHERE p.PersonId = @PersonId

GO 
-- paremetreli stored procedure çalıştırma komutu

Declare @name nvarchar(MAX)
Exec sp_PersonOrders2 3, @name OUTPUT
select @name

*/