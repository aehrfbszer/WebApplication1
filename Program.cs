using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApplication1.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

Todo[] sampleTodos =
[
    new(1, "Walk the dog"),
    new(2, "Do the dishes", true),
    new(3, "Do the laundry", true),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", true)
];

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", async (TodoDb db) =>
    await db.Todos.ToListAsync()
    )
        .WithName("GetTodos");

todosApi.MapGet("/{id}", async Task<Results<Ok<Todo>, NotFound>> (long id, TodoDb db) =>
    await db.Todos.FindAsync(id) is { } todo
        ? TypedResults.Ok(todo)
        : TypedResults.NotFound())
    .WithName("GetTodoById");

todosApi.MapPost("/", async (Todo todo, TodoDb db) =>
{
    // In a real application, you would save the new todo to the database here.
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/todos/{todo.Id}", todo);
}).WithName("CreateTodo");

todosApi.MapPut("/{id}", async Task<Results<NoContent, NotFound>> (long id, Todo updatedTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null)
    {
        return TypedResults.NotFound();
    }

    // In a real application, you would update the existing todo in the database here.
    todo.Name = updatedTodo.Name;
    todo.IsComplete = updatedTodo.IsComplete;
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}).WithName("UpdateTodo");

todosApi.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (long id, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null)
    {
        return TypedResults.NotFound();
    }

    // In a real application, you would delete the existing todo from the database here.
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}).WithName("DeleteTodo");

app.Run();


[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
