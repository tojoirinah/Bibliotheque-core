using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliotheque.Commands.Domains.Entities
{
    public abstract class BaseEntity<T> where T : struct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }
}
