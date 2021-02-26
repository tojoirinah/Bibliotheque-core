using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Commands.Members;
using Bibliotheque.Api.Commands.Users;
using Bibliotheque.Api.Queries.Users;
using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Services.Contracts;
using Bibliotheque.Services.Contracts.Requests.Users;

using MediatR;

using CUser = Bibliotheque.Commands.Domains.Entities.User;

namespace Bibliotheque.Api.Handlers
{
    public class UserHandler : AbstractRequestHandler
        ,IRequestHandler<RegisterUserCommand>
        , IRequestHandler<UnRegisterUserCommand>
        , IRequestHandler<UpdateInformationUserCommand>
        , IRequestHandler<UpdatePasswordUserCommand>
    ,IRequestHandler<GetUsersSearchQuery, IEnumerable<UserInformationResp>>
    ,IRequestHandler<GetUserByIdQuery, UserInformationResp>
    ,IRequestHandler<GetUserByUserNameQuery, UserInformationResp>
    {
        protected readonly IUserService _userService;

        public UserHandler(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
        }

        #region "COMMANDS"
        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<CUser>(request.Model);
            await _userService.RegisterUserAsync(user);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UnRegisterUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.UnregisterUserAsync(request.Id);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateInformationUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UpdateInformationUserReq>(request.Model);
            await _userService.ChangeUserInformationAsync(user);
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdatePasswordUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UpdatePasswordUserReq>(request.Model);
            await _userService.ChangeUserPasswordAsync(user);
            return Unit.Value;
        }
        #endregion

        #region "QUERY"
        public async Task<UserInformationResp> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.RetrieveOneUserById(request.Id);
            return _mapper.Map<UserInformationResp>(user);
        }

        public async Task<IEnumerable<UserInformationResp>> Handle(GetUsersSearchQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.SearchUser(request.RoleId, request.Criteria);
            var list =  _mapper.Map<IEnumerable<UserInformationResp>>(users);
            return list;
        }

        public async Task<UserInformationResp> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.RetrieveOneUserByUserName(request.UserName);
            return _mapper.Map<UserInformationResp>(user);
        }
        #endregion
    }
}
