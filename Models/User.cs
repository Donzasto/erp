using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Login), IsUnique = true)]
[Index(nameof(Password), IsUnique = true)]
[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("login")]
    public string? Login { get; set; }

    [Required]
    [Column("password")]
    public string? Password { get; set; }
}