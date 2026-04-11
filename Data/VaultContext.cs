using Microsoft.EntityFrameworkCore;
using PasswordVaultApi.Models;

namespace PasswordVaultApi.Data;

public class VaultContext : DbContext
{
    public VaultContext(DbContextOptions<VaultContext> options) : base(options) { }
    public DbSet<PasswordEntry> Passwords => Set<PasswordEntry>();
}
