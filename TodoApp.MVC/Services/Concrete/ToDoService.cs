// Services/Concrete/ToDoService.cs
using TodoApp.MVC.Data.Repositories.Abstract;
using TodoApp.MVC.Models.Domain.Concrete;
using TodoApp.MVC.Models.Exceptions;
using TodoApp.MVC.Services.Abstract;

namespace TodoApp.MVC.Services.Concrete;

/*
 * ToDoService sınıfı, yapılacaklar listesi ile ilgili işlemleri yönetmek için gerekli iş mantığını sağlar.
 * Bu sınıf, görevleri listeleme, ekleme, güncelleme, tamamlama ve silme gibi işlemleri gerçekleştirir.
 * Kullanıcı bilgilerine erişim, HttpContext üzerinden sağlanır.
 *
 * Özellikler:
 * - _todoRepository: Görevleri veritabanında yönetmek için `IToDoItemRepository` örneği.
 * - _httpContextAccessor: HttpContext'e erişim sağlar, böylece giriş yapmış kullanıcının bilgilerine ulaşılır.
 * - _userRepository: Kullanıcı bilgilerini yönetmek için `IUserRepository` örneği.
 *
 * Metotlar:
 * - GetCurrentUserAsync: Giriş yapmış kullanıcının bilgilerini alır ve doğrular. Kullanıcı bulunamazsa bir hata fırlatır.
 * - GetAllTodosAsync: Kullanıcının tüm görevlerini alır.
 * - GetCompletedTodosAsync: Kullanıcının tamamlanmış görevlerini alır.
 * - GetIncompleteTodosAsync: Kullanıcının tamamlanmamış görevlerini alır.
 * - AddTodoAsync: Yeni bir görev ekler.
 * - UpdateTodoAsync: Mevcut bir görevi günceller.
 * - CompleteTodoAsync: Görevi tamamlanmış olarak işaretler.
 * - DeleteTodoAsync: Görevi siler.
 */
public class ToDoService : IToDoService
{
    private readonly IToDoItemRepository _todoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public ToDoService(
        IToDoItemRepository todoRepository, 
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository)
    {
        _todoRepository = todoRepository;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    /*
     * GetCurrentUserAsync metodu, giriş yapmış kullanıcının bilgilerini alır ve doğrular.
     * Kullanıcı bulunamazsa bir hata fırlatır.
     *
     * Geri Dönüş Değeri:
     * - Giriş yapmış kullanıcının User nesnesi.
     *
     * Hatalar:
     * - UnauthorizedAccessException: Giriş yapmış kullanıcı bulunamazsa fırlatılır.
     */
    private async Task<User> GetCurrentUserAsync()
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            throw new UnauthorizedAccessException("Giriş yapmış kullanıcı bulunamadı.");
        }

