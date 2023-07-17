// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

DataSeedDbContext context = new();
// Seed Data'lar migrationların dışında eklenmesi ve değiştirilmesi beklenemeyen durumlar için kullanılan bir özelliktir.

// { seed: tohum }
#region Data Seeding Nedir?
// EF Core ile inşa edilen veritabanı içersinde veritabanı nesneleri olabileceği gibi'de verilerin'de migrate sürecinde üretilmesini isteyebilririz.
// bu ihtiyaca binaen seed data özelliği ile ef core üzerinde migrationlarda veriler oluşturabilir ve migrate ederken bu verilerin hedef tablolarımıza basabiliriz.
// Seed Data'lar, migrate süreçlerinde hazır verileri tablolara basabilmek için bunları yazılım kısmında tutmamızı gerektirmektedirler. Böylece bu veriler üzerinde veritabanı sevisiyesinde istenilen manipulasyonlar gönül rahatlığıyla gerçekleştirilebilmektedir.
// Data Seeding özelliği şu noktalarda kullanılabilir;
// Test için geçici verilere ihtiyaç varsa,
// Asp.Net Core'daki Identity yapılanmasındaki roller gibi static değerler'de tutulabilir.
// Yazılım için temel konfigurasyonel değerler.
// 
#endregion

#region Seed Data Ekleme
// on model creating methodu içinde enytity fonksiyonundan sonra çağrılan HasData fonksiyonu ilgili entitye karşılık Seed Data'ları eklememizi sağlayan bir fonksiyondur.
// pk değerlerinin manuel olarak bildirilmesi/verilmesi gerekmektedir. Neden dire sorarsanız eğer, ilişkisel verileri de Seed Datalarla üretebilmek için.
#endregion

#region İlişkisel Tablolar için Seed Data Ekleme
// İlişkisel senaryolarda dependent table'a veri eklerken varsa foreign key kolonunun propertysi varsa eğer ona ilişkisel değerini vererek ekleme işlemini yapıyoruz.
#endregion

#region Seed Datanın Primary Key'ini değiştirme
// Eğer ki migrate edilen herhangi bir data sonrasında pk değiştirilirse bu data ile varsa ilişlkisel başka veriler onlara cascade davranışı sergilenecektir.
#endregion

class Post
{
    public int Id { get; set; } 
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Blog Blog { get; set; }
}
class Blog
{
    public int Id { get; set; }
    public string Url { get; set; }
    public ICollection<Post> Posts { get; set; }
}
class DataSeedDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }  
    public DbSet<Blog> Blogs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DataSeedDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Seed Data Ekleme

        modelBuilder.Entity<Blog>()
            .HasData(
                new Blog() { Id = 1, Url = "www.talhasatir.com/blog" },
                new Blog() { Id = 2, Url = "www.talhasatir.com/blog" }
            );
        #endregion

        #region İlişkisel Tablolar için Seed Data Ekleme
        modelBuilder.Entity<Post>()
            .HasData(
            new Post() { Id = 1, BlogId = 1, Title = "A", Content ="..."},
            new Post() { Id = 2, BlogId = 1, Title = "B", Content ="..."},
            new Post() { Id = 3, BlogId = 2, Title = "B", Content ="..."}
            );
        #endregion
        
    }
}