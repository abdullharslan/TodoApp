// Models/Exceptions/CustomException.cs
namespace TodoApp.MVC.Models.Exceptions;

/*
 * CustomException sınıfı, uygulama içinde özel hata durumlarını temsil etmek için kullanılan bir istisna sınıfıdır.
 * Bu sınıf, bir hata mesajı ve isteğe bağlı bir durum kodu alarak oluşturulur.
 *
 * Özellikler:
 * - StatusCode: Hatanın HTTP durum kodunu temsil eder. Varsayılan olarak 500 (Internal Server Error) değerini alır.
 *
 * Constructor Metot:
 * - Parametreli Constructor: Hata mesajı ve durum kodu alır, StatusCode özelliğini başlatır.
 */
public class CustomException : Exception
{
    public int StatusCode { get; }

    public CustomException(string message, int statusCode = 500) 
        : base(message)
    {
        StatusCode = statusCode;
    }
}