using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using AvisosAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AvisosEscolaresContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddScoped(typeof(Repository<>));

builder.Services.AddAutoMapper(x=> { }, typeof(Program).Assembly);

builder.Services.AddHttpContextAccessor();  // necesario para leer claims en servicios

builder.Services.AddScoped<RegistroService>();
builder.Services.AddScoped<AuthService>();


var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            // Para una API interna simple no necesitamos validar
            // emisor ni audiencia, así que los desactivamos.
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();



app.UseHttpsRedirection();

// Importante: Authentication antes que Authorization, y ambos antes de MapControllers.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();