
using Bibliotheque.Api.Resp.Users;

using MediatR;

namespace Bibliotheque.Api.Queries.Users
{
    public class GetUserByIdQuery : IRequest<UserInformationResp>
    {
        public long Id { get; }

        public GetUserByIdQuery(long id)
        {
            Id = id;
        }
    }
}
