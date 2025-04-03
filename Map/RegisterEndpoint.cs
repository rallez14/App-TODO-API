using System.Text.Json;
using App_TODO_API.Services;
using App_TODO_API.Models;
using App_TODO_API.Requests;
using Microsoft.AspNetCore.Mvc;

namespace App_TODO_API.Map;

public static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this WebApplication app)
    {
        app.MapPost("/api/register", async ([FromServices] RegisterService registerService, [FromBody] RegisterRequest request) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Results.BadRequest("Missing Name field");

            if (string.IsNullOrWhiteSpace(request.Email))
                return Results.BadRequest("Missing Email field");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Results.BadRequest("Missing Password field");

            var result = await registerService.Register(request.Name, request.Email, request.Password);

            return result ? Results.Ok("OK") : Results.Problem("User already exists", statusCode: 409);
        });


    }
}