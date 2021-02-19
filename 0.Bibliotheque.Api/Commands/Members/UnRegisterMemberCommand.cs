
using Bibliotheque.Api.Req.Members;

using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UnRegisterMemberCommand : IRequest
    {
        public UnRegisterMemberReq Model { get; }

        public UnRegisterMemberCommand(UnRegisterMemberReq model)
        {
            Model = model;
        }
    }
}
