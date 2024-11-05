// Services/Abstract/IAuthService.cs
using TodoApp.MVC.Models.Domain.Concrete;
using TodoApp.MVC.Models.DTOs;

namespace TodoApp.MVC.Services.Abstract;

/*
 * IAuthService arayüzü, kimlik doğrulama işlemleri için gereken temel işlevleri tanımlar. Bu arayüz, kullanıcıların
 * kayıt olma, giriş yapma, token üretme ve kullanıcı varlığını kontrol etme işlemlerini sağlar.
 *
 * Metotlar:
 * - GenerateToken: Kullanıcı nesnesine dayalı olarak bir JWT token üretir.
 * - RegisterUserAsync: Yeni bir kullanıcıyı sisteme kaydeder ve başarılı kayıtta giriş yanıtı döner.
 * - LoginAsync: Kullanıcı bilgileriyle giriş yapar ve giriş yanıtı döner.
 * - UserExistsAsync: Belirli bir kullanıcı adının sistemde var olup olmadığını asenkron olarak kontrol eder.
 * - ValidatePassword: Girilen parolanın güvenlik politikasına uygun olup olmadığını kontrol eder.
 *
 * Bu arayüz, kimlik doğrulama işlemlerini kapsüller ve uygulama genelinde erişim kontrolü ve kullanıcı yönetimi sağlar.
 */
public interface IAuthService
{
    string GenerateToken(User user);
    Task<LoginResponseDTO> RegisterUserAsync(RegisterDTO registerDto);
    Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
    Task<bool> UserExistsAsync(string username);
    void ValidatePassword(string password);
}