
using MediatR;

namespace Bibliotheque.Api.Commands.Members
{
    public class UnRegisterUserCommand : IRequest
    {
        public long Id { get; }

        public UnRegisterUserCommand(long id)
        {
            Id = id;
        }
    }
}
