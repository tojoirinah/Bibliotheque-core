using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using AutoFixture;

using Bibliotheque.Commands.Domains.Contracts;
using Bibliotheque.Commands.Domains.Enums;
using Bibliotheque.Queries.Domains.Entities;
using Bibliotheque.Services.Contracts;
using Bibliotheque.Services.Implementations;
using Bibliotheque.Transverse.Helpers;

using Moq;

using TechTalk.SpecFlow;

using CIUserRepository = Bibliotheque.Commands.Domains.Contracts.IUserRepository;
using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QIUserRepository = Bibliotheque.Queries.Domains.Contracts.IUserRepository;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Specs.Steps.User
{
    public class BaseUserStep : BaseStep
    {

        protected const string LIST_USER = "listUser";
        private const string NEW_LIST_USER = "newListUser";
        protected IUserService _userService;

        protected ILoggerService _logger;
        protected readonly Fixture _fixture = new Fixture();

        protected QUser _qNewOrUpdateUser;

        protected BaseUserStep()
        {
            SetupStep();
        }

        void SetUpDatas()
        {
            var admin = GetAdmin();
            var disabledMember = GetMemberByStatus(2, (byte)EStatus.DISABLED);
            var waitingMember = GetMemberByStatus(3, (byte)EStatus.WAITING);

            var listUser = new List<QUser>();
            listUser.Add(admin);
            listUser.Add(disabledMember);
            listUser.Add(waitingMember);

            for(var i = 10; i < 20; i++)
            {
                listUser.Add(GetMemberByStatus(i, (byte)EStatus.ENABLED));
            }

            ScenarioContext.Current.Add(LIST_USER, listUser);
        }

        QUser GetAdmin()
        {
            var status = _fixture.Build<Status>().With(u => u.Id, 1).Create();
            var role = _fixture.Build<Role>().With(u => u.Id, 1).Create();
            var securitySalt = "securitySalt_123456";
            var password = PasswordContractor.GeneratePassword("123456", securitySalt);

            var adminUser = _fixture.Build<QUser>()
                           .With(u => u.Id, 1)
                           .With(u => u.Login, "admin@test.com")
                           .With(u => u.LastName, "Administrator")
                           .With(u => u.SecuritySalt, securitySalt)
                           .With(u => u.Password, password)
                           .With(u => u.RoleId, role.Id)
                           .With(u => u.RoleName, role.Name)
                           .With(u => u.StatusId, status.Id)
                           .With(u => u.StatusName, status.Name)
                           .Create();
            return adminUser;
        }

        QUser GetMemberByStatus(long id, byte status)
        {
            var roleMember = _fixture.Build<Role>().With(u => u.Id, 4).Create();
            var securitySalt = "securitySalt_123456";
            var password = PasswordContractor.GeneratePassword("123456", securitySalt);
            var disabledStatus = _fixture.Build<Status>()
                                         .With(x => x.Id, status).Create();

            var user = _fixture.Build<QUser>()
                            .With(u => u.Id, id)
                           .With(u => u.Login, $"member_0{id}@test.com")
                           .With(u => u.SecuritySalt, securitySalt)
                           .With(u => u.Password, password)
                           .With(u => u.RoleId, roleMember.Id)
                           .With(u => u.RoleName, roleMember.Name)
                           .With(u => u.StatusId, disabledStatus.Id)
                           .With(u => u.StatusName, disabledStatus.Name)
                           .Create();
            return user;
        }

        protected override void SetupStep()
        {
            SetUpDatas();
            var mockLogger = new Mock<ILoggerService>();
            _logger = mockLogger.Object;

            var mockUow = GetMockUnitOfWork();
            _uow = mockUow.Object;
            var mockCRepository = GetMockDataCommandRepository();
            var mockQRepository = GetMockDataQueryRepository();

            _userService = new UserService(_mapper, _logger, mockCRepository.Object, mockQRepository.Object, mockUow.Object);
        }

        Mock<IUnitOfWork> GetMockUnitOfWork()
        {
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(x => x.CommitAsync())
                   .Callback(() =>
                   {
                       List<QUser> newListUser = null;
                       try
                       {
                           newListUser = GetListUser(NEW_LIST_USER);
                       }
                       catch { }

                       if (newListUser != null)
                       {
                           SetListUser(LIST_USER, newListUser);
                           SetListUser(NEW_LIST_USER, null);
                           return;
                       }

                       var listUser = GetListUser(LIST_USER);
                       var u = listUser.FirstOrDefault(x => x.Id == _qNewOrUpdateUser.Id);
                       if (u != null && u.Id < 0)
                       {
                           var l = listUser.Where(x => x.Id != _qNewOrUpdateUser.Id).ToList();
                           l.Add(_qNewOrUpdateUser);
                           listUser = l;
                       }
                       else if(u!=null && u.Id > 0 && _qNewOrUpdateUser !=null)
                       {
                           u.Password = _qNewOrUpdateUser.Password;
                           u.SecuritySalt = _qNewOrUpdateUser.SecuritySalt;
                           u.LastName = _qNewOrUpdateUser.LastName;
                           u.FirstName = _qNewOrUpdateUser.FirstName;
                           u.StatusId = _qNewOrUpdateUser.StatusId;
                           u.RoleId = _qNewOrUpdateUser.RoleId;
                       }
                       else
                       {
                           listUser.Add(_qNewOrUpdateUser);
                       }
                       SetListUser(LIST_USER,listUser);

                   });
            return mockUow;
        }

        Mock<QIUserRepository> GetMockDataQueryRepository()
        {
            Mock<QIUserRepository> mockQRepository = new Mock<QIUserRepository>();

            // Get one
            MockRetrieveOneAsync(mockQRepository);

            // Get all
            MockRetrieveAllAsync(mockQRepository);

            return mockQRepository;
        }

        Mock<CIUserRepository> GetMockDataCommandRepository()
        {
            Mock<CIUserRepository> mockCRepository = new Mock<CIUserRepository>();

            // subscribe item
            MockSubscribeItemAsync(mockCRepository);

            // Change user information
            MockChangeUserInformationAsync(mockCRepository);

            // Unregister user
            MockUnregisterUserAsync(mockCRepository);

            return mockCRepository;
        }

        void MockSubscribeItemAsync(Mock<CIUserRepository> mockCRepository)
        {
            mockCRepository.Setup(x => x.SubscribeItemAsync(It.IsAny<CUser>()))
                           .Callback<CUser>(u =>
                           {
                               _qNewOrUpdateUser = new QUser()
                               {
                                   LastName = u.LastName,
                                   FirstName = u.FirstName,
                                   DateCreated = DateTime.Now,
                                   Login = u.Login,
                                   Password = u.Password,
                                   RoleId = u.RoleId,
                                   StatusId = u.StatusId
                               };
                           });
        }

        void MockChangeUserInformationAsync(Mock<CIUserRepository> mockCRepository)
        {
            mockCRepository.Setup(x => x.ChangeItemAsync(It.IsAny<CUser>()))
                           .Callback<CUser>(u =>
                           {
                               _qNewOrUpdateUser = new QUser()
                               {
                                   Id = u.Id,
                                   LastName = u.LastName,
                                   FirstName = u.FirstName,
                                   DateCreated = DateTime.Now,
                                   Login = u.Login,
                                   Password = u.Password,
                                   SecuritySalt = u.SecuritySalt,
                                   RoleId = u.RoleId,
                                   StatusId = u.StatusId
                               };
                           });
        }

       

        void MockUnregisterUserAsync(Mock<CIUserRepository> mockCRepository)
        {
            mockCRepository.Setup(x => x.UnsubscribeItemAsync(It.IsAny<CUser>()))
                           .Callback<CUser>(u =>
                           {
                               var listUser = GetListUser(LIST_USER);
                               var newListUser = listUser.Where(x => x.Id != u.Id).ToList();
                               SetListUser(NEW_LIST_USER, newListUser);
                           });
        }

        void MockRetrieveOneAsync(Mock<QIUserRepository> mockQRepository)
        {
            mockQRepository.Setup(x => x.RetrieveOneAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                  .Returns<string, Dapper.DynamicParameters, CommandType>((sp, p, tp) =>
                  {
                      string login = string.Empty;
                      long id = -1;

                      try { login = p.Get<dynamic>("login"); } catch { }
                      try { id = p.Get<dynamic>("userid"); } catch { }

                      var listUser = GetListUser(LIST_USER);

                      if (!string.IsNullOrEmpty(login))
                          return Task.FromResult(listUser.FirstOrDefault(x => x.Login == login));
                      else if (id > 0)
                          return Task.FromResult(listUser.FirstOrDefault(x => x.Id == id));

                      return Task.FromResult<QUser>(null);
                  });
        }

        void MockRetrieveAllAsync(Mock<QIUserRepository> mockQRepository)
        {
            mockQRepository.Setup(x => x.RetrieveAllAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                         .Returns<string, Dapper.DynamicParameters, CommandType>((sp, p, tp) =>
                         {

                             string criteria = p.Get<string>("criteria");
                             var listUser = GetListUser(LIST_USER);
                             if (string.IsNullOrEmpty(criteria))
                             {
                                 return Task.FromResult(listUser);
                             }

                             var newList = listUser.Where(u => u.LastName.ToLower().Contains(criteria.ToLower()) ||
                                                                        u.FirstName.ToLower().Contains(criteria.ToLower()) ||
                                                                        u.Login.ToLower().Contains(criteria.ToLower()))
                                                            .ToList();
                             return Task.FromResult(newList);
                         });
        }

        protected List<QUser> GetListUser(string key)
        {
            return ScenarioContext.Current.Get<List<QUser>>(key);
        }

        void SetListUser(string key, List<QUser> listUser)
        {
            ScenarioContext.Current[key] = listUser;
        }
    }
}
