using PasswordVaultApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordVaultApi.Data;
using PasswordVaultApi.Models;

namespace PasswordVaultApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordsController : ControllerBase
{
    private readonly VaultContext _context;
    private readonly EncryptionService _encryptionService;

    // The Service is "Injected" here automatically by ASP.NET
    public PasswordsController(VaultContext context, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PasswordEntry>>> GetPasswords()
    {
        var passwords = await _context.Passwords.ToListAsync();

        // Decrypt them before sending back to the user
        foreach (var entry in passwords)
        {
            entry.EncryptedPassword = _encryptionService.Decrypt(entry.EncryptedPassword, entry.EncryptionIv);
        }

        return passwords;
    }

    [HttpPost]
    public async Task<ActionResult<PasswordEntry>> PostPassword(PasswordEntry entry)
    {
        // Encrypt the password before it touches the database
        var (cipherText, iv) = _encryptionService.Encrypt(entry.EncryptedPassword);

        entry.EncryptedPassword = cipherText;
        entry.EncryptionIv = iv;

        _context.Passwords.Add(entry);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPasswords), new { id = entry.Id }, entry);
    }
    // POST: api/passwords/decrypt-test
    [HttpPost("decrypt-test")]
    public ActionResult<string> DecryptManual(string cipherText, string iv)
    {
    try
    {
        return _encryptionService.Decrypt(cipherText, iv);
    }
    catch
    {
        return BadRequest("Could not decrypt. Check your strings!");
    }
}
}
