using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Interfaces.Repositories;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Friends.AcceptFriendRequest;
using ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests;
using ToDoApp.Application.UseCases.Friends.RejectFriendRequest;
using ToDoApp.Application.UseCases.Friends.RemoveFriendship;
using ToDoApp.Application.UseCases.Friends.SendFriendRequest;
using ToDoApp.Application.UseCases.Friends.GetAllMyFriendships;
using ToDoApp.Application.UseCases.Notes.CreateNewNote;
using ToDoApp.Application.UseCases.Notes.DeleteUserNote;
using ToDoApp.Application.UseCases.Notes.GetAllNotes;
using ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes;
using ToDoApp.Application.UseCases.Notes.GetNoteById;
using ToDoApp.Application.UseCases.Notes.UpdateUserNote;
using ToDoApp.Application.UseCases.Users.ChangeUserVisibility;
using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Application.UseCases.Users.GetAllUsers;
using ToDoApp.Application.UseCases.Users.GetUserById;
using ToDoApp.Application.UseCases.Users.LoginUser;
using ToDoApp.Infrastructure;
using ToDoApp.Infrastructure.Authentication.Jwt;
using ToDoApp.Infrastructure.Authentication.Password;
using ToDoApp.Infrastructure.DependencyInjection;
using ToDoApp.Infrastructure.Repositories;
using ToDoApp.WebApi.Endpoints;
using ToDoApp.WebApi.Extensions;
using ToDoApp.WebApi.Middlewares;

namespace ToDoApp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.AddSerilogLogging();
            builder.AddSwagger();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

            builder.AddAuth();
            builder.AddRateLimiting();

            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IPasswordHasher, Argon2Hasher>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<CreateUserHandler>();
            builder.Services.AddScoped<LoginUserHandler>();
            builder.Services.AddScoped<GetAllUsersHandler>();
            builder.Services.AddScoped<GetUserByIdHandler>();
            builder.Services.AddScoped<GetAllNotesHandler>();
            builder.Services.AddScoped<GetNoteByIdHandler>();
            builder.Services.AddScoped<CreateNewNoteHandler>();
            builder.Services.AddScoped<ChangeUserVisibilityHandler>();
            builder.Services.AddScoped<GetAllUserNotesHandler>();
            builder.Services.AddScoped<UpdateUserNoteHandler>();
            builder.Services.AddScoped<DeleteUserNoteHandler>();

            builder.Services.AddScoped<SendFriendshipsRequestHandler>();
            builder.Services.AddScoped<AcceptFriendshipsRequestHandler>();
            builder.Services.AddScoped<RejectFriendshipsRequestHandler>();
            builder.Services.AddScoped<RemoveFriendshipHandler>();
            builder.Services.AddScoped<GetIncomingFriendshipRequestsHandler>();
            builder.Services.AddScoped<GetAllMyFriendshipsHandler>();

            builder.Services.AddScoped<INotesAuthorizationService, NotesAuthorizationService>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddValidation(); // Add Validation Services

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if(app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseSerilogRequestLogging(); // Enable Serilog Request Logging

            app.MapOpenApi();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseCookiePolicy(
                new CookiePolicyOptions
                {
                    HttpOnly = HttpOnlyPolicy.Always, // Ensure Cookies Are Marked As HttpOnly To Prevent Client-Side Access
                    Secure = CookieSecurePolicy.Always, // Ensure Cookies Are Only Sent Over HTTPS
                    MinimumSameSitePolicy = SameSiteMode.Lax // Set SameSite Policy To Lax To Prevent CSRF Attacks
                });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>(); // Global Exception Handling Middleware

            app.MapAuthEndpoints();
            app.MapUsersEndpoints();
            app.MapNotesEndpoints();
            app.MapFriendsEndpoints();

            app.MigrateDatabase(); // Apply Database Migrations On Startup

            app.MapGet("/health", () => Results.Ok("Healthy")).WithName("HealthCheck"); // Health Check Endpoint

            app.Run();
        }
    }
}
