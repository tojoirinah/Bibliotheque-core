using System;

using Bibliotheque.Api.Resp.Role;
using Bibliotheque.Api.Resp.Status;

namespace Bibliotheque.Api.Resp.Users
{
    public class UserInformationResp : IBaseResp
    {
        public long Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Login { get; set; }

        public DateTime DateCreated { get; set; }

        public RoleInformationResp Role { get; set; }

        public StatusInformationResp Status { get; set; }
    }
}
