using System.ComponentModel.DataAnnotations;

namespace KitabKlubu.Models;

public class AdminUser
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}