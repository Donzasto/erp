using System.ComponentModel.DataAnnotations.Schema;

[Table("nomenclature_relations")]
public class NomenclatureRelations
{
    [Column("id")]
    public int Id { get; set; }

    [Column("parrent_id")]
    public int ParrentId { get; set; }

    [Column("child_id")]
    public int ChildId { get; set; }

    public Nomenclature NomenclatureParrents { get; set; } = null!;
    public Nomenclature NomenclatureChilds { get; set; } = null!;
}