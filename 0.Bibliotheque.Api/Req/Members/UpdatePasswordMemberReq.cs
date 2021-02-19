namespace Bibliotheque.Api.Req.Members
{
    public class UpdatePasswordMemberReq : IBaseReq
    {
        public long Id { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
