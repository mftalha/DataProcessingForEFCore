// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

#region En temel basit bir sorgulama nasıl yapılır?

#endregion

public class QueringClass : DbContext
{
    DbSet<Student> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
}