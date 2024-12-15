using NexusEcom.Controllers.Services;
using NexusEcom.Utils;
using Microsoft.EntityFrameworkCore;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NexusEcom.Controllers.Services.Interfaces;
using NexusEcom.Components;
using NexusEcom.Data.Context;
using NexusEcom.Data.Repositories;
using NexusEcom.Data.Repositories.Interfaces;
using NexusEcom.Data.Mappings;
using NexusEcom.DataAccess.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IIWLocalStorageService, LocalStorageUtil>();

builder.Services.AddRazorComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EcommerceDataBase"));

DefaultConfigs.Initialize(builder.Configuration);

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NexusEcom API", Version = "v1" });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Ensure database creation and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); 
    var users = context.Users.ToList();
    Console.WriteLine($"Seeded {users.Count} users."); // Log seeded user count
}

// Map application endpoints
app.MapHealthChecks("/health");
app.MapDefaultEndpoints();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexusEcom API v1"));
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>();

app.Run();
