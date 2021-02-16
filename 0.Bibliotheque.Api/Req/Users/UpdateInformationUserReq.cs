namespace Bibliotheque.Api.Req.Users
{
    public class UpdateInformationUserReq : IBaseReq
    {
        public long Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}
