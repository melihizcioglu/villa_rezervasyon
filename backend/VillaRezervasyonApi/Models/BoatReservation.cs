using System.ComponentModel.DataAnnotations;

namespace VillaRezervasyonApi.Models
{
    public class BoatReservation
    {
        public int Id { get; set; }
        
        [Required]
        public int BoatId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Status { get; set; }
        
        public Boat Boat { get; set; }
    }
} 