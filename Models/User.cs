using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Login), IsUnique = true)]
[Index(nameof(Password), IsUnique = true)]
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