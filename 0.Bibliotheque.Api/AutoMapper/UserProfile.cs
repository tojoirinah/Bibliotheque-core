
using Bibliotheque.Api.Queries.Auth;
using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Services.Contracts.Requests;

using QUser = Bibliotheque.Queries.Domains.Entities.User;
using CUser = Bibliotheque.Commands.Domains.Entities.User;
using Bibliotheque.Commands.Domains.Entities;
using Bibliotheque.Api.Resp.Role;
using Bibliotheque.Api.Resp.Status;

namespace Bibliotheque.Api.AutoMapper
{
    public class UserProfile : BaseProfile
    {
        public UserProfile()
        {
            CreateMap<GetAuthenticationQuery, AuthReq>();
            CreateMap<QUser, UserInformationResp> ()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new RoleInformationResp(src.RoleId, src.RoleName)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new StatusInformationResp(src.StatusId, src.StatusName)));
            CreateMap<UserInformationReq, CUser>();
            CreateMap<UserStatusReq, CUser>();
            CreateMap<QUser, CUser>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role(src.RoleId, src.RoleName)))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => new Status(src.StatusId, src.StatusName)));
        }
    }
}
