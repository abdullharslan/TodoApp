// Services/Abstract/IUserService.cs
using TodoApp.MVC.Models.Domain.Concrete;

namespace TodoApp.MVC.Services.Abstract;

/*
 * IUserService arayüzü, giriş yapmış kullanıcının profil yönetimi için gerekli metotları tanımlar. Token ile tanımlanan
 * kullanıcı sadece kendi profilini yönetebilir.
 *
 * Metotlar:
 * - GetCurrentUserProfileAsync: Giriş yapmış kullanıcının profil bilgilerini getirir.
 * - UpdateCurrentUserProfileAsync: Giriş yapmış kullanıcının profil bilgilerini günceller.
 * - ChangePasswordAsync: Giriş yapmış kullanıcının şifresini değiştirir.
 * - DeleteCurrentUserAsync: Giriş yapmış kullanıcının hesabını siler.
 */
public interface IUserService
{
    Task<User?> GetCurrentUserProfileAsync();
    Task UpdateCurrentUserProfileAsync(string firstName, string lastName);
    Task ChangePasswordAsync(string currentPassword, string newPassword);
    Task DeleteCurrentUserAsync();
}