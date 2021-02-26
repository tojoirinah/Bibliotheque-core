
using System.Collections.Generic;

using Bibliotheque.Api.Resp.Users;
using Bibliotheque.Commands.Domains.Enums;
using Bibliotheque.Services.Contracts.Requests.Members;
using Bibliotheque.Transverse.Helpers;

using CUser = Bibliotheque.Commands.Domains.Entities.User;
using QUser = Bibliotheque.Queries.Domains.Entities.User;

namespace Bibliotheque.Api.AutoMapper
{
    public class MemberProfile : BaseProfile
    {
        public MemberProfile()
        {
            var securitySalt = PasswordContractor.CreateRandomPassword(8);

            CreateMap<RegisterMemberReq, CUser>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (byte)ERole.MEMBER))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (byte)EStatus.WAITING))
                .ForMember(dest => dest.SecuritySalt, opt => opt.MapFrom(src => securitySalt))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordContractor.GeneratePassword(src.Password, securitySalt)));

            //securitySalt = PasswordContractor.CreateRandomPassword(8);
            //CreateMap<UpdatePasswordMemberReq, UserInformationReq>()
            //    .ForMember(dest => dest.SecuritySalt, opt => opt.MapFrom(src => securitySalt))
            //    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordContractor.GeneratePassword(src.Password, securitySalt)));
        }
    }
}
