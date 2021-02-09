using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibliotheque.Commands.Domains.Entities
{
    [Table("User", Schema = "dbo")]
    public class User : BaseEntity<long>
    {
        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; }

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Password { get; set; }

        [StringLength(500)]
        public string SecuritySalt { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public byte RoleId { get; set; }

        [Required]
        public byte StatusId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status UserStatus { get; set; }
    }
}
