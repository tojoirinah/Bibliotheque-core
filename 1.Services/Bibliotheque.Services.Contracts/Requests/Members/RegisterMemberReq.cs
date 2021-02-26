using Bibliotheque.Commands.Domains.Enums;
using Bibliotheque.Services.Contracts.Requests.Users;

namespace Bibliotheque.Services.Contracts.Requests.Members
{
    public class RegisterMemberReq : RegisterUserReq
    {
        public RegisterMemberReq()
        {
            RoleId = (byte)ERole.MEMBER;
        }
    }
}
