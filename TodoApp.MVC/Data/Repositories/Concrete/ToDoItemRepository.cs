// Data/Repositories/Abstract/ToDoItemRepository.cs
using Microsoft.EntityFrameworkCore;
using TodoApp.MVC.Data.Context;
using TodoApp.MVC.Data.Repositories.Abstract;
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Data.Repositories.Concrete;

/*
 * ToDoItemRepository sınıfı, `IToDoItemRepository` arayüzünü uygulayarak görev işlemlerini gerçekleştiren bir veri
 * erişim sınıfıdır. Bu sınıf, veritabanındaki `ToDoItem` nesneleri üzerinde CRUD işlemleri sağlar.
 *
 * Özellikler:
 * - _appDbContext: `AppDbContext` örneği üzerinden veritabanına erişim sağlar.
 *
 * Metotlar:
 * - GetUserTodos: Görevlerin tamamlanma durumuna göre filtrelenmiş görevleri getirir. `isCompleted` parametresi null
 *   ise, tüm görevleri döner.
 * - AddAsync: Yeni bir `ToDoItem` nesnesini asenkron olarak veritabanına ekler.
 * - Update: Var olan bir `ToDoItem` nesnesini günceller.
 * - Delete: Görevin `IsDeleted` durumunu true yaparak soft delete işlemi gerçekleştirir.
 * - SaveChangesAsync: Yapılan değişiklikleri veritabanına kaydeder ve işlem sonucunu bool olarak döner.
 *
 * Bu sınıf, görev yönetimi işlemlerini veri erişim katmanında kapsüller ve veritabanı işlemlerini soyutlayarak servis
 * katmanına sağlar.
 */
public class ToDoItemRepository : IToDoItemRepository
{
    private readonly AppDbContext _appDbContext;

    public ToDoItemRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    /*
     * GetUserTodos metodu, belirtilen tamamlanma durumuna göre kullanıcıya ait görevleri alır.
     *
     * Parametreler:
     * - isCompleted: Görevlerin tamamlanma durumu (true, false veya null).
     *
     * Geri Dönüş Değeri:
     * - Kullanıcıya ait filtrelenmiş ToDoItem nesneleri.
     */
    public async Task<IEnumerable<ToDoItem>> GetUserTodos(bool? isCompleted = null)
    {
        IQueryable<ToDoItem> query = _appDbContext.ToDoItems
            .Include(t => t.User);

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        var todos = await query.ToListAsync();
        
        if (isCompleted.HasValue)
        {
            // İlgili duruma göre filtreleme yap ve aynı duruma sahip görevleri getir
            return todos.Where(t => t.IsCompleted == isCompleted.Value);
        }

        return todos;
    }

    /*
     * GetByIdAsync metodu, verilen kimliğe göre bir ToDoItem nesnesini getirir.
     *
     * Parametreler:
     * - id: Getirilecek ToDoItem'ın kimliği.
     *
     * Geri Dönüş Değeri:
     * - Belirtilen kimlikteki ToDoItem nesnesi.
     */
    public async Task<ToDoItem> GetByIdAsync(int id)
    {
        return (await _appDbContext.ToDoItems.FirstOrDefaultAsync(t => t.Id == id))!;
    }

    /*
     * AddAsync metodu, yeni bir ToDoItem nesnesini veritabanına ekler.
     *
     * Parametreler:
     * - todoItem: Eklenecek ToDoItem nesnesi.
     */
    public async Task AddAsync(ToDoItem todoItem)
    {
        await _appDbContext.ToDoItems.AddAsync(todoItem);
    }

    /*
     * Update metodu, var olan bir ToDoItem nesnesini günceller.
     *
     * Parametreler:
     * - todoItem: Güncellenecek ToDoItem nesnesi.
     */
    public void Update(ToDoItem todoItem)
    {
        todoItem.UpdatedAt = DateTime.UtcNow;
        _appDbContext.ToDoItems.Update(todoItem);
    }

    /*
     * Delete metodu, verilen ToDoItem nesnesini silmek için soft delete işlemi gerçekleştirir.
     *
     * Parametreler:
     * - todoItem: Silinecek ToDoItem nesnesi.
     */
    public void Delete(ToDoItem todoItem)
    {
        todoItem.IsDeleted = true;
        todoItem.UpdatedAt = DateTime.UtcNow;
        _appDbContext.ToDoItems.Update(todoItem);
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