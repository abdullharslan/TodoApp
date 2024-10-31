// Models/Domain/Concrete/ToDoItem.cs
using TodoApp.MVC.Models.Domain.Abstract;

namespace TodoApp.MVC.Models.Domain.Concrete;

/*
 * ToDoItem sınıfı, kullanıcıların yapılacaklar listesindeki her bir öğeyi temsil eder. EntityBase sınıfından
 * türetilmiştir ve tüm temel özellikleri miras alır. Bu sınıf, yapılacak öğesiyle ilgili başlık, açıklama, tamamlanma
 * durumu ve kullanıcı bilgilerini içerir.
 *
 * Özellikler:
 * - Title: Yapılacak öğenin kısa başlığı.
 * - Description: Yapılacak öğenin detaylı açıklaması.
 * - IsCompleted: Öğenin tamamlanıp tamamlanmadığını belirtir.
 * - CompletedAt: Öğenin tamamlanma tarihi ve saati (eğer tamamlandıysa).
 * - UserId: Yapılacak öğeyi oluşturan kullanıcıya ait kimlik.
 * - User: Yapılacak öğeyi oluşturan kullanıcıya referans, kullanıcı bilgilerine erişim sağlar.
 *
 * Yapıcı Metotlar:
 * - Varsayılan yapıcı metot, CreatedAt özelliğini ayarlar ve IsCompleted'i false olarak başlatır.
 * - Parametreli yapıcı metot, başlık, açıklama ve UserId bilgilerini alır,
 *   IsCompleted'i false olarak başlatır, CreatedAt ve IsDeleted özelliklerini ayarlar.
 */
public class ToDoItem : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public ToDoItem()
    {
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;
    }

    public ToDoItem(string title, string description, int userId)
    {
        Title = title;
        Description = description;
        UserId = userId;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }
}