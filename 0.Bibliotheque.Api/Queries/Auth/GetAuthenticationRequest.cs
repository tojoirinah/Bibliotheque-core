
using Bibliotheque.Api.Resp;
using Bibliotheque.Api.Resp.Users;

namespace Bibliotheque.Api.Queries.Auth
{
    public class GetAuthenticationRequest : IQuery<UserInformationResp>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
