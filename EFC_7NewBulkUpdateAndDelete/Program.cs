// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

Console.WriteLine("Hello, World!");
BulkUpdateAndDeleteDbContext context = new();

#region EF Core 7 öncesi Toplu Güncelleme
//var persons = await context.Persons.Where(p => p.PersonId > 5).ToListAsync();
//foreach(var person in persons)
//{
//    person.Name = $"{person.Name}...";
//}
//await context.SaveChangesAsync();
#endregion
#region EF Core 7 Öncesi Toplu Silme
//var persons = await context.Persons.Where(p => p.PersonId > 5).ToListAsync();
//context.RemoveRange(persons);
//await context.SaveChangesAsync();
#endregion

// ExecuteUpdate ve ExecuteDelete fonksiyonları ile bulk(tıolu) veri güncelleme ve silme işlemelri gerçekleştirirken savechanges fonksiyonunu çağğrımamız gerekmektedir : çünkü bu fonksiyonlar isimleri/adları üzerinde Execute... fonksiyonlarıdır. Yani direk veritbanına fiziksel etkide bulunurlar.
// Eğer ki istiyorsak transaction kontrolü ele alarak bu fonksiyonların işlevlerini de süreçte kontrol edebiliriz.
#region ExecuteUpdate
// p.SetProperty(p => p.Name, v => v.Name + " new") => p.Name kolonuna ; v.Name + " new" yazdır
// string interproter kullanırsam expresin kısmında hata verdirecektir.
//await context.Persons.Where(p => p.PersonId > 3).ExecuteUpdateAsync(p => p.SetProperty(p => p.Name, v => v.Name + " new"));
#endregion
#region ExecuteDelete
//await context.Persons.Where(p => p.PersonId > 3).ExecuteDeleteAsync();
#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
}

class BulkUpdateAndDeleteDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=BulkUpdateAndDeleteDb;User Id=SA;Password=123!;TrustServerCertificate=True");
    }

}