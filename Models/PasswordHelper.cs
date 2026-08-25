using System.Security.Cryptography;
using System.Text;

namespace KitabKlubu.Models;

public static class PasswordHelper
{
    public static string Hash(string parol)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(parol);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}