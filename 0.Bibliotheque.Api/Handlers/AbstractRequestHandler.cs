
using AutoMapper;

namespace Bibliotheque.Api.Handlers
{
    public abstract class AbstractRequestHandler
    {
        protected IMapper _mapper;

        protected AbstractRequestHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
