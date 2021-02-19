namespace Bibliotheque.Api.Req.Members
{
    public class UpdateInformationMemberReq : IBaseReq
    {
        public long Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}
