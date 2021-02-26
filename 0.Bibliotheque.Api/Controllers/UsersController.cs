using System;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Commands.Members;
using Bibliotheque.Api.Queries.Users;
using Bibliotheque.Services.Contracts.Requests.Users;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheque.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController
    {

        public UsersController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _mediator.Send(new GetUsersSearchQuery(0, string.Empty));
                return Ok(new { List = list });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("search/{criteria}")]
        public async Task<IActionResult> Search(string criteria = "")
        {
            try
            {
                var list = await _mediator.Send(new GetUsersSearchQuery(0, criteria));
                return Ok(new { List = list });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        #region "COMMANDS"
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterMemberReq req)
        {
            try
            {
                await _mediator.Send(new RegisterMemberCommand(req));
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("unregister/{id}")]
        public async Task<IActionResult> UnRegister([FromQuery] long id)
        {
            try
            {
                await _mediator.Send(new UnRegisterUserCommand(id));
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("update/{id}/information")]
        public async Task<IActionResult> UpdateInformation([FromQuery] long id, [FromBody] UpdateInformationUserReq req)
        {
            try
            {
                await _mediator.Send(new UpdateInformationUserCommand(id, req));
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("update/{id}/password")]
        public async Task<IActionResult> UpdatePassword([FromQuery] long id, [FromBody] UpdatePasswordUserReq req)
        {
            try
            {
                await _mediator.Send(new UpdatePasswordUserCommand(id, req));
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion
    }
}
