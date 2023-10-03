// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

public class MyDbContext : DbContext
{
	public DbSet<MyEntity> MyEntities { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DifferentSchemaDb;User Id=SA;Password=123!;TrustServerCertificate=True");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("MySchema"); // bütün tablolara Schema ismi atıyacaksam

		//modelBuilder.Entity<MyEntity>().ToTable("MyEntity", "MySchema"); // belirli tabloya Schema ismi atıyacaksam
	}
}

public class MyEntity
{
	public int Id { get; set; }
	public string Name { get; set; }
}