using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Services.Contracts.Requests.Auths;

using MediatR;

namespace Bibliotheque.Api.Queries.Auth
{
    public class GetAuthenticationQuery : IRequest<UserInformationResp>
    {
        public AuthenticationReq Model { get; }

        public GetAuthenticationQuery(AuthenticationReq model)
        {
            Model = model;
        }
    }
}
