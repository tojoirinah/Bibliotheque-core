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

        public virtual Role Role { get; set; }

        public virtual Status UserStatus { get; set; }
    }

}
