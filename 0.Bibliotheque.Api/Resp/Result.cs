using System.Collections.Generic;

namespace Bibliotheque.Api.Resp
{
    public class Result<Entity> where Entity : IBaseResp
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public Entity Data { get; set; }
        public IEnumerable<Entity> List { get; set; }
    }
}
