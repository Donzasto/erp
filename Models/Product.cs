using System.ComponentModel.DataAnnotations.Schema;

[Table("product")]
public class Product
{
    [Column("id")]
    public int Id { get; set; }
    [Column("code")]
    public string? Code { get; set; }
    [Column("name")]
    public string? Name { get; set; }
}