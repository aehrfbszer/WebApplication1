namespace WebApplication1.Domain;

public class Todo(string title, bool isCompleted = false)
{
    public Guid Id { get; set; }
    public string Title { get; set; } = title;
    public bool IsCompleted { get; set; } = isCompleted;
}