namespace Bibliotheque.Api.Resp.Role
{
    public class RoleInformationResp : IBaseResp
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public RoleInformationResp()
        {
        }

        public RoleInformationResp(byte id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
