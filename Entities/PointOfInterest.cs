using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key] // This designates the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // IDENTITY(1,1)
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set;}
        
        public City? City { get; set; } // Foreign key to City table; convention based

        [ForeignKey("Id")] // Tells EF What the FK is
        public int CityId { get; set; }

        public PointOfInterest(string name)
        {
            Name = name;
        }
    }
}