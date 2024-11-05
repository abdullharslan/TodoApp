# ToDo Application

Bu proje, .NET 8 MVC kullanılarak geliştirilmiş bir yapılacaklar listesi uygulamasıdır. Kullanıcılar, kişisel görevlerini yönetebilir, düzenleyebilir ve takip edebilirler.

## Özellikler

- **Kullanıcı Yönetimi**
  - Kayıt olma
  - Giriş yapma
  - Profil düzenleme
  - Şifre değiştirme
  - Hesap silme

- **Görev Yönetimi**
  - Yeni görev ekleme
  - Görev düzenleme
  - Görev silme
  - Görevleri tamamlandı/tamamlanmadı olarak işaretleme
  - Görevleri filtreleme (Tümü/Tamamlananlar/Devam Edenler)

## Teknolojiler

- .NET 8 MVC
- PostgreSQL
- Entity Framework Core
- JWT Authentication
- Bootstrap 5
- JavaScript/jQuery

## Kurulum

1. PostgreSQL'i yükleyin ve çalıştırın
2. Veritabanını oluşturun
3. Connection string'i `appsettings.json` dosyasında güncelleyin
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ToDoAppDB;Username=your_username;Password=your_password;"
  }
}
```
4. Package Manager Console'da migration'ları çalıştırın:
```
Update-Database
```

## Kullanım

1. Uygulamayı çalıştırın
2. Kayıt olun veya giriş yapın
3. Görev ekleyin, düzenleyin ve yönetin

## Güvenlik

- JWT tabanlı kimlik doğrulama
- Şifre hashleme (BCrypt)
- CSRF koruması
- XSS koruması
- Form validasyonları

## Database Şeması

### Users Tablosu
- Id (int, Primary Key)
- Username (varchar(50))
- Password (varchar(70))
- FirstName (varchar(50))
- LastName (varchar(50))
- CreatedAt (timestamp)
- UpdatedAt (timestamp)
- IsDeleted (boolean)

### ToDoItems Tablosu
- Id (int, Primary Key)
- Title (varchar(255))
- Description (varchar(1000))
- IsCompleted (boolean)
- CompletedAt (timestamp)
- UserId (int, Foreign Key)
- CreatedAt (timestamp)
- UpdatedAt (timestamp)
- IsDeleted (boolean)


## İletişim

Adınız - [@abdullharslan](https://github.com/abdullharslan)

Proje Linki: [https://github.com/abdullharslan/TodoApp]

---
**Not:** Projeyi kullanmadan önce `appsettings.json` dosyasındaki JWT secret key'i değiştirmeyi unutmayın.