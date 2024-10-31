// Models/ViewModels/RegisterViewModel.cs
namespace TodoApp.MVC.Models.ViewModels;

/*
 * RegisterViewModel sınıfı, kullanıcı kayıt işlemleri için gerekli olan veri modelini temsil eder. Bu model,
 * kullanıcının kayıt olabilmesi için ihtiyaç duyduğu bilgileri içermektedir.
 *
 * Özellikler:
 * - FirstName: Kullanıcının ilk adı.
 * - LastName: Kullanıcının soyadı.
 * - Username: Kullanıcının kayıt sırasında belirleyeceği kullanıcı adı.
 * - Password: Kullanıcının kayıt sırasında belirleyeceği şifre.
 *
 * Bu sınıf, kullanıcıların kayıt işlemleri sırasında gereken bilgilerin toplanmasını ve işlenmesini sağlar.
 */
public class RegisterViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}