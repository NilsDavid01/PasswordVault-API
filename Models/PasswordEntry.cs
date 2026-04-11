using System.ComponentModel.DataAnnotations;

namespace PasswordVaultApi.Models;

public class PasswordEntry
{
    public int Id { get; set; }
    [Required]
    public string Website { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    [Required]
    public string EncryptedPassword { get; set; } = string.Empty;
    public string EncryptionIv { get; set; } = string.Empty;
}
