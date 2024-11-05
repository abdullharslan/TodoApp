// Controllers/AccountController.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.MVC.Models.DTOs;
using TodoApp.MVC.Models.Exceptions;
using TodoApp.MVC.Models.ViewModels;
using TodoApp.MVC.Services.Abstract;

namespace TodoApp.MVC.Controllers;

/*
 * AccountController, kullanıcı kimlik ve profil yönetimi işlemlerini gerçekleştirir. Bu sınıf, kullanıcıların oturum
 * açma, kayıt olma, profil yönetimi ve şifre değiştirme işlemlerini içerir.
 *
 * Özellikler:
 * - IAuthService: Kullanıcı kimlik doğrulama işlemleri için kullanılan servis.
 * - IUserService: Kullanıcı profil işlemleri için kullanılan servis.
 *
 * Metotlar:
 * - Login: Kullanıcıyı oturum açtırmak için gerekli işlemleri gerçekleştirir.
 * - Register: Yeni bir kullanıcı kaydı oluşturur.
 * - Profile: Kullanıcının profil bilgilerini gösterir ve günceller.
 * - ChangePassword: Kullanıcının şifresini değiştirmesine olanak tanır.
 * - Logout: Kullanıcıyı sistemden çıkartır ve tüm cookie'leri temizler.
 * - DeleteAccount: Kullanıcının hesabını siler ve çıkış yapar.
 */
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AccountController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Index", "Home");
        }
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var loginDto = new LoginDTO(model.Username, model.Password);
            var result = await _authService.LoginAsync(loginDto);

            // JWT Token'ı cookie'ye kaydet
            Response.Cookies.Append("jwt", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.ExpiresAt
            });

            // Basit bir authentication cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, model.Username) },
                    CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = result.ExpiresAt
                });

            return RedirectToAction("Index", "Home");
        }
        catch (UnauthorizedAccessException)
        {
            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            return View(model);
        }
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var registerDto = new RegisterDTO(model.FirstName, model.LastName, model.Username, model.Password);
            var result = await _authService.RegisterUserAsync(registerDto);

            // Token'ı güvenli cookie'de sakla
            Response.Cookies.Append("jwt", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.ExpiresAt
            });

            return RedirectToAction("Index", "Home");
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu.");
            return View(model);
        }
    }

    // GET: /Account/Profile
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        try
        {
            var user = await _userService.GetCurrentUserProfileAsync();
        
            if (user == null)
            {
                throw new CustomException("Kullanıcı bulunamadı.", 404);
            }

            var model = new UserViewModel
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(model);
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(new UserViewModel());
        }
    }

    // POST: /Account/Profile
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _userService.UpdateCurrentUserProfileAsync(model.FirstName, model.LastName);
            TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
            return RedirectToAction(nameof(Profile));
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // GET: /Account/ChangePassword
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    // POST: /Account/ChangePassword
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(PasswordChangeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _userService.ChangePasswordAsync(model.CurrentPassword, model.NewPassword);
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction(nameof(Profile));
        }
        catch (CustomException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // POST: /Account/Logout
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // Tüm cookie'leri temizle
        Response.Cookies.Delete("jwt");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
    
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount()
    {
        try
        {
            await _userService.DeleteCurrentUserAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("jwt");
            TempData["SuccessMessage"] = "Hesabınız başarıyla silindi.";
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Profile));
        }
    }
}