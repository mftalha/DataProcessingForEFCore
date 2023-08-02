// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

Console.WriteLine("Hello, World!");
OwnedEntityTypeDbContext contxt = new();

#region Owned Entity Types Nedir ?
// EF Core entity sınıflarını parçalıyarak, properylerini kümesel olarak farklı sınıflarda barındırmamıza ve tüm bu sınıfları ilgili entity'de birleştirip bütünsel olarak çalışmamıza izin vermektedir.
// böylece bir entity, sahip olunan(owned) birden fazla alt sınıfın birleşmesi ile meydana gelebilmektedir.
#endregion

#region Owned Entity Types'ı neden kullanırız ?
// https://www.gencayyildiz.com/blog/entity-framework-core-owned-entities-and-table-splitting/

// Domain Driver Design(DDD) yaklaşımında Value Object'lere karşılık olarak Owned Entity Types'lar kullanılır!.
#endregion

#region Owned Entity Types nasul uygulanır?
// Normal bir entity'de farklı sınıfların referans edilmesi primary key vs gibi hatalara sebep verecektir çünkü direk bir sınıfın referans olarak alınması : ef core tarafından ilişkisel bir tasarım olarak algılanır. Bizlerin entity içerisindeki propertyleri kümesel olarak barındıran sınıfları o entity'nin bir parçası olarak bildirmemiz gerekmektedir.

#region OwnsOne Metodu
// onmodelcreating de
#endregion
#region Owned Attribute'u
// data attribute ile alt sınıfları işaretleme yöntemi : [Owned]
// alt sınıfların class ının üstüne bu tagı koyarız.
#endregion
#region IEntityTypeConfiguration<T> Arayüzü
// birden fazla yertde tagları mevcut
#endregion

#region OwnsMany Metodu
// OwnsMany Metodu, entity'nin farklı özelliklerine başka bir sınıftan ICollection türünden Navigation property aracılığıyla ilişkisel olarak erişebilmemizi sağlayan bir işleve sahiptir.
// Normalde Has ilişki olarak kurulabilecek bu ilişşkinin emel farkı Has ilişkisinin DbSet bağlantısı gerektirirken, OwnsMany metodu ise DbSet'e ihtiyaç duymaksınız gerçekleştirmemizi sağlamaktadır.

//var d = await contxt.Employees.ToListAsync();
//Console.WriteLine( );
#endregion
#endregion

#region Sınırlılıklar
// Herhangbi bir Owned Entity Type için db set propertysine ihtiyaç yoktur.
// OnModelCreating fonksiyonunda Entity<T> metodu ile owned entity type türünde bir sınıf üzerinde herhangi bir konfigürasyon gerçekleştirilemez!
// Owned Entity Type'ların kalıtımsal hiyerarşi desteği yoktur!
#endregion

class Order
{
    public string OrderDate { get; set; }
    public int Price { get; set; }
    // public Employee Employee { get; set; } // Owned oldugundan bu navigation koyulmuyor
}
class Employee
{
    public int Id { get; set; }
    //public string Name { get; set; }
    //public string MiddleName { get; set; }
    //public string LastName { get; set;}
    //public string StreetAddress { get; set; }
    //public string Location { get; set; }
    public bool IsActive { get; set; }

    public EmployeeName EmployeeName { get; set; }
    public Address Address { get; set; }    
    public ICollection<Order> Orders { get; set; }
}

class EmployeeName
{
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
}
//[Owned]
class Address
{
    public string StreetAddress { get; set; }
    public string Location { get; set; }
}
//[Owned]
class OwnedEntityTypeDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region OwnsOne Metodu
        //// alt sınıfları bildirme işlemini bu şekilde gerçekleştirebiliyoruz ef core'da
        //modelBuilder.Entity<Employee>().OwnsOne(e => e.EmployeeName,builder =>
        //{
        //    //EmployeeName_Name şeklinde db - column klasörü altında gözükür ismi bunun ismini bu şekilde değiştirebiliyoruz : bulundugu sınıf_kendi ismi gibi bir kuralı var default
        //    builder.Property(e => e.Name).HasColumnName("Name");
        //});
        //modelBuilder.Entity<Employee>().OwnsOne(e => e.Address);
        #endregion

        #region IEntityTypeConfiguration<T> Arayüzü
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        #endregion

        #region OwnsMany Metodu
        // Order class'ı nı empyoee ile ilişkisel yapıyoruz ama db set ile değil bu şekilde. : order tablosuna direk erişim olmıyaca : db de tablo olacak ama projeden order. diyemiyecez DbSet ile tanımlanamdığından : order'ın id si , ve navigation'ı yok : employee üzerinden erişim sağlıyabileceğiz order'a => yine sahip ilişkili bir ilişkili tablo olmuş oluyor.
        modelBuilder.Entity<Employee>().OwnsMany(e => e.Orders, builder =>
        {
            builder.WithOwner().HasForeignKey("OwnedEmployeeId");
            builder.Property<int>("Id");
            builder.HasKey("Id");
        });
        #endregion

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=OwnedEntityTypeDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}

#region IEntityTypeConfiguration<T> Arayüzü
class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.OwnsOne(e => e.EmployeeName, builder =>
        {
            //EmployeeName_Name şeklinde db - column klasörü altında gözükür ismi bunun ismini bu şekilde değiştirebiliyoruz : bulundugu sınıf_kendi ismi gibi bir kuralı var default
            builder.Property(e => e.Name).HasColumnName("Name");
        });
        builder.OwnsOne(e => e.Address);
    }
}
#endregion
