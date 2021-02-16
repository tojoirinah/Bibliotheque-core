using System;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Helpers;
using Bibliotheque.Api.Queries.Auth;
using Bibliotheque.Api.Req.Auth;
using Bibliotheque.Api.Resp.Users;
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

        [HttpPost("SignIn")]
        public async Task<IActionResult> Signin([FromBody] AuthenticationReq req)
        {
            var query = _mapper.Map<GetAuthenticationQuery>(req);
            var user  = await _mediator.Send(query);
            var claimIdentity = new ClaimsIdentity(new Claim[] {
                    new Claim("UserId",user.Id.ToString()),
                    new Claim("RoleId",user.Role.Id.ToString()),
                    new Claim("UserName",user.Login)
                });

            var token = JwtTokenHelper.CreateToken(
                claimIdentity,
                Int32.Parse(_settings.TokenExpireMinute),
                _settings.JwtSecretKey
                );

            return Ok(new { Token = token });

        }
    }
}
