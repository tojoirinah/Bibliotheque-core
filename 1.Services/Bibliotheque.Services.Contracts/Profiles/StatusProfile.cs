
using CStatus = Bibliotheque.Commands.Domains.Entities.Status;
using QStatus = Bibliotheque.Queries.Domains.Entities.Status;

namespace Bibliotheque.Services.Contracts.Profiles
{
    public class StatusProfile : BaseProfile
    {
        public StatusProfile()
        {
            CreateMap<QStatus, CStatus>();
        }
    }
}
