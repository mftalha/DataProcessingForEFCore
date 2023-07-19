// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");

#region Loading Related Data

#region Eager Loading
// {Eager: İstekli}
// eager loading generate edilen bir sorguya ilişkisel verilerin parça parça eklenmesini sağlayan ve bunu yaparken iradeli/istekli bir şekilde yapmamızı sağlayan bir yöntemdir.
#region Include

#endregion

#region ThenInclude

#endregion

#region Filtered Include

#endregion

#region Eager Loading için kritik bir bilgi

#endregion

#region Birbirlerinden türetilmiş entity'ler arasında Include

#endregion

#region AutoInclude - EF Core 6

#endregion

#region IgnoreAutoIncludes

#endregion

#endregion

#region Explicit Loading
#endregion

#region Lazy Loading
#endregion
#endregion

public class Person
{
    public int Id { get; set; }
}
public class Employee
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
    }

}


/*

select * from Employees
select * from Orders
select * from Regions 

 */