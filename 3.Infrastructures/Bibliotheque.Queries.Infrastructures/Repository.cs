using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Bibliotheque.Queries.Domains.Contracts;
using Bibliotheque.Queries.Domains.Entities;

using Dapper;

using Microsoft.Extensions.Configuration;

namespace Bibliotheque.Queries.Infrastructures
{
    public abstract class Repository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : BaseEntity<TId> where TId : struct
    {
        protected readonly IConfiguration _config;
        private readonly string Connectionstring = "BibliothequeConnection";

        public DbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString(Connectionstring));
            }
        }

        protected Repository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<TEntity> RetrieveOneAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var result = await db.QueryAsync<TEntity>(sp, parms, commandType: commandType);
                return result.FirstOrDefault();
            }
        }

        public async Task<List<TEntity>> RetrieveAllAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                var result = await db.QueryAsync<TEntity>(sp, parms, commandType: commandType);
                return result.ToList();
            }
        }


    }
}
