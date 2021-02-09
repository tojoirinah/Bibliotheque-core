
using Bibliotheque.Commands.Domains.Contracts;
using Bibliotheque.Commands.Domains.Entities;

namespace Bibliotheque.Commands.Infrastructures
{
    public sealed class UserRepository : Repository<long, User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
