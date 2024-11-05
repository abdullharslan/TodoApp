// Data/Repositories/Abstract/IToDoItemRepository.cs
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Data.Repositories.Abstract;

/*
 * IToDoItemRepository arayüzü, görev (ToDoItem) işlemleri için gerekli CRUD metotlarını tanımlar. Bu arayüz, görevlerin
 * veritabanında yönetilmesi için gereken temel işlevselliği sağlar.
 *
 * Özellikler:
 * - GetUserTodos: Belirtilen tamamlanma durumuna göre kullanıcının görevlerini alır. İsteğe bağlı olarak tamamlanmış
 *   veya tamamlanmamış görevleri dönebilir.
 * - AddAsync: Yeni bir görev nesnesini veritabanına asenkron olarak ekler.
 * - Update: Mevcut bir görev nesnesini günceller.
 * - Delete: Belirtilen görev nesnesini siler. Gerçek silme yerine, silinme durumunu  güncelleyerek soft delete işlemi
 *   gerçekleştirilir.
 * - SaveChangesAsync: Yapılan değişiklikleri veritabanına kaydeder ve asenkron olarak işlem tamamlanır.
 *
 * Bu arayüz, görev yönetimi işlemleri için gerekli olan işlevselliği sağlar ve veri erişim katmanında kullanılan
 * repository deseninin bir parçasıdır.
 */
public interface IToDoItemRepository
{
    Task<IEnumerable<ToDoItem>> GetUserTodos(bool? isCompleted = null);
    Task<ToDoItem> GetByIdAsync(int id);
    Task AddAsync(ToDoItem todoItem);
    void Update(ToDoItem todoItem);
    void Delete(ToDoItem todoItem);
    Task<bool> SaveChangesAsync();
}