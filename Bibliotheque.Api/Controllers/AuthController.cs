using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Bibliotheque.Api.Helpers;
using Bibliotheque.Api.Req.Auth;
using Bibliotheque.Transverse.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bibliotheque.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationSettings _settings;

        public AuthController(IOptions<ApplicationSettings> appSettings)
        {
            _settings = appSettings.Value;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> Signin([FromBody] AuthenticationReq req)
        {
            var claimIdentity = new ClaimsIdentity(new Claim[] {
                    new Claim("UserId","1"),
                    new Claim("RoleId","1"),
                    new Claim("UserName","admin")
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
