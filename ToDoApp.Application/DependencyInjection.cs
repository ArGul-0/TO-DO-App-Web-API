using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Application.Security.Notes;
using ToDoApp.Application.UseCases.Friends.AcceptFriendRequest;
using ToDoApp.Application.UseCases.Friends.GetAllMyFriendships;
using ToDoApp.Application.UseCases.Friends.GetIncomingFriendshipRequests;
using ToDoApp.Application.UseCases.Friends.RemoveFriendship;
using ToDoApp.Application.UseCases.Friends.RejectFriendRequest;
using ToDoApp.Application.UseCases.Friends.SendFriendRequest;
using ToDoApp.Application.UseCases.Notes.AttachTagToNote;
using ToDoApp.Application.UseCases.Notes.CreateNewNote;
using ToDoApp.Application.UseCases.Notes.DeleteUserNote;
using ToDoApp.Application.UseCases.Notes.DetachTagFromNote;
using ToDoApp.Application.UseCases.Notes.GetAllNotes;
using ToDoApp.Application.UseCases.Notes.GetAllOtherPeopleNotes;
using ToDoApp.Application.UseCases.Notes.GetNoteById;
using ToDoApp.Application.UseCases.Notes.GetUserNotesByTag;
using ToDoApp.Application.UseCases.Notes.UpdateUserNote;
using ToDoApp.Application.UseCases.Tags.CreateNewTag;
using ToDoApp.Application.UseCases.Tags.DeleteUserTag;
using ToDoApp.Application.UseCases.Tags.GetAllMyTags;
using ToDoApp.Application.UseCases.Tags.GetMyTagById;
using ToDoApp.Application.UseCases.Tags.UpdateUserTag;
using ToDoApp.Application.UseCases.Users.ChangeUserVisibility;
using ToDoApp.Application.UseCases.Users.CreateUser;
using ToDoApp.Application.UseCases.Users.DeleteMyAccount;
using ToDoApp.Application.UseCases.Users.GetAllUsers;
using ToDoApp.Application.UseCases.Users.GetCurrentUser;
using ToDoApp.Application.UseCases.Users.GetUserById;
using ToDoApp.Application.UseCases.Users.LoginUser;

namespace ToDoApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateUserHandler>();
            services.AddScoped<LoginUserHandler>();

            services.AddScoped<GetAllUsersHandler>();
            services.AddScoped<GetUserByIdHandler>();
            services.AddScoped<GetCurrentUserHandler>();
            services.AddScoped<ChangeUserVisibilityHandler>();
            services.AddScoped<DeleteMyAccountHandler>();

            services.AddScoped<GetAllNotesHandler>();
            services.AddScoped<GetNoteByIdHandler>();
            services.AddScoped<CreateNewNoteHandler>();
            services.AddScoped<GetAllUserNotesHandler>();
            services.AddScoped<GetUserNotesByTagHandler>();
            services.AddScoped<UpdateUserNoteHandler>();
            services.AddScoped<DeleteUserNoteHandler>();
            services.AddScoped<AttachTagToNoteHandler>();
            services.AddScoped<DetachTagFromNoteHandler>();

            services.AddScoped<GetAllMyTagsHandler>();
            services.AddScoped<GetMyTagByIdHandler>();
            services.AddScoped<CreateNewTagHandler>();
            services.AddScoped<UpdateUserTagHandler>();
            services.AddScoped<DeleteUserTagHandler>();

            services.AddScoped<SendFriendshipRequestHandler>();
            services.AddScoped<AcceptFriendshipRequestHandler>();
            services.AddScoped<RejectFriendshipRequestHandler>();
            services.AddScoped<DeleteFriendship>();
            services.AddScoped<GetIncomingFriendshipRequestsHandler>();
            services.AddScoped<GetAllMyFriendshipsHandler>();

            services.AddScoped<INotesAuthorizationService, NotesAuthorizationService>();

            return services;
        }
    }
}
