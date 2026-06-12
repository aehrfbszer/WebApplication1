using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WebApplication1.Data;
using WebApplication1.Features.Users;

var builder = WebApplication.CreateSlimBuilder(args);


builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=app.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddValidation();

var app = builder.Build();

// 2. 自动化建表与初始化数据
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // 🌟 这一行不仅会建表，如果 app.db 文件不存在，它会连文件一起新建！
    context.Database.EnsureCreated();

}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapUserEndpoints();


app.Run();


