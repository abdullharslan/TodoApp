// Models/DTOs/LoginDTO.cs
namespace TodoApp.MVC.Models.DTOs;

/*
 * LoginDTO sınıfı, kullanıcıların giriş işlemleri için gerekli olan veri transfer nesnesidir. Kullanıcının kimlik
 * bilgilerini içermektedir.
 *
 * Özellikler:
 * - Username: Kullanıcının giriş yaparken kullanacağı kullanıcı adı.
 * - Password: Kullanıcının giriş yaparken kullanacağı şifre.
 *
 * Constructor Metot:
 * - Parametreli yapıcı metot, kullanıcı adı ve şifre bilgilerini alır ve ilgili özellikleri başlatır.
 */
public class LoginDTO
{
    public string Username { get; set; }
    public string Password { get; set; }

    public LoginDTO(string username, string password)
    {
        Username = username;
        Password = password;
    }
}