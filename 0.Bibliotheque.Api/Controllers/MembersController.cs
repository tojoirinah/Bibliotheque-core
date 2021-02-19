using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Bibliotheque.Api.Commands.Members;
using Bibliotheque.Api.Req.Members;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("Register")]
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

        [HttpDelete("UnRegister/{id}")]
        public async Task<IActionResult> UnRegister([FromQuery] long id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("Update/{id}/Information")]
        public async Task<IActionResult> UpdateInformation([FromQuery] long id, [FromBody] UpdateInformationMemberReq req)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("Update/{id}/Password")]
        public async Task<IActionResult> UpdatePassword([FromQuery] long id, [FromBody] UpdatePasswordMemberReq req)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("Search/{criteria}")]
        public async Task<IActionResult> Search(string criteria)
        {
            try
            {
                return Ok();
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
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
