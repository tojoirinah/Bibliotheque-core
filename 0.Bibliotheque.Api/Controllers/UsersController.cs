using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        readonly IEnumerable<User> _list = new List<User>()
            {
                new User(){ Id = Guid.NewGuid(), Name = "Statam", FirstName = "Jason"},
                new User(){ Id = Guid.NewGuid(), Name = "Lee", FirstName = "Bruce"},
                new User(){ Id = Guid.NewGuid(), Name = "Stalone", FirstName = "Sylvester"},
                new User(){ Id = Guid.NewGuid(), Name = "Diesel", FirstName = "Vin"},
            };

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _list;
        }

        [HttpGet("{name}")]
        public User GetByName(string name)
        {
            var user = _list.FirstOrDefault(x => x.Name == name);
            return user;
        }
    }
}
