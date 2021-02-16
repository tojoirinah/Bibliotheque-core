
using Bibliotheque.Api.Req;

using MediatR;

namespace Bibliotheque.Api.Commands
{
    public interface ICreateCommand<TEntity>: IRequest where TEntity: class, IBaseReq
    {
    }
}
