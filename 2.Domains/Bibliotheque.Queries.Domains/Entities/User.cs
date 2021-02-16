using System;

namespace Bibliotheque.Queries.Domains.Entities
{
    public class User : BaseEntity<long>
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Login { get; set; }

        public DateTime DateCreated { get; set; }

        public string Password { get; set; }

        public string SecuritySalt { get; set; }

        public byte RoleId { get; set; }

        public string RoleName { get; set; }

        public byte StatusId { get; set; }

        public string StatusName { get; set; }
    }

}
