// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
// {shadow: gölge }
// Shadow Properties'deki amaç : created on , lastupdateon gibi alanları developer'ın veri eklerken veya güncellerken girmesini zorunlu tutmayı ortadan kaldırmakta'dır. : kullanılmalıdır'da

ShadowPropertyDbContext context = new();

#region Shadow Properties - Gölge Özellikler
// Entity sınıflarında fiziksel olarak tnaımlanmayan/modellenmeyen ef core için ilgilili entity için var olan veya var olduğu kabul edilen property'lerdir.
// Tabloda gösterilmesini istemediğimiz, lüzümlu görmediğimiz, entity instance'si üerinde işlem yapmıyacağımız kolonlar için shadow properties kullanılabilir.
// Shadow propertes değerleri ve stateleri Change Tracker tarafından kontrol edilir.
#endregion

#region Foreign Key - Shadow Properties
// İlişkisel senaryolarda foreşgn key propert'sini tanımlamadığımız halde EF Core tarafından dependent entity'e eklenmektedir. İşte bu shadow propert'dir.

//var blogs = await context.Blogs
//    .Include(b => b.Posts)
//    .ToListAsync();
//Console.WriteLine("");
#endregion

#region Shadow property oluşturma
// Bir entity üzerinde shadow property oluşturmak için fluent api'yi kullanmaktadır.

//Shadow property oluşturma
//modelBuilder.Entity<Blog>()
//    .Property<DateTime>("CreatedDate");
#endregion

#region Shadow Propert'e Erişim sağlama
#region ChangeTracker ile erişim
// shadow propery'e erişim sağlıyabilmek için savechange's de istifade edilebilir.

//Blog? blog = await context.Blogs.FirstOrDefaultAsync();
//var createdDate = context.Entry(blog).Property("CreatedDate"); //Entry ile : savechanges'e erişiriz buradanda shadow property'lere erişim sağlıyabiliriz.
//Console.WriteLine(createdDate.CurrentValue); // ben ilgili datayı değiştirdim ise : savechange diyip : daha db'ye atmadım ise bu şekilde değiştirilmiş veriye erişebilirim
//Console.WriteLine(createdDate.OriginalValue); // ben ilgili datayı değiştirdim ise : savechange diyip : daha db'ye atmadım ise bu şekilde değiştirilmemiş(db'deki haline) veriye erişebilirim

//createdDate.CurrentValue = DateTime.Now;
//await context.SaveChangesAsync();
#endregion

#region EF. propert ile erişim
// özellikle link sorgularında shadow property'lerine erişim için Ef. propery static yapılanmasını kullanabiliriz.
var blogs = context.Blogs.OrderBy(b => EF.Property<DateTime>(b, "CreatedDate")).ToList();

var blogs2 = context.Blogs.Where(b => EF.Property<DateTime>(b, "CreatedDate").Year > 2021).ToList();

Console.WriteLine("");
#endregion

#endregion

class Blog
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public ICollection<Post> Posts { get; set;}
}

class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool lastUpload { get; set; }
    public Blog Blog { get; set; }
}


class ShadowPropertyDbContext : DbContext 
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ShadowPropertyDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Shadow property oluşturma
        modelBuilder.Entity<Blog>()
            .Property<DateTime>("CreatedDate");

        //base.OnModelCreating(modelBuilder);
    }
}