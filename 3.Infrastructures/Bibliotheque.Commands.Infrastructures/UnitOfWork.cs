using System;
using System.Threading.Tasks;

using Bibliotheque.Commands.Domains;
using Bibliotheque.Commands.Domains.Contracts;

namespace Bibliotheque.Commands.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        public BibliothequeContext Context { get; }


        private bool disposed = false;

        public UnitOfWork(BibliothequeContext context)
        {
            Context = context;
        }

        public async Task CommitAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    Context.SaveChanges();
                    Dispose();
                });

            }
            catch (Exception ex)
            {
                Dispose();
                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                Context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task RollBackAsync() => await Task.Run(() => Dispose());
    }
}
