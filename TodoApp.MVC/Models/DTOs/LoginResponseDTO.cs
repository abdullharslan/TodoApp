// Models/DTOs/LoginResponseDTO.cs

using TodoApp.MVC.Models.Domain.Concrete;

/*
 * LoginResponseDTO sınıfı, kullanıcı giriş işlemi sonrası döndürülecek olan yanıtı temsil eder. Giriş yapan kullanıcıya
 * ait bilgileri ve erişim token'ını içerir.
 *
 * Özellikler:
 * - Token: Kullanıcının giriş işlemi sonrası elde ettiği erişim token'ı.
 * - ExpiresAt: Token'ın geçerlilik süresinin sona ereceği tarih ve saat.
 *
 * Constructor Metot:
 * - Parametreli Constuctor: Token ve geçerlilik tarihi alır ve ilgili özellikleri başlatır.
 */
namespace TodoApp.MVC.Models.DTOs;

public class LoginResponseDTO
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }

    public LoginResponseDTO(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }
}