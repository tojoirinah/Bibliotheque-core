
using Bibliotheque.Api.Req.Members;
using Bibliotheque.Commands.Domains.Enums;
using Bibliotheque.Transverse.Helpers;

using CUser = Bibliotheque.Commands.Domains.Entities.User;

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
        }
    }
}
