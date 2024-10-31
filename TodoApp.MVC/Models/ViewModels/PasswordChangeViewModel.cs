// Models/ViewModels/PasswordChangeViewModel
namespace TodoApp.MVC.Models.ViewModels;

/*
 * PasswordChangeViewModel sınıfı, kullanıcıların şifre değiştirme işlemleri için gerekli olan veri modelini temsil
 * eder. Bu model, mevcut şifre ve yeni şifre bilgilerini içerir.
 *
 * Özellikler:
 * - CurrentPassword: Kullanıcının mevcut şifresi.
 * - NewPassword: Kullanıcının belirleyeceği yeni şifre.
 * - ConfirmNewPassword: Kullanıcının yeni şifresini doğrulamak için tekrar girdiği alan.
 *
 * Bu sınıf, kullanıcıların şifre değiştirme işlemleri sırasında gereken bilgilerin toplanmasını ve doğrulanmasını
 * sağlar.
 */
public class PasswordChangeViewModel
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}