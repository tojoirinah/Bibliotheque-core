using System.Threading.Tasks;

using Bibliotheque.Services.Contracts.Requests.Auths;
using Bibliotheque.Services.Implementations.Exceptions;
using Bibliotheque.Specs.Steps.User;

using FluentAssertions;

using TechTalk.SpecFlow;

using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Specs.Steps
{
    [Binding]
    public class AuthenticationUserSteps : BaseUserStep
    {

        private readonly AuthenticationReq _req = new AuthenticationReq();
        private QUser _authenticatedUser;

        public AuthenticationUserSteps()
        {
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
