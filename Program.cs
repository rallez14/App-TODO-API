using System.Text.Json;
using App_TODO_API.Map;
using App_TODO_API.Requests;
using App_TODO_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<RegisterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapRegisterEndpoint();


app.Run();
