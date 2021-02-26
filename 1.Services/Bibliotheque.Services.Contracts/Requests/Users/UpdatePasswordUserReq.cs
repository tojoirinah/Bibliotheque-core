namespace Bibliotheque.Services.Contracts.Requests.Users
{
    public class UpdatePasswordUserReq
    {
        public long Id { get; set; }

        public string NewPassword { get; set; }

        public string OldPassword { get; set; }
    }
}
