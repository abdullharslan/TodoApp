// Models/ViewModels/UserViewModel.cs
namespace TodoApp.MVC.Models.ViewModels;

/*
 * UserViewModel sınıfı, kullanıcı arayüzünde (View) kullanıcı bilgilerini temsil eden veri modelini oluşturur.
 * Bu model, kullanıcıların ad ve kullanıcı adı bilgilerini içerir.
 *
 * Özellikler:
 * - FirstName: Kullanıcının ilk adı.
 * - LastName: Kullanıcının soyadı.
 * - Username: Kullanıcının sistemdeki kullanıcı adı.
 *
 * Bu sınıf, kullanıcı bilgilerinin kullanıcı arayüzünde düzgün bir şekilde görüntülenmesine ve yönetilmesine yardımcı
 * olur.
 */
public class UserViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
}