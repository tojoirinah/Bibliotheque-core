using Bibliotheque.Services.Contracts.Requests.Users;

using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UpdatePasswordUserCommand : IRequest
    {
        public long Id { get; }
        public UpdatePasswordUserReq Model { get; }

        public UpdatePasswordUserCommand(long id, UpdatePasswordUserReq model)
        {
            model.Id = id;
            Model = model;
        }
    }
}
