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

using CIUserRepository = Bibliotheque.Commands.Domains.Contracts.IUserRepository;
using QIUserRepository = Bibliotheque.Queries.Domains.Contracts.IUserRepository;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Specs.Steps.User
{
    public class BaseUserStep
    {
        protected IUserService _userService;
        protected readonly Fixture _fixture = new Fixture();

        protected BaseUserStep()
        {
            SetupUsers();
        }

        void SetupUsers()
        {
            var status = _fixture.Build<Status>().With(u => u.Id, 1).Create();

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
                               .With(u => u.Role, role)
                               .With(u => u.UserStatus, status)
                               .Create();
                return adminUser;
            }

            QUser SetUpDisabledUser()
            {
                var role = _fixture.Build<Role>().With(u => u.Id, 4).Create();
                var securitySalt = "securitySalt_123456";
                var password = PasswordContractor.GeneratePassword("123456", securitySalt);
                var disabledStatus = _fixture.Build<Status>()
                                             .With(x => x.Id, (byte)EStatus.DISABLED).Create();

                var user = _fixture.Build<QUser>()
                               .With(u => u.Login, "member_01@test.com")
                               .With(u => u.SecuritySalt, securitySalt)
                               .With(u => u.Password, password)
                               .With(u => u.Role, role)
                               .With(u => u.UserStatus, disabledStatus)
                               .Create();
                return user;
            }

            QUser SetUpWaitingUser()
            {
                var role = _fixture.Build<Role>().With(u => u.Id, 4).Create();
                var securitySalt = "securitySalt_123456";
                var password = PasswordContractor.GeneratePassword("123456", securitySalt);
                var waitingStatus = _fixture.Build<Status>()
                                             .With(x => x.Id, (byte)EStatus.WAITING).Create();

                var user = _fixture.Build<QUser>()
                               .With(u => u.Login, "member_02@test.com")
                               .With(u => u.SecuritySalt, securitySalt)
                               .With(u => u.Password, password)
                               .With(u => u.Role, role)
                               .With(u => u.UserStatus, waitingStatus)
                               .Create();
                return user;
            }

            var roleMember = _fixture.Build<Role>().With(u => u.Id, 4).Create();
            var mockQRepository = new Mock<QIUserRepository>();
            var listUser = new List<QUser>();
            listUser.Add(SetUpAdmin());
            listUser.Add(SetUpWaitingUser());
            listUser.Add(SetUpDisabledUser());
            for (var i = 0; i < 10; i++)
            {
                var user = _fixture.Build<QUser>()
                               .With(u => u.Login, $"member_{i}@test.com")
                               .With(u => u.Role, roleMember)
                               .With(u => u.UserStatus, status)
                               .Create();
                listUser.Add(user);
            }

            // RetrieveAllAsync
            populateRetrieveAllAsyncMock(mockQRepository, listUser);

            // RetrieveOneAsync
            populateRetrieveOneAsync(mockQRepository, listUser);


            var mockUow = new Mock<IUnitOfWork>();
            var mockCRepository = new Mock<CIUserRepository>();
            _userService = new UserService(mockCRepository.Object, mockQRepository.Object, mockUow.Object);
        }

        void populateRetrieveOneAsync(Mock<QIUserRepository> mockQRepository, List<QUser> listUser) {
            mockQRepository.Setup(x => x.RetrieveOneAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                  .Returns<string, Dapper.DynamicParameters, CommandType>((sp, p, tp) => {
                      string login = string.Empty;
                      long id = -1;

                      try { login = p.Get<dynamic>("login"); } catch { }
                      try { id = p.Get<dynamic>("userid"); } catch { }

                      if (!string.IsNullOrEmpty(login))
                          return Task.FromResult(listUser.FirstOrDefault(x => x.Login == login));
                      else if(id > 0)
                          return Task.FromResult(listUser.FirstOrDefault(x => x.Id == id));

                      return Task.FromResult<QUser>(null);
                  });
        }

        void populateRetrieveAllAsyncMock(Mock<QIUserRepository> mockQRepository, List<QUser> listUser)
        {
            mockQRepository.Setup(x => x.RetrieveAllAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                         .Returns<string, Dapper.DynamicParameters, CommandType>((sp, p, tp) => {

                             string criteria = p.Get<string>("criteria");
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
