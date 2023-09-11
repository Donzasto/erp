using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("login")]
    public string? Login { get; set; }

    [Column("password")]
    public string? Password { get; set; }
}