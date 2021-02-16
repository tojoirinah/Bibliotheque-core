using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bibliotheque.Services.Contracts.Requests;
using Bibliotheque.Services.Implementations.Exceptions;
using Bibliotheque.Transverse.Helpers;

using FluentAssertions;

using TechTalk.SpecFlow;
using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Specs.Steps.User
{
    [Binding]
    public class UserCommandSteps : BaseUserStep
    {
        private CUser _newOrUpdatedUser;
        private UserInformationReq _uInfoReq;
        private UserStatusReq _uStatusReq;
        private long _userId;

        public UserCommandSteps() : base()
        {
            _newOrUpdatedUser = new CUser();
        }

        [When(@"I enter a new User member or visitor or supervisor or librarian with information '(.*)', '(.*)', '(.*)', '(.*)', '(.*)', (.*), (.*)")]
        public void WhenIEnterANewUserMemberOrVisitorOrSupervisorOrLibrarianWithInformation(string lastname, string firstname, string login, string password, string securitySalt, byte roleId, byte statusId)
        {
            _newOrUpdatedUser = new CUser()
            {
                LastName = lastname,
                FirstName = firstname,
                Login = login,
                Password = PasswordContractor.GeneratePassword(password, securitySalt),
                SecuritySalt= securitySalt,
                RoleId = roleId,
                StatusId = statusId
            };
        }
        
        [When(@"I enter a new member '(.*)', '(.*)', '(.*)', '(.*)', '(.*)', (.*), (.*)")]
        public void WhenIEnterANewMember(string lastname, string firstname, string login, string password, string securitySalt, byte roleId, byte statusId)
        {
            _newOrUpdatedUser = new CUser()
            {
                LastName = lastname,
                FirstName = firstname,
                Login = login,
                Password = PasswordContractor.GeneratePassword(password, securitySalt),
                SecuritySalt = securitySalt,
                RoleId = roleId,
                StatusId = statusId
            };
        }
        
        
        [When(@"I change status of user by userid with id: (.*) and status: (.*)")]
        public void WhenIChangeStatusOfUserByUseridWithIdAndStatus(long id, byte statusId)
        {
            _uStatusReq = new UserStatusReq() { Id = id, StatusId = statusId };
        }
        
        [When(@"I select user (.*)")]
        public void WhenISelectUser(long id)
        {
            _userId = id;
        }
        
        [Then(@"user information updated \((.*), '(.*)', '(.*)'\)")]
        public async Task ThenUserInformationUpdated(long id, string lastname, string firstname)
        {
            try
            {
                var user = await _userService.RetrieveOneUserById(id);
                user.LastName.Should().Be(lastname);
                user.FirstName.Should().Be(firstname);
            }
            catch(UserNotFoundException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        [Then(@"user status information updated with id: (.*) and status: (.*)")]
        public void ThenUserStatusInformationUpdatedWithIdAndStatus(long id, byte statusId)
        {
            var listUser = ScenarioContext.Current.Get<List<QUser>>("listUser");
            var u = listUser.FirstOrDefault(x => x.Id == id);
            u.Should().NotBeNull();
            u.StatusId.Should().Be(statusId);
        }

        [When(@"I call RegisterUser to store the user")]
        public async Task WhenICallRegisterUserToStoreTheUser()
        {
            try
            {
                await _userService.RegisterUserAsync(_newOrUpdatedUser);
            }
            catch (UserAlreadyExistsException ex) {
                ScenarioContext.Current.Add(nameof(UserAlreadyExistsException), ex);
            }
            catch (Exception ex)
            {
                ScenarioContext.Current.Add(nameof(Exception), ex);
            }
        }

        [When(@"I enter a new information \((.*), '(.*)', '(.*)'\) of user by id")]
        public void WhenIEnterANewInformationOfUserById(long id, string lastname, string firstname)
        {
            _uInfoReq = new UserInformationReq() { Id = id, LastName = lastname, FirstName = firstname };
        }

        [When(@"I call ChangeUser to update user information")]
        public async Task WhenICallChangeUserToUpdateUserInformation()
        {
            try
            {
                await _userService.ChangeUserInformationAsync(_uInfoReq);
            }
            catch(UserNotFoundException ex)
            {
                ScenarioContext.Current.Add(nameof(UserNotFoundException), ex);
            }
            catch(Exception ex)
            {
                ScenarioContext.Current.Add(nameof(Exception), ex);
            }
        }

        [When(@"I call ChangeUser to update user status")]
        public async Task WhenICallChangeUserToUpdateUserStatus()
        {
            try
            {
                await _userService.ChangeUserStatusAsync(_uStatusReq);
            }
            catch (UserNotFoundException ex)
            {
                ScenarioContext.Current.Add(nameof(UserNotFoundException), ex);
            }
            catch (Exception ex)
            {
                ScenarioContext.Current.Add(nameof(Exception), ex);
            }
        }

        [When(@"I call service UnregisterUser to remove user")]
        public async Task WhenICallServiceUnregisterUserToRemoveUser()
        {
            await _userService.UnregisterUserAsync(_userId);
        }

        [Then(@"list of (.*) should be (.*)")]
        public void ThenListOfShouldBe(byte roleid, int expectedCount)
        {
            var listUser = ScenarioContext.Current.Get<List<QUser>>("listUser");
            listUser.Where(x => x.RoleId == roleid).ToList().Should().HaveCount(expectedCount);
        }

        [Then(@"throw error UserAlreadyExistException")]
        public void ThenThrowErrorUserAlreadyExistException()
        {
            var exception = ScenarioContext.Current[nameof(UserAlreadyExistsException)];
            exception.Should().BeOfType<UserAlreadyExistsException>();
        }

        [Then(@"throw error UserNotFoundException")]
        public void ThenThrowErrorUserNotFoundException()
        {
            var exception = ScenarioContext.Current[nameof(UserNotFoundException)];
            exception.Should().BeOfType<UserNotFoundException>();
        }

        [Then(@"user with (.*) should be removed")]
        public void ThenUserWithShouldBeRemoved(long p0)
        {
            var listUser = ScenarioContext.Current.Get<List<QUser>>("listUser");
            var u = listUser.FirstOrDefault(x => x.Id == p0);
            u.Should().BeNull();
        }
    }
}
