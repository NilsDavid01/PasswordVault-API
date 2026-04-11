using System.Security.Cryptography;
using System.Text;

namespace PasswordVaultApi.Services;

public class EncryptionService
{
    // Must be exactly 32 characters for AES-256
    private const string ManagedKey = "a-very-secret-32-character-key!!";
    private readonly byte[] _key = Encoding.UTF8.GetBytes(ManagedKey);

    public (string cipherText, string iv) Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV(); // This creates a unique IV for EVERY password

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Convert to Base64 strings so they can be stored as text in SQLite
        return (Convert.ToBase64String(cipherBytes), Convert.ToBase64String(aes.IV));
    }

    public string Decrypt(string cipherText, string iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = _key;
        aes.IV = Convert.FromBase64String(iv);

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
}
