using App_TODO_API.Requests;
using App_TODO_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace App_TODO_API.Map;

public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this WebApplication app)
    {
        app.MapPost("/login", async ([FromServices] LoginService loginService, [FromBody] LoginRequest request) =>
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return Results.BadRequest("Missing credentials");

            var token = await loginService.Login(request.Email, request.Password);

            return token == null
                ? Results.Unauthorized()
                : Results.Ok(token);
        });
    }
}