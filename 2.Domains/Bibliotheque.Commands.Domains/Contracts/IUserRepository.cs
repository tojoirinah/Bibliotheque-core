
using Bibliotheque.Commands.Domains.Entities;

namespace Bibliotheque.Commands.Domains.Contracts
{
    public interface IUserRepository : IRepository<long, User>
    {
    }
}
