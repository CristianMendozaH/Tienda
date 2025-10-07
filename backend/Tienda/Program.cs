using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Tienda.Data;
using Tienda.Services;

var builder = WebApplication.CreateBuilder(args);

// =======================
// 🔹 CONFIGURACIÓN DE SERVICIOS
// =======================

// Controladores
builder.Services.AddControllers();

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios personalizados (Inyección de dependencias)
builder.Services.AddScoped<AuthService>();

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// =======================
// 🔒 CONFIGURACIÓN JWT
// =======================
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// =======================
// 📘 CONFIGURACIÓN SWAGGER
// =======================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Tienda",
        Version = "v1",
        Description = "API segura con autenticación JWT para Tienda",
        Contact = new OpenApiContact
        {
            Name = "Cristian Mendoza",
            Email = "admin@tienda.com"
        }
    });

    // 🔒 Configuración de Autorización en Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Introduce el token JWT con el prefijo 'Bearer ' (ejemplo: Bearer eyJhbGci...)",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

// =======================
// 🚀 CONSTRUCCIÓN DEL APP
// =======================
var app = builder.Build();

// Middleware de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS + CORS
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Rutas de controladores
app.MapControllers();

// Ruta raíz de prueba
app.MapGet("/", () => Results.Ok("✅ API TiendaAmiga en ejecución correctamente."))
   .AllowAnonymous();

// Inicia la app
app.Run();
