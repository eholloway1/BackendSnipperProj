using Microsoft.EntityFrameworkCore;
using Snipper.server.Models;
using Snipper.server.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddDbContext<SnippetContext>(opt => opt.UseInMemoryDatabase("Snippets"));

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

SnippetInitializer.Initialize(app.Services.GetRequiredService<IWebHostEnvironment>());

app.Run();
