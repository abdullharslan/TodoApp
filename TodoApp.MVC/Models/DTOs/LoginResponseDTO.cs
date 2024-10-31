// Models/DTOs/LoginResponseDTO.cs

using TodoApp.MVC.Models.Domain.Concrete;

/*
 * LoginResponseDTO sınıfı, kullanıcı giriş işlemi sonrası döndürülecek olan yanıtı temsil eder. Giriş yapan kullanıcıya
 * ait bilgileri ve erişim token'ını içerir.
 *
 * Özellikler:
 * - Token: Kullanıcının giriş işlemi sonrası elde ettiği erişim token'ı.
 * - ExpiresAt: Token'ın geçerlilik süresinin sona ereceği tarih ve saat.
 * - User: Giriş yapan kullanıcıya ait bilgileri içeren User nesnesi.
 *
 * Constructor Metot:
 * - Parametreli yapıcı metot, token, geçerlilik tarihi ve kullanıcı bilgilerini alır ve ilgili özellikleri başlatır.
 */
namespace TodoApp.MVC.Models.DTOs;

public class LoginResponseDTO
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public User User { get; set; }

    public LoginResponseDTO(string token, DateTime expiresAt, User user)
    {
        Token = token;
        ExpiresAt = expiresAt;
        User = user;
    }
}