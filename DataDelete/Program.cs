// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

Console.WriteLine("Hello, World!");

#region Veri Nasıl Silinir?
/*
Program4 context = new();
var student = await context.Students.FirstOrDefaultAsync(f => f.Id == 4);
context.Students.Remove(student);
await context.SaveChangesAsync();
*/
#endregion
#region Silme İşleminde ChangeTracker'ın Rolü
//ChangeTracke, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadıdr(select işlemi). Bu takip mekanizması sayesinde context üzerinden gelen verilerle ilgili işlemler neticesinde update yahut delete sorgularının oluşturulacağı anlaşılır! => bun göredb'de silme veya güncelleme komutu ve ardından savechanges() verildiğinde bu takipdeki değişikliklere göre db'de değişim sağlanır. 
#endregion
#region Takip edlmeyen nesneler nasıl silinir ?
/*
Program4 context = new();
Student student = new() //sadece silinecek itemin primary'keyinden bir nesnesini oluşturuyoruz : ayırt edicilik için
{
    Id = 1
};
context.Students.Remove(student);
context.SaveChanges();
*/
#endregion
#region EntityState ile silme işlemi
/*
Program4 context = new();
Student student = new()
{
    Id = 2
};
context.Entry(student).State = EntityState.Deleted;
await context.SaveChangesAsync();
*/
#endregion
#region SaveChanges'i verimli kullanımı
// - ne kadar savechanges'i en az sayıda kullanarak işimizi gördüğümüzde verimli olacaktır.
// her savechanges çağrıldığında veritabanında transection çağrılır. 
#endregion
#region RemoveRange : birden fazla veriyi silmek için
/*
Program4 context = new();
List<Student> student = await context.Students.Where(f => f.Id >= 5 && f.Id <= 8).ToListAsync();
context.Students.RemoveRange(student);
await context.SaveChangesAsync();
*/
#endregion
public class Program4 : DbContext
{
    public DbSet<Student> Students{get; set;}
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