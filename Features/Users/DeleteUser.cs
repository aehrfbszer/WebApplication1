namespace WebApplication1.Features.Users;

using WebApplication1.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class DeleteUser
{
    public record Request(IEnumerable<Guid> Ids);
    public static async Task<Results<NoContent, NotFound>> DeleteSome(Request request, AppDbContext db, CancellationToken cancellationToken)
    {
        int deletedCount = await db.Users.Where(u => request.Ids.Contains(u.Id)).ExecuteDeleteAsync(cancellationToken);
        if (deletedCount > 0)
        {
            return TypedResults.NoContent();
        }
        return TypedResults.NotFound();
    }

    public static async Task<Results<NoContent, NotFound>> DeleteById(Guid id, AppDbContext db, CancellationToken cancellationToken)
    {
        int deletedCount = await db.Users.Where(u => u.Id == id).ExecuteDeleteAsync(cancellationToken);
        if (deletedCount > 0)
        {
            return TypedResults.NoContent();
        }
        return TypedResults.NotFound();
    }
}