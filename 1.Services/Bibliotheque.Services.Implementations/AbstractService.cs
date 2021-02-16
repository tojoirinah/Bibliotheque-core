
using AutoMapper;

using Bibliotheque.Commands.Domains.Contracts;
using Bibliotheque.Services.Contracts;

namespace Bibliotheque.Services.Implementations
{
    public abstract class AbstractService
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;
        protected readonly ILoggerService _logger;

        protected AbstractService(IUnitOfWork uow, IMapper mapper, ILoggerService loggerService)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = loggerService;
        }
    }
}
