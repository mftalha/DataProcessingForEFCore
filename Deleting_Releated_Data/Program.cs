// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
DeletingReleatedDbContext context = new();
#region One to One İlişkisel Senaryolarda veri silme

// 1 id li personelin dependent addresini al ve sil o veriyi : ilişkisel veri silme
//Person? person = await context.Persons
//    .Include(p => p.Address)
//    .FirstOrDefaultAsync(p => p.Id == 1);

//if(person != null)
//{
//    context.Adresses.Remove(person.Address);
//    await context.SaveChangesAsync();
//}

#endregion

#region One to Many İlişkisel Senaryolarda veri silme
// 2 id li post'u sil
//Blog? blog = await context.Blogs
//    .Include(b => b.Posts)
//    .FirstOrDefaultAsync (b => b.Id == 1);

//Post? post = blog.Posts.FirstOrDefault(p => p.Id == 2);

//context.Posts.Remove(post);
//await context.SaveChangesAsync();
#endregion

#region Many to Many ilişkisel senaryolarda veri silme
// 1 id sine sahip kitapda 2 id li yazar ile bağlantısını sil => cross table olan  bookauthor tablosundan silme işlemini gerçekleştirir.
//Book? book = await context.Books
//    .Include(b => b.Authors)
//    .FirstOrDefaultAsync(b => b.Id == 1);

//Author? author = book.Authors.FirstOrDefault(a => a.Id == 2);
//if(author != null)
//{
//    book.Authors.Remove(author);
//    await context.SaveChangesAsync();
//}

#endregion

#region Cascade Delete
// { Cascade : Çağlayan, kademeli ;; Restrict : kısıtlamak }
// bu davranış modelleri fluent api ile configure edilebilmektedir.
// coka cok ilişkilerde sadece cascade yaklaşımını kullanabiliyoruz. : ef core diğerlerine izin vermez
#region Cascade 
// esas tablodan silinen veri ile karşı/bağımlı tabloda bulunan ilişkili verilerin silinmesini sağlar. 

//Blog? blog = await context.Blogs.FindAsync(1);
//context.Blogs.Remove(blog);
//await context.SaveChangesAsync();
#endregion

#region SetNull
// esas tablodan silinen veri ile karşı/bağımlı tabloda bulunan ilişkili verilere null değeri atanmasını sağlar.
//  One to One senaryolarda eğer ki primary key ve foreign key kolonları aynı ise o zaman SetNull davranışını kullanamayız : primary key colomn'u null yapılamıyacağı için.

#endregion

#region Restrict
// esas tablodan herhangi bir veri silinmeye çalışıldığında o veriye karşılık pedendent table'ce ilişkisel veri veya ilişkisel veriler var ise bu sefer bu silme işlmeinin engellenemsini sağlar.
#endregion

Blog? blog = await context.Blogs.FindAsync(2);
context.Blogs.Remove(blog);
await context.SaveChangesAsync();
#endregion

#region Saving Data
//Person person = new()
//{
//    Name = "Talha",
//    Address = new()
//    {
//        PersonAddress = "Kepez/Antalya"
//    }
//};

//Person person2 = new()
//{
//    Name = "Hilmi"
//};

//await context.AddAsync(person);
//await context.AddAsync(person2);

//Blog blog = new()
//{
//    Name = "Talhasatir.com Blog",
//    Posts = new List<Post>
//    {
//        new(){ Title = "1. Post" },
//        new(){ Title = "2. Post" },
//        new(){ Title = "3. Post" },
//    }
//};

//await context.Blogs.AddAsync(blog);

//Book book1 = new() { BookName = "1. Kitap" };
//Book book2 = new() { BookName = "2. Kitap" };
//Book book3 = new() { BookName = "3. Kitap" };

//Author author1 = new() { AuthorName = "1. Yazar" };
//Author author2 = new() { AuthorName = "2. Yazar" };
//Author author3 = new() { AuthorName = "3. Yazar" };

//book1.Authors.Add(author1);
//book1.Authors.Add(author2);

//book2.Authors.Add(author1);
//book2.Authors.Add(author2);
//book2.Authors.Add(author3);

//book3.Authors.Add(author3);

//await context.AddAsync(book1);
//await context.AddAsync(book2);
//await context.AddAsync(book3);
//await context.SaveChangesAsync();
#endregion

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Address Address { get; set; }
}
class Address
{
    public int Id { get; set; }
    public string PersonAddress { get; set; }

    public Person Person { get; set; }
}
class Blog
{
    public Blog()
    {
        Posts = new HashSet<Post>();
    }
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    public int? BlogId { get; set; }
    public string Title { get; set; }

    public Blog Blog { get; set; }
}
class Book
{
    public Book()
    {
        Authors = new HashSet<Author>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }

    public ICollection<Author> Authors { get; set; }
}
class Author
{
    public Author()
    {
        Books = new HashSet<Book>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }

    public ICollection<Book> Books { get; set; }
}

class DeletingReleatedDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Adresses { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DeletingReleatedDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
            //.OnDelete(DeleteBehavior.Cascade); //default olarak Cascade vardır

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .IsRequired(false) //IsRequired(false) : ilgili foreign column'u gerekli değil diye bildiriyruz.
            //.OnDelete(DeleteBehavior.SetNull);
            .OnDelete(DeleteBehavior.Restrict);

         
    }
}