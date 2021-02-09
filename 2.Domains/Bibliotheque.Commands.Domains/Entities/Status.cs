using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliotheque.Commands.Domains.Entities
{
    [Table("Status", Schema = "dbo")]
    public sealed class Status : BaseEntity<byte>
    {
        [StringLength(20)]
        public string Name { get; set; }
    }
}
