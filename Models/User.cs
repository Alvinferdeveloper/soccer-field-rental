using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace proyectoCanchas.Models;

public class User
{
 [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Name { get; set; }

        [Required]
        [StringLength(45)]
        public string LastName { get; set; }

        [Required]
        [StringLength(45)]
        public string Email { get; set; }

        public DateTime? Birthday { get; set; }

        [Required]
        [StringLength(45)]
        public string Password { get; set; }


        public bool? Active { get; set; } = true;

        
        public int? OwnerId { get; set; }

        // Navigation Properties
        public User? Owner { get; set; }
        public ICollection<User> OwnedUsers { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Field> Fields { get; set; }
        public ICollection<Rent> Rents { get; set; }
}