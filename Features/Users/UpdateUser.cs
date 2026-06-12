namespace WebApplication1.Features.Users;

using WebApplication1.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public class UpdateUser
{
    public record Request(Guid? Id, string? Name, string? Email);
    public static async Task<Results<NoContent, NotFound>> Update(Guid id, Request request, AppDbContext db, CancellationToken cancellationToken)
    {
        // 💡 改进 1：如果只通过主键查询，用 FindAsync 性能比 FirstOrDefaultAsync 更好！
        // 因为 FindAsync 会优先在 EF Core 的一级内存缓存里找，找到了就根本不查数据库，且代码更短。
        var user = await db.Users.FindAsync([id], cancellationToken);
        if (user is null) return TypedResults.NotFound();

        // 💡 改进 2：利用 C# 的空值合并赋值运算符（??=）或单行 if 略微精简
        // 保持原本只更新非空字段的局部更新逻辑
        if (!string.IsNullOrWhiteSpace(request.Name)) user.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.Email)) user.Email = request.Email;

        await db.SaveChangesAsync(cancellationToken);

        // 💡 改进 3：如果你引入了对象映射工具（如 Mapster），这一行可以写成 user.Adapt<UserDto>()
        return TypedResults.NoContent();
    }
}