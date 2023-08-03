// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

Console.WriteLine("Hello, World!");
ValueConversionDbContext context = new();

#region Value Conversions Nedir
//EF Core üzerinden veri tabanı ile yapılan sorgulama süreçlerinde veriler üzerinde dönüşümler yapmamızı sağlayan bir özelliktir.
// Select sorguları sürecinde gelecek olan veriler üzerinde dönüşüm yaopabilirz
// Update yahut Insert sorgularında da yazılım sürecinde veritabanına gönderdiğimiz veriler üzerindede dönüşümler yapabilir ve böylece fiziksel manipilasyonlar yapabiliriz.
#endregion
#region  Value Converter kullanımı Nasıldır?
// Value Convertions özelliğini ef core'da ki value conmverter yapıları tarafından uygulayabilmekteyiz.

#region HasConversion
// HasConversion Fonksiyonu, en sade haliyle EF Core üzerinden value converter özelliği gören bir fonksiyondur.
//var persons = await context.Persons.ToListAsync();
//Console.WriteLine();

#endregion
#endregion
#region Enum Değerler ile value converter kullanımı
// Enum türünde tutulan propertylerin veritabanındaki karşılıkları int olacak şeikilde aktarımı gerçekleştirilmektedir. Value Converter sayesinde enum tründe olan parametrelerini dönüşümler istediğimiz türlere sağlıyarak hem ilgili colonun değerini o türde ayarlıyabilri hemde enum üzerinden çalışma sürecinde fiziksel süreçte sağlıyabilriiz.

//var person = new Person() { Name = "Rakıf", Gender2 = Gender.Male, Gender = "M" };
//await context.Persons.AddAsync(person);
//await context.SaveChangesAsync();
//var _person = await context.Persons.FindAsync(person.Id);
//Console.WriteLine();
#endregion
#region ValueConverter Sınıfı
// ValıueConverter sınıfı verisel dönüşümlerdeki çalışmaları/sorumlululşaro üstlenebilecek bir sınıftır
// yani bu sınıfın instance ile HasConvertion fonksiyonunun yapısal çalışmaları üstlenebilir ve direkt bu instane ilgili fonksiyona vererek dönüşümsel çalışmalarımızı gerçekleştirebiliriz.

//var _person = await context.Persons.FindAsync(10);
//Console.WriteLine();
#endregion
#region Custom ValueConverter Sınıfı
// EF Core'da verisel dönüşümler için custom olarak vonverter sınıfları üretebilmekteyiz. Bunun için tek yapılması gereken custom sınıfını ValueConverter sınıfından miras almasını sağlamaktır.

//var _person = await context.Persons.FindAsync(10);
//Console.WriteLine();



#endregion
#region Built-in Converters yapıları
// EF Core basit dönüşümler için kendi bnünyesinde yerlerşik convert sınıfları barındırmaktadır.

#region BoolToZeroConverter
// bool olan verinin int olarak tutulmasını sağlar

#endregion
#region BoolToStringConverter

#endregion
#region BoolToTwoValuesConverter

#endregion
#endregion
#region İlkel Koleksiyonların Serilazasyonu
// içerinde ilkel türlerden oluşturulmuş koleksiyonları barındıran modelleri migrate etmeye çalıştığımızda hata ile karşılaşmaktayız. Bu hatadan kurtulmak ve ilgili veriye koleksiyondaki verileri serileze ederek işeleyebilmek içni bu koleksiyonu normal metinsel değerlere dönüştürmemize fırsat veren bir conversion operasyonu gerçekleştirebiliriz.

//var person = new Person() { Name = "Filanca", Gender = "M", Gender2 = Gender.Male, Married = true, Titles = new() { "A", "B", "C", "D" } };
//await context.Persons.AddAsync(person);
//await context.SaveChangesAsync();

