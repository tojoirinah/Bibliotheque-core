
using Bibliotheque.Api.Resp.Users;

using MediatR;

namespace Bibliotheque.Api.Queries.Users
{
    public class GetUserByUserNameQuery : IRequest<UserInformationResp>
    {
        public string UserName { get; }

        public GetUserByUserNameQuery(string username)
        {
            UserName = username;
        }
    }
}
