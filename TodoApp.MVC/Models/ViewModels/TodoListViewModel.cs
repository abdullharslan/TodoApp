// Models/ViewModels/TodoListViewModel.cs
namespace TodoApp.MVC.Models.ViewModels;

/*
 * TodoListViewModel sınıfı, kullanıcı arayüzünde (View) yapılacaklar listesinin yönetimi için kullanılan veri modelini
 * temsil eder. Bu model, kullanıcının tamamlanmış ve tamamlanmamış yapılacak öğelerini gruplar halinde sunar.
 *
 * Özellikler:
 * - CompletedTodos: Tamamlanmış yapılacak öğelerini içeren koleksiyon.
 * - IncompleteTodos: Tamamlanmamış yapılacak öğelerini içeren koleksiyon.
 * - AllTodos: Tüm yapılacak öğelerini içeren koleksiyon.
 *
 * Bu sınıf, kullanıcıların yapılacaklar listelerini daha etkin bir şekilde görüntülemelerine ve yönetmelerine
 * olanak tanır.
 */
public class TodoListViewModel
{
    public IEnumerable<TodoViewModel> CompletedTodos { get; set; }
    public IEnumerable<TodoViewModel> IncompleteTodos { get; set; }
    public IEnumerable<TodoViewModel> AllTodos { get; set; }
}