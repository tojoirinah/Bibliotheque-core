namespace Bibliotheque.Api.Req.Users
{
    public class RegisterUserReq : IBaseReq
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public byte RoleId { get; set; }
    }
}
