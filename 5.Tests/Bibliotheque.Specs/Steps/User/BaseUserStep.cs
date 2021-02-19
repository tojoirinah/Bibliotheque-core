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
        protected IUserService _userService;
        
        protected ILoggerService _logger;
        protected readonly Fixture _fixture = new Fixture();
        
        protected QUser _qNewOrUpdateUser;

        protected BaseUserStep()
        {
            SetupStep();
        }

        protected override void SetupStep()
        {
            var status = _fixture.Build<Status>().With(u => u.Id, 1).Create();
            var roleMember = _fixture.Build<Role>().With(u => u.Id, 4).Create();

            QUser SetUpAdmin()
            {
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

            QUser SetUpDisabledUser()
            {
                var securitySalt = "securitySalt_123456";
                var password = PasswordContractor.GeneratePassword("123456", securitySalt);
                var disabledStatus = _fixture.Build<Status>()
                                             .With(x => x.Id, (byte)EStatus.DISABLED).Create();

                var user = _fixture.Build<QUser>()
                               .With(u => u.Login, "member_01@test.com")
                               .With(u => u.SecuritySalt, securitySalt)
                               .With(u => u.Password, password)
                               .With(u => u.RoleId, roleMember.Id)
                               .With(u => u.RoleName, roleMember.Name)
                               .With(u => u.StatusId, disabledStatus.Id)
                               .With(u => u.StatusName, disabledStatus.Name)
                               .Create();
                return user;
            }

            QUser SetUpWaitingUser()
            {
                var securitySalt = "securitySalt_123456";
                var password = PasswordContractor.GeneratePassword("123456", securitySalt);
                var waitingStatus = _fixture.Build<Status>()
                                             .With(x => x.Id, (byte)EStatus.WAITING).Create();

                var user = _fixture.Build<QUser>()
                               .With(u => u.Login, "member_02@test.com")
                               .With(u => u.SecuritySalt, securitySalt)
                               .With(u => u.Password, password)
                               .With(u => u.RoleId, roleMember.Id)
                               .With(u => u.RoleName, roleMember.Name)
                               .With(u => u.StatusId, waitingStatus.Id)
                               .With(u => u.StatusName, waitingStatus.Name)
                               .Create();
                return user;
            }

            var mockLogger = new Mock<ILoggerService>();
            _logger = mockLogger.Object;
            var mockQRepository = new Mock<QIUserRepository>();
            var listUser = new List<QUser>();
            listUser.Add(SetUpAdmin());
            listUser.Add(SetUpWaitingUser());
            listUser.Add(SetUpDisabledUser());
            for (var i = 10; i < 20; i++)
            {
                var user = _fixture.Build<QUser>()
                                .With(u => u.Id, i)
                               .With(u => u.Login, $"member_{i}@test.com")
                               .With(u => u.RoleId, roleMember.Id)
                               .With(u => u.RoleName, roleMember.Name)
                               .With(u => u.StatusId, status.Id)
                               .With(u => u.StatusName, status.Name)
                               .Create();
                listUser.Add(user);
            }

            ScenarioContext.Current.Add("listUser", listUser);

            // RetrieveAllAsync
            populateRetrieveAllAsyncMock(mockQRepository);

            // RetrieveOneAsync
            populateRetrieveOneAsync(mockQRepository);


            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(x => x.CommitAsync())
                   .Callback(() => {
                       List<QUser> newListUser = null;
                       try { 
                           newListUser = ScenarioContext.Current.Get<List<QUser>>("newListUser"); 
                       } catch { }
                        
                       if (newListUser != null)
                       {
                           ScenarioContext.Current["listUser"] = newListUser;
                           ScenarioContext.Current["newListUser"] = null;
                           return;
                       }

                       var u = listUser.FirstOrDefault(x => x.Id == _qNewOrUpdateUser.Id);
                       if(u!=null)
                       {
                           var l = listUser.Where(x => x.Id != _qNewOrUpdateUser.Id).ToList();
                           l.Add(_qNewOrUpdateUser);
                           listUser = l;
                       }
                       else
                       {
                           listUser.Add(_qNewOrUpdateUser);
                       }
                       ScenarioContext.Current["listUser"] = listUser;

                   });
            _uow = mockUow.Object;

            
            var mockCRepository = new Mock<CIUserRepository>();
            
            // SubscribeItemAsync
            populateSubscribeItemAsync(mockCRepository);

            // ChangeUserInformationAsync
            populateChangeUserInformationAsync(mockCRepository);

            // UnregisterUserAsync
            populateUnregisterUserAsync(mockCRepository);

            _userService = new UserService(_mapper, _logger, mockCRepository.Object, mockQRepository.Object, _uow);
        }

        void populateSubscribeItemAsync(Mock<CIUserRepository> mockCRepository) {
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

        void populateChangeUserInformationAsync(Mock<CIUserRepository> mockCRepository)
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
                                   RoleId = u.RoleId ,
                                   StatusId = u.StatusId 
                               };
                           });
        }

        void populateRetrieveOneAsync(Mock<QIUserRepository> mockQRepository) {
            mockQRepository.Setup(x => x.RetrieveOneAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                  .Returns<string, Dapper.DynamicParameters, CommandType>((sp, p, tp) => {
                      string login = string.Empty;
                      long id = -1;

                      try { login = p.Get<dynamic>("login"); } catch { }
                      try { id = p.Get<dynamic>("userid"); } catch { }

                      var listUser = ScenarioContext.Current.Get<List<QUser>>("listUser");

                      if (!string.IsNullOrEmpty(login))
                          return Task.FromResult(listUser.FirstOrDefault(x => x.Login == login));
                      else if(id > 0)
                          return Task.FromResult(listUser.FirstOrDefault(x => x.Id == id));

                      return Task.FromResult<QUser>(null);
                  });
        }

        void populateUnregisterUserAsync(Mock<CIUserRepository> mockCRepository)
        {
            mockCRepository.Setup(x => x.UnsubscribeItemAsync(It.IsAny<CUser>()))
                           .Callback<CUser>(u => {
                               var listUser = ScenarioContext.Current.Get<List<QUser>>("listUser");
                               var newListUser = listUser.Where(x => x.Id != u.Id).ToList();
                               ScenarioContext.Current["newListUser"] = newListUser;
                           });
        }

        void populateRetrieveAllAsyncMock(Mock<QIUserRepository> mockQRepository)
        {
            mockQRepository.Setup(x => x.RetrieveAllAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                         .Returns<string, Dapper.DynamicParameters, CommandType>((sp, p, tp) => {

                             string criteria = p.Get<string>("criteria");
                             var listUser = ScenarioContext.Current.Get<List<QUser>>("listUser");
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
    }
}
