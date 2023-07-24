// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;
using System.Runtime.CompilerServices;

LazyLoadingDbContext context = new();
Console.WriteLine("Hello, World!");

#region Lazy Loading Nedir?
// Navigation Propertyler üzerinde bir işlem yapılmaya çalışıldığı takdirde ilgili propert'e temsil ettiği/karşılık tabloya özel bir sorgu oluşturulup execute edilmesi ve verilerin yüklenmesini sağlayan bir yaklaşımdır.
#endregion

//var employee = await context.Employees.FindAsync(2);
//Console.WriteLine(employee.Region.Name);

#region Prox'lerle Lazy Loading
// Microsoft.EntityFrameworkCore.Proxies

#region Propertylerin virtuel olması
// Eğer ki proxler üzerinden lazy loading işlemi gerçekleştiriyor isek navigation propertlerin virtuel olarak işaretlenmiş olması gerekmektedir aksi taktirde patlama meydana gelecektir.
#endregion
#endregion

#region Proxy Olmaksızın Lazy Loading
//proxy'ler tüm platfortmlarda desteklenmiyebilir. Böyle bir durumda manuel bir şekilde lazy loading'i uygulamak mecburiyetinde kalabiliriz.
// Manuel yapılan Lazy loading operasyonlarında Navigaton propertylerin virtual ile işaretlenmesine gerek yoktur.

#region ILazyLoader Interface'i ile Lazy Loading
// Microsoft.EntityFrameworkCore.Abstractions
//var employee = await context.Employees.FindAsync(2);
//Console.WriteLine();
#endregion

#region Delegate ile Lazy Loading
var employee = await context.Employees.FindAsync(2);
Console.WriteLine();
#endregion

#endregion

#region N+1 Problemi
//var region = await context.Regions.FindAsync(1);
//foreach(var employe in region.Employees)
//{
//    var orders = employe.Orders;
//    foreach( var order in orders)
//    {
//        Console.WriteLine(order.OrderDate);
//    }
//}

// lazy loading kullanım açısından oldukça maliyetli ve performans düşürücü bir etkiye sahip bir yöntemdir o yüzden kullanırken mümkün merttebe dikkatli olmalı ve özellikle navigation propertysilerın döngüsel twetiklenem durumlarında lazy loading'i tercih etmemeye odaklanmamalıyız aksi takdirde her bir tetiklemye karşılık aynı sorguları üretip execute edecektir => bu durumu n+1 problemi olarak nitelendrmekteyiz.
// Mümkün mertebe, ilişkisel verileri ekelrken lazy loading kullanmamaya özen göstermeliyiz. 
#endregion

Console.WriteLine();

#region Proxy ile Lazy Loading
public class Employee
{
    public Employee() { }
    
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Salary { get; set; }
    public virtual List<Order> Orders { get; set; }
    public virtual Region Region { get; set; }
}
public class Region
{
    public Region() { }
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
}
public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public virtual Employee Employee { get; set; }
}
#endregion
#region ILazyLoader Interface'i ile lazy loading
//public class Employee
//{
//    public Employee() { }
//    #region ILazyLoader
//    ILazyLoader _lazyLoader;
//    Region _region;
//    public Employee(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader;
//    #endregion
//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public virtual List<Order> Orders { get; set; }
//    //public virtual Region Region { get; set; }
//    #region ILazyLoader
//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region);
//        set => _region = value;
//    }
//    #endregion

//}
//public class Region
//{
//    public Region() { }

//    #region ILazyLoader
//    ILazyLoader _lazyLoader;
//    public ICollection<Employee> _employees;
//    public Region(ILazyLoader lazyLoader)
//        => _lazyLoader = lazyLoader;
//    #endregion
//    public int Id { get; set; }
//    public string Name { get; set; }
//    //public virtual ICollection<Employee> Employees { get; set; }
//    #region ILazyLoader
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
//    #endregion
//}
//public class Order
//{
//    public int Id { get; set; }
//    public int EmployeeId { get; set; }
//    public DateTime? OrderDate { get; set; }
//    public virtual Employee Employee { get; set; }
//}
#endregion

#region Delegate İle Lazy Loading
//public class Employee
//{
//    public Employee() { }
//    #region Delegate
//    Action<object, string> _lazyLoader;
//    Region _region;
//    public Employee(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    #endregion
//    public int Id { get; set; }
//    public int RegionId { get; set; }
//    public string? Name { get; set; }
//    public string? Surname { get; set; }
//    public int Salary { get; set; }
//    public virtual List<Order> Orders { get; set; }
//    //public virtual Region Region { get; set; }
//    #region Delegate
//    public Region Region
//    {
//        get => _lazyLoader.Load(this, ref _region);
//        set => _region = value;
//    }
//    #endregion

//}
//public class Region
//{
//    public Region() { }

//    #region Delegate
//    Action<object, string> _lazyLoader;
//    public ICollection<Employee> _employees;
//    public Region(Action<object, string> lazyLoader)
//        => _lazyLoader = lazyLoader;
//    #endregion
//    public int Id { get; set; }
//    public string Name { get; set; }
//    //public virtual ICollection<Employee> Employees { get; set; }
//    #region Delegate
//    public ICollection<Employee> Employees
//    {
//        get => _lazyLoader.Load(this, ref _employees);
//        set => _employees = value;
//    }
//    #endregion
//}
//public class Order
//{
//    public int Id { get; set; }
//    public int EmployeeId { get; set; }
//    public DateTime? OrderDate { get; set; }
//    public virtual Employee Employee { get; set; }
//}
//// ileri seviye c# egitinminde arastıracagım konular.
//static class LazyLoadingExtension
//{
//    public static TRelated Load<TRelated>(this Action<object, string> loader, object entity, ref TRelated navigation, [CallerMemberName] string navigationName = null)
//    {
//        loader.Invoke(entity, navigationName);
//        return navigation;
//    }
//}
#endregion




class LazyLoadingDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Region> Regions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LazyLoadingDb;Trusted_Connection=True;TrustServerCertificate=True");

        #region Prox'lerle Lazy Loading
        //optionsBuilder.UseLazyLoadingProxies();

        optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LazyLoadingDb;Trusted_Connection=True;TrustServerCertificate=True");
        #endregion
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}