        var user = await _userRepository.GetByUsername(username);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");
        }

        return user;
    }

    /*
     * GetAllTodosAsync metodu, kullanıcının tüm görevlerini alır.
     *
     * Geri Dönüş Değeri:
     * - Kullanıcının tüm ToDoItem nesneleri.
     */
    public async Task<IEnumerable<ToDoItem>> GetAllTodosAsync()
    {
        return await _todoRepository.GetUserTodos(null);
    }

    /*
     * GetCompletedTodosAsync metodu, kullanıcının tamamlanmış görevlerini alır.
     *
     * Geri Dönüş Değeri:
     * - Kullanıcının tamamlanmış ToDoItem nesneleri.
     */
    public async Task<IEnumerable<ToDoItem>> GetCompletedTodosAsync()
    {
        return await _todoRepository.GetUserTodos(true);
    }

    /*
     * GetIncompleteTodosAsync metodu, kullanıcının tamamlanmamış görevlerini alır.
     *
     * Geri Dönüş Değeri:
     * - Kullanıcının tamamlanmamış ToDoItem nesneleri.
     */
    public async Task<IEnumerable<ToDoItem>> GetIncompleteTodosAsync()
    {
        return await _todoRepository.GetUserTodos(false);
    }

    /*
     * AddTodoAsync metodu, yeni bir görev ekler.
     *
     * Parametreler:
     * - title: Görevin başlığı.
     * - description: Görevin açıklaması.
     *
     * Hatalar:
     * - InvalidOperationException: Görev eklenirken bir hata oluşursa fırlatılır.
     */
    public async Task AddTodoAsync(string title, string description)
    {
        var currentUser = await GetCurrentUserAsync();

        var todoItem = new ToDoItem
        {
            Title = title,
            Description = description,
            UserId = currentUser.Id,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _todoRepository.AddAsync(todoItem);
        if (!await _todoRepository.SaveChangesAsync())
        {
            throw new InvalidOperationException("Görev eklenirken bir hata oluştu.");
        }
    }

    /*
     * UpdateTodoAsync metodu, mevcut bir görevi günceller.
     *
     * Parametreler:
     * - todoItem: Güncellenecek ToDoItem nesnesi.
     *
     * Hatalar:
     * - InvalidOperationException: Görev güncellenirken bir hata oluşursa fırlatılır.
     */
    public async Task UpdateTodoAsync(ToDoItem todoItem)
    {
        if (todoItem == null)
        {
            throw new CustomException("Görev bulunamadı.", 404);
        }

        try
        {
            var existingTodo = await _todoRepository.GetByIdAsync(todoItem.Id);
            if (existingTodo == null)
            {
                throw new CustomException("Görev bulunamadı.", 404);
            }

            // Mevcut entity'yi güncelle
            existingTodo.Title = todoItem.Title;
            existingTodo.Description = todoItem.Description;
            existingTodo.IsCompleted = todoItem.IsCompleted;
            existingTodo.UpdatedAt = DateTime.UtcNow;

            _todoRepository.Update(existingTodo);

            if (!await _todoRepository.SaveChangesAsync())
            {
                throw new CustomException("Görev güncellenirken bir hata oluştu.", 500);
            }
        }
        catch (Exception ex) when (ex is not CustomException)
        {
            throw new CustomException("Görev güncellenirken bir hata oluştu.", 500);
        }
    }

    /*
     * CompleteTodoAsync metodu, bir görevi tamamlanmış olarak işaretler.
     *
     * Parametreler:
     * - todoItem: Tamamlanacak ToDoItem nesnesi.
     *
     * Hatalar:
     * - InvalidOperationException: Görev tamamlanırken bir hata oluşursa fırlatılır.
     */
    public async Task CompleteTodoAsync(ToDoItem todoItem)
    {
        var currentUser = await GetCurrentUserAsync();
        todoItem.UserId = currentUser.Id; // Güvenlik için kullanıcı ID'sini set ediyoruz
        todoItem.IsCompleted = true;
        todoItem.CompletedAt = DateTime.UtcNow;

        _todoRepository.Update(todoItem);
        if (!await _todoRepository.SaveChangesAsync())
        {
            throw new InvalidOperationException("Görev tamamlanırken bir hata oluştu.");
        }
    }

    /*
     * DeleteTodoAsync metodu, belirtilen görevi siler.
     *
     * Parametreler:
     * - todoItem: Silinecek ToDoItem nesnesi.
     *
     * Hatalar:
     * - InvalidOperationException: Görev silinirken bir hata oluşursa fırlatılır.
     */
    public async Task DeleteTodoAsync(ToDoItem todoItem)
    {
        var currentUser = await GetCurrentUserAsync();
        todoItem.UserId = currentUser.Id; // Güvenlik için kullanıcı ID'sini set ediyoruz

        _todoRepository.Delete(todoItem);
        if (!await _todoRepository.SaveChangesAsync())
        {
            throw new InvalidOperationException("Görev silinirken bir hata oluştu.");
        }
    }
}