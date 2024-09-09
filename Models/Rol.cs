namespace proyectoCanchas.Models;
using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<UserRole> UserRoles { get; set; }
}
