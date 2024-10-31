// Models/Domain/Concrete/User.cs
using TodoApp.MVC.Models.Domain.Abstract;

namespace TodoApp.MVC.Models.Domain.Concrete;

/*
 * User sınıfı, uygulamadaki kullanıcıları temsil eder ve EntityBase sınıfından türetilmiştir. Bu sınıf, kullanıcıya
 * özgü Username, Password, FirstName, LastName gibi özellikleri içerir ve her kullanıcıya ait yapılacaklar listesini
 * ToDoItems özelliğiyle tutar.
 *
 * Özellikler:
 * - Username: Kullanıcının benzersiz kullanıcı adı.
 * - Password: Kullanıcının şifresi.
 * - FirstName: Kullanıcının ilk adı.
 * - LastName: Kullanıcının soyadı.
 * - ToDoItems: Kullanıcının yapılacaklar listesi, ToDoItem koleksiyonu olarak saklanır.
 *
 * Yapıcı Metotlar:
 * - Varsayılan yapıcı metot, ToDoItems listesini başlatır, CreatedAt özelliğini ayarlar ve IsDeleted'i false yapar.
 * - Parametreli yapıcı metot, kullanıcı bilgilerini alır, ToDoItems listesini başlatır, CreatedAt özelliğini ayarlar
 * ve IsDeleted'i false yapar.
 */
public class User : EntityBase
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<ToDoItem> ToDoItems { get; set; }

    public User()
    {
        ToDoItems = new List<ToDoItem>();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public User(string username, string password, string firstName, string lastName)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        ToDoItems = new List<ToDoItem>();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }
}