// Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using TodoApp.MVC.Services.Abstract;

namespace TodoApp.MVC.Controllers;

/*
 * HomeController, uygulamanın ana sayfa ve hata yönetimi işlemlerini gerçekleştiren ana denetleyicidir.
 *
 * Metotlar:
 * - Index: Kullanıcının oturum durumuna göre ana sayfa veya dashboard görünümünü sağlar. Giriş yapmış kullanıcılar için
 *   yapılacaklar listesine dair özet bilgileri dashboard'ta sunar.
 *
 * Görevler:
 * - Ana sayfa, giriş yapılmamış kullanıcılar için bir karşılama sayfası sunar.
 * - Giriş yapmış kullanıcılar için dashboard görünümünde yapılacakların genel durumu (tamamlanan ve tamamlanmayan)
 *   gösterilir.
 */
public class HomeController : Controller
{
    private readonly IToDoService _todoService;
    private readonly IUserService _userService;

    public HomeController(IToDoService todoService, IUserService userService)
    {
        _todoService = todoService;
        _userService = userService;
    }

    /*
     * Index metodu, kullanıcının oturum durumuna göre karşılama sayfası veya dashboard görünümünü döner.
     * Giriş yapmış kullanıcılar için yapılacaklar listesine dair özet bilgileri sağlar.
     *
     * Dönüş:
     * - Kullanıcı oturum açmışsa dashboard görünümünü döner.
     *   ViewBag'de toplam yapılacak sayısı, tamamlananlar ve tamamlanmayanlar bilgileri bulunur.
     * - Kullanıcı oturum açmamışsa karşılama sayfasını döner.
     *
     * Gösterge Bilgileri:
     * - ViewBag.TotalTodos: Toplam yapılacak öğe sayısı
     * - ViewBag.CompletedTodos: Tamamlanmış yapılacak öğe sayısı
     * - ViewBag.IncompleteTodos: Tamamlanmamış yapılacak öğe sayısı
     */
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            // Dashboard için özet bilgiler
            var allTodos = await _todoService.GetAllTodosAsync();
            ViewBag.TotalTodos = allTodos.Count();
            ViewBag.CompletedTodos = allTodos.Count(t => t.IsCompleted);
            ViewBag.IncompleteTodos = allTodos.Count(t => !t.IsCompleted);

            return View("Dashboard");
        }

        return View();
    }
}