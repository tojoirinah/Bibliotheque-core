using System.Collections.Generic;
using System.Threading.Tasks;

using Bibliotheque.Services.Contracts.Requests;

using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Services.Contracts
{
    public interface IUserService
    {
        Task RegisterUserAsync(CUser userToRegister);
        Task ChangeUserInformationAsync(UserInformationReq req);
        Task ChangeUserStatusAsync(UserStatusReq req);
        Task UnregisterUserAsync(long userId);
        

        Task<QUser> RetrieveOneUserByUserName(string username);
        Task<QUser> RetrieveOneUserById(long id);
        Task<List<QUser>> SearchUser(string querySearch = "");
        Task<QUser> Authenticate(AuthReq req);
    }
}
