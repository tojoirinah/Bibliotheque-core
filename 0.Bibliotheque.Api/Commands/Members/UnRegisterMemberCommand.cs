
using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UnRegisterMemberCommand : UnRegisterUserCommand
    {

        public UnRegisterMemberCommand(long id) : base(id)
        {
        }
    }
}
