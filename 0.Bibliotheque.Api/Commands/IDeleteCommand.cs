
using Bibliotheque.Api.Req;

using MediatR;

namespace Bibliotheque.Api.Commands
{
    public interface IDeleteCommand<TEntity> : IRequest where TEntity : class, IBaseReq
    {
    }
}
