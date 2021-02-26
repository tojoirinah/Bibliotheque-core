using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Queries.Auth;
using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Services.Contracts;
using Bibliotheque.Services.Contracts.Requests.Auths;

using MediatR;

namespace Bibliotheque.Api.Handlers
{
    public class AuthenticationHandler : AbstractRequestHandler, IRequestHandler<GetAuthenticationQuery, UserInformationResp>
    {
        private readonly IUserService _userService;

        public AuthenticationHandler(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserInformationResp> Handle(GetAuthenticationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var req = _mapper.Map<AuthenticationReq>(request.Model);
                var user = await _userService.Authenticate(req);

                return _mapper.Map<UserInformationResp>(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
