using System;
using System.Threading.Tasks;

namespace Bibliotheque.Commands.Domains.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        BibliothequeContext Context { get; }

        Task CommitAsync();

        Task RollBackAsync();
    }
}
