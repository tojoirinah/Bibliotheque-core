using System.Collections.Generic;

using Bibliotheque.Api.Resp.Users;

using MediatR;

namespace Bibliotheque.Api.Queries.Users
{
    public class GetUsersSearchQuery : IRequest<IEnumerable<UserInformationResp>>
    {
        public byte RoleId { get; }
        public string Criteria { get; }

        public GetUsersSearchQuery(byte roleId, string criteria)
        {
            RoleId = roleId;
            Criteria = criteria;
        }
    }
}
