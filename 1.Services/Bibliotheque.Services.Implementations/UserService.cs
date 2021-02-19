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
using AutoMapper;

namespace Bibliotheque.Services.Implementations
{
    public class UserService : AbstractService, IUserService
    {
        private readonly CIRepository _cmdRepository;
        private readonly QIRepository _queryRepository;


        public UserService(IMapper mapper, ILoggerService loggerService , CIRepository cmdRepository, QIRepository queryRepository, IUnitOfWork uow)
            :base(uow,mapper,loggerService)
        {
            _cmdRepository = cmdRepository;
            _queryRepository = queryRepository;
        }

        public async Task<QUser> Authenticate(AuthReq req)
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n Login : {req.Login}");
            var param = new Dapper.DynamicParameters();
            
            param.Add("login", req.Login);
            var user = await _queryRepository.RetrieveOneAsync(StoredProcedure.SP_AUTHENTICATION, param, CommandType.StoredProcedure);
            if (user == null || user.StatusId == (byte)EStatus.DISABLED)
            {
                _logger.SetError("--- UknownOrDisabledUserException --- ");
                throw new UknownOrDisabledUserException();
            }
                

            if (user.StatusId == (byte)EStatus.WAITING)
            {
                _logger.SetError("--- CredentialWaitingException --- ");
                throw new CredentialWaitingException();
            }
                

            var encryptedPassword = PasswordContractor.GeneratePassword(req.Password, user.SecuritySalt);
            if (user.Password != encryptedPassword)
            {
                _logger.SetDebug($"Password in datatabase : {user.Password}  |  encryptedPassword : {encryptedPassword}");
                _logger.SetError("--- CredentialException --- ");
                throw new CredentialException();
            }

            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
            return user;
        }

        public async Task ChangeUserInformationAsync(UserInformationReq req)
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n UserId: {req.Id}");
            try
            {
                var userToUpdate = await RetrieveOneUserById(req.Id);
                if (userToUpdate == null) {
                    _logger.SetError("--- UserNotFoundException --- ");
                    throw new UserNotFoundException();
                }
                    

                var userTopUpdateCmd = _mapper.Map<CUser>(userToUpdate);
                userTopUpdateCmd.LastName = req.LastName;
                userTopUpdateCmd.FirstName = req.FirstName;
                await _cmdRepository.ChangeItemAsync(userTopUpdateCmd);
            }
            catch (Exception ex)
            {
                _logger.SetError($"Message : {ex.Message} \n StackTrace : {ex.StackTrace} \n InnerException : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
                throw ex;
            }
            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
        }

        public async Task ChangeUserStatusAsync(UserStatusReq req)
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n UserId: {req.Id}");
            try
            {
                var userToUpdate = await RetrieveOneUserById(req.Id);
                if (userToUpdate == null)
                {
                    _logger.SetError("--- UserNotFoundException ---");
                    throw new UserNotFoundException();
                }
                    

                var userTopUpdateCmd = _mapper.Map<CUser>(userToUpdate);
                userTopUpdateCmd.StatusId = req.StatusId;
                await _cmdRepository.ChangeItemAsync(userTopUpdateCmd);
            }
            catch (Exception ex)
            {
                _logger.SetError($"Message : {ex.Message} \n StackTrace : {ex.StackTrace} \n InnerException : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
                throw ex;
            }
            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
        }

        public async Task RegisterUserAsync(CUser userToRegister)
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n Login: {userToRegister.Login}");
            try
            {
                var u = await RetrieveOneUserByUserName(userToRegister.Login);
                if (u != null)
                {
                    _logger.SetError("--- UserAlreadyExistsException ---");
                    throw new UserAlreadyExistsException();
                }
                    
                await _cmdRepository.SubscribeItemAsync(userToRegister);
            }
            catch (Exception ex)
            {
                _logger.SetError($"Message : {ex.Message} \n StackTrace : {ex.StackTrace} \n InnerException : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
                throw ex;
            }
            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
        }

        public async Task<QUser> RetrieveOneUserById(long id)
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n UserId: {id}");
            var param = new Dapper.DynamicParameters();
            param.Add("userid", id);

            var user = await _queryRepository.RetrieveOneAsync(StoredProcedure.SP_GETUSER_BY_ID, param, System.Data.CommandType.StoredProcedure);
            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
            return user;
        }

        public async Task<QUser> RetrieveOneUserByUserName(string username)
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n UserName: {username}");
            var param = new Dapper.DynamicParameters();
            param.Add("login", username);

           var user =  await _queryRepository.RetrieveOneAsync(StoredProcedure.SP_AUTHENTICATION, param, System.Data.CommandType.StoredProcedure);
            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
            return user;
        }

        public async Task<List<QUser>> SearchUser(string querySearch = "")
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n querySearch: {querySearch}");
            var param = new Dapper.DynamicParameters();
            param.Add("criteria", querySearch);

            var list =  await _queryRepository.RetrieveAllAsync(StoredProcedure.SP_SEARCH_USER, param, System.Data.CommandType.StoredProcedure);
            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
            return list;
        }

        public async Task UnregisterUserAsync(long userId)
        {
            _logger.SetDebug($"--- STARTING {System.Reflection.MethodBase.GetCurrentMethod().Name } --- \n UserId: {userId}");
            try
            {
                var qUserToRemove = await RetrieveOneUserById(userId);
                if(qUserToRemove != null)
                {
                    var cUserToRemove = _mapper.Map<CUser>(qUserToRemove);
                    await _cmdRepository.UnsubscribeItemAsync(cUserToRemove);
                }
            }
            catch(Exception ex)
            {
                _logger.SetError($"Message : {ex.Message} \n StackTrace : {ex.StackTrace} \n InnerException : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
                throw ex;
            }
            _logger.SetDebug($"--- ENDING {System.Reflection.MethodBase.GetCurrentMethod().Name } ---");
        }
    }
}
