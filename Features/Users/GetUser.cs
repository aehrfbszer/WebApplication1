using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Features.Users;


public class GetUser
{
    public static async Task<Results<Ok<UserDto>, NotFound>> GetById(Guid id, AppDbContext db)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var userDto = new UserDto(user.Id, user.Name, user.Email);
        return TypedResults.Ok(userDto);
    }

    public record Request(

               string? Name,
               string? Email,
                    int Page = 1,
            int PageSize = 10
        );

    public static async Task<Results<Ok<PagedResponse<UserDto>>, NotFound>> GetList([AsParameters] Request request, AppDbContext db)
    {

        var query = db.Users.AsNoTracking();



        // 2. 现代 Like 模糊查询（完美生成参数化 SQL，避免 SQL 注入且走数据库索引优化）
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(u => u.Name.Contains(request.Name)); // EF Core 会智能翻译成 SQL 的 LIKE '%xxx%'
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            query = query.Where(u => u.Email.Contains(request.Email)); // EF Core 会智能翻译成 SQL 的 LIKE '%xxx%'
        }

        // 3. 🎯 高性能并发/异步统计符合条件的总条数（必须在 Skip/Take 之前执行）
        var totalCount = await query.CountAsync();

        // 如果总数为 0，直接短路返回，省去后续无意义的分页查询与内存分配
        if (totalCount is 0)
        {
            return TypedResults.NotFound();
        }

        // 4. 确保分页入参有安全的默认值（防止前端传 0 或负数导致数据库报错）
        int page = request.Page > 0 ? request.Page : 1;
        int pageSize = request.PageSize is > 0 and <= 100 ? request.PageSize : 10; // 限制单页最大 100 条

        // 5. 链式拼接：排序 -> 分页 -> 编译期 DTO 投影 -> 一步到位查出数据
        var userDtos = await query
            .OrderByDescending(u => u.CreatedAt) // 分页前必须排序，否则 SQL 分页结果可能乱序
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.Name, u.Email)) // 只向数据库索要 3 列，极致节省带宽
            .ToListAsync();

        // 6. 返回符合现代规范的统一分页响应体
        return TypedResults.Ok(
            new PagedResponse<UserDto>(
                TotalCount: totalCount,
                Page: page,
                PageSize: pageSize,
                Items: userDtos
            )
        );
    }
}