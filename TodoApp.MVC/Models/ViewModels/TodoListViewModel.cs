// Models/ViewModels/TodoListViewModel.cs
namespace TodoApp.MVC.Models.ViewModels;

/*
 * TodoListViewModel sınıfı, kullanıcı arayüzünde yapılacaklar listesinin yönetimi için kullanılan bir veri modelidir.
 * Bu model, kullanıcıların tamamlanmış ve tamamlanmamış yapılacak öğelerini tek bir koleksiyon olarak alır ve arayüzde
 * gruplandırılmış biçimde sunulmasını sağlar.
 *
 * Özellikler:
 * - AllTodos: Kullanıcının tüm yapılacak öğelerini (tamamlanmış ve tamamlanmamış) içeren koleksiyon.
 *
 * Bu sınıf, kullanıcıların yapılacaklar listesine daha kolay erişim sağlayarak listeyi etkin bir şekilde
 * yönetmelerine ve gözden geçirmelerine olanak tanır.
 */
public class TodoListViewModel
{
    public IEnumerable<TodoViewModel> AllTodos { get; set; } = new List<TodoViewModel>();
}