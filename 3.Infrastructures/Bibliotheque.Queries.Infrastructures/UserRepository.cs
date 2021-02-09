
using Bibliotheque.Queries.Domains.Contracts;
using Bibliotheque.Queries.Domains.Entities;

using Microsoft.Extensions.Configuration;

namespace Bibliotheque.Queries.Infrastructures
{
    public class UserRepository : Repository<long, User>, IUserRepository
    {
        public UserRepository(IConfiguration config) : base(config)
        {
        }
    }
}
