namespace WebApplication1.Models;

public class Todo(long id, string name = "", bool isComplete = false)
{
    public long Id { get; set; } = id;
    public string Name { get; set; } = name;
    public bool IsComplete { get; set; } = isComplete;
}