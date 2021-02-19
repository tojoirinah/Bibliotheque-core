using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Commands.Members;
using Bibliotheque.Services.Contracts;
using CUser = Bibliotheque.Commands.Domains.Entities.User;

using MediatR;

namespace Bibliotheque.Api.Handlers.Member
{
    public class RegisterMemberCommandHandler : AbstractRequestHandler, IRequestHandler<RegisterMemberCommand>
    {
        private readonly IUserService _userService;

        public RegisterMemberCommandHandler(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<CUser>(request.Model);
            await _userService.RegisterUserAsync(user);
            return Unit.Value;
        }
    }
}
