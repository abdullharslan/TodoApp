// Data/Repositories/Abstract/IUserRepository.cs
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Data.Repositories.Abstract;

/*
 * IUserRepository arayüzü, kullanıcı verileriyle etkileşim kurmak için gerekli olan temel yöntemleri tanımlar.
 * Bu arayüz, kullanıcı nesneleri üzerinde CRUD (Create, Read, Update, Delete) işlemlerini gerçekleştirmek için kullanılır.
 *
 * Metotlar:
 * - GetByUsername: Kullanıcı adını kullanarak bir kullanıcı nesnesini asenkron olarak alır.
 * - IsUsernameExistsAsync: Veritabanında belirtilen kullanıcı adının mevcut olup olmadığını kontrol eder.
 * - AddAsync: Yeni bir kullanıcı nesnesini veritabanına asenkron olarak ekler.
 * - Update: Mevcut bir kullanıcı nesnesini günceller.
 * - Delete: Belirtilen kullanıcı nesnesini siler.
 * - SaveChangesAsync: Yapılan değişiklikleri veritabanına kaydeder ve asenkron olarak işlem tamamlanır.
 *
 * Bu arayüz, kullanıcı yönetimi işlemleri için temel bir soyutlama sağlar ve veri erişim katmanında kullanılan
 * repository deseninin bir parçasıdır.
 */
public interface IUserRepository
{
    Task<User?> GetByUsername(string userName);
    Task AddAsync(User user);
    void Update(User user);
    void Delete(User user);
    Task<bool> SaveChangesAsync();
}