using JoyModels.Models.src.Database.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JoyModelsDbContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnectionString"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.8.2-mariadb")
));

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();