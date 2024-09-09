namespace proyectoCanchas.Models;
using System.ComponentModel.DataAnnotations;
public class Field
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(45)]
    public string Name { get; set; }

    [Required]
    public string SoccerType { get; set; } // Use a string for ENUMs

    [Required]
    public int Price { get; set; }

    [Required]
    public TimeSpan ServiceStartTime { get; set; }

    [Required]
    public TimeSpan ServiceEndTime { get; set; }

    public string? CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

 
    public int? OwnerId { get; set; }

    // Navigation Properties
    public User? Owner { get; set; }
    public ICollection<Rent> Rents { get; set; }
}
