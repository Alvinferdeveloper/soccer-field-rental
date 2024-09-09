namespace proyectoCanchas.Models;
using System.ComponentModel.DataAnnotations;
public class Rent
{
    [Key]
        public int Id { get; set; }

        public string? ClientName { get; set; }
        public string? ClientLastName { get; set; }
        public string? ClientCedula { get; set; }

        
        public int? UserId { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required]
        public string ClientType { get; set; } // Use a string for ENUMs

        [Required]
        public DateTime DateTime { get; set; }

        public bool? Active { get; set; } = true;

        // Navigation Properties
        public User? User { get; set; }
        public Field Field { get; set; }
}
