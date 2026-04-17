using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using AvisosAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;

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
builder.Services.AddScoped<AvisosPersonalesService>();
builder.Services.AddScoped<AvisosGeneralesService>();
builder.Services.AddScoped<GruposService>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();