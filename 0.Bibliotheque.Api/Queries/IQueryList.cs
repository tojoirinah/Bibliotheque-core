using System.Collections.Generic;

using Bibliotheque.Api.Resp;

using MediatR;

namespace Bibliotheque.Api.Queries
{
    public interface IQueryList<out Entity> : IEnumerable<Entity>, IRequest where Entity : IBaseResp 
    {
    }
}
