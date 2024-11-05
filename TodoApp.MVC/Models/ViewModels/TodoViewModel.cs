// Models/ViewModels/TodoViewModel.cs
namespace TodoApp.MVC.Models.ViewModels;

/*
 * TodoViewModel sınıfı, kullanıcı arayüzünde (View) yapılacaklar listesinin görüntülenmesi için kullanılan veri
 * modelini temsil eder. Bu model, yapılacak öğesinin temel bilgilerini içerir.
 *
 * Özellikler:
 * - Title: Yapılacak öğenin başlığı.
 * - Description: Yapılacak öğenin detaylı açıklaması.
 * - IsCompleted: Yapılacak öğenin tamamlanıp tamamlanmadığını belirtir.
 * - CreatedAt: Yapılacak öğenin ne zaman oluşturulduğunu gösterir.
 * - CompletedAt: Yapılacak öğenin tamamlandığı zamanı belirtir. Tamamlanmamışsa null olabilir.
 *
 * Bu sınıf, yapılacak öğelerinin kullanıcı arayüzünde düzgün bir şekilde gösterilmesine yardımcı olur ve kullanıcıya
 * görevlerin durumunu zaman bilgisi ile birlikte sunar.
 */
public class TodoViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}