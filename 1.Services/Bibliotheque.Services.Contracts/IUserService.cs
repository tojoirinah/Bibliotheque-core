using System.Collections.Generic;
using System.Threading.Tasks;

using Bibliotheque.Services.Contracts.Requests.Auths;
using Bibliotheque.Services.Contracts.Requests.Statuses;
using Bibliotheque.Services.Contracts.Requests.Users;

using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Services.Contracts
{
    public interface IUserService
    {
        Task RegisterUserAsync(CUser userToRegister);
        Task ChangeUserInformationAsync(UpdateInformationUserReq req);
        Task ChangeUserPasswordAsync(UpdatePasswordUserReq req);
        Task ChangeUserStatusAsync(UpdateUserStatusReq req);
        Task UnregisterUserAsync(long userId);


        Task<QUser> RetrieveOneUserByUserName(string username);
        Task<QUser> RetrieveOneUserById(long id);
        Task<List<QUser>> SearchUser(byte roleId, string querySearch = "");
        Task<QUser> Authenticate(AuthenticationReq req);
    }
}
