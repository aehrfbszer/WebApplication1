namespace WebApplication1.Features.Users;

public static class UserModule
{


    extension(IEndpointRouteBuilder app)
    {
        public IEndpointRouteBuilder MapUserEndpoints()
        {
            var group = app.MapGroup("/users").WithTags("Users");
            group.MapGet("/", GetUser.GetList).WithName("GetUsers");
            group.MapGet("/{id:guid}", GetUser.GetById).WithName("GetUserById");
            // group.MapPost("/", CreateUser.Create).WithName("CreateUser");
            // group.MapPatch("/{id:guid}", UpdateUser.Update).WithName("UpdateUser");
            // group.MapDelete("/{id:guid}", DeleteUser.DeleteById).WithName("DeleteUserById");
            // group.MapDelete("/", DeleteUser.DeleteSome).WithName("DeleteSomeUsers");
            return app;
        }
    }
}