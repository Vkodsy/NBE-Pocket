using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System;
using UserAuthApi.Data;
=======
using NBEProject1.Repositories;
using NBEProject1.Services;
using UserAuthApi.Data;
using UserAuthApi.Services;     // Or NBEProject1.Services
>>>>>>> origin/AAAAAAAAAAAAAAAAAAAAAAAAAAA-TestingBranch

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers
builder.Services.AddControllers();

// 2. Configure In-Memory Database for testing
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AuthTestDb"));

// 3. Register Repositories and Services for Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<AuthService>();

// 4. Swagger/OpenAPI Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

<<<<<<< HEAD
// Database
// -------------------------------------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection is not configured.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
// ------------------------------------------------

=======
>>>>>>> origin/AAAAAAAAAAAAAAAAAAAAAAAAAAA-TestingBranch
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();