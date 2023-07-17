// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

Console.WriteLine("Hello, World!");

// {separate = ayrıştırma}

#region OnModelCreating
//  Genel anlamda veritabanı ile ilgili configurasyonel operasyonellerin dışında Entityler üzerinde configurasyonel çalışmalar yapmamızı sağlayan bir foksiyondur.
#endregion

#region IEntityTypeConfiguration<T> Arayüzü
// Entity bazlı yapılacak olan configurasyonları o entity'e özel harici bir dosya üzerinde yapmamızı sağlayan bir arayüzdür.
// harici bir dosyada configurasyonellerin yürütülmesi : merkezi bir yapılandırma noktası oluşturmamızı sağlamaktadır.
// harici bir dosyada configirasyonnelerin yürütülmesi entity sayısının fazla olduğu seneryaloarda yönetebilirliğpi arttıracak ve yapılandırma ile ilgili geliştiricinin yükünü azaltacaktır.
#endregion

#region ApplyConfiguration Metodu
// Bu method harici konfigurasyonel sınıflarımızı ef core'a bildirebilmek için kullandığımız bir methottur.
#endregion

#region ApplyConfigurationsFromAssembly Metodu
// Uygulama bazında oluşturulan harici konfigurasyonel sınıfların her birini onModelCreating möetghodunda applyConfiguration ile tek ytek bildirmek yerine bu sınıfların buşunduğu assembyl'i bildirerek IEntityTypeConfiuration arayüzünden türüyen tüm sınıfların ilgili entitye karşılık konfigurasyonel değer olarak baz almasını tek kalemde gerçekleştirmemizi sağlayan bir metottur.
#endregion

class Order
{
    public int OrderId { get; set; }
    public string Description { get; set; }
    public DateTime OrderDate { get; set; }
}

class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderId);
        builder.Property(p => p.Description).HasMaxLength(13);
        builder.Property(p => p.OrderDate).HasDefaultValueSql("GETDATE()");
    }
}

class SeparateDbContext : DbContext 
{ 
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SeparateDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}