namespace Bibliotheque.Services.Contracts.Requests.Statuses
{
    public class UpdateUserStatusReq
    {
        public long UserId { get; set; }
        public byte NewStatusId { get; set; }
    }
}
