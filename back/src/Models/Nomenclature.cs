using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Table("nomenclature")]
[Index(nameof(Code), IsUnique = true)]
public class Nomenclature
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("code")]
    public string Code { get; set; } = null!;

    public List<NomenclatureRelations> IdParrents { get; } = new();
    public List<NomenclatureRelations> IdChilds { get; } = new();
}