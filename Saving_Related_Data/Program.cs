// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

ApplicationDbContext context = new();

#region One to One İlişkisel Senaryolarda veri ekleme
//{principal : müdür, ana}
#region 1. Yöntem -> Principal Entity üzerinden Depenent Entity verisi ekleme
//Person person = new();
//person.Name = "Talha";
//person.Address = new() { PersonAddress = "Kepez/Antalya" };

//await context.AddAsync(person);
//await context.SaveChangesAsync();
#endregion

// Principal entity => 1 e 1 ilişkide dependent entity olmayan entity'dir yani Personel entity'sidir : Adress entity'de Id alanı Personele Entity'sine foreign key'di : eğerki Id'yi foreign olarak vermesek yeni bir alan olusturulacak'tı default olarak PersonelId diye => yani diğer entity'e bağlı olmıyan entity : principal entity'dir
// Dependent entity = diğer entity'e bağlı olan entity'dir => Adress entity'i
// Eğer ki principal entity üzerinden ekleme geçekleştiriliyor ise dependent entity nesnesi verilmek zorunda değildir. Ama dependent entity üzerinden ekleme işlemi gerçekleştiriliyor ise burada princeple entity nesnesine ihtiyacımız vardır(zaruridir.).

#region 2. Yöntem -> Dependent entity üzerinden principal entity verisi ekleme
//Address adress = new()
//{
//    PersonAddress = "Bahçelievler/İstanbul",
//    Person = new() { Name = "Turgay" }
//};

//await context.AddAsync(adress);
//await context.SaveChangesAsync();

#endregion
//class Person
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public Address Address { get; set; }
//}
//class Address
//{
//    public int Id { get; set; }
//    public string PersonAddress { get; set; }
//    public Person Person { get; set; }
//}
//class ApplicationDbContext : DbContext
//{
//    public DbSet<Person> Persons { get; set;}
//    public DbSet<Address> Addresss { get; set;}
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True");
//    }
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Address>()
//            .HasOne(a => a.Person)
//            .WithOne(p => p.Address)
//            .HasForeignKey<Address>(a => a.Id);
//    }
//}
#endregion
#region One to Many ilişkisel senaryolarda veri ekleme

// 1. ve 2. yöntemler hiç olmayan verilerin ilişkisel olarak eklenmesini sağlarken, bu 3. yöntem önceden eklenmiş olan bir principal entity verisi ile yeni dependent entitylerin ilişkisel olarak eşleştirilmesini sağlamaktadır.

#region 1. Yöntem -> Principal entity üzerinden dependent entity verisi ekleme
#region Nesne referansı üzerinden ekleme
//Blog blog = new() { Name = "Talhasatir.com Blog" };
//blog.Posts.Add(new() { Title = "Post 1" });
//blog.Posts.Add(new() { Title = "Post 2" });
//blog.Posts.Add(new() { Title = "Post 3" });

//await context.AddAsync(blog);
//await context.SaveChangesAsync();

#endregion
#region Object Initializer üzerinden ekleme
//Blog blog2 = new()
//{
//    Name = "A Blog",
//    Posts = new HashSet<Post>() { new() { Title = "Post 4" }, new() { Title = "Post 5"} }
//};
//await context.AddAsync(blog2);
//await context.SaveChangesAsync();
#endregion
#endregion
#region 2. Yöntem -> Dependent entity üzerinden principal entity verisi ekleme
// bu yöntem yanlıştır kullanılmamlıdır. => bu işlem sonucunda bir tane dependen entity verisi ekleyebiliyoruz bu yöntemde.
//Post post = new()
//{
//    Title = "Post 6",
//    Blog = new() { Name = "B Blog" }
//};

//await context.AddAsync(post);
//await context.SaveChangesAsync();
#endregion
#region 3. Yöntem -> Foreign key kolonu üzerinden veri ekleme
//Post post = new()
//{
//    Title = "Post 7",
//    BlogId = 1    
//};

//await context.AddAsync(post);
//await context.SaveChangesAsync();
#endregion

// Posts = new HashSet<Post>() yapısı neden olusturuldu => nesne tabanlı programlamada herhangi bir referans null ise bu referans üzerinden bir mumber'a erişmek => null referance exception hatası verir; bu yüzden biz blog nesnesi üzerinden Posts'a erişmek için biz bu Posts'ın null olmadığından emin olmamız gerekiyor : o yüzden biz Posts'u Blog class'ının constracter'da new'liyoruz. : böylece null olmıyacağından null referance exception hatasına düşmeyeceğizdir. 
//class Blog
//{
//    public Blog() 
//    {
//        Posts = new HashSet<Post>(); // HashSet(unic bir yapılanma vardır, list'e göre daha az maliyeti vardır,yüksek performans çalışır), List'de kullanılabilir // Nesne referansı üzerinden ekleme işlemi için burası tanımlı olmalıdır.
//    }
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public ICollection<Post> Posts { get; set; }
//}
//class Post
//{
//    public int Id { get; set; }
//    public int BlogId { get; set; }
//    public string Title { get; set; }
//    public Blog Blog { get; set; }
//}

//public class ApplicationDbContext : DbContext
//{
//    DbSet<Blog> Blogs { get; set; }
//    DbSet<Post> Posts { get; set; }
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True");
//    }
//}

#endregion
#region Many to Many İlişkisel senaryolarda veri ekleme
#region 1. yöntem
// n t n ilişkisi eğer ki default convention üzerinden tanımlanmış ise kullanılan bir yöntemdir

//Book book = new()
//{
//    BookName = "A Kitabı",
//    Authors = new HashSet<Author>()
//    {
//        new() { AuthorName = "Hilmi"},
//        new() { AuthorName = "Ayşe"},
//        new() { AuthorName = "Fatma"},
//    }
//};
//await context.AddAsync(book);
//await context.SaveChangesAsync();

//class Book
//{
//    public Book()
//    {
//        Authors = new HashSet<Author>();
//    }
//    public int Id { get; set; }
//    public string BookName { get; set; }
//    public ICollection<Author> Authors { get; set; }
//}

//class Author
//{
//    public Author()
//    {
//        Books = new HashSet<Book>();
//    }
//    public int Id { get; set; }
//    public string AuthorName { get; set; }
//    public ICollection<Book> Books { get; set; }
//}

#endregion
#region 2. yöntem
// n t n ilişkisi eğer ki fluent api ile tanımlanmış ise kullanılan bir yöntemdir
// burada mustafa yazarını hem var olan 1 id'li kitap ile eşleştirdim ; hemde yeni bir kitap oluşturup bu kitap ile eşleştirmesini gerçekleştirdim : 2 sinide aynı anda yapabiliyoruz.
//Author author = new()
//{
//    AuthorName = "Mustafa",
//    Books = new HashSet<BookAuthor>()
//    {
//        new() {BookId = 1},
//        new() {Book = new() { BookName = "B Kitabı"}}
//    }
//};
//await context.AddAsync(author);
//await context.SaveChangesAsync();

class Book
{
    public Book()
    {
        Authors = new HashSet<BookAuthor>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }
    public ICollection<BookAuthor> Authors { get; set; }
}
class BookAuthor
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public Book Book { get; set; }
    public Author Author { get; set; }
}
class Author
{
    public Author()
    {
        Books = new HashSet<BookAuthor>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<BookAuthor> Books { get; set; }
}
#endregion

class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
    
}
#endregion