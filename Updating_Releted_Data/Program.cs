// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
UpdatingRelatedDbContext context = new();
//{ cross : geçmek, çapraz }
#region One to One ilişkisel senaryolarda veri güncelleme
#region Saving
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
//await context.SaveChangesAsync();
#endregion

#region 1. Durum | Esas tablodaki veriye bağımlı veriyi değiştirme
// Personeli Addresleri ile beraber alıyoruz.
//Person? person = await context.Persons
//    .Include(p => p.Address) //İNNER JOin ile address bilgisinide'de getirtiyoruz ilişkili oldugu tablodan=> navigation ile
//    .FirstOrDefaultAsync(p => p.Id == 1);

//context.Addresses.Remove(person.Address);

//person.Address = new()
//{
//    PersonAddress = "New Adress"
//};

//await context.SaveChangesAsync();
#endregion
#region 2. Durum | Bağımlı verilerin ilişkisel olduğu ana veriyi güncelleme 
// dependent veriyi kaldırıp tekrar ekleme yapıyoruz : primary key direkt olarak değiştirilemeyeceği için
//Address? address = await context.Addresses.FindAsync(1);
//address.Id = 2;
//await context.SaveChangesAsync(); // burada hata verecektir primary key colomn'u direk değiştirilemez

//ilgili addresi 1 id'li person'dan kaldırıp, 2'id li person'a dependent yaptık.
//Address? address = await context.Addresses.FindAsync(1);
//context.Addresses.Remove(address);
//await context.SaveChangesAsync();

//Person? person  = await context.Persons.FindAsync(2);
//address.Person = person;
//await context.Addresses.AddAsync(address);
//await context.SaveChangesAsync();

// principle tablosuna yeni bir kayıt olusturup("Hilmi") ilgili addresi o kayıt'a dependent ediyoruz
//Address? address = await context.Addresses.FindAsync(2);
//context.Addresses.Remove(address);
//await context.SaveChangesAsync();

//address.Person = new()
//{
//    Name = "Rıfkı"
//};
//await context.AddAsync(address);
//await context.SaveChangesAsync();

#endregion
#endregion

#region One to Many İlişkisel Senaryolarda veri güncelleme
#region Saving
//Blog blog = new()
//{
//    Name = "talhasatir.com Blog",
//    Posts = new List<Post>()
//    {
//        new(){ Title = "1. Post"},
//        new(){ Title = "2. Post"},
//        new(){ Title = "3. Post"}
//    }
//};
//await context.AddAsync(blog);
//await context.SaveChangesAsync();
#endregion

#region 1. Durum | Esas tablodaki veriye bağımlı verileri değiştirme
// bloglar'ı postlar ile alıyoruz : dependent postları almak için join yapısı kullanmalıyız.
//Blog? blog = await context.Blogs
//    .Include(b => b.Posts)
//    .FirstOrDefaultAsync(b => b.Id == 1);

//Post? deletedetPost = blog.Posts.FirstOrDefault(P => P.Id == 2);
//blog.Posts.Remove(deletedetPost);

//blog.Posts.Add(new() { Title = "4. Post" });
//blog.Posts.Add(new() { Title = "5. Post" });

//await context.SaveChangesAsync();
#endregion
#region 2. Durum | Bağımlı verilerin ilişkisel olduğu ana veriyi güncelleme

//yeni bir blog olustur ve ilgili post'u blog'a ver.
//Post post = await context.Posts.FindAsync(4);
//post.Blog = new()
//{
//    Name = "Blog 2"
//};
//await context.SaveChangesAsync();

//Post? post = await context.Posts.FindAsync(5);
//Blog? blog = await context.Blogs.FindAsync(2);
//post.Blog = blog;
//await context.SaveChangesAsync();

#endregion
#endregion

#region Many to Many ilişkisel senaryolarda veri güncelleme

#region Saving
//Book book1 = new() { BookName = "Book 1" };
//Book book2 = new() { BookName = "Book 2" };
//Book book3 = new() { BookName = "Book 3" };

//Author author1 = new() { AuthorName = "Author 1" };
//Author author2 = new() { AuthorName = "Author 2" };
//Author author3 = new() { AuthorName = "Author 3" };

//book1.Authors.Add(author1);
//book1.Authors.Add(author2);

//book2.Authors.Add(author1);
//book2.Authors.Add(author2);
//book2.Authors.Add(author3);

//book3.Authors.Add(author1);

//await context.AddAsync(book1);
//await context.AddAsync(book2);
//await context.AddAsync(book3);
//await context.SaveChangesAsync();
#endregion
#region Example 1

// book 1 ile kitap 3 ü ilişkilendir.
//Book? book = await context.Books.FindAsync(1);
//Author? author = await context.Authors.FindAsync(3);
//book.Authors.Add(author);
//await context.SaveChangesAsync();

// 3 id li yazarın sadece 1 id'li kitaba bağlantısı olsun diğerlerini koparalım
// BookAuthor tablosu cross tablodur ve dependent tabloda o'dur : dependent tablodan silme işlemi gerçekleştiriliyor.
//Author? author = await context.Authors
//    .Include(a => a.Books)
//    .FirstOrDefaultAsync(a => a.Id == 3);

//foreach(var book  in author.Books)
//{
//    if (book.Id != 1)
//        author.Books.Remove(book);
//}
//await context.SaveChangesAsync();

#endregion
#region Example 2

// book 2'nin : 1 id li yazarla ilişkiiy kes 3 id'li yazarla ilişki bağla , 4 id li yazar ekle ve onunla ilişki kur
Book? book = await context.Books
    .Include(b => b.Authors)
    .FirstOrDefaultAsync(b => b.Id == 2);

//1 id li yazarla ilişkiiy kes
Author author3 = await context.Authors.FindAsync(1);
book.Authors.Remove(author3);


// 3 id'li yazarla ilişki bağla
Author? author1 = await context.Authors.FindAsync(3);
book.Authors.Add(author1);

//4 id li yazar ekle ve onunla ilişki kur
book.Authors.Add(new() { AuthorName = "Author 4" });

await context.SaveChangesAsync();


#endregion
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
        Posts = new HashSet<Post>(); // HashSet(unic bir yapılanma vardır, list'e göre daha az maliyeti vardır,yüksek performans çalışır), List'de kullanılabilir // Nesne referansı üzerinden ekleme işlemi için burası tanımlı olmalıdır.
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; }
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
/*
class BookAuthor
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public Book Book { get; set; }
    public Author Author { get; set; }
}
*/
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

class UpdatingRelatedDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }    
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=UpdatingRelatedDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id);
 
        /*
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.Authors)
            .HasForeignKey(ba => ba.BookId);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(ba => ba.AuthorId);
        */
    }
}