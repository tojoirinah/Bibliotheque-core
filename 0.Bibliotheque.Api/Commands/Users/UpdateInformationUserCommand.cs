using Bibliotheque.Services.Contracts.Requests.Members;
using Bibliotheque.Services.Contracts.Requests.Users;

using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UpdateInformationUserCommand : IRequest
    {
        public long Id { get; }
        public UpdateInformationUserReq Model { get; }

        public UpdateInformationUserCommand(long id, UpdateInformationUserReq model)
        {
            model.Id = id;
            Model = model;

        }
    }
}
