using System.ComponentModel.DataAnnotations.Schema;

[Table("products")]
public class Product
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nomenclature_relations_id")]
    public int NomenclatureRelations { get; set; }

    [Column("count")]
    public double Count { get; set; }
}