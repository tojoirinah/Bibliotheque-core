
using Bibliotheque.Api.Req.Members;

using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class RegisterMemberCommand : IRequest
    {
        public RegisterMemberReq Model { get; }

        public RegisterMemberCommand(RegisterMemberReq model)
        {
            Model = model;
        }
    }
}
