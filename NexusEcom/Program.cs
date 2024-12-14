using NexusEcom.Controllers.Services;
using NexusEcom.Utils;
using NexusEcom.DataAccess.Repositories;
using NexusEcom.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NexusEcom.DataAccess.Mappings;
using NexusEcom.Controllers.Services.Interfaces;
using NexusEcom.DataAccess.Repositories.Interfaces;
using NexusEcom.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddScoped<IIWLocalStorageService, LocalStorageUtil>();

builder.Services.AddRazorComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EcommerceDataBase"));

DefaultConfigs.Initialize(builder.Configuration);

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService>();
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


var app = builder.Build();

app.MapHealthChecks("/health");
app.MapDefaultEndpoints();
app.MapControllers();


builder.Services.AddAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexusEcom API v1"));
}

if (!app.Environment.IsDevelopment())
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
