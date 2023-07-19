// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
SequenceDbContext context = new();

//  Sequence : bir veya daha fazla tabloya etki edecek harici bir int(sadece int biliyoum) degisken olsuturup - bu degisken üzerinden ardışık  - unique bir deger sunma işleminin gerçekleşmesidir : her çağrıldıgında nasıl bir artış ve başlangıç değeri verildi ise ona göre artacaktır otomatik.

// { sequence : sekans, dizi, sıra}
// ilgili tabloda primary key'e sequnce ataması yapınca otomatik olarak identify özelliğini kapatmış oluyoruz : ilgili column'un

#region Sequence nedir?
// Veritabanında benzersiz ve ardışık sayısal değerler üreten veritabanı nesnesidir.
// Sequence herhangi bir tablonun özelliği değildir. Veritabanı nesnesidir. Birden fazla tablo tarafından kullanılabilir.
#endregion

#region Sequence Tanımlama
//Sequence'ler üzerinden değer oluştuturken veritabanına özgü çalışma yapılması zorunludur. SQL Server'a özel yazılan Sequence tanımı oracle için hata oluşturabilir. mssql için => NEXT VALUE FOR EC_Sequence

#region HasSequence

#endregion
#region HasDefaultValueSql

#endregion
#endregion

//await context.Customers.AddAsync(new() { Name = "Ali" });

//await context.Employees.AddAsync(new() { Name = "Talha", Surname = "Satır", Salary = 1000 });
//await context.Employees.AddAsync(new() { Name = "Mustafa", Surname = "ucar", Salary = 1000 });
//await context.Employees.AddAsync(new() { Name = "Mehmet", Surname = "kacar", Salary = 1000 });

//await context.SaveChangesAsync();

#region Sequence Yapılandırması

#region StartsAt

#endregion
#region IncrementsBy

#endregion
#endregion

#region Sequence ile Identity farkı
// Sequence bir veritabanı nesnesi iken Identity ise tabloların özellikleridir.
// yani sequence herhangi bir tabloya bağlı değildir.
// Idendity bir sonraki değeri diskten alırken Sequence ise Ram'den alır. Bu yüzden önemli ölçüde Identity'e nazaran daha hızlı, performanslı ve az maliyetlidir.
#endregion

class Employee
{
    public int Id { get; set; } 
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
}

class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

class SequenceDbContext : DbContext
{
    public DbSet<Employee>Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SequenceDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Sequence Tanımlama

        #region HasSequence
        // bir sequence tanımı yaptık
        //modelBuilder.HasSequence("EC_Sequence");
        #endregion

        #region HasDefaultValueSql
        // HasSequenceile tanımladığım sequenceyi ilgili kolonlarda kullanıyorum.

        //modelBuilder.Entity<Employee>()
        //    .Property(e => e.Id)
        //    .HasDefaultValueSql("NEXT VALUE FOR EC_Sequence");

        //modelBuilder.Entity<Customer>()
        //    .Property(c => c.Id)
        //    .HasDefaultValueSql("NEXT VALUE FOR EC_Sequence");
        #endregion

        #endregion

        #region Sequence Yapılandırması
        modelBuilder.HasSequence("EC_Sequence")
            .StartsAt(100) //100 den başlasın
            .IncrementsBy(5); // 5 er 5 er artsın

        modelBuilder.Entity<Employee>()
            .Property(e => e.Id)
            .HasDefaultValueSql("NEXT VALUE FOR EC_Sequence");

        modelBuilder.Entity<Customer>()
            .Property(c => c.Id)
            .HasDefaultValueSql("NEXT VALUE FOR EC_Sequence");
        #endregion
    }
}