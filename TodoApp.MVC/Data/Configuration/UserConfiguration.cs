// Data/Configuration/UserConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Data.Configuration;

/*
 * UserConfiguration sınıfı, Entity Framework Core kullanarak User tablosunun yapılandırılmasını sağlar. 
 * Bu sınıf, User tablosunun özelliklerini, anahtarlarını ve varsayılan değerlerini tanımlar.
 *
 * Yapılandırmalar:
 * - ToTable: `Users` tablosunu temsil eder.
 * - HasKey: `Id` alanını birincil anahtar olarak tanımlar.
 *
 * Özellik Yapılandırmaları:
 * - Username: Zorunlu olup, maksimum 50 karakter uzunluğunda bir kullanıcı adı gerektirir.
 * - Password: Zorunlu olup, maksimum 70 karakter uzunluğunda bir parola gerektirir.
 * - FirstName: Zorunlu olup, maksimum 50 karakter uzunluğundadır.
 * - LastName: Zorunlu olup, maksimum 50 karakter uzunluğundadır.
 *
 * Tarih ve Silinme Durumu:
 * - CreatedAt: Zorunludur ve varsayılan olarak `DateTime.UtcNow` atanır.
 * - UpdatedAt: Zorunludur ve varsayılan olarak `DateTime.UtcNow` atanır.
 * - IsDeleted: Silinme durumunu belirtir; varsayılan olarak `false` atanır.
 *
 * Bu yapılandırmalar, User nesnesinin veri tabanı tablosunda nasıl temsil edileceğini belirler ve veri doğruluğunu
 * sağlar.
 */
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(70);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(u => u.UpdatedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}