// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

Console.WriteLine("Hello, World!");
BackingFieldDbContext context = new();
#region AddDataForDbTest
/*
for(int i=1; i<=100; i++)
{
    await context.Persons.AddAsync(new()
    {
        Name = $"Person {i}",
        Department = $"Department {i}"
    });
}
context.SaveChanges();
*/
#endregion

Person person = await context.Persons.FindAsync(1);
Console.WriteLine("");

#region Backing Fields
// Tablo içerisineki kolonların, entity classs ları içerisinde propert'ler ile değil fields ile temsil etmemizi sağlayan bir özelliktir.
#endregion

#region Nerede kullanrız Backing Fields
//bazen slug yazarız (seo için id yerine link gibi kullanılır) ve slug product isminden alınır veya name lastname bazen birleştirerek fullname yazmak isteriz o zaman backing field  kullanırız,

// Özellikle Blazor'da çok işe yarayabilir. Orada c# ile kodları yazıyoruz js yerine. Her componentin içinde private olarak tanımlanmış propertyler var. Elbette bunları başka sınıftan da çekebiliyoruz ama konumuz o değil. Diyelim ki private olarak tanımladığım propertylerden birisine başka bir sınıfta ihtiyaç doğdu. Ama başka yerde kullanmayacağım. Şimdi gidip de bunu public yapıp her yere açmanın manası yok. Sadece 1 tane farklı classta kullanacağım. İşte bu durumda o property'i kapsülleriz. Veya diyelim ki adamın yaşını yazmasını, veya bir şeyin fiyatını yazmasını istiyorum. Ama yazan arkadaş -20 yazmış diyelim ki. kapsülleme sayesinde bunu 20 olarak dönüştürebiliyoruz. Veya yazılan string değeri veritabanına her zaman küçük harflere dönüştürüp kaydetmesini istiyorsam gene kapsülleme kullanabiliriz sanırım. ef core tarafında pek lazım olmayacak gibi görünse de blazor componentlerinde bir şekilde ihtiyaç doğabilir.
#endregion

#region BackingField Attributes
/*
Person person2 = new()
{
    name = "Person 103", // [BackingField(nameof(name))] : dedigimizden Name yerine bu field'e atama yapıyoruz
    Department = "Department 103"
};

await context.Persons.AddAsync(person2);
await context.SaveChangesAsync();
*/
/*
class Person
{
    public int Id { get; set; }
    public string name;
    //public string Name { get => name.Substring(0,3); set => name = value.Substring(0,3); } //kapsülleme yapıyoruz
    [BackingField(nameof(name))] // bu şekilde Name property'sine gelen herşeyi name fied'i ile temsil et diyoruz
    public string Name { get; set; }
    public string Department { get; set; }
}
*/
#endregion

#region HasField Fluent API
// Fluent API'DA HasField metodu BackingField özelliğine karşılık gelmektedir.
//class Person
//{
//    public int Id { get; set; }
//    public string name;
//    public string Name { get; set; }
//    public string Department { get; set; }
//}
#endregion

#region Field and property Access
// Ef Core sorgulama sürecinde entity içerisindeki propertyleri ya da field'ları kullanıp kullanmıyacağının davranışını bizlere belirtmektedir.
// Ef Core, hiçbir ayarlama yok ise varsayılan olarak propertlwer üzerinden verileri işler, eğerki backing field bildiriliyor ise field üzerinden işler yok eğer backing field bildirildiği halde davranış belirtiliyor ise : ne belirtilmiş ise ona göre işlemeyi devam ettirir.

// UsePropertAccessMode üzerinden davranış modellemesi gerçekleştirilebilir.
#endregion

#region Field-only Properties
//Entity'lerde değerleri almak için property'ler yerine metotların kullanıldığı veya belirli alanların hiç gösterilmemesi gereksiği durumlarda(örneğin primary key kolonu) kullanılabilir.
class Person
{
    public int Id { get; set; }
    public string name;
    //public string Name { get; set; }
    public string Department { get; set; }
    public string GetName()
        => name;
    public string SetName(string value)
        => this.name = value;
}

#endregion

class BackingFieldDbContext : DbContext 
{ 
    public DbSet<Person> Persons { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=Localhost\\SQLEXPRESS;Database=BackingFieldDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* For HasField Fluent API
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Name)
        //    .HasField(nameof(Person.name))
        //    .UsePropertyAccessMode(PropertyAccessMode.Property);

        //Field => veri erişim süreçlerinde sadece field'ların kullanılmasını söyler : eğer fiel'ın kullanılamayacağı durum söz konusu olur ise bir exception fırlatır.,
        //FieldDuringConstruction => veri erişim süreçlerinde ilgili entity'den bir nesne oluşturulma süreçlerin'de field'ların kullanılması söyler.
        //Property => Veri erişim sürecinde sadece property'in kullanılmasını söyler. Eğer porpertnin kullanılamıyacağı durum söz konusu ise (read-only, write-only) bir exception fırlatır.
        //PreferField => 
        //PreferFieldDuringConstruction
        //PreferProperty
        */

        // For Field-only Properties
        // name alanı Person entitisinde bir alana karşılık gelmektedir diye belirtiyoruz.
        modelBuilder.Entity<Person>()
            .Property(nameof(Person.name)); 
    }
}         