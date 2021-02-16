
using Bibliotheque.Api.Req;

using MediatR;

namespace Bibliotheque.Api.Commands
{
    public interface IUpdateCommand<TEntity> : IRequest where TEntity : class, IBaseReq
    {
    }
}
