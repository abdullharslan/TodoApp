using TodoApp.MVC.Data.Repositories.Abstract;
using TodoApp.MVC.Models.Domain.Concrete;
using TodoApp.MVC.Models.Exceptions;
using TodoApp.MVC.Services.Abstract;

/*
 * UserService sınıfı, kullanıcının profil yönetimi ve hesap işlemlerini gerçekleştiren servis katmanıdır.
 * Bu sınıf, kullanıcı bilgilerini güncelleme, parola değiştirme, hesap silme gibi temel işlevleri sağlar.
 *
 * Kullanıcılar yalnızca kendi hesapları üzerinde işlem yapabilir, bu nedenle tüm işlemler oturum açmış
 * kullanıcının kimliği üzerinden doğrulanır.
 *
 * Metotlar:
 * - GetCurrentUserProfileAsync: Giriş yapmış kullanıcının profil bilgilerini getirir.
 * - UpdateCurrentUserProfileAsync: Giriş yapmış kullanıcının profil bilgilerini günceller.
 * - ChangePasswordAsync: Giriş yapmış kullanıcının şifresini değiştirir.
 * - DeleteCurrentUserAsync: Giriş yapmış kullanıcının hesabını siler (soft delete).
 *
 * Bu sınıf, kullanıcı hesap yönetimi ile ilgili işlemleri güvenli ve organize bir şekilde
 * yürütmek için tasarlanmıştır.
 */
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;

    public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IAuthService authService)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
    }

    /*
     * GetCurrentUserAsync metodu, giriş yapmış kullanıcının bilgilerini alır ve doğrular.
     * Kullanıcı bulunamazsa bir hata fırlatır.
     *
     * Geri Dönüş Değeri:
     * - Giriş yapmış kullanıcının User nesnesi.
     *
     * Hatalar:
     * - CustomException: Kullanıcı bulunamazsa fırlatılır.
     */
    private async Task<User> GetCurrentUserAsync()
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            throw new CustomException("Oturum açmanız gerekiyor.", 401);
        }

        var user = await _userRepository.GetByUsername(username);
        return user ?? throw new CustomException("Kullanıcı bulunamadı.", 404);
    }

    /*
     * GetCurrentUserProfileAsync metodu, giriş yapmış kullanıcının profil bilgilerini alır.
     *
     * Geri Dönüş Değeri:
     * - Kullanıcının User nesnesi veya null.
     *
     * Hatalar:
     * - CustomException: Profil bilgileri alınırken bir hata oluşursa fırlatılır.
     */
    public async Task<User?> GetCurrentUserProfileAsync()
    {
        try
        {
            var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                throw new CustomException("Oturum açmanız gerekiyor.", 401);
            }

            return await _userRepository.GetByUsername(username);
        }
        catch (Exception ex) when (ex is not CustomException)
        {
            throw new CustomException("Profil bilgileri alınırken bir hata oluştu.", 500);
        }
    }

    /*
     * UpdateCurrentUserProfileAsync metodu, giriş yapmış kullanıcının profil bilgilerini günceller.
     *
     * Parametreler:
     * - firstName: Kullanıcının yeni adı.
     * - lastName: Kullanıcının yeni soyadı.
     *
     * Hatalar:
     * - CustomException: Ad veya soyad alanları boşsa fırlatılır.
     */
    public async Task UpdateCurrentUserProfileAsync(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
        {
            throw new CustomException("Ad ve soyad alanları boş bırakılamaz.", 400);
        }

        var user = await GetCurrentUserAsync();
        
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UpdatedAt = DateTime.UtcNow;

        try
        {
            _userRepository.Update(user);
            var result = await _userRepository.SaveChangesAsync();
            if (!result)
            {
                throw new CustomException("Profil güncellenirken bir hata oluştu.", 500);
            }
        }
        catch (Exception ex) when (ex is not CustomException)
        {
            throw new CustomException("Profil güncellenirken bir hata oluştu.", 500);
        }
    }

    /*
     * ChangePasswordAsync metodu, giriş yapmış kullanıcının şifresini değiştirir.
     *
     * Parametreler:
     * - currentPassword: Kullanıcının mevcut şifresi.
     * - newPassword: Kullanıcının yeni şifresi.
     *
     * Hatalar:
     * - CustomException: Şifre alanları boşsa veya yeni şifre mevcut şifre ile aynıysa fırlatılır.
     */
    public async Task ChangePasswordAsync(string currentPassword, string newPassword)
    {
        if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
        {
            throw new CustomException("Şifre alanları boş bırakılamaz.", 400);
        }

        if (currentPassword == newPassword)
        {
            throw new CustomException("Yeni şifre mevcut şifre ile aynı olamaz.", 400);
        }

        var user = await GetCurrentUserAsync();

        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
        {
            throw new CustomException("Mevcut şifre yanlış.", 400);
        }

        try
        {
            _authService.ValidatePassword(newPassword);
            
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            var result = await _userRepository.SaveChangesAsync();
            
            if (!result)
            {
                throw new CustomException("Şifre değiştirilirken bir hata oluştu.", 500);
            }
        }
        catch (Exception ex) when (ex is not CustomException)
        {
            throw new CustomException("Şifre değiştirilirken bir hata oluştu.", 500);
        }
    }
    
    /*
     * DeleteCurrentUserAsync metodu, giriş yapmış kullanıcının hesabını siler.
     * Bu işlem geri alınamaz ve kullanıcının tüm verileri silinir.
     *
     * Hatalar:
     * - CustomException: Hesap silinirken bir hata oluşursa fırlatılır.
     */
    public async Task DeleteCurrentUserAsync()
    {
        try
        {
            var user = await GetCurrentUserAsync();
        
            // Kullanıcıyı sil (soft delete)
            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
        
            _userRepository.Update(user);
            var result = await _userRepository.SaveChangesAsync();
        
            if (!result)
            {
                throw new CustomException("Hesap silinirken bir hata oluştu.", 500);
            }
        }
        catch (Exception ex) when (ex is not CustomException)
        {
            throw new CustomException("Hesap silinirken bir hata oluştu.", 500);
        }
    }
}