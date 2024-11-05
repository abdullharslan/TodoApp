// Services/Concrete/AuthService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApp.MVC.Data.Repositories.Abstract;
using TodoApp.MVC.Models.Domain.Concrete;
using TodoApp.MVC.Models.DTOs;
using TodoApp.MVC.Models.Exceptions;
using TodoApp.MVC.Services.Abstract;

namespace TodoApp.MVC.Services.Concrete;

/*
 * AuthService sınıfı, kimlik doğrulama işlemleri için gereken işlevleri gerçekleştirir. 
 * Bu sınıf, kullanıcı kaydı, giriş yapma, JWT token oluşturma ve kullanıcı adı kontrolü gibi işlevleri sağlar.
 *
 * Metotlar:
 * - GenerateToken: Belirtilen kullanıcıya dayalı olarak bir JWT token oluşturur.
 * - RegisterUserAsync: Yeni bir kullanıcıyı kaydeder ve başarılı kayıt durumunda giriş yanıtı döner.
 * - LoginAsync: Kullanıcı adı ve şifre ile giriş yapar ve başarılı giriş durumunda JWT token döner.
 * - UserExistsAsync: Kullanıcı adının mevcut olup olmadığını kontrol eder.
 * - ValidatePassword: Şifreyi belirli kurallara göre doğrular.
 */
public class AuthService : IAuthService
{
    /*
     * _userRepository: Kullanıcı işlemleri için kullanılan repository instance'ı
     * _secretKey: JWT token imzalama için kullanılan gizli anahtar
     * _audience: JWT token'ın hedef kitlesi
     * _issuer: JWT token'ı oluşturan kaynak
     * _tokenExpirationInHours: Token'ın geçerlilik süresi (saat cinsinden)
     */
    private readonly IUserRepository _userRepository;
    private readonly string _secretKey, _audience, _issuer;
    private readonly int _tokenExpirationInHours;

    /*
     * AuthService sınıfı, JWT tabanlı kimlik doğrulama işlemlerini yürütür. 
     * Bu sınıf, kullanıcı verilerini almak için IUserRepository arayüzünü kullanır 
     * ve JWT yapılandırma bilgilerini almak için IConfiguration arayüzünden yararlanır. 
     * Constructor, JWT için gerekli yapılandırma değerlerini kontrol eder ve 
     * herhangi bir değer eksikse InvalidOperationException fırlatır. 
     * Token süresini yapılandırmadan alır, belirtilmemişse varsayılan olarak 24 saat kullanır.
     */
    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _secretKey = configuration["Jwt:Key"] 
                     ?? throw new InvalidOperationException("JWT Key is not configured.");
        _audience = configuration["Jwt:Audience"] 
                    ?? throw new InvalidOperationException("JWT Audience is not configured.");
        _issuer = configuration["Jwt:Issuer"] 
                  ?? throw new InvalidOperationException("JWT Issuer is not configured.");
        _tokenExpirationInHours = int.Parse(configuration["Jwt:ExpirationHours"] ?? "24");
    }

    /*
     * GenerateToken metodu, belirtilen kullanıcıya dayalı olarak bir JWT token oluşturur.
     *
     * Parametreler:
     * - user: Token oluşturulacak kullanıcı bilgisi.
     *
     * Geri Dönüş Değeri:
     * - Kullanıcı için oluşturulmuş JWT token.
     */
    public string GenerateToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim("IsAuthenticated", "true")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddHours(_tokenExpirationInHours);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /*
     * RegisterUserAsync metodu, yeni bir kullanıcıyı kaydeder ve başarılı kayıt durumunda giriş yanıtı döner.
     *
     * Parametreler:
     * - registerDto: Kayıt edilecek kullanıcı bilgilerini içeren DTO.
     *
     * Geri Dönüş Değeri:
     * - Kayıt başarılı olursa LoginResponseDTO döner.
     *
     * Hatalar:
     * - ArgumentNullException: Kullanıcı adı veya şifre boşsa.
     * - InvalidOperationException: Kullanıcı adı zaten kullanılıyorsa veya kayıt işlemi başarısız olursa.
     */
    public async Task<LoginResponseDTO> RegisterUserAsync(RegisterDTO registerDto)
    {
        if (string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Password))
        {
            throw new ArgumentNullException(nameof(registerDto), "Kullanıcı adı veya şifre boş olamaz.");
        }

        if (await UserExistsAsync(registerDto.Username))
        {
            throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor.");
        }
        
        ValidatePassword(registerDto.Password);
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var user = new User
        {
            Username = registerDto.Username,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Password = hashedPassword
        };

        await _userRepository.AddAsync(user);
        if (!await _userRepository.SaveChangesAsync())
        {
            throw new InvalidOperationException("Kullanıcı kaydı yapılırken bir hata oluştu.");
        }

        var token = GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(_tokenExpirationInHours);

        return new LoginResponseDTO(token, expiresAt);
    }

    /*
     * LoginAsync metodu, kullanıcı adı ve şifre ile giriş yapar ve başarılı giriş durumunda JWT token döner.
     *
     * Parametreler:
     * - loginDto: Giriş yapmak için gerekli kullanıcı bilgilerini içeren DTO.
     *
     * Geri Dönüş Değeri:
     * - Giriş başarılı olursa LoginResponseDTO döner.
     *
     * Hatalar:
     * - ArgumentNullException: Kullanıcı adı veya şifre boşsa.
     * - UnauthorizedAccessException: Kullanıcı adı veya şifre geçersizse.
     */
    public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
        {
            throw new ArgumentNullException(nameof(loginDto), "Kullanıcı adı veya şifre boş olamaz.");
        }

        var user = await _userRepository.GetByUsername(loginDto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Geçersiz kullanıcı adı veya şifre.");
        }

        var token = GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(_tokenExpirationInHours);

        return new LoginResponseDTO(token, expiresAt);
    }

    /*
     * UserExistsAsync metodu, kullanıcı adının mevcut olup olmadığını kontrol eder.
     *
     * Parametreler:
     * - username: Kontrol edilecek kullanıcı adı.
     *
     * Geri Dönüş Değeri:
     * - Kullanıcı adı mevcutsa true, yoksa false döner.
     *
     * Hatalar:
     * - ArgumentNullException: Kullanıcı adı boşsa.
     */
    public async Task<bool> UserExistsAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException(nameof(username), "Kullanıcı adı boş olamaz.");
        }

        var user = await _userRepository.GetByUsername(username);
        return user != null;
    }

    /*
     * ValidatePassword metodu, şifrenin belirli kurallara göre doğruluğunu sağlar.
     * 
     * Parametre:
     * - Şifre
     * 
     * Hatalar:
     * - ArgumentException: Şifre belirlenen kurallara uymuyorsa fırlatır.
     */
    public void ValidatePassword(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(password))
            errors.Add("Şifre boş olamaz.");
        else
        {
            if (password.Length < 6)
                errors.Add("Şifre en az 6 karakter uzunluğunda olmalıdır.");
            if (!password.Any(char.IsUpper))
                errors.Add("Şifre en az bir büyük harf içermelidir.");
            if (!password.Any(char.IsLower))
                errors.Add("Şifre en az bir küçük harf içermelidir.");
            if (!password.Any(char.IsDigit))
                errors.Add("Şifre en az bir rakam içermelidir.");
        }

        if (errors.Any())
            throw new CustomException(string.Join(" ", errors), 400);
    }
}