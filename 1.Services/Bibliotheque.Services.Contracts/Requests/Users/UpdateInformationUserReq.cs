namespace Bibliotheque.Services.Contracts.Requests.Users
{
    public class UpdateInformationUserReq
    {
        public long Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}
