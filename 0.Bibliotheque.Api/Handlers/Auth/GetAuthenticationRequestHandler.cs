using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Queries.Auth;
using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Services.Contracts;
using Bibliotheque.Services.Contracts.Requests;

using MediatR;

namespace Bibliotheque.Api.Handlers.Auth
{
    public class GetAuthenticationRequestHandler : AbstractRequestHandler,  IRequestHandler<GetAuthenticationRequest, UserInformationResp>
    {
        private readonly IUserService _userService;

        public GetAuthenticationRequestHandler(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserInformationResp> Handle(GetAuthenticationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = _mapper.Map<AuthReq>(request);
                var user =  await _userService.Authenticate(req);

                return _mapper.Map<UserInformationResp>(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
