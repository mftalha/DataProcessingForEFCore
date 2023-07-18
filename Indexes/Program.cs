// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region Index Nedir?
// Index, bir sütuna dayalı sorgulamarı daha verimli ve performanslı hale getirmek için kullanılan yapıdır.
#endregion

#region Index'leme nasıl yapılır?
// Default olarak PK, FK ve AK olan kolonlar otomatik olarak indexlenir. 

#region Index Attribute'u

#endregion

#region HasIndex Metodu

#endregion

#endregion

#region Composite Index

#endregion

#region Birden fazla index tanımlama

#endregion

#region Index Uniqueness

#endregion

#region Index Sort Order - Sıralama Düzeni (EF Core 7.0)

#endregion

#region Index Name

#endregion

#region Index Filter

#endregion

#region Included Columns

#endregion

//[Index(nameof(Name))] //Name kolonuna 1 tane index ata ;; class seviyeside kullanılır.
class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; } 
    public string? Surname { get; set; }
    public int Salary { get; set; }
}

class IndexDbContext : DbContext 
{
    public DbSet<Employee> Employees { get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=IndexDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region HasIndex Metodu
        //[Index(nameof(Name))] : data attribuyte
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Name);
        #endregion
    }
}