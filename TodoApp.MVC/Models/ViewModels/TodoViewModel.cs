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
 *
 * Bu sınıf, yapılacak öğelerinin kullanıcı arayüzünde düzgün bir şekilde gösterilmesine yardımcı olur.
 */
public class TodoViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}