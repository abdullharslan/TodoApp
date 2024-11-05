// Data/Context/AppDbContext.cs
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoApp.MVC.Models.Domain.Abstract;
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Data.Context;

/*
 * AppDbContext sınıfı, Entity Framework Core kullanılarak uygulamanın veritabanı bağlamını temsil eder. Bu sınıf,
 * veritabanı işlemlerini yönetir ve `DbSet` özellikleriyle tablo yapılarını sağlar.
 *
 * Özellikler:
 * - Users: Kullanıcı tablosunu temsil eden `DbSet`.
 * - ToDoItems: Yapılacak öğeler tablosunu temsil eden `DbSet`.
 *
 * Yapılandırmalar:
 * - OnModelCreating: Model yapılandırmaları için kullanılır ve assembly'deki tüm model yapılandırmalarını uygular.
 * - Soft delete işlemleri: `HasQueryFilter` ile `IsDeleted` özelliğine göre otomatik filtreleme uygulanır.
 *
 * SaveChangesAsync Metodu:
 * - Her kayıt ekleme veya güncelleme işleminde `CreatedAt` ve `UpdatedAt` alanlarını günceller.
 * - Entity eklenirken `IsDeleted` alanını false olarak ayarlar.
 * - Güncelleme sırasında `CreatedAt` alanını korur ve yalnızca `UpdatedAt` alanını günceller.
 *
 * Bu sınıf, uygulamanın veritabanı işlemlerini düzenler, veritabanı erişimini soyutlar ve veri bütünlüğünü sağlar.
 */
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /*
     * SaveChangesAsync metodu, yapılan değişiklikleri veritabanına kaydederken,
     * ekleme ve güncelleme işlemleri için ilgili alanları günceller.
     *
     * Parametreler:
     * - cancellationToken: İşlemi iptal etmek için kullanılacak token.
     *
     * Geri Dönüş Değeri:
     * - Asenkron olarak yapılan işlem sonucunda etkilenen kayıt sayısını döner.
     */
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Modified:
                    // e = EntityBase'den türeyen herhangi bir entity (User veya ToDoItem). CreatedAt'i korumak için
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}