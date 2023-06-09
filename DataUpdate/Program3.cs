// See https://aka.ms/new-console-template for more information


using Microsoft.EntityFrameworkCore;

//Console.WriteLine("Hello, World!");

#region Veri Nasıl Güncellenir
/*
Program3 context = new();

Student student = await context.Students.FirstOrDefaultAsync(f => f.Id == 3);
student.City = "Elazig";

await context.SaveChangesAsync();

//veri select'le çekildiği ve changetracker ile takip edildigi için update yazmaya gerek yok : degisikligi changetracker anlıyacakyır.
*/
#endregion
#region ChangeTracker Nedir? Kısaca!
/*
 - ChangeTracke, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadıdr(select işlemi). Bu takip mekanizması sayesinde context üzerinden gelen verilerle ilgili işlemler neticesinde update yahut delete sorgularının oluşturulacağı anlaşılır! => bun göredb'de silme veya güncelleme komutu ve ardından savechanges() verildiğinde bu takipdeki değişikliklere göre db'de değişim sağlanır. 
*/
#endregion
#region Takip Edilmeyen Nesneler Nasıl Güncellenir? ; Update Fonksiyonu?
/*
Program3 context = new();
Student student = new()
{
    Id = 3,
    Name = "Sami",
    City = "Balikesir"
};
context.Students.Update(student);
await context.SaveChangesAsync();

//ChangeTracker mekanizması tarafından takip edilmeyen nesnelerin güncellenebilmesi için Update fonksiyonu kullanılır!
//Update fonksiyonun kullanılabilmesi için nesnede Id değeri verilmelidir! Bu değer güncellenecek(update sorgusu oluşturulacak) verinin hangisi olduğunu igade edeceltir.
*/
#endregion
#region EntityState Nedir
/*
// Bir entity instance'ının durumunu ifade eden bir referanstır.
Program3 context = new Program3();
Student student = new();
Console.WriteLine(context.Entry(student).State);
*/
#endregion
#region EF Core açısından bir verinin güncellenmesi gerektiği nasıl anlaşılıyor?
/*
Program3 context = new();
Student student = await context.Students.FirstOrDefaultAsync(f => f.Id == 3);
Console.WriteLine(context.Entry(student).State);

student.Name = "Hilmi";
Console.WriteLine(context.Entry(student).State);

await context.SaveChangesAsync();
Console.WriteLine(context.Entry(student).State);
*/
#endregion
#region Birden fazla veri güncellenirken nelere dikkat etilmelidir ?
Program3 context = new();
var students = context.Students.ToListAsync().Result; //tablodaki tüm verileri çek
foreach(var student in students)
{
    student.Name += "-";
}
await context.SaveChangesAsync();
#endregion

public class Program3 : DbContext
{
    public DbSet<Student> Students { get; set; }
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