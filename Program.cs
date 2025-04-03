using System.Security.Claims;
using System.Text;
using System.Text.Json;
using App_TODO_API.Database;
using App_TODO_API.Map;
using App_TODO_API.Requests;
using App_TODO_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LoginService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapRegisterEndpoint();
app.MapLoginEndpoint();
app.MapRefreshEndpoint();

app.MapGet("/me", [Authorize] async (ClaimsPrincipal user) =>
{
    var id = int.Parse(user.FindFirst("userId")!.Value);
    var dbUser = await DbManager.GetUserById(id);

    return new
    {
        id = dbUser.Id,
        name = dbUser.Name,
        email = dbUser.Email
    };
});


app.Run();
