// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");
OneToManyDbContext context = new();

// her çalışanın bir dapartmanın olabilir ama bir departman'da birden fazla çalışan olabilir.

#region Default Convention
// {Convention: Ortak düşünce; Collection: koleksiyon, toplamak }

// Default convention yönteminde bire çok ilişkiyi kurar ien foreign key column'una karşılık gelen bir propert tanımlamak zorunda değiliz. Eğer tanımlamazsak EF Core bunu kendisi tanımlayacak yok eğer tanımlarsak, tanımladığımızı baz alacaktır.
//public class Employee //Dependent Entity
//{
//    public int Id { get; set; } 
//    public int DepartmentId { get; set; } // foreign key : oluşturmaz isek kendisi oluşturur.
//    public string Name { get; set; }
//    public Department Department { get; set; }
//}

//public class Department
//{
//    public int Id { get; set; }
//    public string DepartmentName { get; set; }
//    public ICollection<Employee> Employees { get; set; }
//}
#endregion

#region Data Annotations
// Default convent yönteminde foreign key column'una karşılık gelen propert tanımladığımızda bu propert ismi temek geleneksel entity tanımlama kurallarına uymuyor ise eğer data annotations lar ile müdahalede bulunabiliriz.
//public class Employee //Dependent Entity
//{
//    public int Id { get; set; }
//    [ForeignKey(nameof(Department))]
//    public int DId { get; set; } //dependent
//    public string Name { get; set; }
//    public Department Department { get; set; }
//}

//public class Department
//{
//    public int Id { get; set; }
//    public string DepartmentName { get; set; }
//    public ICollection<Employee> Employees { get; set; }
//}
#endregion

#region Fluent API

public class Employee //Dependent Entity
{
    public int Id { get; set; }
    //public int DId { get; set; }
    public string Name { get; set; }
    public Department Department { get; set; }
}

public class Department
{
    public int Id { get; set; }
    public string DepartmentName { get; set; }
    public ICollection<Employee> Employees { get; set; }
}

#endregion

class OneToManyDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=OneToManyDb;Trusted_Connection=True;TrustServerCertificate=True");
    }

    //Fluent API yöntemi
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasOne(c => c.Department)
            .WithMany(c => c.Employees);
            //.HasForeignKey(c => c.DId); // default değilde benim istediğim isimde foreign key oluşturmak için
    }
}