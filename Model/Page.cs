namespace WebApplication1.Model;

public record PagedResponse<T>(int TotalCount, int Page, int PageSize, IReadOnlyList<T> Items);
