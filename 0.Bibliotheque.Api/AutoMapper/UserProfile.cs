using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Bibliotheque.Api.Resp.Role;
using Bibliotheque.Api.Resp.Status;
using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Commands.Domains.Entities;

using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Api.AutoMapper
{
    public class UserProfile : BaseProfile
    {
        public UserProfile()
        {
            // query
            CreateMap<QUser, UserInformationResp>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new RoleInformationResp(src.RoleId, src.RoleName)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new StatusInformationResp(src.StatusId, src.StatusName)));


            // commands
            CreateMap<QUser, CUser>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role(src.RoleId, src.RoleName)))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => new Status(src.StatusId, src.StatusName)));
        }
    }
}
