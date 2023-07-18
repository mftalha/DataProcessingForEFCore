// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

// { constraint: kısıtlama}
#region Primary Key Constraint

//Bir kolonu pk constraint ile birincil anahtarr yapmak istiyor isek bunun için name converntion'dan istifade edebiliriz. Id, ID, EntityNameId, EntityNameID şeklinde tanımlanan tüm propertler default olarak ef core tarafından pk constrait olacak şekilde generate edilebilir.
// Eğer ki, farklı bir property'e PK özelliğini atamak istiyor isek HasKey Fluent API'ı yahut Key attribute'u ile bu bildirimi iradeli bir şekilde yapmak zorundayız.

#region HasKey Fonksiyonu

#endregion

#region Key Attribute'u

#endregion

#region Alternate Keys - HasAlternateKey
// Alternate : alternatif
// bir entity içerisinde PK'e ek olarak her entity instance için alternatif bir benzersiz tanımlayıcı işlevine sahip olan bir key'dir.
// sql de Unique kolona eşdeğerdir.
#endregion

#region Composite Alternate key

#endregion

#region HasName fonksiyonu ile primary key constraint'e isim verme

#endregion
#endregion

#region Foreign key constraint

#region HasForeignKey Fonksiyonu

#endregion

#region ForeiginKey Attribute'u

#endregion

#region Composite Foreign Key

#endregion

#region Shadow Property üzerinden foreign key

#endregion

#region HasConstraintName Fonksiyonu ile primary key constraint'e isim verme

#endregion
#endregion

#region Unique Constraint

#region HasIndex - IsUnique Fonksiyonları

#endregion

#region Index, IsUnique Attribute'ları

#endregion

#region Alternate Key
#endregion

#endregion

#region Check Constraint
// belirli şarta göre verisel işlemelrin gerçekleştirilebilmesi. : şart sağlanmıyorsa : mesela ilgili column > 5 degilse hata döndürür veriyi eklemez.
#region HasCheckConstraint

#endregion
#endregion

//[Index(nameof(Blog.Url), IsUnique = true)]
class Blog
{
    public int Id { get; set; }
    //[Key]
    public string BlogName { get; set; }
    public string Url { get; set; }
    public ICollection<Post> Posts { get; set; }
}

class Post
{
    public int Id { get; set; }
    //[ForeignKey(nameof(Blog))]
    //public int BlogId { get; set; }
    public string Title { get; set; }
    public string BlogUrl { get; set; }
    public int A { get; set; }
    public int B { get; set; }
    public Blog Blog { get; set; }
}

class ConstraintDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post>Posts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ConstraintDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Blog>()
        //    .HasKey(b => b.BlogName);

        #region Alternate Keys - HasAlternateKey
        //modelBuilder.Entity<Blog>()
        //    .HasAlternateKey(b => b.Url);
        #endregion

        #region Composite Alternate key
        //modelBuilder.Entity<Blog>()
        //    .HasAlternateKey(b => new { b.Url, b.BlogName });
        #endregion

        #region HasName fonksiyonu ile primary key constraint'e isim verme
        //modelBuilder.Entity<Blog>()
        //    .HasKey(b => b.Id)
        //    .HasName("ornek"); // primary key ismi olarak : 'ornek' dedik veritabanında keys lerde pk ismi 'ornek' görünecektir.
        #endregion

        #region Foreign key constraint
        //modelBuilder.Entity<Blog>()
        //    .HasMany(b => b.Posts)
        //    .WithOne(p => p.Blog)
        //    .HasForeignKey(p => p.BlogId);

        #region Composite Foreign Key
        //modelBuilder.Entity<Blog>()
        //    .HasMany(b => b.Posts)
        //    .WithOne(p => p.Blog)
        //    .HasForeignKey(p => new { p.BlogId, p.BlogUrl });
        #endregion

        #region Shadow Property üzerinden foreign key

        //modelBuilder.Entity<Blog>()
        //    .Property<int>("BlogForeignKeyId"); // shadow property oluşturuyoruz

        //modelBuilder.Entity<Blog>()
        //    .HasMany(b => b.Posts)
        //    .WithOne(p => p.Blog)
        //    .HasForeignKey("BlogForeignKeyId"); // shadow propert'i veriyoruz.
        #endregion

        #region HasConstraintName Fonksiyonu ile primary key constraint'e isim verme
        //modelBuilder.Entity<Blog>()
        //    .Property<int>("BlogForeignKeyId");

        //modelBuilder.Entity<Blog>()
        //    .HasMany(b => b.Posts)
        //    .WithOne(p => p.Blog)
        //    .HasForeignKey("BlogForeignKeyId")
        //    .HasConstraintName("ornekforeignkey");
        #endregion
        #endregion

        #region HasIndex - IsUnique Fonksiyonları
        // [Index(nameof(Blog.Url), IsUnique = true)] => data annotion
        //modelBuilder.Entity<Blog>()
        //    .HasIndex(b => b.Url)
        //    .IsUnique();

        //modelBuilder.Entity<Blog>()
        //    .HasAlternateKey(b => b.Url);

        #endregion

        #region HasCheckConstraint
        modelBuilder.Entity<Post>()
            .HasCheckConstraint("a_b_check_const", "[A] > [B]");
        #endregion

    }
}