using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApplication1.Data;
using WebApplication1.Features.Users;

var builder = WebApplication.CreateSlimBuilder(args);



builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("myDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddValidation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapUserEndpoints();


app.Run();


