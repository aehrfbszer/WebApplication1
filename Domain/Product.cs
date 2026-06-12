namespace WebApplication1.Domain;

public class Product(string name, decimal price)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
}