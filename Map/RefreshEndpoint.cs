using App_TODO_API.Database;
using App_TODO_API.Models;
using App_TODO_API.Requests;
using App_TODO_API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace App_TODO_API.Map;

public static class RefreshEndpoint
{
    public static void MapRefreshEndpoint(this WebApplication app)
    {
        app.MapPost("/refresh", async ([FromBody] RefreshRequest request, [FromServices] IConfiguration config) =>
        {
            var token = await DbManager.GetValidRefreshToken(request.RefreshToken);
            if (token == null)
                return Results.Unauthorized();

            var user = await DbManager.GetUserById(token.UserId);
            if (user == null)
                return Results.Unauthorized();

            var accessToken = TokenHelper.GenerateAccessToken(user, config);
            var newRefreshToken = TokenHelper.GenerateRefreshToken(user, config);

            await DbManager.RevokeRefreshToken(request.RefreshToken);
            await DbManager.StoreRefreshToken(new RefreshToken
            {
                Token = newRefreshToken.Token,
                UserId = user.Id!.Value,
                CreatedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                Revoked = false
            });

            return Results.Ok(new Token(accessToken, newRefreshToken.Token, DateTime.UtcNow.AddHours(1)));
        });
    }
}