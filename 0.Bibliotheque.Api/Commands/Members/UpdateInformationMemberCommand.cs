using Bibliotheque.Services.Contracts.Requests.Members;

using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UpdateInformationMemberCommand : UpdateInformationUserCommand
    {
        public UpdateInformationMemberCommand(long id, UpdateInformationMemberReq model) : base(id, model)
        {

        }
    }
}
