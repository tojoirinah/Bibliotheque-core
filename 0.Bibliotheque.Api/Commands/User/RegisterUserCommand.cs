using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bibliotheque.Api.Req.Users;

using MediatR;

namespace Bibliotheque.Api.Commands.User
{
    public class RegisterUserCommand : ICreateCommand<RegisterUserReq>
    {
    }
}
