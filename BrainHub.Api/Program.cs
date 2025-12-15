using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BrainHub.Api.Data;
using BrainHub.Api.Data.Interface;
using BrainHub.Api.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BrainHubDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BrainHubBackContext") ?? throw new InvalidOperationException("Connection string 'BrainHubBackContext' not found.")));

builder.Services.AddScoped<IArtigoRepository, ArtigoRepository>();

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
