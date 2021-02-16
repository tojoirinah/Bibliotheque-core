namespace Bibliotheque.Api.Req.Users
{
    public class UpdatePasswordUserReq : IBaseReq
    {
        public long Id { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
