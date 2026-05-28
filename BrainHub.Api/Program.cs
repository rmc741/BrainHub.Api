using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BrainHub.Api.Config;
using BrainHub.Api.Data;
using BrainHub.Api.Data.Interface;
using BrainHub.Api.Data.Repository;
using BrainHub.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BrainHubDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BrainHubBackContext") ?? throw new InvalidOperationException("Connection string 'BrainHubBackContext' not found.")));

builder.Services.AddScoped<IArtigoRepository, ArtigoRepository>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()    // permite qualquer origem
            .AllowAnyMethod()    // permite qualquer método (GET, POST, PUT, DELETE, etc.)
            .AllowAnyHeader();   // permite qualquer cabeçalho
    });
});


builder.Services.AddControllers();
JwtConfig.ConfigureJwt(builder.Services, builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

