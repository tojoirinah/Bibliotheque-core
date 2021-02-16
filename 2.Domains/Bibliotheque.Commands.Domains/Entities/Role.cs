using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliotheque.Commands.Domains.Entities
{
    [Table("Role", Schema = "dbo")]
    public sealed class Role : BaseEntity<byte>
    {
        [StringLength(20)]
        public string Name { get; set; }

        public Role()
        {

        }
        public Role(byte id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
