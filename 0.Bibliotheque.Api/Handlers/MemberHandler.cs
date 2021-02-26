using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Commands.Members;
using Bibliotheque.Services.Contracts;
using Bibliotheque.Services.Contracts.Requests.Members;

using MediatR;

using CUser = Bibliotheque.Commands.Domains.Entities.User;

namespace Bibliotheque.Api.Handlers
{
    public class MemberHandler : AbstractRequestHandler
        , IRequestHandler<RegisterMemberCommand>
        , IRequestHandler<UnRegisterMemberCommand>
        , IRequestHandler<UpdateInformationMemberCommand>
        , IRequestHandler<UpdatePasswordMemberCommand>
    {

        protected  readonly IUserService _userService;

        public MemberHandler(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
        }


        #region "COMMANDS"
        public async Task<Unit> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<CUser>(request.Model);
            await _userService.RegisterUserAsync(user);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UnRegisterMemberCommand request, CancellationToken cancellationToken)
        {
            await _userService.UnregisterUserAsync(request.Id);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateInformationMemberCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UpdateInformationMemberReq>(request.Model);
            await _userService.ChangeUserInformationAsync(user);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdatePasswordMemberCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UpdatePasswordMemberReq>(request.Model);
            await _userService.ChangeUserPasswordAsync(user);
            return Unit.Value;
        }
        #endregion

    }
}