//var _person = await context.Persons.FindAsync(person.Id);
//Console.WriteLine();
#endregion
#region .Net6 - Value Converter For Nullable Fields
// .Net 6'dan önce value converter'lar null değerlerin dönüşünü desteklememekteydi. .NET 6 ile artık Null değerlerde dönüştürülebilmektedir.
#endregion

#region Custom ValueConverter Sınıfı
public class GenderConverter : ValueConverter<Gender, string>
{
    public GenderConverter() : base(
        //Insert - Update
        g => g.ToString()
        ,
        //Select
        g => (Gender)Enum.Parse(typeof(Gender), g)
        )
    {
    }
}
#endregion


public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; } 
    public Gender Gender2 { get; set; }
    public bool Married { get; set; }
    public List<string>? Titles { get; set; }
}
public enum Gender
{
    Male,
    Female
}

public class ValueConversionDbContext : DbContext
{
    public DbSet<Person> Persons { get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        #region Value Converter kullanımı Nasıldır -HasConversion
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Gender)
        //    .HasConversion(
        //    // Insert - Update
        //    g => g.ToUpper() // büyük olarak gönder db ye
        //    ,
        //    //Select
        //    g => g == "M" ? "Male" : "Female"
        //    );
        #endregion

        #region Enum Değerler ile value converter kullanımı
        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Gender2)
        //    .HasConversion(
        //    // Insert - Update
        //    //g => g.ToUpper() // büyük olarak gönder db ye
        //    //g => (int)g // int olarak gönder enum değerini
        //    g => g.ToString()
        //    ,
        //    //Select
        //    g => (Gender)Enum.Parse(typeof(Gender), g)
        //    );
        #endregion
        #region ValueConverter Sınıfı

        // bu kullanım ike model creater'ı şişirmeden çalışamlarımızı gerçekleştirebiliriz.
        // farklı bir class'da oluşturuz bunları sadece onmodelcreatingde ismini veririz .HasConversion() içinde.
        //ValueConverter<Gender, string> converter = new(
        //    // Insert - Update
        //    g => g.ToString()
        //    ,
        //    //Select
        //    g => (Gender)Enum.Parse(typeof(Gender), g)
        //    );


        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Gender2)
        //    .HasConversion(converter);
        #endregion
        #region Custom ValueConverter Sınıfı
            modelBuilder.Entity<Person>()
            .Property(p => p.Gender2)
            .HasConversion<GenderConverter>();
        #endregion

        #region Built-in Converters yapıları
        // EF Core basit dönüşümler için kendi bnünyesinde yerlerşik convert sınıfları barındırmaktadır.

        #region BoolToZeroConverter
        // bool olan verinin int olarak tutulmasını sağlar

        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Married)
        //    .HasConversion<BoolToZeroOneConverter<int>>();

        // aşşağıdaki gibi int türünü belirtirsekde veritabanında aynı şekilde tutulacaktır

        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Married)
        //    .HasConversion<int>();
        #endregion
        #region BoolToStringConverter

        //BoolToStringConverter converter = new("Bekar", "Evli");

        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Married)
        //    .HasConversion(converter);
        #endregion
        #region BoolToTwoValuesConverter

        //BoolToTwoValuesConverter<char> converter = new('B', 'E');

        //modelBuilder.Entity<Person>()
        //    .Property(p => p.Married)
        //    .HasConversion(converter);
        #endregion
        #endregion

        #region İlkel Koleksiyonların Serilazasyonu

        modelBuilder.Entity<Person>()
            .Property(p => p.Titles)
            .HasConversion(
            // Insert - Update
            t => JsonSerializer.Serialize(t, (JsonSerializerOptions)null)
            ,
            //Select
            t => JsonSerializer.Deserialize<List<string>>(t, (JsonSerializerOptions)null)
            );

        #endregion
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ValueConversionDb;User Id=SA; Password=123!;TrustServerCertificate=True");
    }
}



//Diğer built-in converters yapılarını aşağıdaki linkten gözlemleyebilirsiniz.
//https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#built-in-converters