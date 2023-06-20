// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

#region Relationships(İlişkiler) Terimler

#region Principal Entity(Asıl Entity)
// kendi başına var olabilen tabloyu modelleyen entity'e denir.
// Departmanlar tablosunu modelleyen 'Departman' entity'sidir. // herhangi bir tablo olmadan tek başına var olabilen => çalışan tablosu olamaz : çünkü her çalışanın bir departmanı vardır. => departman ıd tutar : departman tablosunda karşılıgı olan gibi.
#endregion

#region Dependent Entity(Bağımlı Entity)
// kendi başına var olamayan, bir başka tabloya bağımlı(ilişkisel olarak bağımlı) olan tabloyu modelleyen entiy'e denir.
// Calisanlar tablosunu modelleyen 'Calisan' entity'sidir. Çalışanlar => Departmanlar tablosuna bağlıdır.
#endregion

#region Foreign Key
// Principal Entity ile Dependent Entity arasındaki ilişkiyi sağlayan key'dir
// Calisanlar tablosunda'ki DepartmanId alanıdır.

// Dependent Entity'de tanımlanır(Employees table)
// Principal Entity'de ki Principal Key'i tutar.
#endregion

#region Principal Key
// Principle entity'deki id'nin kendisidir.
// Principle entity'nin kimliği olan kolonu ifade eder
// Departments tablosundaki Id'dir
#endregion


class Employee
{
    public int Id { get; set; }
    public string EmployeeName { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; } // her çalışanın 1 tane departmanı olabilir
}

class Department
{
    public int Id { get; set; }
    public string DepartmentName { get; set; }
    public ICollection<Employee> Employees { get; set; } // bir departmanın birden fazla çalışanı olabilir.
}


#endregion

#region Navigation Property Nedir?
// İlişkisel tablolar arasındaki fiziksel erişimi entiy class'ları üzerinden sağlayan property'lerdir.
// Bir propert'nin navigation property olabilmesi için kesinlikle entity türünden olması gerekiyor.
// Navigation propert'ler entity lerdeki takımlarına göre n*n yahut 1*n ilişki türlerini ifade etmekteridler. : sonraki derslerde uygulayacaz bunu
/* Navigation Propert'lerdir.
 - public Department Department { get; set; }
 - public ICollection<Employee> Employees { get; set; }
*/
#endregion

#region İlişki Türleri

#region One to One
// Çalışan ile adresi arasındaki ilişki.
// public Department Department { get; set; } => 2 tabloda böyle temsil edecek birbirini ama detayları varmış.
#endregion

#region One to Many
// Çalışan ile Departman arasındaki ilişki (bir çalışan birden fazla departman'da olabilir)
#endregion

#region Many to Many
// Çalışanlar ile projeler arasında'ki ilişki
// public ICollection<Employee> Employees { get; set; } => 2 entity'de bu şekilde kullanılır.
#endregion

#endregion

#region Entity Framework Core'da İlişki Yapılandırma Yöntemleri
//  Default Conventions, Data Annotations Attributes, Fluent API => birbirlerine alternatif'lerdir => durumdan duruma daha iyi daha kötü olabilirler.
#region Default Conventions
// Varsayılan entity kurallarını kullanarak yapılan ilişki yapılandırma yöntemleridir.
// Navigation property'leri kullanarak ilişki şablonlarını çıkarmaktadır.
// forenky olması için ilişkili tablo ismi + Id dir default'u => burdada bu belirtiliyor
#endregion

#region Data Annotations Attributes
// Entity'nin niteliklerine göre ince ayarlar yapmamızı sağlayan attribute'lardır. [Key], [ForeignKey]
// devault değilde ben farklı özellikler vereeksem bu yapıları kullanmalıyım.
#endregion

#region Fluent API
// Entity modellerindeki ilişkileri yapılandırırken daha detaylı çalışmamızı sağlayan yöntemdir.

#region HasOne
// ilgili entity'in ilişkisel entity'e 1*1 veya 1*n oladcak şekilde ilişkisini yapılandırmaya başlayan methot'dur.
// 1 ile başlıyor ise ilişki.
#endregion

#region HasMany
// İlgili entity'in ilişkisel entity'e n*1 veya n*n  olacak şekilde ilişkisel yapılandırmayı başlayan metottur.
// başladığı nokta çok kısmı olacak.
#endregion

#region WithOne
// HasOne veya HasMany'den sonra 1 * 1 ya da n * 1 olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion

#region WithMany
// HasOne veya HasMany'den sonra 1 * n ya da n * n olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion
#endregion
#endregion
