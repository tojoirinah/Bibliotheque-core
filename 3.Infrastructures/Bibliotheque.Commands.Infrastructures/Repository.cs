using System.Threading.Tasks;

using Bibliotheque.Commands.Domains;
using Bibliotheque.Commands.Domains.Contracts;
using Bibliotheque.Commands.Domains.Entities;

using Microsoft.EntityFrameworkCore;

namespace Bibliotheque.Commands.Infrastructures
{
    public abstract class Repository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : BaseEntity<TId> where TId : struct
    {
        protected readonly BibliothequeContext _context;
        private readonly DbSet<TEntity> dbSet;

        protected Repository(IUnitOfWork unitOfWork)
        {
            _context = (BibliothequeContext)unitOfWork.Context;
            dbSet = _context.Set<TEntity>();
        }

        public async virtual Task ChangeItemAsync(TEntity updatedItem)
        {
            await Task.Run(() =>
            {

                var entry = _context.Entry<TEntity>(updatedItem);

                if (entry.State == EntityState.Detached)
                {
                    dbSet.Attach(updatedItem);
                    entry.State = EntityState.Modified; // This should attach entity
                }


            });
        }

        public async virtual Task SubscribeItemAsync(TEntity newItem) => await Task.Run(() => _context.Set<TEntity>().Add(newItem));

        public async virtual Task UnsubscribeItemAsync(TEntity removalItem)
        {
            await Task.Run(() =>
            {
                var entry = _context.Entry<TEntity>(removalItem);
                if (entry.State == EntityState.Detached)
                {
                    dbSet.Attach(removalItem);
                }
                dbSet.Remove(removalItem);
            });
        }
    }
}
