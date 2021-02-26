using Bibliotheque.Services.Contracts.Requests.Members;

namespace Bibliotheque.Api.Commands.Members
{
    public class UpdatePasswordMemberCommand : UpdatePasswordUserCommand
    {

        public UpdatePasswordMemberCommand(long id, UpdatePasswordMemberReq model) : base(id, model)
        {
        }
    }
}
