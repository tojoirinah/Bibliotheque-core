using Bibliotheque.Services.Contracts.Requests.Users;

using MediatR;

namespace Bibliotheque.Api.Commands.Users
{
    public class RegisterUserCommand : IRequest
    {
        public RegisterUserReq Model { get; }

        public RegisterUserCommand(RegisterUserReq model)
        {
            Model = model;
        }
    }
}
