// Middlewares/ExceptionMiddleware.cs
using TodoApp.MVC.Models.Exceptions;

namespace TodoApp.MVC.Middlewares;

/*
 * ExceptionMiddleware sınıfı, HTTP istekleri sırasında oluşabilecek hataları yakalayarak uygun bir yanıt döner. Bu
 * sınıf, uygulama genelinde hata yönetimini sağlamak amacıyla kullanılmaktadır.
 *
 * Özellikler:
 * - _next: Bir sonraki middleware bileşenine geçiş yapmak için kullanılır.
 * - _logger: Hata günlükleme işlemleri için ILogger arayüzü.
 *
 * Metotlar:
 * - InvokeAsync: HTTP isteklerini alır ve hata oluştuğunda
 *   HandleExceptionAsync metodunu çağırır.
 * - HandleExceptionAsync: Hata türüne göre uygun HTTP durum kodunu ayarlar ve kullanıcıya dost bir hata mesajı
 *   gönderir. AJAX istekleri için JSON formatında yanıt verir, form submit durumunda ise kullanıcıyı ilgili sayfaya
 *   yönlendirir.
 *
 * Bu sınıf, uygulamadaki hataların merkezi olarak yönetilmesini sağlar ve kullanıcı dostu bir deneyim sunar.
 */
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen bir hata oluştu.");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            CustomException customException => customException.StatusCode,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;
        
        var errorMessage = exception is CustomException customEx ? customEx.Message : "Bir hata oluştu.";

        if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = errorMessage });
            return;
        }

        if (context.Request.Method == "POST" && context.Request.HasFormContentType)
        {
            var returnUrl = context.Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                context.Response.Redirect($"{returnUrl}?error={Uri.EscapeDataString(errorMessage)}");
                return;
            }
        }

        context.Response.Redirect(
            $"/Home/Error?message={Uri.EscapeDataString(errorMessage)}&statusCode={statusCode}");
    }
}