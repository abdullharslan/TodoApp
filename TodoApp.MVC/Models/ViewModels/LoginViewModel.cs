// Models/ViewModels/LoginViewModel.cs
namespace TodoApp.MVC.Models.ViewModels;

/*
 * LoginViewModel sınıfı, kullanıcı giriş işlemleri için gerekli olan veri modelini temsil eder. Bu model, kullanıcının
 * kimlik bilgilerini içermektedir.
 *
 * Özellikler:
 * - Username: Kullanıcının giriş yaparken kullanacağı kullanıcı adı.
 * - Password: Kullanıcının giriş yaparken kullanacağı şifre.
 *
 * Bu sınıf, kullanıcıların giriş işlemleri sırasında gereken bilgilerin toplanmasını ve işlenmesini sağlar.
 */
public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}