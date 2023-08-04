// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
EntitySplittingDbContext context = new();

#region Entity Splitting
//Birden fazla fiziksel tabloyu Entity Framework Core kısmında tek bir entity ile temsil etmemizi sağlayan bir özelliktir.
#endregion
#region Örnek
#region Veri Eklerken
Person person = new()
{
Name = "Talha",
Surname = "Satır",
City = "Antalya",
Country = "Türkiye",
PhoneNumber = "1234567890",
PostCode = "1234567890",
Street = "..."
};

//await context.Persons.AddAsync(person);
//await context.SaveChangesAsync();
#endregion
#region Veri Okurken
//person = await context.Persons.FindAsync(1);
Console.WriteLine();
#endregion
#endregion
public class Person
{
    #region Persons Tablosu
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    #endregion
    #region PhoneNumbers Tablosu
    public string? PhoneNumber { get; set; }
    #endregion
    #region Addresses Tablosu
    public string Street { get; set; }
    public string City { get; set; }
    public string? PostCode { get; set; }
    public string Country { get; set; }
    #endregion
}
class EntitySplittingDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entityBuilder =>
        {
            entityBuilder.ToTable("Persons")
                .SplitToTable("PhoneNumbers", tableBuilder =>
                {
                    tableBuilder.Property(person => person.Id).HasColumnName("PersonId");
                    tableBuilder.Property(person => person.PhoneNumber);
                })
                .SplitToTable("Addresses", tableBuilder =>
                {
                    tableBuilder.Property(person => person.Id).HasColumnName("PersonId");
                    tableBuilder.Property(person => person.Street);
                    tableBuilder.Property(person => person.City);
                    tableBuilder.Property(person => person.PostCode);
                    tableBuilder.Property(person => person.Country);
                });
        });
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=EntitySplittingDb;User ID=SA;Password=123!;TrustServerCertificate=True");
    }
}