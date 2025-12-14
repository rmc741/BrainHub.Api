using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BrainHub.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BrainHubDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BrainHubBackContext") ?? throw new InvalidOperationException("Connection string 'BrainHubBackContext' not found.")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
