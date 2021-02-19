
using Bibliotheque.Api.Queries.Auth;
using Bibliotheque.Api.Resp.Role;
using Bibliotheque.Api.Resp.Status;
using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Commands.Domains.Entities;
using Bibliotheque.Services.Contracts.Requests;

using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Api.AutoMapper
{
    public class UserProfile : BaseProfile
    {
        public UserProfile()
        {
            CreateMap<GetAuthenticationRequest, AuthReq>();
            CreateMap<QUser, UserInformationResp> ()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new RoleInformationResp(src.RoleId, src.RoleName)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new StatusInformationResp(src.StatusId, src.StatusName)));

            CreateMap<UserInformationReq,CUser>();
            CreateMap<UserStatusReq, CUser>();
            CreateMap<QUser, CUser>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new Role(src.RoleId, src.RoleName)))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => new Status(src.StatusId, src.StatusName)));
        }
    }
}
