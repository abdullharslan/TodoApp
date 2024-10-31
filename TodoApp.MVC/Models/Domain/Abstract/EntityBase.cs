// Models/Domain/Abstract/EntityBase.cs
namespace TodoApp.MVC.Models.Domain.Abstract;

/*
 * EntityBase sınıfı, tüm veritabanı nesneleri için ortak özellikleri tanımlar. Bu sınıf, diğer domain sınıfları
 * tarafından miras alınarak Id, CreatedAt, UpdatedAt ve IsDeleted gibi temel özelliklerin tekrar kullanılmasını sağlar.
 *
 * Id: Her varlık için benzersiz bir kimlik.
 * CreatedAt: Varlığın oluşturulma tarihi ve saati.
 * UpdatedAt: Varlığın son güncellenme tarihi ve saati.
 * IsDeleted: Varlığın silinip silinmediğini belirtir.
 */
public class EntityBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}