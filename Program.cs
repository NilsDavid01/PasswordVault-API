using Microsoft.EntityFrameworkCore;
using PasswordVaultApi.Data;
using PasswordVaultApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:8080"); 

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite
builder.Services.AddDbContext<VaultContext>(options =>
    options.UseSqlite("Data Source=vault.db"));

// Register Encryption Service
builder.Services.AddSingleton<EncryptionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VaultContext>();
    db.Database.EnsureCreated();
}
app.Run();
