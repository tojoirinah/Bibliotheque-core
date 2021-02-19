
using Bibliotheque.Api.Req.Members;

using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UpdatePasswordMemberCommand : IRequest
    {
        public UpdatePasswordMemberReq Model { get; }

        public UpdatePasswordMemberCommand(UpdatePasswordMemberReq model)
        {
            Model = model;
        }
    }
}
