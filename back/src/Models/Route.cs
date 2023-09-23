using System.ComponentModel.DataAnnotations.Schema;

[Table("routes")]
public class Route
{
    [Column("id")]
    public int Id { get; set; }
}