using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Services.Contracts.Profiles
{
    public class UserProfile : BaseProfile
    {
        public UserProfile()
        {
            CreateMap<QUser, CUser>();
        }
    }
}
