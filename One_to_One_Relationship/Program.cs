// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

Console.WriteLine("Hello, World!");

OneToOneDbContext context = new();

#region Default Convention 
//{convention : ortak düşünce, toplantı; foreign: yabancı }
//Her iki entity'de navigation porpert'ler ile birbirlerini tekil referans ederek fiziksel bir ilişkinini olacağı ifade edilir. => public EmployeeAddress EmployeeAddress { get; set; }, public Employee Employee { get; set; } (Navigation'lar tür olarak entity alır.)
// One to One ilişki türünde dependent entity'in hangisi olduğunu default olarak belirleyebilmek pek kolay değildir. Bu durumda fiziksel olarak bir foreign key'e karşılık property/kolon tanımlayarak çözüm getirebiliyoruz. => public int EmployeeId { get; set; } //dependent Id 
// Böylece foreign key'e karşılık property tanımlayarak lüzumsuz bir kolon oluşturmuş oluyoruz. => public int EmployeeId { get; set; }

//class Employee
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public EmployeeAddress EmployeeAddress { get; set; }
//}

//class EmployeeAddress
//{
//    public int Id { get; set; }
//    public string Address { get; set; }
//    public int EmployeeId { get; set; } //dependent Id
//    public Employee Employee { get; set; }
//}
#endregion

#region Data Annotations
// {unique: benzersiz; annotation: dipnot, açıklama}
// Navigation property'ler tanımlanmalıdır. => public EmployeeAddress EmployeeAddress { get; set; }; public Employee Employee { get; set; }
// Foreign kolonun ismi default conventionun dışında bir column olacak ise eğer ForeiginKey attribuda ile bunu bildirebiliriz. => [ForeignKey(nameof(Employee))] public int C { get; set; }
// Foreign Key kolonu oluşturulmak zorunda değildir. => [Key, ForeignKey(nameof(Employee))] public int Id { get; set; }
// 1'e 1 ilişkide ekstradan foreign key kolonuna ihtiyaç olmıyacağından dolayı dependent entity'deki id column'unu hem foreignkey hem'de primary key olarak kullanmayı tercih ediyoruz ve bu duruma özen gösterilmelidir. : fazladan bir alan tahsis etmemek için. => [Key, ForeignKey(nameof(Employee))] public int Id { get; set; }
//class Employee
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public EmployeeAddress EmployeeAddress { get; set; }
//}

//class EmployeeAddress
//{
//    [Key, ForeignKey(nameof(Employee))] // Id hem primaryKey hem de ForeignKey olacaktır => hem birebir ilişkiyi garantiye aldık dependent belirttik hem'de ekstra bir foreignKey için ekstra bir satır oluşturmamız gerekmiyor.
//    public int Id { get; set; }
/*
[ForeignKey(nameof(Employee))] // ForeignKey("Employee") şeklinde'de olabilir ama nameof(Employee) diyerek hatalı yazım veya ileride entity isim değişmesi durumunda burayı unutmamızı engelliyoruz. : hata verir untursak otomatik
public int C { get; set; }
*/
//    public string Address { get; set; }
//    public Employee Employee { get; set; }
//}
#endregion

#region Fluent API
// { fluent: akıcı, düzgün }
// Navigation Propertler tanımlanmalı 
// Fluent Apı yönteminde entity'^ler arasındaki ilişki context sınıfı içerisinde onModelCreating fonksiyonu override edilerek metotlar aracılığıyla tasarlanması gerekmektedir. Yani tüm sorumluluk bu fonksiyon içerisindeki çalışmalardadır.
class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EmployeeAddress EmployeeAddress { get; set; }
}

class EmployeeAddress
{
    [Key, ForeignKey(nameof(Employee))] // Id hem primaryKey hem de ForeignKey olacaktır => hem birebir ilişkiyi garantiye aldık dependent belirttik hem'de ekstra bir foreignKey için ekstra bir satır oluşturmamız gerekmiyor.
    public int Id { get; set; }
    public string Address { get; set; }
    public Employee Employee { get; set; }
}
#endregion

// çalışma sırası olarak : Default Convention'a bakılır ilk başta ,
// daha sonra Data Annotations a bakılır
// en son da override eilen fonksiyon içinde Fluent API' ye bakılır 
// eğerki herhangi bir hata yok ise migration işlemleri başarılı bir şekilde gerçekleştirilir.
class OneToOneDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeAddress> EmployeAddresses { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=OneToOneDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
    // Model'ların(entty) veritabanında generate edilecek yapıları bu fonksiyon içerisinde konfigure edilir.
    // Fluent API yöntemi kullanılacağı zaman oluşturulur bu methot.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeAddress>()
            .HasKey(c => c.Id); // EmployeeAddress tablosunda'ki Id alanı'nın primarykey oldugunu belirtiyoruz. : normal'de ID alanı default olarak primary key'dir isminden dolayı ama biz aşşagıda ilgili alanı foreignKey olarak tanımladığım için => primary key özelliğinin kaybolmama'sı için burada belirtiyoruz primary key oldugunu

        // One to One ilişki kuruldu 
        modelBuilder.Entity<Employee>() // Employee entity'e gir
            .HasOne(c => c.EmployeeAddress) // EmployeeAddress entity ile : bire - bir ilişki kur ;; c => Employee
            .WithOne(c => c.Employee) // c: EmployeeAddress ; Employe ile bire bir ilişki kur 
            .HasForeignKey<EmployeeAddress>(c => c.Id); // EmployeeAddress tablosunda'ki Id alanını foreignkey yap.

    }
}