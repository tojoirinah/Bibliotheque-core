using System.Threading.Tasks;

using Bibliotheque.Commands.Domains.Entities;

namespace Bibliotheque.Commands.Domains.Contracts
{
    public interface IRepository<TId, TEntity> where TEntity : BaseEntity<TId> where TId : struct
    {
        Task SubscribeItemAsync(TEntity newItem);
        Task ChangeItemAsync(TEntity updatedItem);
        Task UnsubscribeItemAsync(TEntity removalItem);
    }
}
