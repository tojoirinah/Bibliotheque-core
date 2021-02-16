
using Bibliotheque.Api.Resp;

using MediatR;

namespace Bibliotheque.Api.Queries
{
    public interface IQuery<out Entity> : IRequest<Entity> where Entity : IBaseResp
    {
    }
}
