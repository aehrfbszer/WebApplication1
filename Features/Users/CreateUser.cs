namespace WebApplication1.Features.Users;

using WebApplication1.Domain;
using WebApplication1.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class CreateUser
{
    public record Request(
        [Required, StringLength(20, MinimumLength = 2)] string Name,
        [Required, EmailAddress] string Email);
    public static async Task<Results<CreatedAtRoute<UserDto>, Conflict<string>>> Create(Request request, AppDbContext db, CancellationToken cancellationToken)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            return TypedResults.Conflict("Email must be unique.");
        }
        var user = new User(request.Name, request.Email);
        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return TypedResults.CreatedAtRoute(new UserDto(user.Id, user.Name, user.Email), "GetUserById", new { user.Id });
    }
}