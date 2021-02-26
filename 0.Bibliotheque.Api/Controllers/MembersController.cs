using System;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Commands.Members;
using Bibliotheque.Api.Queries.Users;
using Bibliotheque.Commands.Domains.Enums;
using Bibliotheque.Services.Contracts.Requests.Members;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheque.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembersController : BaseController
    {
        public MembersController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

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
        public async Task<IActionResult> UpdateInformation([FromQuery] long id, [FromBody] UpdateInformationMemberReq req)
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
        public async Task<IActionResult> UpdatePassword([FromQuery] long id, [FromBody] UpdatePasswordMemberReq req)
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

        [HttpGet("search/{criteria}")]
        public async Task<IActionResult> Search(string criteria = "")
        {
            try
            {
                var list = await _mediator.Send(new GetUsersSearchQuery((byte)ERole.MEMBER,criteria));
                return Ok(new { List = list });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByIdQuery(id));
                return Ok(new { Item = user });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
