using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bibliotheque.Api.Queries.Auth;
using Bibliotheque.Api.Req.Auth;

namespace Bibliotheque.Api.AutoMapper
{
    public class AuthProfile : BaseProfile
    {
        public AuthProfile()
        {
            CreateMap<AuthenticationReq, GetAuthenticationQuery>();
        }
    }
}
