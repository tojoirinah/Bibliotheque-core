using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Bibliotheque.Services.Contracts;

using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;
using CIRepository = Bibliotheque.Commands.Domains.Contracts.IUserRepository;
using QIRepository = Bibliotheque.Queries.Domains.Contracts.IUserRepository;
using Bibliotheque.Commands.Domains.Contracts;
using Bibliotheque.Services.Contracts.Requests;
using Bibliotheque.Transverse.Helpers;
using Bibliotheque.Services.Implementations.Exceptions;
using System.Data;
using Bibliotheque.Commands.Domains.Enums;

namespace Bibliotheque.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly CIRepository _cmdRepository;
        private readonly QIRepository _queryRepository;
        private readonly IUnitOfWork _uow;

        public UserService(CIRepository cmdRepository, QIRepository queryRepository, IUnitOfWork uow)
        {
            _cmdRepository = cmdRepository;
            _queryRepository = queryRepository;
            _uow = uow;
        }

        public async Task<QUser> Authenticate(AuthReq req)
        {
            var param = new Dapper.DynamicParameters();
            param.Add("login", req.Login);
            var user = await _queryRepository.RetrieveOneAsync(StoredProcedure.SP_AUTHENTICATION, param, CommandType.StoredProcedure);
            if (user == null || user.UserStatus.Id == (byte)EStatus.DISABLED)
                throw new UknownOrDisabledUserException();

            if (user.UserStatus.Id == (byte)EStatus.WAITING)
                throw new CredentialWaitingException();

            var encryptedPassword = PasswordContractor.GeneratePassword(req.Password, user.SecuritySalt);
            if (user.Password != encryptedPassword)
                throw new CredentialException();

            return user;
        }

        public async Task ChangeUserAsync(CUser userToUpdate)
        {
            try
            {
                await _cmdRepository.ChangeItemAsync(userToUpdate)
                    .ContinueWith((prev) => _uow.CommitAsync());
            }
            catch (Exception ex)
            {
                await _uow.RollBackAsync();
            }
        }

        public async Task RegisterUserAsync(CUser userToRegister)
        {
            try
            {
                await _cmdRepository.SubscribeItemAsync(userToRegister)
                                    .ContinueWith((prev) => _uow.CommitAsync());
            }
            catch (Exception)
            {
                await _uow.RollBackAsync();
            }
        }

        public async Task<QUser> RetrieveOneUserById(long id)
        {
            var param = new Dapper.DynamicParameters();
            param.Add("userid", id);

            var user = await _queryRepository.RetrieveOneAsync(StoredProcedure.SP_GETUSER_BY_ID, param, System.Data.CommandType.StoredProcedure);
            if (user == null)
                throw new UserNotFoundException();

            return user;
        }

        public async Task<QUser> RetrieveOneUserByUserName(string username)
        {
            var param = new Dapper.DynamicParameters();
            param.Add("login", username);

            var user =  await _queryRepository.RetrieveOneAsync(StoredProcedure.SP_GETUSER_BY_USERNAME, param, System.Data.CommandType.StoredProcedure);
            if (user == null)
                throw new UserNotFoundException();

            return user;
        }

        public async Task<List<QUser>> SearchUser(string querySearch = "")
        {
            var param = new Dapper.DynamicParameters();
            param.Add("criteria", querySearch);

            return await _queryRepository.RetrieveAllAsync(StoredProcedure.SP_SEARCH_USERS_BY_CRITERIA, param, System.Data.CommandType.StoredProcedure);
        }

        public async Task UnregisterUserAsync(CUser userToRemove)
        {
            await _cmdRepository.UnsubscribeItemAsync(userToRemove);
        }
    }
}
