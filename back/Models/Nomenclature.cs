using System.ComponentModel.DataAnnotations.Schema;

[Table("nomenclature")]
public class Nomenclature
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("code")]
    public string? Code { get; set; }
}