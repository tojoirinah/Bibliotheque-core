namespace Bibliotheque.Api.Resp.Status
{
    public class StatusInformationResp : IBaseResp
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public StatusInformationResp()
        {
        }

        public StatusInformationResp(byte id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
