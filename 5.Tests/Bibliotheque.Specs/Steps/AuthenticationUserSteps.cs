using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using AutoFixture;

using Bibliotheque.Commands.Domains.Contracts;
using Bibliotheque.Queries.Domains.Contracts;
using Bibliotheque.Queries.Domains.Entities;
using Bibliotheque.Services.Contracts;
using Bibliotheque.Services.Contracts.Requests;
using Bibliotheque.Services.Implementations;
using Bibliotheque.Transverse.Helpers;
using CIUserRepository = Bibliotheque.Commands.Domains.Contracts.IUserRepository;
using QIUserRepository = Bibliotheque.Queries.Domains.Contracts.IUserRepository;
using Moq;

using TechTalk.SpecFlow;
using FluentAssertions;
using Bibliotheque.Services.Implementations.Exceptions;
using Bibliotheque.Commands.Domains.Enums;

namespace Bibliotheque.Specs.Steps
{
    [Binding]
    public class AuthenticationUserSteps
    {

        private readonly AuthReq _req = new AuthReq();
        private IUserService _userService;
        private readonly Fixture _fixture = new Fixture();
        private User _authenticatedUser;

        public AuthenticationUserSteps()
        {
            SetupUsers();
        }

        void SetupUsers()
        {
            var status = _fixture.Build<Status>().With(u => u.Id, 1).Create();

            User SetUpAdmin()
            {
                var role = _fixture.Build<Role>().With(u => u.Id, 1).Create();
                var securitySalt = "securitySalt_123456";
                var password = PasswordContractor.GeneratePassword("123456", securitySalt);

                var adminUser = _fixture.Build<User>()
                               .With(u => u.Login, "admin@test.com")
                               .With(u => u.SecuritySalt, securitySalt)
                               .With(u => u.Password, password)
                               .With(u => u.Role, role)
                               .With(u => u.UserStatus, status)
                               .Create();
                return adminUser;
            }

            User SetUpDisabledUser()
            {
                var role = _fixture.Build<Role>().With(u => u.Id, 4).Create();
                var securitySalt = "securitySalt_123456";
                var password = PasswordContractor.GeneratePassword("123456", securitySalt);
                var disabledStatus = _fixture.Build<Status>()
                                             .With(x => x.Id, (byte)EStatus.DISABLED).Create();

                var user = _fixture.Build<User>()
                               .With(u => u.Login, "member_01@test.com")
                               .With(u => u.SecuritySalt, securitySalt)
                               .With(u => u.Password, password)
                               .With(u => u.Role, role)
                               .With(u => u.UserStatus, disabledStatus)
                               .Create();
                return user;
            }

            User SetUpWaitingUser()
            {
                var role = _fixture.Build<Role>().With(u => u.Id, 4).Create();
                var securitySalt = "securitySalt_123456";
                var password = PasswordContractor.GeneratePassword("123456", securitySalt);
                var waitingStatus = _fixture.Build<Status>()
                                             .With(x => x.Id, (byte)EStatus.WAITING).Create();

                var user = _fixture.Build<User>()
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
            var listUser = new List<User>();
            listUser.Add(SetUpAdmin());
            listUser.Add(SetUpWaitingUser());
            listUser.Add(SetUpDisabledUser());
            for (var i = 0; i< 10; i++)
            {
                var user = _fixture.Build<User>()
                               .With(u => u.Login, $"member_{i}@test.com")
                               .With(u => u.Role, roleMember)
                               .With(u => u.UserStatus, status)
                               .Create();
                listUser.Add(user);
            }
            mockQRepository.Setup(x => x.RetrieveAllAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                          .Returns(Task.FromResult(listUser));

            mockQRepository.Setup(x => x.RetrieveOneAsync(It.IsAny<string>(), It.IsAny<Dapper.DynamicParameters>(), It.IsAny<CommandType>()))
                          .Returns<string, Dapper.DynamicParameters, CommandType>((sp, p, tp) => Task.FromResult(listUser.FirstOrDefault(user => user.Login == p.Get<dynamic>("login"))));


            var mockUow = new Mock<IUnitOfWork>();
            var mockCRepository = new Mock<CIUserRepository>();
            _userService = new UserService(mockCRepository.Object,mockQRepository.Object,mockUow.Object);
        }



        [Given(@"the login is '(.*)'")]
        public void GivenTheLoginIs(string p0)
        {
            _req.Login = p0;
        }
        
        [Given(@"the password is '(.*)'")]
        public void GivenThePasswordIs(string p0)
        {
            _req.Password = p0;
        }
        
        [When(@"calling authenciation")]
        public async Task WhenCallingAuthenciation()
        {
            try
            {
                _authenticatedUser = await _userService.Authenticate(_req);
            }
            catch (UknownOrDisabledUserException ex)
            {
                ScenarioContext.Current.Add(nameof(UknownOrDisabledUserException), ex);
            }
            catch (CredentialWaitingException ex)
            {
                ScenarioContext.Current.Add(nameof(CredentialWaitingException), ex);
            }
            catch (CredentialException ex)
            {
                ScenarioContext.Current.Add(nameof(CredentialException), ex);
            }
        }
        
        [Then(@"throws uknown or disabled error")]
        public void ThenThrowsNotFoundError()
        {
            var exception = ScenarioContext.Current[nameof(UknownOrDisabledUserException)];
            exception.Should().BeOfType<UknownOrDisabledUserException>();
        }
        
        [Then(@"user is null")]
        public void ThenUserIsNull()
        {
            _authenticatedUser.Should().BeNull();
        }
        
        [Then(@"throws credential error")]
        public void ThenThrowsCredentialError()
        {
            var exception = ScenarioContext.Current[nameof(CredentialException)];
            exception.Should().BeOfType<CredentialException>();
        }

        [Then(@"throws waiting error")]
        public void ThenWaitingError()
        {
            var exception = ScenarioContext.Current[nameof(CredentialWaitingException)];
            exception.Should().BeOfType<CredentialWaitingException>();
        }

        [Then(@"user is not null")]
        public void ThenUserIsNotNull()
        {
            _authenticatedUser.Should().NotBeNull();
        }
    }
}
