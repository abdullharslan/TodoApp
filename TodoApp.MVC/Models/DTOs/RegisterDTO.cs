// Models/DTOs/RegisterDTO.cs
namespace TodoApp.MVC.Models.DTOs;

/*
 * RegisterDTO sınıfı, kullanıcıların kayıt işlemleri için gerekli olan veri transfer nesnesidir. Kullanıcının kaydı
 * için gereken bilgileri içerir.
 *
 * Özellikler:
 * - FirstName: Kullanıcının ilk adı.
 * - LastName: Kullanıcının soyadı.
 * - Username: Kullanıcının kayıt sırasında belirleyeceği kullanıcı adı.
 * - Password: Kullanıcının kayıt sırasında belirleyeceği şifre.
 *
 * Constructor Metot:
 * - Parametreli constructor: Kullanıcının ilk adı, soyadı, kullanıcı adı ve şifre bilgilerini alır ve ilgili
 *   özellikleri başlatır.
 */
public class RegisterDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public RegisterDTO(string firstName, string lastName, string username, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Password = password;
    }
}