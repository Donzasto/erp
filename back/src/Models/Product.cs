using System.ComponentModel.DataAnnotations.Schema;

[Table("products")]
public class Product
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_nomenclature")]
    public int IdNomenclature { get; set; }

    [Column("count")]
    public double Count { get; set; }
}