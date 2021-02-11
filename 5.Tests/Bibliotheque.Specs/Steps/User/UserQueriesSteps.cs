using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bibliotheque.Services.Implementations.Exceptions;

using FluentAssertions;

using TechTalk.SpecFlow;

using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Specs.Steps.User
{
    [Binding]
    public class UserQueriesSteps : BaseUserStep
    {
        private IList<QUser> _users;
        private QUser _user;

        public UserQueriesSteps() : base()
        {

        }

        [When(@"I enter '(.*)' as name")]
        public void WhenIEnterAsName(string p0)
        {
            ScenarioContext.Current.Add("querySearch", p0);
        }

        [When(@"I call (.*) as userId")]
        public void WhenICallAsUserId(long p0)
        {
            ScenarioContext.Current.Add("userId", p0);
        }

        
        [When(@"I enter exactly the '(.*)' as username")]
        public void WhenIEnterExactlyTheAsUsername(string p0)
        {
            ScenarioContext.Current.Add("username", p0);
        }
        
        [Then(@"list user should count (.*)")]
        public void ThenListUserShouldCount(int p0)
        {
            _users.Should().HaveCount(p0);
        }
        
        [Then(@"username should by '(.*)'")]
        public void ThenUsernameShouldBy(string p0)
        {
            _user.Login.Should().Be(p0);
        }
        
        [Then(@"username in database should be '(.*)' as expected")]
        public void ThenUsernameInDatabaseShouldBeAsExpected(string p0)
        {
            _user.Login.Should().Be(p0);
        }

        [When(@"I call userService SearchUser")]
        public async Task WhenICallUserServiceSearchUser()
        {
            try
            {
                var querySearch = ScenarioContext.Current["querySearch"] as string;
                _users = await _userService.SearchUser(querySearch);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [When(@"I call userService RetrieveOneUserById")]
        public async Task WhenICallUserServiceRetrieveOneUserById()
        {
            long userId = (long)ScenarioContext.Current["userId"];
            _user = await _userService.RetrieveOneUserById(userId);
        }

        [When(@"I call userService RetriveOneUserByUsername")]
        public async Task WhenICallUserServiceRetriveOneUserByUsername()
        {
            var username = ScenarioContext.Current["username"] as string;
            _user = await _userService.RetrieveOneUserByUserName(username);
        }

        [Then(@"Response should return the list of user")]
        public void ThenResponseShouldReturnTheListOfUser()
        {
            _users.Should().NotBeNull();
        }

        [Then(@"Response should return the user")]
        public void ThenResponseShouldReturnTheUser()
        {
            _user.Should().NotBeNull();
        }

        [Then(@"Return should be null")]
        public void ThenResultShouldBeNull()
        {
            _user.Should().BeNull();
        }

    }
}
