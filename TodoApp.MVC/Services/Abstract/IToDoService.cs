// Services/Abstract/IToDoService.cs
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Services.Abstract;

/*
 * IToDoService arayüzü, giriş yapmış kullanıcının görev yönetimi için gerekli metotları tanımlar. Bu arayüz,
 * kullanıcıların kendi görevlerini yönetmelerine olanak tanır ve görevlerle ilgili işlemleri gerçekleştirir.
 * Kullanıcıların yalnızca kendi görevlerini yönetmesi için güvenlik mekanizmaları (örneğin, token doğrulama) ile
 * entegre edilmelidir.
 *
 * Metotlar:
 * - GetAllTodosAsync: Kullanıcının tüm görevlerini alır.
 * - GetCompletedTodosAsync: Kullanıcının tamamlanmış görevlerini alır.
 * - GetIncompleteTodosAsync: Kullanıcının tamamlanmamış görevlerini alır.
 * - AddTodoAsync: Yeni bir görev ekler.
 * - UpdateTodoAsync: Mevcut bir görevi günceller.
 * - CompleteTodoAsync: Belirtilen görevi tamamlanmış olarak işaretler.
 * - DeleteTodoAsync: Belirtilen görevi siler.
 */
public interface IToDoService
{
    Task<IEnumerable<ToDoItem>> GetAllTodosAsync();
    Task<IEnumerable<ToDoItem>> GetCompletedTodosAsync();
    Task<IEnumerable<ToDoItem>> GetIncompleteTodosAsync();
    Task AddTodoAsync(string title, string description);
    Task UpdateTodoAsync(ToDoItem todoItem);
    Task CompleteTodoAsync(ToDoItem todoItem);
    Task DeleteTodoAsync(ToDoItem todoItem);
}