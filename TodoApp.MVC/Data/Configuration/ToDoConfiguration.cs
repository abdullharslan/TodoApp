// Data/Configuration/ToDoConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Data.Configuration;

/*
 * ToDoConfiguration sınıfı, Entity Framework Core kullanarak ToDoItem tablosunun yapılandırılmasını sağlar.
 * Bu sınıf, ToDoItem nesnesinin veri tabanı tablosunda nasıl temsil edileceğini ve veri doğruluğunu sağlamak
 * için gereken ayarları yapar.
 *
 * Yapılandırmalar:
 * - ToTable: `ToDoItems` tablosunu temsil eder.
 * - HasKey: `Id` alanını birincil anahtar olarak tanımlar.
 *
 * Özellik Yapılandırmaları:
 * - Title: Zorunlu olup, maksimum 100 karakter uzunluğunda
 *   bir başlık gerektirir.
 * - Description: Zorunlu olup, maksimum 1000 karakter
 *   uzunluğunda bir açıklama gerektirir.
 * - IsCompleted: Zorunludur ve varsayılan olarak `false`
 *   atanır.
 *
 * Tarih ve Silinme Durumu:
 * - CreatedAt: Zorunludur ve varsayılan olarak `DateTime.UtcNow`
 *   atanır.
 * - UpdatedAt: Zorunludur ve varsayılan olarak `DateTime.UtcNow`
 *   atanır.
 * - IsDeleted: Silinme durumunu belirtir; varsayılan olarak
 *   `false` atanır.
 *
 * İlişkilendirme:
 * - HasOne: Her bir ToDoItem nesnesinin bir User nesnesine ait olduğunu belirtir ve User nesnesinin birden fazla
 *   ToDoItem'ı olabileceğini tanımlar.
 * - WithMany: User ile ToDoItems arasındaki bire çok ilişkiyi belirtir.
 * - HasForeignKey: ToDoItem'ın UserId alanının, User tablosundaki User nesnesine referans olduğunu belirtir.
 * - OnDelete: Kullanıcı silindiğinde, ilişkili ToDoItem'ların da silinmesini sağlar.
 *
 * Bu yapılandırmalar, ToDoItem nesnesinin veri tabanı tablosunda nasıl temsil edileceğini belirler ve veri bütünlüğünü
 * sağlar.
 */
public class ToDoConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.ToTable("ToDoItems");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(1000);
        
        builder.Property(t => t.IsCompleted)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);
            
        builder.Property(t => t.UpdatedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);
            
        builder.Property(t => t.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasOne(t => t.User)
            .WithMany(u => u.ToDoItems)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}