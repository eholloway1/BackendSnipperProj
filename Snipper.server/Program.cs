using Microsoft.EntityFrameworkCore;
using Snipper.server.Models;
using Snipper.server.Data;
using Snipper.server.Utilities;
using Snipper.server.Services;
using Snipper.server.Middleware;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddDbContext<SnippetContext>(opt => opt.UseInMemoryDatabase("Snippets"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//use dependency injection to register EncryptUtility as a Singleton to be created once and used across the application when needed
builder.Services.AddSingleton<EncryptUtility>();
//add IdentityService for basic auth
builder.Services.AddSingleton<IdentityService>();

//configure JWT settings
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

if(jwtSettings is null || jwtSettings.Secret is null)
{
    throw new ApplicationException("JwtSettings.Secret is null. Ensure it is set in configuration.");
}

var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<BasicAuthMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

SnippetInitializer.Initialize(app.Services.GetRequiredService<IWebHostEnvironment>());

app.Run();
