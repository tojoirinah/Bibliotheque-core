
using CRole = Bibliotheque.Commands.Domains.Entities.Role;
using QRole = Bibliotheque.Queries.Domains.Entities.Role;

namespace Bibliotheque.Services.Contracts.Profiles
{
    public class RoleProfile : BaseProfile
    {
        public RoleProfile()
        {
            CreateMap<QRole, CRole>();
        }
    }
}
