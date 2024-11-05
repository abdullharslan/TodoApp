using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApp.MVC.Data.Context;
using TodoApp.MVC.Data.Repositories.Abstract;
using TodoApp.MVC.Data.Repositories.Concrete;
using TodoApp.MVC.Middlewares;
using TodoApp.MVC.Services.Abstract;
using TodoApp.MVC.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext Registration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository Registration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

// Service Registration
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToDoService, ToDoService>();

// HttpContext Accessor
builder.Services.AddHttpContextAccessor();

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"] 
    ?? throw new InvalidOperationException("JWT Key is not configured.");
var key = Encoding.ASCII.GetBytes(jwtKey);

// Program.cs'deki authentication kısmını güncelliyoruz
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // (Giriş yolu)
        options.LogoutPath = "/Account/Logout"; // (Çıkış yolu)
        options.Cookie.Name = "TodoApp.Auth"; // (Cookie adı)
        options.Cookie.HttpOnly = true; // (Cookie sadece HTTP üzerinden erişilebilir)
        options.ExpireTimeSpan = TimeSpan.FromHours(24); // (Cookie'nin geçerlilik süresi)
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // (Verici doğrulaması)
            ValidateAudience = true, // (Hedef doğrulaması)
            ValidateLifetime = true, // (Yaşam süresi doğrulaması)
            ValidateIssuerSigningKey = true, // (İmza anahtarı doğrulaması)
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // (Geçerli verici)
            ValidAudience = builder.Configuration["Jwt:Audience"], // (Geçerli hedef)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] 
                                                                               ?? throw new InvalidOperationException(
                                                                                   "JWT Anahtarı yapılandırılmamış."))) 
            // (JWT Anahtarı)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];  // (JWT'yi cookie'den al)
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Global exception handler middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // (Kimlik doğrulama kullan)
app.UseAuthorization(); // (Yetkilendirme kullan)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");  // (Varsayılan rota)

app.Run();