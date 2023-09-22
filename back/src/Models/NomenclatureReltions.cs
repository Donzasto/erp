using System.ComponentModel.DataAnnotations.Schema;

[Table("nomenclature_relations")]
public class NomenclatureRelations
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_parrent")]
    public int IdParrent { get; set; }

    [Column("id_child")]
    public int IdChild { get; set; }

    public Nomenclature NomenclatureParrents { get; set; } = null!;
    public Nomenclature NomenclatureChilds { get; set; } = null!;
}