
using Bibliotheque.Api.Req.Members;

using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UpdateInformationMemberCommand : IRequest
    {
        public UpdateInformationMemberReq Model { get; }

        public UpdateInformationMemberCommand(UpdateInformationMemberReq model)
        {
            Model = model;
        }
    }
}
