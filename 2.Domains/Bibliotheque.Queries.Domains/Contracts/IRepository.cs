using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

using Bibliotheque.Queries.Domains.Entities;

using Dapper;

namespace Bibliotheque.Queries.Domains.Contracts
{
    public interface IRepository<TId, TEntity> where TEntity : BaseEntity<TId> where TId : struct
    {
        DbConnection Connection { get; }
        Task<TEntity> RetrieveOneAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<TEntity>> RetrieveAllAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
