// Models/ErrorViewModel.cs
namespace TodoApp.MVC.Models;

/*
 * ErrorViewModel sınıfı, uygulamada oluşabilecek hata bilgilerini temsil eden veri modelidir. Bu model, hatanın
 * tanımlanması ve kullanıcıya uygun bir mesaj gösterilmesi için gerekli bilgileri içerir.
 *
 * Özellikler:
 * - RequestId: Hatanın oluştuğu isteğe ait benzersiz kimlik bilgisi.
 * - ShowRequestId: RequestId'nin geçerli olup olmadığını kontrol eden bir özellik.
 * - ErrorMessage: Hata hakkında kullanıcıya gösterilecek mesaj.
 * - StatusCode: Hatanın HTTP durum kodunu temsil eder.
 * - ReturnUrl: Kullanıcının hatadan sonra yönlendirileceği URL.
 *
 * Bu sınıf, kullanıcıya hata durumunda anlamlı bilgiler sunarak daha iyi bir kullanıcı deneyimi sağlar.
 */
public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? ErrorMessage { get; set; } = "Bir hata oluştu.";
    public string? ReturnUrl { get; set; } = "/";
    public int StatusCode { get; set; }
}