using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Api.Req.Members
{
    public class RegisterMemberReq : IBaseReq
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
