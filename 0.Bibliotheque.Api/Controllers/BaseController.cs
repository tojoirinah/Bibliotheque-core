
using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Bibliotheque.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IMediator _mediator;
        protected IMapper _mapper;

        protected BaseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
    }
}
