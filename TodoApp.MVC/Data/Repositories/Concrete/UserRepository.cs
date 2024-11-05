// Data/Repositories/Concrete/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using TodoApp.MVC.Data.Context;
using TodoApp.MVC.Data.Repositories.Abstract;
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Data.Repositories.Concrete;

/*
 * UserRepository sınıfı, IUserRepository arayüzünü uygulayarak kullanıcı işlemlerinin veritabanında yönetilmesini
 * sağlar. Entity Framework Core kullanarak veritabanı ile etkileşimde bulunur. Bu sınıf, kullanıcı bilgilerini almak,
 * eklemek, güncellemek ve silmek için gereken yöntemleri içerir.
 *
 * Özellikler:
 * - _appDbContext: Uygulamanın veritabanı bağlamını temsil eder ve veritabanı işlemlerini yönetir.
 *
 * Metotlar:
 * - GetByUsername: Verilen kullanıcı adına göre kullanıcı nesnesini asenkron olarak getirir, bulunamazsa null döner.
 * - AddAsync: Yeni bir kullanıcı nesnesini veritabanına asenkron olarak ekler.
 * - Update: Mevcut bir kullanıcı nesnesini günceller.
 * - Delete: Belirtilen kullanıcı nesnesinin silinme durumunu günceller. Gerçek silme işlemi yerine, kullanıcı
 *   nesnesinin IsDeleted alanını true olarak ayarlayarak soft delete işlemi gerçekleştirir.
 * - SaveChangesAsync: Yapılan değişiklikleri veritabanına kaydeder ve asenkron olarak işlem tamamlanır.
 *
 * Bu sınıf, kullanıcı yönetimi işlemleri için gerekli olan işlevselliği sağlar ve veri erişim katmanında kullanılan
 * repository deseninin bir parçasıdır.
 */
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    /*
     * GetByUsername metodu, verilen kullanıcı adına göre kullanıcı nesnesini asenkron olarak getirir.
     *
     * Parametreler:
     * - userName: Getirilecek kullanıcının adı.
     *
     * Geri Dönüş Değeri:
     * - Kullanıcı nesnesi (varsa) ya da null.
     */
    public async Task<User?> GetByUsername(string userName)
    {
        return await _appDbContext.Users
            .Include(u => u.ToDoItems)
            .FirstOrDefaultAsync(u => u.Username == userName);
    }

    /*
     * AddAsync metodu, yeni bir kullanıcı nesnesini veritabanına asenkron olarak ekler.
     *
     * Parametreler:
     * - user: Eklenecek kullanıcı nesnesi.
     */
    public async Task AddAsync(User user)
    {
        await _appDbContext.Users.AddAsync(user);
    }

    /*
     * Update metodu, mevcut bir kullanıcı nesnesini günceller.
     *
     * Parametreler:
     * - user: Güncellenecek kullanıcı nesnesi.
     */
    public void Update(User user)
    {
        _appDbContext.Users.Update(user);
    }

    /*
     * Delete metodu, belirtilen kullanıcı nesnesinin silinme durumunu günceller.
     *
     * Parametreler:
     * - user: Silinecek kullanıcı nesnesi.
     */
    public void Delete(User user)
    {
        user.IsDeleted = true;
        _appDbContext.Users.Update(user);
    }

    /*
     * SaveChangesAsync metodu, yapılan değişiklikleri veritabanına kaydeder.
     *
     * Geri Dönüş Değeri:
     * - İşlem sonucunu belirten bool değeri.
     */
    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }
        catch
        {
            return false;
        }
    }
}