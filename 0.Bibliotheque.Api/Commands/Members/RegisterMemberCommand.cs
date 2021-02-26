using Bibliotheque.Api.Commands.Users;
using Bibliotheque.Services.Contracts.Requests.Members;

namespace Bibliotheque.Api.Commands.Members
{
    public class RegisterMemberCommand : RegisterUserCommand
    {

        public RegisterMemberCommand(RegisterMemberReq model) : base(model)
        {
        }
    }
}
