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
        Task ChangeUserAsync(CUser userToUpdate);
        Task UnregisterUserAsync(CUser userToRemove);
        

        Task<QUser> RetrieveOneUserByUserName(string username);
        Task<QUser> RetrieveOneUserById(long id);
        Task<List<QUser>> SearchUser(SearchReq req = null);
        Task<QUser> Authenticate(AuthReq req);
    }
}
