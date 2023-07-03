// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

// {cross : geçmek, kesişen ; composite: bileşik, karma}
// Best practic olarak Fluent API iyidir => Data Annotations singletion'a aykırı olabilir.

#region Default Convention
// { convention: Ortak düşünce, toplantı }
// İki entity arasındaki ilişkiyi navigation propert'ler üzerinden çoğul olarak kurmalıyız(ICollection, List) => public List<Author> Authors { get; set; } ; public List<Book> Books { get; set; }
// Default Convention'da cross table'ı manuel oluşturmak zorunda değiliz. EF Core tasarıma uygun bir şekilde cross table'ı kendisi otomatik basacak ve generate edecektir. => BookAuthor veya AuthorBook isminde.
// Ve oluşturulan cross table'ın içerisinde composite primary key'i de otomatik oluşturmuş olacaktır. => AuthorsId, BooksId olarak.
//class Book
//{
//    public int Id { get; set; }
//    public string BookName { get; set; }
//    public List<Author> Authors { get; set; }
//}

//class Author
//{
//    public int Id { get; set; }
//    public string AuthorName { get; set; } 
//    public List<Book> Books { get; set; }
//}
#endregion

#region Data Annotations
// Cross table manuel olarak oluşturulmak zorundadır
// Entity'leri oluşturduğumuz cross table entity si ile bire çok bir ilişki kurulmalı.
// Cross table'da composite primary key'i data annotations(attributes)lar ile manuel kuramıyoruz. Bunun için'de Fluent API'da çalışma yapmamız gerekiyor.
// Cross table'a karşılık bir entity modeli oluşturuyor isek eğer bunu context sınıfı içerisinde DbSet propery'si olarak bildirmek zorunda değiliz.
//class Book
//{
//    public int Id { get; set; }
//    public string BookName { get; set; }
//    public ICollection<BookAuthor> Authors { get; set; }
//}

//// Cross Table
//class BookAuthor
//{
//    [ForeignKey(nameof(Book))]
//    public int BId { get; set; } //BookId değilde bu şekilde isim verir isem ForeignKey olarak belirtmeliyim yoksa arka planda otomatik Book entity'ini temsil etmek için => BookId isimli bir alan daha oluşturur.

//    [ForeignKey(nameof(Author))]
//    public int AId { get; set; }
//    public Book Book { get; set; }
//    public Author Author { get; set; }
//}

//class Author
//{
//    public int Id { get; set; }
//    public string AuthorName { get; set; }
//    public ICollection<BookAuthor> Books { get; set; }
//}
#endregion

#region Fluent API
// Cross table manuel oluşturulmalı
// DbSet olarak eklenmesine lüzüm yok
// composite PK Haskey metodu ile kurulmalı!
class Book
{
    public int Id { get; set; }
    public string BookName { get; set; }
    public ICollection<BookAuthor> Authors { get; set; }
}

// Cross Table
class BookAuthor
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public Book Book { get; set; }
    public Author Author { get; set; }
}

class Author
{
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<BookAuthor> Books { get; set; }
}
#endregion

class ManyToManyDbContext : DbContext
{
    public DbSet<Book> Books { get; set;}
    public DbSet<Author> Authors { get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ManyToManyDb;Trusted_Connection=True;TrustServerCertificate=True");
    }

    /*
    //Data Annotations için
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // composite key verebilmek için.
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ky => new { ky.BId, ky.AId }); 
    }
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //composite primary key oldugunu belirtiyoruz
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.AuthorId, ba.BookId });

        // kitap - yazar için n - n
        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.Authors)
            .HasForeignKey(ba => ba.BookId); // foreign key 

        // yazar - kitap için n - n
        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(ba => ba.AuthorId); // foreign key
    }
}