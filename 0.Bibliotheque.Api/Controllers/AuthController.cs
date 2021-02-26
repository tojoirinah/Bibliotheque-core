using System;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Helpers;
using Bibliotheque.Api.Queries.Auth;
using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Services.Contracts.Requests.Auths;
using Bibliotheque.Transverse.Helpers;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bibliotheque.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ApplicationSettings _settings;

        public AuthController(IMediator mediator, IMapper mapper, IOptions<ApplicationSettings> appSettings) : base(mediator, mapper)
        {
            _settings = appSettings.Value;
        }

        string GetTokenAndClaimIdentity(UserInformationResp user)
        {
            var claimIdentity = new ClaimsIdentity(new Claim[] {
                    new Claim("UserId",user.Id.ToString()),
                    new Claim("RoleId",user.Role.Id.ToString()),
                    new Claim("UserName",user.Login)
                });

            return JwtTokenHelper.CreateToken(
                claimIdentity,
                Int32.Parse(_settings.TokenExpireMinute),
                _settings.JwtSecretKey
                );


        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> Signin([FromBody] AuthenticationReq req)
        {
            try
            {
                var user = await _mediator.Send(new GetAuthenticationQuery(req));
                var token = GetTokenAndClaimIdentity(user);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
    }
}
