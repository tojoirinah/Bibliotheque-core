using System;
using System.Linq;
using System.Reflection;

using AutoMapper;

using Bibliotheque.Commands.Domains.Contracts;
using Bibliotheque.Services.Contracts.Profiles;

namespace Bibliotheque.Specs.Steps
{
    public abstract class BaseStep
    {
        protected IMapper _mapper;
        protected IUnitOfWork _uow;

        protected abstract void SetupStep();

        protected BaseStep()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(cfg =>
                {
                    foreach (Type type in Assembly.GetAssembly(typeof(BaseProfile)).GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(BaseProfile))))
                    {
                        cfg.AddProfile(type);
                    }
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

        }
    }
}
