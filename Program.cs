using System.Security.Claims;
using System.Text.Json;
using App_TODO_API.Map;
using App_TODO_API.Requests;
using App_TODO_API.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LoginService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapRegisterEndpoint();
app.MapLoginEndpoint();
app.MapRefreshEndpoint();

app.Run();
